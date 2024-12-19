using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace BDAS_2_dog_shelter.Add.Storage
{

    internal class AddStorageViewModel
    {


        private Tables.Storage d;
        private string stype;
        private int cap;
        private string name;
        private int? iD;
        RelayCommand okCommand;
        private SkladTypy? st;

        // Command for confirming the selection
        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok, () => sType is not null && sType != "" && Capacity > 0 && Name.Length>0);

        // Delegate and event for notifying when the action is done
        public delegate void OkStorageAddEditDone();
        public event OkStorageAddEditDone? OkClickFinished;

        // Method that is called when the Ok command is executed
        private void Ok()
        {
            d.Type = sType;
            d.Capacity = Capacity;
            d.Name = Name;
            d.id = ID;
            OkClickFinished?.Invoke();
        }

        // Property for storage type, will be bound to ComboBox
        public string? sType
        {
            get => st switch
            {
                SkladTypy.Zdr_Material => "z",
                SkladTypy.Hracky => "h",
                SkladTypy.Krmivo => "k",
                _=>null
            };
            set { st = value switch
            {
                "z" => SkladTypy.Zdr_Material,
                "h" => SkladTypy.Hracky,
                "k" => SkladTypy.Krmivo,
                _ => null
            };
            okCommand?.NotifyCanExecuteChanged();
            } }
            public SkladTypy? SType { get => st; set { st = value; okCommand.NotifyCanExecuteChanged(); } }
        // Property for capacity, can be adjusted as needed
        public int Capacity { get => cap; set { cap = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }

        // Property for the name of the storage
        public string Name { get => name; set => name = value; }

        // Property for ID
        public int? ID { get => iD; set => iD = value; }

        // The storage object itself
        public Tables.Storage Storage => d;

        // Constructor that initializes the view model
        public AddStorageViewModel(Tables.Storage d)
        {
            this.d = d;
            sType = d.Type;
            Capacity = d.Capacity;
            Name = d.Name;
            ID = d.id;
        }
    }
}
