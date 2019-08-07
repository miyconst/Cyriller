using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using ReactiveUI;
using OfficeOpenXml;
using Newtonsoft.Json;
using Cyriller.Desktop.Models;

namespace Cyriller.Desktop.ViewModels
{
    public class NumberViewModel : DeclineViewModel
    {
        protected decimal inputValue = 1;
        protected bool isInputNumber = true;
        protected bool isInputAmount = false;
        protected bool isInputQuantity = false;
        protected CyrNumber.Currency inputCurrency;

        public decimal InputValue
        {
            get => this.inputValue;
            set
            {
                this.RaiseAndSetIfChanged(ref this.inputValue, value);
                this.IsDeclineResultVisible = false;
            }
        }

        public bool IsInputNumber
        {
            get => this.isInputNumber;
            set
            {
                this.RaiseAndSetIfChanged(ref this.isInputNumber, value);
                this.IsDeclineResultVisible = false;
            }
        }

        public bool IsInputAmount
        {
            get => this.isInputAmount;
            set
            {
                this.RaiseAndSetIfChanged(ref this.isInputAmount, value);
                this.IsDeclineResultVisible = false;
            }
        }

        public bool IsInputQuantity
        {
            get => this.isInputQuantity;
            set
            {
                this.RaiseAndSetIfChanged(ref this.isInputQuantity, value);
                this.IsDeclineResultVisible = false;
            }
        }

        public CyrNumber.Currency InputCurrency
        {
            get => this.inputCurrency;
            set
            {
                this.RaiseAndSetIfChanged(ref this.inputCurrency, value);
                this.IsDeclineResultVisible = false;
            }
        }

        public CyrNumber CyrNumber { get; protected set; }
        public CyrNounCollection CyrNounCollection { get; protected set; }
        public List<CyrNumber.Currency> Currencies { get; protected set; }
        public List<SingleValueDeclineResultRowModel> DeclineResult { get; protected set; }

        public NumberViewModel(CyrNumber cyrNumber, CyrCollectionContainer container, Application application) : base(application)
        {
            this.CyrNumber = cyrNumber ?? throw new ArgumentNullException(nameof(cyrNumber));
            this.CyrNounCollection = container.CyrNounCollection ?? throw new ArgumentNullException(nameof(CyrCollectionContainer.CyrNounCollection));
            this.InitDropDowns();
        }

        protected override void InitDropDowns()
        {
            base.InitDropDowns();

            this.Currencies = new List<CyrNumber.Currency>()
            {
                new CyrNumber.RurCurrency(),
                new CyrNumber.EurCurrency(),
                new CyrNumber.YuanCurrency(),
                new CyrNumber.UsdCurrency()
            };

            this.inputCurrency = this.Currencies.First();
        }

        public void Decline()
        {
            CyrResult result = null;
            string valueStr = this.inputValue.ToString();

            this.DeclineResult = new List<SingleValueDeclineResultRowModel>();
            this.WordProperties = new List<KeyValuePair<string, string>>();

            this.RaisePropertyChanged(nameof(DeclineResult));
            this.RaisePropertyChanged(nameof(WordProperties));

            this.WordProperties.Add(new KeyValuePair<string, string>("Значение", valueStr));

            if (!this.isInputAmount)
            {
                this.WordProperties.Add(new KeyValuePair<string, string>("Падеж", this.InputGender.Name));
            }

            if (this.isInputAmount)
            {
                result = this.CyrNumber.Decline(this.inputValue, this.inputCurrency);
                this.WordProperties.Add(new KeyValuePair<string, string>("Валюта", this.InputCurrency.Name));
            }
            else if (this.isInputQuantity)
            {
                CyrNoun noun = this.CyrNounCollection.GetOrDefault(this.inputText, this.InputGender.Value, Model.CasesEnum.Nominative, Model.NumbersEnum.Singular);

                if (noun != null)
                {
                    result = this.CyrNumber.Decline(this.inputValue, new CyrNumber.Item(noun));
                    this.WordProperties.Add(new KeyValuePair<string, string>("Счетное слово", noun.Name));
                }
            }

            if (result == null)
            {
                result = this.CyrNumber.Decline(this.inputValue, this.InputGender.Value, Model.AnimatesEnum.Animated);
            }

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
            this.SearchResultTitle = $"Результат склонения \"{valueStr}\"";
        }

        protected override void FillExportExcelPackage(ExcelPackage package)
        {
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add(this.InputNumber.ToString());
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
                Value = this.inputValue,
                ValueProperties = this.WordProperties,
                this.DeclineResult
            };

            string json = JsonConvert.SerializeObject(export, Formatting.Indented);

            return json;
        }
    }
}
