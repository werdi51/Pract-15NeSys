using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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

namespace Pract_15.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autorisation.xaml
    /// </summary>
    public partial class Autorisation : Page
    {
        public Autorisation()
        {
            InitializeComponent();
        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainForm(false));
        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ADMIN.Text))
            {
                MessageBox.Show("Введите данные для входа как админ");
                return;
            }

            if (ADMIN.Text != "1234")
            {
                MessageBox.Show("Введите корретные данные для входа как админ");
            }
            else
            {
                NavigationService.Navigate(new MainForm(true));
            }
        }
    }
}
