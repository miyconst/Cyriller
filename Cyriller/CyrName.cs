using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyriller.Model;

namespace Cyriller
{
    public class CyrName
    {
        /// <summary>
        /// Склоняет полное имя в указанный падеж.
        /// </summary>
        /// <param name="surname">Фамилия, в именительном падеже.</param>
        /// <param name="name">Имя, в именительном падеже.</param>
        /// <param name="patronymic">Отчество, в именительном падеже.</param>
        /// <param name="@case">Падеж, в который нужно просклонять.</param>
        /// <param name="gender">Пол, указанного имени, где <see cref="GendersEnum.Undefined"/> – определить автоматически.</param>
        /// <param name="shorten">Сократить ли имя и отчество в результате склонения. К примеру, Иванов Иван Иванович, будет Иванов И. И.</param>
        /// <returns>Возвращает результат склонения в виде <see cref="CyrNameResult"/>.</returns>
        public virtual CyrNameResult Decline(string surname, string name, string patronymic, CasesEnum @case, GendersEnum gender = GendersEnum.Undefined, bool shorten = false)
        {
            string[] values = this.Decline(surname, name, patronymic, (int)@case, (int)gender, shorten);
            CyrNameResult result = new CyrNameResult(values);

            return result;
        }

        /// <summary>
        /// Склоняет полное имя в указанный падеж.
        /// </summary>
        /// <param name="fullName">Полное имя, в именительном падеже.</param>
        /// <param name="case">Падеж, в который нужно просклонять.</param>
        /// <param name="gender">Пол, указанного имени, где <see cref="GendersEnum.Undefined"/> – определить автоматически.</param>
        /// <param name="shorten">Сократить ли имя и отчество в результате склонения. К примеру, Иванов Иван Иванович, будет Иванов И. И.</param>
        /// <returns>Возвращает результат склонения в виде <see cref="CyrNameResult"/>.</returns>
        public virtual CyrNameResult Decline(string fullName, CasesEnum @case, GendersEnum gender = GendersEnum.Undefined, bool shorten = false)
        {
            string[] values = this.Decline(fullName, (int)@case, (int)gender, shorten);
            CyrNameResult result = new CyrNameResult(values);

            return result;
        }

        /// <summary>
        /// Склоняет полное имя во все возможные падежи.
        /// </summary>
        /// <param name="surname">Фамилия, в именительном падеже.</param>
        /// <param name="name">Имя, в именительном падеже.</param>
        /// <param name="patronymic">Отчество, в именительном падеже.</param>
        /// <param name="gender">Пол, указанного имени, где <see cref="GendersEnum.Undefined"/> – определить автоматически.</param>
        /// <param name="shorten">Сократить ли имя и отчество в результате склонения. К примеру, Иванов Иван Иванович, будет Иванов И. И.</param>
        /// <returns>Возвращает результат склонения в виде <see cref="CyrResult"/>.</returns>
        public virtual CyrResult Decline(string surname, string name, string patronymic, GendersEnum gender = GendersEnum.Undefined, bool shorten = false)
        {
            CyrResult result = this.DeclinePerCaseAndCombine(caseIndex => this.Decline(surname, name, patronymic, caseIndex, (int)gender, shorten));
            return result;
        }

        /// <summary>
        /// Склоняет полное имя во все возможные падежи.
        /// </summary>
        /// <param name="fullName">Полное имя, в именительном падеже.</param>
        /// <param name="gender">Пол, указанного имени, где <see cref="GendersEnum.Undefined"/> – определить автоматически.</param>
        /// <param name="shorten">Сократить ли имя и отчество в результате склонения. К примеру, Иванов Иван Иванович, будет Иванов И. И.</param>
        /// <returns>Возвращает результат склонения в виде <see cref="CyrResult"/>.</returns>
        public virtual CyrResult Decline(string fullName, GendersEnum gender = GendersEnum.Undefined, bool shorten = false)
        {
            CyrResult result = this.DeclinePerCaseAndCombine(caseIndex => this.Decline(fullName, caseIndex, (int)gender, shorten));
            return result;
        }

