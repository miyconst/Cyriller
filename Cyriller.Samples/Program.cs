using System;
using System.Text;
using Cyriller.Model;

namespace Cyriller.Samples
{
    class Program
    {
        private static CyrNounCollection cyrNounCollection;
        private static CyrAdjectiveCollection cyrAdjectiveCollection;
        private static CyrPhrase cyrPhrase;
        private static CyrName cyrName = new CyrName();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Загружаю словарь существительных.");
            cyrNounCollection = new CyrNounCollection();

            Console.WriteLine("Загружаю словарь прилагательных.");
            cyrAdjectiveCollection = new CyrAdjectiveCollection();

            cyrPhrase = new CyrPhrase(cyrNounCollection, cyrAdjectiveCollection);
            cyrName = new CyrName();

            Console.WriteLine("Склоняю существительные.");
            Console.WriteLine();
            NounSamples();

            Console.WriteLine("Склоняю прилагательные.");
            Console.WriteLine();
            AdjectiveSamples();

            Console.WriteLine("Склоняю словосочетания.");
            Console.WriteLine();
            PhraseSamples();

            Console.WriteLine("Склоняю имена без использования словаря.");
            Console.WriteLine();
            NameSamples();
        }

        static void NounSamples()
        {
            {
                Console.WriteLine("Поиск существительного по точному совпадению с автоматическим определением рода, падежа и числа.");
                CyrNoun noun = cyrNounCollection.Get("компьютер", out CasesEnum @case, out NumbersEnum number);
                WriteToConsole(noun);
            }

            {
                Console.WriteLine("Поиск существительного по неточному совпадению с автоматическим определением рода, падежа и числа.");
                CyrNoun noun = cyrNounCollection.Get("трипьютер", out string foundWord, out CasesEnum @case, out NumbersEnum number);
                WriteToConsole(noun, foundWord);
            }

            {
                Console.WriteLine("Поиск существительного по точному совпадению с указанием рода, падежа и числа.");
                CyrNoun noun = cyrNounCollection.Get("кошка", GendersEnum.Feminine, CasesEnum.Nominative, NumbersEnum.Singular);
                WriteToConsole(noun);
            }

            {
                Console.WriteLine("Поиск существительного по неточному совпадению с указанием рода, падежа и числа.");
                CyrNoun noun = cyrNounCollection.GetOrDefault("фошки", out string foundWord, GendersEnum.Feminine, CasesEnum.Genitive, NumbersEnum.Singular);
                WriteToConsole(noun, foundWord);
            }
        }

        static void AdjectiveSamples()
        {
            {
                Console.WriteLine("Поиск прилагательного по точному совпадению с автоматическим определением рода, падежа, числа и одушевленности.");
                CyrAdjective adjective = cyrAdjectiveCollection.Get("пушистая", out GendersEnum gender, out CasesEnum @case, out NumbersEnum number, out AnimatesEnum animate);
                WriteToConsole(adjective);
            }

            {
                Console.WriteLine("Поиск прилагательного по неточному совпадению с автоматическим определением рода, падежа, числа и одушевленности.");
                CyrAdjective adjective = cyrAdjectiveCollection.Get("кушистый", out string foundWord, out GendersEnum gender, out CasesEnum @case, out NumbersEnum number, out AnimatesEnum animate);
                WriteToConsole(adjective, foundWord);
            }

            {
                Console.WriteLine("Поиск прилагательного по точному совпадению с указанием рода, падежа, числа и одушевленности.");
                CyrAdjective adjective = cyrAdjectiveCollection.Get("пушистого", GendersEnum.Masculine, CasesEnum.Genitive, NumbersEnum.Singular, AnimatesEnum.Animated);
                WriteToConsole(adjective);
            }

            {
                Console.WriteLine("Поиск прилагательного по неточному совпадению с указанием рода, падежа, числа и одушевленности.");
                CyrAdjective adjective = cyrAdjectiveCollection.GetOrDefault("кушистого", out string foundWord, GendersEnum.Masculine, CasesEnum.Genitive, NumbersEnum.Singular, AnimatesEnum.Animated);
                WriteToConsole(adjective, foundWord);
            }
        }

        static void PhraseSamples()
        {
            {
                string phrase = "пушистый кот";
                Console.WriteLine("Склоняю словосочетание с поиском слов по точному совпадению.");
                CyrResult result = cyrPhrase.Decline(phrase, GetConditionsEnum.Strict);
                WriteToConsole(phrase, result);
            }

            {
                string phrase = "кушистый мот";
                Console.WriteLine("Склоняю словосочетание с поиском слов по неточному совпадению.");
                CyrResult result = cyrPhrase.Decline(phrase, GetConditionsEnum.Similar);
                WriteToConsole(phrase, result);
            }
        }

