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
                    int positionWeight = 0;
                    for (int i = 1; i <= minLength; i++)
                    {
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

            if (!string.IsNullOrEmpty(foundWord))
            {
                return foundWord;
            }
            if (keys.Any())
            {
                int maxWeight = 0;
                int minLength = 1000000;
                List<KeyValuePair<string, int[]>> shortList = new List<KeyValuePair<string, int[]>>();
                foreach (var kv in keys)
                {
                    var value = kv.Value;
                    if (value[1] > maxWeight || (value[1] == maxWeight && value[0] <= minLength))
                    {
                        minLength = value[0];
                        maxWeight = value[1];
                        shortList.Add(kv);
                    }
                }
                foundWord = shortList.Where(val => val.Value[1] == maxWeight && val.Value[0] == minLength).OrderBy(x => x.Key).FirstOrDefault().Key;
            }

            return foundWord;
        }
    }
}
