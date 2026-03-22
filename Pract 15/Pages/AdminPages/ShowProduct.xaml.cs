using Microsoft.EntityFrameworkCore;
using Pract_15.Models;
using Pract_15.Pages.AdminPagesEdit;
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
using Pract_15.Pages.Set;

namespace Pract_15.Pages.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для ShowProduct.xaml
    /// </summary>
    public partial class ShowProduct : Page
    {
        public ObservableCollection<Product> Product { get; set; } = new ObservableCollection<Product>();
        public Product SelectedProduct { get; set; }

        public ShowProduct()
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
            var items = await context.Products
                .AsNoTracking()
                .Include(p => p.Category) 
                .Include(p => p.Brand)
                .ToListAsync();

            Product.Clear();
            foreach (var i in items) Product.Add(i);
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService?.GoBack();
        private void Add_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new ProductAddOrEdit());
        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Сначала выберите товар в списке!");
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить товар: {SelectedProduct.Name}?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var context = DBService.Instance.Context;

                    var productToDelete = await context.Products
                        .Include(p => p.Tags) 
                        .FirstOrDefaultAsync(p => p.Id == SelectedProduct.Id);

                    if (productToDelete != null)
                    {
                        productToDelete.Tags.Clear();

                        context.Products.Remove(productToDelete);

                        await context.SaveChangesAsync();

                        context.ChangeTracker.Clear();

                        Product.Remove(SelectedProduct);
                        MessageBox.Show("Товар успешно удален.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}\n{ex.InnerException?.Message}");

                    foreach (var entry in DBService.Instance.Context.ChangeTracker.Entries())
                    {
                        entry.State = EntityState.Detached;
                    }
                }
            }
        }
        private void Edit_DoubleClick(object sender, MouseButtonEventArgs e) => NavigationService?.Navigate(new ProductAddOrEdit(SelectedProduct));

        private void TagAddToProducr_Click(object sender, RoutedEventArgs e)
        {
            var selected = productsListView.SelectedItem as Product;

            if (selected != null)
            {
                NavigationService?.Navigate(new SetTags(selected));
            }
            else
            {
                MessageBox.Show("Сначала выберите товар в списке!");
            }
        }
    }
}
