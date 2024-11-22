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
using System.Windows.Shapes;

namespace DapperTest.Views
{
    /// <summary>
    /// Interaktionslogik für StartWindowView.xaml
    /// </summary>
    public partial class StartWindowView : Window
    {
        public StartWindowView()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Öffne das Login-Fenster
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();  // Schließe das Startfenster
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Öffne das Registrierungs-Fenster
            RegisterView registerView = new RegisterView();
            registerView.Show();
            this.Close();  // Schließe das Startfenster
        }

    }
}