        /// <summary>
        /// Склоняет полное имя в указанный падеж.
        /// </summary>
        /// <param name="inputSurname">Фамилия, в именительном падеже.</param>
        /// <param name="inputName">Имя, в именительном падеже.</param>
        /// <param name="inputPatronymic">Отчество, в именительном падеже.</param>
        /// <param name="inputCase">Падеж, в который нужно просклонять, где 1 – именительный, 2 – родительный, 3 – дательный, 4 – винительный, 5 – творительный, 6 – предложный.</param>
        /// <param name="inputGender">Пол, указанного имени, где 0 – определить автоматически, 1 – мужской, 2 – женский.</param>
        /// <param name="inputShorten">Сократить ли имя и отчество в результате склонения. К примеру, Иванов Иван Иванович, будет Иванов И. И.</param>
        /// <returns>Возвращает результат склонения в виде массива из трех элементов [Фамилия, Имя, Отчество].</returns>
        public virtual string[] Decline(string inputSurname, string inputName, string inputPatronymic, int inputCase, int inputGender = 0, bool inputShorten = false)
        {
            string temp = null;
            int caseNumber = 0;
            string surname = null;
            string name = null;
            string patronymic = null;
            string patronymicAfter = null;
            string patronymicBefore = null;
            int gender = 0;
            bool isFeminine = false;
            int index = 0;
            string surnameNew = null;
            string surnameOld = null;

            caseNumber = inputCase;
            gender = inputGender;
            surname = this.ProperCase(inputSurname);
            name = this.ProperCase(inputName);
            patronymic = this.ProperCase(inputPatronymic);
            patronymicBefore = string.Empty;
            patronymicAfter = string.Empty;

            if (patronymic.StartsWith("Ибн"))
            {
                patronymicBefore = "ибн ";
                patronymic = patronymic.Substring(4);
            }

            if (patronymic.EndsWith("-оглы") || patronymic.EndsWith("-кызы"))
            {
                patronymicAfter = patronymic.Substring(patronymic.Length - 5);
                patronymic = patronymic.Substring(0, patronymic.Length - 5);
            }

            if (patronymic.EndsWith("оглы") || patronymic.EndsWith("кызы"))
            {
                patronymicAfter = patronymic.Substring(patronymic.Length - 4);
                patronymic = patronymic.Substring(0, patronymic.Length - 4);
            }

            if (caseNumber < 1 || caseNumber > 6)
            {
                caseNumber = 1;
            }

            if (gender < 0 || gender > 2)
            {
                gender = 0;
            }

            if (gender == 0)
            {
                gender = patronymic.EndsWith("на") ? 2 : 1;
            }

            isFeminine = (gender == 2);

            surnameOld = surname;
            surnameNew = string.Empty;
            index = surnameOld.IndexOf("-");

            while (index > 0)
            {
                temp = this.ProperCase(surnameOld.Substring(0, index));
                surnameNew = surnameNew + DeclineSurname(temp, caseNumber, isFeminine) + "-";
                surnameOld = surnameOld.Substring(index + 1);
                index = surnameOld.IndexOf("-");
            }

            temp = this.ProperCase(surnameOld);
            surnameNew = surnameNew + DeclineSurname(temp, caseNumber, isFeminine);
            surname = surnameNew;

            switch (caseNumber)
            {
                case 1:
                    if (inputShorten)
                    {
                        name = this.Shorten(name);
                        patronymic = this.Shorten(patronymic);
                    }
                    break;
                case 2:
                    name = this.DeclineNameGenitive(name, isFeminine, inputShorten);
                    patronymic = this.DeclinePatronymicGenitive(patronymic, patronymicAfter, isFeminine, inputShorten);
                    break;

                case 3:
                    name = this.DeclineNameDative(name, isFeminine, inputShorten);
                    patronymic = this.DeclinePatronymicDative(patronymic, patronymicAfter, isFeminine, inputShorten);
                    break;

                case 4:
                    name = this.DeclineNameAccusative(name, isFeminine, inputShorten);
                    patronymic = this.DeclinePatronymicAccusative(patronymic, patronymicAfter, isFeminine, inputShorten);
                    break;

                case 5:
                    name = this.DeclineNameInstrumental(name, isFeminine, inputShorten);
                    patronymic = this.DeclinePatronymicInstrumental(patronymic, patronymicAfter, isFeminine, inputShorten);
                    break;

                case 6:
                    name = this.DeclineNamePrepositional(name, isFeminine, inputShorten);
                    patronymic = this.DeclinePatronymicPrepositional(patronymic, patronymicAfter, isFeminine, inputShorten);
                    break;
            }

            if (!inputShorten)
            {
                patronymic = patronymicBefore + patronymic + patronymicAfter;
            }

            return new string[] { surname, name, patronymic };
        }

        /// <summary>
        /// Склоняет полное имя в указанный падеж.
        /// </summary>
        /// <param name="fullName">Полное имя, в именительном падеже.</param>
        /// <param name="case">Падеж, в который нужно просклонять, где 1 – именительный, 2 – родительный, 3 – дательный, 4 – винительный, 5 – творительный, 6 – предложный.</param>
        /// <param name="gender">Пол, указанного имени, где 0 – определить автоматически, 1 – мужской, 2 – женский.</param>
        /// <param name="shorten">Сократить ли имя и отчество в результате склонения. К примеру, Иванов Иван Иванович, будет Иванов И. И.</param>
        /// <returns>Возвращает результат склонения в виде массива из трех элементов [Фамилия, Имя, Отчество].</returns>
        public virtual string[] Decline(string fullName, int @case, int gender = 0, bool shorten = false)
        {
            string surname = null;
            string name = null;
            string patronymic = null;
            string str1 = null;
            string str2 = null;
            string str3 = null;
            int spaceIndex = 0;

            spaceIndex = fullName.IndexOf(" ");

            if (spaceIndex > 0)
            {
                str1 = fullName.Substring(0, spaceIndex).Trim().ToLower();
                fullName = fullName.Substring(spaceIndex).Trim();

                spaceIndex = fullName.IndexOf(" ");

                if (spaceIndex > 0)
                {
                    str2 = fullName.Substring(0, spaceIndex).Trim().ToLower();
                    str3 = fullName.Substring(spaceIndex).Trim().ToLower();
                }
                else
                {
                    str2 = fullName.Trim().ToLower();
                }
            }
            else
            {
                str1 = fullName.Trim().ToLower();
            }

            if (!string.IsNullOrEmpty(str3))
            {
                if (str2.EndsWith("ич") || str2.EndsWith("вна") || str2.EndsWith("чна"))
                {
                    surname = this.ProperCase(str3);
                    name = this.ProperCase(str1);
                    patronymic = this.ProperCase(str2);
                }
                else
                {
                    surname = this.ProperCase(str1);
                    name = this.ProperCase(str2);
                    patronymic = this.ProperCase(str3);
                }
            }
            else if (!string.IsNullOrEmpty(str2))
            {
                if (str2.EndsWith("ич") || str2.EndsWith("вна") || str2.EndsWith("чна"))
                {
                    name = this.ProperCase(str1); ;
                    patronymic = this.ProperCase(str2);
                }
                else
                {
                    surname = this.ProperCase(str1);
                    name = this.ProperCase(str2);
                }
            }
            else
            {
                surname = str1;
            }

            return Decline(surname, name, patronymic, @case, gender, shorten);
        }

