using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class KeyValueUS : INotifyPropertyChanged
    {
        private ulong? perms = null;
        private int? id = null;
        private string nazev;

        public KeyValueUS(int? value, string nazev)
        {
            Id = value;
            Nazev = nazev;
        }

        public string Nazev { get => nazev; set { if (nazev != value) { nazev = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nazev))); } } }
        public int? Id { get => id; set => id = value; }
        public ulong? Perms { get => perms; set { if (perms != value) { perms = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Perms))); } } }

        public event PropertyChangedEventHandler? PropertyChanged;

        public override string ToString()
        {
            return Nazev;
        }
    }
}