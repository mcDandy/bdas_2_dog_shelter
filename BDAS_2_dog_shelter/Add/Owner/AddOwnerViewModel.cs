using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace BDAS_2_dog_shelter.Add.Owner
{
    internal class AddOwnerViewModel
    {
        private Tables.Owner d;
        private int? iD;
        private string jmeno;
        private string prijmeni;
        private string telefon;
        private string? email;
        private int? addressID;
        RelayCommand okCommand;

        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, () => jmeno is not null and not "" && prijmeni is not null and not "" && telefon is not null and not "" && addressID != 0);

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.Name = jmeno;
            d.Surname = prijmeni;
            d.Phone = telefon;
            d.Email = email;
            d.AddressID = Adresa.id;
            OkClickFinished?.Invoke();
        }

        public string Name { get => jmeno; set { jmeno = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public string Surname { get => prijmeni; set { prijmeni = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public int? AdressID { get => addressID; set { addressID = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public string Phone { get => telefon; set { telefon = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public string Email { get => email; set { email = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public int? ID { get => iD; set => iD = value; }
        public Tables.Owner Owner => d;
        public Tables.Adress Adresa { get; set; }

        public AddOwnerViewModel(Tables.Owner d)
        {
            this.d = d;
            ID = d.id;
            Name = d.Name;
            Surname = d.Surname;
            AdressID = d.AddressID;
            Phone = d.Phone;
            Email = d.Email;
        }
    }
}