        /// <summary>
        /// Родительный, Кого? Чего? (нет)
        /// </summary>
        /// <param name="name">Имя, для склонения.</param>
        /// <param name="isFeminine">True, для женских имен.</param>
        /// <param name="shorten">Сократить ли имя, к примеру, Иван будет И.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclineNameGenitive(string name, bool isFeminine, bool shorten)
        {
            string temp;

            if (this.IsShorten(name))
            {
                return name;
            }

            if (shorten)
            {
                name = this.Shorten(name);
            }
            else
            {
                temp = name;

                switch (SubstringRight(name, 3).ToLower())
                {
                    case "лев":
                        name = SetEnd(name, 2, "ьва");
                        break;
                }

                if (name == temp)
                {
                    switch (SubstringRight(name, 2))
                    {
                        case "ей":
                        case "ий":
                        case "ай":
                            name = SetEnd(name, "я");
                            break;
                        case "ел":
                            name = SetEnd(name, "ла");
                            break;
                        case "ец":
                            name = SetEnd(name, "ца");
                            break;
                        case "га":
                        case "жа":
                        case "ка":
                        case "ха":
                        case "ча":
                        case "ща":
                            name = SetEnd(name, "и");
                            break;
                    }
                }

                if (name == temp)
                {
                    switch (SubstringRight(name, 1))
                    {
                        case "а":
                            name = SetEnd(name, "ы");
                            break;
                        case "е":
                        case "ё":
                        case "и":
                        case "о":
                        case "у":
                        case "э":
                        case "ю":
                            break;
                        case "я":
                            name = SetEnd(name, "и");
                            break;
                        case "ь":
                            name = SetEnd(name, (isFeminine ? "и" : "я"));
                            break;
                        default:
                            if (!isFeminine)
                                name = name + "а";
                            break;
                    }
                }

            }

            return name;
        }

        /// <summary>
        /// Дательный, Кому? Чему? (дам)
        /// </summary>
        /// <param name="name">Имя, для склонения.</param>
        /// <param name="isFeminine">True, для женских имен.</param>
        /// <param name="shorten">Сократить ли имя, к примеру, Иван будет И.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclineNameDative(string name, bool isFeminine, bool shorten)
        {
            string temp;

            if (this.IsShorten(name))
            {
                return name;
            }

            if (shorten)
            {
                name = this.Shorten(name);
            }
            else
            {
                temp = name;

                switch (SubstringRight(name, 3).ToLower())
                {
                    case "лев":
                        name = SetEnd(name, 2, "ьву");
                        break;
                }

                if (name == temp)
                {
                    switch (SubstringRight(name, 2))
                    {
                        case "ей":
                        case "ий":
                        case "ай":
                            name = SetEnd(name, "ю");
                            break;
                        case "ел":
                            name = SetEnd(name, "лу");
                            break;
                        case "ец":
                            name = SetEnd(name, "цу");
                            break;
                        case "ия":
                            name = SetEnd(name, "ии");
                            break;
                    }
                }

                if (name == temp)
                {
                    switch (SubstringRight(name, 1))
                    {
                        case "а":
                        case "я":
                            name = SetEnd(name, "е");
                            break;
                        case "е":
                        case "ё":
                        case "и":
                        case "о":
                        case "у":
                        case "э":
                        case "ю":
                            break;
                        case "ь":
                            name = SetEnd(name, (isFeminine ? "и" : "ю"));
                            break;
                        default:
                            if (!isFeminine)
                            {
                                name = name + "у";
                            }
                            break;
                    }
                }
            }

            return name;
        }

        /// <summary>
        /// Винительный, Кого? Что? (вижу)
        /// </summary>
        /// <param name="name">Имя, для склонения.</param>
        /// <param name="isFeminine">True, для женских имен.</param>
        /// <param name="shorten">Сократить ли имя, к примеру, Иван будет И.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclineNameAccusative(string name, bool isFeminine, bool shorten)
        {
            string temp;

            if (this.IsShorten(name))
            {
                return name;
            }

            if (shorten)
            {
                name = this.Shorten(name);
            }
            else
            {
                temp = name;

                switch (SubstringRight(name, 3).ToLower())
                {
                    case "лев":
                        name = SetEnd(name, 2, "ьва");
                        break;
                }

                if (name == temp)
                {
                    switch (SubstringRight(name, 2))
                    {
                        case "ей":
                        case "ий":
                        case "ай":
                            name = SetEnd(name, "я");
                            break;
                        case "ел":
                            name = SetEnd(name, "ла");
                            break;
                        case "ец":
                            name = SetEnd(name, "ца");
                            break;
                    }
                }

                if (name == temp)
                {
                    switch (SubstringRight(name, 1))
                    {
                        case "а":
                            name = SetEnd(name, "у");
                            break;
                        case "е":
                        case "ё":
                        case "и":
                        case "о":
                        case "у":
                        case "э":
                        case "ю":
                            break;
                        case "я":
                            name = SetEnd(name, "ю");
                            break;
                        case "ь":
                            if (!isFeminine)
                            {
                                name = SetEnd(name, "я");
                            }
                            break;
                        default:
                            if (!isFeminine)
                                name = name + "а";
                            break;
                    }
                }
            }

            return name;
        }

        /// <summary>
        /// Творительный, Кем? Чем? (горжусь)
        /// </summary>
        /// <param name="name">Имя, для склонения.</param>
        /// <param name="isFeminine">True, для женских имен.</param>
        /// <param name="shorten">Сократить ли имя, к примеру, Иван будет И.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclineNameInstrumental(string name, bool isFeminine, bool shorten)
        {
            string temp;

            if (this.IsShorten(name))
            {
                return name;
            }

            if (shorten)
            {
                name = this.Shorten(name);
            }
            else
            {
                temp = name;

                switch (SubstringRight(name, 3).ToLower())
                {
                    case "лев":
                        name = SetEnd(name, 2, "ьвом");
                        break;
                }

                if (name == temp)
                {
                    switch (SubstringRight(name, 2))
                    {
                        case "ей":
                        case "ий":
                        case "ай":
                            name = SetEnd(name, 1, "ем");
                            break;
                        case "ел":
                            name = SetEnd(name, 2, "лом");
                            break;
                        case "ец":
                            name = SetEnd(name, 2, "цом");
                            break;
                        case "жа":
                        case "ца":
                        case "ча":
                        case "ша":
                        case "ща":
                            name = name = SetEnd(name, 1, "ей");
                            break;
                    }
                }

                if (name == temp)
                {
                    switch (SubstringRight(name, 1))
                    {
                        case "а":
                            name = SetEnd(name, 1, "ой");
                            break;
                        case "е":
                        case "ё":
                        case "и":
                        case "о":
                        case "у":
                        case "э":
                        case "ю":
                            break;
                        case "я":
                            name = SetEnd(name, 1, "ей");
                            break;
                        case "ь":
                            name = SetEnd(name, 1, (isFeminine ? "ью" : "ем"));
                            break;
                        default:
                            if (!isFeminine)
                            {
                                name = name + "ом";
                            }
                            break;
                    }
                }
            }

            return name;
        }

