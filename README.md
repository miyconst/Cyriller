Cyriller - бесплатная программа склонения по падежам
========

Кириллер, это полностью открытый проект на c#, который является бесплатной альтернативой Морферу. 
На данные момент программа умеет:

* Склонять личные имена, фамилии, отчества без использования словаря.
* Склонять все существительные русского языка, включая имена с использованием словаря.
* Склонять существительные, которых нет в словаре по ближайшему совпадению.
* Склонять прилагательные русского языка с использованием словаря.
* Склонять прилагательные, которых нет в словаре по ближайшему совпадению.
* Писать числа прописью во всех падежах.
* Писать деньги прописью, в рублях, долларах, евро, юанях.
* Писать числа прописью вместе с единицей измерения.

### Как пользоваться

#### Личные имена

Для склонения личных имен, можно использовать класс `CyrNoun`, в данном случае идет поиск имени в словаре.

Так же можно использовать класс `CyrName`, который склоняет личные имена по алгоритму, и не пользуется словарем.

- [Пример склонения личных имен и фамилий с использованием класса `Cyriller.CyrName`](https://gist.github.com/miyconst/47b108c868a934ac182fd2a3dd999e67).
- Класс `CyrName`, так же доступен в Java Script варианте - [`CyrName.js`](CyrName.js).

*Более детальное описание будет позже*.

#### Существительное

Склонение существительных выполняется при помощи класса `CyrNounCollection`.

При создании коллекции, весь словарь существительных склоняется и загружается в память.
Для этого требуется около 200 MB памяти и 2 секунд одного ядра процессора.

Коллекция `CyrNounCollection` имеет следующие варианты поиска слова:

- Поиск существительного по точному совпадению с автоматическим определением рода, падежа и числа.
- Поиск существительного по неточному совпадению с автоматическим определением рода, падежа и числа.
- Поиск существительного по точному совпадению с указанием рода, падежа и числа.
- Поиск существительного по неточному совпадению с указанием рода, падежа и числа.

Смотри пример использования класса `CyrNounCollection` и `CyrNoun` в методе `/Cyriller.Samples/Program.cs/NounSamples`.

#### Прилагательное

Склонение прилагательных выполняется при помощи класса `CyrAdjectiveCollection`.

При создании коллекции, весь словарь прилагательных склоняется и загружается в память,
Для этого требуется около 250 MB памяти и 2 секунд одного ядра процессора.

Коллекция `CyrAdjectiveCollection` имеет следующие варианты поиска слова:

- Поиск прилагательного по точному совпадению с автоматическим определением рода, падежа, числа и одушевленности.
- Поиск прилагательного по неточному совпадению с автоматическим определением рода, падежа, числа и одушевленности.
- Поиск прилагательного по точному совпадению с указанием рода, падежа, числа и одушевленности.
- Поиск прилагательного по неточному совпадению с указанием рода, падежа, числа и одушевленности.

Смотри пример использования класса `CyrAdjectiveCollection` и `CyrAdjective` в методе `/Cyriller.Samples/Program.cs/AdjectiveSamples`.

#### Фраза

При помощи класса `CyrPhrase` можно склонять словосочетания из существительных и прилагательных.

Смотри пример использования класса `CyrPhrase` в методе `/Cyriller.Samples/Program.cs/AdjectiveSamples`.

#### Число

Класс `CyrNumber`, отвечающий за склонение чисел стоит немного особняком и не нуждается в коллекциях.
Тем не менее, `CyrNumber` может склонять не просто числа, а количество чего-то. Это что-то может выражаться 
при помощи `CyrNoun`, которые в свою очередь можно получить из `CyrNounCollection`.

```cs
// Создаем генератор случайных чисел.
Random rand = new Random();

// Создаем склонятор чисел.
CyrNumber number = new CyrNumber();

{
    // Склоняем случайное число.
    CyrResult result = number.Decline(rand.Next(0, 100));
}

{
    // Склоняем случайное количество рублей.
    // Так же можно использовать классы `CyrNumber.EurCurrency`, `CyrNumber.UsdCurrency` и `CyrNumber.YuanCurrency`.
    // Либо создать свой класс унаследовав его от `CyrNumber.Currency`.
    CyrNumber.Currency currency = new CyrNumber.RurCurrency();
    CyrResult result = number.Decline(rand.Next(0, 100), currency);
}

{
    // Создаем коллекцию всех существительных.
    CyrNounCollection nouns = new CyrNounCollection();

    // Получаем существительное из коллекции используя точное совпадение.
    CyrNoun noun = nouns.Get("компьютер", GetConditionsEnum.Strict);

    // Упаковываем существительное для склонения количества.
    CyrNumber.Item item = new CyrNumber.Item(noun);

    // Склоняем случайное количество компьютеров.
    CyrResult result = number.Decline(rand.Next(0, 100), item);
}

// Выбираем правильное склонение существительного в зависимости от количества.
string name = number.Case(rand.Next(0, 100), "компьютер", "компьютера", "компьютеров");
```

#### Результат склонения

Класс `CyrResult` используется для хранения результатов склонения и содержит всевозможные свойства и методы для удобства доступа.

Список наиболее часто используемых свойств:

| Название        | Синоним        | Тип      | Описание                          |
|-----------------|----------------|----------|-----------------------------------|
| `Nominative`    | `Именительный` | `string` | Именительный, Кто? Что? (есть)    |
| `Genitive`      | `Родительный`  | `string` | Родительный, Кого? Чего? (нет)    |
| `Dative`        | `Дательный`    | `string` | Дательный, Кому? Чему? (дам)      |
| `Accusative`    | `Винительный`  | `string` | Винительный, Кого? Что? (вижу)    |
| `Instrumental`  | `Творительный` | `string` | Творительный, Кем? Чем? (горжусь) |
| `Prepositional` | `Предложный`   | `string` | Предложный, О ком? О чем? (думаю) |

Список наиболее часто используемых методов:

| Название        | Параметры                       | Возвращает                      | Описание                                                       |
|-----------------|---------------------------------|---------------------------------|----------------------------------------------------------------|
| `Get`           | `Cyriller.Model.CasesEnum Case` | `string`                        | Возвращает результат склонения в указанном падеже.             |
| `ToList`        |                                 | `List<string>`                  | Возвращает результат склонения по всем падежам в виде списка.  |
| `ToArray`       |                                 | `string[]`                      | Возвращает результат склонения по всем падежам в виде массива. |
| `ToDictionary`  |                                 | `Dictionary<CasesEnum, string>` | Возвращает результат склонения по всем падежам в виде словаря. |

#### Исключения

Коллекции `CyrNounCollection` и `CyrAdjectiveCollection` выбрасывают `CyrWordNotFoundException` исключение если слово не найдено.

### Демо

Cyriller можно протестировать онлайн - [http://cyriller.2try.ws/](http://cyriller.2try.ws/).

### NuGet пакет

https://www.nuget.org/packages/Miyconst.Cyriller