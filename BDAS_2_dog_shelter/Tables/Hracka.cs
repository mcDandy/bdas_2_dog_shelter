﻿using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Hracka : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public int? id;
        private string _nazev;
        public string Nazev
        {
            get => _nazev;
            set
            {
                if (_nazev != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Nazev)));
                    _nazev = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nazev)));
                }
            }
        }
        private int _pocet;
        public int Pocet
        {
            get => _pocet;
            set
            {
                if (_pocet != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Pocet)));
                    _pocet = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pocet)));
                }
            }
        }
        private int? _sklad;

        public Hracka(int? id, string nazev, int pocet, int sklad_id)
        {
            this.id = id;
            _nazev = nazev;
            _pocet = pocet;
            _sklad = sklad_id;
        }

        public Hracka()
        {
            _nazev = "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        public int? SkladID
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
