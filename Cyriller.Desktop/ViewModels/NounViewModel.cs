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
    public class NounViewModel : ViewModelBase
    {
        private static CyrNounCollection CyrNounCollection { get; set; }
        private static Task LoadCyrNounCollectionTask = null;

        protected bool isManualCaseGenderNumberInput = false;
        protected bool isDeclineResultVisible = false;
        protected string searchResultTitle = null;
        protected string inputText = "кот";

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

        public string InputText
        {
            get => this.inputText;
            set
            {
                this.NounProperties.Clear();
                this.DeclineResult.Clear();

                this.RaiseAndSetIfChanged(ref this.inputText, value);
                this.RaisePropertyChanged(nameof(NounProperties));
                this.RaisePropertyChanged(nameof(DeclineResult));
                this.IsDeclineResultVisible = false;
            }
        }

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

            this.NounProperties.Add(new KeyValuePair<string, string>("Род", new GenderModel(noun.Gender).Name));

            if (noun.WordType != WordTypesEnum.None)
            {
                this.NounProperties.Add(new KeyValuePair<string, string>("Тип слова", new WordTypeModel(noun.WordType).Name));
            }

            this.NounProperties.Add(new KeyValuePair<string, string>("Одушевленность", new AnimateModel(noun.Animate).Name));
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

        public async Task ExportToJson(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);

            if (fi.Exists)
            {
                fi.Delete();
            }

            object export = new
            {
                Word = this.InputText,
                this.NounProperties,
                this.DeclineResult
            };

            string json = JsonConvert.SerializeObject(export, Formatting.Indented);
            StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8);

            await writer.WriteAsync(json);
            writer.Dispose();
        }

        public async Task ExportToExcel(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);

            if (fi.Exists)
            {
                fi.Delete();
            }

            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add(this.inputText);
            int rowIndex = 1;

            sheet.Cells[rowIndex++, 1].Value = this.inputText;
            rowIndex++;

            {
                ExcelRange range = sheet.Cells[rowIndex, 1, rowIndex, 2];
                range.Merge = true;
                range.Value = "Свойства существительного";
                rowIndex++;
            }

            foreach (KeyValuePair<string, string> property in this.NounProperties)
            {
                sheet.Cells[rowIndex, 1].Value = property.Key;
                sheet.Cells[rowIndex, 2].Value = property.Value;
                rowIndex++;
            }

            rowIndex++;

            {
                ExcelRange range = sheet.Cells[rowIndex, 1, rowIndex, 2];
                range.Merge = true;
                range.Value = "Падеж";

                sheet.Cells[rowIndex, 3].Value = "Единственное число";
                sheet.Cells[rowIndex, 4].Value = "Множественное число";
                rowIndex++;
            }

            foreach (NounDeclineResultRowModel row in this.DeclineResult)
            {
                sheet.Cells[rowIndex, 1].Value = row.CaseName;
                sheet.Cells[rowIndex, 2].Value = row.CaseDescription;
                sheet.Cells[rowIndex, 3].Value = row.Singular;
                sheet.Cells[rowIndex, 4].Value = row.Plural;
                rowIndex++;
            }

            await Task.Run(() =>
            {
                sheet.Cells.AutoFitColumns();
                package.SaveAs(fi);
                package.Dispose();
            });
        }
    }
}
