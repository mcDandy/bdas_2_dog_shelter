using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using static BDAS_2_dog_shelter.Secrets;

namespace BDAS_2_dog_shelter.Add.Users
{
    internal partial class AddUsersViewModel
    {
        private Tables.Users d;
        private string name;
        private string hash;
        private ulong perms;
        RelayCommand okCommand;

        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok, () => name is not null and not "" &&
        name is not null and not "" &&
        hash is not null && HashRegex().IsMatch(hash));

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.Perms = perms;
            d.Hash = hash;
            d.Uname = name;
            OkClickFinished?.Invoke();
        }

        public ulong Perms { get => perms;  set { perms = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public string Name { get => name;  set { name = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public string Hash { get => hash; set { hash = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public Tables.Users Uzivatel => d;

        
        public AddUsersViewModel(Tables.Users d)
        {
            this.d = d;
            Perms = d.Perms;
            Name = d.Uname;
            Hash = d.Hash;
        }

        [GeneratedRegex("[0-9A-F]{64}")]
        private static partial Regex HashRegex();
    }
}