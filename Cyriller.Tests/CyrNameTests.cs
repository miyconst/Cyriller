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
    }
}
