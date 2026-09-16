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

namespace WPF_P417
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Button button1 = new Button();
            button1.Content = "Кнопка 2";
            button1.Width = 120;
            button1.Height = 40;
            button1.Margin = new Thickness(5, 5, 5, 100);
            button1.HorizontalAlignment = HorizontalAlignment.Center;
            button1.Background = new SolidColorBrush(Colors.Red);
            button1.Background = (Brush)System.ComponentModel.
                TypeDescriptor.
                GetConverter(typeof(Brush)).
                ConvertFromInvariantString("Red");
            layoutGrid.Children.Add(button1);
        }
    }
}