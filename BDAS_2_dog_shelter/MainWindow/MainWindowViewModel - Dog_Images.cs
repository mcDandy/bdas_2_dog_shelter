using BDAS_2_dog_shelter;
using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Add.Dog_Historie;
using BDAS_2_dog_shelter.Add.Dog_Image;
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
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BDAS_2_dog_shelter.MainWindow
{
    internal partial class MainWindowViewModel
    {
        private RelayCommand ImgesadCMD;
        private RelayCommand<object> ImgesrmCMD;
        private RelayCommand<object> ImgesedCMD;
        public ICommand cmdImagesAdd => ImgesadCMD ??= new RelayCommand(CommandImagesAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_INSERT)));
        public ICommand cmdImagesRm => ImgesrmCMD ??= new RelayCommand<object>(CommandImagesRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_DELETE)));
        public ICommand cmdImagesEd => ImgesedCMD ??= new RelayCommand<object>(CommandImagesEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_UPDATE)));

        public ObservableCollection<Dog_Images> Images { get; set; } = new();

        private void CommandImagesEdit(object? obj)
        {
            DogImageAdd da = new(((IEnumerable)obj).Cast<Dog_Images>().First());
            da.ShowDialog();
        }
        private void CommandImagesRemove(object? SelectedShelters)
        {
            if (Permission.HasAnyOf(permissions,Permissions.ADMIN))
            {
                List<Dog_Images> e = new List<Dog_Images>();
                foreach (Dog_Images d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Dog_Images history in e)
                {
                    Images.Remove(history);
                }
            }
        }
        private void CommandImagesAdd()
        {
            DogImageAdd s = new DogImageAdd();
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Images.Add(((AddDogImageViewModel)s.DataContext).Food);
                if (Permission.HasAnyOf(permissions, Permissions.ADMIN)) Images.Last().PropertyChanged += DogImageChanged;
            }
        }
        internal void LoadImages(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "SELECT ID_IMAGE,IMAGE,FILENAME FROM DOG_IMAGES";
                        OracleDataReader v = cmd.ExecuteReader();
                        if (v.HasRows)
                        {
                            while (v.Read())
                            {
                                BitmapDecoder decoder = null;
                                if (!v.IsDBNull(1))
                                {
                                    byte[] data = v.GetOracleBlob(1)?.Value;
                                    MemoryStream ms = new MemoryStream(data);
                                    decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.None, BitmapCacheOption.Default);
                                }
                                Images.Add(new(v.GetInt32(0), decoder.Frames.First(), v.GetString(2)));

                                if (Permission.HasAnyOf(permissions, Permissions.ADMIN)) Images.Last().PropertyChanged += DogImageChanged;
                            }
                        }
                    }
                    catch (Exception ex)//something went wrong
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private async void DogImageChanged(object? sender, PropertyChangedEventArgs e)
        {
            Dog_Images? history = sender as Dog_Images;
            using (OracleCommand cmd = con.CreateCommand())
            {

                await SaveImages(history);
            }
        }

        private async Task SaveImages(Dog_Images history)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "INS_SET.IU_DOG_IMAGES"; // Replace this with your actual stored procedure
                    cmd.Parameters.Add("V_ID_IMAGE", OracleDbType.Decimal, history.id ?? (object)DBNull.Value, ParameterDirection.InputOutput);
                    PngBitmapEncoder pe = new PngBitmapEncoder();
                    pe.Frames.Add(BitmapFrame.Create(history.Image));
                    MemoryStream ms = new MemoryStream();
                    pe.Save(ms);
                    byte[] b = ms.ToArray();
                    cmd.Parameters.Add("V_DOG_IMAGE", OracleDbType.Blob, b, ParameterDirection.Input);
                    cmd.Parameters.Add("V_FILENAME", OracleDbType.Varchar2, history.FileName, ParameterDirection.Input);

                    await cmd.ExecuteNonQueryAsync();
                    history.id = Convert.ToInt32(cmd.Parameters["V_ID_IMAGE"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Images.CollectionChanged -= DogImages_CollectionChanged;
                Images.Clear();
                LoadImages(permissions);
                Historie.CollectionChanged += DogImages_CollectionChanged;

            }
        }
        private async void DogImages_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Dog_Images history in e.NewItems ?? Array.Empty<Dog_Images>())
            {
                await SaveImages(history);
            }

            foreach (Dog_Images history in e.OldItems ?? Array.Empty<Dog_Images>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "DELETE FROM DOG_IMAGES WHERE ID_IMAGE = :ID";
                        cmd.Parameters.Add("ID", OracleDbType.Decimal, history.id, ParameterDirection.Input);
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Images.CollectionChanged -= DogImages_CollectionChanged;
                        Images.Clear();
                        LoadImages(permissions);
                        Images.CollectionChanged += DogImages_CollectionChanged;
                    }
                }
            }
        }
    }

    }