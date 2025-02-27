using MySqlConnector;
using Mysqlx.Cursor;
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

namespace CARS_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString = "Server=localhost;Database=classicmodels;User ID=root;Password=";
        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();      
        }

        private void LoadProducts()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT productCode, productName FROM products";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string productInfo = $"{reader["productCode"]} - {reader["productName"]}";
                        lbProducts.Items.Add(productInfo);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba a termékek betöltésekor: " + ex.Message);
                }
            }
        }

        private void lbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbProducts.SelectedItem == null) return;

            string selectedProduct = lbProducts.SelectedItem.ToString();
            string productCode = selectedProduct.Split('-')[0].Trim();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT quantityOrdered FROM orderDetails WHERE productCode = @productCode";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@productCode", productCode);

                    int orderCount = Convert.ToInt32(cmd.ExecuteScalar());
                    lblProductCount.Content = $"{orderCount}";

                    if (orderCount == 0)
                    {
                        MessageBox.Show("Ehhez a termékhez nincs rendelés!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void btnB_Click(object sender, RoutedEventArgs e)
        {     
            bFeladat b = new bFeladat();
            b.Show();
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