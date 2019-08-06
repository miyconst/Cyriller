using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyriller.Model;

namespace Cyriller
{
    public class CyrDeclineCase
    {
        public string NameRu { get; protected set; }
        public string NameEn { get; protected set; }
        public string Description { get; protected set; }
        public int Index { get; protected set; }
        public CasesEnum Value { get; protected set; }

        protected CyrDeclineCase()
        {
        }

        public CyrDeclineCase(string nameRu, string nameEn, string description, int index, CasesEnum value)
        {
            this.NameRu = nameRu;
            this.NameEn = nameEn;
            this.Description = description;
            this.Index = index;
            this.Value = value;
        }

        public static IEnumerable<CyrDeclineCase> GetEnumerable()
        {
            return new CyrDeclineCase[] { Case1, Case2, Case3, Case4, Case5, Case6 };
        }

        /// <summary>
        /// Именительный (Nominative), Кто? Что? (есть).
        /// </summary>
        public static CyrDeclineCase Case1
        {
            get
            {
                return new CyrDeclineCase()
                {
                    NameRu = "Именительный",
                    NameEn = "Nominative",
                    Description = "Кто? Что? (есть)",
                    Index = 1,
                    Value = CasesEnum.Nominative
                };
            }
        }

        /// <summary>
        /// Родительный (Genitive), Кого? Чего? (нет).
        /// </summary>
        public static CyrDeclineCase Case2
        {
            get
            {
                return new CyrDeclineCase()
                {
                    NameRu = "Родительный",
                    NameEn = "Genitive",
                    Description = "Кого? Чего? (нет)",
                    Index = 2,
                    Value = CasesEnum.Genitive
                };
            }
        }

        /// <summary>
        /// Дательный (Dative), Кому? Чему? (дам).
        /// </summary>
        public static CyrDeclineCase Case3
        {
            get
            {
                return new CyrDeclineCase()
                {
                    NameRu = "Дательный",
                    NameEn = "Dative",
                    Description = "Кому? Чему? (дам)",
                    Index = 3,
                    Value = CasesEnum.Dative
                };
            }
        }

        /// <summary>
        /// Винительный (Accusative), Кого? Что? (вижу).
        /// </summary>
        public static CyrDeclineCase Case4
        {
            get
            {
                return new CyrDeclineCase()
                {
                    NameRu = "Винительный",
                    NameEn = "Accusative",
                    Description = "Кого? Что? (вижу)",
                    Index = 4,
                    Value = CasesEnum.Accusative
                };
            }
        }

        /// <summary>
        /// Творительный (Instrumental), Кем? Чем? (горжусь).
        /// </summary>
        public static CyrDeclineCase Case5
        {
            get
            {
                return new CyrDeclineCase()
                {
                    NameRu = "Творительный",
                    NameEn = "Instrumental",
                    Description = "Кем? Чем? (горжусь)",
                    Index = 5,
                    Value = CasesEnum.Instrumental
                };
            }
        }

        /// <summary>
        /// Предложный (Prepositional), О ком? О чем? (думаю).
        /// </summary>
        public static CyrDeclineCase Case6
        {
            get
            {
                return new CyrDeclineCase()
                {
                    NameRu = "Предложный",
                    NameEn = "Prepositional",
                    Description = "О ком? О чем? (думаю)",
                    Index = 6,
                    Value = CasesEnum.Prepositional
                };
            }
        }

        /// <summary>
        /// Именительный (Nominative), Кто? Что? (есть).
        /// </summary>
        public static CyrDeclineCase Nominative
        {
            get
            {
                return Case1;
            }
        }

        /// <summary>
        /// Родительный (Genitive), Кого? Чего? (нет).
        /// </summary>
        public static CyrDeclineCase Genitive
        {
            get
            {
                return Case2;
            }
        }

        /// <summary>
        /// Дательный (Dative), Кому? Чему? (дам).
        /// </summary>
        public static CyrDeclineCase Dative
        {
            get
            {
                return Case3;
            }
        }

        /// <summary>
        /// Винительный (Accusative), Кого? Что? (вижу).
        /// </summary>
        public static CyrDeclineCase Accusative
        {
            get
            {
                return Case4;
            }
        }

        /// <summary>
        /// Творительный (Instrumental), Кем? Чем? (горжусь).
        /// </summary>
        public static CyrDeclineCase Instrumental
        {
            get
            {
                return Case5;
            }
        }

        /// <summary>
        /// Предложный (Prepositional), О ком? О чем? (думаю).
        /// </summary>
        public static CyrDeclineCase Prepositional
        {
            get
            {
                return Case6;
            }
        }

        /// <summary>
        /// Именительный (Nominative), Кто? Что? (есть).
        /// </summary>
        public static CyrDeclineCase Именительный
        {
            get
            {
                return Case1;
            }
        }

        /// <summary>
        /// Родительный (Genitive), Кого? Чего? (нет).
        /// </summary>
        public static CyrDeclineCase Родительный
        {
            get
            {
                return Case2;
            }
        }

        /// <summary>
        /// Дательный (Dative), Кому? Чему? (дам).
        /// </summary>
        public static CyrDeclineCase Дательный
        {
            get
            {
                return Case3;
            }
        }

        /// <summary>
        /// Винительный (Accusative), Кого? Что? (вижу).
        /// </summary>
        public static CyrDeclineCase Винительный
        {
            get
            {
                return Case4;
            }
        }

        /// <summary>
        /// Творительный (Instrumental), Кем? Чем? (горжусь).
        /// </summary>
        public static CyrDeclineCase Творительный
        {
            get
            {
                return Case5;
            }
        }

        /// <summary>
        /// Предложный (Prepositional), О ком? О чем? (думаю).
        /// </summary>
        public static CyrDeclineCase Предложный
        {
            get
            {
                return Case6;
            }
        }
    }
}
