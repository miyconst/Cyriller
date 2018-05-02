using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Resources;
using Cyriller.Model;

namespace Cyriller
{
    public class CyrNounCollection
    {
        protected Dictionary<int, string> rules = new Dictionary<int, string>();
        protected Dictionary<string, List<string>> words = new Dictionary<string, List<string>>();

        public CyrNounCollection()
        {
            CyrData data = new CyrData();
            TextReader treader = data.GetData("noun-rules.gz");
            string line;
            string[] parts;

            line = treader.ReadLine();

            while (line != null)
            {
                parts = line.Split(' ');
                rules.Add(int.Parse(parts[0]), parts[1]);
                line = treader.ReadLine();
            }

            treader.Dispose();
            treader = data.GetData("nouns.gz");
            line = treader.ReadLine();

            while (line != null)
            {
                parts = line.Split(' ');

                if (!words.ContainsKey(parts[0]))
                {
                    words.Add(parts[0], new List<string>());
                }

                words[parts[0]].Add(parts[1]);

                line = treader.ReadLine();
            }

            treader.Dispose();
        }

        public CyrNoun Get(string Word)
        {
            return this.Get(Word, GetConditionsEnum.Strict);
        }

        public CyrNoun Get(string Word, GetConditionsEnum Condition, GendersEnum? GenderID = null, AnimatesEnum? AnimateID = null, WordTypesEnum? TypeID = null)
        {
            string t = Word;
            List<string> list = this.GetDetails(t);

            if (list == null || !list.Any())
            {
                t = Word.ToLower();
                list = this.GetDetails(t);
            }

            if (list == null || !list.Any())
            {
                t = Word.ToLower().UppercaseFirst();
                list = this.GetDetails(t);
            }

            if (list == null || !list.Any())
            {
                List<int> indexes = new List<int>();
                string lower = Word.ToLower();

                for (int i = 0; i < lower.Length; i++)
                {
                    if (lower[i] == 'е')
                    {
                        indexes.Add(i);
                    }
                }

                foreach (int index in indexes)
                {
                    t = lower.Substring(0, index) + "ё" + lower.Substring(index + 1);
                    list = this.GetDetails(t);

                    if (list != null && list.Any())
                    {
                        break;
                    }
                }
            }

            if ((list == null || !list.Any()) && Condition == GetConditionsEnum.Similar)
            {
                list = this.GetSimilarDetails(Word, out t);
            }

            if (list == null || !list.Any())
            {
                throw new CyrWordNotFoundException(Word);
            }

            IEnumerable<CyrNounCollectionRow> rows = list.Select(x => CyrNounCollectionRow.Parse(x));
            IEnumerable<CyrNounCollectionRow> filter = rows;

            if (GenderID.HasValue)
            {
                filter = filter.Where(x => x.GenderID == (int)GenderID);
            }

            if (AnimateID.HasValue)
            {
                filter = filter.Where(x => x.AnimateID == (int)AnimateID);
            }

            if (TypeID.HasValue)
            {
                filter = filter.Where(x => x.TypeID == (int)TypeID);
            }

            CyrNounCollectionRow row = filter.FirstOrDefault();

            if (row == null && Condition == GetConditionsEnum.Similar)
            {
                row = rows.FirstOrDefault();
            }

            if (row == null)
            {
                throw new CyrWordNotFoundException(Word);
            }

            string[] parts = this.rules[row.RuleID].Split(new char[] { ',', '|' });

            CyrRule[] rules = parts.Select(val => new CyrRule(val)).ToArray();
            CyrNoun noun = new CyrNoun(Word, t, (GendersEnum)row.GenderID, (AnimatesEnum)row.AnimateID, (WordTypesEnum)row.TypeID, rules);

            return noun;
        }

        protected List<string> GetSimilarDetails(string Word, out string CollectionWord)
        {
            CyrData data = new CyrData();
            
            CollectionWord = data.GetSimilar(Word, words.Keys.ToList());

            if (CollectionWord.IsNullOrEmpty())
            {
                return null;
            }

            return this.GetDetails(CollectionWord);
        }

        protected List<string> GetDetails(string Word)
        {
            if (Word.IsNullOrEmpty())
            {
                return null;
            }

            if (words.ContainsKey(Word))
            {
                return words[Word];
            }

            return null;
        }
    }
}
