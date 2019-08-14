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
    public class NameViewModel : DeclineViewModel
    {
        protected bool isShorten = false;
        protected string inputSurname;
        protected string inputName;
        protected string inputPatronymic;

        public CyrName CyrName { get; protected set; }
        public List<SingleValueDeclineResultRowModel> DeclineResult { get; protected set; }

        public bool IsShorten
        {
            get => this.isShorten;
            set => this.RaiseAndSetIfChanged(ref this.isShorten, value);
        }

        public string InputSurname
        {
            get => this.inputSurname;
            set
            {
                this.RaiseAndSetIfChanged(ref this.inputSurname, value);
                this.IsDeclineResultVisible = false;
            }
        }

        public string InputName
        {
            get => this.inputName;
            set
            {
                this.RaiseAndSetIfChanged(ref this.inputName, value);
                this.IsDeclineResultVisible = false;
            }
        }

        public string InputPatronymic
        {
            get => this.inputPatronymic;
            set
            {
                this.RaiseAndSetIfChanged(ref this.inputPatronymic, value);
                this.IsDeclineResultVisible = false;
            }
        }

        public NameViewModel(CyrName cyrName, Application application) : base(application)
        {
            this.CyrName = cyrName ?? throw new ArgumentNullException(nameof(cyrName));
            this.InitDropDowns();
        }

        public void Decline()
        {
            this.InputText = this.inputText?.Trim();
            this.InputSurname = this.inputSurname?.Trim();
            this.InputName = this.inputName?.Trim();
            this.InputPatronymic = this.inputPatronymic?.Trim();

            if (string.IsNullOrWhiteSpace(this.inputText) && !this.isManualPropertiesInput)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.inputSurname) && string.IsNullOrEmpty(this.inputName) && string.IsNullOrEmpty(this.inputPatronymic) && this.isManualPropertiesInput)
            {
                return;
            }

            this.DeclineResult = new List<SingleValueDeclineResultRowModel>();
            this.WordProperties = new List<KeyValuePair<string, string>>();

            this.RaisePropertyChanged(nameof(DeclineResult));
            this.RaisePropertyChanged(nameof(WordProperties));

            CyrResult result;
            string fullName = this.GetInputFullName();

            if (this.isManualPropertiesInput)
            {
                result = this.CyrName.Decline(this.inputSurname, this.inputName, this.inputPatronymic, gender: this.InputGender.Value, shorten: this.isShorten);

                this.WordProperties.Add(new KeyValuePair<string, string>("Фамилия", this.inputSurname));
                this.WordProperties.Add(new KeyValuePair<string, string>("Имя", this.inputName));
                this.WordProperties.Add(new KeyValuePair<string, string>("Отчество", this.inputPatronymic));
            }
            else
            {
                result = this.CyrName.Decline(this.inputText, gender: this.InputGender.Value, shorten: this.isShorten);
                this.WordProperties.Add(new KeyValuePair<string, string>("Имя", fullName));
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
            this.SearchResultTitle = $"Результат склонения имени \"{fullName}\"";
        }

        protected virtual string[] GetInputNameParts()
        {
            string[] parts = new string[] { this.inputSurname, this.inputName, this.inputPatronymic }
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray();

            return parts;
        }

        protected virtual string GetInputFullName()
        {
            string fullName;

            if (this.isManualPropertiesInput)
            {
                string[] parts = this.GetInputNameParts();
                fullName = string.Join(" ", parts);
            }
            else
            {
                fullName = this.inputText;
            }

            return fullName;
        }

        protected override void FillExportExcelPackage(ExcelPackage package)
        {
            string fullName = this.GetInputFullName();
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add(fullName);
            int rowIndex = 1;

            if (this.isManualPropertiesInput)
            {
                foreach (string part in this.GetInputNameParts())
                {
                    sheet.Cells[rowIndex, 1].Value = part;
                    rowIndex++;
                }
            }
            else
            {
                sheet.Cells[rowIndex, 1].Value = fullName;
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
                Word = this.InputText,
                this.WordProperties,
                this.DeclineResult
            };

            string json = JsonConvert.SerializeObject(export, Formatting.Indented);

            return json;
        }
    }
}
