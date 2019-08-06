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
    public class NounViewModel : DeclineViewModel
    {
        public CyrNounCollection CyrNounCollection { get; protected set; }

        public NounViewModel(CyrCollectionContainer container)
        {
            this.CyrNounCollection = container?.CyrNounCollection ?? throw new ArgumentNullException(nameof(CyrCollectionContainer.CyrNounCollection));
            this.InitDropDowns();
        }

        public override string InputText
        {
            get => base.InputText;
            set
            {
                this.WordProperties?.Clear();
                this.DeclineResult?.Clear();
                this.RaisePropertyChanged(nameof(WordProperties));
                this.RaisePropertyChanged(nameof(DeclineResult));

                base.InputText = value;
            }
        }
        
        public List<NounDeclineResultRowModel> DeclineResult { get; protected set; }

        public void ButtonSearch_Click()
        {
            if (string.IsNullOrEmpty(this.InputText))
            {
                return;
            }

            CyrNoun noun = null;
            string foundWord = null;
            CasesEnum foundCase = CasesEnum.Nominative;
            string foundCaseName = null;
            NumbersEnum foundNumber = NumbersEnum.Singular;

            if (this.IsStrictSearch && !this.IsManualPropertiesInput)
            {
                noun = CyrNounCollection.GetOrDefault(this.InputText, out foundCase, out foundNumber);
            }
            else if (!this.IsStrictSearch && !this.IsManualPropertiesInput)
            {
                noun = CyrNounCollection.GetOrDefault(this.InputText, out foundWord, out foundCase, out foundNumber);
            }
            else if (this.IsStrictSearch && this.IsManualPropertiesInput)
            {
                foundCase = this.InputCase.Value;
                foundNumber = this.InputNumber.Value;
                noun = CyrNounCollection.GetOrDefault(this.InputText, this.InputGender.Value, foundCase, foundNumber);
            }
            else if (!this.IsStrictSearch && this.IsManualPropertiesInput)
            {
                foundCase = this.InputCase.Value;
                foundNumber = this.InputNumber.Value;
                noun = CyrNounCollection.GetOrDefault(this.InputText, out foundWord, this.InputGender.Value, foundCase, foundNumber);
            }

            this.DeclineResult = new List<NounDeclineResultRowModel>();
            this.WordProperties = new List<KeyValuePair<string, string>>();

            this.RaisePropertyChanged(nameof(DeclineResult));
            this.RaisePropertyChanged(nameof(WordProperties));

            if (noun == null)
            {
                this.IsDeclineResultVisible = false;
                this.SearchResultTitle = $"По запросу \"{this.InputText}\" ничего не найдено";
                return;
            }

            CyrResult singular = noun.Decline();
            CyrResult plural = noun.DeclinePlural();

            foreach (CyrDeclineCase @case in CyrDeclineCase.GetEnumerable())
            {
                if (@case.Value == foundCase)
                {
                    foundCaseName = @case.NameRu;
                }

                this.DeclineResult.Add(new NounDeclineResultRowModel()
                {
                    CaseName = @case.NameRu,
                    CaseDescription = @case.Description,
                    Singular = singular.Get(@case.Value),
                    Plural = plural.Get(@case.Value)
                });
            }

            this.WordProperties.Add(new KeyValuePair<string, string>("Слово в словаре", foundWord));
            this.WordProperties.Add(new KeyValuePair<string, string>("Род", new GenderModel(noun.Gender).Name));
            this.WordProperties.Add(new KeyValuePair<string, string>("Падеж", foundCaseName));
            this.WordProperties.Add(new KeyValuePair<string, string>("Число", new NumberModel(foundNumber).Name));
            this.WordProperties.Add(new KeyValuePair<string, string>("Одушевленность", new AnimateModel(noun.Animate).Name));

            if (noun.WordType != WordTypesEnum.None)
            {
                this.WordProperties.Add(new KeyValuePair<string, string>("Тип слова", new WordTypeModel(noun.WordType).Name));
            }
            
            this.IsDeclineResultVisible = true;
            this.SearchResultTitle = $"Результат поиска по запросу \"{this.InputText}\"";
        }

        public override async Task ExportToJson(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);

            if (fi.Exists)
            {
                fi.Delete();
            }

            object export = new
            {
                Word = this.InputText,
                this.WordProperties,
                this.DeclineResult
            };

            string json = JsonConvert.SerializeObject(export, Formatting.Indented);
            StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8);

            await writer.WriteAsync(json);
            writer.Dispose();
        }

        public override async Task ExportToExcel(string fileName)
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

            foreach (KeyValuePair<string, string> property in this.WordProperties)
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
