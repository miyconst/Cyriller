using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Cyriller.Model;

namespace Cyriller
{
    public class CyrNounCollection
    {
        /// <summary>RuleID => Rule</summary>
        protected Dictionary<int, CyrRule[]> nounRules;
        /// <summary>Существительное => RuleIDs</summary>
        protected Dictionary<string, List<int[]>> nounWords;

        public CyrNounCollection()
        {
            using (TextReader treader = CyrData.GetReader("noun-rules.gz"))
            {
                nounRules = treader.ReadToEnd().Split('\n')
                    .Select(x => x.Trim().Split(' '))
                    .ToDictionary(x => int.Parse(x[0]), x => x[1].Split(',', '|').Select(q => new CyrRule(q)).ToArray());
            }

            using (TextReader treader = CyrData.GetReader("nouns.gz"))
            {
                nounWords = new Dictionary<string, List<int[]>>(StringComparer.CurrentCultureIgnoreCase);
                foreach (string line in treader.ReadToEnd().Split('\n'))
                {
                    string[] parts = line.Trim().Split(' ');
                    if (parts.Length != 2 || string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]))
                        continue;
                    if (!nounWords.ContainsKey(parts[0]))
                        nounWords.Add(parts[0], new List<int[]>());
                    nounWords[parts[0]].Add(parts[1].Split(',').Select(q => int.Parse(q)).ToArray());
                }
            }
        }

        /// <summary>Получение существительного с вариантами окончания для разных падежей, числа. Поиск по точному совпадению.</summary>
        /// <param name="Word">Существительное</param>
        public CyrNoun Get(string Word)
        {
            return Get(Word, GetConditionsEnum.Strict);
        }

        /// <summary>Получение существительного с вариантами окончания для разных падежей, числа, пола, одушевлённости, типу слова.</summary>
        /// <param name="Word">Существительное</param>
        /// <param name="Condition">Вариант поиска в словаре (точный или приблизительный)</param>
        /// <param name="GenderID">Фильтр по полу</param>
        /// <param name="AnimateID">Фильтр по одушевлённости</param>
        /// <param name="TypeID">Фильтр по типу слова</param>
        public CyrNoun Get(string Word, GetConditionsEnum Condition, GendersEnum? GenderID = null, AnimatesEnum? AnimateID = null, WordTypesEnum? TypeID = null)
        {
            Word = (Word ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(Word))
                throw new ArgumentException("Empty Word");
            string t = Word;
            List<int[]> details = CyrData.GetDictionaryItem(t, nounWords);

            if (details == null && Condition == GetConditionsEnum.Similar)
                details = CyrData.GetSimilarDetails(Word, nounWords, out t);

            if (details == null)
                throw new CyrWordNotFoundException(Word);

            IEnumerable<CyrNounCollectionRow> rows = details.Select(x => CyrNounCollectionRow.Parse(x));
            IEnumerable<CyrNounCollectionRow> filter = rows;

            if (GenderID.HasValue)
                filter = filter.Where(x => x.GenderID == (int)GenderID);

            if (AnimateID.HasValue)
                filter = filter.Where(x => x.AnimateID == (int)AnimateID);

            if (TypeID.HasValue)
                filter = filter.Where(x => x.TypeID == (int)TypeID);

            CyrNounCollectionRow row = filter.FirstOrDefault();

            if (row == null && Condition == GetConditionsEnum.Similar)
                row = rows.FirstOrDefault();
            if (row == null)
                throw new CyrWordNotFoundException(Word);

            return new CyrNoun(Word, t, (GendersEnum)row.GenderID, (AnimatesEnum)row.AnimateID, (WordTypesEnum)row.TypeID, nounRules[row.RuleID]);
        }
    }
}
