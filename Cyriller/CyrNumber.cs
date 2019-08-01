using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyriller.Model;

namespace Cyriller
{
    public partial class CyrNumber
    {
        /// <summary>
        /// Максимально большое число для написания прописью.
        /// </summary>
        public const long MaxValue = 9999999999;

        /// <summary>
        /// Склоняет число прописью в мужском роде <see cref="GendersEnum.Masculine"/> для неодушевленных предметов <see cref="AnimatesEnum.Inanimated"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CyrResult Decline(long value)
        {
            return this.Decline(value, GendersEnum.Masculine, AnimatesEnum.Inanimated);
        }

        /// <summary>
        /// Склоняет число прописью в указанном роде и одушевленности.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="gender"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        public CyrResult Decline(long value, GendersEnum gender, AnimatesEnum animate)
        {
            CyrResult result = new CyrResult(
                this.ToString(value, CasesEnum.Nominative, gender, animate),
                this.ToString(value, CasesEnum.Genitive, gender, animate),
                this.ToString(value, CasesEnum.Dative, gender, animate),
                this.ToString(value, CasesEnum.Accusative, gender, animate),
                this.ToString(value, CasesEnum.Instrumental, gender, animate),
                this.ToString(value, CasesEnum.Prepositional, gender, animate)
            );

            return result;
        }

        /// <summary>
        /// Склоняет число прописью в мужском роде <see cref="GendersEnum.Masculine"/> для неодушевленных предметов <see cref="AnimatesEnum.Inanimated"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CyrResult Decline(decimal value)
        {
            return this.Decline(value, GendersEnum.Masculine, AnimatesEnum.Inanimated);
        }

        /// <summary>
        /// Склоняет число прописью в указанном роде и одушевленности.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="gender"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        public CyrResult Decline(decimal value, GendersEnum gender, AnimatesEnum animate)
        {
            CyrResult result = new CyrResult(
                this.ToString(value, CasesEnum.Nominative, gender, animate),
                this.ToString(value, CasesEnum.Genitive, gender, animate),
                this.ToString(value, CasesEnum.Dative, gender, animate),
                this.ToString(value, CasesEnum.Accusative, gender, animate),
                this.ToString(value, CasesEnum.Instrumental, gender, animate),
                this.ToString(value, CasesEnum.Prepositional, gender, animate)
            );

            return result;
        }

        /// <summary>
        /// Склоняет денежную сумму в указанной валюте.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public CyrResult Decline(decimal value, Currency currency)
        {
            CyrResult result = new CyrResult(
                this.ToString(value, CasesEnum.Nominative, currency),
                this.ToString(value, CasesEnum.Genitive, currency),
                this.ToString(value, CasesEnum.Dative, currency),
                this.ToString(value, CasesEnum.Accusative, currency),
                this.ToString(value, CasesEnum.Instrumental, currency),
                this.ToString(value, CasesEnum.Prepositional, currency)
            );

            return result;
        }

        /// <summary>
        /// Склоняет количество указанных единиц.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public CyrResult Decline(decimal value, Item item)
        {
            CyrResult result = new CyrResult(
                this.ToString(value, CasesEnum.Nominative, item),
                this.ToString(value, CasesEnum.Genitive, item),
                this.ToString(value, CasesEnum.Dative, item),
                this.ToString(value, CasesEnum.Accusative, item),
                this.ToString(value, CasesEnum.Instrumental, item),
                this.ToString(value, CasesEnum.Prepositional, item)
            );

            return result;
        }

        /// <summary>
        /// Склоняет число прописью в указнный падеж мужского рода <see cref="GendersEnum.Masculine"/> для неодушевленных предметов <see cref="AnimatesEnum.Inanimated"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="case"></param>
        /// <returns></returns>
        public string ToString(long value, CasesEnum @case)
        {
            return this.ToString(value, @case, GendersEnum.Masculine, AnimatesEnum.Inanimated);
        }

        /// <summary>
        /// Склоняет число прописью в указнный падеж мужского рода <see cref="GendersEnum.Masculine"/> для неодушевленных предметов <see cref="AnimatesEnum.Inanimated"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="case"></param>
        /// <returns></returns>
        public string ToString(decimal value, CasesEnum @case)
        {
            return this.ToString(value, @case, GendersEnum.Masculine, AnimatesEnum.Inanimated);
        }

