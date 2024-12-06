using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Xml.Linq;

namespace BDAS_2_dog_shelter.Add.Dog_Historie
{
    internal class AddDogHistoryViewModel
    {
        private Tables.Dog_History d;
        private int? id;

        public DateTime DateOfEvent { get; set; }

        RelayCommand okCommand;

        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok, () => { return true; });

        public delegate void OkHistorieAddEditDone();
        public event OkHistorieAddEditDone? OkClickFinished;

        private void Ok()
        {
            Historie.DateOfEvent = DateOfEvent;
            Historie.TypeId = Typ.Id;
            OkClickFinished?.Invoke();
            Historie.EventDescription = EventDescription;
            Historie.DogId = SelectedPes.ID;
        }

        // Podmínka pro aktivaci příkazu
        private bool CanExecuteOk()
        {
            return !string.IsNullOrWhiteSpace(historyEntry.EventDescription) && historyEntry.DateOfEvent != default && historyEntry.EventDescription is not null;
        }
        private string ed;
        private Dog_History historyEntry;
        private Tables.KeyValueUS Typ;


        public string EventDescription { get => ed; set { if (value != ed) { ed = value; okCommand.NotifyCanExecuteChanged(); } } }
        // Instance historie pro binding
        public Dog_History Historie => historyEntry;

        public Tables.Dog SelectedPes { get; set; }
        public List<KeyValueUS> Typy { get; set; }

        public Tables.Dog_History DogHistory => d;

        private List<Tables.Dog> dogs;

        public AddDogHistoryViewModel(Tables.Dog_History d, List<Tables.Dog> dogs, List<KeyValueUS> Types)
        {
            this.dogs = dogs;
            this.d = d;
            id = d.DogId;
            DateOfEvent = d.DateOfEvent;
            SelectedPes = d.Pes;
            this.Typy = Types;
        }
    }
}