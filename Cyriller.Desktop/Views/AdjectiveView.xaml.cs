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

        public new void Focus()
        {
            base.Focus();
            this.FindControl<TextBox>("txtInputText").Focus();
        }

        protected override IControl GetTemplateFocusTarget()
        {
            return this.FindControl<TextBox>("txtInputText");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.FindControl<Button>("btnExportToJson").Click += ButtonExportToJson_Click;
            this.FindControl<Button>("btnExportToExcel").Click += ButtonExportToExcel_Click;
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
