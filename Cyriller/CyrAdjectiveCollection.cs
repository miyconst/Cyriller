using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Cyriller.Model;

namespace Cyriller
{
    public partial class CyrAdjectiveCollection : CyrBaseCollection
    {
        public const string AdjectivesResourceName = "adjectives.gz";

        /// <summary>
        /// Список правил склонения.
        /// </summary>
        protected List<CyrRule[]> rules = new List<CyrRule[]>();

        /// <summary>
        /// Словарь всех доступных прилагательных.
        /// </summary>
        protected Dictionary<DictionaryKey, CyrAdjective> words = new Dictionary<DictionaryKey, CyrAdjective>();

        /// <summary>
        /// Список всех прилагательных во всех доступных формах, для поиска наиболее подходящего совпадения, если слово отсутствует в словаре.
        /// </summary>
        protected string[] wordCandidates;

        protected AnimatesEnum[] animates = Enum.GetValues(typeof(AnimatesEnum)).OfType<AnimatesEnum>().ToArray();

        /// <summary>
        /// Минимальное кол-во совпадающих символов с конца слова, при поиске наиболее подходящего варианта.
        /// </summary>
        public int AdjectiveMinSameLetters { get; set; } = 3;

        /// <summary>
        /// Максимальное кол-во совпадающих символов с конца слова, при поиске наиболее подходящего варианта.
        /// </summary>
        public int AdjectiveMaxSameLetters { get; set; } = 4;

        public CyrAdjectiveCollection()
        {
            this.FillDictionaries();
        }

        #region Public methods
        /// <summary>
        /// Возвращает список всех прилагательных (<see cref="CyrAdjective"/>) из текущего словаря.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CyrAdjective> SelectAdjectives()
        {
            return this.words.Distinct().Select(x => new CyrAdjective(x.Value));
        }

        /// <summary>
        /// Поиск прилагательного по точному совпадению с автоматическим определением рода, числа и падежа.
        /// Выбрасывает <see cref="CyrWordNotFoundException"/> если слово не было найдено.
        /// </summary>
        /// <param name="word">Прилагательное в любом роде, числе и падеже.</param>
        /// <param name="gender">Род найденного прилагательного.</param>
        /// <param name="number">Число найденного прилагательного.</param>
        /// <param name="case">Падеж найденного прилагательного.</param>
        /// <returns></returns>
        public CyrAdjective Get(string word, out GendersEnum gender, out CasesEnum @case, out NumbersEnum number, out AnimatesEnum animate)
        {
            CyrAdjective adjective = this.GetOrDefault(word, out gender, out @case, out number, out animate);

            if (adjective == null)
            {
                throw new CyrWordNotFoundException(word, gender, @case, number, animate);
            }

            return adjective;
        }

        /// <summary>
        /// Поиск прилагательного по неточному совпадению с автоматическим определением рода, числа и падежа.
        /// Выбрасывает <see cref="CyrWordNotFoundException"/> если слово не было найдено.
        /// </summary>
        /// <param name="word">Прилагательное в любом роде, числе и падеже.</param>
        /// <param name="foundWord">Прилагательное, найденное в словаре.</param>
        /// <param name="gender">Род найденного прилагательного.</param>
        /// <param name="number">Число найденного прилагательного.</param>
        /// <param name="case">Падеж найденного прилагательного.</param>
        /// <returns></returns>
        public CyrAdjective Get(string word, out string foundWord, out GendersEnum gender, out CasesEnum @case, out NumbersEnum number, out AnimatesEnum animate)
        {
            CyrAdjective adjective = this.GetOrDefault(word, out foundWord, out gender, out @case, out number, out animate);

            if (adjective == null)
            {
                throw new CyrWordNotFoundException(word, gender, @case, number, animate);
            }

            return adjective;
        }

        /// <summary>
        /// Поиск прилагательного по точному совпадению с автоматическим определением рода, числа и падежа.
        /// Возвращает null если слово не было найдено.
        /// </summary>
        /// <param name="word">Прилагательное в любом роде, числе и падеже.</param>
        /// <param name="gender">Род найденного прилагательного.</param>
        /// <param name="number">Число найденного прилагательного.</param>
        /// <param name="case">Падеж найденного прилагательного.</param>
        /// <returns></returns>
        public CyrAdjective GetOrDefault(string word, out GendersEnum gender, out CasesEnum @case, out NumbersEnum number, out AnimatesEnum animate)
        {
            DictionaryKey key;
            CyrAdjective adjective;

            foreach (AnimatesEnum a in this.animates)
            {
                foreach (GendersEnum g in this.genders)
                {
                    foreach (CasesEnum c in this.cases)
                    {
                        key = new DictionaryKey(word, g, c, NumbersEnum.Singular, a);

                        if (this.words.TryGetValue(key, out adjective))
                        {
                            gender = key.Gender;
                            @case = key.Case;
                            number = key.Number;
                            animate = key.Animate;

                            return adjective;
                        }
                    }
                }

                foreach (CasesEnum c in this.cases)
                {
                    key = new DictionaryKey(word, 0, c, NumbersEnum.Plural, a);

                    if (this.words.TryGetValue(key, out adjective))
                    {
                        gender = key.Gender;
                        @case = key.Case;
                        number = key.Number;
                        animate = key.Animate;

                        return adjective;
                    }
                }
            }

            gender = 0;
            @case = 0;
            number = 0;
            animate = 0;

            return null;
        }

