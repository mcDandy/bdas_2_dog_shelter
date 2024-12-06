using BDAS_2_dog_shelter.Add.Food;
using System.Windows;

namespace BDAS_2_dog_shelter.Add.Dog_Historie
{
    /// <summary>
    /// Interakční logika pro Dog_Historie_Add.xaml
    /// </summary>
    public partial class Dog_Historie_Add : Window
    {
        public Dog_Historie_Add(List<Tables.Dog> storages)
        {
            InitializeComponent();
            Tables.Dog_History d = new();
            this.DataContext = new AddDogHistoryViewModel(d, storages);
            ((AddDogHistoryViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
        public Dog_Historie_Add(Tables.Dog_History d, List<Tables.Dog> storages)
        {
            InitializeComponent();
            this.DataContext = new AddDogHistoryViewModel(d, storages);
            ((AddDogHistoryViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
    }
}
