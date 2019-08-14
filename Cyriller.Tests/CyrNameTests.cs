using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cyriller.Tests
{
    public class CyrNameTests
    {
        public CyrName CyrName { get; protected set; } = new CyrName();

        #region Full name declension.
        [Fact]
        public void FeminineFullNameIsCorrectlyDeclined()
        {
            {
                CyrResult result = this.CyrName.Decline("Иванова Наталья Петровна", Cyriller.Model.GendersEnum.Feminine, false);

                Assert.Equal("Ивановой Натальи Петровны", result.Get(Cyriller.Model.CasesEnum.Genitive));
                Assert.Equal("Ивановой Наталье Петровне", result.Get(Cyriller.Model.CasesEnum.Dative));
                Assert.Equal("Иванову Наталью Петровну", result.Get(Cyriller.Model.CasesEnum.Accusative));
                Assert.Equal("Ивановой Натальей Петровной", result.Get(Cyriller.Model.CasesEnum.Instrumental));
                Assert.Equal("Ивановой Наталье Петровне", result.Get(Cyriller.Model.CasesEnum.Prepositional));
            }

            {
                CyrResult result = this.CyrName.Decline("Сафаралиева Койкеб Кямил Кызы", Cyriller.Model.GendersEnum.Feminine, false);

                Assert.Equal("Сафаралиевой Койкеб Кямил Кызы", result.Get(Cyriller.Model.CasesEnum.Genitive));
                Assert.Equal("Сафаралиевой Койкеб Кямил Кызы", result.Get(Cyriller.Model.CasesEnum.Dative));
                Assert.Equal("Сафаралиеву Койкеб Кямил Кызы", result.Get(Cyriller.Model.CasesEnum.Accusative));
                Assert.Equal("Сафаралиевой Койкеб Кямил Кызы", result.Get(Cyriller.Model.CasesEnum.Instrumental));
                Assert.Equal("Сафаралиевой Койкеб Кямил Кызы", result.Get(Cyriller.Model.CasesEnum.Prepositional));
            }

            {
                CyrResult result = this.CyrName.Decline("Сафаралиева Койкеб Кямил-Кызы", Cyriller.Model.GendersEnum.Feminine, false);

                Assert.Equal("Сафаралиевой Койкеб Кямил-Кызы", result.Get(Cyriller.Model.CasesEnum.Genitive));
                Assert.Equal("Сафаралиевой Койкеб Кямил-Кызы", result.Get(Cyriller.Model.CasesEnum.Dative));
                Assert.Equal("Сафаралиеву Койкеб Кямил-Кызы", result.Get(Cyriller.Model.CasesEnum.Accusative));
                Assert.Equal("Сафаралиевой Койкеб Кямил-Кызы", result.Get(Cyriller.Model.CasesEnum.Instrumental));
                Assert.Equal("Сафаралиевой Койкеб Кямил-Кызы", result.Get(Cyriller.Model.CasesEnum.Prepositional));
            }

            {
                CyrResult result = this.CyrName.Decline("Иванова Наталья Петровна", Cyriller.Model.GendersEnum.Feminine, true);

                Assert.Equal("Ивановой Н. П.", result.Get(Cyriller.Model.CasesEnum.Genitive));
                Assert.Equal("Ивановой Н. П.", result.Get(Cyriller.Model.CasesEnum.Dative));
                Assert.Equal("Иванову Н. П.", result.Get(Cyriller.Model.CasesEnum.Accusative));
                Assert.Equal("Ивановой Н. П.", result.Get(Cyriller.Model.CasesEnum.Instrumental));
                Assert.Equal("Ивановой Н. П.", result.Get(Cyriller.Model.CasesEnum.Prepositional));
            }

            {
                CyrResult result = this.CyrName.Decline("Сафаралиева Койкеб Кямил Кызы", Cyriller.Model.GendersEnum.Feminine, true);

                Assert.Equal("Сафаралиевой К. К.", result.Get(Cyriller.Model.CasesEnum.Genitive));
                Assert.Equal("Сафаралиевой К. К.", result.Get(Cyriller.Model.CasesEnum.Dative));
                Assert.Equal("Сафаралиеву К. К.", result.Get(Cyriller.Model.CasesEnum.Accusative));
                Assert.Equal("Сафаралиевой К. К.", result.Get(Cyriller.Model.CasesEnum.Instrumental));
                Assert.Equal("Сафаралиевой К. К.", result.Get(Cyriller.Model.CasesEnum.Prepositional));
            }
        }

        [Fact]
        public void MasculineFullNameIsCorrectlyDeclined()
        {
            {
                CyrResult result = this.CyrName.Decline("Иванов Иван Иванович", Cyriller.Model.GendersEnum.Masculine, false);

                Assert.Equal("Иванова Ивана Ивановича", result.Get(Cyriller.Model.CasesEnum.Genitive));
                Assert.Equal("Иванову Ивану Ивановичу", result.Get(Cyriller.Model.CasesEnum.Dative));
                Assert.Equal("Иванова Ивана Ивановича", result.Get(Cyriller.Model.CasesEnum.Accusative));
                Assert.Equal("Ивановым Иваном Ивановичем", result.Get(Cyriller.Model.CasesEnum.Instrumental));
                Assert.Equal("Иванове Иване Ивановиче", result.Get(Cyriller.Model.CasesEnum.Prepositional));
            }

            {
                CyrResult result = this.CyrName.Decline("Карим Куржов Салим Оглы", Cyriller.Model.GendersEnum.Masculine, false);

                Assert.Equal("Карима Куржова Салим Оглы", result.Get(Cyriller.Model.CasesEnum.Genitive));
                Assert.Equal("Кариму Куржову Салим Оглы", result.Get(Cyriller.Model.CasesEnum.Dative));
                Assert.Equal("Карима Куржова Салим Оглы", result.Get(Cyriller.Model.CasesEnum.Accusative));
                Assert.Equal("Каримом Куржовом Салим Оглы", result.Get(Cyriller.Model.CasesEnum.Instrumental));
                Assert.Equal("Кариме Куржове Салим Оглы", result.Get(Cyriller.Model.CasesEnum.Prepositional));
            }

            {
                CyrResult result = this.CyrName.Decline("Карим Куржов Салим-Оглы", Cyriller.Model.GendersEnum.Masculine, false);

                Assert.Equal("Карима Куржова Салим-Оглы", result.Get(Cyriller.Model.CasesEnum.Genitive));
                Assert.Equal("Кариму Куржову Салим-Оглы", result.Get(Cyriller.Model.CasesEnum.Dative));
                Assert.Equal("Карима Куржова Салим-Оглы", result.Get(Cyriller.Model.CasesEnum.Accusative));
                Assert.Equal("Каримом Куржовом Салим-Оглы", result.Get(Cyriller.Model.CasesEnum.Instrumental));
                Assert.Equal("Кариме Куржове Салим-Оглы", result.Get(Cyriller.Model.CasesEnum.Prepositional));
            }

            {
                CyrResult result = this.CyrName.Decline("Иванов Иван Иванович", Cyriller.Model.GendersEnum.Masculine, true);

                Assert.Equal("Иванова И. И.", result.Get(Cyriller.Model.CasesEnum.Genitive));
                Assert.Equal("Иванову И. И.", result.Get(Cyriller.Model.CasesEnum.Dative));
                Assert.Equal("Иванова И. И.", result.Get(Cyriller.Model.CasesEnum.Accusative));
                Assert.Equal("Ивановым И. И.", result.Get(Cyriller.Model.CasesEnum.Instrumental));
                Assert.Equal("Иванове И. И.", result.Get(Cyriller.Model.CasesEnum.Prepositional));
            }

            {
                CyrResult result = this.CyrName.Decline("Карим Куржов Салим Оглы", Cyriller.Model.GendersEnum.Masculine, true);

                Assert.Equal("Карима К. С.", result.Get(Cyriller.Model.CasesEnum.Genitive));
                Assert.Equal("Кариму К. С.", result.Get(Cyriller.Model.CasesEnum.Dative));
                Assert.Equal("Карима К. С.", result.Get(Cyriller.Model.CasesEnum.Accusative));
                Assert.Equal("Каримом К. С.", result.Get(Cyriller.Model.CasesEnum.Instrumental));
                Assert.Equal("Кариме К. С.", result.Get(Cyriller.Model.CasesEnum.Prepositional));
            }

            {
                CyrResult result = this.CyrName.Decline("Илон МакФерсон", Cyriller.Model.GendersEnum.Masculine, false);

                Assert.Equal("Илона МакФерсона", result.Get(Cyriller.Model.CasesEnum.Genitive));
                Assert.Equal("Илону МакФерсону", result.Get(Cyriller.Model.CasesEnum.Dative));
                Assert.Equal("Илона МакФерсона", result.Get(Cyriller.Model.CasesEnum.Accusative));
                Assert.Equal("Илоном МакФерсоном", result.Get(Cyriller.Model.CasesEnum.Instrumental));
                Assert.Equal("Илоне МакФерсоне", result.Get(Cyriller.Model.CasesEnum.Prepositional));
            }

            {
                CyrResult result = this.CyrName.Decline("Ахмед Гафуров ибн Мухаммад", Cyriller.Model.GendersEnum.Masculine, false);

                Assert.Equal("Ахмеда Гафурова ибн Мухаммада", result.Get(Cyriller.Model.CasesEnum.Genitive));
                Assert.Equal("Ахмеду Гафурову ибн Мухаммаду", result.Get(Cyriller.Model.CasesEnum.Dative));
                Assert.Equal("Ахмеда Гафурова ибн Мухаммада", result.Get(Cyriller.Model.CasesEnum.Accusative));
                Assert.Equal("Ахмедом Гафуровом ибн Мухаммадом", result.Get(Cyriller.Model.CasesEnum.Instrumental));
                Assert.Equal("Ахмеде Гафурове ибн Мухаммаде", result.Get(Cyriller.Model.CasesEnum.Prepositional));
            }
        }
        #endregion

        #region Name declension.
        [Fact]
        public void MasculineNameIsCorrectlyDeclinedInAccusativeCase()
        {
            string result = this.CyrName.DeclineNameAccusative("иван", false, false);
            Assert.Equal("ивана", result);
        }

        [Fact]
        public void MasculineNameIsCorrectlyDeclinedInDativeCase()
        {
            string result = this.CyrName.DeclineNameDative("иван", false, false);
            Assert.Equal("ивану", result);
        }

        [Fact]
        public void MasculineNameIsCorrectlyDeclinedInGenitiveCase()
        {
            string result = this.CyrName.DeclineNameGenitive("иван", false, false);
            Assert.Equal("ивана", result);
        }

        [Fact]
        public void MasculineNameIsCorrectlyDeclinedInInstrumentalCase()
        {
            string result = this.CyrName.DeclineNameInstrumental("иван", false, false);
            Assert.Equal("иваном", result);
        }

        [Fact]
        public void MasculineNameIsCorrectlyDeclinedInPrepositionalCase()
        {
            string result = this.CyrName.DeclineNamePrepositional("иван", false, false);
            Assert.Equal("иване", result);
        }

        [Fact]
        public void FeminineNameIsCorrectlyDeclinedInAccusativeCase()
        {
            string result = this.CyrName.DeclineNameAccusative("наталья", true, false);
            Assert.Equal("наталью", result);
        }

        [Fact]
        public void FeminineNameIsCorrectlyDeclinedInDativeCase()
        {
            string result = this.CyrName.DeclineNameDative("наталья", true, false);
            Assert.Equal("наталье", result);
        }

        [Fact]
        public void FeminineNameIsCorrectlyDeclinedInGenitiveCase()
        {
            string result = this.CyrName.DeclineNameGenitive("наталья", true, false);
            Assert.Equal("натальи", result);
        }

        [Fact]
        public void FeminineNameIsCorrectlyDeclinedInInstrumentalCase()
        {
            string result = this.CyrName.DeclineNameInstrumental("наталья", true, false);
            Assert.Equal("натальей", result);
        }

        [Fact]
        public void FeminineNameIsCorrectlyDeclinedInPrepositionalCase()
        {
            string result = this.CyrName.DeclineNamePrepositional("наталья", true, false);
            Assert.Equal("наталье", result);
        }
        #endregion

        #region Surname declension.
        [Fact]
        public void MasculineSurnameIsCorrectlyDeclinedInAccusativeCase()
        {
            string result = this.CyrName.DeclineSurnameAccusative("иванов", false);
            Assert.Equal("иванова", result);
        }

        [Fact]
        public void MasculineSurnameIsCorrectlyDeclinedInDativeCase()
        {
            string result = this.CyrName.DeclineSurnameDative("иванов", false);
            Assert.Equal("иванову", result);
        }

        [Fact]
        public void MasculineSurnameIsCorrectlyDeclinedInGenitiveCase()
        {
            string result = this.CyrName.DeclineSurnameGenitive("иванов", false);
            Assert.Equal("иванова", result);
        }

        [Fact]
        public void MasculineSurnameIsCorrectlyDeclinedInInstrumentalCase()
        {
            string result = this.CyrName.DeclineSurnameInstrumental("иванов", false);
            Assert.Equal("ивановым", result);
        }

        [Fact]
        public void MasculineSurnameIsCorrectlyDeclinedInPrepositionalCase()
        {
            string result = this.CyrName.DeclineSurnamePrepositional("иванов", false);
            Assert.Equal("иванове", result);
        }

        [Fact]
        public void FeminineSurnameIsCorrectlyDeclinedInAccusativeCase()
        {
            string result = this.CyrName.DeclineSurnameAccusative("петрова", true);
            Assert.Equal("петрову", result);
        }

        [Fact]
        public void FeminineSurnameIsCorrectlyDeclinedInDativeCase()
        {
            string result = this.CyrName.DeclineSurnameDative("петрова", true);
            Assert.Equal("петровой", result);
        }

        [Fact]
        public void FeminineSurnameIsCorrectlyDeclinedInGenitiveCase()
        {
            string result = this.CyrName.DeclineSurnameGenitive("петрова", true);
            Assert.Equal("петровой", result);
        }

        [Fact]
        public void FeminineSurnameIsCorrectlyDeclinedInInstrumentalCase()
        {
            string result = this.CyrName.DeclineSurnameInstrumental("петрова", true);
            Assert.Equal("петровой", result);
        }

        [Fact]
        public void FeminineSurnameIsCorrectlyDeclinedInPrepositionalCase()
        {
            string result = this.CyrName.DeclineSurnamePrepositional("петрова", true);
            Assert.Equal("петровой", result);
        }
        #endregion

        #region Patronymic declension.
        [Fact]
        public void MasculinePatronymicIsCorrectlyDeclinedInAccusativeCase()
        {
            string result = this.CyrName.DeclinePatronymicAccusative("иванович", false, false);
            Assert.Equal("ивановича", result);
        }

        [Fact]
        public void MasculinePatronymicIsCorrectlyDeclinedInDativeCase()
        {
            string result = this.CyrName.DeclinePatronymicDative("иванович", false, false);
            Assert.Equal("ивановичу", result);
        }

        [Fact]
        public void MasculinePatronymicIsCorrectlyDeclinedInGenitiveCase()
        {
            string result = this.CyrName.DeclinePatronymicGenitive("иванович", false, false);
            Assert.Equal("ивановича", result);
        }

        [Fact]
        public void MasculinePatronymicIsCorrectlyDeclinedInInstrumentalCase()
        {
            string result = this.CyrName.DeclinePatronymicInstrumental("иванович", false, false);
            Assert.Equal("ивановичем", result);
        }

        [Fact]
        public void MasculinePatronymicIsCorrectlyDeclinedInPrepositionalCase()
        {
            string result = this.CyrName.DeclinePatronymicPrepositional("иванович", false, false);
            Assert.Equal("ивановиче", result);
        }

        [Fact]
        public void FemininePatronymicIsCorrectlyDeclinedInAccusativeCase()
        {
            string result = this.CyrName.DeclinePatronymicAccusative("ивановна", true, false);
            Assert.Equal("ивановну", result);
        }

        [Fact]
        public void FemininePatronymicIsCorrectlyDeclinedInDativeCase()
        {
            string result = this.CyrName.DeclinePatronymicDative("ивановна", true, false);
            Assert.Equal("ивановне", result);
        }

        [Fact]
        public void FemininePatronymicIsCorrectlyDeclinedInGenitiveCase()
        {
            string result = this.CyrName.DeclinePatronymicGenitive("ивановна", true, false);
            Assert.Equal("ивановны", result);
        }

        [Fact]
        public void FemininePatronymicIsCorrectlyDeclinedInInstrumentalCase()
        {
            string result = this.CyrName.DeclinePatronymicInstrumental("ивановна", true, false);
            Assert.Equal("ивановной", result);
        }

        [Fact]
        public void FemininePatronymicIsCorrectlyDeclinedInPrepositionalCase()
        {
            string result = this.CyrName.DeclinePatronymicPrepositional("ивановна", true, false);
            Assert.Equal("ивановне", result);
        }
        #endregion

        #region Patronymic parts.
        [Fact]
        public void PatronymicEmptySplitIntoPartsCorrectly()
        {
            string prefix;
            string patronymic;
            string suffix;

            {
                this.CyrName.SplitPatronymic(string.Empty, out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal(string.Empty, patronymic);
                Assert.Null(suffix);
            }

            {
                this.CyrName.SplitPatronymic(null, out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Null(patronymic);
                Assert.Null(suffix);
            }

            {
                this.CyrName.SplitPatronymic(" ", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal(" ", patronymic);
                Assert.Null(suffix);
            }
        }

        [Fact]
        public void PatronymicIbnSplitIntoPartsCorrectly()
        {
            string prefix;
            string patronymic;
            string suffix;

            {
                this.CyrName.SplitPatronymic("Ибн Салим", out prefix, out patronymic, out suffix);

                Assert.Equal("Ибн ", prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Null(suffix);
            }

            {
                this.CyrName.SplitPatronymic("Ибн-Салим", out prefix, out patronymic, out suffix);

                Assert.Equal("Ибн-", prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Null(suffix);
            }

            {
                this.CyrName.SplitPatronymic("Ибн-салим", out prefix, out patronymic, out suffix);

                Assert.Equal("Ибн-", prefix);
                Assert.Equal("салим", patronymic);
                Assert.Null(suffix);
            }
        }


        [Fact]
        public void PatronymicOglySplitIntoPartsCorrectly()
        {
            string prefix;
            string patronymic;
            string suffix;

            {
                this.CyrName.SplitPatronymic("Салим Оглы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal(" Оглы", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим оглы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal(" оглы", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим-Оглы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal("-Оглы", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим-оглы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal("-оглы", suffix);
            }
        }

        [Fact]
        public void PatronymicUlySplitIntoPartsCorrectly()
        {
            string prefix;
            string patronymic;
            string suffix;

            {
                this.CyrName.SplitPatronymic("Салим Улы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal(" Улы", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим улы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal(" улы", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим-Улы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal("-Улы", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим-улы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal("-улы", suffix);
            }
        }

        [Fact]
        public void PatronymicUulуSplitIntoPartsCorrectly()
        {
            string prefix;
            string patronymic;
            string suffix;

            {
                this.CyrName.SplitPatronymic("Салим Уулу", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal(" Уулу", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим уулу", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal(" уулу", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим-Уулу", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal("-Уулу", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим-уулу", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal("-уулу", suffix);
            }
        }

        [Fact]
        public void PatronymicKyzySplitIntoPartsCorrectly()
        {
            string prefix;
            string patronymic;
            string suffix;

            {
                this.CyrName.SplitPatronymic("Салим Кызы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal(" Кызы", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим кызы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal(" кызы", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим-Кызы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal("-Кызы", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим-кызы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal("-кызы", suffix);
            }
        }

        [Fact]
        public void PatronymicGyzySplitIntoPartsCorrectly()
        {
            string prefix;
            string patronymic;
            string suffix;

            {
                this.CyrName.SplitPatronymic("Салим Гызы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal(" Гызы", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим гызы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal(" гызы", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим-Гызы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal("-Гызы", suffix);
            }

            {
                this.CyrName.SplitPatronymic("Салим-гызы", out prefix, out patronymic, out suffix);

                Assert.Null(prefix);
                Assert.Equal("Салим", patronymic);
                Assert.Equal("-гызы", suffix);
            }
        }
        #endregion
    }
}
