namespace Cyriller
{
    public class CyrDeclineCase
    {
        public string NameRu { get; protected set; }
        public string NameEn { get; protected set; }
        public string Description { get; protected set; }
        public int Index { get; protected set; }

        public static CyrDeclineCase[] List => new CyrDeclineCase[] { Case1, Case2, Case3, Case4, Case5, Case6 };


        public static CyrDeclineCase Case1 => new CyrDeclineCase()
        {
            NameRu = "Именительный",
            NameEn = "Nominative",
            Description = "Кто? Что? (есть)",
            Index = 1
        };

        public static CyrDeclineCase Case2 => new CyrDeclineCase()
        {
            NameRu = "Родительный",
            NameEn = "Genitive",
            Description = "Кого? Чего? (нет)",
            Index = 2
        };

        public static CyrDeclineCase Case3 => new CyrDeclineCase()
        {
            NameRu = "Дательный",
            NameEn = "Dative",
            Description = "Кому? Чему? (дам)",
            Index = 3
        };

        public static CyrDeclineCase Case4 => new CyrDeclineCase()
        {
            NameRu = "Винительный",
            NameEn = "Accusative",
            Description = "Кого? Что? (вижу)",
            Index = 4
        };

        public static CyrDeclineCase Case5 => new CyrDeclineCase()
        {
            NameRu = "Творительный",
            NameEn = "Instrumental",
            Description = "Кем? Чем? (горжусь)",
            Index = 5
        };

        public static CyrDeclineCase Case6 => new CyrDeclineCase()
        {
            NameRu = "Предложный",
            NameEn = "Prepositional",
            Description = "О ком? О чем? (думаю)",
            Index = 6
        };

        /// <summary>
        /// Именительный, Кто? Что? (есть)
        /// </summary>
        public CyrDeclineCase Nominative => Case1;

        /// <summary>
        /// Родительный, Кого? Чего? (нет)
        /// </summary>
        public CyrDeclineCase Genitive => Case2;

        /// <summary>
        /// Дательный, Кому? Чему? (дам)
        /// </summary>
        public CyrDeclineCase Dative => Case3;

        /// <summary>
        /// Винительный, Кого? Что? (вижу)
        /// </summary>
        public CyrDeclineCase Accusative => Case4;

        /// <summary>
        /// Творительный, Кем? Чем? (горжусь)
        /// </summary>
        public CyrDeclineCase Instrumental => Case5;

        /// <summary>
        /// Предложный, О ком? О чем? (думаю)
        /// </summary>
        public CyrDeclineCase Prepositional => Case6;

        /// <summary>
        /// Именительный, Кто? Что? (есть)
        /// </summary>
        public CyrDeclineCase Именительный => Case1;

        /// <summary>
        /// Родительный, Кого? Чего? (нет)
        /// </summary>
        public CyrDeclineCase Родительный => Case2;

        /// <summary>
        /// Дательный, Кому? Чему? (дам)
        /// </summary>
        public CyrDeclineCase Дательный => Case3;

        /// <summary>
        /// Винительный, Кого? Что? (вижу)
        /// </summary>
        public CyrDeclineCase Винительный => Case4;

        /// <summary>
        /// Творительный, Кем? Чем? (горжусь)
        /// </summary>
        public CyrDeclineCase Творительный => Case5;

        /// <summary>
        /// Предложный, О ком? О чем? (думаю)
        /// </summary>
        public CyrDeclineCase Предложный => Case6;
    }
}
