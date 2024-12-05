using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Xml.Linq;
namespace BDAS_2_dog_shelter.Add.Food
{
    internal class AddFoodViewModel
    {
        private Tables.Feed d;

        RelayCommand okCommand;
        private int count;
        private string name;

        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok, () => name is not null and not "" && count>0 /*&& psc is not null and not < 0*/ );

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.Count = Count;
            d.FeedName = Name;
            OkClickFinished?.Invoke();
        }

        public string Name { get => name; set { name = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }

        public Tables.Storage Sklad;
        public List<Tables.Storage> Sklady;


        public int Count { get => count; set => count = value; }
        public Tables.Feed Food => d;

        public AddFoodViewModel(Tables.Feed d, List<Tables.Storage> storages)
        {
            Sklady = storages;
            this.d = d;
            Count = d.Count;
            Name = d.FeedName;
            Sklad = d.Sklad;
        }
    }
}