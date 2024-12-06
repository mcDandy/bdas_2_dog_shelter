using BDAS_2_dog_shelter.Add.Dog_Historie;
using BDAS_2_dog_shelter.Tables;
using System.Windows;

namespace BDAS_2_dog_shelter.Add.Dog_Historie
{
    /// <summary>
    /// Interakční logika pro Dog_Historie_Add.xaml
    /// </summary>
    public partial class Dog_Historie_Add : Window
    {
        public Dog_Historie_Add(List<Tables.Dog> storages, List<KeyValueUS> typy)
        {
            InitializeComponent();
            Tables.Dog_History d = new();
            this.DataContext = new AddDogHistoryViewModel(d, storages,typy);
            ((AddDogHistoryViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
        public Dog_Historie_Add(Tables.Dog_History d, List<Tables.Dog> storages, List<KeyValueUS> typy )
        {
            InitializeComponent();
            this.DataContext = new AddDogHistoryViewModel(d, storages, typy);
            ((AddDogHistoryViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
    }
}
