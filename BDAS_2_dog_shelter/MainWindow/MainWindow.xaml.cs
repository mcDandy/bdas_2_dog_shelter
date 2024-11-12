using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Tables;
using Oracle.ManagedDataAccess.Client;
using static BDAS_2_dog_shelter.Secrets;

namespace BDAS_2_dog_shelter.MainWindow
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ulong permissions)
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(permissions);
        }

    }
}
