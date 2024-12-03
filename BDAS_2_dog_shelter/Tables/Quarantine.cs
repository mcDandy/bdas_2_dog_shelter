using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Quarantine : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public int? id;
        private string begin_of_date;
        public string BeginOfDate
        {
            get => begin_of_date;
            set
            {
                if (begin_of_date != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(BeginOfDate)));
                    begin_of_date = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BeginOfDate)));
                }
            }
        }
        private string end_of_date;
        public string EndOfDate
        {
            get => end_of_date;
            set
            {
                if (end_of_date != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(EndOfDate)));
                    end_of_date = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndOfDate)));
                }
            }
        }
        public Quarantine() { begin_of_date = ""; end_of_date = ""; }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}