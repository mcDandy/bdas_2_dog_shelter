using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Users : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public int? ID { get; set; }

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
        private string hash;
        public string Hash
        {
            get => hash;
            set
            {
                if (hash != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Hash)));
                    hash = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Hash)));
                }
            }
        }
        private ulong perms;
        public ulong Perms
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
        public Users() { u_name = ""; hash = ""; perms = 0; }

    

        public Users(int? v, string value, string v1, ulong v2)
        {
            this.ID = v;
            u_name = value;
            Hash = v1;
            Perms = v2;
        }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
