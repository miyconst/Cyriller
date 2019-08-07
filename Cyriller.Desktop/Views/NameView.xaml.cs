using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Cyriller.Desktop.ViewModels;

namespace Cyriller.Desktop.Views
{
    public class NameView : UserControl
    {
        public NameView()
        {
            this.InitializeComponent();
        }

        public new void Focus()
        {
            base.Focus();

            NameViewModel model = this.DataContext as NameViewModel;

            if (model == null || !model.IsManualPropertiesInput)
            {
                this.FindControl<TextBox>("txtInputText").Focus();
            }
            else
            {
                this.FindControl<TextBox>("txtSurname").Focus();
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
