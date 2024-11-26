using Dapper;
using DapperTest.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
    
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (ValidateUser(username, password))
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ungültige Anmeldedaten.");
            }
        }

        

        private bool ValidateUser(string username, string password)
        {
            try
            {
                string connectionString = Properties.Settings.Default.DapperConnectionString;

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT PasswortHash FROM Benutzer WHERE Benutzername = @Username";
                    string storedHash = connection.QuerySingleOrDefault<string>(query, new { Username = username });

                    if (storedHash != null && PasswordHelper.HashPassword(password) == storedHash)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler bei der Benutzerüberprüfung: {ex.Message}");
            }
            return false;
        }



    }
}
