using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace CARS_WPF
{
    /// <summary>
    /// Interaction logic for bFeladat.xaml
    /// </summary>
    public partial class bFeladat : Window
    {
        string connectionString = "Server=localhost;Database=classicmodels;User ID=root;Password=";
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();
        public bFeladat()
        {
            InitializeComponent();
            LoadCountries();
            dgCustomers.ItemsSource = Customers;
        }

        private void LoadCountries()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT DISTINCT country FROM customers ORDER BY country";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cbCountries.Items.Add(reader["country"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void cbCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCountries.SelectedItem == null) return;

            string selectedCountry = cbCountries.SelectedItem.ToString();
            Customers.Clear();  // Meglévő lista törlése

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT customerName, phone, city FROM customers WHERE country = @country";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@country", selectedCountry);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Customers.Add(new Customer
                        {
                            CustomerName = reader["customerName"].ToString(),
                            Phone = reader["phone"].ToString(),
                            City = reader["city"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba az ügyfelek betöltésekor: " + ex.Message);
                }
            }
        }

        public class Customer
        {
            public string CustomerName { get; set; }
            public string Phone { get; set; }
            public string City { get; set; }
        }

        private void btnA_Click(object sender, RoutedEventArgs e)
        {
            MainWindow a = new MainWindow();
            a.Show();
            this.Close();
        }

        private void btnC_Click(object sender, RoutedEventArgs e)
        {
            cFeladat c = new cFeladat();
            c.Show();
            this.Close();
        }
    }
}
