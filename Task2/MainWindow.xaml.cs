using System.Windows;
using Task2.ViewModels;

namespace Task2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Явно привязываем ViewModel к контексту данных окна
            DataContext = new MainViewModel();
        }
    }
}