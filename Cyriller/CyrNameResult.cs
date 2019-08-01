using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Cyriller.Model;

namespace Cyriller
{
    public class CyrNameResult
    {
        public string Name { get; protected set; }
        public string Surname { get; protected set; }
        public string Patronymic { get; protected set; }

        /// <summary>
        /// Создает экземпляр класса <see cref="CyrNameResult"/> и заполняет значения <see cref="Surname"/>, <see cref="Name"/>, <see cref="Patronymic"/>.
        /// </summary>
        /// <param name="surname"></param>
        /// <param name="name"></param>
        /// <param name="patronymic"></param>
        public CyrNameResult(string surname, string name, string patronymic)
        {
            this.Name = name;
            this.Surname = surname;
            this.Patronymic = patronymic;
        }

        /// <summary>
        /// Создает экземпляр класса <see cref="CyrNameResult"/> и заполняет значения <see cref="Surname"/>, <see cref="Name"/>, <see cref="Patronymic"/> из массива.
        /// Предоставленный массив должен иметь 2 или 3 элемента, в противном случае, <see cref="ArgumentException"/> исключение будет выброшено.
        /// </summary>
        /// <param name="values"></param>
        public CyrNameResult(string[] values)
        {
            if (values?.Length != 2 && values?.Length != 3)
            {
                throw new ArgumentException("Name must contain two or three parts.");
            }

            this.Surname = values[0];
            this.Name = values[1];
            this.Patronymic = values.Length == 3 ? values[2] : null;
        }

        public string FullName
        {
            get
            {
                string[] values = new string[] { this.Surname, this.Name, this.Patronymic }
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToArray();

                string fullName = string.Join(" ", values);

                return fullName;
            }
        }

        public override string ToString()
        {
            return this.FullName;
        }

        public override bool Equals(object obj)
        {
            if (obj is CyrNameResult that)
            {
                return string.Equals(this.Name, that.Name)
                    && string.Equals(this.Surname, that.Surname)
                    && string.Equals(this.Patronymic, that.Patronymic);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
