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
    /// Логика взаимодействия для CategoryAddOrEdit.xaml
    /// </summary>
    public partial class CategoryAddOrEdit : Page, INotifyPropertyChanged
    {
        private Category _editingCategory;
        private string _categoryName;

        public string CategoryName
        {
            get => _categoryName;
            set { _categoryName = value; OnPropertyChanged(); }
        }

        public CategoryAddOrEdit(Category category = null)
        {
            InitializeComponent();
            _editingCategory = category;

            if (_editingCategory != null)
            {
                CategoryName = _editingCategory.Name;
                Title = "Редактирование категории";
            }
            else
            {
                Title = "Добавление категории";
            }

            DataContext = this; 
            txtName.Focus();
        }

        private async void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(txtName))
            {
                MessageBox.Show("Исправьте ошибки в названии.");
                return;
            }

            try
            {
                var context = DBService.Instance.Context;
                context.ChangeTracker.Clear();

                if (_editingCategory == null)
                {
                    int maxId = context.Categories.Any() ? (int)context.Categories.Max(c => c.Id) : 0;
                    var newCategory = new Category { Id = maxId + 1, Name = CategoryName.Trim() };
                    context.Categories.Add(newCategory);
                }
                else
                {
                    var category = await context.Categories.FindAsync(_editingCategory.Id);
                    if (category != null) category.Name = CategoryName.Trim();
                }

                await context.SaveChangesAsync();
                NavigationService?.GoBack();
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void Cancel_Click(object sender, RoutedEventArgs e) => NavigationService?.GoBack();
    }
}
