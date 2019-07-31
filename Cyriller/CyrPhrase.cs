using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyriller.Model;

namespace Cyriller
{
    public class CyrPhrase
    {
        protected CyrNounCollection nounCollection;
        protected CyrAdjectiveCollection adjCollection;

        public CyrPhrase(CyrNounCollection nounCollection, CyrAdjectiveCollection adjCollection)
        {
            this.nounCollection = nounCollection;
            this.adjCollection = adjCollection;
        }

        public SpeechPartsEnum DetermineSpeechPart(string word)
        {
            string[] ends = new string[] { "ый", "ий", "ой", "ся", "ая", "яя", "ое", "ее" };

            if (ends.Any(val => word.EndsWith(val)))
            {
                return SpeechPartsEnum.Adjective;
            }

            return SpeechPartsEnum.Noun;
        }

        public CyrResult Decline(string phrase, GetConditionsEnum condition)
        {
            return this.Decline(phrase, condition, NumbersEnum.Singular);
        }

        public CyrResult DeclinePlural(string Phrase, GetConditionsEnum Condition)
        {
            return this.Decline(Phrase, Condition, NumbersEnum.Plural);
        }

        protected CyrResult Decline(string phrase, GetConditionsEnum condition, NumbersEnum number)
        {
            if (phrase.IsNullOrEmpty())
            {
                return new CyrResult();
            }

            List<object> words = new List<object>();
            string[] parts = phrase.Split(' ').Select(val => val.Trim()).Where(val => val.IsNotNullOrEmpty()).ToArray();
            List<CyrResult> results = new List<CyrResult>();
            
            foreach (string w in parts)
            {
                SpeechPartsEnum speech = this.DetermineSpeechPart(w);
                string fw;
                GendersEnum g;
                CasesEnum c;
                NumbersEnum n;

                switch (speech)
                {
                    case SpeechPartsEnum.Adjective:
                        CyrAdjective adj = null;

                        if (condition == GetConditionsEnum.Strict)
                        {
                            adj = this.adjCollection.Get(w, out g, out c, out n, out AnimatesEnum a);
                        }
                        else
                        {
                            adj = this.adjCollection.Get(w, out fw, out g, out c, out n, out AnimatesEnum a);
                        }

                        words.Add(adj);
                        break;
                    case SpeechPartsEnum.Noun:
                        CyrNoun noun = null;

                        if (condition == GetConditionsEnum.Strict)
                        {
                            noun = this.nounCollection.Get(w, out c, out n);
                        }
                        else
                        {
                            noun = this.nounCollection.Get(w, out fw, out c, out n);
                        }

                        words.Add(noun);
                        break;
                    default:
                        throw new NotImplementedException("This speech part is not supported yet. Speech part: " + speech.ToString());
                }
            }

            for (int i = 0; i < words.Count; i++)
            {
                CyrNoun noun = words[i] as CyrNoun;

                if (noun != null)
                {
                    if (number == NumbersEnum.Plural)
                    {
                        results.Add(noun.DeclinePlural());
                    }
                    else
                    {
                        results.Add(noun.Decline());
                    }
                    
                    continue;
                }

                CyrAdjective adj = words[i] as CyrAdjective;
                
                noun = this.GetNextPreviousNoun(words, i);

                if (number == NumbersEnum.Plural)
                {
                    if (noun != null)
                    {
                        results.Add(adj.DeclinePlural(noun.Animate));
                    }
                    else
                    {
                        results.Add(adj.DeclinePlural(AnimatesEnum.Animated));
                    }
                }
                else
                {
                    if (noun != null)
                    {
                        results.Add(adj.Decline(noun.Gender, noun.Animate));
                    }
                    else
                    {
                        results.Add(adj.Decline(GendersEnum.Masculine, AnimatesEnum.Animated));
                    }
                }
            }

            CyrResult result = results.First();

            for (int i = 1; i < results.Count; i++)
            {
                result = result + results[i];
            }

            return result;
        }

        protected CyrNoun GetNextPreviousNoun(List<object> words, int index)
        {
            CyrNoun noun = this.GetNextNoun(words, index);

            if (noun == null)
            {
                noun = this.GetPreviousNoun(words, index);
            }

            return noun;
        }

        protected CyrNoun GetNextNoun(List<object> words, int index)
        {
            for (int i = index + 1; i < words.Count; i++)
            {
                if (words[i] is CyrNoun)
                {
                    return words[i] as CyrNoun;
                }
            }

            return null;
        }

        protected CyrNoun GetPreviousNoun(List<object> words, int index)
        {
            for (int i = index - 1; i >= 0; i--)
            {
                if (words[i] is CyrNoun)
                {
                    return words[i] as CyrNoun;
                }
            }

            return null;
        }
    }
}
