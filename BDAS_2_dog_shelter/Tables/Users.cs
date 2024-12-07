using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Users : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string u_name;
        public string Uname
        {
            get => u_name;
            set
            {
                if (u_name != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Uname)));
                    u_name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Uname)));
                }
            }
        }
        private string password;
        public string Password
        {
            get => password;
            set
            {
                if (password != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Password)));
                    password = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
                }
            }
        }
        private int perms;
        public int Perms
        {
            get => perms;
            set
            {
                if (perms != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Perms)));
                    perms = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Perms)));
                }
            }
        }
        public Users() { u_name = ""; password = ""; perms = 0; }
        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
