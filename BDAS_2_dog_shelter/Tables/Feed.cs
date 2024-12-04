using System;
using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Feed : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Properties for the columns in the KRMIVO table
        public int? IdKrmivo { get; set; }

        private string feedName;
        public string FeedName
        {
            get => feedName;
            set
            {
                if (feedName != value)
                {
                    OnPropertyChanging(nameof(FeedName));
                    feedName = value;
                    OnPropertyChanged(nameof(FeedName));
                }
            }
        }

        private int countFeed;
        public int CountFeed
        {
            get => countFeed;
            set
            {
                if (countFeed != value)
                {
                    OnPropertyChanging(nameof(CountFeed));
                    countFeed = value;
                    OnPropertyChanged(nameof(CountFeed));
                }
            }
        }

        private int idSklad;
        public int IdSklad
        {
            get => idSklad;
            set
            {
                if (idSklad != value)
                {
                    OnPropertyChanging(nameof(IdSklad));
                    idSklad = value;
                    OnPropertyChanged(nameof(IdSklad));
                }
            }
        }

        // Constructor
        public Feed()
        {
            feedName = string.Empty;
            countFeed = 0;
            idSklad = 0; // Assuming a default value for idSklad
        }

        // PropertyChanged and PropertyChanging event support
        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanging(string propertyName)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }
    }
}