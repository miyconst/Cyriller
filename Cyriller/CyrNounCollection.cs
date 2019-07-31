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
    public partial class CyrNounCollection : CyrBaseCollection
    {
        private const string NounsResourceName = "nouns.gz";

        /// <summary>
        /// Список правил склонения.
        /// </summary>
        protected List<CyrRule[]> rules = new List<CyrRule[]>();

        /// <summary>
        /// Словарь всех доступных существительных.
        /// </summary>
        protected Dictionary<DictionaryKey, CyrNoun> words = new Dictionary<DictionaryKey, CyrNoun>();

        /// <summary>
        /// Минимальное кол-во совпадающих символов с конца слова, при поиске наиболее подходящего варианта.
        /// </summary>
        public int NounMinSameLetters { get; set; } = 2;

        /// <summary>
        /// Максимальное кол-во совпадающих символов с конца слова, при поиске наиболее подходящего варианта.
        /// </summary>
        public int NounMaxSameLetters { get; set; } = int.MaxValue;

        public CyrNounCollection()
        {
            CyrData data = new CyrData();
            TextReader treader;
            string line;
            bool rulesBlock = true;

            treader = data.GetData(NounsResourceName);

            List<string>[] singularWordCandidates = new List<string>[] { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() };
            List<string>[] pluralWordCandidates = new List<string>[] { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() };

            while (true)
            {
                line = treader.ReadLine();

                if (line == null)
                {
                    break;
                }
                else if (rulesBlock && line == EndOfTheRulesBlock)
                {
                    rulesBlock = false;
                    continue;
                }
                else if (this.IsSkipLine(line))
                {
                    continue;
                }

                if (rulesBlock)
                {
                    string[] parts = line.Split(',', '|');
                    CyrRule[] rule = new CyrRule[parts.Length];

                    for (int i = 0; i < parts.Length; i++)
                    {
                        rule[i] = new CyrRule(parts[i]);
                    }

                    this.rules.Add(rule);
                }
                else
                {
                    this.AddWordToTheCollection(line, singularWordCandidates, pluralWordCandidates);
                }
            }

            treader.Dispose();

            {
                IEnumerable<string> candidates = new string[0];

                foreach (List<string> candidate in singularWordCandidates)
                {
                    candidates = candidates.Concat(candidate);
                }

                foreach (List<string> candidate in pluralWordCandidates)
                {
                    candidates = candidates.Concat(candidate);
                }

                this.wordCandidates = candidates.Distinct().ToList();
            }
        }

        /// <summary>
        /// Поиск существительного по точному совпадению с автоматическим определением рода, числа и падежа.
        /// Выбрасывает <see cref="CyrWordNotFoundException"/> если слово не было найдено.
        /// </summary>
        /// <param name="word">Существительное в любом роде, числе и падеже.</param>
        /// <param name="number">Число найденного существительного.</param>
        /// <param name="case">Падеж найденного существительного.</param>
        /// <returns></returns>
        public CyrNoun Get(string word, out CasesEnum @case, out NumbersEnum number)
        {
            CyrNoun noun = this.GetOrDefault(word, out @case, out number);

            if (noun == null)
            {
                throw new CyrWordNotFoundException(word, 0, @case, number, 0);
            }

            return noun;
        }

        /// <summary>
        /// Поиск существительного по неточному совпадению с автоматическим определением рода, числа и падежа.
        /// Выбрасывает <see cref="CyrWordNotFoundException"/> если слово не было найдено.
        /// </summary>
        /// <param name="word">Существительное в любом роде, числе и падеже.</param>
        /// <param name="foundWord">Существительное, найденное в словаре.</param>
        /// <param name="number">Число найденного существительного.</param>
        /// <param name="case">Падеж найденного существительного.</param>
        /// <returns></returns>
        public CyrNoun Get(string word, out string foundWord, out CasesEnum @case, out NumbersEnum number)
        {
            CyrNoun noun = this.GetOrDefault(word, out foundWord, out @case, out number);

            if (noun == null)
            {
                throw new CyrWordNotFoundException(word, 0, @case, number, 0);
            }

            return noun;
        }

        /// <summary>
        /// Поиск существительного по точному совпадению с автоматическим определением рода, числа и падежа.
        /// Возвращает null если слово не было найдено.
        /// </summary>
        /// <param name="word">Существительное в любом роде, числе и падеже.</param>
        /// <param name="number">Число найденного существительного.</param>
        /// <param name="case">Падеж найденного существительного.</param>
        /// <returns></returns>
        public CyrNoun GetOrDefault(string word, out CasesEnum @case, out NumbersEnum number)
        {
            DictionaryKey key;
            CyrNoun noun;

            // Существительные, без формы единственного числа, не имеют рода. К примеру "ворота" или "шахматы".
            GendersEnum[] gendersWithZero = new GendersEnum[] { GendersEnum.Masculine, GendersEnum.Feminine, GendersEnum.Neuter, GendersEnum.Undefined };

            foreach (CasesEnum c in this.cases)
            {
                foreach (NumbersEnum n in this.numbers)
                {
                    foreach (GendersEnum g in gendersWithZero)
                    {
                        key = new DictionaryKey(word, g, c, n);

                        if (this.words.TryGetValue(key, out noun))
                        {
                            @case = key.Case;
                            number = key.Number;

                            return noun;
                        }
                    }
                }
            }

            @case = 0;
            number = 0;

            return null;
        }

        /// <summary>
        /// Поиск существительного по неточному совпадению с автоматическим определением рода, числа и падежа.
        /// Возвращает null если слово не было найдено.
        /// </summary>
        /// <param name="word">Существительное в любом роде, числе и падеже.</param>
        /// <param name="foundWord">Существительное, найденное в словаре.</param>
        /// <param name="number">Число найденного существительного.</param>
        /// <param name="case">Падеж найденного существительного.</param>
        /// <returns></returns>
        public CyrNoun GetOrDefault(string word, out string foundWord, out CasesEnum @case, out NumbersEnum number)
        {
            CyrNoun noun = this.GetOrDefault(word, out @case, out number);

            if (noun != null)
            {
                foundWord = word;
                return new CyrNoun(noun);
            }

            foundWord = this.GetSimilar(word, this.NounMinSameLetters, this.NounMaxSameLetters);

            if (string.IsNullOrEmpty(foundWord))
            {
                return null;
            }

            noun = this.GetOrDefault(foundWord, out @case, out number);

            if (noun != null)
            {
                noun = new CyrNoun(noun);
                noun.SetName(word, @case, number);
                return noun;
            }

            return null;
        }

        /// <summary>
        /// Поиск существительного по точному совпадению.
        /// Выбрасывает <see cref="CyrWordNotFoundException"/> если слово не было найдено.
        /// </summary>
        /// <param name="word">Существительное.</param>
        /// <param name="gender">Род, в котором указано существительного.</param>
        /// <param name="case">Падеж, в котором указано существительного.</param>
        /// <param name="number">Число, в котором указано существительного.</param>
        /// <returns></returns>
        public CyrNoun Get(string word, GendersEnum gender, CasesEnum @case, NumbersEnum number)
        {
            CyrNoun noun = this.GetOrDefault(word, gender, @case, number);

            if (noun == null)
            {
                throw new CyrWordNotFoundException(word, gender, @case, number, 0);
            }

            return noun;
        }

        /// <summary>
        /// Поиск существительного по точному совпадению.
        /// Возвращает null если слово не было найдено.
        /// </summary>
        /// <param name="word">Существительное.</param>
        /// <param name="gender">Род, в котором указано существительного.</param>
        /// <param name="case">Падеж, в котором указано существительного.</param>
        /// <param name="number">Число, в котором указано существительного.</param>
        /// <returns></returns>
        public CyrNoun GetOrDefault(string word, GendersEnum gender, CasesEnum @case, NumbersEnum number)
        {
            DictionaryKey key = new DictionaryKey(word, gender, @case, number);
            CyrNoun noun;

            if (this.words.TryGetValue(key, out noun))
            {
                return new CyrNoun(noun);
            }

            return null;
        }

        /// <summary>
        /// Поиск существительного по неточному совпадению.
        /// Возвращает null если слово не было найдено.
        /// </summary>
        /// <param name="word">Существительное.</param>
        /// <param name="foundWord">Существительное, найденное в словаре.</param>
        /// <param name="gender">Род, в котором указано существительного.</param>
        /// <param name="case">Падеж, в котором указано существительного.</param>
        /// <param name="number">Число, в котором указано существительного.</param>
        /// <returns></returns>
        public CyrNoun GetOrDefault(string word, out string foundWord, GendersEnum gender, CasesEnum @case, NumbersEnum number)
        {
            CyrNoun noun = this.GetOrDefault(word, gender, @case, number);

            if (noun != null)
            {
                foundWord = word;
                return new CyrNoun(noun);
            }

            foundWord = this.GetSimilar(word, this.NounMinSameLetters, this.NounMaxSameLetters);

            if (string.IsNullOrEmpty(foundWord))
            {
                return null;
            }

            noun = this.GetOrDefault(foundWord, gender, @case, number);

            if (noun != null)
            {
                noun = new CyrNoun(noun);
                noun.SetName(word, @case, number);
                return noun;
            }

            return null;
        }

        /// <summary>
        /// Возвращает список всех существительных (<see cref="CyrNoun"/>) из текущего словаря.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CyrNoun> SelectNouns()
        {
            return this.words.Select(x => x.Value).Distinct().Select(x => new CyrNoun(x));
        }

        /// <summary>
        /// Добавляет слово в <see cref="words"/> словарь.
        /// </summary>
        /// <param name="line">
        /// Строка со словом в формате словаря.
        /// Смотри /Cyriller/App_Data/nouns.txt.
        /// </param>
        protected virtual void AddWordToTheCollection(string line, List<string>[] singularWordCandidates, List<string>[] pluralWordCandidates)
        {
            string[] parts = line.Split(' ');

            CyrNounCollectionRow row = CyrNounCollectionRow.Parse(parts[1]);
            CyrRule[] rules = this.rules[row.RuleID];
            CyrNoun noun = new CyrNoun(parts[0], (GendersEnum)row.GenderID, (AnimatesEnum)row.AnimateID, (WordTypesEnum)row.TypeID, rules);

            CyrResult singular = noun.Decline();
            CyrResult plural = noun.DeclinePlural();

            foreach (CasesEnum @case in this.cases)
            {
                if (!string.IsNullOrEmpty(singular.Get(@case)))
                {
                    DictionaryKey key = new DictionaryKey(singular.Get(@case), noun.Gender, @case, NumbersEnum.Singular);
                    singularWordCandidates[(int)@case - 1].Add(singular.Get(@case));
                    this.words[key] = noun;
                }

                if (!string.IsNullOrEmpty(plural.Get(@case)))
                {
                    DictionaryKey key = new DictionaryKey(plural.Get(@case), noun.Gender, @case, NumbersEnum.Plural);
                    pluralWordCandidates[(int)@case - 1].Add(plural.Get(@case));
                    this.words[key] = noun;
                }
            }
        }
    }
}
