using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BDAS_2_dog_shelter.Add.Dog_Historie
{
    internal class AddDogHistoryViewModel
    {
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
            Historie.EventDescription = EventDescription;
            Historie.DogId = SelectedPes.ID;
            Historie.Typ = Typ.Nazev; 
            Historie.Pes = SelectedPes;
            OkClickFinished?.Invoke();
        }

        private string ed;
        private Dog_History historyEntry;
        public KeyValueUS Typ { get; set; }


        public string EventDescription { get => ed; set { if (value != ed) { ed = value; okCommand?.NotifyCanExecuteChanged(); } } }
        // Instance historie pro binding
        public Dog_History Historie => historyEntry;

        public Tables.Dog SelectedPes { get; set; }
        public List<KeyValueUS> Typy { get; set; }

        public Dog_History DogHistory => historyEntry;

        public List<Tables.Dog> Dogs { get; set; }

        public AddDogHistoryViewModel(Dog_History d, List<Tables.Dog> dogs, List<KeyValueUS> Types)
        {
            this.Dogs = dogs;
            id = d.DogId;
            DateOfEvent = d.DateOfEvent;
            EventDescription = d.EventDescription;
            SelectedPes = d.Pes;
            Typ = Types.Where(x => x.Id == d.TypeId).FirstOrDefault();
            Typy = Types;
            historyEntry = d;
        }
    }
}