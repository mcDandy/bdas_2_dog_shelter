using System.ComponentModel;

namespace BDAS_2_dog_shelter
{
    public class Dog : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs("Name"));
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        } 
        private int _age;
        public int Age
        {
            get => _age;
            set
            {
                if (_age != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs("Age"));
                    _age = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Age"));
                }
            }
        }
        public string _body_color;
        public string BodyColor
        {
            get => _body_color;
            set
            {
                if (_body_color != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs("BodyColor"));
                    _body_color = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BodyColor"));
                }
            }
        }
        public Dog() { _name = ""; _age = 0;_body_color = ""; }
        public Dog(string name,int age,string bodycolor) { _name = name;_age = age;_body_color = bodycolor; }


        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}