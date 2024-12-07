using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Procedure : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public int? id;
        private string proc_name;
        public string ProcName
        {
            get => proc_name;
            set
            {
                if (proc_name != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(ProcName)));
                    proc_name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProcName)));
                }
            }
        }
        private string descr_name;
        public string DescrName
        {
            get => descr_name;
            set
            {
                if (descr_name != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(DescrName)));
                    descr_name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DescrName)));
                }
            }
        }
        private int? zdrZaznam;
        public int? ZdrZaznam
        {
            get => zdrZaznam;
            set
            {
                if (zdrZaznam != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(ZdrZaznam)));
                    zdrZaznam = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZdrZaznam)));
                }
            }
        }
        public Tables.Medical_Record record { get; set; }
        public Procedure() { proc_name = ""; descr_name = ""; }

        public Procedure(int v1, string v2, string v3, int v4)
        {
            id = v1;
            ProcName = v2;
            descr_name = v3;
            ZdrZaznam = v4;
        }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
