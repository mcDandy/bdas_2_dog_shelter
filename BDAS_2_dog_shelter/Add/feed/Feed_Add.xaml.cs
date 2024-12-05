﻿using BDAS_2_dog_shelter.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace BDAS_2_dog_shelter.Add.Food
{
    /// <summary>
    /// Interakční logika pro Feed_Add.xaml
    /// </summary>
    public partial class FeedAdd : Window
    {
        public FeedAdd()
        {
            InitializeComponent();
        }

        public FeedAdd(Feed feed, List<Tables.Storage> storages)
        {
            this.DataContext = new AddFoodViewModel(feed,storages);
        }


    }
}
