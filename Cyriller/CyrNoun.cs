using System.Linq;
using System.Collections.Generic;
using Cyriller.Model;

namespace Cyriller
{
    public class CyrNoun
    {
        public const string Hyphen = "-";
        protected GendersEnum gender;
        protected AnimatesEnum animate;
        protected WordTypesEnum type;
        protected string name;
        protected string collectionName;
        protected CyrRule[] rules;
        protected int rulesOffset = 11;
        public bool IsAnimated => animate == AnimatesEnum.Animated;
        public string Name => name;
        public string CollectionName => collectionName;
        public bool ExactMatch => name == collectionName;
        public GendersEnum Gender => gender;
        public AnimatesEnum Animate => animate;
        public WordTypesEnum WordType => type;

        public CyrNoun(string Name, GendersEnum Gender, AnimatesEnum Animate, WordTypesEnum Type, CyrRule[] Rules) : this(Name, Name, Gender, Animate, Type, Rules) { }
        public CyrNoun(string Name, string CollectionName, GendersEnum Gender, AnimatesEnum Animate, WordTypesEnum Type, CyrRule[] Rules)
        {
            collectionName = CollectionName;
            name = Name;
            gender = Gender;
            animate = Animate;
            type = Type;
            rules = Rules;
        }

        public CyrResult Decline()
        {
            string[] parts = name.Split(Hyphen[0]);
            if (parts.Length == 1 || rules.Length <= 11)
            {
                return new CyrResult(
                    name,
                    rules[0].Apply(name),
                    rules[1].Apply(name),
                    rules[2].Apply(name),
                    rules[3].Apply(name),
                    rules[4].Apply(name)
                );
            }
            return DeclineMultipleParts(parts);
        }

        public CyrResult DeclinePlural()
        {
            string[] parts = name.Split(Hyphen[0]);
            if (parts.Length == 1 || rules.Length <= 11)
            {
                return new CyrResult(
                    rules[5].Apply(name),
                    rules[6].Apply(name),
                    rules[7].Apply(name),
                    rules[8].Apply(name),
                    rules[9].Apply(name),
                    rules[10].Apply(name)
                );
            }
            return DeclinePluralMultipleParts(parts);
        }

        protected CyrResult DeclineMultipleParts(string[] parts)
        {
            List<CyrResult> results = new List<CyrResult>();

            for (int i = 0; i < parts.Length; i++)
            {
                int offest = i * this.rulesOffset;

                CyrResult partResult = new CyrResult
                    (parts[i],
                    rules[0 + offest].Apply(parts[i]),
                    rules[1 + offest].Apply(parts[i]),
                    rules[2 + offest].Apply(parts[i]),
                    rules[3 + offest].Apply(parts[i]),
                    rules[4 + offest].Apply(parts[i])
                );

                results.Add(partResult);
            }

            return JoinResults(results);
        }

        protected CyrResult DeclinePluralMultipleParts(string[] parts)
        {
            List<CyrResult> results = new List<CyrResult>();

            for (int i = 0; i < parts.Length; i++)
            {
                int offest = i * rulesOffset;

                CyrResult partResult = new CyrResult(
                    rules[5 + offest].Apply(parts[i]),
                    rules[6 + offest].Apply(parts[i]),
                    rules[7 + offest].Apply(parts[i]),
                    rules[8 + offest].Apply(parts[i]),
                    rules[9 + offest].Apply(parts[i]),
                    rules[10 + offest].Apply(parts[i])
                );

                results.Add(partResult);
            }

            return JoinResults(results);
        }

        protected CyrResult JoinResults(IEnumerable<CyrResult> results)
        {
            return new CyrResult(
                string.Join(Hyphen, results.Select(x => x[(int)CasesEnum.Nominative]).Where(x => !string.IsNullOrEmpty(x)).ToArray()),
                string.Join(Hyphen, results.Select(x => x[(int)CasesEnum.Genitive]).Where(x => !string.IsNullOrEmpty(x)).ToArray()),
                string.Join(Hyphen, results.Select(x => x[(int)CasesEnum.Dative]).Where(x => !string.IsNullOrEmpty(x)).ToArray()),
                string.Join(Hyphen, results.Select(x => x[(int)CasesEnum.Accusative]).Where(x => !string.IsNullOrEmpty(x)).ToArray()),
                string.Join(Hyphen, results.Select(x => x[(int)CasesEnum.Instrumental]).Where(x => !string.IsNullOrEmpty(x)).ToArray()),
                string.Join(Hyphen, results.Select(x => x[(int)CasesEnum.Prepositional]).Where(x => !string.IsNullOrEmpty(x)).ToArray()));
        }
    }
}
