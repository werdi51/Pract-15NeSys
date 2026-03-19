using Microsoft.IdentityModel.Tokens;
using Pract15.Models;
using Pract15.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pract15
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Pract15TrpoContext db = DBService.Instance.Context;

        public ObservableCollection<Form> forms { get; set; } = new();

        public string searchQuerry { get; set; } = null;

        public ICollectionView formsView { get; set; }


        public string filterHeightFrom { get; set; } = null;
        public string filterHeightTo { get; set; } = null;

        private void ComboBox_SelectionChanged(object sender,SelectionChangedEventArgs e)
        {
            formsView.SortDescriptions.Clear();
            var cb = (ComboBox)sender;
            var selected = (ComboBoxItem)cb.SelectedItem;
            switch (selected.Tag)
            {
                case "Age":
                    formsView.SortDescriptions.Add(new SortDescription("Age",
                    ListSortDirection.Ascending));
                    break;
                case "Weight":
                    formsView.SortDescriptions.Add(new SortDescription("Weight",
                    ListSortDirection.Ascending));
                    break;
                case "Height":
                    formsView.SortDescriptions.Add(new SortDescription("Height",
                    ListSortDirection.Ascending));
                    break;
            }
            formsView.Refresh();
        }

        public bool FilterForms(object obj)
        {
            if (obj is not Form)
                return false;

            var form = (Form)obj;

            if (searchQuerry != null && !form.Name.Contains(searchQuerry,StringComparison.CurrentCultureIgnoreCase))
                return false;

            if (!filterHeightFrom.IsNullOrEmpty() && Convert.ToInt32(filterHeightFrom) > form.Height)
                return false;

            if (!filterHeightTo.IsNullOrEmpty() && Convert.ToInt32(filterHeightTo) < form.Height)
                return false;

            return true;
        }

        public void LoadList(object sender, EventArgs e)
        {
            forms.Clear();
            foreach (var form in db.Forms.ToList())
            {
                forms.Add(form);
            }
        }
        public MainWindow()
        {
            formsView = CollectionViewSource.GetDefaultView(forms);
            formsView.Filter = FilterForms;
            InitializeComponent();
        }



        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            formsView.Refresh();
        }
    }
}