        /// <summary>
        /// Предложный, О ком? О чем? (думаю)
        /// </summary>
        /// <param name="name">Имя, для склонения.</param>
        /// <param name="isFeminine">True, для женских имен.</param>
        /// <param name="shorten">Сократить ли имя, к примеру, Иван будет И.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclineNamePrepositional(string name, bool isFeminine, bool shorten)
        {
            string temp;

            if (this.IsShorten(name))
            {
                return name;
            }

            if (shorten)
            {
                name = this.Shorten(name);
            }
            else
            {
                temp = name;

                switch (SubstringRight(name, 3).ToLower())
                {
                    case "лев":
                        name = SetEnd(name, 2, "ьве");
                        break;
                }

                if (name == temp)
                {
                    switch (SubstringRight(name, 2))
                    {
                        case "ей":
                        case "ай":
                            name = SetEnd(name, "е");
                            break;
                        case "ий":
                            name = SetEnd(name, "и");
                            break;
                        case "ел":
                            name = SetEnd(name, "ле");
                            break;
                        case "ец":
                            name = SetEnd(name, "це");
                            break;
                        case "ия":
                            name = SetEnd(name, "ии");
                            break;
                    }
                }

                if (name == temp)
                {
                    switch (SubstringRight(name, 1))
                    {
                        case "а":
                        case "я":
                            name = SetEnd(name, "е");
                            break;
                        case "е":
                        case "ё":
                        case "и":
                        case "о":
                        case "у":
                        case "э":
                        case "ю":
                            break;
                        case "ь":
                            name = SetEnd(name, (isFeminine ? "и" : "е"));
                            break;
                        default:
                            if (!isFeminine)
                            {
                                name = name + "е";
                            }
                            break;
                    }
                }
            }

            return name;
        }

        /// <summary>
        /// Родительный, Кого? Чего? (нет)
        /// </summary>
        /// <param name="patronymic">Отчество, для склонения.</param>
        /// <param name="patronymicAfter">
        /// Используется для составных отчеств, к примеру тюркские варианты Салим-оглы или Салим-кызы.
        /// https://ru.wikipedia.org/wiki/%D0%9E%D1%82%D1%87%D0%B5%D1%81%D1%82%D0%B2%D0%BE
        /// </param>
        /// <param name="isFeminine">True, для женских отчеств.</param>
        /// <param name="shorten">Сократить ли отчество, к примеру, Иванович будет И.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclinePatronymicGenitive(string patronymic, string patronymicAfter, bool isFeminine, bool shorten)
        {
            if (this.IsShorten(patronymic))
            {
                return patronymic;
            }

            if (shorten)
            {
                patronymic = this.Shorten(patronymic);
            }
            else
            {
                if (string.IsNullOrEmpty(patronymicAfter))
                {
                    switch (SubstringRight(patronymic, 1))
                    {
                        case "а":
                            patronymic = SetEnd(patronymic, "ы");
                            break;
                        case "е":
                        case "ё":
                        case "и":
                        case "о":
                        case "у":
                        case "э":
                        case "ю":
                            break;
                        case "я":
                            patronymic = SetEnd(patronymic, "и");
                            break;
                        case "ь":
                            patronymic = SetEnd(patronymic, (isFeminine ? "и" : "я"));
                            break;
                        default:
                            if (!isFeminine)
                            {
                                patronymic = patronymic + "а";
                            }
                            break;
                    }
                }
            }

            return patronymic;
        }

        /// <summary>
        /// Дательный, Кому? Чему? (дам)
        /// </summary>
        /// <param name="patronymic">Отчество, для склонения.</param>
        /// <param name="patronymicAfter">
        /// Используется для составных отчеств, к примеру тюркские варианты Салим-оглы или Салим-кызы.
        /// https://ru.wikipedia.org/wiki/%D0%9E%D1%82%D1%87%D0%B5%D1%81%D1%82%D0%B2%D0%BE
        /// </param>
        /// <param name="isFeminine">True, для женских отчеств.</param>
        /// <param name="shorten">Сократить ли отчество, к примеру, Иванович будет И.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclinePatronymicDative(string patronymic, string patronymicAfter, bool isFeminine, bool shorten)
        {
            if (this.IsShorten(patronymic))
            {
                return patronymic;
            }

            if (shorten)
            {
                patronymic = this.Shorten(patronymic);
            }
            else
            {
                if (string.IsNullOrEmpty(patronymicAfter))
                {
                    switch (SubstringRight(patronymic, 1))
                    {
                        case "а":
                        case "я":
                            patronymic = SetEnd(patronymic, "е");
                            break;
                        case "е":
                        case "ё":
                        case "и":
                        case "о":
                        case "у":
                        case "э":
                        case "ю":
                            break;
                        case "ь":
                            patronymic = SetEnd(patronymic, (isFeminine ? "и" : "ю"));
                            break;
                        default:
                            if (!isFeminine)
                            {
                                patronymic = patronymic + "у";
                            }
                            break;
                    }
                }
            }

            return patronymic;
        }

