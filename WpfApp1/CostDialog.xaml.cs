using System;
using System.Collections.Generic;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для CostDialog.xaml
    /// </summary>
    public partial class CostDialog : Window
    {

        int result;

        public int Cost { get { return result; } }

        public CostDialog()
        {
            result = 1;
            InitializeComponent();

            costBox.MaxLength = 5;
            costBox.Focus();
        }

        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
                e.Handled = true;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (costBox.Text != "")
                result = int.Parse(costBox.Text);
            else
                result = 1;
            DialogResult = true;
        }
    }
}
