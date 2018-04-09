using System;
using System.Collections.Generic;
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

        public CyrResult() { }

        public CyrResult(string Word)
        {
            case1 = Word;
            case2 = Word;
            case3 = Word;
            case4 = Word;
            case5 = Word;
            case6 = Word;
        }

        public CyrResult(string Case1, string Case2, string Case3, string Case4, string Case5, string Case6)
        {
            case1 = Case1;
            case2 = Case2;
            case3 = Case3;
            case4 = Case4;
            case5 = Case5;
            case6 = Case6;
        }

        public static CyrResult operator +(CyrResult A, CyrResult B)
        {
            return new CyrResult(
                A.case1 + " " + B.case1,
                A.case2 + " " + B.case2,
                A.case3 + " " + B.case3,
                A.case4 + " " + B.case4,
                A.case5 + " " + B.case5,
                A.case6 + " " + B.case6
            );
        }

        /// <summary>
        /// Именительный, Кто? Что? (есть)
        /// </summary>
        public string Nominative => case1;

        /// <summary>
        /// Родительный, Кого? Чего? (нет)
        /// </summary>
        public string Genitive => case2;

        /// <summary>
        /// Дательный, Кому? Чему? (дам)
        /// </summary>
        public string Dative => case3;

        /// <summary>
        /// Винительный, Кого? Что? (вижу)
        /// </summary>
        public string Accusative => case4;

        /// <summary>
        /// Творительный, Кем? Чем? (горжусь)
        /// </summary>
        public string Instrumental => case5;

        /// <summary>
        /// Предложный, О ком? О чем? (думаю)
        /// </summary>
        public string Prepositional => case6;

        /// <summary>
        /// Именительный, Кто? Что? (есть)
        /// </summary>
        public string Именительный => case1;

        /// <summary>
        /// Родительный, Кого? Чего? (нет)
        /// </summary>
        public string Родительный => case2;

        /// <summary>
        /// Дательный, Кому? Чему? (дам)
        /// </summary>
        public string Дательный => case3;

        /// <summary>
        /// Винительный, Кого? Что? (вижу)
        /// </summary>
        public string Винительный => case4;

        /// <summary>
        /// Творительный, Кем? Чем? (горжусь)
        /// </summary>
        public string Творительный => case5;

        /// <summary>
        /// Предложный, О ком? О чем? (думаю)
        /// </summary>
        public string Предложный => case6;

        public string Get(CasesEnum Case)
        {
            switch (Case)
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

        public void Set(CasesEnum Case, string Value)
        {
            switch (Case)
            {
                case CasesEnum.Nominative: case1 = Value; break;
                case CasesEnum.Genitive: case2 = Value; break;
                case CasesEnum.Dative: case3 = Value; break;
                case CasesEnum.Accusative: case4 = Value; break;
                case CasesEnum.Instrumental: case5 = Value; break;
                case CasesEnum.Prepositional: case6 = Value; break;
                default: case1 = Value; break;
            }
        }

        public void Add(CyrResult Result, string Separator = "-")
        {
            case1 += Separator + Result.case1;
            case2 += Separator + Result.case2;
            case3 += Separator + Result.case3;
            case4 += Separator + Result.case4;
            case5 += Separator + Result.case5;
            case6 += Separator + Result.case6;
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
            return new Dictionary<CasesEnum, string> {
                { CasesEnum.Nominative, case1 },
                { CasesEnum.Genitive, case2 },
                { CasesEnum.Dative, case3 },
                { CasesEnum.Accusative, case4 },
                { CasesEnum.Instrumental, case5 },
                { CasesEnum.Prepositional, case6 },
            };
        }

        public Dictionary<string, string> ToStringDictionary()
        {
            return new Dictionary<string, string> {
                { CasesEnum.Nominative.ToString(), case1 },
                { CasesEnum.Genitive.ToString(), case2 },
                { CasesEnum.Dative.ToString(), case3 },
                { CasesEnum.Accusative.ToString(), case4 },
                { CasesEnum.Instrumental.ToString(), case5 },
                { CasesEnum.Prepositional.ToString(), case6 },
            };
        }

        public Dictionary<string, string> ToRussianStringDictionary()
        {
            return new Dictionary<string, string> {
                { "Именительный", case1 },
                { "Родительный", case2 },
                { "Дательный", case3 },
                { "Винительный", case4 },
                { "Творительный", case5 },
                { "Предложный", case6 },
            };
        }

        /// <summary>
        /// One based index. See the CasesEnum enumeration.
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public string this[int Index]
        {
            get
            {
                CheckIndex(Index);
                return Get((CasesEnum)Index);
            }
            set
            {
                CheckIndex(Index);
                Set((CasesEnum)Index, value);
            }
        }

        protected void CheckIndex(int Index)
        {
            if (Index == 0)
            {
                throw new IndexOutOfRangeException("This is a one based index!");
            }
        }
    }
}
