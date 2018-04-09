using System;

namespace Cyriller
{
    public class CyrRule
    {
        /// <summary>Применяемое окончание</summary>
        protected string end;
        /// <summary>Количество заменяемых символов в конце слова</summary>
        protected int cut;

        /// <summary>Правило для применения окончания</summary>
        /// <param name="Rule">Строка, включающая в себя применяемое окончание и количество заменяемых символов в конце слова</param>
        public CyrRule(string Rule)
        {
            Rule = (Rule ?? string.Empty).Trim();
            int pos = -1;
            while (++pos < Rule.Length)
            {
                if (Char.IsDigit(Rule[pos]))
                    break;
            }

            end = Rule.Substring(0, pos);
            int.TryParse(Rule.Substring(pos), out cut);
        }

        /// <summary>Применение правила замены окончания к заданному слову</summary>
        /// <param name="Name">Слово, к которому применяется правило замены окончания</param>
        public string Apply(string Name)
        {
            if (end == "*")
                return string.Empty;
            if (Name.Length <= cut)
                return string.Empty;
            return Name.Substring(0, Name.Length - cut) + end;
        }
    }
}
