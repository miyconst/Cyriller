using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using ReactiveUI;
using OfficeOpenXml;
using Newtonsoft.Json;
using Cyriller.Desktop.Models;

namespace Cyriller.Desktop.ViewModels
{
    public class PhraseViewModel : DeclineViewModel
    {
        public CyrNounCollection CyrNounCollection { get; protected set; }
        public CyrAdjectiveCollection CyrAdjectiveCollection { get; protected set; }
        public CyrPhrase CyrPhrase { get; protected set; }
        public List<SingleValueDeclineResultRowModel> DeclineResult { get; protected set; }

        public PhraseViewModel(CyrCollectionContainer container, Application application) : base(application)
        {
            this.CyrNounCollection = container?.CyrNounCollection ?? throw new ArgumentNullException(nameof(CyrCollectionContainer.CyrNounCollection));
            this.CyrAdjectiveCollection = container?.CyrAdjectiveCollection ?? throw new ArgumentNullException(nameof(CyrCollectionContainer.CyrAdjectiveCollection));
            this.CyrPhrase = new CyrPhrase(this.CyrNounCollection, this.CyrAdjectiveCollection);
        }

        public void Decline()
        {
            this.InputText = this.inputText?.Trim();

            if (string.IsNullOrEmpty(this.inputText))
            {
                return;
            }

            this.DeclineResult = new List<SingleValueDeclineResultRowModel>();
            this.WordProperties = new List<KeyValuePair<string, string>>();

            this.RaisePropertyChanged(nameof(DeclineResult));
            this.RaisePropertyChanged(nameof(WordProperties));

            CyrResult result;

            try
            {
                result = this.CyrPhrase.Decline(this.inputText, Model.GetConditionsEnum.Similar);
            }
            catch (CyrWordNotFoundException ex)
            {
                this.IsDeclineResultVisible = false;
                this.SearchResultTitle = $"Не удалось распознать слово \"{ex.Word}\"";
                return;
            }

            this.WordProperties.Add(new KeyValuePair<string, string>("Фраза", this.inputText));

            foreach (CyrDeclineCase @case in CyrDeclineCase.GetEnumerable())
            {
                this.DeclineResult.Add(new SingleValueDeclineResultRowModel()
                {
                    CaseName = @case.NameRu,
                    CaseDescription = @case.Description,
                    Value = result.Get(@case.Value)
                });
            }

            this.IsDeclineResultVisible = true;
            this.SearchResultTitle = $"Результат склонения \"{this.inputText}\"";
        }

        protected override void FillExportExcelPackage(ExcelPackage package)
        {
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Фраза");
            int rowIndex = 1;

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

                sheet.Cells[rowIndex, 3].Value = "Значение";
                rowIndex++;
            }

            foreach (SingleValueDeclineResultRowModel row in this.DeclineResult)
            {
                sheet.Cells[rowIndex, 1].Value = row.CaseName;
                sheet.Cells[rowIndex, 2].Value = row.CaseDescription;
                sheet.Cells[rowIndex, 3].Value = row.Value;
                rowIndex++;
            }
        }

        protected override string GetExportJsonString()
        {
            object export = new
            {
                Phrase = this.inputText,
                this.DeclineResult
            };

            string json = JsonConvert.SerializeObject(export, Formatting.Indented);

            return json;
        }
    }
}
