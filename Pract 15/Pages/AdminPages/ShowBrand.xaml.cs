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
    /// Логика взаимодействия для ShowBrand.xaml
    /// </summary>
    public partial class ShowBrand : Page
    {
        public ObservableCollection<Brand> Brands { get; set; } = new ObservableCollection<Brand>();
        public Brand SelectedBrand { get; set; }

        public ShowBrand()
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
            try
            {
                var context = DBService.Instance.Context;
                var brands = await context.Brands.ToListAsync();
                Brands.Clear();
                foreach (var b in brands)
                    Brands.Add(b);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Здесь вы откроете своё окно добавления бренда
            MessageBox.Show("Открыть окно добавления бренда");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBrand == null)
            {
                MessageBox.Show("Выберите элемент для удаления.");
                return;
            }
            // Здесь вы вызовете своё окно подтверждения или удаление
            MessageBox.Show($"Удалить бренд: {SelectedBrand.Name}");
        }

        private void Edit_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedBrand == null) return;
            // Здесь вы откроете окно редактирования бренда
            MessageBox.Show($"Редактировать бренд: {SelectedBrand.Name}");
        }
    }
}
