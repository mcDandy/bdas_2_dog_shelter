using BDAS_2_dog_shelter;
using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Add.Dog_Historie;
using BDAS_2_dog_shelter.Add.Hracka;
using BDAS_2_dog_shelter.Add.Shelter;
using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace BDAS_2_dog_shelter.MainWindow
{
    internal partial class MainWindowViewModel
    {
        private RelayCommand HistoryadCMD;
        private RelayCommand<object> HistoryrmCMD;
        private RelayCommand<object> HistoryedCMD;
        public ICommand cmdHistoryAdd => HistoryadCMD ??= new RelayCommand(CommandHistoryAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_INSERT)));
        public ICommand cmdHistoryRm => HistoryrmCMD ??= new RelayCommand<object>(CommandHistoryRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_DELETE)));
        public ICommand cmdHistoryEd => HistoryedCMD ??= new RelayCommand<object>(CommandHistoryEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_UPDATE)));

        public ObservableCollection<Dog_History> Historie { get; set; } = new();

        private void CommandHistoryEdit(object? obj)
        {
            Dog_Historie_Add da = new(((IEnumerable)obj).Cast<Dog_History>().First(), Dogs.ToList(),Typy.ToList());
            da.ShowDialog();
        }
        private void CommandHistoryRemove(object? SelectedShelters)
        {
            if ((permissions & (long)Permissions.HISTORIE_PSA_DELETE) > 0)
            {
                List<Dog_History> e = new List<Dog_History>();
                foreach (Dog_History d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Dog_History history in e)
                {
                    Historie.Remove(history);
                }
            }
        }
        private void CommandHistoryAdd()
        {
            Dog_Historie_Add s = new Dog_Historie_Add(Dogs.ToList(),Typy.ToList());
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Historie.Add(((AddDogHistoryViewModel)s.DataContext).Historie);
                if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_UPDATE)) Historie.Last().PropertyChanged += HistoryChanged;
            }
        }
        internal void LoadHistory(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_SELECT))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "SELECT ID_HISTORIE, DATUM_UDALOSTI, POPIS_UDALOSTI, TYP_UDALOSTI_ID_TYPU, ID_PSA FROM HISTORIE_PSA";
                        OracleDataReader v = cmd.ExecuteReader();
                        if (v.HasRows)
                        {
                            while (v.Read())
                            {
                                Historie.Add(new(v.GetInt32(0), v.GetDateTime(1), v.GetString(2), v.GetInt32(3), v.GetInt32(4)));

                                if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_UPDATE)) Dogs.Last().PropertyChanged += DogChanged;
                            }
                            List<Dog_History> DogForest = Historie.Select<Dog_History, Dog_History>
                                (a => {
                                    a.Pes = Dogs.Where(d => d.ID == a.DogId).FirstOrDefault();
                                   
                                    return a;
                                }).ToList();
                            Historie.CollectionChanged -= DogHistory_CollectionChanged;
                            Historie.Clear();
                            foreach (var item in DogForest)
                            {
                                Historie.Add(item);
                            }
                            Historie.CollectionChanged += DogHistory_CollectionChanged;
                        }
                    }
                    catch (Exception ex)//something went wrong
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private async void HistoryChanged(object? sender, PropertyChangedEventArgs e)
        {
            Dog_History? history = sender as Dog_History;
            using (OracleCommand cmd = con.CreateCommand())
            {

                await SaveHistory(history);
            }
        }

        private async Task SaveHistory(Dog_History history)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = "INS_SET.IU_HISTORIE_PSA"; // Replace this with your actual stored procedure
                    cmd.Parameters.Add("V_ID_HISTORIE", OracleDbType.Decimal, history.ID ?? (object)DBNull.Value, ParameterDirection.InputOutput);
                    cmd.Parameters.Add("V_DATUM_UDALOSTI", OracleDbType.Date, history.DateOfEvent, ParameterDirection.Input);
                    cmd.Parameters.Add("V_POPIS_UDALOSTI", OracleDbType.Varchar2, history.EventDescription, ParameterDirection.Input);
                    cmd.Parameters.Add("V_TYP_UDALOSTI_ID_TYPU", OracleDbType.Decimal, history.TypeId ?? (object)DBNull.Value, ParameterDirection.Input);
                    cmd.Parameters.Add("V_ID_PSA", OracleDbType.Decimal, history.DogId ?? (object)DBNull.Value, ParameterDirection.Input);

                    await cmd.ExecuteNonQueryAsync();
                    history.ID = Convert.ToInt32(cmd.Parameters["V_ID_HISTORIE"].Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Historie.CollectionChanged -= DogHistory_CollectionChanged;
                LoadHistory(permissions);
                Historie.CollectionChanged += DogHistory_CollectionChanged;

            }
        }
        private async void DogHistory_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Dog_History history in e.NewItems ?? Array.Empty<Dog_History>())
            {
                await SaveHistory(history);
            }

            foreach (Dog_History history in e.OldItems ?? Array.Empty<Dog_History>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "DELETE FROM HISTORIE_PSA WHERE ID_HISTORIE = :ID";
                        cmd.Parameters.Add("ID", OracleDbType.Decimal, history.ID, ParameterDirection.Input);
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        LoadHistory(permissions);
                    }
                }
            }
        }
    }

    }