        /// <summary>
        /// Винительный, Кого? Что? (вижу)
        /// </summary>
        /// <param name="patronymic">Отчество, для склонения.</param>
        /// <param name="patronymicAfter">
        /// Используется для составных отчеств, к примеру тюркские варианты Салим-оглы или Салим-кызы.
        /// https://ru.wikipedia.org/wiki/%D0%9E%D1%82%D1%87%D0%B5%D1%81%D1%82%D0%B2%D0%BE
        /// </param>
        /// <param name="isFeminine">True, для женских отчеств.</param>
        /// <param name="shorten">Сократить ли отчество, к примеру, Иванович будет И.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclinePatronymicAccusative(string patronymic, string patronymicAfter, bool isFeminine, bool shorten)
        {
            if (this.IsShorten(patronymic))
            {
                return patronymic;
            }

            if (shorten)
            {
                patronymic = this.Shorten(patronymic);
            }
            else
            {
                if (string.IsNullOrEmpty(patronymicAfter))
                {
                    switch (SubstringRight(patronymic, 1))
                    {
                        case "а":
                            patronymic = SetEnd(patronymic, "у");
                            break;
                        case "е":
                        case "ё":
                        case "и":
                        case "о":
                        case "у":
                        case "э":
                        case "ю":
                            break;
                        case "я":
                            patronymic = SetEnd(patronymic, "ю");
                            break;
                        case "ь":
                            if (!isFeminine)
                                patronymic = SetEnd(patronymic, "я");
                            break;
                        default:
                            if (!isFeminine)
                                patronymic = patronymic + "а";
                            break;
                    }
                }
            }

            return patronymic;
        }

        /// <summary>
        /// Творительный, Кем? Чем? (горжусь)
        /// </summary>
        /// <param name="patronymic">Отчество, для склонения.</param>
        /// <param name="patronymicAfter">
        /// Используется для составных отчеств, к примеру тюркские варианты Салим-оглы или Салим-кызы.
        /// https://ru.wikipedia.org/wiki/%D0%9E%D1%82%D1%87%D0%B5%D1%81%D1%82%D0%B2%D0%BE
        /// </param>
        /// <param name="isFeminine">True, для женских отчеств.</param>
        /// <param name="shorten">Сократить ли отчество, к примеру, Иванович будет И.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclinePatronymicInstrumental(string patronymic, string patronymicAfter, bool isFeminine, bool shorten)
        {
            string temp;

            if (this.IsShorten(patronymic))
            {
                return patronymic;
            }

            if (shorten)
            {
                patronymic = this.Shorten(patronymic);
            }
            else
            {
                if (string.IsNullOrEmpty(patronymicAfter))
                {
                    temp = patronymic;

                    switch (SubstringRight(patronymic, 2))
                    {
                        case "ич":
                            patronymic = patronymic + (patronymic.ToLower() == "ильич" ? "ом" : "ем");
                            break;
                        case "на":
                            patronymic = SetEnd(patronymic, 2, "ной");
                            break;
                    }

                    if (patronymic == temp)
                    {
                        switch (SubstringRight(patronymic, 1))
                        {
                            case "а":
                                patronymic = SetEnd(patronymic, 1, "ой");
                                break;
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ю":
                                break;
                            case "я":
                                patronymic = SetEnd(patronymic, 1, "ей");
                                break;
                            case "ь":
                                patronymic = SetEnd(patronymic, 1, (isFeminine ? "ью" : "ем"));
                                break;
                            default:
                                if (!isFeminine)
                                {
                                    patronymic = patronymic + "ом";
                                }
                                break;
                        }
                    }
                }
            }

            return patronymic;
        }

        /// <summary>
        /// Творительный, Кем? Чем? (горжусь)
        /// </summary>
        /// <param name="patronymic">Отчество, для склонения.</param>
        /// <param name="patronymicAfter">
        /// Используется для составных отчеств, к примеру тюркские варианты Салим-оглы или Салим-кызы.
        /// https://ru.wikipedia.org/wiki/%D0%9E%D1%82%D1%87%D0%B5%D1%81%D1%82%D0%B2%D0%BE
        /// </param>
        /// <param name="isFeminine">True, для женских отчеств.</param>
        /// <param name="shorten">Сократить ли отчество, к примеру, Иванович будет И.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclinePatronymicPrepositional(string patronymic, string patronymicAfter, bool isFeminine, bool shorten)
        {
            if (this.IsShorten(patronymic))
            {
                return patronymic;
            }

            if (shorten)
            {
                patronymic = this.Shorten(patronymic);
            }
            else
            {
                if (string.IsNullOrEmpty(patronymicAfter))
                {
                    switch (SubstringRight(patronymic, 1))
                    {
                        case "а":
                        case "я":
                            patronymic = SetEnd(patronymic, "е");
                            break;
                        case "е":
                        case "ё":
                        case "и":
                        case "о":
                        case "у":
                        case "э":
                        case "ю":
                            break;
                        case "ь":
                            patronymic = SetEnd(patronymic, (isFeminine ? "и" : "е"));
                            break;
                        default:
                            if (!isFeminine)
                            {
                                patronymic = patronymic + "е";
                            }
                            break;
                    }
                }
            }

            return patronymic;
        }

        /// <summary>
        /// Родительный, Кого? Чего? (нет)
        /// </summary>
        /// <param name="surname">Фамилия, для склонения.</param>
        /// <param name="isFeminine">True, для женских фамилий.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclineSurnameGenitive(string surname, bool isFeminine)
        {
            string temp = surname;
            string end = null;

            end = SubstringRight(surname, 3);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "жий":
                    case "ний":
                    case "ций":
                    case "чий":
                    case "ший":
                    case "щий":
                        surname = SetEnd(surname, 2, "его");
                        break;
                    case "лец":
                        surname = SetEnd(surname, 2, "ьца");
                        break;
                    case "нок":
                        surname = SetEnd(surname, "нка");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ова":
                    case "ева":
                    case "ина":
                    case "ына":
                        surname = SetEnd(surname, 1, "ой");
                        break;
                    case "жая":
                    case "цая":
                    case "чая":
                    case "шая":
                    case "щая":
                        surname = SetEnd(surname, 2, "ей");
                        break;
                    case "ска":
                    case "цка":
                        surname = SetEnd(surname, 1, "ой");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 2);

