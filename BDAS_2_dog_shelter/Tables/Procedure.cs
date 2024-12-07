using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private string type_proc;
        public string TypeProc
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
        public Procedure() { proc_name = ""; descr_name = "";type_proc = ""; }
        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
