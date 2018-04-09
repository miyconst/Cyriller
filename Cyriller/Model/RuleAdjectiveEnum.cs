namespace Cyriller.Model
{
    /// <summary>
    /// Варианты изменения окончания для прилагательного мужского рода в именительном падеже 
    /// (+ неодушевлённое мужского рода в винительном падеже).
    /// Привязаны к номеру поля в файле adjective-rules.
    /// См. CyrAdjective.Decline и CyrAdjective.DeclinePlural
    /// </summary>
    public enum RuleAdjectiveEnum
    {
        /// <summary>Прилагательное мужского рода в родительном падеже</summary>
        MasculineGenitive = 0,
        /// <summary>Прилагательное мужского рода в дательном падеже</summary>
        MasculineDative = 1,
        /// <summary>Прилагательное мужского рода одушевлённое в винительном падеже</summary>
        MasculineAnimated = 2,
        /// <summary>Прилагательное мужского рода в творительном падеже</summary>
        MasculineInstrumental = 3,
        /// <summary>Прилагательное мужского рода в предложном падеже</summary>
        MasculinePrepositional = 4,
        /// <summary>Прилагательное женского рода в именительном падеже</summary>
        FeminineNominative = 5,
        /// <summary>Прилагательное женского рода в родительном падеже</summary>
        FeminineGenitive = 6,
        /// <summary>Прилагательное женского рода в дательном падеже</summary>
        FeminineDative = 7,
        /// <summary>Прилагательное женского рода в винительном падеже</summary>
        FeminineAccusative = 8,
        /// <summary>Прилагательное женского рода в творительном падеже</summary>
        FeminineInstrumental = 9,
        /// <summary>Прилагательное женского рода в предложном падеже</summary>
        FemininePrepositional = 10,
        /// <summary>Прилагательное среднего рода в именительном падеже</summary>
        NeuterNominative = 11,
        /// <summary>Прилагательное среднего рода в родительном падеже</summary>
        NeuterGenitive = 12,
        /// <summary>Прилагательное среднего рода в дательном падеже</summary>
        NeuterDative = 13,
        /// <summary>Прилагательное среднего рода в винительном падеже</summary>
        NeuterAccusative = 14,
        /// <summary>Прилагательное среднего рода в творительном падеже</summary>
        NeuterInstrumental = 15,
        /// <summary>Прилагательное среднего рода в предложном падеже</summary>
        NeuterPrepositional = 16,
        /// <summary>Прилагательное множественного числа в именительном падеже + неодушевлённое в винительном падеже</summary>
        PluralNominative = 17,
        /// <summary>Прилагательное множественного числа в родительном падеже</summary>
        PluralGenitive = 18,
        /// <summary>Прилагательное множественного числа в дательном падеже</summary>
        PluralDative = 19,
        /// <summary>Прилагательное множественного числа одушевлённое в винительном падеже</summary>
        PluralAccusative = 21,
        /// <summary>Прилагательное множественного числа в творительном падеже</summary>
        PluralInstrumental = 20,
        /// <summary>Прилагательное множественного числа в предложном падеже</summary>
        PluralPrepositional = 21,
        /// <summary>Получение прилагательного мужского рода из прилагательного женского рода</summary>
        FromFeminine = 22,
        /// <summary>Получение прилагательного мужского рода из прилагательного среднего рода</summary>
        FromNeuter = 23,
    }
}
