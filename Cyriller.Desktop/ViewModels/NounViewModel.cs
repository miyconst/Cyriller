using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Cyriller.Model;
using Cyriller.Desktop.Models;

namespace Cyriller.Desktop.ViewModels
{
    public class NounViewModel : ViewModelBase
    {
        private static CyrNounCollection CyrNounCollection { get; set; }
        private static Task LoadCyrNounCollectionTask = null;

        protected bool isManualCaseGenderNumberInput = false;
        protected bool isDeclineResultVisible = false;
        protected string searchResultTitle = null;

        public NounViewModel()
        {
            InitCyrNounCollection().GetAwaiter();

            this.Genders = new List<KeyValuePair<GendersEnum, string>>()
            {
                new KeyValuePair<GendersEnum, string>(GendersEnum.Masculine, "Мужской род"),
                new KeyValuePair<GendersEnum, string>(GendersEnum.Feminine, "Женский род"),
                new KeyValuePair<GendersEnum, string>(GendersEnum.Neuter, "Нейтральный род"),
                new KeyValuePair<GendersEnum, string>(GendersEnum.Undefined, "Неопределенный род")
            };

            this.Cases = CyrDeclineCase.List.ToList();

            this.Numbers = new List<KeyValuePair<NumbersEnum, string>>()
            {
                new KeyValuePair<NumbersEnum, string>(NumbersEnum.Singular, "Единственное число"),
                new KeyValuePair<NumbersEnum, string>(NumbersEnum.Plural, "Множественное число")
            };

            this.InputGender = this.Genders.First();
            this.InputCase = this.Cases.First();
            this.InputNumber = this.Numbers.First();
        }

        public string InputText { get; set; }
        public KeyValuePair<GendersEnum, string> InputGender { get; set; }
        public CyrDeclineCase InputCase { get; set; }
        public KeyValuePair<NumbersEnum, string> InputNumber { get; set; }
        public bool IsStrictSearch { get; set; }
        public bool IsDeclineResultVisible
        {
            get => this.isDeclineResultVisible;
            set => this.RaiseAndSetIfChanged(ref this.isDeclineResultVisible, value);
        }
        public bool IsManualCaseGenderNumberInput
        {
            get => this.isManualCaseGenderNumberInput;
            set => this.RaiseAndSetIfChanged(ref this.isManualCaseGenderNumberInput, value);
        }
        public string SearchResultTitle
        {
            get => this.searchResultTitle;
            set => this.RaiseAndSetIfChanged(ref this.searchResultTitle, value);
        }
        public List<NounDeclineResultRowModel> DeclineResult { get; protected set; }
        public List<KeyValuePair<string, string>> NounProperties { get; protected set; }
        public List<KeyValuePair<GendersEnum, string>> Genders { get; protected set; }
        public List<CyrDeclineCase> Cases { get; protected set; }
        public List<KeyValuePair<NumbersEnum, string>> Numbers { get; protected set; }

        public async Task ButtonSearch_Click()
        {
            if (string.IsNullOrEmpty(this.InputText))
            {
                return;
            }

            await InitCyrNounCollection();

            CyrNoun noun = null;
            string foundWord = null;

            if (this.IsStrictSearch && !this.IsManualCaseGenderNumberInput)
            {
                noun = CyrNounCollection.GetOrDefault(this.InputText, out CasesEnum _, out NumbersEnum _);
            }
            else if (!this.IsStrictSearch && !this.IsManualCaseGenderNumberInput)
            {
                noun = CyrNounCollection.GetOrDefault(this.InputText, out foundWord, out CasesEnum _, out NumbersEnum _);
            }
            else if (this.IsStrictSearch && this.IsManualCaseGenderNumberInput)
            {
                noun = CyrNounCollection.GetOrDefault(this.InputText, this.InputGender.Key, this.InputCase.Value, this.InputNumber.Key);
            }
            else if (!this.IsStrictSearch && this.IsManualCaseGenderNumberInput)
            {
                noun = CyrNounCollection.GetOrDefault(this.InputText, out foundWord, this.InputGender.Key, this.InputCase.Value, this.InputNumber.Key);
            }

            this.DeclineResult = new List<NounDeclineResultRowModel>();
            this.NounProperties = new List<KeyValuePair<string, string>>();

            this.RaisePropertyChanged(nameof(DeclineResult));
            this.RaisePropertyChanged(nameof(NounProperties));

            if (noun == null)
            {
                this.IsDeclineResultVisible = false;
                this.SearchResultTitle = $"По запросу \"{this.InputText}\" ничего не найдено";
                return;
            }

            CyrResult singular = noun.Decline();
            CyrResult plural = noun.DeclinePlural();

            foreach (CyrDeclineCase @case in CyrDeclineCase.List)
            {
                this.DeclineResult.Add(new NounDeclineResultRowModel()
                {
                    CaseName = @case.NameRu,
                    CaseDescription = @case.Description,
                    Singular = singular.Get(@case.Value),
                    Plural = plural.Get(@case.Value)
                });
            }

            if (!string.IsNullOrWhiteSpace(foundWord) && !string.Equals(foundWord, noun.Name, StringComparison.InvariantCulture))
            {
                this.NounProperties.Add(new KeyValuePair<string, string>("Слово в словаре", foundWord));
            }

            this.NounProperties.Add(new KeyValuePair<string, string>("Род", noun.Gender.ToString()));

            if (noun.WordType != 0)
            {
                this.NounProperties.Add(new KeyValuePair<string, string>("Тип слова", noun.WordType.ToString()));
            }

            this.NounProperties.Add(new KeyValuePair<string, string>("Одушевленность", noun.IsAnimated ? "Одушевленный предмет" : "Неодушевленный предмет"));
            this.IsDeclineResultVisible = true;
            this.SearchResultTitle = $"Результат поиска по запросу \"{this.InputText}\"";
        }

        public async Task InitCyrNounCollection()
        {
            if (CyrNounCollection != null)
            {
                return;
            }

            if (LoadCyrNounCollectionTask == null)
            {
                LoadCyrNounCollectionTask = Task.Run(() =>
                {
                    CyrNounCollection = new CyrNounCollection();
                    LoadCyrNounCollectionTask = null;
                });
            }

            await LoadCyrNounCollectionTask;
        }
    }
}
