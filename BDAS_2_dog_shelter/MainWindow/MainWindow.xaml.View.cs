﻿using BDAS_2_dog_shelter;
using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static BDAS_2_dog_shelter.Secrets;


namespace BDAS_2_dog_shelter.MainWindow
{
    internal partial class MainWindowViewModel
    {
        private ulong permissions;
        private OracleConnection con;


        public MainWindowViewModel(ulong permissions)
        {
            this.permissions = permissions;
            con = new OracleConnection(ConnectionString);
            con.Open();
            con.BeginTransaction();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PES_SELECT)) LoadDogs(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_SELECT)) LoadShelters(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_SELECT)) LoadAdresses(permissions);

            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PES_INSERT, Permissions.PES_DELETE))
            { //TODO: nějaká lepší prevence úpravy
                Dogs.CollectionChanged += Dogs_CollectionChanged;

            }
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_INSERT, Permissions.UTULEK_DELETE))
            { //TODO: nějaká lepší prevence úpravy
                Shelters.CollectionChanged += Utulek_CollectionChanged;

            }
            
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_INSERT, Permissions.ADRESA_DELETE))
            { //TODO: nějaká lepší prevence úpravy
                Adresses.CollectionChanged += Adress_CollectionChanged;

            }
        }

        RelayCommand rsCMD;
        RelayCommand cmCMD;
        public ICommand cmdRst => rsCMD ??= new RelayCommand(CommandReset, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PES_DELETE, Permissions.PES_INSERT, Permissions.PES_UPDATE)));
        public ICommand cmdCom => cmCMD ??= new RelayCommand(CommandCommit);

        private void CommandCommit()
        {
            con.Commit();
        }
        private void CommandReset()
        {
            con.Rollback();
            Dogs.CollectionChanged -= Dogs_CollectionChanged;
            Dogs.Clear();
            LoadDogs(permissions);
            Dogs.CollectionChanged += Dogs_CollectionChanged;
            Shelters.CollectionChanged -= Utulek_CollectionChanged;
            Shelters.Clear();
            LoadShelters(permissions);
            Shelters.CollectionChanged += Utulek_CollectionChanged;
            Adresses.CollectionChanged -= Adress_CollectionChanged;
            Adresses.Clear();
            LoadAdresses(permissions);
            Adresses.CollectionChanged += Adress_CollectionChanged;
        }
    }
}

