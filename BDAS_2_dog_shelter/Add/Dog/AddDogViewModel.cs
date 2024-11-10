﻿using System;
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
using System.Diagnostics.Metrics;


namespace BDAS_2_dog_shelter.Add.Dog
{
    internal partial class AddDogViewModel : ObservableObject
    {
        public AddDogViewModel(Tables.Dog d) {
            Name = d.Name;
            BodyColor = d.BodyColor;
            Duvod = d.DuvodPrijeti;
            Stav = d.StavPes;
            Age = d.Age;
            int i = 0;
            SelectedUT = Utulek.Select(a => new Tuple<int?, int>(a.Item1, i++)).FirstOrDefault(a => a.Item1 == d.UtulekId).Item2;

        }

        public delegate void OkDogAddEditDone();
        public event OkDogAddEditDone? OkClickFinished;

        private string name;

        public string Name { get => name; set => SetProperty(ref name, value); }

        private string bodycolor;

        public string BodyColor { get => name; set => SetProperty(ref bodycolor, value); }

        private string duvod;

        public string Duvod { get => name; set => SetProperty(ref duvod, value); }

        private string stav;

        public string Stav { get => name; set => SetProperty(ref stav, value); }

        private int age = 0;

        public int Age { get => age; set => SetProperty(ref age, value); }

        public List<Tuple<int?, string>> Utulek {
            get 
            {
                if (ulek == null)
                {
                    ulek = [new(null, "<Žádný>")];

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

        private int selectedUT;

        public int SelectedUT { 
            get => selectedUT;
            set => SetProperty(ref selectedUT, value); 
        }

        private RelayCommand okCommand;
        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok);

        private void Ok()
        {
            OkClickFinished?.Invoke();
        }

        private DateTime? date;

        public DateTime? Date { get => date; set => SetProperty(ref date, value); }
        public BitmapSource Obrazek { get; private set; }
    }
}
