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
            int equalWeight = char.MaxValue;
            // SimilarWord => [lengthSimilarWord, similarWeight]
            ConcurrentDictionary<string, decimal[]> keys = new ConcurrentDictionary<string, decimal[]>();
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
                    decimal similarWeight = 0;
                    int positionMultiply;
                    for (int i = 1; i <= minLength; i++)
                    {
                        positionMultiply = minLength - i + 1;
                        if (str[strLength - i] != Word[wordLength - i])
                        {
                            if (i <= MinWordLength)
                            {
                                isSimilar = false;
                                break;
                            }

                            positionMultiply--;
                        }
                        similarWeight += positionMultiply * equalWeight;
                    }
                    if (isSimilar)
                    {
                        keys.TryAdd(str, new decimal[] { str.Length, similarWeight });
                    }
                }
            });

            if (!string.IsNullOrEmpty(foundWord))
            {
                return foundWord;
            }

            return keys.OrderByDescending(val => val.Value[1]).ThenBy(x => x.Value[0]).ThenBy(x => x.Key).FirstOrDefault().Key;
        }
    }
}
