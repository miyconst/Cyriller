using System;
using System.Collections.Generic;
using System.Text;
using Cyriller.Model;

namespace Cyriller.Desktop.Models
{
    public class AnimateModel
    {
        public string Name { get; protected set; }
        public AnimatesEnum Value { get; protected set; }

        public AnimateModel(AnimatesEnum value)
        {
            this.Value = value;

            switch (value)
            {
                case AnimatesEnum.Animated:
                    this.Name = "Одушевленный предмет";
                    break;
                case AnimatesEnum.Inanimated:
                    this.Name = "Неодушевленный предмет";
                    break;
                default:
                    this.Name = null;
                    break;
            }
        }

        public static IEnumerable<AnimateModel> GetEnumerable()
        {
            Array values = Enum.GetValues(typeof(AnimatesEnum));

            foreach (AnimatesEnum value in values)
            {
                yield return new AnimateModel(value);
            }
        }
    }
}
