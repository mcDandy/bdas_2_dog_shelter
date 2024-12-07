﻿using System.Windows;

namespace BDAS_2_dog_shelter.Add.feed
{
    /// <summary>
    /// Interakční logika pro Feed_Add.xaml
    /// </summary>
    public partial class FeedAdd : Window
    {
        public FeedAdd()
        {
            InitializeComponent();
            Tables.Feed d = new();
            DataContext = new AddFeedViewModel(d);
            ((AddFeedViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public FeedAdd(Tables.Feed d, List<Tables.Storage> storages)
        {
            InitializeComponent();
            DataContext = new AddFeedViewModel(d);
            ((AddFeedViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
      
    }
}
