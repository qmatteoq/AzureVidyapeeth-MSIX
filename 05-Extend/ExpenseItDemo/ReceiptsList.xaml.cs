using System;
using System.Collections.Generic;
using System.Windows;
using Windows.Storage;

namespace ExpenseItDemo
{
    /// <summary>
    /// Interaction logic for ExpensesList.xaml
    /// </summary>
    public partial class ReceiptsList : Window
    {
        public ReceiptsList()
        {
            InitializeComponent();
            this.Loaded += ExpensesList_Loaded;
        }

        private async void ExpensesList_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> photos = new List<string>();

            var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();
            foreach (var file in files)
            {
                photos.Add(file.Path);
            }

            Photos.ItemsSource = photos;
        }
    }
}
