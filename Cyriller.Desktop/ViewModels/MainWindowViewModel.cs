using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Avalonia;
using ReactiveUI;
using Cyriller.Desktop.Models;
using Cyriller.Desktop.Views;

namespace Cyriller.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        protected string title = "Cyriller Desktop";
        protected bool isNounViewVisible = false;
        protected bool isAdjectiveVisible = false;

        public event EventHandler NounFormOpened;
        public event EventHandler AdjectiveFormOpened;

        public string Title
        {
            get => this.title;
            protected set => this.RaiseAndSetIfChanged(ref this.title, value);
        }

        public bool IsNounViewVisible
        {
            get => this.isNounViewVisible;
            set
            {
                if (!value)
                {
                    this.NounViewModel = null;
                    this.RaisePropertyChanged(nameof(this.NounViewModel));
                }

                this.RaiseAndSetIfChanged(ref this.isNounViewVisible, value);
            }
        }

        public bool IsAdjectiveVisible
        {
            get => this.isAdjectiveVisible;
            set => this.RaiseAndSetIfChanged(ref this.isAdjectiveVisible, value);
        }

        public CyrCollectionContainer CyrCollectionContainer { get; protected set; }
        public NounViewModel NounViewModel { get; protected set; }
        public AdjectiveViewModel AdjectiveViewModel { get; protected set; }

        public MainWindowViewModel(CyrCollectionContainer container)
        {
            this.CyrCollectionContainer = container ?? throw new ArgumentNullException(nameof(container));
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

            this.NounViewModel = Program.ServiceProvider.GetService<NounViewModel>();
            this.RaisePropertyChanged(nameof(NounViewModel));
            this.NounFormOpened?.Invoke(this, EventArgs.Empty);
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

            this.AdjectiveViewModel = Program.ServiceProvider.GetService<AdjectiveViewModel>();
            this.RaisePropertyChanged(nameof(AdjectiveViewModel));
            this.AdjectiveFormOpened?.Invoke(this, EventArgs.Empty);
        }
    }
}
