using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BDAS_2_dog_shelter.Add.feed
{
    internal class AddFeedViewModel : INotifyPropertyChanged
    {
        // Property for the Feed object being edited/added
        public Feed Feed { get; private set; }

        // Constructor for adding a new Feed
        public AddFeedViewModel()
        {
            Feed = new Feed(); // Initialize a new Feed
        }

        // Constructor for editing an existing Feed
        public AddFeedViewModel(Feed feed)
        {
            Feed = feed; // Set the existing Feed
        }

        // Commands for saving and canceling
        private RelayCommand saveCommand;
        public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(SaveFeed);

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand => cancelCommand ??= new RelayCommand(Cancel);

        // Method to save the Feed entry
        private void SaveFeed()
        {
            if (ValidateFeed())
            {
                // Logic to save the feed to the database can go here
                // For example, notifying the MainWindowViewModel to refresh data, etc.
                MessageBox.Show("Feed saved successfully!");
            }
            else
            {
                MessageBox.Show("Please fill in all required fields.");
            }
        }

        // Method to validate input data
        private bool ValidateFeed()
        {
            return !string.IsNullOrWhiteSpace(Feed.FeedName) && Feed.CountFeed >= 0 && Feed.IdSklad > 0;
        }

        // Method to handle cancel action
        private void Cancel()
        {
            // Logic to close the dialog/window can go here
            MessageBox.Show("Operation canceled.");
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
