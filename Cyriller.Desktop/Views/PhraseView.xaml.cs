using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Cyriller.Desktop.Views
{
    public class PhraseView : UserControl
    {
        public PhraseView()
        {
            this.InitializeComponent();
        }

        public new void Focus()
        {
            base.Focus();
            this.FindControl<TextBox>("txtInputText").Focus();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
