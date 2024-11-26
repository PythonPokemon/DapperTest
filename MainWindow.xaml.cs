using System;
using System.Data.SqlClient;
using System.Windows;
using Dapper;

namespace DapperTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabase();
        }

        private async void InitializeDatabase()
        {
            try
            {
                string connectionString = Properties.Settings.Default.DapperConnectionString;

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Tabelle 'Benutzer' erstellen, falls sie nicht existiert
                    string createTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Benutzer')
                CREATE TABLE Benutzer (
                    BenutzerId INT IDENTITY(1,1) PRIMARY KEY,
                    Vorname NVARCHAR(100),
                    Nachname NVARCHAR(100),
                    Benutzername NVARCHAR(100) UNIQUE NOT NULL,
                    Passwort NVARCHAR(256) NOT NULL,
                    Email NVARCHAR(100),
                    Telefon NVARCHAR(50),
                    Adresse NVARCHAR(200),
                    Geburtsdatum DATE,
                    Registrierungsdatum DATETIME DEFAULT GETDATE()
                )";

                    await connection.ExecuteAsync(createTableQuery);
                    Console.WriteLine("Tabelle 'Benutzer' wurde erstellt oder existiert bereits.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Zugriff auf die Datenbank: {ex.Message}", "Datenbankfehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
               

    }
}
