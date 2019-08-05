using System;
using System.Collections.Generic;
using System.Text;
using Cyriller.Model;

namespace Cyriller.Desktop.Models
{
    public class GenderModel
    {
        public virtual string Name { get; protected set; }
        public virtual GendersEnum Value { get; protected set; }

        public GenderModel(GendersEnum value)
        {
            this.Value = value;

            switch (value)
            {
                case GendersEnum.Feminine:
                    this.Name = "Женский род";
                    break;
                case GendersEnum.Masculine:
                    this.Name = "Мужской род";
                    break;
                case GendersEnum.Neuter:
                    this.Name = "Нейтральный род";
                    break;
                default:
                    this.Name = "Неопределенный род";
                    break;
            }
        }
    }
}
