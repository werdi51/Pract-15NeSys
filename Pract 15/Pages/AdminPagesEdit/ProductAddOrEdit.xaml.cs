using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Pract_15.Models;
using Pract15;

namespace Pract_15.Pages.AdminPagesEdit
{
    /// <summary>
    /// Логика взаимодействия для ProductAddOrEdit.xaml
    /// </summary>
    public partial class ProductAddOrEdit : Page
    {
        private Product _product;
        private bool _isEdit;

        public ProductAddOrEdit(Product product = null)
        {
            InitializeComponent();
            _product = product;
            _isEdit = product != null;
            Title = _isEdit ? "Редактирование товара" : "Добавление товара";
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadComboBoxes();

            if (_isEdit)
            {
                txtName.Text = _product.Name;
                txtDescription.Text = _product.Description;
                txtPrice.Text = _product.Price?.ToString();
                txtStock.Text = _product.Stock?.ToString();
                txtRating.Text = _product.Rating?.ToString();

                if (_product.CategoryId.HasValue)
                    cmbCategory.SelectedValue = _product.CategoryId.Value;
                if (_product.BrandId.HasValue)
                    cmbBrand.SelectedValue = _product.BrandId.Value;
            }
        }

        private async Task LoadComboBoxes()
        {
            try
            {
                var context = DBService.Instance.Context;
                var categories = await context.Categories.ToListAsync();
                var brands = await context.Brands.ToListAsync();

                cmbCategory.ItemsSource = categories;
                cmbBrand.ItemsSource = brands;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки списков: {ex.Message}");
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название товара.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var context = DBService.Instance.Context;

                if (_isEdit)
                {
                    // Редактирование
                    var product = await context.Products.FindAsync(_product.Id);
                    if (product != null)
                    {
                        product.Name = txtName.Text.Trim();
                        product.Description = txtDescription.Text?.Trim();
                        product.Price = ParseDouble(txtPrice.Text);
                        product.Stock = ParseDouble(txtStock.Text);
                        product.Rating = ParseDouble(txtRating.Text);
                        product.CategoryId = (double?)cmbCategory.SelectedValue;
                        product.BrandId = (double?)cmbBrand.SelectedValue;

                        await context.SaveChangesAsync();
                        MessageBox.Show("Товар обновлён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    // Добавление
                    var newProduct = new Product
                    {
                        Name = txtName.Text.Trim(),
                        Description = txtDescription.Text?.Trim(),
                        Price = ParseDouble(txtPrice.Text),
                        Stock = ParseDouble(txtStock.Text),
                        Rating = ParseDouble(txtRating.Text),
                        CategoryId = (double?)cmbCategory.SelectedValue,
                        BrandId = (double?)cmbBrand.SelectedValue,
                        CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    context.Products.Add(newProduct);
                    await context.SaveChangesAsync();
                    MessageBox.Show("Товар добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                NavigationService?.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private double? ParseDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            if (double.TryParse(value, out double result))
                return result;
            return null;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
