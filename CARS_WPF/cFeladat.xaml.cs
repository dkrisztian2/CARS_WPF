using System;
using MySqlConnector;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static CARS_WPF.bFeladat;


namespace CARS_WPF
{
    /// <summary>
    /// Interaction logic for cFeladat.xaml
    /// </summary>
    public partial class cFeladat : Window
    {
        string connectionString = "Server=localhost;Database=classicmodels;User ID=root;Password=";
        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
        public ObservableCollection<OrderDetail> OrderDetails { get; set; } = new ObservableCollection<OrderDetail>();
        public cFeladat()
        {
            InitializeComponent();
            LoadOrders();
            dgOrders.ItemsSource = Orders;
            lbOrderDetails.ItemsSource = OrderDetails;
        }
        private void LoadOrders()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT orderNumber, orderDate, requiredDate, status FROM orders ORDER BY orderDate";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Orders.Add(new Order
                        {
                            OrderNumber = Convert.ToInt32(reader["orderNumber"]),
                            OrderDate = Convert.ToDateTime(reader["orderDate"]),
                            RequiredDate = Convert.ToDateTime(reader["requiredDate"]),
                            Status = reader["status"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba a rendelések betöltésekor: " + ex.Message);
                }
            }
        }

        private void dgOrders_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgOrders.SelectedItem == null) return;

            Order selectedOrder = dgOrders.SelectedItem as Order;
            OrderDetails.Clear();

            LoadOrderDetails(selectedOrder.OrderNumber);
        }

        private void LoadOrderDetails(int orderNumber)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT p.productName, od.priceEach " +
                                   "FROM orderdetails od " +
                                   "JOIN products p ON od.productCode = p.productCode " +
                                   "WHERE od.orderNumber = @orderNumber";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@orderNumber", orderNumber);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        OrderDetails.Add(new OrderDetail
                        {
                            ProductName = reader["productName"].ToString(),
                            BuyPrice = Convert.ToDecimal(reader["priceEach"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public class Order
        {
            public int OrderNumber { get; set; }
            public DateTime OrderDate { get; set; }
            public DateTime RequiredDate { get; set; }
            public string Status { get; set; }
        }

        public class OrderDetail
        {
            public string ProductName { get; set; }
            public decimal BuyPrice { get; set; }
        }

        private void btnA_Click(object sender, RoutedEventArgs e)
        {
            MainWindow a = new MainWindow();
            a.Show();
            this.Close();
        }

        private void btnB_Click(object sender, RoutedEventArgs e)
        {
            bFeladat b = new bFeladat();
            b.Show();
            this.Close();
        }

        private void dgOrders_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
