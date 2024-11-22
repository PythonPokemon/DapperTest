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
                // Hole die Verbindung aus den Einstellungen (App.config oder Settings.settings)
                string connectionString = Properties.Settings.Default.DapperConnectionString;

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Erstelle die Employees-Tabelle, falls sie nicht existiert
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Employees')
                        CREATE TABLE Employees (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Name NVARCHAR(100),
                            Age INT,
                            Salary DECIMAL(18, 2)
                        )";

                    await connection.ExecuteAsync(createTableQuery);
                    Console.WriteLine("Tabelle 'Employees' wurde erstellt oder existiert bereits.");

                    // Einen neuen Mitarbeiter in die Tabelle 'Employees'hinzufügen
                    var newEmployee = new Employee
                    {
                        Name = "Paragon",
                        Age = 57,
                        Salary = 19.00M
                    };
                    // Einfügen der neuen Datensätze in die entsprechenden Spalten 'Name, Age, Salary' in die Tabelle
                    string insertQuery = "INSERT INTO Employees (Name, Age, Salary) VALUES (@Name, @Age, @Salary)";
                    await connection.ExecuteAsync(insertQuery, newEmployee);

                    Console.WriteLine("Neuer Mitarbeiter wurde hinzugefügt.");

                    // Alle Mitarbeiter abfragen
                    string selectQuery = "SELECT * FROM Employees";
                    var employees = await connection.QueryAsync<Employee>(selectQuery);

                    Console.WriteLine("Mitarbeiter:");
                    foreach (var employee in employees)
                    {
                        Console.WriteLine($"Id: {employee.Id}, Name: {employee.Name}, Age: {employee.Age}, Salary: {employee.Salary}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                MessageBox.Show($"Fehler beim Zugriff auf die Datenbank: {ex.Message}", "Datenbankfehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
