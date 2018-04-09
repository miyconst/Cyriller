using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyriller.Model;

namespace Cyriller
{
    public partial class CyrNumber
    {
        public CyrResult Decline(long Value)
        {
            return Decline(Value, GendersEnum.Masculine, AnimatesEnum.Inanimated);
        }

        public CyrResult Decline(long Value, GendersEnum Gender, AnimatesEnum Animate)
        {
            return new CyrResult(
                ToString(Value, CasesEnum.Nominative, Gender, Animate),
                ToString(Value, CasesEnum.Genitive, Gender, Animate),
                ToString(Value, CasesEnum.Dative, Gender, Animate),
                ToString(Value, CasesEnum.Accusative, Gender, Animate),
                ToString(Value, CasesEnum.Instrumental, Gender, Animate),
                ToString(Value, CasesEnum.Prepositional, Gender, Animate)
            );
        }

        public CyrResult Decline(decimal Value)
        {
            return Decline(Value, GendersEnum.Masculine, AnimatesEnum.Inanimated);
        }

        public CyrResult Decline(decimal Value, GendersEnum Gender, AnimatesEnum Animate)
        {
            CyrResult result = new CyrResult(
                ToString(Value, CasesEnum.Nominative, Gender, Animate),
                ToString(Value, CasesEnum.Genitive, Gender, Animate),
                ToString(Value, CasesEnum.Dative, Gender, Animate),
                ToString(Value, CasesEnum.Accusative, Gender, Animate),
                ToString(Value, CasesEnum.Instrumental, Gender, Animate),
                ToString(Value, CasesEnum.Prepositional, Gender, Animate)
            );

            return result;
        }

        public CyrResult Decline(decimal Value, Currency Currency)
        {
            CyrResult result = new CyrResult(
                ToString(Value, CasesEnum.Nominative, Currency),
                ToString(Value, CasesEnum.Genitive, Currency),
                ToString(Value, CasesEnum.Dative, Currency),
                ToString(Value, CasesEnum.Accusative, Currency),
                ToString(Value, CasesEnum.Instrumental, Currency),
                ToString(Value, CasesEnum.Prepositional, Currency)
            );

            return result;
        }

        public CyrResult Decline(decimal Value, Item Item)
        {
            CyrResult result = new CyrResult(
                ToString(Value, CasesEnum.Nominative, Item),
                ToString(Value, CasesEnum.Genitive, Item),
                ToString(Value, CasesEnum.Dative, Item),
                ToString(Value, CasesEnum.Accusative, Item),
                ToString(Value, CasesEnum.Instrumental, Item),
                ToString(Value, CasesEnum.Prepositional, Item)
            );

            return result;
        }

        public string ToString(long Value, CasesEnum Case)
        {
            return ToString(Value, Case, GendersEnum.Masculine, AnimatesEnum.Inanimated);
        }

        public string ToString(decimal Value, CasesEnum Case)
        {
            return ToString(Value, Case, GendersEnum.Masculine, AnimatesEnum.Inanimated);
        }

        public string ToString(decimal Value, CasesEnum Case, GendersEnum Gender, AnimatesEnum Animate)
        {
            string str = Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string[] parts = str.Split('.');
            long i = long.Parse(parts[0]);
            StringBuilder sb = new StringBuilder();
            Strings s = new Strings(Case, Gender, Animate);

            sb.Append(ToString(i, Case, Gender, Animate));

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
                sb.Append(ToString(d, Case, Gender, Animate));
                sb.Append(" ");
                sb.Append(this.Case(d, decimals[0], decimals[1], decimals[2]));
            }

            return sb.ToString();
        }

