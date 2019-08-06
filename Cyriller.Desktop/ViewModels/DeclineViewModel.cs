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
    public abstract class DeclineViewModel : ViewModelBase
    {
        protected bool isManualPropertiesInput = false;
        protected bool isDeclineResultVisible = false;
        protected string searchResultTitle = null;
        protected string inputText;

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
            set => this.RaiseAndSetIfChanged(ref this.isManualPropertiesInput, value);
        }

        public string SearchResultTitle
        {
            get => this.searchResultTitle;
            set => this.RaiseAndSetIfChanged(ref this.searchResultTitle, value);
        }

        #region Export methods.
        public abstract Task ExportToJson(string fileName);

        public abstract Task ExportToExcel(string fileName);
        #endregion
    }
}
