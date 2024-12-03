using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS_2_dog_shelter.Tables
{
    public class Feed : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public int? id;
        private string feed_name;
        public string FeedName
        {
            get => feed_name;
            set
            {
                if (feed_name != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(FeedName)));
                    feed_name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FeedName)));
                }
            }
        }
        private int count_feed;
        public int CountFeed
        {
            get => count_feed;
            set
            {
                if (count_feed != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(CountFeed)));
                    count_feed = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CountFeed)));
                }
            }
        }

        public Feed() { feed_name = ""; count_feed= 0; }
        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