            switch (end)
            {
                case "га":
                case "жа":
                case "ка":
                case "ха":
                case "ча":
                case "ша":
                case "ща":
                    surname = SetEnd(surname, 1, "и");
                    break;
            }

            if (surname != temp)
            {
                return surname;
            }

            if (!isFeminine)
            {
                switch (end)
                {
                    case "ок":
                        surname = SetEnd(surname, 1, "ка");
                        break;
                    case "ёк":
                    case "ек":
                        surname = SetEnd(surname, 2, "ька");
                        break;
                    case "ец":
                        surname = SetEnd(surname, 2, "ца");
                        break;
                    case "ий":
                    case "ый":
                    case "ой":
                        if (surname.Length > 4)
                        {
                            surname = SetEnd(surname, 2, "ого");
                        }
                        break;
                    case "ей":
                        if (surname.ToLower() == "соловей" || surname.ToLower() == "воробей")
                        {
                            surname = SetEnd(surname, 2, "ья");
                        }
                        else
                        {
                            surname = SetEnd(surname, 2, "ея");
                        }
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ая":
                        surname = SetEnd(surname, 2, "ой");
                        break;
                    case "яя":
                        surname = SetEnd(surname, 2, "ей");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 1);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, 1, "ы");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, 1, "и");
                        break;
                    case "б":
                    case "в":
                    case "г":
                    case "д":
                    case "ж":
                    case "з":
                    case "к":
                    case "л":
                    case "м":
                    case "н":
                    case "п":
                    case "р":
                    case "с":
                    case "т":
                    case "ф":
                    case "ц":
                    case "ч":
                    case "ш":
                    case "щ":
                        surname = surname + "а";
                        break;
                    case "х":
                        if (!surname.EndsWith("их") && !surname.EndsWith("ых"))
                        {
                            surname = surname + "а";
                        }
                        break;
                    case "ь":
                    case "й":
                        surname = SetEnd(surname, 1, "я");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, 1, "ы");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, 1, "и");
                        break;
                }
            }

            return surname;
        }

        /// <summary>
        /// Дательный, Кому? Чему? (дам)
        /// </summary>
        /// <param name="surname">Фамилия, для склонения.</param>
        /// <param name="isFeminine">True, для женских фамилий.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclineSurnameDative(string surname, bool isFeminine)
        {
            string temp = surname;
            string end;

            end = SubstringRight(surname, 3);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "жий":
                    case "ний":
                    case "ций":
                    case "чий":
                    case "ший":
                    case "щий":
                        surname = SetEnd(surname, 2, "ему");
                        break;
                    case "лец":
                        surname = SetEnd(surname, 2, "ьцу");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ова":
                    case "ева":
                    case "ина":
                    case "ына":
                        surname = SetEnd(surname, 1, "ой");
                        break;
                    case "жая":
                    case "цая":
                    case "чая":
                    case "шая":
                    case "щая":
                        surname = SetEnd(surname, 2, "ей");
                        break;
                    case "ска":
                    case "цка":
                        surname = SetEnd(surname, 1, "ой");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 2);

            switch (end)
            {
                case "ия":
                    surname = SetEnd(surname, 1, "и");
                    break;
            }

            if (surname != temp)
            {
                return surname;
            }

            if (!isFeminine)
            {
                switch (end)
                {
                    case "ок":
                        surname = SetEnd(surname, 2, "ку");
                        break;
                    case "ёк":
                    case "ек":
                        surname = SetEnd(surname, 2, "ьку");
                        break;
                    case "ец":
                        surname = SetEnd(surname, 2, "цу");
                        break;
                    case "ий":
                    case "ый":
                    case "ой":
                        if (surname.Length > 4)
                        {
                            surname = SetEnd(surname, 2, "ому");
                        }
                        break;
                    case "ей":
                        if (surname.ToLower() == "соловей" || surname.ToLower() == "воробей")
                        {
                            surname = SetEnd(surname, 2, "ью");
                        }
                        else
                        {
                            surname = SetEnd(surname, 2, "ею");
                        }
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ая":
                        surname = SetEnd(surname, 2, "ой");
                        break;
                    case "яя":
                        surname = SetEnd(surname, 2, "ей");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 1);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, 1, "е");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, 1, "е");
                        break;
                    case "б":
                    case "в":
                    case "г":
                    case "д":
                    case "ж":
                    case "з":
                    case "к":
                    case "л":
                    case "м":
                    case "н":
                    case "п":
                    case "р":
                    case "с":
                    case "т":
                    case "ф":
                    case "ц":
                    case "ч":
                    case "ш":
                    case "щ":
                        surname = surname + "у";
                        break;
                    case "х":
                        if (!surname.EndsWith("их") && !surname.EndsWith("ых"))
                        {
                            surname = surname + "у";
                        }
                        break;
                    case "ь":
                    case "й":
                        surname = SetEnd(surname, 1, "ю");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, 1, "е");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, 1, "е");
                        break;
                }
            }

            return surname;
        }

        /// <summary>
        /// Винительный, Кого? Что? (вижу)
        /// </summary>
        /// <param name="surname">Фамилия, для склонения.</param>
        /// <param name="isFeminine">True, для женских фамилий.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclineSurnameAccusative(string surname, bool isFeminine)
        {
            string temp = surname;
            string end;

            end = SubstringRight(surname, 3);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "жий":
                    case "ний":
                    case "ций":
                    case "чий":
                    case "ший":
                    case "щий":
                        surname = SetEnd(surname, 2, "его");
                        break;
                    case "лец":
                        surname = SetEnd(surname, 2, "ьца");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ова":
                    case "ева":
                    case "ина":
                    case "ына":
                        surname = SetEnd(surname, "у");
                        break;
                    case "ска":
                    case "цка":
                        surname = SetEnd(surname, 1, "ую");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 2);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "ок":
                        surname = SetEnd(surname, "ка");
                        break;
                    case "ёк":
                    case "ек":
                        surname = SetEnd(surname, 2, "ька");
                        break;
                    case "ец":
                        surname = SetEnd(surname, "ца");
                        break;
                    case "ий":
                    case "ый":
                    case "ой":
                        if (surname.Length > 4)
                        {
                            surname = SetEnd(surname, 2, "ого");
                        }
                        break;
                    case "ей":
                        if (surname.ToLower() == "соловей" || surname.ToLower() == "воробей")
                        {
                            surname = SetEnd(surname, "ья");
                        }
                        else
                        {
                            surname = SetEnd(surname, "ея");
                        }
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ая":
                        surname = SetEnd(surname, "ую");
                        break;
                    case "яя":
                        surname = SetEnd(surname, "юю");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 1);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, "у");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, "ю");
                        break;
                    case "б":
                    case "в":
                    case "г":
                    case "д":
                    case "ж":
                    case "з":
                    case "к":
                    case "л":
                    case "м":
                    case "н":
                    case "п":
                    case "р":
                    case "с":
                    case "т":
                    case "ф":
                    case "ц":
                    case "ч":
                    case "ш":
                    case "щ":
                        surname = surname + "а";
                        break;
                    case "х":
                        if (!surname.EndsWith("их") && !surname.EndsWith("ых"))
                        {
                            surname = surname + "а";
                        }
                        break;
                    case "ь":
                    case "й":
                        surname = SetEnd(surname, "я");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, "у");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, "ю");
                        break;
                }
            }

            return surname;
        }

        /// <summary>
        /// Творительный, Кем? Чем? (горжусь)
        /// </summary>
        /// <param name="surname">Фамилия, для склонения.</param>
        /// <param name="isFeminine">True, для женских фамилий.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclineSurnameInstrumental(string surname, bool isFeminine)
        {
            string temp = surname;
            string end;

            end = SubstringRight(surname, 3);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "лец":
                        surname = SetEnd(surname, 2, "ьцом");
                        break;
                    case "бец":
                        surname = SetEnd(surname, 2, "цем");
                        break;
                    case "кой":
                        surname = SetEnd(surname, "им");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "жая":
                    case "цая":
                    case "чая":
                    case "шая":
                    case "щая":
                        surname = SetEnd(surname, "ей");
                        break;
                    case "ска":
                    case "цка":
                        surname = SetEnd(surname, 1, "ой");
                        break;
                    case "еца":
                    case "ица":
                    case "аца":
                    case "ьца":
                        surname = SetEnd(surname, 1, "ей");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 2);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "ок":
                        surname = SetEnd(surname, 2, "ком");
                        break;
                    case "ёк":
                    case "ек":
                        surname = SetEnd(surname, 2, "ьком");
                        break;
                    case "ец":
                        surname = SetEnd(surname, 2, "цом");
                        break;
                    case "ий":
                        if (surname.Length > 4)
                        {
                            surname = SetEnd(surname, "им");
                        }
                        break;
                    case "ый":
                    case "ой":
                        if (surname.Length > 4)
                        {
                            surname = SetEnd(surname, "ым");
                        }
                        break;
                    case "ей":
                        if (surname.ToLower() == "соловей" || surname.ToLower() == "воробей")
                        {
                            surname = SetEnd(surname, 2, "ьем");
                        }
                        else
                        {
                            surname = SetEnd(surname, 2, "еем");
                        }
                        break;
                    case "оч":
                    case "ич":
                    case "иц":
                    case "ьц":
                    case "ьш":
                    case "еш":
                    case "ыш":
                    case "яц":
                        surname = surname + "ем";
                        break;
                    case "ин":
                    case "ын":
                    case "ен":
                    case "эн":
                    case "ов":
                    case "ев":
                    case "ёв":
                    case "ун":
                        if (surname.ToLower() != "дарвин" && surname.ToLower() != "франклин" && surname.ToLower() != "чаплин" && surname.ToLower() != "грин")
                        {
                            surname = surname + "ым";
                        }
                        break;
                    case "жа":
                    case "ца":
                    case "ча":
                    case "ша":
                    case "ща":
                        surname = SetEnd(surname, 1, "ей");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ая":
                        surname = SetEnd(surname, "ой");
                        break;
                    case "яя":
                        surname = SetEnd(surname, "ей");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 1);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, 1, "ой");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, 1, "ей");
                        break;
                    case "б":
                    case "в":
                    case "г":
                    case "д":
                    case "ж":
                    case "з":
                    case "к":
                    case "л":
                    case "м":
                    case "н":
                    case "п":
                    case "р":
                    case "с":
                    case "т":
                    case "ф":
                    case "ц":
                    case "ч":
                    case "ш":
                        surname = surname + "ом";
                        break;
                    case "х":
                        if (!surname.EndsWith("их") && !surname.EndsWith("ых"))
                        {
                            surname = surname + "ом";
                        }
                        break;
                    case "щ":
                        surname = surname + "ем";
                        break;
                    case "ь":
                    case "й":
                        surname = SetEnd(surname, 1, "ем");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, 1, "ой");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, 1, "ей");
                        break;
                }
            }

            return surname;
        }

        /// <summary>
        /// Предложный, О ком? О чем? (думаю)
        /// </summary>
        /// <param name="surname">Фамилия, для склонения.</param>
        /// <param name="isFeminine">True, для женских фамилий.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclineSurnamePrepositional(string surname, bool isFeminine)
        {
            string temp = surname;
            string end;

            end = SubstringRight(surname, 3);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "жий":
                    case "ний":
                    case "ций":
                    case "чий":
                    case "ший":
                    case "щий":
                        surname = SetEnd(surname, "ем");
                        break;
                    case "лец":
                        surname = SetEnd(surname, 2, "ьце");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ова":
                    case "ева":
                    case "ина":
                    case "ына":
                        surname = SetEnd(surname, 1, "ой");
                        break;
                    case "жая":
                    case "цая":
                    case "чая":
                    case "шая":
                    case "щая":
                        surname = SetEnd(surname, "ей");
                        break;
                    case "ска":
                    case "цка":
                        surname = SetEnd(surname, 1, "ой");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 2);

            switch (end)
            {
                case "ия":
                    surname = SetEnd(surname, "и");
                    break;
            }

            if (surname != temp)
            {
                return surname;
            }

            if (!isFeminine)
            {
                switch (end)
                {
                    case "ок":
                        surname = SetEnd(surname, "ке");
                        break;
                    case "ёк":
                    case "ек":
                        surname = SetEnd(surname, 2, "ьке");
                        break;
                    case "ец":
                        surname = SetEnd(surname, "це");
                        break;
                    case "ий":
                    case "ый":
                    case "ой":
                        if (surname.Length > 4)
                        {
                            surname = SetEnd(surname, "ом");
                        }
                        break;
                    case "ей":
                        if (surname.ToLower() == "соловей" || surname.ToLower() == "воробей")
                        {
                            surname = SetEnd(surname, "ье");
                        }
                        else
                        {
                            surname = SetEnd(surname, "ее");
                        }
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ая":
                        surname = SetEnd(surname, "ой");
                        break;
                    case "яя":
                        surname = SetEnd(surname, "ей");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 1);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, "е");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, "е");
                        break;
                    case "б":
                    case "в":
                    case "г":
                    case "д":
                    case "ж":
                    case "з":
                    case "к":
                    case "л":
                    case "м":
                    case "н":
                    case "п":
                    case "р":
                    case "с":
                    case "т":
                    case "ф":
                    case "ц":
                    case "ч":
                    case "ш":
                    case "щ":
                        surname = surname + "е";
                        break;
                    case "х":
                        if (!surname.EndsWith("их") && !surname.EndsWith("ых"))
                        {
                            surname = surname + "е";
                        }
                        break;
                    case "ь":
                    case "й":
                        surname = SetEnd(surname, "е");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, "е");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, "е");
                        break;
                }
            }

            return surname;
        }

        /// <summary>
        /// Склоняет фамилию в указанный падеж.
        /// </summary>
        /// <param name="surname">Фамилия, в именительном падеже.</param>
        /// <param name="case">Падеж, в который нужно просклонять фамилию, где 1 – именительный, 2 – родительный, 3 – дательный, 4 – винительный, 5 – творительный, 6 – предложный.</param>
        /// <param name="isFeminine">True, для женских фамилий.</param>
        /// <returns>Возвращает результат склонения.</returns>
        public virtual string DeclineSurname(string surname, int @case, bool isFeminine)
        {
            string result = surname;

            if (surname.Length <= 1 || @case < 2 || @case > 6)
            {
                result = surname;
                return result;
            }

            switch (@case)
            {
                case 2:
                    result = this.DeclineSurnameGenitive(surname, isFeminine);
                    break;

                case 3:
                    result = this.DeclineSurnameDative(surname, isFeminine);
                    break;

                case 4:
                    result = this.DeclineSurnameAccusative(surname, isFeminine);
                    break;

                case 5:
                    result = this.DeclineSurnameInstrumental(surname, isFeminine);
                    break;

                case 6:
                    result = this.DeclineSurnamePrepositional(surname, isFeminine);
                    break;
            }

            return result;
        }

        protected virtual string ProperCase(string value)
        {
            if (value != null)
            {
                value = value.Replace("\uFEFF", string.Empty).Trim();
            }

            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            value = value.ToLower();

            return char.ToUpper(value[0]) + value.Substring(1);
        }

        protected virtual string SetEnd(string value, string add)
        {
            return SetEnd(value, add.Length, add);
        }

        protected virtual string SetEnd(string value, int cut, string add)
        {
            return value.Substring(0, value.Length - cut) + add;
        }

        protected virtual string SubstringRight(string value, int cut)
        {
            if (cut > value.Length)
            {
                cut = value.Length;
            }

            return value.Substring(value.Length - cut);
        }

        /// <summary>
        /// Вызывает предоставленную функцию для каждого падежа (<see cref="CyrDeclineCase.List"/>) и возвращает общий результат.
        /// </summary>
        /// <param name="decline">
        /// Функция для склонения в определенном падеже.
        /// Принимает <see cref="int"/> номер падежа. 
        /// Возвращает результат склонения в виде массива из трех элементов [Фамилия, Имя, Отчество].
        /// </param>
        /// <returns>Возвращает результат склонения в виде <see cref="CyrResult"/>.</returns>
        protected virtual CyrResult DeclinePerCaseAndCombine(Func<int, string[]> decline)
        {
            string[] cases = new string[6];

            foreach (CyrDeclineCase @case in CyrDeclineCase.GetEnumerable())
            {
                int caseIndex = @case.Index;
                string[] values = decline(caseIndex);
                CyrNameResult caseResult = new CyrNameResult(values);

                cases[caseIndex - 1] = caseResult.ToString();
            }

            CyrResult result = new CyrResult(cases);

            return result;
        }

        /// <summary>
        /// Сокращает указанное значение. Пример: "Петровна" -> "П.", "Николай" -> "Н.".
        /// Возвращает null, если входное значение является пустой строкой или null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string Shorten(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            string result = new string(new char[] { value[0], '.' });

            return result;
        }

        /// <summary>
        /// Возвращает true, если входная строка является пустой, null или заканчивается на точку (.).
        /// Пример: "Петровна" -> false, "Н." -> true.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual bool IsShorten(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            return value[value.Length - 1] == '.';
        }
    }
}
