using System;

namespace Cyriller
{
    internal class CyrNounCollectionRow
    {
        public int GenderID { get; set; }
        public int AnimateID { get; set; }
        public int TypeID { get; set; }
        public int RuleID { get; set; }

        public static CyrNounCollectionRow Parse(string Value)
        {
            string[] parts = Value.Split(',');

            if (parts.Length != 4)
                throw new ArgumentException($"{Value} is not a valid representation of {nameof(CyrNounCollectionRow)}!");

            return new CyrNounCollectionRow
            {
                GenderID = int.Parse(parts[0]),
                AnimateID = int.Parse(parts[1]),
                TypeID = int.Parse(parts[2]),
                RuleID = int.Parse(parts[3]),
            };
        }
        public static CyrNounCollectionRow Parse(int[] details)
        {
            if (details.Length != 4)
                throw new ArgumentException($"details is not a valid representation of {nameof(CyrNounCollectionRow)}!");

            return new CyrNounCollectionRow
            {
                GenderID = details[0],
                AnimateID = details[1],
                TypeID = details[2],
                RuleID = details[3],
            };
        }
    }
}
