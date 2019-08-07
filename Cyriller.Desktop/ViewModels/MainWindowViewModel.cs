using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Avalonia;
using Avalonia.Input;
using ReactiveUI;
using Cyriller.Desktop.Models;

namespace Cyriller.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        protected string title = "Cyriller Desktop";
        protected bool isNounViewVisible = false;
        protected bool isAdjectiveVisible = false;
        protected bool isNameVisible = false;
        protected bool isAboutVisible = true;
        protected Cursor cursor = Cursor.Default;

        public event EventHandler NounFormOpened;
        public event EventHandler AdjectiveFormOpened;
        public event EventHandler NameFormOpened;

        public string Title
        {
            get => this.title;
            protected set => this.RaiseAndSetIfChanged(ref this.title, value);
        }

        public Cursor Cursor
        {
            get => this.cursor;
            set => this.RaiseAndSetIfChanged(ref this.cursor, value);
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

        public bool IsNameVisible
        {
            get => this.isNameVisible;
            set => this.RaiseAndSetIfChanged(ref this.isNameVisible, value);
        }

        public bool IsAboutVisible
        {
            get => this.isAboutVisible;
            set => this.RaiseAndSetIfChanged(ref this.isAboutVisible, value);
        }

        public CyrCollectionContainer CyrCollectionContainer { get; protected set; }
        public NounViewModel NounViewModel { get; protected set; }
        public AdjectiveViewModel AdjectiveViewModel { get; protected set; }
        public NameViewModel NameViewModel { get; protected set; }

        public MainWindowViewModel(CyrCollectionContainer container)
        {
            this.CyrCollectionContainer = container ?? throw new ArgumentNullException(nameof(container));
            this.CyrCollectionContainer.InitCollectionsInBackground();
        }

        public virtual void MenuItem_About_Click()
        {
            this.HideAll();
            this.IsAboutVisible = true;
        }

        public virtual void MenuItem_Exit_Click()
        {
            Application.Current.Exit();
        }

        public virtual async void MenuItem_Decline_Noun_Click()
        {
            this.Busy();
            this.Cursor = new Cursor(StandardCursorType.Wait);
            this.Title = "Склонение существительного по падежам";
            this.HideAll();
            this.IsNounViewVisible = true;

            if (this.NounViewModel != null)
            {
                this.OnFormOpened(this.NounFormOpened);
                return;
            }

            await this.CyrCollectionContainer.InitOrDefault();

            this.NounViewModel = Program.ServiceProvider.GetService<NounViewModel>();
            this.RaisePropertyChanged(nameof(NounViewModel));
            this.OnFormOpened(this.NounFormOpened);
        }

        public async virtual void MenuItem_Decline_Adjective_Click()
        {
            this.Busy();
            this.Title = "Склонение прилагательного по падежам";
            this.HideAll();
            this.IsAdjectiveVisible = true;

            if (this.AdjectiveViewModel != null)
            {
                this.OnFormOpened(this.AdjectiveFormOpened);
                return;
            }

            await this.CyrCollectionContainer.InitOrDefault();

            this.AdjectiveViewModel = Program.ServiceProvider.GetService<AdjectiveViewModel>();
            this.RaisePropertyChanged(nameof(AdjectiveViewModel));
            this.OnFormOpened(this.AdjectiveFormOpened);
        }

        public virtual void MenuItem_Decline_Name_Click()
        {
            this.Busy();
            this.title = "Склонение личных имен без использования словаря";
            this.HideAll();
            this.IsNameVisible = true;

            if (this.NameViewModel != null)
            {
                this.OnFormOpened(this.NameFormOpened);
                return;
            }

            this.NameViewModel = Program.ServiceProvider.GetService<NameViewModel>();
            this.RaisePropertyChanged(nameof(NameViewModel));
            this.OnFormOpened(this.NameFormOpened);
        }

        protected virtual void HideAll()
        {
            this.IsAboutVisible = false;
            this.IsNounViewVisible = false;
            this.IsAdjectiveVisible = false;
            this.IsNameVisible = false;
        }

        protected virtual void Busy()
        {
            this.Cursor = new Cursor(StandardCursorType.Wait);
        }

        protected virtual void Free()
        {
            this.Cursor = Cursor.Default;
        }

        protected virtual void OnFormOpened(EventHandler @event)
        {
            @event?.Invoke(this, EventArgs.Empty);
            this.Free();
        }
    }
}
