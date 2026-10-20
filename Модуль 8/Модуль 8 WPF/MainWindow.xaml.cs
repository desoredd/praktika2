using System.Windows;
using System.Windows.Controls;
using Модуль_8_WPF;
namespace _8
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // Цифры
        private void BtnDigit_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            textBox1.AppendText(btn.Content.ToString());
        }
        // Операции
        private void BtnOp_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null || btn.Tag == null) return;
            Class1.Op(textBox1, btn.Tag.ToString());
        }
        // Запятая
        private void BtnZap_Click(object sender, RoutedEventArgs e)
        {
            Class1.zap(textBox1);
        }
        // Специальные операции
        private void BtnCl_Click(object sender, RoutedEventArgs e) => Class1.Cl(textBox1);
        private void BtnDel_Click(object sender, RoutedEventArgs e) => Class1.Del(textBox1);
        private void BtnReverse_Click(object sender, RoutedEventArgs e) => Class1.Reverse(textBox1);
        private void BtnDrob_Click(object sender, RoutedEventArgs e) => Class1.Drob(textBox1);
        private void BtnPow_Click(object sender, RoutedEventArgs e) => Class1.Pow(textBox1);
        private void BtnSqrt_Click(object sender, RoutedEventArgs e) => Class1.Sqrt(textBox1);
        private void BtnRavno_Click(object sender, RoutedEventArgs e) => Class1.RAVNO(textBox1);
    }
}