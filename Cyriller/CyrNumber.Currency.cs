using System;
using Cyriller.Model;

namespace Cyriller
{
    public partial class CyrNumber
    {
        public abstract class Currency
        {
            public GendersEnum IntegerGender { get; set; }
            public GendersEnum DecimalGender { get; set; }
            public int Decimals { get; set; }

            public abstract string[] GetIntegerName(CasesEnum Case);
            public abstract string[] GetDecimalName(CasesEnum Case);
        }

        public class RurCurrency : Currency
        {
            public RurCurrency()
            {
                IntegerGender = GendersEnum.Masculine;
                DecimalGender = GendersEnum.Feminine;
                Decimals = 2;
            }

            public override string[] GetIntegerName(CasesEnum Case)
            {
                switch (Case)
                {
                    case CasesEnum.Nominative:
                        return new string[] { "рубль", "рубля", "рублей" };
                    case CasesEnum.Genitive:
                        return new string[] { "рубля", "рублей", "рублей" };
                    case CasesEnum.Dative:
                        return new string[] { "рублю", "рублям", "рублям" };
                    case CasesEnum.Accusative:
                        return new string[] { "рубль", "рубля", "рублей" };
                    case CasesEnum.Instrumental:
                        return new string[] { "рублем", "рублями", "рублями" };
                    case CasesEnum.Prepositional:
                        return new string[] { "рубле", "рублях", "рублях" };
                }

                throw new ArgumentOutOfRangeException("Invalid decline case!");
            }

            public override string[] GetDecimalName(CasesEnum Case)
            {
                switch (Case)
                {
                    case CasesEnum.Nominative:
                        return new string[] { "копейка", "копейки", "копеек" };
                    case CasesEnum.Genitive:
                        return new string[] { "копейки", "копеек", "копеек" };
                    case CasesEnum.Dative:
                        return new string[] { "копейке", "копейкам", "копейкам" };
                    case CasesEnum.Accusative:
                        return new string[] { "копейку", "копейки", "копеек" };
                    case CasesEnum.Instrumental:
                        return new string[] { "копейкой", "копейками", "копейками" };
                    case CasesEnum.Prepositional:
                        return new string[] { "копейке", "копейках", "копейках" };
                }

                throw new ArgumentOutOfRangeException("Invalid decline case!");
            }
        }

        public class UsdCurrency : Currency
        {
            public UsdCurrency()
            {
                IntegerGender = GendersEnum.Masculine;
                DecimalGender = GendersEnum.Masculine;
                Decimals = 2;
            }

            public override string[] GetIntegerName(CasesEnum Case)
            {
                switch (Case)
                {
                    case CasesEnum.Nominative:
                        return new string[] { "доллар", "доллара", "долларов" };
                    case CasesEnum.Genitive:
                        return new string[] { "доллара", "долларов", "долларов" };
                    case CasesEnum.Dative:
                        return new string[] { "доллару", "долларам", "долларам" };
                    case CasesEnum.Accusative:
                        return new string[] { "доллар", "доллара", "долларов" };
                    case CasesEnum.Instrumental:
                        return new string[] { "долларов", "долларами", "долларами" };
                    case CasesEnum.Prepositional:
                        return new string[] { "долларе", "долларах", "долларах" };
                }

                throw new ArgumentOutOfRangeException("Invalid decline case!");
            }

            public override string[] GetDecimalName(CasesEnum Case)
            {
                switch (Case)
                {
                    case CasesEnum.Nominative:
                        return new string[] { "цент", "цента", "центов" };
                    case CasesEnum.Genitive:
                        return new string[] { "цента", "центов", "центов" };
                    case CasesEnum.Dative:
                        return new string[] { "центу", "центам", "центам" };
                    case CasesEnum.Accusative:
                        return new string[] { "цент", "цента", "центов" };
                    case CasesEnum.Instrumental:
                        return new string[] { "центом", "центами", "центами" };
                    case CasesEnum.Prepositional:
                        return new string[] { "центе", "центах", "центах" };
                }

                throw new ArgumentOutOfRangeException("Invalid decline case!");
            }
        }

        public class EurCurrency : Currency
        {
            public EurCurrency()
            {
                IntegerGender = GendersEnum.Neuter;
                DecimalGender = GendersEnum.Masculine;
                Decimals = 2;
            }

            public override string[] GetIntegerName(CasesEnum Case)
            {
                switch (Case)
                {
                    case CasesEnum.Nominative:
                        return new string[] { "евро", "евро", "евро" };
                    case CasesEnum.Genitive:
                        return new string[] { "евро", "евро", "евро" };
                    case CasesEnum.Dative:
                        return new string[] { "евро", "евро", "евро" };
                    case CasesEnum.Accusative:
                        return new string[] { "евро", "евро", "евро" };
                    case CasesEnum.Instrumental:
                        return new string[] { "евро", "евро", "евро" };
                    case CasesEnum.Prepositional:
                        return new string[] { "евро", "евро", "евро" };
                }

                throw new ArgumentOutOfRangeException("Invalid decline case!");
            }

            public override string[] GetDecimalName(CasesEnum Case)
            {
                switch (Case)
                {
                    case CasesEnum.Nominative:
                        return new string[] { "цент", "цента", "центов" };
                    case CasesEnum.Genitive:
                        return new string[] { "цента", "центов", "центов" };
                    case CasesEnum.Dative:
                        return new string[] { "центу", "центам", "центам" };
                    case CasesEnum.Accusative:
                        return new string[] { "цент", "цента", "центов" };
                    case CasesEnum.Instrumental:
                        return new string[] { "центом", "центами", "центами" };
                    case CasesEnum.Prepositional:
                        return new string[] { "центе", "центах", "центах" };
                }

                throw new ArgumentOutOfRangeException("Invalid decline case!");
            }
        }

        public class YuanCurrency : Currency
        {
            public YuanCurrency()
            {
                IntegerGender = GendersEnum.Masculine;
                DecimalGender = GendersEnum.Masculine;
                Decimals = 1;
            }

            public override string[] GetIntegerName(CasesEnum Case)
            {
                switch (Case)
                {
                    case CasesEnum.Nominative:
                        return new string[] { "юань", "юаня", "юаней" };
                    case CasesEnum.Genitive:
                        return new string[] { "юаня", "юаней", "юаней" };
                    case CasesEnum.Dative:
                        return new string[] { "юаню", "юаням", "юаням" };
                    case CasesEnum.Accusative:
                        return new string[] { "юань", "юаня", "юаней" };
                    case CasesEnum.Instrumental:
                        return new string[] { "юанем", "юанями", "юанями" };
                    case CasesEnum.Prepositional:
                        return new string[] { "юане", "юанях", "юанях" };
                }

                throw new ArgumentOutOfRangeException("Invalid decline case!");
            }

            public override string[] GetDecimalName(CasesEnum Case)
            {
                switch (Case)
                {
                    case CasesEnum.Nominative:
                        return new string[] { "цзяо", "цзяо", "цзяо" };
                    case CasesEnum.Genitive:
                        return new string[] { "цзяо", "цзяо", "цзяо" };
                    case CasesEnum.Dative:
                        return new string[] { "цзяо", "цзяо", "цзяо" };
                    case CasesEnum.Accusative:
                        return new string[] { "цзяо", "цзяо", "цзяо" };
                    case CasesEnum.Instrumental:
                        return new string[] { "цзяо", "цзяо", "цзяо" };
                    case CasesEnum.Prepositional:
                        return new string[] { "цзяо", "цзяо", "цзяо" };
                }

                throw new ArgumentOutOfRangeException("Invalid decline case!");
            }
        }
    }
}
