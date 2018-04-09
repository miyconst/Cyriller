using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Cyriller
{
    internal static class CyrData
    {
        /// <summary>Читатель данных из файла</summary>
        /// <param name="FileName"></param>
        public static TextReader GetReader(string FileName)
        {
            Stream stream = typeof(CyrData).Assembly.GetManifestResourceStream("Cyriller.App_Data." + FileName);
            GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress);
            return new StreamReader(gzip);
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
                return Word;

            string foundWord = string.Empty;
            // SimilarWord => [ Length, NumSameChars ]
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
                    else
                    {
                        int restLength = (OriginalWord.Length < s.Length ? OriginalWord.Length : s.Length) - Word.Length;
                        int NumSameChars = 0;
                        for (int i = 1; i <= restLength; i++)
                        {
                            int pos = s.Length - (Word.Length + i);
                            if (s[pos] == OriginalWord[pos])
                                NumSameChars++;
                        }
                        keys.TryAdd(s, new int[] { s.Length, NumSameChars });
                    }
                }
            });

            if (foundWord.Length > 0)
                return foundWord;

            if (keys.Count == 0 && Word.Length > 2)
                return GetSimilar(Word.Substring(1), Collection, OriginalWord);

            return keys.OrderBy(val => val.Value[0]).OrderByDescending(val => val.Value[1]).FirstOrDefault().Key;
        }

        public static T GetSimilarDetails<T>(string Word, Dictionary<string, T> Collection, out string CollectionWord)
        {
            CollectionWord = Collection.ContainsKey(Word) ? Word : GetSimilar(Word, Collection.Keys);
            return !string.IsNullOrEmpty(CollectionWord) ? Collection[CollectionWord] : default(T);
        }

        public static T GetDictionaryItem<T>(string Key, Dictionary<string, T> Items)
        {
            if (Items.ContainsKey(Key))
                return Items[Key];

            //бинарный перебор подстановок
            Dictionary<char, char> subst = new Dictionary<char, char>
            {
                { 'е', 'ё' },
                { 'ё', 'е' },
            };
            foreach (var kv in subst)
            {
                List<int> positions = new List<int>();
                for (int i = 0; i < Key.Length; i++)
                {
                    if (Key[i] == kv.Key)
                        positions.Add(i);
                }
                if (positions.Count == 0)
                    continue;//не нашли

                int steps = (1 << positions.Count) - 1;
                //Количество вариантов замен равно двойке в степени количества найденных позиций минус 1 (начальный) вариант.
                //Получается бинарно только единицы, что удобно для бинарного сложения и проверки.
                for (int step = 1; step <= steps; step++)
                {
                    char[] chars = Enumerable.Repeat(kv.Key, positions.Count).ToArray();
                    int nest = 0;
                    int queue = step;
                    while ((queue & steps) > 0)
                    {
                        if ((queue & 1) > 0)
                            chars[nest] = kv.Value;
                        queue = step >> ++nest;
                    }
                    string subKey = Key;
                    for (int i = 0; i < positions.Count; i++)
                        subKey = subKey.Substring(0, positions[i]) + chars[i] + subKey.Substring(positions[i] + 1);

                    if (Items.ContainsKey(subKey))
                        return Items[subKey];
                }
            }
            return default(T);
        }
    }
}
