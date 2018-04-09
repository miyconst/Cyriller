using Cyriller.Model;

namespace Cyriller
{
    public partial class CyrNumber
    {
        protected class Strings
        {
            public string[] Hundreds { get; set; }
            public string[] Tens { get; set; }
            public string[] Numbers { get; set; }
            public string[] Thousand { get; set; }
            public string[] Million { get; set; }
            public string[] Billion { get; set; }
            public string Zero { get; set; }

            public string[] Integer { get; set; }
            public string[] DecimalTen { get; set; }
            public string[] DecimalHundred { get; set; }
            public string[] DecimalThousand { get; set; }
            public string[] DecimalMillion { get; set; }
            public string[] DecimalBillion { get; set; }

            public Strings() : this(CasesEnum.Nominative, GendersEnum.Masculine, AnimatesEnum.Inanimated) { }

            public Strings(CasesEnum Case, GendersEnum Gender, AnimatesEnum Animate)
            {
                switch (Case)
                {
                    case CasesEnum.Nominative:
                        Hundreds = new string[] { "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };
                        Tens = new string[] { "десять", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
                        Numbers = new string[] { "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять", "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
                        Thousand = new string[] { "тысяча", "тысячи", "тысяч" };
                        Million = new string[] { "миллион", "миллиона", "миллионов" };
                        Billion = new string[] { "миллиард", "миллиарда", "миллиардов" };
                        Zero = "ноль";

                        if (Gender == GendersEnum.Feminine)
                        {
                            Integer = new string[] { "целая", "целые", "целых" };
                            DecimalTen = new string[] { "десятая", "десятые", "десятых" };
                            DecimalHundred = new string[] { "сотая", "сотые", "сотых" };
                            DecimalThousand = new string[] { "тысячная", "тысячные", "тысячных" };
                            DecimalMillion = new string[] { "миллионная", "миллионные", "миллионных" };
                            DecimalBillion = new string[] { "миллиардная", "миллиардные", "миллиардных" };
                        }
                        else
                        {
                            Integer = new string[] { "целый", "целых", "целых" };
                            DecimalTen = new string[] { "десятый", "десятых", "десятых" };
                            DecimalHundred = new string[] { "сотый", "сотых", "сотых" };
                            DecimalThousand = new string[] { "тысячный", "тысячных", "тысячных" };
                            DecimalMillion = new string[] { "миллионный", "миллионных", "миллионных" };
                            DecimalBillion = new string[] { "миллиардный", "миллиардных", "миллиардных" };
                        }
                        break;
                    case CasesEnum.Genitive:
                        Hundreds = new string[] { "ста", "двухсот", "трехсот", "четырехсот", "пятисот", "шестисот", "семисот", "восьмисот", "девятисот" };
                        Tens = new string[] { "десяти", "двадцати", "тридцати", "сорока", "пятидесяти", "шестидесяти", "семидесяти", "весьмидесяти", "девяноста" };
                        Numbers = new string[] { "одного", "двух", "трех", "четырех", "пяти", "шести", "семи", "восьми", "девяти", "десяти", "одиннадцати", "двенадцати", "тринадцати", "четырнадцати", "пятнадцати", "шестнадцати", "семнадцати", "восемнадцати", "девятнадцати" };
                        Thousand = new string[] { "тысячи", "тысяч", "тысяч" };
                        Million = new string[] { "миллиона", "миллионов", "миллионов" };
                        Billion = new string[] { "миллиарда", "миллиардов", "миллиардов" };
                        Zero = "ноля";

                        if (Gender == GendersEnum.Feminine)
                        {
                            Integer = new string[] { "целой", "целыми", "целыми" };
                            DecimalTen = new string[] { "десятой", "десятых", "десятых" };
                            DecimalHundred = new string[] { "сотой", "сотых", "сотых" };
                            DecimalThousand = new string[] { "тысячной", "тысячных", "тысячных" };
                            DecimalMillion = new string[] { "миллионной", "миллионных", "миллионных" };
                            DecimalBillion = new string[] { "миллиардной", "миллиардных", "миллиардных" };
                        }
                        else
                        {
                            Integer = new string[] { "целого", "целых", "целых" };
                            DecimalTen = new string[] { "десятого", "десятых", "десятых" };
                            DecimalHundred = new string[] { "сотого", "сотых", "сотых" };
                            DecimalThousand = new string[] { "тысячного", "тысячных", "тысячных" };
                            DecimalMillion = new string[] { "миллионного", "миллионных", "миллионных" };
                            DecimalBillion = new string[] { "миллиардного", "миллиардных", "миллиардных" };
                        }
                        break;
                    case CasesEnum.Dative:
                        Hundreds = new string[] { "ста", "двумстам", "тремстам", "четыремстам", "пятистам", "шестистам", "семистам", "восьмистам", "девятистам" };
                        Tens = new string[] { "десяти", "двадцати", "тридцати", "сорока", "пятидесяти", "шестидесяти", "семидесяти", "весьмидесяти", "девяноста" };
                        Numbers = new string[] { "одному", "двум", "трем", "четырем", "пяти", "шести", "семи", "восьми", "девяти", "десяти", "одиннадцати", "двенадцати", "тринадцати", "четырнадцати", "пятнадцати", "шестнадцати", "семнадцати", "восемнадцати", "девятнадцати" };
                        Thousand = new string[] { "тысяче", "тысячам", "тысячам" };
                        Million = new string[] { "миллиону", "миллионам", "миллионам" };
                        Billion = new string[] { "миллиарду", "миллиардам", "миллиардам" };
                        Zero = "ноля";

                        if (Gender == GendersEnum.Feminine)
                        {
                            Integer = new string[] { "целой", "целым", "целым" };
                            DecimalTen = new string[] { "десятой", "десятым", "десятым" };
                            DecimalHundred = new string[] { "сотой", "сотым", "сотым" };
                            DecimalThousand = new string[] { "тысячной", "тысячным", "тысячным" };
                            DecimalMillion = new string[] { "миллионной", "миллионным", "миллионным" };
                            DecimalBillion = new string[] { "миллиардной", "миллиардным", "миллиардным" };
                        }
                        else
                        {
                            Integer = new string[] { "целому", "целым", "целым" };
                            DecimalTen = new string[] { "десятому", "десятым", "десятым" };
                            DecimalHundred = new string[] { "сотому", "сотым", "сотым" };
                            DecimalThousand = new string[] { "тысячному", "тысячным", "тысячным" };
                            DecimalMillion = new string[] { "миллионному", "миллионным", "миллионным" };
                            DecimalBillion = new string[] { "миллиардному", "миллиардным", "миллиардным" };
                        }
                        break;
                    case CasesEnum.Accusative:
                        Hundreds = new string[] { "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };
                        Tens = new string[] { "десять", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };

                        if (Animate == AnimatesEnum.Animated)
                        {
                            Numbers = new string[] { "одного", "двух", "трех", "четырех", "пять", "шести", "семи", "восьми", "девяти", "десяти", "одиннадцати", "двенадцати", "тринадцати", "четырнадцати", "пятнадцати", "шестнадцати", "семнадцати", "восемнадцати", "девятнадцати" };
                        }
                        else
                        {
                            Numbers = new string[] { "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять", "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
                        }

                        Thousand = new string[] { "тысячу", "тысячи", "тысяч" };
                        Million = new string[] { "миллион", "миллиона", "миллионов" };
                        Billion = new string[] { "миллиард", "миллиарда", "миллиардов" };
                        Zero = "ноль";

                        if (Gender == GendersEnum.Feminine)
                        {
                            Integer = new string[] { "целую", "целые", "целых" };
                            DecimalTen = new string[] { "десятую", "десятых", "десятых" };
                            DecimalHundred = new string[] { "сотую", "сотых", "сотых" };
                            DecimalThousand = new string[] { "тысячную", "тысячных", "тысячных" };
                            DecimalMillion = new string[] { "миллионную", "миллионныех", "миллионных" };
                            DecimalBillion = new string[] { "миллиардную", "миллиардных", "миллиардных" };
                        }
                        else
                        {
                            Integer = new string[] { "целого", "целых", "целых" };
                            DecimalTen = new string[] { "десятый", "десятых", "десятых" };
                            DecimalHundred = new string[] { "сотый", "сотых", "сотых" };
                            DecimalThousand = new string[] { "тысячный", "тысячных", "тысячных" };
                            DecimalMillion = new string[] { "миллионный", "миллионныех", "миллионных" };
                            DecimalBillion = new string[] { "миллиардный", "миллиардных", "миллиардных" };
                        }
                        break;
                    case CasesEnum.Instrumental:
                        Hundreds = new string[] { "ста", "двумястами", "тремястами", "четырьмястами", "пятьюстами", "шестьюстами", "семьюстами", "восьмьюстами", "девятьюстами" };
                        Tens = new string[] { "десятью", "двадцатью", "тридцатью", "сорока", "пятьюдесятью", "шестьюдесятью", "семьюдесятью", "восьмьюдесятью", "девяноста" };
                        Numbers = new string[] { "одним", "двумя", "тремя", "четырьмя", "пятью", "шестью", "семью", "восемью", "девятью", "десятью", "одиннадцатью", "двенадцатью", "тринадцатью", "четырнадцатью", "пятнадцатью", "шестнадцатью", "семнадцатью", "восемнадцатью", "девятнадцатью" };
                        Thousand = new string[] { "тысячей", "тысячами", "тысячами" };
                        Million = new string[] { "миллионом", "миллионами", "миллионами" };
                        Billion = new string[] { "миллиардом", "миллиардами", "миллиардами" };
                        Zero = "нолем";

                        if (Gender == GendersEnum.Feminine)
                        {
                            Integer = new string[] { "целой", "целыми", "целыми" };
                            DecimalTen = new string[] { "десятой", "десятыми", "десятыми" };
                            DecimalHundred = new string[] { "сотой", "сотыми", "сотыми" };
                            DecimalThousand = new string[] { "тысячной", "тысячными", "тысячными" };
                            DecimalMillion = new string[] { "миллионной", "миллионными", "миллионными" };
                            DecimalBillion = new string[] { "миллиардной", "миллиардными", "миллиардными" };
                        }
                        else
                        {
                            Integer = new string[] { "целым", "целыми", "целыми" };
                            DecimalTen = new string[] { "десятым", "десятыми", "десятыми" };
                            DecimalHundred = new string[] { "сотым", "сотыми", "сотыми" };
                            DecimalThousand = new string[] { "тысячным", "тысячными", "тысячными" };
                            DecimalMillion = new string[] { "миллионным", "миллионными", "миллионными" };
                            DecimalBillion = new string[] { "миллиардным", "миллиардными", "миллиардными" };
                        }
                        break;
                    case CasesEnum.Prepositional:
                        Hundreds = new string[] { "ста", "двухстах", "трехстах", "четырехстах", "пятистах", "шестистах", "семистах", "восьмистах", "девятистах" };
                        Tens = new string[] { "десяти", "двадцати", "тридцати", "сорока", "пятидесяти", "шестидесяти", "семидесяти", "восьмидесяти", "девяноста" };
                        Numbers = new string[] { "одном", "двух", "трех", "четырех", "пяти", "шести", "семи", "восеми", "девяти", "десяти", "одиннадцати", "двенадцати", "тринадцати", "четырнадцати", "пятнадцати", "шестнадцати", "семнадцати", "восемнадцати", "девятнадцати" };
                        Thousand = new string[] { "тысяче", "тысячах", "тысячах" };
                        Million = new string[] { "миллионе", "миллионах", "миллионах" };
                        Billion = new string[] { "миллиарде", "миллиардах", "миллиардах" };
                        Zero = "ноле";

                        if (Gender == GendersEnum.Feminine)
                        {
                            Integer = new string[] { "целой", "целых", "целых" };
                            DecimalTen = new string[] { "десятой", "десятых", "десятых" };
                            DecimalHundred = new string[] { "сотой", "сотых", "сотых" };
                            DecimalThousand = new string[] { "тысячной", "тысячных", "тысячных" };
                            DecimalMillion = new string[] { "миллионной", "миллионных", "миллионных" };
                            DecimalBillion = new string[] { "миллиардной", "миллиардных", "миллиардных" };
                        }
                        else
                        {
                            Integer = new string[] { "целом", "целых", "целых" };
                            DecimalTen = new string[] { "десятом", "десятых", "десятых" };
                            DecimalHundred = new string[] { "сотом", "сотых", "сотых" };
                            DecimalThousand = new string[] { "тысячном", "тысячных", "тысячных" };
                            DecimalMillion = new string[] { "миллионном", "миллионных", "миллионных" };
                            DecimalBillion = new string[] { "миллиардном", "миллиардных", "миллиардных" };
                        }
                        break;
                }

                if (Gender == GendersEnum.Feminine)
                {
                    switch (Case)
                    {
                        case CasesEnum.Nominative:
                            Numbers[0] = "одна";
                            Numbers[1] = "две";
                            break;
                        case CasesEnum.Genitive:
                            Numbers[0] = "одной";
                            Numbers[1] = "двух";
                            break;
                        case CasesEnum.Dative:
                            Numbers[0] = "одной";
                            Numbers[1] = "двум";
                            break;
                        case CasesEnum.Accusative:
                            Numbers[0] = "одну";

                            if (Animate == AnimatesEnum.Animated)
                            {
                                Numbers[1] = "двух";
                            }
                            else
                            {
                                Numbers[1] = "две";
                            }

                            break;
                        case CasesEnum.Instrumental:
                            Numbers[0] = "одной";
                            Numbers[1] = "двумя";
                            break;
                        case CasesEnum.Prepositional:
                            Numbers[0] = "одной";
                            Numbers[1] = "двух";
                            break;
                    }
                }
                else if (Gender == GendersEnum.Neuter)
                {
                    switch (Case)
                    {
                        case CasesEnum.Nominative:
                            Numbers[0] = "одно";
                            break;
                        case CasesEnum.Genitive:
                            Numbers[0] = "одного";
                            break;
                        case CasesEnum.Dative:
                            Numbers[0] = "одному";
                            break;
                        case CasesEnum.Accusative:
                            Numbers[0] = "одно";
                            break;
                        case CasesEnum.Instrumental:
                            Numbers[0] = "одним";
                            break;
                        case CasesEnum.Prepositional:
                            Numbers[0] = "одном";
                            break;
                    }
                }
            }
        }
    }
}
