using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Модуль_5_задание_5_WPF
{
    internal class Class1
    {
        // ---------- Цифры и запятая ----------
        public static void zap(TextBox textBox)
        {
            // Не даём поставить две запятые подряд
            if (textBox.Text.Contains(",")) return;
            textBox.AppendText(",");
        }

        // ---------- Ввод оператора ----------
        public static void Op(TextBox textBox, string op)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text)) return;

            string input = textBox.Text;

            // Если последний символ — уже оператор, заменяем его
            char last = input[input.Length - 1];
            if (last == '+' || last == '-' || last == '*' || last == '/')
            {
                textBox.Text = input.Substring(0, input.Length - 1) + op;
            }
            else
            {
                textBox.AppendText(op);
            }
        }

        // ---------- % ----------
        public static void Persent(TextBox textBox)
        {
            if (!TryGetNumber(textBox, out double a)) return;
            double per = a / 100.0;
            textBox.Text = per.ToString();
        }

        // ---------- +/- ----------
        public static void Reverse(TextBox textBox)
        {
            if (!TryGetNumber(textBox, out double a)) return;
            textBox.Text = (-a).ToString();
        }

        // ---------- C ----------
        public static void Cl(TextBox textBox)
        {
            textBox.Clear();
        }

        // ---------- DEL (удалить последний символ) ----------
        public static void Del(TextBox textBox)
        {
            if (textBox.Text.Length > 0)
                textBox.Text = textBox.Text.Substring(0, textBox.Text.Length - 1);
        }

        // ---------- 1/X ----------
        public static void Drob(TextBox textBox)
        {
            if (!TryGetNumber(textBox, out double a)) return;
            if (a == 0)
            {
                System.Windows.MessageBox.Show("Деление на ноль!");
                return;
            }
            textBox.Text = (1.0 / a).ToString();
        }

        // ---------- X^2 ----------
        public static void Pow(TextBox textBox)
        {
            if (!TryGetNumber(textBox, out double a)) return;
            textBox.Text = Math.Pow(a, 2).ToString();
        }

        // ---------- √X ----------
        public static void Sqrt(TextBox textBox)
        {
            if (!TryGetNumber(textBox, out double a)) return;
            if (a < 0)
            {
                System.Windows.MessageBox.Show("Корень из отрицательного числа!");
                return;
            }
            textBox.Text = Math.Sqrt(a).ToString();
        }

        // ---------- = ----------
        public static void RAVNO(TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text)) return;

            string input = textBox.Text.Trim();

            // Ищем оператор, начиная со 2-го символа
            // (чтобы не спутать с унарным минусом в начале)
            int opIndex = input.IndexOfAny(new char[] { '+', '-', '*', '/' }, 1);
            if (opIndex == -1)
            {
                // Нет оператора — ничего не делаем
                return;
            }

            string firstStr = input.Substring(0, opIndex).Trim();
            char operation = input[opIndex];
            string secondStr = input.Substring(opIndex + 1).Trim();

            if (!double.TryParse(firstStr, out double firstNumber) ||
                !double.TryParse(secondStr, out double secondNumber))
            {
                System.Windows.MessageBox.Show("Некорректное выражение!");
                return;
            }

            double result = 0;
            switch (operation)
            {
                case '+':
                    result = firstNumber + secondNumber;
                    break;
                case '-':
                    result = firstNumber - secondNumber;
                    break;
                case '*':
                    result = firstNumber * secondNumber;
                    break;
                case '/':
                    if (secondNumber == 0)
                    {
                        System.Windows.MessageBox.Show("Деление на ноль!");
                        return;
                    }
                    result = firstNumber / secondNumber;
                    break;
            }

            textBox.Text = result.ToString();
        }

        // ---------- Вспомогательный метод ----------
        private static bool TryGetNumber(TextBox textBox, out double value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                System.Windows.MessageBox.Show("Введите число!");
                return false;
            }

            if (!double.TryParse(textBox.Text, out value))
            {
                System.Windows.MessageBox.Show("Некорректное число!");
                return false;
            }

            return true;
        }
    }
}
