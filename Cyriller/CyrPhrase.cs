using System;
using System.Collections.Generic;
using System.Linq;
using Cyriller.Model;

namespace Cyriller
{
    public class CyrPhrase
    {
        protected CyrNounCollection nounCollection;
        protected CyrAdjectiveCollection adjCollection;

        public CyrPhrase(CyrNounCollection NounCollection, CyrAdjectiveCollection AdjCollection)
        {
            nounCollection = NounCollection;
            adjCollection = AdjCollection;
        }

        public SpeechPartsEnum DetermineSpeechPart(string Word)
        {
            string[] ends = new string[] { "ый", "ий", "ой", "ся", "ая", "яя", "ое", "ее" };

            if (ends.Any(val => Word.EndsWith(val)))
            {
                return SpeechPartsEnum.Adjective;
            }

            return SpeechPartsEnum.Noun;
        }

        public CyrResult Decline(string Phrase, GetConditionsEnum Condition)
        {
            return Decline(Phrase, Condition, NumbersEnum.Singular);
        }

        public CyrResult DeclinePlural(string Phrase, GetConditionsEnum Condition)
        {
            return Decline(Phrase, Condition, NumbersEnum.Plural);
        }

        protected CyrResult Decline(string Phrase, GetConditionsEnum Condition, NumbersEnum Number)
        {
            if (Phrase.IsNullOrEmpty())
            {
                return new CyrResult();
            }

            List<object> words = new List<object>();
            string[] parts = Phrase.Split(' ').Select(val => val.Trim()).Where(val => val.IsNotNullOrEmpty()).ToArray();
            List<CyrResult> results = new List<CyrResult>();

            foreach (string w in parts)
            {
                SpeechPartsEnum speech = DetermineSpeechPart(w);

                switch (speech)
                {
                    case SpeechPartsEnum.Adjective:
                        CyrAdjective adj = adjCollection.Get(w, Condition);
                        words.Add(adj);
                        break;
                    case SpeechPartsEnum.Noun:
                        CyrNoun noun = nounCollection.Get(w, Condition);
                        words.Add(noun);
                        break;
                    default:
                        throw new ArgumentException("This speech part is not supported yet. Speech part: " + speech.ToString());
                }
            }

            for (int i = 0; i < words.Count; i++)
            {
                if (words[i] is CyrNoun noun)
                {
                    results.Add(Number == NumbersEnum.Plural ? noun.DeclinePlural() : noun.Decline());
                    continue;
                }

                CyrAdjective adj = words[i] as CyrAdjective;
                noun = GetNextPreviousNoun(words, i);
                AnimatesEnum animate = noun != null ? noun.Animate : AnimatesEnum.Animated;
                results.Add(Number == NumbersEnum.Plural ? adj.DeclinePlural(animate): adj.Decline(animate));
            }

            CyrResult result = results.First();

            for (int i = 1; i < results.Count; i++)
            {
                result = result + results[i];
            }

            return result;
        }

        protected CyrNoun GetNextPreviousNoun(List<object> Words, int Index)
        {
            CyrNoun noun = GetNextNoun(Words, Index);

            if (noun == null)
            {
                noun = GetPreviousNoun(Words, Index);
            }

            return noun;
        }

        protected CyrNoun GetNextNoun(List<object> Words, int Index)
        {
            for (int i = Index + 1; i < Words.Count; i++)
            {
                if (Words[i] is CyrNoun)
                {
                    return Words[i] as CyrNoun;
                }
            }

            return null;
        }

        protected CyrNoun GetPreviousNoun(List<object> Words, int Index)
        {
            for (int i = Index - 1; i >= 0; i--)
            {
                if (Words[i] is CyrNoun)
                {
                    return Words[i] as CyrNoun;
                }
            }

            return null;
        }
    }
}
