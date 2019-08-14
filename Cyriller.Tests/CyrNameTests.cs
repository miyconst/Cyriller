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
        #region Name declension.
        public CyrName CyrName { get; protected set; } = new CyrName();

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
        #endregion

        #region Patronymic declension.
        [Fact]
        public void MasculinePatronymicIsCorrectlyDeclinedInAccusativeCase()
        {
            string result = this.CyrName.DeclinePatronymicAccusative("иванович", null, false, false);
            Assert.Equal("ивановича", result);

            result = this.CyrName.DeclinePatronymicAccusative("салим", "оглы", false, false);
            Assert.Equal("салим", result);
        }

        [Fact]
        public void MasculinePatronymicIsCorrectlyDeclinedInDativeCase()
        {
            string result = this.CyrName.DeclinePatronymicDative("иванович", null, false, false);
            Assert.Equal("ивановичу", result);

            result = this.CyrName.DeclinePatronymicAccusative("салим", "оглы", false, false);
            Assert.Equal("салим", result);
        }

        [Fact]
        public void MasculinePatronymicIsCorrectlyDeclinedInGenitiveCase()
        {
            string result = this.CyrName.DeclinePatronymicGenitive("иванович", null, false, false);
            Assert.Equal("ивановича", result);

            result = this.CyrName.DeclinePatronymicAccusative("салим", "оглы", false, false);
            Assert.Equal("салим", result);
        }

        [Fact]
        public void MasculinePatronymicIsCorrectlyDeclinedInInstrumentalCase()
        {
            string result = this.CyrName.DeclinePatronymicInstrumental("иванович", null, false, false);
            Assert.Equal("ивановичем", result);

            result = this.CyrName.DeclinePatronymicAccusative("салим", "оглы", false, false);
            Assert.Equal("салим", result);
        }

        [Fact]
        public void MasculinePatronymicIsCorrectlyDeclinedInPrepositionalCase()
        {
            string result = this.CyrName.DeclinePatronymicPrepositional("иванович", null, false, false);
            Assert.Equal("ивановиче", result);

            result = this.CyrName.DeclinePatronymicAccusative("салим", "оглы", false, false);
            Assert.Equal("салим", result);
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
