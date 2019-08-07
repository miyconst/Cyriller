using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using ReactiveUI;
using OfficeOpenXml;
using Cyriller.Model;
using Cyriller.Desktop.Models;
using Cyriller.Desktop.Views;

namespace Cyriller.Desktop.ViewModels
{
    public abstract class DeclineViewModel : ViewModelBase
    {
        protected bool isManualPropertiesInput = false;
        protected bool isDeclineResultVisible = false;
        protected string searchResultTitle = null;
        protected string inputText;

        public Application Application { get; protected set; }
        public IClipboard Clipboard => Application.Clipboard;

        public DeclineViewModel(Application application)
        {
            this.Application = application ?? throw new ArgumentNullException(nameof(application));
        }

        #region DropDown options.
        public List<GenderModel> Genders { get; protected set; }
        public List<CyrDeclineCase> Cases { get; protected set; }
        public List<NumberModel> Numbers { get; protected set; }
        public List<KeyValuePair<string, string>> WordProperties { get; protected set; }

        protected virtual void InitDropDowns()
        {
            this.Genders = GenderModel
                .GetEnumerable()
                .OrderBy(x => x.Value == GendersEnum.Undefined ? int.MaxValue : (int)x.Value)
                .ToList();

            this.Cases = CyrDeclineCase
                .GetEnumerable()
                .ToList();

            this.Numbers = NumberModel
                .GetEnumerable()
                .ToList();

            this.InputGender = this.Genders.First();
            this.InputCase = this.Cases.First();
            this.InputNumber = this.Numbers.First();
        }
        #endregion

        #region Search input values.
        public bool IsStrictSearch { get; set; }

        public virtual string InputText
        {
            get => this.inputText;
            set
            {
                this.RaiseAndSetIfChanged(ref this.inputText, value);
                this.IsDeclineResultVisible = false;
            }
        }

        public GenderModel InputGender { get; set; }
        public CyrDeclineCase InputCase { get; set; }
        public NumberModel InputNumber { get; set; }
        #endregion

        public bool IsDeclineResultVisible
        {
            get => this.isDeclineResultVisible;
            set => this.RaiseAndSetIfChanged(ref this.isDeclineResultVisible, value);
        }

        public bool IsManualPropertiesInput
        {
            get => this.isManualPropertiesInput;
            set
            {
                this.RaiseAndSetIfChanged(ref this.isManualPropertiesInput, value);
                this.IsDeclineResultVisible = false;
            }
        }

        public string SearchResultTitle
        {
            get => this.searchResultTitle;
            set => this.RaiseAndSetIfChanged(ref this.searchResultTitle, value);
        }

        #region Export methods.
        protected abstract string GetExportJsonString();
        protected abstract void FillExportExcelPackage(ExcelPackage package);

        public virtual async void ExportToClipboard()
        {
            string json = this.GetExportJsonString();
            await this.Clipboard.SetTextAsync(json);
        }

        public virtual async void ExportToJson()
        {
            string fileName = await this.Application.MainWindow.SaveFileDialog("Сохранить результат склонения в JSON", "json", "Файлы JSON");

            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            FileInfo fi = new FileInfo(fileName);

            if (fi.Exists)
            {
                fi.Delete();
            }

            string json = this.GetExportJsonString();
            StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8);

            await writer.WriteAsync(json);
            writer.Dispose();
        }

        public virtual async void ExportToExcel()
        {
            string fileName = await this.Application.MainWindow.SaveFileDialog("Сохранить результат склонения в Microsoft Excel документ", "xlsx", "Файлы Microsoft Excel");

            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            FileInfo fi = new FileInfo(fileName);

            if (fi.Exists)
            {
                fi.Delete();
            }

            ExcelPackage package = new ExcelPackage();

            this.FillExportExcelPackage(package);

            await Task.Run(() =>
            {
                foreach (ExcelWorksheet sheet in package.Workbook.Worksheets)
                {
                    sheet.Cells.AutoFitColumns();
                }

                package.SaveAs(fi);
                package.Dispose();
            });
        }
        #endregion
    }
}
