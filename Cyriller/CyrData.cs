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
            if (MinWordLength < 2)
            {
                MinWordLength = 2;
            }
            else if (MinWordLength > 10)
            {
                MinWordLength = 10;
            }
            if (Word == null || Word.Length < MinWordLength)
            {
                return Word;
            }

            string foundWord = null;
            int wordLength = Word.Length;
            int superLength = 10;
            int superMultiply = 100;
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
                    int strLength = str.Length;
                    int minLength = Math.Min(wordLength, strLength);
                    bool isSimilar = true;
                    int similarWeight = 0;
                    for (int i = 1; i <= minLength; i++)
                    {
                        int positionWeight;
                        if (str[strLength - i] == Word[wordLength - i])
                        {
                            if (i <= superLength)
                            {
                                positionWeight = (1 << (superLength - i + 1)) * superMultiply;
                            }
                            else
                            {
                                positionWeight = 1;
                            }
                        }
                        else if (i > MinWordLength)
                        {
                            positionWeight = 0;
                        }
                        else
                        {
                            isSimilar = false;
                            break;
                        }
                        similarWeight += positionWeight;
                    }
                    if (isSimilar)
                    {
                        keys.TryAdd(str, new int[] { str.Length, similarWeight });
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
