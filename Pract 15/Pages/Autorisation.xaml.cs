using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Pract_15.Pages
{
    public partial class Autorisation : Page
    {
        public string AdminPassword { get; set; }

        public Autorisation()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainForm(false));
        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            ADMIN.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

            if (Validation.GetHasError(ADMIN))
            {
                return;
            }

            if (AdminPassword != "1234")
            {
                MessageBox.Show("Введите корректные данные для входа как админ");
            }
            else
            {
                NavigationService.Navigate(new MainForm(true));
            }
        }
    }
}