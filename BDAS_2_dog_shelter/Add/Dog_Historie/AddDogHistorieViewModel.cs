using System;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Threading.Tasks;
using BDAS_2_dog_shelter.Tables;
using System.Reflection.PortableExecutable;

namespace BDAS_2_dog_shelter.Add.Dog_Historie
{
    internal class AddDogHistorieViewModel
    {
        private Dog_History historyEntry; // Instance historie psa

        // Příkaz pro potvrzení
        private RelayCommand okCommand;

        public ICommand OkCommand => okCommand ??= new RelayCommand(async () => await OkAsync(), CanExecuteOk);

        public delegate void OkHistorieAddEditDone();
        public event OkHistorieAddEditDone? OkClickFinished;

        // Asynchronní metoda pro vykonání příkazu
        private async Task OkAsync()
        {
            // Vyvolání události pro potvrzení vytvoření nebo úpravy historie psa
            OkClickFinished?.Invoke();
            Historie.EventDescription = EventDescription;
            Historie.DogId = SelectedPes.ID;
            // Uložení změn (pokud je potřeba, můžete implementovat asynchronní logiku pro uložení)
            // await SaveHistoryAsync(historyEntry);
        }

        // Podmínka pro aktivaci příkazu
        private bool CanExecuteOk()
        {
            return !string.IsNullOrWhiteSpace(historyEntry.EventDescription) && historyEntry.DateOfEvent != default && historyEntry.EventDescription is not null;
        }
        private string ed;
        public string EventDescription { get => ed; set { if (value != ed) { ed = value; okCommand.NotifyCanExecuteChanged(); } } }
        // Instance historie pro binding
        public Dog_History Historie => historyEntry;

        public Tables.Dog SelectedPes { get; private set; }

        // Konstruktor pro inicializaci ViewModelu
        public AddDogHistorieViewModel(Dog_History entry, List<Tables.Dog> dogs)
        {
            historyEntry = entry ?? throw new ArgumentNullException(nameof(entry)); // Ověření, že entry není null

            // Zaregistrujte změny v historických datech
            historyEntry.PropertyChanged += (s, e) =>
            {
                // Pokud došlo k změní v `Dog_History`, můžete provést další akce (např. aktualizaci UI)
            };
        }
    }
}