        static void NameSamples()
        {
            CyrDeclineCase[] cases = CyrDeclineCase.List;

            {
                Console.WriteLine("Склоняю полное имя с указанием фамилии, имени и отчества отдельно.");
                CyrResult result = cyrName.Decline("Петров", "Сергей", "Витальевич");
                WriteToConsole(result.Nominative, result);
            }

            {
                Console.WriteLine("Склоняю полное имя в сокращенном варианте с указанием фамилии, имени и отчества отдельно.");
                CyrResult result = cyrName.Decline("Петров", "С.", "В.");
                WriteToConsole(result.Nominative, result);
            }

            {
                string name = "Семенова Дарья Николаевна";
                Console.WriteLine("Склоняю полное имя с указанием всего имени одной строкой.");
                CyrResult result = cyrName.Decline(name);
                WriteToConsole(name, result);
            }

            {
                string name = "Семенова Д. Н.";
                Console.WriteLine("Склоняю полное имя в сокращенном варианте с указанием всего имени одной строкой.");
                CyrResult result = cyrName.Decline(name);
                WriteToConsole(name, result);
            }

            {
                string name = "Семенова Дарья Николаевна";
                Console.WriteLine("Склоняю и сокращаю полное имя с указанием всего имени одной строкой.");
                CyrResult result = cyrName.Decline(name, shorten: true);
                WriteToConsole(name, result);
            }

            {
                string name = "Петров Сергей Витальевич";
                Console.WriteLine("Склоняю полное имя в определенный падеж.");
                CyrNameResult result = cyrName.Decline(name, CasesEnum.Prepositional);
                WriteToConsole(name, CyrDeclineCase.Prepositional, result);
            }

            {
                string name = "Семенова Дарья";
                Console.WriteLine("Склоняю неполное имя в определенный падеж.");
                CyrNameResult result = cyrName.Decline(name, CasesEnum.Genitive);
                WriteToConsole(name, CyrDeclineCase.Genitive, result);
            }
        }

        static void WriteToConsole(CyrNoun noun, string foundWord = null)
        {
            Console.WriteLine($"{noun.Name}");

            if (!string.IsNullOrWhiteSpace(foundWord) && !string.Equals(noun.Name, foundWord, StringComparison.InvariantCulture))
            {
                Console.Write(" - слово в словаре: ");
                Console.WriteLine(foundWord);
            }

            switch (noun.Gender)
            {
                case GendersEnum.Feminine:
                    Console.WriteLine(" - женский род");
                    break;
                case GendersEnum.Masculine:
                    Console.WriteLine(" - мужской род");
                    break;
                case GendersEnum.Neuter:
                    Console.WriteLine(" - нейтральный род");
                    break;
            }

            if (noun.IsAnimated)
            {
                Console.WriteLine(" - одушевленный предмет");
            }
            else
            {
                Console.WriteLine(" - неодушевленный предмет");
            }

            Console.Write(" - Единственное число: ");
            Console.WriteLine(string.Join(", ", noun.Decline().ToArray()));

            Console.Write(" - Множественное число: ");
            Console.WriteLine(string.Join(", ", noun.DeclinePlural().ToArray()));

            Console.WriteLine();
        }

        static void WriteToConsole(CyrAdjective adjective, string foundWord = null)
        {
            Console.WriteLine(adjective.Name);

            if (!string.IsNullOrWhiteSpace(foundWord) && !string.Equals(adjective.Name, foundWord, StringComparison.InvariantCulture))
            {
                Console.Write(" - слово в словаре: ");
                Console.WriteLine(foundWord);
            }

            Console.WriteLine(" - Единственное число, мужской род, одушевленный предмет: ");
            Console.Write("   ");
            Console.WriteLine(string.Join(", ", adjective.Decline(GendersEnum.Masculine, AnimatesEnum.Animated).ToArray()));

            Console.WriteLine(" - Единственное число, мужской род, неодушевленный предмет: ");
            Console.Write("   ");
            Console.WriteLine(string.Join(", ", adjective.Decline(GendersEnum.Masculine, AnimatesEnum.Inanimated).ToArray()));

            Console.WriteLine(" - Единственное число, женский род, одушевленный предмет: ");
            Console.Write("   ");
            Console.WriteLine(string.Join(", ", adjective.Decline(GendersEnum.Feminine, AnimatesEnum.Animated).ToArray()));

            Console.WriteLine(" - Единственное число, женский род, неодушевленный предмет: ");
            Console.Write("   ");
            Console.WriteLine(string.Join(", ", adjective.Decline(GendersEnum.Feminine, AnimatesEnum.Inanimated).ToArray()));

            Console.WriteLine(" - Множественное число, одушевленный предмет: ");
            Console.Write("   ");
            Console.WriteLine(string.Join(", ", adjective.DeclinePlural(AnimatesEnum.Animated).ToArray()));

            Console.WriteLine(" - Множественное число, неодушевленный предмет: ");
            Console.Write("   ");
            Console.WriteLine(string.Join(", ", adjective.DeclinePlural(AnimatesEnum.Inanimated).ToArray()));

            Console.WriteLine();
        }

        static void WriteToConsole(string phrase, CyrResult result)
        {
            Console.WriteLine(phrase);
            Console.Write(" - ");
            Console.WriteLine(string.Join(", ", result.ToArray()));
            Console.WriteLine();
        }

        static void WriteToConsole(string name, CyrDeclineCase @case, CyrNameResult result)
        {
            Console.WriteLine(name);
            Console.Write(" - ");
            Console.Write(@case.NameRu);
            Console.Write(", ");
            Console.Write(@case.Description);
            Console.Write(": ");
            Console.WriteLine(result.ToString());
            Console.WriteLine();
        }
    }
}
