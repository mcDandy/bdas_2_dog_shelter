using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using static BDAS_2_dog_shelter.Secrets;

namespace BDAS_2_dog_shelter.Add.Shelter
{
    internal partial class AddShelterViewModel
    {
        private Tables.Shelter d;
        private string? email;
        private string name;
        private string telephone;
        private int? iD;
        private int? addressID;
        RelayCommand okCommand;

        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok, () => true);

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
           d.Email = email;
            d.Name = name;
            d.Telephone = telephone;
           // d.AddressID = Adresa.id;
            d.id = iD;
            d.AddressID = addressID;
            OkClickFinished?.Invoke();
        }

        public string? Email { get => email;  set { email = value; /*if (okCommand is not null) okCommand.NotifyCanExecuteChanged();*/ } }
        public string Name { get => name;  set { name = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public string Telephone { get => telephone; set { telephone = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public int? ID { get => iD;  set => iD = value; }
        public int? AdressID { get => addressID; set { addressID = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public Tables.Shelter Utulek => d;

        public List<Tables.Adress> Adresses
        {
            get
            {
                if (ulek == null)
                {
                    ulek = [];

                    OracleConnection con = new OracleConnection(ConnectionString);
                    if (con.State == System.Data.ConnectionState.Closed) con.Open();
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = "select id_adresa,ulice,mesto,psc,cislopopisne from adresa";
                            OracleDataReader v = cmd.ExecuteReader();
                            if (v.HasRows)
                            {
                                while (v.Read())
                                {
                                    ulek.Add(
                                        new(
                                    v.IsDBNull(0) ? null : v.GetInt32(0),
                                    v.GetString(1),
                                    v.GetString(2),
                                    v.IsDBNull(3) ? null : v.GetString(3),
                                    int.Parse(v.GetString(4)
                                    )
                                ));
                                }
                            }
                        }
                        catch (Exception ex)//something went wrong
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                }
                return ulek;
            }
        }

        public Tables.Adress Adresa { get; set; }

        private List<Tables.Adress> ulek;

        public AddShelterViewModel(Tables.Shelter d)
        {
            this.d = d;
            Name = d.Name;
            Email = d.Email;
            Telephone = d.Telephone;
            ID = d.id;
            AdressID = d.AddressID;
            
        }

        [GeneratedRegex("[+]?[0-9]+")]
        private static partial Regex PhoneRegex();
    }
}