        public string ToString(decimal Value, CasesEnum Case, Currency Currency)
        {
            string str = Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string[] parts = str.Split('.');
            long i = long.Parse(parts[0]);
            StringBuilder sb = new StringBuilder();
            Strings s = new Strings(Case, Currency.IntegerGender, AnimatesEnum.Inanimated);
            string[] iname = Currency.GetIntegerName(Case);
            string[] dname = Currency.GetDecimalName(Case);

            sb.Append(ToString(i, Case, Currency.IntegerGender, AnimatesEnum.Inanimated));
            sb.Append(" ").Append(this.Case(i, iname[0], iname[1], iname[2]));

            if (parts.Length > 1)
            {
                parts[1] = parts[1].TrimEnd('0');
            }

            if (parts.Length > 1 && !string.IsNullOrEmpty(parts[1]))
            {
                string v = parts[1];

                while (v.Length < Currency.Decimals)
                {
                    v += "0";
                }

                if (v.Length > Currency.Decimals)
                {
                    List<char> chars = v.ToCharArray().ToList();

                    chars.Insert(Currency.Decimals, '.');
                    v = string.Join(string.Empty, chars.ToArray());
                }

                long d = (long)Math.Round(decimal.Parse(v, System.Globalization.CultureInfo.InvariantCulture.NumberFormat), 0);

                sb.Append(" и ");
                sb.Append(ToString(d, Case, Currency.DecimalGender, AnimatesEnum.Inanimated));
                sb.Append(" ");
                sb.Append(this.Case(d, dname[0], dname[1], dname[2]));
            }

            return sb.ToString();
        }

        public string ToString(decimal Value, CasesEnum Case, Item Item)
        {
            long i = (long)Value;
            StringBuilder sb = new StringBuilder();
            //Strings s = new Strings(Case, Item.Gender, Item.Animate);
            GendersEnum gender = i < Value ? GendersEnum.Feminine : Item.Gender;
            AnimatesEnum animate = i == Value && i < 20 ? Item.Animate : AnimatesEnum.Inanimated;
            string[] name;

            sb.Append(ToString(Value, Case, gender, animate)).Append(" ");

            if (i < Value)
            {
                name = Item.GetName(CasesEnum.Nominative, i);
                sb.Append(name[1]);
            }
            else
            {
                name = Item.GetName(Case, i);
                sb.Append(this.Case(i, name[0], name[1], name[2]));
            }

            return sb.ToString();
        }

        public string ToString(long Value, CasesEnum Case, GendersEnum Gender, AnimatesEnum Animate)
        {
            if (Value > 9999999999)
            {
                throw new ArgumentOutOfRangeException("The maximum value is 9999999999.");
            }

            Strings s = new Strings(Case, Gender, Animate);

            if (Value == 0)
            {
                return s.Zero;
            }

            StringBuilder r = new StringBuilder();
            long v;

            if (Value < 0)
            {
                r.Append("минус ");
                Value = Math.Abs(Value);
            }

            v = Value / 1000000000;

            if (v > 0)
            {
                r.Append(ToString(v, Case, GendersEnum.Masculine, Animate)).Append(" ").Append(this.Case(v, s.Billion[0], s.Billion[1], s.Billion[2])).Append(" ");
                Value = Value - 1000000000 * v;
            }

            v = Value / 1000000;

            if (v > 0)
            {
                r.Append(ToString(v, Case, GendersEnum.Masculine, Animate)).Append(" ").Append(this.Case(v, s.Million[0], s.Million[1], s.Million[2])).Append(" ");
                Value = Value - 1000000 * v;
            }

            v = Value / 1000;

            if (v > 0)
            {
                r.Append(ToString(v, Case, GendersEnum.Feminine, Animate)).Append(" ").Append(this.Case(v, s.Thousand[0], s.Thousand[1], s.Thousand[2])).Append(" ");
                Value = Value - 1000 * v;
            }

            v = Value / 100;

            if (v > 0)
            {
                r.Append(s.Hundreds[v - 1]).Append(" ");
                Value = Value - 100 * v;
            }

            if (Value >= 20 || Value == 10)
            {
                v = Value / 10;
                r.Append(s.Tens[v - 1]).Append(" ");
                Value = Value - v * 10;
            }

            if (Value > 0)
            {
                r.Append(s.Numbers[Value - 1]);
            }

            return r.ToString().Trim(' ');
        }

        public string Case(long Value, string One, string Two, string Five)
        {
            long t = (Value % 100 > 20) ? Value % 10 : Value % 20;

            switch (t)
            {
                case 1:
                    return One;
                case 2:
                case 3:
                case 4:
                    return Two;
                default:
                    return Five;
            }
        }
    }
}
