using Pract_15.Models;
using Pract15;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pract_15.Pages.AdminPagesEdit
{
    /// <summary>
    /// Логика взаимодействия для BrandAddOrEdit.xaml
    /// </summary>
    public partial class BrandAddOrEdit : Page
    {
        private Brand _editingBrand;

        public BrandAddOrEdit(Brand brand = null)
        {
            InitializeComponent();
            if (brand != null)
            {
                _editingBrand = brand;
                Title = "Редактирование бренда";
                txtName.Text = brand.Name;
            }
            else
            {
                Title = "Добавление юренда";
            }
            txtName.Focus();
        }

        private async void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Название не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var context = DBService.Instance.Context;
                string newName = txtName.Text.Trim();

                if (_editingBrand == null)
                {
                    var newCategory = new Brand { Name = newName };
                    context.Brands.Add(newCategory);
                    await context.SaveChangesAsync();
                }
                else
                {
                    var category = await context.Categories.FindAsync(_editingBrand.Id);
                    if (category != null)
                    {
                        category.Name = newName;
                        await context.SaveChangesAsync();
                        _editingBrand.Name = newName;
                    }
                }
                NavigationService?.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
