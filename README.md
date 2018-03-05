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

#### Существительное

Склонение существительных выполняется при помощи класса `CyrNounCollection`. 
При создании коллекции, весь словарь существительных загружается в память,
поэтому не рекомендуется создавать много коллекций, 
лучше использовать один экземпляр для всех операций.

```cs
// Создаем коллекцию всех существительных.
CyrNounCollection collection = new CyrNounCollection();

// Получаем существительное из коллекции используя точное совпадение.
CyrNoun strict = collection.Get("компьютер", GetConditionsEnum.Strict);

// Склоняем существительное в единственном числе.
CyrResult singular = strict.Decline();

// Склоняем существительное в множественном числе.
CyrResult plural = strict.DeclinePlural();

// Получаем существительное из коллекции включая похожие слова.
// Найдет "ёж".
CyrNoun similar = collection.Get("еж", GetConditionsEnum.Similar);

// Склоняем существительное в единственном числе.
singular = noun.Decline();

// Склоняем существительное в множественном числе.
plural = noun.DeclinePlural();
```

#### Прилагательное

Склонение прилагательных выполняется при помощи класса `CyrAdjectiveCollection`.
При создании коллекции, весь словарь прилагательных загружается в память,
поэтому не рекомендуется создавать много коллекций, 
лучше использовать один экземпляр для всех операций.

```cs
// Создаем коллекцию всех прилагательных.
CyrAdjectiveCollection collection = new CyrAdjectiveCollection();

// Получаем прилагательное в мужском роде из коллекции используя точное совпадение.
CyrAdjective adjective = collection.Get("быстрый", GetConditionsEnum.Strict, GendersEnum.Masculine);

// Склоняем прилагательное в единственном числе для одушевленных предметов.
CyrResult singular = adjective.Decline(AnimatesEnum.Animated);

// Склоняем прилагательное в множественном числе для неодушевленных предметов.
CyrResult plural = adjective.DeclinePlural(AnimatesEnum.Inanimated);
```

#### Фраза

При помощи класса `CyrPhrase` можно склонять словосочетания из существительных и прилагательных.

```cs
// Создаем коллекцию всех существительных.
CyrNounCollection nouns = new CyrNounCollection();

// Создаем коллекцию всех прилагательных.
CyrAdjectiveCollection adjectives = new CyrAdjectiveCollection();

// Создаем фразу с использование созданных коллекций.
CyrPhrase phrase = new CyrPhrase(nouns, adjectives);

// Склоняем словосочетание "быстрый компьютер" в единственном числе используя точное совпадение при поиске слов.
CyrResult singular = phrase.Decline("быстрый компьютер", GetConditionsEnum.Strict);
```

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
