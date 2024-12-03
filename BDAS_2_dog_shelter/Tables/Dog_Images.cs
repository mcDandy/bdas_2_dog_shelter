using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS_2_dog_shelter.Tables
{
    public class Dog_Images : INotifyPropertyChanged, INotifyPropertyChanging
    {
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
        private string file_name;
        public string FileName
        {
            get => file_name;
            set
            {
                if (file_name != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(FileName)));
                    file_name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FileName)));
                }
            }
        }
        public Dog_Images() { event_description = ""; file_name = ""; }
        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
