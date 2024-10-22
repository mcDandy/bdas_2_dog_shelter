using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS_2_dog_shelter.Tables
{
    internal class Hracka : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public readonly int id;
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
        private int _sklad;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

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
    }
}
