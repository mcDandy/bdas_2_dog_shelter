using BDAS_2_dog_shelter.Add.Shelter;
using System.Windows;

namespace BDAS_2_dog_shelter.Add.Adress
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class AdressAdd : Window
    {
        public AdressAdd()
        {
            InitializeComponent();
            Tables.Adress d = new();
            DataContext = new AddAdressViewModel(d);
            ((AddAdressViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public AdressAdd(Tables.Adress d)
        {
            InitializeComponent();
            DataContext = new AddAdressViewModel(d);
            ((AddAdressViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
