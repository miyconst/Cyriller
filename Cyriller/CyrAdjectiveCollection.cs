using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Cyriller.Model;

namespace Cyriller
{
    public class CyrAdjectiveCollection
    {
        /// <summary>RuleID => Rule</summary>
        protected Dictionary<int, CyrRule[]> adjectiveRules;
        /// <summary>Прилагательное мужского рода => RuleID</summary>
        protected Dictionary<string, KeyValuePair<string, int>> masculineWords;
        /// <summary>Прилагательное женского рода => Прилагательное мужского рода => RuleID</summary>
        protected Dictionary<string, KeyValuePair<string, int>> feminineWords;
        /// <summary>Прилагательное среднего рода => Прилагательное мужского рода => RuleID</summary>
        protected Dictionary<string, KeyValuePair<string, int>> neuterWords;

        public CyrAdjectiveCollection()
        {
            using (TextReader treader = CyrData.GetReader("adjective-rules.gz"))
            {
                adjectiveRules = treader.ReadToEnd().Split('\n')
                    .Select(x => x.Trim().Split(' '))
                    .ToDictionary(x => int.Parse(x[0]), x => x[1].Split(',', '|').Select(q => new CyrRule(q)).ToArray());
            }
            using (TextReader treader = CyrData.GetReader("adjectives.gz"))
            {
                masculineWords = new Dictionary<string, KeyValuePair<string, int>>();
                foreach (string line in treader.ReadToEnd().Split('\n'))
                {
                    string[] parts = line.Trim().ToLower().Split(' ');//прилагательные в нижнем регистре
                    if (parts.Length != 2 || string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]))
                        continue;
                    if (masculineWords.ContainsKey(parts[0]))
                        continue;
                    masculineWords.Add(parts[0], new KeyValuePair<string, int>(parts[0], int.Parse(parts[1])));
                }
            }
            Fill();
        }

        /// <summary>Получение прилагательного с вариантами окончания для разных падежей, числа, рода. Поиск по точному совпадению.</summary>
        /// <param name="Word">Прилагательное</param>
        /// <param name="Gender">Пол, для которого указано прилагательное</param>
        public CyrAdjective Get(string Word, GendersEnum Gender = 0)
        {
            return Get(Word, GetConditionsEnum.Strict, Gender);
        }

        /// <summary>Получение прилагательного с вариантами окончания для разных падежей, числа, рода.</summary>
        /// <param name="Word">Прилагательное</param>
        /// <param name="Condition">Вариант поиска в словаре (точный или приблизительный)</param>
        /// <param name="Gender">Пол, в котором указано прилагательное, используется при поиске неточных совпадений</param>
        public CyrAdjective Get(string Word, GetConditionsEnum Condition, GendersEnum Gender = 0)
        {
            Word = (Word ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(Word))
                throw new ArgumentException("Empty Word");

            string FoundWord = string.Empty;
            string LowerWord = Word.ToLower();
            KeyValuePair<string, int> details = GetStrictDetails(ref LowerWord, ref Gender);
            if (!string.IsNullOrEmpty(details.Key))
                FoundWord = LowerWord;
            else if (Condition == GetConditionsEnum.Similar)
                details = GetSimilarDetails(LowerWord, Gender, ref Gender, out FoundWord);

            if (string.IsNullOrEmpty(details.Key))
                throw new CyrWordNotFoundException(Word);

            CyrRule[] rule = adjectiveRules[details.Value];
            switch (Gender)
            {
                case GendersEnum.Feminine:
                    Word = rule[(int)RuleAdjectiveEnum.FromFeminine].Apply(Word);
                    break;
                case GendersEnum.Neuter:
                    Word = rule[(int)RuleAdjectiveEnum.FromNeuter].Apply(Word);
                    break;
                default:
                    Gender = GendersEnum.Masculine;
                    break;
            }

            return new CyrAdjective(Word, FoundWord, Gender, rule);
        }

        /// <summary>Поиск ID правила по точному совпадению по всем словарям. Сначала мужской, потом женский, потом средний.</summary>
        /// <param name="Word">Прилагательное</param>
        /// <param name="Gender">Пол, в котором указано прилагательное</param>
        protected KeyValuePair<string, int> GetStrictDetails(ref string Word, ref GendersEnum Gender)
        {
            KeyValuePair<string, int> details = default(KeyValuePair<string, int>);
            switch (Gender)
            {
                case GendersEnum.Feminine:
                    details = CyrData.GetDictionaryItem(Word, feminineWords);
                    if (details.Key.IsNotNullOrEmpty())
                    {
                        Gender = GendersEnum.Feminine;
                        Word = details.Key;
                        return details;
                    }
                    break;
                case GendersEnum.Neuter:
                    details = CyrData.GetDictionaryItem(Word, neuterWords);
                    if (details.Key.IsNotNullOrEmpty())
                    {
                        Gender = GendersEnum.Neuter;
                        Word = details.Key;
                        return details;
                    }
                    break;

            }

            details = CyrData.GetDictionaryItem(Word, masculineWords);
            if (!string.IsNullOrEmpty(details.Key))
            {
                Gender = GendersEnum.Masculine;
                Word = details.Key;
                return details;
            }

            if (Gender == 0)
            {
                details = CyrData.GetDictionaryItem(Word, feminineWords);
                if (details.Key.IsNotNullOrEmpty())
                {
                    Gender = GendersEnum.Feminine;
                    Word = details.Key;
                    return details;
                }

                details = CyrData.GetDictionaryItem(Word, neuterWords);
                if (details.Key.IsNotNullOrEmpty())
                {
                    Gender = GendersEnum.Neuter;
                    Word = details.Key;
                    return details;
                }
            }

            return details;
        }

        /// <summary>Поиск пары (прилагательное мужского рода => ID правила) по неточному совпадению по словарям</summary>
        /// <param name="Word">Искомое слово</param>
        /// <param name="DefaultGender">Если 0, то ищем в мужском, потом в женском, потом в среднем. Если нет, то в соответствующем, потом в мужском.</param>
        /// <param name="Gender">Род, для которого нашлось совпадение</param>
        /// <param name="FoundWord">Слово, которое по окончанию подходит к искомому слову</param>
        protected KeyValuePair<string, int> GetSimilarDetails(string Word, GendersEnum DefaultGender, ref GendersEnum Gender, out string FoundWord)
        {
            FoundWord = string.Empty;
            KeyValuePair<string, int> details = default(KeyValuePair<string, int>);

            switch (DefaultGender)
            {
                case GendersEnum.Feminine:
                    details = CyrData.GetSimilarDetails(Word, feminineWords, out FoundWord);
                    Gender = GendersEnum.Feminine;
                    break;
                case GendersEnum.Neuter:
                    details = CyrData.GetSimilarDetails(Word, neuterWords, out FoundWord);
                    Gender = GendersEnum.Neuter;
                    break;
            }
            if (!string.IsNullOrEmpty(details.Key))
                return details;

            details = CyrData.GetSimilarDetails(Word, masculineWords, out FoundWord);
            if (!string.IsNullOrEmpty(details.Key))
            {
                Gender = GendersEnum.Masculine;
                return details;
            }

            if (DefaultGender == 0)
            {
                details = CyrData.GetSimilarDetails(Word, feminineWords, out FoundWord);
                if (!string.IsNullOrEmpty(details.Key))
                {
                    Gender = GendersEnum.Feminine;
                    return details;
                }

                details = CyrData.GetSimilarDetails(Word, neuterWords, out FoundWord);
                if (!string.IsNullOrEmpty(details.Key))
                {
                    Gender = GendersEnum.Neuter;
                    return details;
                }
            }

            return details;
        }

        protected void Fill()
        {
            feminineWords = new Dictionary<string, KeyValuePair<string, int>>();
            neuterWords = new Dictionary<string, KeyValuePair<string, int>>();
            foreach (KeyValuePair<string, int> item in masculineWords.Values)
            {
                string w = adjectiveRules[item.Value][(int)RuleAdjectiveEnum.FeminineNominative].Apply(item.Key);
                if (!string.IsNullOrEmpty(w) && !feminineWords.ContainsKey(w))
                    feminineWords.Add(w, item);

                w = adjectiveRules[item.Value][(int)RuleAdjectiveEnum.NeuterNominative].Apply(item.Key);
                if (!string.IsNullOrEmpty(w) && !neuterWords.ContainsKey(w))
                    neuterWords.Add(w, item);
            }
        }
    }
}