        /// <summary>
        /// Поиск прилагательного по неточному совпадению с автоматическим определением рода, числа и падежа.
        /// Возвращает null если слово не было найдено.
        /// </summary>
        /// <param name="word">Прилагательное в любом роде, числе и падеже.</param>
        /// <param name="foundWord">Прилагательное, найденное в словаре.</param>
        /// <param name="gender">Род найденного прилагательного.</param>
        /// <param name="number">Число найденного прилагательного.</param>
        /// <param name="case">Падеж найденного прилагательного.</param>
        /// <returns></returns>
        public CyrAdjective GetOrDefault(string word, out string foundWord, out GendersEnum gender, out CasesEnum @case, out NumbersEnum number, out AnimatesEnum animate)
        {
            CyrAdjective adjective = this.GetOrDefault(word, out gender, out @case, out number, out animate);

            if (adjective != null)
            {
                foundWord = word;
                return new CyrAdjective(adjective);
            }

            foundWord = this.cyrData.GetSimilar(word, this.wordCandidates, this.AdjectiveMinSameLetters, this.AdjectiveMaxSameLetters);

            if (string.IsNullOrEmpty(foundWord))
            {
                return null;
            }

            adjective = this.GetOrDefault(foundWord, out gender, out @case, out number, out animate);

            if (adjective != null)
            {
                adjective = new CyrAdjective(adjective);
                adjective.SetName(word, gender, @case, number, animate);
                return adjective;
            }

            return null;
        }

        /// <summary>
        /// Поиск прилагательного по точному совпадению.
        /// Выбрасывает <see cref="CyrWordNotFoundException"/> если слово не было найдено.
        /// </summary>
        /// <param name="word">Прилагательное.</param>
        /// <param name="gender">Род, в котором указано прилагательное.</param>
        /// <param name="case">Падеж, в котором указано прилагательное.</param>
        /// <param name="number">Число, в котором указано прилагательное.</param>
        /// <param name="animate">Одушевленность прилагательного.</param>
        /// <returns></returns>
        public CyrAdjective Get(string word, GendersEnum gender, CasesEnum @case, NumbersEnum number, AnimatesEnum animate)
        {
            CyrAdjective adjective = this.GetOrDefault(word, gender, @case, number, animate);

            if (adjective == null)
            {
                throw new CyrWordNotFoundException(word, gender, @case, number, animate);
            }

            return adjective;
        }

        /// <summary>
        /// Поиск прилагательного по точному совпадению.
        /// Возвращает null если слово не было найдено.
        /// </summary>
        /// <param name="word">Прилагательное.</param>
        /// <param name="gender">Род, в котором указано прилагательное.</param>
        /// <param name="case">Падеж, в котором указано прилагательное.</param>
        /// <param name="number">Число, в котором указано прилагательное.</param>
        /// <param name="animate">Одушевленность прилагательного.</param>
        /// <returns></returns>
        public CyrAdjective GetOrDefault(string word, GendersEnum gender, CasesEnum @case, NumbersEnum number, AnimatesEnum animate)
        {
            DictionaryKey key = new DictionaryKey(word, gender, @case, number, animate);
            CyrAdjective adjective;

            if (this.words.TryGetValue(key, out adjective))
            {
                return new CyrAdjective(adjective);
            }

            return null;
        }

        /// <summary>
        /// Поиск прилагательного по неточному совпадению.
        /// Возвращает null если слово не было найдено.
        /// </summary>
        /// <param name="word">Прилагательное.</param>
        /// <param name="foundWord">Прилагательное, найденное в словаре.</param>
        /// <param name="gender">Род, в котором указано прилагательное.</param>
        /// <param name="case">Падеж, в котором указано прилагательное.</param>
        /// <param name="number">Число, в котором указано прилагательное.</param>
        /// <param name="animate">Одушевленность прилагательного.</param>
        /// <returns></returns>
        public CyrAdjective GetOrDefault(string word, out string foundWord, GendersEnum gender, CasesEnum @case, NumbersEnum number, AnimatesEnum animate)
        {
            CyrAdjective adjective = this.GetOrDefault(word, gender, @case, number, animate);

            if (adjective != null)
            {
                foundWord = word;
                return new CyrAdjective(adjective);
            }

            foundWord = this.cyrData.GetSimilar(word, this.wordCandidates, this.AdjectiveMinSameLetters, this.AdjectiveMaxSameLetters);

            if (string.IsNullOrEmpty(foundWord))
            {
                return null;
            }

            adjective = this.GetOrDefault(foundWord, gender, @case, number, animate);

            if (adjective != null)
            {
                adjective = new CyrAdjective(adjective);
                adjective.SetName(word, gender, @case, number, animate);
                return adjective;
            }

            return null;
        }
        #endregion

