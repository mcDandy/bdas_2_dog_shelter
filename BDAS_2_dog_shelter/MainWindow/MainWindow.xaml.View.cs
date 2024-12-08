using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using System.Windows.Input;
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
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN)) LoadImages(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN)) LoadLogs(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.TYPES_SELECT)) LoadTypes(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDRAVOTNICKY_MATERIAL_INSERT)) LoadMedical(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.SKLAD_SELECT)) LoadStorages(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HRACKA_SELECT)) LoadHracky(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_SELECT)) LoadAdresses(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_SELECT)) LoadShelters(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PES_SELECT)) LoadDogs(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_SELECT)) LoadHistory(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.REZERVACE_SELECT)) LoadReservations(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KRMIVO_SELECT)) LoadFood(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDRAVOTNICKY_MATERIAL_SELECT)) LoadMedical(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KARANTENA_SELECT)) LoadKarantena(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.MAJITEL_SELECT))  LoadOwner(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PAVILON_SELECT)) LoadPavilon(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PROCEDURA_SELECT)) LoadProcedure(permissions);
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDR_ZAZNAM_SELECT)) LoadMedicalRec(permissions);


            if (Permission.HasAnyOf(permissions, Permissions.ADMIN))
            { //TODO: nějaká lepší prevence úpravy
                Images.CollectionChanged += DogImages_CollectionChanged;

            }
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
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDRAVOTNICKY_MATERIAL_INSERT, Permissions.ZDRAVOTNICKY_MATERIAL_DELETE))
            { //TODO: nějaká lepší prevence úpravy
                Medical_Equipment.CollectionChanged += Medical_CollectionChanged;

            }
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KARANTENA_INSERT, Permissions.KARANTENA_DELETE))
            { //TODO: nějaká lepší prevence úpravy
                Karanteny.CollectionChanged += Karantena_CollectionChanged;

            }
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.MAJITEL_INSERT, Permissions.MAJITEL_DELETE))
            { //TODO: nějaká lepší prevence úpravy
                Owners.CollectionChanged += Owner_CollectionChanged;

            }
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PAVILON_INSERT, Permissions.PAVILON_DELETE))
            { //TODO: nějaká lepší prevence úpravy
                Pavilony.CollectionChanged += Pavilon_CollectionChanged;

            }
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PROCEDURA_INSERT, Permissions.PROCEDURA_DELETE))
            { //TODO: nějaká lepší prevence úpravy
                Procedures.CollectionChanged += Procedure_CollectionChanged;

            }
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDR_ZAZNAM_INSERT, Permissions.ZDR_ZAZNAM_DELETE))
            { //TODO: nějaká lepší prevence úpravy
                MedicalRec.CollectionChanged += MedicalRec_CollectionChanged;

            }
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN)) Typy.CollectionChanged += Typy_CollectionChanged;
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
            Medical_Equipment.CollectionChanged -= Medical_CollectionChanged;
            Medical_Equipment.Clear();
            LoadMedical(permissions);
            Medical_Equipment.CollectionChanged += Medical_CollectionChanged;
            Typy.CollectionChanged -= Typy_CollectionChanged;
            Typy.Clear();
            LoadTypes(permissions);
            Typy.CollectionChanged += Typy_CollectionChanged;
            Karanteny.CollectionChanged -= Karantena_CollectionChanged;
            Karanteny.Clear();
            LoadKarantena(permissions);
            Karanteny.CollectionChanged += Karantena_CollectionChanged;
            Owners.CollectionChanged -= Owner_CollectionChanged;
            Owners.Clear();
            LoadOwner(permissions);
            Owners.CollectionChanged += Owner_CollectionChanged;
            Procedures.CollectionChanged -= Procedure_CollectionChanged;
            Procedures.Clear();
            LoadProcedure(permissions);
            Procedures.CollectionChanged += Procedure_CollectionChanged;
            MedicalRec.CollectionChanged -= MedicalRec_CollectionChanged;
            MedicalRec.Clear();
            LoadMedicalRec(permissions);
            MedicalRec.CollectionChanged += MedicalRec_CollectionChanged;
        }

        public bool AnyDogPerms => Permission.HasAnyOf(permissions, Permissions.PES_SELECT, Permissions.PES_INSERT, Permissions.PES_DELETE, Permissions.PES_UPDATE, Permissions.ADMIN);
        public bool AnyAdressPerms => Permission.HasAnyOf(permissions, Permissions.ADRESA_SELECT, Permissions.ADRESA_INSERT, Permissions.ADRESA_DELETE, Permissions.ADRESA_UPDATE, Permissions.ADMIN);
        public bool AnyUtulekPerms => Permission.HasAnyOf(permissions, Permissions.UTULEK_SELECT, Permissions.UTULEK_INSERT, Permissions.UTULEK_DELETE, Permissions.UTULEK_UPDATE, Permissions.ADMIN);
        public bool AnyHrackaPerms => Permission.HasAnyOf(permissions, Permissions.HRACKA_SELECT, Permissions.HRACKA_INSERT, Permissions.HRACKA_DELETE, Permissions.HRACKA_UPDATE, Permissions.ADMIN);
        public bool AnyKrmivoPerms => Permission.HasAnyOf(permissions, Permissions.KRMIVO_SELECT, Permissions.KRMIVO_INSERT, Permissions.KRMIVO_DELETE, Permissions.KRMIVO_UPDATE, Permissions.ADMIN);
        public bool AnySkladPerms => Permission.HasAnyOf(permissions, Permissions.SKLAD_SELECT, Permissions.SKLAD_INSERT, Permissions.SKLAD_DELETE, Permissions.SKLAD_UPDATE, Permissions.ADMIN);
        public bool AnyDogHistoryPerms => Permission.HasAnyOf(permissions, Permissions.HISTORIE_PSA_SELECT, Permissions.HISTORIE_PSA_INSERT, Permissions.HISTORIE_PSA_DELETE, Permissions.HISTORIE_PSA_UPDATE, Permissions.ADMIN);
        public bool AnyMedicalPerms => Permission.HasAnyOf(permissions, Permissions.ZDRAVOTNICKY_MATERIAL_SELECT, Permissions.ZDRAVOTNICKY_MATERIAL_INSERT, Permissions.ZDRAVOTNICKY_MATERIAL_DELETE, Permissions.ZDRAVOTNICKY_MATERIAL_UPDATE, Permissions.ADMIN);
        public bool AnyKaratenaPerms => Permission.HasAnyOf(permissions, Permissions.KARANTENA_SELECT, Permissions.KARANTENA_INSERT, Permissions.KARANTENA_DELETE, Permissions.KARANTENA_UPDATE, Permissions.ADMIN);
        public bool AnyMajitelPerms => Permission.HasAnyOf(permissions, Permissions.MAJITEL_SELECT, Permissions.MAJITEL_INSERT, Permissions.MAJITEL_DELETE, Permissions.MAJITEL_UPDATE, Permissions.ADMIN);
        public bool AnyPavilonPerms => Permission.HasAnyOf(permissions, Permissions.PAVILON_SELECT, Permissions.PAVILON_INSERT, Permissions.PAVILON_DELETE, Permissions.PAVILON_UPDATE, Permissions.ADMIN);
        public bool AnyMedicalRecPerms => Permission.HasAnyOf(permissions, Permissions.ZDR_ZAZNAM_SELECT, Permissions.ZDR_ZAZNAM_INSERT, Permissions.ZDR_ZAZNAM_DELETE, Permissions.ZDR_ZAZNAM_UPDATE, Permissions.ADMIN);
        public bool AnyKarantenaPerms => Permission.HasAnyOf(permissions, Permissions.KARANTENA_SELECT, Permissions.KARANTENA_INSERT, Permissions.KARANTENA_DELETE, Permissions.KARANTENA_UPDATE, Permissions.ADMIN);
        public bool AnyProceduryPerms => Permission.HasAnyOf(permissions, Permissions.PROCEDURA_SELECT, Permissions.PROCEDURA_INSERT, Permissions.PROCEDURA_DELETE, Permissions.PROCEDURA_UPDATE, Permissions.ADMIN);
        public bool AnyRezervacePerms => Permission.HasAnyOf(permissions, Permissions.REZERVACE_SELECT, Permissions.REZERVACE_INSERT, Permissions.PROCEDURA_DELETE, Permissions.REZERVACE_UPDATE, Permissions.ADMIN);

        public bool isAdmin => Permission.HasAnyOf(permissions, Permissions.ADMIN);
    }
}

