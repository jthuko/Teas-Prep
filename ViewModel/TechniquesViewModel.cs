﻿
using TeasPrep.Models;
using TeasPrep.Services;
using TeasPrep.StaticMethods;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace TeasPrep.ViewModel
{

    public class TechniquesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<PublicQuestion> publicQuestions;
        public ObservableCollection<PublicQuestion> PublicQuestions
        {
            get { return publicQuestions; }
            set
            {
                if (publicQuestions != value)
                {
                    publicQuestions = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }

        private string selectedValue;
        public string SelectedValue
        {
            get { return selectedValue; }
            set
            {
                if (selectedValue != value)
                {
                    selectedValue = value;
                    OnPropertyChanged();
                }
            }
        }
       

        private readonly AppService appService;

        public TechniquesViewModel(AppService appService)
        {
            this.appService = appService;
            PublicQuestions = new ObservableCollection<PublicQuestion>();
            LoadPublicQuestions();           
        }
        public void ReloadQuestions()
        {
            // Implement logic to reload questions or reset properties
            // For example:
            LoadPublicQuestions();
        }

        private async void LoadPublicQuestions()
        {
            // Check if user settings already exist
            var existingUserSettings = AuthenticationService.GetUserSettings();
            // Retrieve public questions from the QuestionService
            PublicQuestions.Clear();
            IsBusy = true;
            var publicQuestions = await appService.GetPublicQuestionsAsync(5);
            if (existingUserSettings != null && existingUserSettings.IsPremium)
            {
                foreach (var question in publicQuestions)
                {
                    PublicQuestions.Add(question);
                }
                CollectionShuffler.Shuffle(PublicQuestions);
                KeepElements.KeepAtMostNElements(PublicQuestions, 10);
            }
            else
            {
                // Show only the first 20 questions
                var first10Questions = publicQuestions.Take(10).ToList();
                foreach (var question in first10Questions)
                {
                    PublicQuestions.Add(question);
                }
                CollectionShuffler.Shuffle(PublicQuestions);
                KeepElements.KeepAtMostNElements(PublicQuestions, 5);
            }
            IsBusy = false;
        }


        // Implement INotifyPropertyChanged interface for property change notification
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }   
}
