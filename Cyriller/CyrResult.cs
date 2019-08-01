using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyriller.Model;

namespace Cyriller
{
    public class CyrResult
    {
        protected string case1;
        protected string case2;
        protected string case3;
        protected string case4;
        protected string case5;
        protected string case6;

        public CyrResult()
        {
        }

        public CyrResult(string word)
        {
            this.case1 = word;
            this.case2 = word;
            this.case3 = word;
            this.case4 = word;
            this.case5 = word;
            this.case6 = word;
        }

        public CyrResult(string case1, string case2, string case3, string case4, string case5, string case6)
        {
            this.case1 = case1;
            this.case2 = case2;
            this.case3 = case3;
            this.case4 = case4;
            this.case5 = case5;
            this.case6 = case6;
        }

        public CyrResult(string[] cases)
        {
            if (cases.Length != 6)
            {
                throw new ArgumentException("Declension result must have six parts.");
            }

            this.case1 = cases[0];
            this.case2 = cases[1];
            this.case3 = cases[2];
            this.case4 = cases[3];
            this.case5 = cases[4];
            this.case6 = cases[5];
        }

        public static CyrResult operator +(CyrResult a, CyrResult b)
        {
            return new CyrResult(a.case1 + " " + b.case1,
                a.case2 + " " + b.case2,
                a.case3 + " " + b.case3,
                a.case4 + " " + b.case4,
                a.case5 + " " + b.case5,
                a.case6 + " " + b.case6);
        }

        /// <summary>
        /// Именительный, Кто? Что? (есть)
        /// </summary>
        public string Nominative
        {
            get
            {
                return this.case1;
            }
        }

        /// <summary>
        /// Родительный, Кого? Чего? (нет)
        /// </summary>
        public string Genitive
        {
            get
            {
                return this.case2;
            }
        }

        /// <summary>
        /// Дательный, Кому? Чему? (дам)
        /// </summary>
        public string Dative
        {
            get 
            {
                return this.case3;
            }
        }

        /// <summary>
        /// Винительный, Кого? Что? (вижу)
        /// </summary>
        public string Accusative
        {
            get
            {
                return this.case4;
            }
        }

        /// <summary>
        /// Творительный, Кем? Чем? (горжусь)
        /// </summary>
        public string Instrumental
        {
            get 
            {
                return this.case5;
            }
        }

        /// <summary>
        /// Предложный, О ком? О чем? (думаю)
        /// </summary>
        public string Prepositional
        {
            get
            {
                return this.case6;
            }
        }

        /// <summary>
        /// Именительный, Кто? Что? (есть)
        /// </summary>
        public string Именительный
        {
            get
            {
                return this.case1;
            }
        }

        /// <summary>
        /// Родительный, Кого? Чего? (нет)
        /// </summary>
        public string Родительный
        {
            get
            {
                return this.case2;
            }
        }

        /// <summary>
        /// Дательный, Кому? Чему? (дам)
        /// </summary>
        public string Дательный
        {
            get
            {
                return this.case3;
            }
        }

        /// <summary>
        /// Винительный, Кого? Что? (вижу)
        /// </summary>
        public string Винительный
        {
            get
            {
                return this.case4;
            }
        }

        /// <summary>
        /// Творительный, Кем? Чем? (горжусь)
        /// </summary>
        public string Творительный
        {
            get
            {
                return this.case5;
            }
        }

        /// <summary>
        /// Предложный, О ком? О чем? (думаю)
        /// </summary>
        public string Предложный
        {
            get
            {
                return this.case6;
            }
        }

        public string Get(CasesEnum @case)
        {
            switch (@case)
            {
                case CasesEnum.Nominative: return case1;
                case CasesEnum.Genitive: return case2;
                case CasesEnum.Dative: return case3;
                case CasesEnum.Accusative: return case4;
                case CasesEnum.Instrumental: return case5;
                case CasesEnum.Prepositional: return case6;
                default: return case1;
            }
        }

        public void Set(CasesEnum @case, string value)
        {
            switch (@case)
            {
                case CasesEnum.Nominative: case1 = value; break;
                case CasesEnum.Genitive: case2 = value; break;
                case CasesEnum.Dative: case3 = value; break;
                case CasesEnum.Accusative: case4 = value; break;
                case CasesEnum.Instrumental: case5 = value; break;
                case CasesEnum.Prepositional: case6 = value; break;
                default: case1 = value; break;
            }
        }

        public void Add(CyrResult result, string separator = "-")
        {
            this.case1 += separator + result.case1;
            this.case2 += separator + result.case2;
            this.case3 += separator + result.case3;
            this.case4 += separator + result.case4;
            this.case5 += separator + result.case5;
            this.case6 += separator + result.case6;
        }

        public List<string> ToList()
        {
            return new List<string>() { case1, case2, case3, case4, case5, case6 };
        }

        public string[] ToArray()
        {
            return new string[] { case1, case2, case3, case4, case5, case6 };
        }

        public Dictionary<CasesEnum, string> ToDictionary()
        {
            CasesEnum[] caseNames = new CasesEnum[] { CasesEnum.Nominative, CasesEnum.Genitive, CasesEnum.Dative, CasesEnum.Accusative, CasesEnum.Instrumental, CasesEnum.Prepositional };
            Dictionary<CasesEnum, string> result = new Dictionary<CasesEnum, string>();

            result.Add(caseNames[0], case1);
            result.Add(caseNames[1], case2);
            result.Add(caseNames[2], case3);
            result.Add(caseNames[3], case4);
            result.Add(caseNames[4], case5);
            result.Add(caseNames[5], case6);

            return result;
        }

        public Dictionary<string, string> ToStringDictionary()
        {
            string[] caseNames = new string[] { "Nominative", "Genitive", "Dative", "Accusative", "Instrumental", "Prepositional" };
            Dictionary<string, string> result = new Dictionary<string, string>();

            result.Add(caseNames[0], case1);
            result.Add(caseNames[1], case2);
            result.Add(caseNames[2], case3);
            result.Add(caseNames[3], case4);
            result.Add(caseNames[4], case5);
            result.Add(caseNames[5], case6);

            return result;
        }

        public Dictionary<string, string> ToRussianStringDictionary()
        {
            string[] caseNames = new string[] { "Именительный", "Родительный", "Дательный", "Винительный", "Творительный", "Предложный" };
            Dictionary<string, string> result = new Dictionary<string, string>();

            result.Add(caseNames[0], case1);
            result.Add(caseNames[1], case2);
            result.Add(caseNames[2], case3);
            result.Add(caseNames[3], case4);
            result.Add(caseNames[4], case5);
            result.Add(caseNames[5], case6);

            return result;
        }

        /// <summary>
        /// Индекс для получения формы слова по номеру падежа. Доступные значения 1 – 6. 
        /// Смотри <see cref="CasesEnum"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index]
        {
            get
            {
                this.CheckIndex(index);
                return this.Get((CasesEnum)index);
            }
            set
            {
                this.CheckIndex(index);
                this.Set((CasesEnum)index, value);
            }
        }

        protected void CheckIndex(int index)
        {
            if (index == 0)
            {
                throw new IndexOutOfRangeException("This is a one based index!");
            }
        }
    }
}
