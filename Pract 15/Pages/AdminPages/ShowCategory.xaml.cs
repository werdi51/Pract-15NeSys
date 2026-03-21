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

namespace Pract_15.Pages.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для ShowCategory.xaml
    /// </summary>
    public partial class ShowCategory : Page
    {
        public ObservableCollection<Category> Category { get; set; } = new ObservableCollection<Category>();
        public Category SelectedCategoty { get; set; }

        public ShowCategory()
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
            var items = await context.Categories.ToListAsync();
            Category.Clear();
            foreach (var i in items) Category.Add(i);
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService?.GoBack();
        private void Add_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Открыть окно добавления категории");
        private void Delete_Click(object sender, RoutedEventArgs e) => MessageBox.Show($"Удалить категорию: {SelectedCategoty?.Name}");
        private void Edit_DoubleClick(object sender, MouseButtonEventArgs e) => MessageBox.Show($"Редактировать категорию: {SelectedCategoty?.Name}");

    }
}
