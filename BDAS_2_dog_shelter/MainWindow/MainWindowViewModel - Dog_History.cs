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
        public ICommand cmdHistoryAdd => HistoryadCMD ??= new RelayCommand(CommandDogHistoryAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_INSERT)));
        public ICommand cmdHistoryRm => HistoryrmCMD ??= new RelayCommand<object>(CommandPesHistoryRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_DELETE)));
        public ICommand cmdHistoryEd => HistoryedCMD ??= new RelayCommand<object>(CommandPesHistoryEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_UPDATE)));

        public ObservableCollection<Dog_History> Historie { get; set; } = new();

        private void CommandPesHistoryEdit(object? obj)
        {
            if (obj is Dog_History history)
            {
                Dog_Historie_Add editWindow = new Dog_Historie_Add(history);
                editWindow.ShowDialog();
            }
        }

        private void CommandPesHistoryRemove(object? SelectedShelters)
        {
            if ((permissions & (long)Permissions.HISTORIE_PSA_DELETE) > 0)
            {
                if (SelectedShelters is IEnumerable<Dog_History> selectedHistories)
                {
                    foreach (var history in selectedHistories)
                    {
                        Historie.Remove(history);
                    }
                }
            }
        }

        private void CommandDogHistoryAdd()
        {
            Dog_Historie_Add addWindow = new Dog_Historie_Add();
            if (addWindow.ShowDialog() == true)
            {
                Dog_History newHistory = ((AddDogHistorieViewModel)addWindow.DataContext).Historie;
                Historie.Add(newHistory);
                if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_UPDATE))
                {
                    newHistory.PropertyChanged += HistorieChanged;
                }
            }
        }

        private void LoadPesHistory(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_SELECT))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT ID_HISTORIE, DATUM_UDALOSTI, POPIS_UDALOSTI, TYP_UDALOSTI_ID_TYPU, ID_PSA FROM HISTORIE_PSA;";

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Historie.Add(new Dog_History
                            {
                                ID = reader.GetInt32(0),
                                DateOfEvent = reader.GetDateTime(1),
                                EventDescription = reader.GetString(2),
                                TypeId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                DogId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
        }

        private void HistorieChanged(object? sender, PropertyChangedEventArgs e)
        {
            Dog_History? history = sender as Dog_History;
            if (history != null)
            {
                _ = Savehistory(history);
            }
        }

        private async Task Savehistory(Dog_History history)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = "INSERT_OR_UPDATE_HISTORIE_PSA"; // Replace this with your actual stored procedure
                    cmd.Parameters.Add("V_ID_HISTORIE", OracleDbType.Decimal, history.ID ?? (object)DBNull.Value, ParameterDirection.InputOutput);
                    cmd.Parameters.Add("V_DATUM_UDALOSTI", OracleDbType.Date, history.DateOfEvent, ParameterDirection.Input);
                    cmd.Parameters.Add("V_POPIS_UDALOSTI", OracleDbType.Varchar2, history.EventDescription, ParameterDirection.Input);
                    cmd.Parameters.Add("V_TYP_UDALOSTI_ID_TYPU", OracleDbType.Decimal, history.TypeId ?? (object)DBNull.Value, ParameterDirection.Input);
                    cmd.Parameters.Add("ID_PSA", OracleDbType.Decimal, history.DogId ?? (object)DBNull.Value, ParameterDirection.Input);

                    await cmd.ExecuteNonQueryAsync();
                    history.ID = Convert.ToInt32(cmd.Parameters["V_ID_HISTORIE"].Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LoadPesHistory(permissions);
            }
        }

        private async void DogHistory_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Dog_History history in e.NewItems ?? Array.Empty<Dog_History>())
            {
                await Savehistory(history);
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
                        LoadPesHistory(permissions);
                    }
                }
            }
        }
    }
}