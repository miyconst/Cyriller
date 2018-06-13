using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Cyriller
{
    public class CyrRule
    {
        /// <summary>Ending part of a word</summary>
        protected string end;
        /// <summary>The number of characters to replace at the end of a word</summary>
        protected int cut;

        /// <summary>The rule applied to the end of a word</summary>
        /// <param name="Rule">A string that includes the applied ending and the number of characters to replace at the end of a word</param>
        public CyrRule(string Rule)
        {
            if (string.IsNullOrEmpty(Rule))
            {
                this.end = string.Empty;
                this.cut = 0;
                return;
            }
            int pos = -1;
            while (++pos < Rule.Length)
            {
                if (Char.IsDigit(Rule[pos]))
                    break;
            }

            this.end = Rule.Substring(0, pos);
            int.TryParse(Rule.Substring(pos), out this.cut);
        }

        /// <summary>Applies replacement rule for end of word</summary>
        /// <param name="Name">The word, to which applies rule of replacement the ending</param>
        public string Apply(string Name)
        {
            if (this.end == "*"
                || string.IsNullOrEmpty(Name)
                || Name.Length <= cut)
            {
                return string.Empty;
            }

            int length = Name.Length - cut;

            return Name.Substring(0, length) + end;
        }
    }
}
