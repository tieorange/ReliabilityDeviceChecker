using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddElementWindow.xaml
    /// </summary>
    public partial class AddElementWindow : Window
    {
        private int selectedElement, selectedType1, selectedType2;
        public static string GettedName = null;
        public static List<ListElement> TmpElementsList = new List<ListElement>();
        public AddElementWindow()
        {
            InitializeComponent();

            CreateLists(MainWindow.exportedElements);
            TmpElementsList = MainWindow.exportedElements;
        }
        void CreateLists(List<ListElement> exported)
        {
            CbElement.SelectedIndex = -1;
            CbElement.Items.Clear();
            foreach (var listElement in exported)
            {
                CbElement.Items.Add(listElement.Name); //добавили  елементи
            }
        }

        private void CbElement_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (CbElement.SelectedValue == null)
            {
                BtnAddNewType1.IsEnabled = false;
                BtnAddNewType2.IsEnabled = false;
            }
            CbType2.Items.Clear();
            BtnAddNewType1.IsEnabled = true;
            CbType1.SelectedIndex = -1;
            CbType2.SelectedIndex = -1;
            TbIntens1.Clear();
            TbIntens2.Clear();
            if (CbElement.SelectedValue == null) return;
            string selectedElementName = CbElement.SelectedValue.ToString(); //get selected Element name
            CbType1.Items.Clear();

            for (int index = 0; index < TmpElementsList.Count; index++)
            {
                ListElement element = TmpElementsList[index];
                if (element.Name == selectedElementName)
                {
                    selectedElement = index; //індекс вибраного елемента
                    if (element.Type1s != null)
                        foreach (var type1s in element.Type1s)
                        {
                            CbType1.Items.Add(type1s.Name);
                        }
                }
            }
        }

        //Коли міняється вибраний обєкт в "Тип1"
        private void CbType1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (CbType1.SelectedValue == null)
            {
                BtnAddNewType2.IsEnabled = false;
                return;
            }
            TbIntens1.Clear();
            TbIntens2.Clear();
            BtnAddNewType2.IsEnabled = true;
            string selectedType1Name = CbType1.SelectedValue.ToString(); //get selected Type1 name
            CbType2.SelectedIndex = -1;
            CbType2.Items.Clear();
            CbType2.IsEnabled = true;
            if (CbType1.SelectedItem != null)
            {
                for (int i = 0; i < TmpElementsList[selectedElement].Type1s.Count; i++)//получяєм індекс вибраного типа1
                    if (TmpElementsList[selectedElement].Type1s[i].Name == selectedType1Name)
                    {
                        selectedType1 = i;
                        var listType2s = TmpElementsList[selectedElement].Type1s[i].Type2s;
                        if (listType2s != null && listType2s.Count <= 0)
                        {
                            TbIntens1.Text = TmpElementsList[selectedElement].Type1s[i].Intensity1.ToString();
                            TbIntens2.Text = TmpElementsList[selectedElement].Type1s[i].Intensity2.ToString();
                            CbType2.IsEnabled = true;
                        }
                        else
                        {
                            var type2Ses = TmpElementsList[selectedElement].Type1s[i].Type2s;
                            if (type2Ses != null)
                                foreach (var type2s in type2Ses)
                                {
                                    CbType2.Items.Add(type2s.Name);
                                }
                        }
                    }

            }
        }

        private void CbType2_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (CbType2.SelectedValue != null)
            {
                string selectedType2Name = CbType2.SelectedValue.ToString(); //get selected Type1 name
                for (int i = 0; i < TmpElementsList[selectedElement].Type1s[selectedType1].Type2s.Count; i++)//получяєм індекс вибраного типа2
                    if (TmpElementsList[selectedElement].Type1s[selectedType1].Type2s[i].Name == selectedType2Name)
                    {
                        selectedType2 = i;
                    }
            }
            TbIntens1.Text = TmpElementsList[selectedElement].Type1s[selectedType1].Type2s[selectedType2].Intensity1.ToString();
            TbIntens2.Text = TmpElementsList[selectedElement].Type1s[selectedType1].Type2s[selectedType2].Intensity2.ToString();
        }
        private void IsDecimal_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void BtnAddNewListElement_Click_1(object sender, RoutedEventArgs e)
        {
            //очищаємо всі контроли шо нижче
            //CbElement.SelectedIndex = -1;
            //CbElement.Items.Clear();
            CbType1.Items.Clear();
            CbType2.Items.Clear();
            TbIntens1.Clear();
            TbIntens2.Clear();
            WindowName wndName = new WindowName();
            wndName.ShowDialog();
            if (GettedName == null) return;
            TmpElementsList.Add(new ListElement(GettedName));
            //MainWindow.SerializeToXML(TmpElementsList);
            CreateLists(TmpElementsList);
            CbElement.SelectedIndex = TmpElementsList.Count - 1;
        }

        private void BtnAddNewType1_Click(object sender, RoutedEventArgs e)
        {
            WindowName wndName = new WindowName();
            wndName.ShowDialog();
            if (GettedName == null) return;
            if (String.IsNullOrWhiteSpace(TbIntens1.Text) || String.IsNullOrWhiteSpace(TbIntens2.Text))
            {
                if (TmpElementsList[selectedElement].Type1s != null)
                    TmpElementsList[selectedElement].Type1s.Add(new ListType1(GettedName));
                else
                {
                    TmpElementsList[selectedElement].Type1s = new List<ListType1> { new ListType1(GettedName) };
                }
            }
            else
            {
                if (TmpElementsList[selectedElement].Type1s != null)
                    TmpElementsList[selectedElement].Type1s.Add(new ListType1(GettedName, Convert.ToDouble(TbIntens1.Text.Replace('.', ',')),
                                                                              Convert.ToDouble(TbIntens2.Text.Replace('.', ','))));
                else
                {
                    TmpElementsList[selectedElement].Type1s = new List<ListType1>
                                                                  {
                                                                      new ListType1(GettedName,
                                                                                    Convert.ToDouble(TbIntens1.Text.Replace('.', ',')),
                                                                                    Convert.ToDouble(TbIntens2.Text.Replace('.', ',')))
                                                                  };
                }
            }
            CbElement.SelectedIndex = -1;
            CbElement.SelectedIndex = selectedElement;
            CbType1.SelectedIndex = TmpElementsList[selectedElement].Type1s.Count - 1;
        }

        private void BtnAddNewType2_Click(object sender, RoutedEventArgs e)
        {
            WindowName wndName = new WindowName();
            wndName.ShowDialog();
            if (GettedName == null) return;
            var listType2s = TmpElementsList[selectedElement].Type1s[selectedType1].Type2s;
            if (listType2s != null)
                TmpElementsList[selectedElement].Type1s[selectedType1].Type2s.Add(new ListType2(GettedName));
            else
            {
                TmpElementsList[selectedElement].Type1s[selectedType1].Type2s = new List<ListType2> { new ListType2(GettedName) };
            }
            CbType1.SelectedIndex = -1;
            CbType1.SelectedIndex = selectedType1;
            CbType2.SelectedIndex = TmpElementsList[selectedElement].Type1s[selectedType1].Type2s.Count - 1;
            TbIntens1.Clear();
            TbIntens2.Clear();
        }

        private void BtnAddNewElementToBase_Click_1(object sender, RoutedEventArgs e)
        {
            if (CbElement.SelectedValue == null || CbType1.SelectedItem == null || String.IsNullOrWhiteSpace(TbIntens1.Text))
            {
                MessageBox.Show("Заповніть будь-ласка УСІ поля");
                return;
            }
            if (CbType2.SelectedItem != null)
            {
                if (TbIntens1.Text == "." || TbIntens2.Text == ".") return;
                if (String.IsNullOrWhiteSpace(TbIntens2.Text))
                {
                    TmpElementsList[selectedElement].Type1s[selectedType1].Type2s[selectedType2].Intensity1 = Convert.ToDouble(TbIntens1.Text.Replace('.', ','));
                    TmpElementsList[selectedElement].Type1s[selectedType1].Type2s[selectedType2].Intensity2 = Convert.ToDouble(TbIntens1.Text.Replace('.', ','));
                }
                else if (String.IsNullOrWhiteSpace(TbIntens1.Text))
                {
                    TmpElementsList[selectedElement].Type1s[selectedType1].Type2s[selectedType2].Intensity1 = Convert.ToDouble(TbIntens2.Text.Replace('.', ','));
                    TmpElementsList[selectedElement].Type1s[selectedType1].Type2s[selectedType2].Intensity2 = Convert.ToDouble(TbIntens2.Text.Replace('.', ','));
                }
                else
                {
                    TmpElementsList[selectedElement].Type1s[selectedType1].Type2s[selectedType2].Intensity1 = Convert.ToDouble(TbIntens1.Text.Replace('.', ','));
                    TmpElementsList[selectedElement].Type1s[selectedType1].Type2s[selectedType2].Intensity2 = Convert.ToDouble(TbIntens2.Text.Replace('.', ','));
                }
            }
            else
            {
                if (TbIntens1.Text == "." || TbIntens2.Text == ".") return;
                if (String.IsNullOrWhiteSpace(TbIntens2.Text))
                {
                    TmpElementsList[selectedElement].Type1s[selectedType1].Intensity1 = Convert.ToDouble(TbIntens1.Text.Replace('.', ','));
                    TmpElementsList[selectedElement].Type1s[selectedType1].Intensity2 = Convert.ToDouble(TbIntens1.Text.Replace('.', ','));
                }
                else if (String.IsNullOrWhiteSpace(TbIntens1.Text))
                {
                    TmpElementsList[selectedElement].Type1s[selectedType1].Intensity1 = Convert.ToDouble(TbIntens2.Text.Replace('.', ','));
                    TmpElementsList[selectedElement].Type1s[selectedType1].Intensity2 = Convert.ToDouble(TbIntens2.Text.Replace('.', ','));
                }
                else
                {
                    TmpElementsList[selectedElement].Type1s[selectedType1].Intensity1 = Convert.ToDouble(TbIntens1.Text.Replace('.', ','));
                    TmpElementsList[selectedElement].Type1s[selectedType1].Intensity2 = Convert.ToDouble(TbIntens2.Text.Replace('.', ','));
                }
            }
            MainWindow.exportedElements = TmpElementsList;
            MainWindow.SerializeToXML(TmpElementsList);
            MessageBox.Show("Додано");
        }

    }
}
