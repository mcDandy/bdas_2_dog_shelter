using BDAS_2_dog_shelter.Tables;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.ComponentModel;

namespace BDAS_2_dog_shelter.DogTree
{
    internal class DogTreeViewModel :INotifyPropertyChanged
        {
            private Dog dog;

            public DogTreeViewModel(Dog d)
            {
                dog = d;
                LoadDogTree(); // Načíst data
            }

            public string DogName => dog?.Name ?? "No name";
            public string FatherName => dog.Otec?.Name ?? "No father"; // Zde používáme vlastnost Otec
            public string MotherName => dog.Matka?.Name ?? "No mother"; // Zde používáme vlastnost Matka

            private void LoadDogTree()
            {
                // Můžete rozšiřovat funkcionalitu, například načítat další data, pokud by byla třeba
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
