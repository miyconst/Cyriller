using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Cyriller.Desktop.Views
{
    public class NumberView : UserControl
    {
        public NumberView()
        {
            this.InitializeComponent();
        }

        public new void Focus()
        {
            base.Focus();
            this.FindControl<NumericUpDown>("txtInputValue").Focus();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
