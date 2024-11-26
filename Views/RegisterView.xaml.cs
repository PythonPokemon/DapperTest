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
    
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string email = EmailTextBox.Text;
            string phone = PhoneTextBox.Text;
            string address = AddressTextBox.Text;
            string birthDate = BirthDatePicker.SelectedDate?.ToString("yyyy-MM-dd");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || password != confirmPassword)
            {
                MessageBox.Show("Überprüfen Sie die Eingaben.");
                return;
            }

            string hashedPassword = PasswordHelper.HashPassword(password);

            // Speichere den Benutzer in der Datenbank
            SaveUser(firstName, lastName, username, hashedPassword, email, phone, address, birthDate);
            MessageBox.Show("Registrierung erfolgreich!");

            var startWindowView = new StartWindowView();
            startWindowView.Show();
            this.Close();
        }


        

        private void SaveUser(string firstName, string lastName, string username, string hashedPassword, string email, string phone, string address, string birthDate)
        {
            try
            {
                string connectionString = Properties.Settings.Default.DapperConnectionString;

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                INSERT INTO Benutzer (Vorname, Nachname, Benutzername, PasswortHash, Email, Telefonnummer, Adresse, Geburtsdatum)
                VALUES (@FirstName, @LastName, @Username, @PasswordHash, @Email, @Phone, @Address, @BirthDate)";

                    var parameters = new
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Username = username,
                        PasswordHash = hashedPassword,
                        Email = email,
                        Phone = phone,
                        Address = address,
                        BirthDate = birthDate
                    };

                    connection.Execute(query, parameters);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern der Benutzerdaten: {ex.Message}");
            }
        }



    }
}
