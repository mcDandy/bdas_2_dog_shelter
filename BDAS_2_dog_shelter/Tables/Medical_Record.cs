using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Medical_Record : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public int? id;
        private DateTime date_rec;
        public DateTime DateRec
        {
            get => date_rec;
            set
            {
                if (date_rec != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(DateRec)));
                    date_rec = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateRec)));
                }
            }
        }
        private int? type_proc;
        private DateTime? date;
        private KeyValueUS type;

        public Tables.KeyValueUS Type { get => type;
            set
            {
                if (type != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Type)));
                    type = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type)));
                }
            }
        }

        public int? TypeProcId
        {
            get => type_proc;
            set
            {
                if (type_proc != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(TypeProcId)));
                    type_proc = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TypeProcId)));
                }
            }
        }
        public Medical_Record() { date_rec = DateTime.Now; type_proc = 0; }

        public Medical_Record(int v1, DateTime dateTime, int v2)
        {
            id = v1;
            date_rec = dateTime;
            type_proc = v2;
        }

        public Medical_Record(int? id, DateTime? date, int? type)
        {
            this.id = id;
            this.date = date;
            this.type_proc = type;
        }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    public Medical_Record medRecord { get; set; }
    }
}