using System.Windows;
using BDAS_2_dog_shelter.Tables;

namespace BDAS_2_dog_shelter.DogTree
{
    /// <summary>
    /// Interakční logika pro DogEdit.xaml
    /// </summary>
    public partial class DogTree : Window
    {
        public DogTree(Dog d)
        {
            DataContext = new DogTreeViewModel(d);
            InitializeComponent();
        }
    }
}
