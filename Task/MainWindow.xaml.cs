using System.Windows;
using System.Windows.Controls;

namespace Task
{
    public partial class MainWindow : Window
    {
        private bool _isUpdating = false;
        private double _fuelPrice = 0;

        public MainWindow()
        {
            InitializeComponent();
            FuelTypeComboBox.SelectedIndex = 0;
            UpdateInputStates();
        }

        private void FuelTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FuelTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                // Достаем цену топлива из свойства Tag элемента и сохраняем в переменную _fuelPrice
                _fuelPrice = Convert.ToDouble(selectedItem.Tag);

                // Пересчитываем стоимость. Передаем true, если активен переключатель "По литрам"
                RecalculateFuel(RbLitres.IsChecked == true);
            }
        }

        private void RadioButtons_Checked(object sender, RoutedEventArgs e)
        {
            UpdateInputStates();
        }

        private void UpdateInputStates()
        {
            if (TxtLitres == null || TxtMoney == null) return;
            TxtLitres.IsEnabled = RbLitres.IsChecked == true;
            TxtMoney.IsEnabled = RbMoney.IsChecked == true;
        }

        private void TxtLitres_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RbLitres.IsChecked == true) 
                RecalculateFuel(byLitres: true);
        }

        private void TxtMoney_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RbMoney.IsChecked == true)
                RecalculateFuel(byLitres: false);
        }

        private void RecalculateFuel(bool byLitres)
        {
            if (_isUpdating || _fuelPrice == 0) return;
            _isUpdating = true;

            try
            {
                if (byLitres)
                {
                    // Пытаемся превратить текст из TxtLitres в число
                    if (double.TryParse(TxtLitres.Text, out double litres))
                    {
                        double cost = litres * _fuelPrice;
                        TxtMoney.Text = cost.ToString("F2"); // Записываем результат в поле денег (2 знака после запятой)
                        FuelTotalTextBlock.Text = cost.ToString("F2"); // Обновляем мини-итог за топливо
                    }
                    else { TxtMoney.Text = ""; FuelTotalTextBlock.Text = "0.00"; }
                }
                else
                {
                    if (double.TryParse(TxtMoney.Text, out double money))
                    {
                        double litres = money / _fuelPrice;
                        TxtLitres.Text = litres.ToString("F2");
                        FuelTotalTextBlock.Text = money.ToString("F2");
                    }
                    else { TxtLitres.Text = ""; FuelTotalTextBlock.Text = "0.00"; }
                }
            }
            finally { _isUpdating = false; }
        }

        private void CafeItem_Changed(object sender, RoutedEventArgs e)
        {
            if (CafeTotalTextBlock == null) return;

            double total = 0;

            if (CbEspresso.IsChecked == true && int.TryParse(TxtEspressoCount.Text, out int c1)) total += c1 * 100;

            if (CbCappuccino.IsChecked == true && int.TryParse(TxtCappuccinoCount.Text, out int c2)) total += c2 * 150;

            if (CbHotDog.IsChecked == true && int.TryParse(TxtHotDogCount.Text, out int c3)) total += c3 * 180;

            if (CbCroissant.IsChecked == true && int.TryParse(TxtCroissantCount.Text, out int c4)) total += c4 * 90;

            CafeTotalTextBlock.Text = total.ToString("F2");
        }

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            double.TryParse(FuelTotalTextBlock.Text, out double fuelTotal);
            double.TryParse(CafeTotalTextBlock.Text, out double cafeTotal);

            MessageBox.Show(
                $"--- ЧЕК ОПЛАТЫ ---\n\n" +
                $"Топливо: {fuelTotal:F2} руб.\n" +
                $"Кафетерий: {cafeTotal:F2} руб.\n" +
                $"-------------------\n" +
                $"ИТОГО К ОПЛАТЕ: {(fuelTotal + cafeTotal):F2} руб.",
                "Расчет стоимости", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}