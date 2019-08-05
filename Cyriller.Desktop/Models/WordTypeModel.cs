using System;
using System.Collections.Generic;
using System.Text;
using Cyriller.Model;

namespace Cyriller.Desktop.Models
{
    public class WordTypeModel
    {
        public virtual string Name { get; protected set; }
        public virtual WordTypesEnum Value { get; protected set; }

        public WordTypeModel(WordTypesEnum value)
        {
            this.Value = value;

            switch (value)
            {
                case WordTypesEnum.Abbreviation:
                    this.Name = "Аббревиатура";
                    break;
                case WordTypesEnum.Comparative:
                    this.Name = "Сравнительная степень на по-";
                    break;
                case WordTypesEnum.ComparativeEj:
                    this.Name = "Форма компаратива на -ей";
                    break;
                case WordTypesEnum.FormEy:
                    this.Name = "Форма на -ею";
                    break;
                case WordTypesEnum.FormOy:
                    this.Name = "Форма на -ою";
                    break;
                case WordTypesEnum.Name:
                    this.Name = "Имя";
                    break;
                case WordTypesEnum.Ordinal:
                    this.Name = "Порядковое";
                    break;
                case WordTypesEnum.Organization:
                    this.Name = "Организация";
                    break;
                case WordTypesEnum.Patronymic:
                    this.Name = "Отчество";
                    break;
                case WordTypesEnum.Possessive:
                    this.Name = "Притяжательное";
                    break;
                case WordTypesEnum.Pronominal:
                    this.Name = "Местоименное";
                    break;
                case WordTypesEnum.Qualitative:
                    this.Name = "Качественное";
                    break;
                case WordTypesEnum.Superlative:
                    this.Name = "Превосходная степень";
                    break;
                case WordTypesEnum.Surname:
                    this.Name = "Фамилия";
                    break;
                case WordTypesEnum.Toponym:
                    this.Name = "Топоним";
                    break;
                case WordTypesEnum.TradeMark:
                    this.Name = "Торговая марка";
                    break;
                case WordTypesEnum.None:
                default:
                    this.Name = null;
                    break;
            }
        }
    }
}
