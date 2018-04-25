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
        public static TextReader GetData(string FileName)
        {
            Stream stream = typeof(CyrData).Assembly.GetManifestResourceStream("Cyriller.App_Data." + FileName);
            GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress);
            TextReader treader = new StreamReader(gzip);

            return treader;
        }

        /// <summary>Нахождение подходящего по окончанию слова в коллекции</summary>
        /// <param name="Word">Искомое слово</param>
        /// <param name="Collection">Список слов для поиска</param>
        public static string GetSimilar(string Word, IEnumerable<string> Collection)
        {
            return GetSimilar(Word, Collection, Word);
        }

        /// <summary>Нахождение подходящего по окончанию слова в коллекции</summary>
        /// <param name="Word">Искомое слово</param>
        /// <param name="Collection">Список слов для поиска</param>
        /// <param name="OriginalWord">Изначальное искомое слово получения весового коэффициента при поиске подходящих слов</param>
        private static string GetSimilar(string Word, IEnumerable<string> Collection, string OriginalWord)
        {
            if (Word == null || Word.Length <= 1)
            {
                return Word;
            }

            string foundWord = null;
            // SimilarWord => [lengthSimilarWord, quantitySameChars]
            ConcurrentDictionary<string, int[]> keys = new ConcurrentDictionary<string, int[]>();
            Parallel.ForEach(Collection, (s, loopState) =>
            {
                if (s.EndsWith(Word))
                {
                    if (s == Word)
                    {
                        foundWord = s;
                        loopState.Stop();
                    }

                    int quantitySameChars = 0;
                    for (int i = Math.Min(OriginalWord.Length, s.Length); i > Word.Length; i--)
                    {
                        if (s[s.Length - i] == OriginalWord[OriginalWord.Length - i])
                        {
                            quantitySameChars++;
                        }
                    }
                    keys.TryAdd(s, new int[] { s.Length, quantitySameChars });
                }
            });

            if (!string.IsNullOrEmpty(foundWord))
            {
                return foundWord;
            }

            if (keys.Count == 0 && Word.Length > 2)
            {
                return GetSimilar(Word.Substring(1), Collection, OriginalWord);
            }

            return keys.OrderBy(val => val.Value[0]).ThenByDescending(val => val.Value[1]).FirstOrDefault().Key;
        }
    }
}
