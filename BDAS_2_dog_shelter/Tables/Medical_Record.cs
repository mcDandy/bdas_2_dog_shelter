using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS_2_dog_shelter.Tables
{
    public class Medical_Record : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public int? id;
        private string date_rec;
        public string DateRec
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
        private int type_proc;
        public int TypeProc
        {
            get => type_proc;
            set
            {
                if (type_proc != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(TypeProc)));
                    type_proc = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TypeProc)));
                }
            }
        }
        public Medical_Record() { date_rec = ""; type_proc = 0; }
        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
