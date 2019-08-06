using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Cyriller.Desktop.ViewModels;

namespace Cyriller.Desktop.Views
{
    public class NounView : UserControl
    {
        public NounView()
        {
            this.InitializeComponent();
        }

        public NounViewModel ViewModel => this.DataContext as NounViewModel;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.PropertyChanged += NounView_PropertyChanged;
            this.FindControl<Button>("btnExportToJson").Click += ButtonExportToJson_Click;
            this.FindControl<Button>("btnExportToExcel").Click += ButtonExportToExcel_Click;
        }

        private void NounView_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == nameof(this.TransformedBounds))
            {
                this.FindControl<TextBox>("txtInputText")?.Focus();
            }
        }

        private async void ButtonExportToJson_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string file = await this.SaveFileDialog("Сохранить результат склонения в JSON", "json", "Файлы JSON");

            if (!string.IsNullOrEmpty(file))
            {
                this.ViewModel?.ExportToJson(file);
            }
        }

        private async void ButtonExportToExcel_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string file = await this.SaveFileDialog("Сохранить результат склонения в Microsoft Excel документ", "xlsx", "Файлы Microsoft Excel");

            if (!string.IsNullOrEmpty(file))
            {
                this.ViewModel?.ExportToExcel(file);
            }
        }
    }
}
