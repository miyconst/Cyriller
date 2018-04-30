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

        /// <summary>Нахождение подходящего по окончанию слова в коллекции</summary>
        /// <param name="Word">Искомое слово</param>
        /// <param name="Collection">Список слов для поиска</param>
        /// <param name="MinWordLength">Минимальная длина окончания слова, которая позволяет различать слова</param>
        public string GetSimilar(string Word, IEnumerable<string> Collection, int MinWordLength = 2)
        {
            if (Word == null || Word.Length < MinWordLength)
            {
                return Word;
            }

            string foundWord = null;
            int wordLength = Word.Length;
            // SimilarWord => [lengthSimilarWord, quantitySameChars]
            ConcurrentDictionary<string, int[]> keys = new ConcurrentDictionary<string, int[]>();
            Parallel.ForEach(Collection, (s, loopState) =>
            {
                if (s != Word)
                {
                    int sLength = s.Length;
                    int minLength = Math.Min(wordLength, sLength);
                    int quantitySameChars = 0;
                    bool isSimilar = true;
                    for (int i = 1; i <= minLength; i++)
                    {
                        if (s[sLength - i] == Word[wordLength - i])
                        {
                            quantitySameChars++;
                        }
                        if (i <= MinWordLength && quantitySameChars < i)
                        {
                            isSimilar = false;
                            break;
                        }
                    }
                    if (isSimilar)
                    {
                        keys.TryAdd(s, new int[] { s.Length, quantitySameChars });
                    }
                }
                else
                {
                    foundWord = s;
                    loopState.Stop();
                }
            });

            if (!string.IsNullOrEmpty(foundWord))
            {
                return foundWord;
            }

            return keys.OrderBy(val => val.Value[0]).ThenByDescending(val => val.Value[1]).FirstOrDefault().Key;
        }
    }
}
