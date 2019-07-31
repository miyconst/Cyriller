using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Cyriller.Model;

namespace Cyriller
{
    public abstract class CyrBaseCollection
    {
        public const string EndOfTheRulesBlock = "// -- End of the Rules block -- //";

        protected GendersEnum[] genders = Enum.GetValues(typeof(GendersEnum)).OfType<GendersEnum>().ToArray();
        protected CasesEnum[] cases = Enum.GetValues(typeof(CasesEnum)).OfType<CasesEnum>().ToArray();
        protected NumbersEnum[] numbers = Enum.GetValues(typeof(NumbersEnum)).OfType<NumbersEnum>().ToArray();
        protected CyrData cyrData = new CyrData();

        /// <summary>
        /// Список всех существительных во всех доступных формах, для поиска наиболее подходящего совпадения, если слово отсутствует в словаре.
        /// </summary>
        protected List<string> wordCandidates;

        /// <summary>
        /// Кэш для слов, которых нет в словаре.
        /// </summary>
        protected Dictionary<string, string> similarSearchCache = new Dictionary<string, string>();

        /// <summary>
        /// Если true - результат склонения несуществующих слов будет добавлен в кэш.
        /// True - по умолчанию.
        /// </summary>
        public bool EnableCache { get; set; } = true;

        protected virtual bool IsSkipLine(string line)
        {
            bool comment = string.IsNullOrWhiteSpace(line) || line.StartsWith("//") || line.StartsWith("#");
            return comment;
        }

        /// <summary>
        /// Находит ближайшее совпадение для предоставленного слова в коллекции <see cref="wordCandidates"/>.
        /// Использует кэш, если <see cref="EnableCache"/> true.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="minSameLetters"></param>
        /// <param name="maxSameLetters"></param>
        /// <returns></returns>
        protected virtual string GetSimilar(string word, int minSameLetters, int maxSameLetters)
        {
            string foundWord;

            if (!this.EnableCache || !this.similarSearchCache.TryGetValue(word, out foundWord))
            {
                foundWord = this.cyrData.GetSimilar(word, this.wordCandidates, minSameLetters, maxSameLetters);
            }

            if (this.EnableCache)
            {
                this.similarSearchCache[word] = foundWord;
            }

            return foundWord;
        }
    }
}
