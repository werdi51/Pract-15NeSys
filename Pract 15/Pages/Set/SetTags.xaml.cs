using Microsoft.EntityFrameworkCore;
using Pract_15.Models;
using Pract15;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Pract_15.Pages.Set
{
    public class TagSelection
    {
        public Tag Tag { get; set; }
        public bool IsSelected { get; set; }
    }

    public partial class SetTags : Page
    {
        private Product _product;
        public string ProductTitle => $"Теги {_product.Name}";

        public ObservableCollection<TagSelection> TagItems { get; set; } = new();

        public SetTags(Product product)
        {
            _product = product;
            InitializeComponent();
            DataContext = this;
            LoadTags();
        }

        private async void LoadTags()
        {
            var context = DBService.Instance.Context;

            var allTags = await context.Tags.ToListAsync();

            var currentProduct = await context.Products
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == _product.Id);

            TagItems.Clear();
            foreach (var tag in allTags)
            {
                TagItems.Add(new TagSelection
                {
                    Tag = tag,
                    IsSelected = currentProduct.Tags.Any(t => t.Id == tag.Id)
                });
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var context = DBService.Instance.Context;

            try
            {
                var productInDb = await context.Products
                    .Include(p => p.Tags)
                    .FirstOrDefaultAsync(p => p.Id == _product.Id);

                if (productInDb != null)
                {
                    productInDb.Tags.Clear();

                    var selectedTags = TagItems
                        .Where(x => x.IsSelected)
                        .Select(x => x.Tag)
                        .ToList();

                    foreach (var tag in selectedTags)
                    {
                        var trackedTag = await context.Tags.FindAsync(tag.Id);
                        productInDb.Tags.Add(trackedTag);
                    }

                    await context.SaveChangesAsync();
                    MessageBox.Show("Теги доавьоены");
                    NavigationService.GoBack();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибк {ex.Message}");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}