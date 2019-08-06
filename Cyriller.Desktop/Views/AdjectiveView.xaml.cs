using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Cyriller.Desktop.Views
{
    public class AdjectiveView : UserControl
    {
        public AdjectiveView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.PropertyChanged += AdjectiveView_PropertyChanged;
            this.FindControl<Button>("btnExportToJson").Click += ButtonExportToJson_Click;
            this.FindControl<Button>("btnExportToExcel").Click += ButtonExportToExcel_Click;
        }

        private void AdjectiveView_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == nameof(this.TransformedBounds))
            {
                this.FindControl<TextBox>("txtInputText")?.Focus();
            }
        }

        private async void ButtonExportToJson_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void ButtonExportToExcel_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
