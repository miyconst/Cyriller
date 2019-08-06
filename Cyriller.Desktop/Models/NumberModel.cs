using System;
using System.Collections.Generic;
using System.Text;
using Cyriller.Model;

namespace Cyriller.Desktop.Models
{
    public class NumberModel
    {
        public virtual string Name { get; protected set; }
        public virtual NumbersEnum Value { get; protected set; }

        public NumberModel(NumbersEnum value)
        {
            this.Value = value;

            switch (value)
            {
                case NumbersEnum.Plural:
                    this.Name = "Множественное число";
                    break;
                case NumbersEnum.Singular:
                    this.Name = "Единственное число";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static IEnumerable<NumberModel> GetEnumerable()
        {
            Array values = Enum.GetValues(typeof(NumbersEnum));

            foreach (NumbersEnum value in values)
            {
                yield return new NumberModel(value);
            }
        }
    }
}
