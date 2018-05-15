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
        /// <summary>Минимальная длина слова</summary>
        public const int MinDiffLength = 2;
        /// <summary>Вес позиции от конца слова</summary>
        private static readonly int[] WeightPositions = { 102400, 51200, 25600, 12800, 6400, 3200, 1600, 800, 400, 200 };

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
        /// <param name="word">Искомое слово</param>
        /// <param name="collection">Список слов для поиска</param>
        /// <param name="minWordLength">Минимальная длина слова, меньше которой нет смысла искать подходящие слова</param>
        public string GetSimilar(string word, IEnumerable<string> collection, int minWordLength = MinDiffLength)
        {
            if (minWordLength < MinDiffLength)
            {
                minWordLength = MinDiffLength;
            }
            if (word == null || word.Length < minWordLength)
            {
                return word;
            }

            string foundWord = null;
            int wordMaxPosition = word.Length - 1;
            // SimilarWord => [lengthSimilarWord, similarWeight]
            ConcurrentDictionary<string, int[]> keys = new ConcurrentDictionary<string, int[]>();
            Parallel.ForEach(collection, (str, loopState) =>
            {
                if (str == word)
                {
                    foundWord = str;
                    loopState.Stop();
                }
                else
                {
                    int strMaxPosition = str.Length - 1;
                    int maxPosition = Math.Min(wordMaxPosition, strMaxPosition);
                    bool isSimilar = true;
                    int similarWeight = 0;
                    for (int i = 0; i <= maxPosition; i++)
                    {
                        if (str[strMaxPosition - i] == word[wordMaxPosition - i])
                        {
                            similarWeight += i < WeightPositions.Length ? WeightPositions[i] : 1;
                        }
                        else if (i < minWordLength)
                        {
                            isSimilar = false;
                            break;
                        }
                    }
                    if (isSimilar)
                    {
                        keys.TryAdd(str, new[] { str.Length, similarWeight });
                    }
                }
            });

            if (!string.IsNullOrEmpty(foundWord) || !keys.Any())
            {
                return foundWord;
            }

            int valueWeight = 0;
            int keyLength = 1000000;
            foreach (var kv in keys)
            {
                var value = kv.Value;
                var key = kv.Key;
                if (value[1] < valueWeight || (value[1] == valueWeight && value[0] > keyLength))
                {
                    continue;
                }

                if (value[1] > valueWeight || value[0] < keyLength || key.CompareTo(foundWord) < 0)
                {
                    foundWord = key;
                }
                keyLength = value[0];
                valueWeight = value[1];
            }

            return foundWord;
        }
    }
}
