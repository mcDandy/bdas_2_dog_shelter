using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(ID)));
                    id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
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

        public Dog_History()
        {
            date_of_event = DateTime.Now;
            event_description = "";
        }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}