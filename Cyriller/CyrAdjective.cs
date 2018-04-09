using Cyriller.Model;

namespace Cyriller
{
    public class CyrAdjective
    {
        /// <summary>Род прилагательного</summary>
        protected GendersEnum gender;
        protected string name;
        protected string collectionName;
        protected CyrRule[] rules;
        public GendersEnum Gender => gender;
        public string Name => name;
        public string CollectionName => collectionName;
        public bool ExactMatch => name == collectionName;

        /// <summary></summary>
        /// <param name="Name">Прилагательное мужского рода в именительном падеже</param>
        /// <param name="CollectionName">Слово найденное в коллекции</param>
        /// <param name="Gender">Пол для склонения</param>
        /// <param name="Rules">Правила изменения окончания при склонении прилагательных</param>
        public CyrAdjective(string Name, string CollectionName, GendersEnum Gender, CyrRule[] Rules)
        {
            name = Name;
            collectionName = CollectionName;
            gender = Gender;
            rules = Rules;
        }

        public CyrResult Decline(AnimatesEnum Animate)
        {
            switch (gender)
            {
                case GendersEnum.Feminine:
                    return new CyrResult(
                        rules[5].Apply(name),
                        rules[6].Apply(name),
                        rules[7].Apply(name),
                        rules[8].Apply(name),
                        rules[9].Apply(name),
                        rules[10].Apply(name)
                    );
                case GendersEnum.Neuter:
                    return new CyrResult(
                        rules[11].Apply(name),
                        rules[12].Apply(name),
                        rules[13].Apply(name),
                        rules[14].Apply(name),
                        rules[15].Apply(name),
                        rules[16].Apply(name)
                    );
                default:
                    return new CyrResult(
                        name,
                        rules[0].Apply(name),
                        rules[1].Apply(name),
                        Animate == AnimatesEnum.Animated ? rules[2].Apply(name) : name,
                        rules[3].Apply(name),
                        rules[4].Apply(name)
                    );
            }
        }

        public CyrResult DeclinePlural(AnimatesEnum Animate)
        {
            return new CyrResult(
                rules[17].Apply(name),
                rules[18].Apply(name),
                rules[19].Apply(name),
                Animate == AnimatesEnum.Animated ? rules[21].Apply(name) : rules[17].Apply(name),
                rules[20].Apply(name),
                rules[21].Apply(name)
            );
        }
    }
}
