using System.Windows;
using System.Windows.Media;

namespace P417_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TranslateTransform translateTransform = new TranslateTransform(); //Сдвигает элементы по горизонтали и вертикали
            RotateTransform rotateTransform = new RotateTransform(); // Вращает элемент
            ScaleTransform scaleTransform = new ScaleTransform(); // Выполняет масштабирование
            SkewTransform skewTransform = new SkewTransform(); // Изменение элем. путем наклона на N градусов
            MatrixTransform matrixTransform = new MatrixTransform(); // Изменяет координатную систему в соответствии с матрицей
            TransformGroup transformGroup = new TransformGroup(); // Группирует несколько трансформаций, чтобы применить их к одному элементу
        }
    }
}