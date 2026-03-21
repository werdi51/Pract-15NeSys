using Microsoft.EntityFrameworkCore;
using Pract_15.Models;
using Pract15;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Pract_15.Pages.AdminPagesEdit;

namespace Pract_15.Pages.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для ShowTag.xaml
    /// </summary>
    public partial class ShowTag : Page
    {
        public ObservableCollection<Tag> Tags { get; set; } = new ObservableCollection<Tag>();
        public Tag SelectedTag { get; set; }

        public ShowTag()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var context = DBService.Instance.Context;
            var items = await context.Tags.ToListAsync();
            Tags.Clear();
            foreach (var i in items) Tags.Add(i);
        }
        
        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService?.GoBack();
        private void Add_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new TagAddOrEdit());
        private void Delete_Click(object sender, RoutedEventArgs e) => MessageBox.Show($"Удалить категорию: {SelectedTag?.Name}");
        private void Edit_DoubleClick(object sender, MouseButtonEventArgs e) => NavigationService.Navigate(new TagAddOrEdit(SelectedTag));



    }
}
