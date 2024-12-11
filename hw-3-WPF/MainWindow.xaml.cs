using System.Windows;

namespace hw_3_WPF
{
    public partial class MainWindow : Window
    {
        private string numberToDisplay = "0";
        private double? previousResultValue = null;
        private string currentOperator = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DigitButton_Click(object sender, RoutedEventArgs e)
        {
            string clickedDigit = (sender as System.Windows.Controls.Button)?.Content.ToString();

            if (numberToDisplay == "0" && clickedDigit != ",")
                numberToDisplay = string.Empty;

            numberToDisplay += clickedDigit;
            UpdateDisplayedNumber();
        }

        private void DecimalButton_Click(object sender, RoutedEventArgs e)
        {
            if (!numberToDisplay.Contains(","))
            {
                numberToDisplay += ",";
                UpdateDisplayedNumber();
            }
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            string operatorClicked = (sender as System.Windows.Controls.Button)?.Content.ToString();

            if (!string.IsNullOrEmpty(numberToDisplay))
            {
                if (previousResultValue.HasValue && !string.IsNullOrEmpty(currentOperator))
                {
                    PerformCalculation();
                }
                else
                {
                    previousResultValue = double.Parse(numberToDisplay);
                }

                currentOperator = operatorClicked;
                numberToDisplay = "0";
                UpdateOperationHistory();
            }
        }

        private void CalculateResult_Click(object sender, RoutedEventArgs e)
        {
            if (previousResultValue.HasValue && !string.IsNullOrEmpty(currentOperator))
            {
                PerformCalculation();
                currentOperator = string.Empty;
                UpdateOperationHistory();
            }
        }

        private void ClearCurrentEntry_Click(object sender, RoutedEventArgs e)
        {
            numberToDisplay = "0";
            UpdateDisplayedNumber();
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            numberToDisplay = "0";
            previousResultValue = null;
            currentOperator = string.Empty;
            UpdateOperationHistory();
            UpdateDisplayedNumber();
        }

        private void RemoveLastDigit_Click(object sender, RoutedEventArgs e)
        {
            if (numberToDisplay.Length > 1)
            {
                numberToDisplay = numberToDisplay[..^1];
            }
            else
            {
                numberToDisplay = "0";
            }
            UpdateDisplayedNumber();
        }

        private void PerformCalculation()
        {
            try
            {
                double currentValue = double.Parse(numberToDisplay);

                if (previousResultValue.HasValue)
                {
                    previousResultValue = currentOperator switch
                    {
                        "+" => previousResultValue + currentValue,
                        "-" => previousResultValue - currentValue,
                        "*" => previousResultValue * currentValue,
                        "/" => currentValue != 0 ? previousResultValue / currentValue : throw new DivideByZeroException(),
                        _ => previousResultValue
                    };
                }
                else
                {
                    previousResultValue = currentValue;
                }

                numberToDisplay = previousResultValue.Value.ToString("0.####");
                UpdateDisplayedNumber();
            }
            catch (FormatException)
            {
                MessageBox.Show("Помилка: некоректний формат числа.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearAll_Click(null, null);
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Помилка: ділення на нуль.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearAll_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Невідома помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearAll_Click(null, null);
            }
        }

        private void UpdateDisplayedNumber()
        {
            DisplayedNumber.Text = numberToDisplay;
        }

        private void UpdateOperationHistory()
        {
            if (previousResultValue.HasValue && !string.IsNullOrEmpty(currentOperator))
            {
                OperationHistory.Text = $"{previousResultValue} {currentOperator}";
            }
            else
            {
                OperationHistory.Text = string.Empty;
            }
        }
    }
}
