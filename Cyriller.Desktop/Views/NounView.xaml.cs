using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Cyriller.Desktop.Views
{
    public class NounView : UserControl
    {
        public NounView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.PropertyChanged += NounView_PropertyChanged;
        }

        private void NounView_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == nameof(this.TransformedBounds))
            {
                this.FindControl<TextBox>("txtInputText")?.Focus();
            }
        }
    }
}
