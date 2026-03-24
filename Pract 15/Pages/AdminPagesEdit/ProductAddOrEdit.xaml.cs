using Microsoft.EntityFrameworkCore;
using Pract_15.Models;
using Pract15;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Pract_15.Pages.AdminPagesEdit
{
    /// <summary>
    /// Логика взаимодействия для ProductAddOrEdit.xaml
    /// </summary>
    public partial class ProductAddOrEdit : Page, INotifyPropertyChanged
    {
        private Product _product;
        private bool _isEdit;

        public string ProductName { get; set; } 
        public string Description { get; set; }
        public string Price { get; set; }
        public string Stock { get; set; }
        public string Rating { get; set; }
        public double? CategoryId { get; set; }
        public double? BrandId { get; set; }

        public ProductAddOrEdit(Product product = null)
        {
            InitializeComponent();
            _product = product;
            _isEdit = product != null;
            Title = _isEdit ? "Редактирование товара" : "Добавление товара";
            DataContext = this;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadComboBoxes();
            if (_isEdit)
            {
                ProductName = _product.Name;
                Description = _product.Description;
                Price = _product.Price?.ToString();
                Stock = _product.Stock?.ToString();
                Rating = _product.Rating?.ToString();
                CategoryId = (double?)_product.CategoryId;
                BrandId = (double?)_product.BrandId;

                OnPropertyChanged(string.Empty);
            }
            TriggerValidation(txtName, TextBox.TextProperty);
            TriggerValidation(txtDescription, TextBox.TextProperty);
            TriggerValidation(txtPrice, TextBox.TextProperty);
            TriggerValidation(txtStock, TextBox.TextProperty);
            TriggerValidation(txtRating, TextBox.TextProperty);
            TriggerValidation(cmbCategory, ComboBox.SelectedValueProperty);
            TriggerValidation(cmbBrand, ComboBox.SelectedValueProperty);

        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(txtName) ||
                Validation.GetHasError(txtDescription) ||
                Validation.GetHasError(txtPrice) ||
                Validation.GetHasError(txtStock) ||
                Validation.GetHasError(txtRating) ||
                Validation.GetHasError(cmbCategory) ||
                Validation.GetHasError(cmbBrand))
            {
                MessageBox.Show("ошибки валидации");
                return;
            }



            try
            {
                var context = DBService.Instance.Context;
                context.ChangeTracker.Clear();

                if (_isEdit)
                {
                    var product = await context.Products.FindAsync(_product.Id);
                    if (product != null) FillProduct(product);
                }
                else
                {
                    double maxId = await context.Products.AnyAsync() ? await context.Products.MaxAsync(p => p.Id) : 0;
                    var newProduct = new Product { Id = maxId + 1, CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                    FillProduct(newProduct);
                    context.Products.Add(newProduct);
                }

                await context.SaveChangesAsync();
                NavigationService?.GoBack();
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        private void FillProduct(Product p)
        {
            p.Name = ProductName?.Trim();
            p.Description = Description?.Trim();
            p.Price = ParseDouble(Price);
            p.Stock = ParseDouble(Stock);
            p.Rating = ParseDouble(Rating);
            p.CategoryId = CategoryId;
            p.BrandId = BrandId;
        }

        private double? ParseDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            string normalizedValue = value.Replace(',', '.');

            if (double.TryParse(normalizedValue, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void Cancel_Click(object sender, RoutedEventArgs e) => NavigationService?.GoBack();

        private async Task LoadComboBoxes()
        {
            try
            {
                var context = DBService.Instance.Context;
                var categories = await context.Categories.AsNoTracking().ToListAsync();
                var brands = await context.Brands.AsNoTracking().ToListAsync();

                cmbCategory.ItemsSource = categories;
                cmbBrand.ItemsSource = brands;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка {ex.Message}");
            }
        }
        private void TriggerValidation(DependencyObject target, DependencyProperty property)
        {
            var binding = BindingOperations.GetBindingExpression(target, property);
            binding?.UpdateSource(); 
        }
    }
}
