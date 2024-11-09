using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDAS_2_dog_shelter.Tables;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using static BDAS_2_dog_shelter.Secrets;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BDAS_2_dog_shelter.Add.Dog
{
    internal partial class AddDogViewModel : ObservableObject
    {
        public AddDogViewModel() {
        }

        private string name;

        public string Name { get => name; set => SetProperty(ref name, value); }

        private string bodycolor;

        public string BodyColor { get => name; set => SetProperty(ref name, value); }

        private string duvod;

        public string Duvod { get => name; set => SetProperty(ref name, value); }

        private string stav;

        public string Stav { get => name; set => SetProperty(ref name, value); }

        private int age = 0;

        public string Age { get => name; set => SetProperty(ref name, value); }

        public Tables.Dog dog { get => new Tables.Dog(Name,Age,BodyColor,Date,Duvod,Stav,Utulek.FirstOrDefault(a=>a.Item2==SelectedUT).Item1,Obrazek)} 

        public List<Tuple<int?, string>> Utulek {
            get 
            {
                if (ulek == null)
                {
                    ulek = new List<Tuple<int?, string>>();
                    ulek.Add(new(null, "<Žádný>"));



                    OracleConnection con = new OracleConnection(ConnectionString);
                    if (con.State == System.Data.ConnectionState.Closed) con.Open();
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = "select id_utulek,nazev from utulek";
                            OracleDataReader v = cmd.ExecuteReader();
                            if (v.HasRows)
                            {
                                while (v.Read())
                                {
                                    ulek.Add(new(v.GetInt32(0), v.GetString(1)));
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
        private List<Tuple<int?, string>> ulek;

        private string selectedUT;

        public string SelectedUT { 
            get => selectedUT;
            set => SetProperty(ref selectedUT, value); 
        }

        private RelayCommand okCommand;
        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok);

        private void Ok()
        {
            
        }

        private DateTime? date;

        public DateTime? Date { get => date; set => SetProperty(ref date, value); }
        public BitmapSource Obrazek { get; private set; }
    }
}
