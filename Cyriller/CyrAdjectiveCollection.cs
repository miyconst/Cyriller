using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Cyriller.Model;

namespace Cyriller
{
    public class CyrAdjectiveCollection
    {
        protected Dictionary<int, string> rules = new Dictionary<int, string>();
        protected Dictionary<string, string> masculineWords = new Dictionary<string, string>();
        protected Dictionary<string, KeyValuePair<string, string>> feminineWords = new Dictionary<string, KeyValuePair<string, string>>();
        protected Dictionary<string, KeyValuePair<string, string>> neuterWords = new Dictionary<string, KeyValuePair<string, string>>();
        /// <summary>Минимальная длина слова, которая позволяет различать слова</summary>
        protected int MinWordLength = 3;

        public CyrAdjectiveCollection()
        {
            CyrData data = new CyrData();
            TextReader treader = data.GetData("adjective-rules.gz");
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
            treader = data.GetData("adjectives.gz");
            line = treader.ReadLine();

            while (line != null)
            {
                parts = line.Split(' ');

                if (!masculineWords.ContainsKey(parts[0]))
                {
                    masculineWords.Add(parts[0], parts[1]);
                }

                line = treader.ReadLine();
            }

            treader.Dispose();

            this.Fill();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Word">Прилагательное</param>
        /// <param name="DefaultGender">Пол, в котором указано прилагательное, используется при поиске неточных совпадений</param>
        /// <returns></returns>
        public CyrAdjective Get(string Word, GendersEnum DefaultGender = GendersEnum.Masculine)
        {
            return this.Get(Word, GetConditionsEnum.Strict, DefaultGender);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Word">Прилагательное</param>
        /// <param name="Condition">Вариант поиска в словаре</param>
        /// <param name="DefaultGender">Пол, в котором указано прилагательное, используется при поиске неточных совпадений</param>
        /// <returns></returns>
        public CyrAdjective Get(string Word, GetConditionsEnum Condition, GendersEnum DefaultGender = GendersEnum.Masculine)
        {
            GendersEnum gender = GendersEnum.Masculine;
            string t = Word;
            string details = this.GetStrictDetails(ref t, ref gender);

            if (details.IsNullOrEmpty() && Condition == GetConditionsEnum.Similar)
            {
                details = this.GetSimilarDetails(Word, DefaultGender, ref gender, out t);
            }

            if (details.IsNullOrEmpty())
            {
                throw new CyrWordNotFoundException(Word);
            }

            int ruleID = int.Parse(details);
            string[] parts = this.rules[ruleID].Split(',');

            CyrRule[] rules = parts.Select(val => new CyrRule(val)).ToArray();

            if (gender == GendersEnum.Feminine)
            {
                Word = rules[22].Apply(Word);
            }
            else if(gender == GendersEnum.Neuter)
            {
                Word = rules[23].Apply(Word);
            }

            CyrAdjective adj = new CyrAdjective(Word, t, gender, rules);

            return adj;
        }

        protected string GetStrictDetails(ref string Word, ref GendersEnum Gender)
        {
            string details = this.GetDictionaryItem(Word, this.masculineWords);

            if (details.IsNullOrEmpty())
            {
                KeyValuePair<string, string> f = this.GetDictionaryItem(Word, this.feminineWords);

                if (f.Key.IsNotNullOrEmpty())
                {
                    Word = f.Key;
                    details = f.Value;
                    Gender = GendersEnum.Feminine;
                }
            }

            if (details.IsNullOrEmpty())
            {
                KeyValuePair<string, string> f = this.GetDictionaryItem(Word, this.neuterWords);

                if (f.Key.IsNotNullOrEmpty())
                {
                    Word = f.Key;
                    details = f.Value;
                    Gender = GendersEnum.Neuter;
                }
            }

            return details;
        }

        protected string GetSimilarDetails(string Word, GendersEnum DefaultGender, ref GendersEnum Gender, out string FoundWord)
        {
            string similar = Word;
            KeyValuePair<string, string> v;

            switch (DefaultGender)
            {
                case GendersEnum.Feminine:
                    v = this.GetSimilarDetails(Word, this.feminineWords, out FoundWord);
                    similar = v.Key;
                    Gender = GendersEnum.Feminine;
                    break;
                case GendersEnum.Neuter:
                    v = this.GetSimilarDetails(Word, this.neuterWords, out FoundWord);
                    similar = v.Key;
                    Gender = GendersEnum.Neuter;
                    break;
            }

            string details = this.GetSimilarDetails(similar, this.masculineWords, out FoundWord);

            if (details.IsNullOrEmpty() && DefaultGender == 0)
            {
                v = this.GetSimilarDetails(Word, this.feminineWords, out FoundWord);
                similar = v.Key;
                Gender = GendersEnum.Feminine;
                details = this.GetSimilarDetails(similar, this.masculineWords, out FoundWord);
            }

            if (details.IsNullOrEmpty() && DefaultGender == 0)
            {
                v = this.GetSimilarDetails(Word, this.neuterWords, out FoundWord);
                similar = v.Key;
                Gender = GendersEnum.Neuter;
                details = this.GetSimilarDetails(similar, this.masculineWords, out FoundWord);
            }

            return details;
        }

        protected T GetDictionaryItem<T>(string Key, Dictionary<string, T> Items)
        {
            string t = Key;
            T details = this.GetDetails(t, Items);

            if (details == null)
            {
                t = Key.ToLower();
                details = this.GetDetails(t, Items);
            }

            if (details == null)
            {
                t = Key.ToLower().UppercaseFirst();
                details = this.GetDetails(t, Items);
            }

            if (details == null)
            {
                List<int> indexes = new List<int>();
                string lower = Key.ToLower();

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
                    details = this.GetDetails(t, Items);

                    if (details != null)
                    {
                        break;
                    }
                }
            }

            return details;
        }

        protected T GetSimilarDetails<T>(string Word, Dictionary<string, T> Collection, out string CollectionWord)
        {
            CyrData data = new CyrData();

            CollectionWord = data.GetSimilar(Word, Collection.Keys.ToList(), MinWordLength);

            if (CollectionWord.IsNullOrEmpty())
            {
                return default(T);
            }

            return this.GetDetails(CollectionWord, Collection);
        }

        protected T GetDetails<T>(string Word, Dictionary<string, T> Collection)
        {
            if (Collection.ContainsKey(Word))
            {
                return Collection[Word];
            }

            return default(T);
        }

        protected void Fill()
        {
            foreach (KeyValuePair<string, string> item in this.masculineWords)
            {
                string rules = this.rules[int.Parse(item.Value)];
                string[] parts = rules.Split(',');
                CyrRule rule = new CyrRule(parts[5]);
                string w = rule.Apply(item.Key);

                if (!this.feminineWords.ContainsKey(w))
                {
                    this.feminineWords.Add(w, item);
                }

                rule = new CyrRule(parts[11]);
                w = rule.Apply(item.Key);

                if (!this.neuterWords.ContainsKey(w))
                {
                    this.neuterWords.Add(w, item);
                }
            }
        }
    }
}
