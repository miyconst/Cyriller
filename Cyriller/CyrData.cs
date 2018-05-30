using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Collections.Concurrent;

namespace Cyriller
{
    internal class CyrData
    {
        /// <summary>The minimum number of matching letters, from the right end, when searching for similar matches</summary>
        public const int DefaultMinSameLetters = 2;

        /// <summary>The weights of matching letters per position, from the right end</summary>
        public static readonly int[] SameLetterWeights = { 1000, 100, 10 };

        public CyrData()
        {
        }

        public TextReader GetData(string FileName)
        {
            Stream stream = typeof(CyrData).Assembly.GetManifestResourceStream("Cyriller.App_Data." + FileName);
            GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress);
            TextReader treader = new StreamReader(gzip);

            return treader;
        }

        /// <summary>Search for similar words in the collection</summary>
        /// <param name="Word">The word to search for</param>
        /// <param name="Collection">The collection to look in</param>
        /// <param name="MinSameLetters">The minimum number of matching letters, from the right end, min value - <see cref="DefaultMinSameLetters"/></param>
        public string GetSimilar(string Word, IEnumerable<string> Collection, int MinSameLetters = DefaultMinSameLetters)
        {
            if (MinSameLetters < DefaultMinSameLetters)
            {
                throw new ArgumentOutOfRangeException($"{nameof(MinSameLetters)} value can not be smaller than {DefaultMinSameLetters}!");
            }

            if (Word == null || Word.Length < MinSameLetters)
            {
                return Word;
            }

            string foundWord = null;
            ConcurrentBag<SimilarCandidate> candidates = new ConcurrentBag<SimilarCandidate>();

            Parallel.ForEach(Collection, (str, loopState) =>
            {
                if (str == Word)
                {
                    foundWord = str;
                    loopState.Stop();
                    return;
                }

                int maxPosition = Math.Min(Word.Length, str.Length);
                int weight = 0;

                for (int i = 1; i <= maxPosition; i++)
                {
                    if (str[str.Length - i] == Word[Word.Length - i])
                    {
                        int wi = i - 1;
                        weight += wi < SameLetterWeights.Length ? SameLetterWeights[wi] : 1;
                    }
                    else if (i <= MinSameLetters)
                    {
                        return;
                    }
                }

                SimilarCandidate c = new SimilarCandidate()
                {
                    Name = str,
                    Weight = weight
                };

                candidates.Add(c);
            });

            if (!string.IsNullOrEmpty(foundWord))
            {
                return foundWord;
            }

            SimilarCandidate candidate = null;
            foreach (var c in candidates)
            {
                if (candidate == null
                    || c.Weight > candidate.Weight
                    || (c.Weight == candidate.Weight && c.Name.Length < candidate.Name.Length)
                    || (c.Weight == candidate.Weight && c.Name.Length == candidate.Name.Length && c.Name.CompareTo(candidate.Name) < 0))
                {
                    candidate = c;
                }
            }

            return candidate?.Name;
        }

        internal class SimilarCandidate
        {
            /// <summary>
            /// Found similar word
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The weight of the matching letters, <see cref="SameLetterWeights"/>
            /// </summary>
            public int Weight { get; set; }
        }
    }
}
