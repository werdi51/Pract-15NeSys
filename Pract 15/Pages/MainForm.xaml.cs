using Microsoft.EntityFrameworkCore;
using Pract_15.Models;
using Pract15;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Pract_15.Pages
{
    public partial class MainForm : Page, INotifyPropertyChanged
    {
        private List<Product> _allProducts;
        private ICollectionView _productsView;

        public ICollectionView ProductsView
        {
            get => _productsView;
            set { _productsView = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public ObservableCollection<Brand> Brands { get; set; } = new ObservableCollection<Brand>();

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; OnPropertyChanged(); ProductsView?.Refresh(); }
        }

        private double? _selectedCategoryId;
        public double? SelectedCategoryId
        {
            get => _selectedCategoryId;
            set { _selectedCategoryId = value; OnPropertyChanged(); ProductsView?.Refresh(); }
        }

        private double? _selectedBrandId;
        public double? SelectedBrandId
        {
            get => _selectedBrandId;
            set { _selectedBrandId = value; OnPropertyChanged(); ProductsView?.Refresh(); }
        }

        private string _filterPriceFrom;
        public string FilterPriceFrom
        {
            get => _filterPriceFrom;
            set { _filterPriceFrom = value; OnPropertyChanged(); ProductsView?.Refresh(); }
        }

        private string _filterPriceTo;
        public string FilterPriceTo
        {
            get => _filterPriceTo;
            set { _filterPriceTo = value; OnPropertyChanged(); ProductsView?.Refresh(); }
        }

        private string _sortBy = "NameAsc";
        public string SortBy
        {
            get => _sortBy;
            set { _sortBy = value; OnPropertyChanged(); ApplySorting(); }
        }

        public int ProductsCount => ProductsView?.Cast<object>().Count() ?? 0;

        public MainForm(bool admin)
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            try
            {
                var context = DBService.Instance.Context;
                _allProducts = await context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .ToListAsync();

                // Категории с пунктом "Все" (Id = -1)
                var categories = await context.Categories.ToListAsync();
                Categories.Clear();
                Categories.Add(new Category { Id = -1, Name = "Все" });
                foreach (var cat in categories)
                    Categories.Add(cat);

                // Бренды с пунктом "Все" (Id = -1)
                var brands = await context.Brands.ToListAsync();
                Brands.Clear();
                Brands.Add(new Brand { Id = -1, Name = "Все" });
                foreach (var br in brands)
                    Brands.Add(br);

                SelectedCategoryId = -1;
                SelectedBrandId = -1;

                ProductsView = CollectionViewSource.GetDefaultView(_allProducts);
                ProductsView.Filter = FilterProducts;
                ApplySorting();

                OnPropertyChanged(nameof(ProductsCount));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private bool FilterProducts(object obj)
        {
            if (obj is not Product p) return false;

            // Поиск
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                bool match = (p.Name != null && p.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)) ||
                             (p.Description != null && p.Description.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
                if (!match) return false;
            }

            // Категория (пропускаем, если выбрано "Все" = -1)
            if (SelectedCategoryId.HasValue && SelectedCategoryId.Value != -1)
            {
                if (p.CategoryId != SelectedCategoryId.Value) return false;
            }

            // Бренд (пропускаем, если выбрано "Все" = -1)
            if (SelectedBrandId.HasValue && SelectedBrandId.Value != -1)
            {
                if (p.BrandId != SelectedBrandId.Value) return false;
            }

            // Цена от
            if (!string.IsNullOrWhiteSpace(FilterPriceFrom))
            {
                if (double.TryParse(FilterPriceFrom, out double from))
                {
                    if (p.Price < from) return false;
                }
            }

            // Цена до
            if (!string.IsNullOrWhiteSpace(FilterPriceTo))
            {
                if (double.TryParse(FilterPriceTo, out double to))
                {
                    if (p.Price > to) return false;
                }
            }

            return true;
        }

        private void ApplySorting()
        {
            if (ProductsView == null) return;
            ProductsView.SortDescriptions.Clear();

            switch (SortBy)
            {
                case "NameAsc":
                    ProductsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    break;
                case "PriceAsc":
                    ProductsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Ascending));
                    break;
                case "PriceDesc":
                    ProductsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Descending));
                    break;
                case "StockAsc":
                    ProductsView.SortDescriptions.Add(new SortDescription("Stock", ListSortDirection.Ascending));
                    break;
                case "StockDesc":
                    ProductsView.SortDescriptions.Add(new SortDescription("Stock", ListSortDirection.Descending));
                    break;
            }
            ProductsView.Refresh();
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            ProductsView?.Refresh();
            OnPropertyChanged(nameof(ProductsCount));
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            ProductsView?.Refresh();
            OnPropertyChanged(nameof(ProductsCount));
        }

        private void SortingChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox cb && cb.SelectedItem is ComboBoxItem item)
            {
                SortBy = item.Tag.ToString();
            }
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchQuery = null;
            SelectedCategoryId = -1;
            SelectedBrandId = -1;
            FilterPriceFrom = null;
            FilterPriceTo = null;
            SortBy = "NameAsc";
            ProductsView?.Refresh();
            OnPropertyChanged(nameof(ProductsCount));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}