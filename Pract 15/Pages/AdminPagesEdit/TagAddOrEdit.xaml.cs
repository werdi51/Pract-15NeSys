using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Pract_15.Models;
using Pract15;

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

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TagName))
            {
                MessageBox.Show("Введите название тега!");
                return;
            }

            var context = DBService.Instance.Context;

            try
            {
                if (_currentTag == null)
                {
                    var newTag = new Tag { Name = TagName };
                    context.Tags.Add(newTag);
                }
                else
                {
                    _currentTag.Name = TagName;
                    context.Tags.Update(_currentTag);
                }

                await context.SaveChangesAsync();
                MessageBox.Show("Данные сохранены!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
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
    }
}