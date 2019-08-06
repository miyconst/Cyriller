using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using ReactiveUI;
using Cyriller.Desktop.Models;

namespace Cyriller.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        protected string title = "Cyriller Desktop";
        protected bool isNounViewVisible = false;
        protected bool isAdjectiveVisible = false;

        public string Title
        {
            get => this.title;
            protected set => this.RaiseAndSetIfChanged(ref this.title, value);
        }

        public bool IsNounViewVisible
        {
            get => this.isNounViewVisible;
            set => this.RaiseAndSetIfChanged(ref this.isNounViewVisible, value);
        }

        public bool IsAdjectiveVisible
        {
            get => this.isAdjectiveVisible;
            set => this.RaiseAndSetIfChanged(ref this.isAdjectiveVisible, value);
        }

        public CyrCollectionContainer CyrCollectionContainer { get; protected set; }
        public NounViewModel NounViewModel { get; protected set; }
        public AdjectiveViewModel AdjectiveViewModel { get; protected set; }

        public MainWindowViewModel()
        {
            this.CyrCollectionContainer = new CyrCollectionContainer();
            this.CyrCollectionContainer.InitCollectionsInBackground();
        }

        public virtual void MenuItem_Exit_Click()
        {
            Application.Current.Exit();
        }

        public virtual async void MenuItem_Decline_Noun_Click()
        {
            this.Title = "Склонение существительного по падежам";
            this.IsNounViewVisible = true;
            this.IsAdjectiveVisible = false;

            if (this.NounViewModel != null)
            {
                return;
            }

            await this.CyrCollectionContainer.InitOrDefault();

            this.NounViewModel = new NounViewModel(this.CyrCollectionContainer);
            this.RaisePropertyChanged(nameof(NounViewModel));
        }

        public async virtual void MenuItem_Decline_Adjective_Click()
        {
            this.Title = "Склонение прилагательного по падежам";
            this.IsNounViewVisible = false;
            this.IsAdjectiveVisible = true;

            if (this.AdjectiveViewModel != null)
            {
                return;
            }

            await this.CyrCollectionContainer.InitOrDefault();
            
            this.AdjectiveViewModel = new AdjectiveViewModel(this.CyrCollectionContainer);
            this.RaisePropertyChanged(nameof(AdjectiveViewModel));
        }
    }
}