        #region Fill dictionaries
        /// <summary>
        /// Заполняет список правил (<see cref="rules"/>) склонения.
        /// Заполняет словарь слов (<see cref="words"/>) и коллекцию (<see cref="wordCandidates"/>) для поиска ближайших совпадений.
        /// </summary>
        protected virtual void FillDictionaries()
        {
            bool rulesBlock = true;

            List<KeyValuePair<DictionaryKey, CyrAdjective>> adjectives = new List<KeyValuePair<DictionaryKey, CyrAdjective>>();

            List<string>[] pluralWordCandidates = new List<string>[] { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() };
            List<string>[] masculineWordCandidates = new List<string>[] { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() };
            List<string>[] feminineWordCandidates = new List<string>[] { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() };
            List<string>[] neuterWordCandidates = new List<string>[] { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() };

            TextReader treader = this.cyrData.GetData(AdjectivesResourceName);
            string line;

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
                    string[] parts = line.Split(',');
                    CyrRule[] rule = new CyrRule[parts.Length];

                    for (int i = 0; i < parts.Length; i++)
                    {
                        rule[i] = new CyrRule(parts[i]);
                    }

                    this.rules.Add(rule);
                }
                else
                {
                    this.AddWordToDictionary(line, adjectives, masculineWordCandidates, feminineWordCandidates, neuterWordCandidates, pluralWordCandidates);
                }
            }

            treader.Dispose();

            foreach (KeyValuePair<DictionaryKey, CyrAdjective> pair in adjectives)
            {
                this.words[pair.Key] = pair.Value;
            }

            IEnumerable<string> candidates = null;
            List<string>[][] candidatesCollections = new List<string>[][]
            {
                masculineWordCandidates,
                feminineWordCandidates,
                neuterWordCandidates,
                pluralWordCandidates
            };

            foreach (List<string>[] collection in candidatesCollections)
            {
                for (int i = 0; i < collection.Length; i++)
                {
                    if (candidates == null)
                    {
                        candidates = collection[i];
                    }
                    else
                    {
                        candidates = candidates.Concat(collection[i]);
                    }
                }
            }

            this.wordCandidates = candidates.Distinct().ToArray();
        }

        protected virtual void AddWordToDictionary
        (
            string line,
            List<KeyValuePair<DictionaryKey, CyrAdjective>> adjectives,
            List<string>[] masculineWordCandidates,
            List<string>[] feminineWordCandidates,
            List<string>[] neuterWordCandidates,
            List<string>[] pluralWordCandidates
        )
        {
            string[] parts = line.Split(' ');

            int ruleIndex = int.Parse(parts[1]);
            CyrRule[] rules = this.rules[ruleIndex];
            CyrAdjective adjective = new CyrAdjective(parts[0], rules);

            // Женский и средний род склоняются одинаково для одушевленных и неодушевленных предметов.
            {
                CyrResult result = adjective.Decline(GendersEnum.Feminine, AnimatesEnum.Animated);

                foreach (CasesEnum @case in cases)
                {
                    feminineWordCandidates[(int)@case - 1].Add(result[(int)@case]);

                    foreach (AnimatesEnum animate in this.animates)
                    {
                        DictionaryKey key = new DictionaryKey(result[(int)@case], GendersEnum.Feminine, @case, NumbersEnum.Singular, animate);
                        adjectives.Add(new KeyValuePair<DictionaryKey, CyrAdjective>(key, adjective));
                    }
                }
            }
            {
                CyrResult result = adjective.Decline(GendersEnum.Neuter, AnimatesEnum.Animated);

                foreach (CasesEnum @case in cases)
                {
                    neuterWordCandidates[(int)@case - 1].Add(result[(int)@case]);

                    foreach (AnimatesEnum animate in this.animates)
                    {
                        DictionaryKey key = new DictionaryKey(result[(int)@case], GendersEnum.Neuter, @case, NumbersEnum.Singular, animate);
                        adjectives.Add(new KeyValuePair<DictionaryKey, CyrAdjective>(key, adjective));
                    }
                }
            }

            // Мужской род и множественное число склоняются по-разному для одушевленных и неодушевленных предметов.
            foreach (AnimatesEnum animate in animates)
            {
                CyrResult result = adjective.Decline(GendersEnum.Masculine, animate);

                foreach (CasesEnum @case in cases)
                {
                    DictionaryKey key = new DictionaryKey(result[(int)@case], GendersEnum.Masculine, @case, NumbersEnum.Singular, animate);
                    adjectives.Add(new KeyValuePair<DictionaryKey, CyrAdjective>(key, adjective));
                    masculineWordCandidates[(int)@case - 1].Add(key.Name);
                }

                result = adjective.DeclinePlural(animate);

                foreach (CasesEnum @case in cases)
                {
                    DictionaryKey key = new DictionaryKey(result[(int)@case], 0, @case, NumbersEnum.Plural, animate);
                    adjectives.Add(new KeyValuePair<DictionaryKey, CyrAdjective>(key, adjective));
                    pluralWordCandidates[(int)@case - 1].Add(key.Name);
                }
            }
        }
        #endregion
    }
}
