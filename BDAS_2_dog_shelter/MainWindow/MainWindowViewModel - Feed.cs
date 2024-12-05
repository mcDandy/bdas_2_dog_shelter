using BDAS_2_dog_shelter;
using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Add.Dog_Historie;
using BDAS_2_dog_shelter.Add.Food;
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
        private RelayCommand feedAddCmd;
        private RelayCommand<object> feedEditCmd;
        private RelayCommand<object> feedRmCmd;

        public ICommand CommandFeedAdd => feedAddCmd ??= new RelayCommand(AddFeed, () => Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KRMIVO_INSERT));
        public ICommand CommandFeedEdit => feedEditCmd ??= new RelayCommand<object>(EditFeed, (p) => (p is Feed && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KRMIVO_UPDATE)));
        public ICommand CommandFeedRemove => feedRmCmd ??= new RelayCommand<object>(RemoveFeed, (p) => (p is Feed && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KRMIVO_DELETE)));

        public ObservableCollection<Feed> Feeds { get; set; } = new ObservableCollection<Feed>();

        // Load feeds from the database
        private void LoadFeeds(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KRMIVO_SELECT))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT ID_KRMIVO, NAZEV, POCET, ID_SKLAD FROM KRMIVO;";

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Feeds.Add(new Feed
                            {
                                IdKrmivo = reader.GetInt32(0),
                                FeedName = reader.GetString(1),
                                Count = reader.GetInt32(2),
                                IdSklad = reader.GetInt32(3)
                            });
                        }
                    }
                }
            }
        }

        private void AddFeed()
        {
            FeedAdd addWindow = new FeedAdd(new Feed(),Storages.ToList()); // Assuming you have a Feed_Add window similar to Dog_Historie_Add
            if (addWindow.ShowDialog() == true)
            {
                Feed newFeed = ((AddFoodViewModel)addWindow.DataContext).Food; // Assuming your Feed_Add has a corresponding ViewModel
                Feeds.Add(newFeed);
            }
        }

        private void EditFeed(object obj)
        {
            FeedAdd editWindow = new(((IEnumerable)obj).Cast<Feed>().First(), Storages.ToList());
                editWindow.ShowDialog();
            
        }

        private void RemoveFeed(object selectedFeed)
        {
            if ((permissions & (long)Permissions.KRMIVO_DELETE) > 0)
            {
                if (selectedFeed is Feed feedToRemove)
                {
                    Feeds.Remove(feedToRemove);
                    // Execute delete command in the database, if necessary
                    DeleteFeedFromDatabase(feedToRemove);
                }
            }
        }

        private async void DeleteFeedFromDatabase(Feed feed)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            using (OracleCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM KRMIVO WHERE ID_KRMIVO = :ID";
                cmd.Parameters.Add("ID", OracleDbType.Decimal, feed.IdKrmivo, ParameterDirection.Input);
                try
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    LoadFeeds(permissions); // Reload feed records in case of error
                }
            }
        }

    }
}