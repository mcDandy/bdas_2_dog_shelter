﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
