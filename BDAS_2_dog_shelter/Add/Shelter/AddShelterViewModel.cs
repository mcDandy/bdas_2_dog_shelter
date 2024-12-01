using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Xml.Linq;
namespace BDAS_2_dog_shelter.Add.Shelter
{
    internal class AddShelterViewModel
    {
        private Tables.Shelter d;
        private string? email;
        private string name;
        private string telephone;
        private int? iD;
        private int? addressID;
        RelayCommand okCommand;

        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok, () => name is not null and not "" /*&& addressID is not null and not < 0*/ && telephone is not null and not "");

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
           d.Email = email;
            d.Name = name;
            d.Telephone = telephone;
            d.AddressID = addressID;
            d.ID = iD;
            OkClickFinished?.Invoke();
        }

        public string? Email { get => email;  set { email = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public string Name { get => name;  set { name = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public string Telephone { get => telephone;  set => telephone = value; }
        public int? ID { get => iD;  set => iD = value; }
        public int? AddressID { get => addressID;  set => addressID = value; }
        public Tables.Shelter Utulek => d;

        public AddShelterViewModel(Tables.Shelter d)
        {
            this.d = d;
            Name = d.Name;
            Email = d.Email;
            Telephone = d.Telephone;
            ID = d.ID;
            AddressID = d.AddressID;
        }
    }
}