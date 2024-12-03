using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS_2_dog_shelter.Tables
{
    public class Pavilion : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public int? id;
        private string pav_name;
        public string PavName
        {
            get => pav_name;
            set
            {
                if (pav_name != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(PavName)));
                    pav_name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PavName)));
                }
            }
        }
        private int capacity_pav;
        public int CapacityPav
        {
            get => capacity_pav;
            set
            {
                if (capacity_pav != value)
                {

                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(CapacityPav)));
                    capacity_pav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CapacityPav)));
                }

            }
        }
        public Pavilion() { pav_name = ""; capacity_pav = 0; }
        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
   

}
