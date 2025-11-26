using DalList;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            

            InitializeComponent();

            lv_orders.ItemsSource = InitDO.CreateSampleOrders(100); ;
        }

        private void Button_Click(object sender, RoutedEventArgs e) 
            => MessageBox.Show("Hello, World!");

        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("All Right!");
        }

        private void Click_Me_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Click_Me_Button.Content = "Click Me Now!";
        }

        private void Click_Me_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Click_Me_Button.Content = "Click Me!";
        }

      
    }
}