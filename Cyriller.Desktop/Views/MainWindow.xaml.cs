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
    public class MainWindow : Window
    {
        protected int FocusTimeout { get; set; } = 1;

        public MainWindow(MainWindowViewModel dataContext)
        {
            InitializeComponent();
            this.DataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));

            dataContext.NounFormOpened += DataContext_NounFormOpened;
            dataContext.AdjectiveFormOpened += DataContext_AdjectiveFormOpened;
            dataContext.NameFormOpened += DataContext_NameFormOpened;
        }

        private async void DataContext_NameFormOpened(object sender, EventArgs e)
        {
            // This delay is needed to focus element after UI is updated.
            await Task.Delay(this.FocusTimeout);
            this.Find<NameView>("ucName").Focus();
        }

        private async void DataContext_AdjectiveFormOpened(object sender, EventArgs e)
        {
            // This delay is needed to focus element after UI is updated.
            await Task.Delay(this.FocusTimeout);
            this.Find<AdjectiveView>("ucAdjective").Focus();
        }

        private async void DataContext_NounFormOpened(object sender, EventArgs e)
        {
            // This delay is needed to focus element after UI is updated.
            await Task.Delay(this.FocusTimeout);
            this.Find<NounView>("ucNoun").Focus();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}