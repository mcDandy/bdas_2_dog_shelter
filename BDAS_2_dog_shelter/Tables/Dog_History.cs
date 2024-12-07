using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Dog_History : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int? id;
        public int? ID
        {
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                }
            }
        }

        private DateTime date_of_event;
        public DateTime DateOfEvent
        {
            get => date_of_event;
            set
            {
                if (date_of_event != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(DateOfEvent)));
                    date_of_event = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateOfEvent)));
                }
            }
        }

        private string event_description;
        public string EventDescription
        {
            get => event_description;
            set
            {
                if (event_description != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(EventDescription)));
                    event_description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EventDescription)));
                }
            }
        }

        private int? typid;
        public int? TypeId
        {
            get => typid;
            set
            {
                if (typid != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(TypeId)));
                    typid = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TypeId)));
                }
            }
        }

        private int? dogId;
        public int? DogId
        {
            get => dogId;
            set
            {
                if (dogId != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(DogId)));
                    dogId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DogId)));
                }
            }
        }
        public Dog Pes { get; set; }
        public string Typ {  get; set; }

        public Dog_History()
        {
            date_of_event = DateTime.Now;
            event_description = "";
        }

        public Dog_History(int v1, DateTime dateTime, string v2, int v3, int v4)
        {
            id = v1;
            date_of_event = dateTime;
            event_description = v2;
            typid = v3;
            dogId = v4;
        }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}