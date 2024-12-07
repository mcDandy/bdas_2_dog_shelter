using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS_2_dog_shelter.Tables
{
    public class Logs : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string c_user;
        public string CUser
        {
            get => c_user;
            set
            {
                if (c_user != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(CUser)));
                    c_user = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CUser)));
                }
            }
        }
        private DateTime event_time;
        public DateTime EventTime
        {
            get => event_time;
            set
            {
                if (event_time != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(EventTime)));
                    event_time = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EventTime)));
                }
            }
        }
        private string table_name;
        public string TableName
        {
            get => table_name;
            set
            {
                if (table_name != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(TableName)));
                    table_name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TableName)));
                }
            }
        }
        private string _operation;
        public string Operation
        {
            get => _operation;
            set
            {
                if (_operation != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Operation)));
                    _operation = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Operation)));
                }
            }
        }
        private string old_value;
        public string OldValue
        {
            get => old_value;
            set
            {
                if (old_value != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(OldValue)));
                    old_value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OldValue)));
                }
            }
        }
        private string new_value;
        public string NewValue
        {
            get => new_value;
            set
            {
                if (new_value != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(NewValue)));
                    new_value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewValue)));
                }
            }
        }
        public Logs(){ c_user = ""; event_time = DateTime.Now; table_name = ""; _operation = ""; old_value = ""; new_value = ""; }
        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
