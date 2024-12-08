﻿using System.Windows;
using static BDAS_2_dog_shelter.Secrets;
using BDAS_2_dog_shelter.Login;

namespace BDAS_2_dog_shelter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        public LoginWindow()
        {
            InitializeComponent();
            try
            {
                if (ConnectionString == null) return;
            }
            catch (System.TypeInitializationException)
            {
                MessageBox.Show("Nenašel se soubor secrets.json s tajnými konstantami ve složce programu nebo formát souboru není JSON.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ((LoginWindowViewModel)DataContext).OnCloaseRequest += Colse;

        }

        private void Colse()
        {
            Close();
        }
    }
}