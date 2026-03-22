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
                MessageBox.Show("Данные сохранены!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
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
    }
}