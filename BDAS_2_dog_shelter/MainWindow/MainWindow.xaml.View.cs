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
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.SKLAD_SELECT)) LoadStorages(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HRACKA_SELECT)) LoadHracky(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_SELECT)) LoadAdresses(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_SELECT)) LoadShelters(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PES_SELECT)) LoadDogs(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_SELECT)) LoadHistory(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.SKLAD_SELECT)) LoadStorages(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.REZERVACE_SELECT)) LoadReservations(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KRMIVO_SELECT)) LoadFood(permissions);

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

            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_INSERT, Permissions.HISTORIE_PSA_DELETE))
            { //TODO: nějaká lepší prevence úpravy
                Historie.CollectionChanged += DogHistory_CollectionChanged;

            }
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HRACKA_INSERT, Permissions.HRACKA_DELETE))
            { //TODO: nějaká lepší prevence úpravy
                Hracky.CollectionChanged += Hracka_CollectionChanged;

            }
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.SKLAD_INSERT, Permissions.SKLAD_DELETE))
            { //TODO: nějaká lepší prevence úpravy
                Storages.CollectionChanged += Sklad_CollectionChanged;

            }
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.REZERVACE_INSERT, Permissions.REZERVACE_DELETE))
            { //TODO: nějaká lepší prevence úpravy
                Rezervace.CollectionChanged += Reservation_CollectionChanged;

            }
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KRMIVO_INSERT, Permissions.KRMIVO_DELETE))
            { //TODO: nějaká lepší prevence úpravy
                Krmiva.CollectionChanged += Food_CollectionChanged;

            }
        }

        RelayCommand rsCMD;
        RelayCommand cmCMD;
        public ICommand cmdRst => rsCMD ??= new RelayCommand(CommandReset);
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
            Storages.CollectionChanged -= Sklad_CollectionChanged;
            Storages.Clear();
            LoadStorages(permissions);
            Storages.CollectionChanged += Sklad_CollectionChanged;
            Hracky.CollectionChanged -= Hracka_CollectionChanged;
            Hracky.Clear();
            LoadHracky(permissions);
            Hracky.CollectionChanged += Hracka_CollectionChanged;
            Rezervace.CollectionChanged -= Reservation_CollectionChanged;
            Rezervace.Clear();
            LoadReservations(permissions);
            Rezervace.CollectionChanged += Reservation_CollectionChanged;
            Krmiva.CollectionChanged -= Food_CollectionChanged;
            Krmiva.Clear();
            LoadFood(permissions);
            Krmiva.CollectionChanged += Food_CollectionChanged;
        }

        public bool AnyDogPerms => Permission.HasAnyOf(permissions, Permissions.PES_SELECT, Permissions.PES_INSERT, Permissions.PES_DELETE, Permissions.PES_UPDATE, Permissions.ADMIN);

    }
}