        /// <summary>
        /// Склоняет число прописью в указнный падеж, род и одушевленность.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="case"></param>
        /// <param name="gender"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        public string ToString(decimal value, CasesEnum @case, GendersEnum gender, AnimatesEnum animate)
        {
            string str = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string[] parts = str.Split('.');
            long i = long.Parse(parts[0]);
            StringBuilder sb = new StringBuilder();
            Strings s = new Strings(@case, gender, animate);

            sb.Append(this.ToString(i, @case, gender, animate));

            if (parts.Length > 1)
            {
                parts[1] = parts[1].TrimEnd('0');
            }

            if (parts.Length > 1 && !string.IsNullOrEmpty(parts[1]))
            {
                string[] decimals;
                long d = long.Parse(parts[1]);
                long dr = long.Parse(string.Join(string.Empty, parts[1].Reverse().ToArray()));

                if (dr < 10)
                {
                    decimals = s.DecimalTen;
                }
                else if (dr < 100)
                {
                    decimals = s.DecimalHundred;
                }
                else if (dr < 1000)
                {
                    decimals = s.DecimalThousand;
                }
                else if (dr < 1000000)
                {
                    decimals = s.DecimalMillion;
                }
                else
                {
                    decimals = s.DecimalBillion;
                }

                sb.Append(" ");
                sb.Append(this.Case(i, s.Integer[0], s.Integer[1], s.Integer[2]));
                sb.Append(" и ");
                sb.Append(this.ToString(d, @case, gender, animate));
                sb.Append(" ");
                sb.Append(this.Case(d, decimals[0], decimals[1], decimals[2]));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Склоняет денежную сумму в указанный падеж и валюту.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="case"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public string ToString(decimal value, CasesEnum @case, Currency currency)
        {
            string str = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string[] parts = str.Split('.');
            long i = long.Parse(parts[0]);
            StringBuilder sb = new StringBuilder();
            Strings s = new Strings(@case, currency.IntegerGender, AnimatesEnum.Inanimated);
            string[] iname = currency.GetIntegerName(@case);
            string[] dname = currency.GetDecimalName(@case);

            sb.Append(this.ToString(i, @case, currency.IntegerGender, AnimatesEnum.Inanimated));
            sb.Append(" ").Append(this.Case(i, iname[0], iname[1], iname[2]));

            if (parts.Length > 1)
            {
                parts[1] = parts[1].TrimEnd('0');
            }

            if (parts.Length > 1 && !string.IsNullOrEmpty(parts[1]))
            {
                string v = parts[1];

                while (v.Length < currency.Decimals)
                {
                    v += "0";
                }

                if (v.Length > currency.Decimals)
                {
                    List<char> chars = v.ToCharArray().ToList();

                    chars.Insert(currency.Decimals, '.');
                    v = string.Join(string.Empty, chars.ToArray());
                }

                long d = (long)Math.Round(decimal.Parse(v, System.Globalization.CultureInfo.InvariantCulture.NumberFormat), 0);

                sb.Append(" и ");
                sb.Append(this.ToString(d, @case, currency.DecimalGender, AnimatesEnum.Inanimated));
                sb.Append(" ");
                sb.Append(this.Case(d, dname[0], dname[1], dname[2]));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Склоняет количество указанных единиц в указанный падеж.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="case"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public string ToString(decimal value, CasesEnum @case, Item item)
        {
            long i = (long)value;
            StringBuilder sb = new StringBuilder();
            //Strings s = new Strings(Case, Item.Gender, Item.Animate);
            GendersEnum gender = i < value ? GendersEnum.Feminine : item.Gender;
            AnimatesEnum animate = i == value && i < 20 ? item.Animate : AnimatesEnum.Inanimated;
            string[] name;

            sb.Append(this.ToString(value, @case, gender, animate)).Append(" ");

            if (i < value)
            {
                name = item.GetName(CasesEnum.Nominative, i);
                sb.Append(name[1]);
            }
            else
            {
                name = item.GetName(@case, i);
                sb.Append(this.Case(i, name[0], name[1], name[2]));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Склоняет число прописью в указнный падеж, род и одушевленность.
        /// Выбрасывает <see cref="ArgumentOutOfRangeException"/>, если значение больше максимально допустимого <see cref="MaxValue"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="case"></param>
        /// <param name="gender"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        public string ToString(long value, CasesEnum @case, GendersEnum gender, AnimatesEnum animate)
        {
            if (value > MaxValue)
            {
                throw new ArgumentOutOfRangeException($"The maximum value is {MaxValue}.");
            }

            Strings s = new Strings(@case, gender, animate);

            if (value == 0)
            {
                return s.Zero;
            }

            StringBuilder r = new StringBuilder();
            long v;

            if (value < 0)
            {
                r.Append("минус ");
                value = Math.Abs(value);
            }

            v = value / 1000000000;

            if (v > 0)
            {
                r.Append(this.ToString(v, @case, GendersEnum.Masculine, animate)).Append(" ").Append(this.Case(v, s.Billion[0], s.Billion[1], s.Billion[2])).Append(" ");
                value = value - 1000000000 * v;
            }

            v = value / 1000000;

            if (v > 0)
            {
                r.Append(this.ToString(v, @case, GendersEnum.Masculine, animate)).Append(" ").Append(this.Case(v, s.Million[0], s.Million[1], s.Million[2])).Append(" ");
                value = value - 1000000 * v;
            }

            v = value / 1000;

            if (v > 0)
            {
                r.Append(this.ToString(v, @case, GendersEnum.Feminine, animate)).Append(" ").Append(this.Case(v, s.Thousand[0], s.Thousand[1], s.Thousand[2])).Append(" ");
                value = value - 1000 * v;
            }

            v = value / 100;

            if (v > 0)
            {
                r.Append(s.Hundreds[v - 1]).Append(" ");
                value = value - 100 * v;
            }

            if (value >= 20 || value == 10)
            {
                v = value / 10;
                r.Append(s.Tens[v - 1]).Append(" ");
                value = value - v * 10;
            }

            if (value > 0)
            {
                r.Append(s.Numbers[value - 1]);
            }

            return r.ToString().Trim(' ');
        }

        /// <summary>
        /// Выбирает правильный вариант слова в зависимости от указанного числа.
        /// 1 - one (один кот)
        /// 2..4 - two (два кота, три кота, четыре кота)
        /// >4 - five (пять котов, шесть котов ...)
        /// </summary>
        /// <param name="value">Число для выбора правильного варианта слова.</param>
        /// <param name="one">Вариант слова для употребления с числительным один.</param>
        /// <param name="two">Вариант слова для употребления с числительными два, три, четыре.</param>
        /// <param name="five">Вариант слова для употребления с числительными больше четырех.</param>
        /// <returns></returns>
        public string Case(long value, string one, string two, string five)
        {
            long t = (value % 100 > 20) ? value % 10 : value % 20;

            switch (t)
            {
                case 1:
                    return one;
                case 2:
                case 3:
                case 4:
                    return two;
                default:
                    return five;
            }
        }
    }
}
