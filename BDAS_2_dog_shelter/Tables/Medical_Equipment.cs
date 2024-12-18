﻿using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Medical_Equipment : INotifyPropertyChanged, INotifyPropertyChanging
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
        public int Count
        {
            get => count_medical;
            set
            {
                if (count_medical != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Count)));
                    count_medical = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
                }
            }
        }
        public Medical_Equipment() { medical_name = ""; count_medical = 0; }
        public Medical_Equipment(int? id, string nazev, int pocet, int sklad_id)
        {
            this.id = id;
            medical_name = nazev;
            count_medical = pocet;
            _sklad = sklad_id;
        }
        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
        private int _sklad;
        public int SkladID
        {
            get => _sklad;
            set
            {
                if (_sklad != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(SkladID)));
                    _sklad = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SkladID)));
                }
            }
        }
        public Storage Sklad { get; set; }
    }
}
