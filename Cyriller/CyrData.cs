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
            const int minDiffLength = 2;
            const int maxDiffLength = 10;
            if (MinWordLength < minDiffLength)
            {
                MinWordLength = minDiffLength;
            }
            else if (MinWordLength > maxDiffLength)
            {
                MinWordLength = maxDiffLength;
            }
            if (Word == null || Word.Length < MinWordLength)
            {
                return Word;
            }

            const int superLength = 10;
            const int superMultiply = 100;
            // вес позиции от конца слова
            int[] weightPositions = new int[superLength];
            for (int i = 0; i < superLength; i++)
            {
                weightPositions[i] = (1 << (superLength - i)) * superMultiply;
            }

            string foundWord = null;
            int wordMaxPosition = Word.Length - 1;
            // SimilarWord => [lengthSimilarWord, similarWeight]
            ConcurrentDictionary<string, int[]> keys = new ConcurrentDictionary<string, int[]>();
            Parallel.ForEach(Collection, (str, loopState) =>
            {
                if (str == Word)
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
                        if (str[strMaxPosition - i] == Word[wordMaxPosition - i])
                        {
                            similarWeight += i < superLength ? weightPositions[i] : 1;
                        }
                        else if (i < MinWordLength)
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
