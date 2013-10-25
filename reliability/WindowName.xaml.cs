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

namespace reliability
{
    /// <summary>
    /// Interaction logic for WindowName.xaml
    /// </summary>
    public partial class WindowName : Window
    {
        public WindowName()
        {
            InitializeComponent();
            TbName.Focus();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TbName.Text))
            {
                MessageBox.Show("Введіть коректне ім'я");
                return;
            }
            bool IsInBase = false;
            foreach (var element in MainWindow.exportedElements)
            {
                if (element.Name == TbName.Text)
                {
                    MessageBox.Show(TbName.Text + " вже існує в базі");
                    IsInBase = true;
                    break;
                }
            }
            if(IsInBase) return;
            AddElementWindow.GettedName = TbName.Text;
            Close();
        }

        //якшо відкрили і тупо закрили вікно, то назва = null
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TbName.Text))
                AddElementWindow.GettedName = null;
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            //AddElementWindow.GettedName = null;
        }
    }
}
