using Pract_15.Models;
using Pract15;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class BrandAddOrEdit : Page, INotifyPropertyChanged 
    {
        private Brand _editingBrand;
        private string _itemName;

        public string ItemName
        {
            get => _itemName;
            set { _itemName = value; OnPropertyChanged(); }
        }

        public BrandAddOrEdit(Brand brand = null)
        {
            InitializeComponent();
            _editingBrand = brand;

            if (_editingBrand != null)
            {
                ItemName = _editingBrand.Name;
                Title = "Редактирование бренда";
            }
            else
            {
                Title = "Добавление бренда";
            }

            DataContext = this;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TriggerValidation(txtName, TextBox.TextProperty);
        }

        private async void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(txtName))
            {
                MessageBox.Show("Исправьте ошибки");
                return;
            }

            if (string.IsNullOrWhiteSpace(ItemName))  
            {
                MessageBox.Show("Название не может быть пустым");
                return;
            }

            try
            {
                var context = DBService.Instance.Context;
                context.ChangeTracker.Clear();

                if (_editingBrand == null)
                {
                    int maxId = 0;
                    if (context.Brands.Any())
                        maxId = (int)context.Brands.Max(b => b.Id);

                    var newBrand = new Brand
                    {
                        Id = maxId + 1,
                        Name = ItemName.Trim() 
                    };
                    context.Brands.Add(newBrand);
                }
                else
                {
                    var brand = await context.Brands.FindAsync(_editingBrand.Id);
                    if (brand != null)
                    {
                        brand.Name = ItemName.Trim(); 
                    }
                }

                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();
                NavigationService?.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка {ex.Message}");
                DBService.Instance.Context.ChangeTracker.Clear();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => NavigationService?.GoBack();
        private void TriggerValidation(DependencyObject target, DependencyProperty property)
        {
            var binding = BindingOperations.GetBindingExpression(target, property);
            binding?.UpdateSource();
        }
    }
}
