using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Quarantine : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public int? id;
        private DateTime begin_of_date;
        public DateTime BeginOfDate
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
        private DateTime end_of_date;
        public DateTime EndOfDate
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
        public Quarantine() { begin_of_date = DateTime.MinValue; end_of_date = DateTime.MaxValue; }

        public Quarantine(int v, DateTime dateTime1, DateTime dateTime2)
        {
            id = v;
            begin_of_date = dateTime1;
            end_of_date = dateTime2;
        }
        public override string ToString()
        {
            return begin_of_date.ToString()+" "+(end_of_date-begin_of_date).TotalDays +" days";
        }
        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}