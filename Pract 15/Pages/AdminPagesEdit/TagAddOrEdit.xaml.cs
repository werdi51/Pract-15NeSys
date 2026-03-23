using Pract_15.Models;
using Pract15;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Pract_15.Pages.AdminPagesEdit
{
    public partial class TagAddOrEdit : Page, INotifyPropertyChanged
    {
        private Tag _currentTag;
        private string _tagName;

        public string TagName
        {
            get => _tagName;
            set { _tagName = value; OnPropertyChanged(); }
        }

        public TagAddOrEdit(Tag tag = null)
        {
            InitializeComponent();
            _currentTag = tag;

            if (_currentTag != null)
            {
                TagName = _currentTag.Name;
            }

            DataContext = this;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TriggerValidation(txtName, TextBox.TextProperty);
        }
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TagName))
            {
                MessageBox.Show("Введите тег");
                return;
            }

            try
            {
                var context = DBService.Instance.Context;
                context.ChangeTracker.Clear();

                if (_currentTag == null)
                {
                    int maxId = 0;
                    if (context.Tags.Any())
                        maxId = (int)context.Tags.Max(t => t.Id);

                    var newTag = new Tag
                    {
                        Id = maxId + 1,
                        Name = TagName
                    };
                    context.Tags.Add(newTag);
                }
                else
                {
                    var tag = await context.Tags.FindAsync(_currentTag.Id);
                    if (tag != null)
                    {
                        tag.Name = TagName;
                    }
                }

                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();
                MessageBox.Show("Данные сохранены");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка {ex.Message}");
                DBService.Instance.Context.ChangeTracker.Clear();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void TriggerValidation(DependencyObject target, DependencyProperty property)
        {
            var binding = BindingOperations.GetBindingExpression(target, property);
            binding?.UpdateSource();
        }
    }
}