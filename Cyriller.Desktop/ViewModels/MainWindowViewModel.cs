using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using ReactiveUI;

namespace Cyriller.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Title { get; protected set; } = "Cyriller Desktop";
        public bool IsNounViewVisible => this.NounViewModel != null;
        public NounViewModel NounViewModel { get; protected set; }

        public virtual void MenuItem_Exit_Click()
        {
            Application.Current.Exit();
        }

        public virtual void MenuItem_Decline_Noun_Click()
        {
            this.Title = "Склонение существительного по падежам";
            this.NounViewModel = new NounViewModel();

            this.RaisePropertyChanged(nameof(Title));
            this.RaisePropertyChanged(nameof(NounViewModel));
            this.RaisePropertyChanged(nameof(IsNounViewVisible));
        }
    }
}
