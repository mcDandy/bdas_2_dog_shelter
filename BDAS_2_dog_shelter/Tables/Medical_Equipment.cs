using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS_2_dog_shelter.Tables
{
    public class Medical_Equpment : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public int? id;
        private string medical_name;
        public string MedicalName
        {
            get => medical_name;
            set
            {
                if (medical_name != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(MedicalName)));
                    medical_name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MedicalName)));
                }
            }
        }
        private int count_medical;
        public int CountMedical
        {
            get => count_medical;
            set
            {
                if (count_medical != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(CountMedical)));
                    count_medical = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CountMedical)));
                }
            }
        }
        public Medical_Equpment() { medical_name = ""; count_medical = 0; }
        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
