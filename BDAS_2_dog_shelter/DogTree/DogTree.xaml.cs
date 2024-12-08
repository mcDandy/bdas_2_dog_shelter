using BDAS_2_dog_shelter.Tables;
using System.Windows;

namespace BDAS_2_dog_shelter.DogTree
{
    public partial class DogTree : Window
    {
        public DogTree(Dog rootDog)
        {
            InitializeComponent();
            DataContext = new DogTreeViewModel(rootDog);
        }
    }
}
