using BDAS_2_dog_shelter.Tables;
using Oracle.ManagedDataAccess.Client;
using System.Windows;

namespace BDAS_2_dog_shelter.DogTree
{
    public partial class DogTree : Window
    {
        public DogTree(Dog rootDog, OracleConnection connection)
        {
            InitializeComponent();
            DataContext = new DogTreeViewModel(rootDog, connection);
        }
    }
}
