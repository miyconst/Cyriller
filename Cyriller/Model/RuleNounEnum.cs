namespace Cyriller.Model
{
    /// <summary>
    /// Варианты изменения окончания для существительных в именительном падеже.
    /// Привязаны к номеру поля в файле noun-rules.
    /// См. CyrNoun.Decline и CyrNoun.DeclinePlural
    /// </summary>
    public enum RuleNounEnum
    {
        /// <summary>Существительное в единственном числе в родительном падеже</summary>
        SingularGenitive = 0,
        /// <summary>Существительное в единственном числе в дательном падеже</summary>
        SingularDative = 1,
        /// <summary>Существительное в единственном числе одушевлённое в винительном падеже</summary>
        SingularAnimated = 2,
        /// <summary>Существительное в единственном числе в творительном падеже</summary>
        SingularInstrumental = 3,
        /// <summary>Существительное в единственном числе в предложном падеже</summary>
        SingularPrepositional = 4,
        /// <summary>Существительное во множественном числе в именительном падеже</summary>
        PluralNominative = 5,
        /// <summary>Существительное во множественном числе в родительном падеже</summary>
        PluralGenitive = 6,
        /// <summary>Существительное во множественном числе в дательном падеже</summary>
        PluralDative = 7,
        /// <summary>Существительное во множественном числе в винительном падеже</summary>
        PluralAccusative = 8,
        /// <summary>Существительное во множественном числе в творительном падеже</summary>
        PluralInstrumental = 9,
        /// <summary>Существительное во множественном числе в предложном падеже</summary>
        PluralPrepositional = 10,
    }
}
