using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Avalonia.Controls;
using ReactiveUI;
using OfficeOpenXml;
using Cyriller.Model;
using Cyriller.Desktop.Models;


namespace Cyriller.Desktop.ViewModels
{
    public class AdjectiveViewModel : DeclineViewModel
    {
        public CyrAdjectiveCollection CyrAdjectiveCollection { get; protected set; }
        public List<AdjectiveDeclineResultRowModel> DeclineResult { get; protected set; }

        public AdjectiveViewModel(CyrCollectionContainer container)
        {
            this.CyrAdjectiveCollection = container?.CyrAdjectiveCollection ?? throw new ArgumentNullException(nameof(CyrCollectionContainer.CyrAdjectiveCollection));
            this.InitDropDowns();
        }

        public List<AnimateModel> Animates { get; protected set; }
        public AnimateModel InputAnimate { get; protected set; }

        protected override void InitDropDowns()
        {
            base.InitDropDowns();

            this.Animates = AnimateModel.GetEnumerable().ToList();
            this.InputAnimate = this.Animates.First();
        }

        public void ButtonSearch_Click()
        {
            if (string.IsNullOrEmpty(this.InputText))
            {
                return;
            }

            CyrAdjective adj = null;
            string foundWord = null;
            GendersEnum foundGender = GendersEnum.Undefined;
            CasesEnum foundCase = CasesEnum.Nominative;
            string foundCaseName = null;
            NumbersEnum foundNumber = NumbersEnum.Singular;
            AnimatesEnum foundAnimate = AnimatesEnum.Animated;

            if (this.IsStrictSearch && !this.IsManualPropertiesInput)
            {
                adj = this.CyrAdjectiveCollection.GetOrDefault(this.InputText, out foundGender, out foundCase, out foundNumber, out foundAnimate);
            }
            else if (!this.IsStrictSearch && !this.IsManualPropertiesInput)
            {
                adj = this.CyrAdjectiveCollection.GetOrDefault(this.InputText, out foundWord, out foundGender, out foundCase, out foundNumber, out foundAnimate);
            }
            else if (this.IsStrictSearch && this.IsManualPropertiesInput)
            {
                foundGender = InputGender.Value;
                foundCase = InputCase.Value;
                foundNumber = InputNumber.Value;
                foundAnimate = InputAnimate.Value;
                adj = this.CyrAdjectiveCollection.GetOrDefault(this.InputText, foundGender, foundCase, foundNumber, foundAnimate);
            }
            else if (!this.IsStrictSearch && this.IsManualPropertiesInput)
            {
                foundGender = InputGender.Value;
                foundCase = InputCase.Value;
                foundNumber = InputNumber.Value;
                foundAnimate = InputAnimate.Value;
                adj = this.CyrAdjectiveCollection.GetOrDefault(this.InputText, out foundWord, foundGender, foundCase, foundNumber, foundAnimate);
            }

            this.DeclineResult = new List<AdjectiveDeclineResultRowModel>();
            this.WordProperties = new List<KeyValuePair<string, string>>();

            this.RaisePropertyChanged(nameof(DeclineResult));
            this.RaisePropertyChanged(nameof(WordProperties));

            if (adj == null)
            {
                this.IsDeclineResultVisible = false;
                this.SearchResultTitle = $"По запросу \"{this.InputText}\" ничего не найдено";
                return;
            }

            Dictionary<KeyValuePair<GendersEnum, AnimatesEnum>, CyrResult> results = new System.Collections.Generic.Dictionary<KeyValuePair<GendersEnum, AnimatesEnum>, CyrResult>();

            foreach (GenderModel gender in this.Genders)
            {
                foreach (AnimateModel animate in this.Animates)
                {
                    KeyValuePair<GendersEnum, AnimatesEnum> key = new KeyValuePair<GendersEnum, AnimatesEnum>(gender.Value, animate.Value);
                    CyrResult value;

                    if (gender.Value == GendersEnum.Undefined)
                    {
                        value = adj.DeclinePlural(animate.Value);
                    }
                    else
                    {
                        value = adj.Decline(gender.Value, animate.Value);
                    }

                    results.Add(key, value);
                }
            }

            foreach (CyrDeclineCase @case in CyrDeclineCase.GetEnumerable())
            {
                if (@case.Value == foundCase)
                {
                    foundCaseName = @case.NameRu;
                }

                AdjectiveDeclineResultRowModel row = new AdjectiveDeclineResultRowModel()
                {
                    CaseName = @case.NameRu,
                    CaseDescription = @case.Description
                };

                row.SingularMasculineAnimate = results[new KeyValuePair<GendersEnum, AnimatesEnum>(GendersEnum.Masculine, AnimatesEnum.Animated)].Get(@case.Value);
                row.SingularFeminineAnimate = results[new KeyValuePair<GendersEnum, AnimatesEnum>(GendersEnum.Feminine, AnimatesEnum.Animated)].Get(@case.Value);
                row.SingularNeuterAnimate = results[new KeyValuePair<GendersEnum, AnimatesEnum>(GendersEnum.Neuter, AnimatesEnum.Animated)].Get(@case.Value);

                row.SingularMasculineInanimate = results[new KeyValuePair<GendersEnum, AnimatesEnum>(GendersEnum.Masculine, AnimatesEnum.Inanimated)].Get(@case.Value);
                row.SingularFeminineInanimate = results[new KeyValuePair<GendersEnum, AnimatesEnum>(GendersEnum.Feminine, AnimatesEnum.Inanimated)].Get(@case.Value);
                row.SingularNeuterInanimate = results[new KeyValuePair<GendersEnum, AnimatesEnum>(GendersEnum.Neuter, AnimatesEnum.Inanimated)].Get(@case.Value);

                row.PluralAnimate = results[new KeyValuePair<GendersEnum, AnimatesEnum>(GendersEnum.Undefined, AnimatesEnum.Animated)].Get(@case.Value);
                row.PluralInanimate = results[new KeyValuePair<GendersEnum, AnimatesEnum>(GendersEnum.Undefined, AnimatesEnum.Inanimated)].Get(@case.Value);

                this.DeclineResult.Add(row);
            }

            this.WordProperties.Add(new KeyValuePair<string, string>("Слово в словаре", foundWord));
            this.WordProperties.Add(new KeyValuePair<string, string>("Род", new GenderModel(foundGender).Name));
            this.WordProperties.Add(new KeyValuePair<string, string>("Падеж", foundCaseName));
            this.WordProperties.Add(new KeyValuePair<string, string>("Число", new NumberModel(foundNumber).Name));
            this.WordProperties.Add(new KeyValuePair<string, string>("Одушевленность", new AnimateModel(foundAnimate).Name));

            this.IsDeclineResultVisible = true;
            this.SearchResultTitle = $"Результат поиска по запросу \"{this.InputText}\"";
        }

        public override Task ExportToExcel(string fileName)
        {
            throw new NotImplementedException();
        }

        public override Task ExportToJson(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
