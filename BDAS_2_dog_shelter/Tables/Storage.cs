using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Storage : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public int? id;
        private int _capacity;
        public int Capacity
        {
            get => _capacity;
            set
            {
                if (_capacity != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Capacity)));
                    _capacity = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Capacity)));
                }
            }
        }
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Name)));
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }
        private string _type;
        public string Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Type)));
                    _type = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type)));
                }
            }
        }
        public Storage() { _capacity = 0; _name = ""; _type = ""; }

        public Storage(int id, int capacity, string name, string? v4)
        {
            this.id = id;
            this.Capacity = capacity;
            this.Name = name;
            this.Type = v4??"";
        }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}