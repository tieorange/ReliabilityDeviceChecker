using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace reliability
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public List<GroupOfElements> ElementsGroupList = new List<GroupOfElements>(); //список груп подібних елементів та їх к-сть
        public List<Element> Elements = new List<Element>(); //Список доданих елементів ВСІХ
        List<Tuple<string, double, double>> types2List = new List<Tuple<string, double, double>>(); //Список Типів2
        static public List<ListElement> exportedElements; //експортовані елементи списка
        private int selectedElement, selectedType1, selectedType2;
        public MainWindow()
        {
            InitializeComponent();
            //Element tmp = new Element("name", "type1", "type2", 1,2, 5);
            //SerializeToXML(tmp);
            //List<ListType2> listType2s = new List<ListType2>();
            //listType2s.Add(new ListType2("0,5 Вт", 2, 2.5));
            //listType2s.Add(new ListType2("0,5 Вт", 2, 2.5));
            //listType2s.Add(new ListType2("0,5 Вт", 2, 2.5));

            //List<ListType1> listType1s = new List<ListType1>();
            //listType1s.Add(new ListType1(listType2s, "недротяний потужністю:"));

            //List<ListElement> listElements = new List<ListElement>();
            //listElements.Add(new ListElement(listType1s, "резистор"));

            //List<ListType1> listType1s2 = new List<ListType1>();
            //listType1s2.Add(new ListType1("паперовий та металопаперовий", 2, 6));
            //listType1s2.Add(new ListType1("керамічний", 1.5, 2));
            //listElements.Add(new ListElement(listType1s2, "Конденсатор:"));
            //SerializeToXML(listElements);
            try
            {
                exportedElements = DeserializeFromXML();
            }
            catch (Exception)
            {
                exportedElements = new List<ListElement>();
                MessageBox.Show("Помилка завантаження бази. Створена тимчасова");
            }
            CreateLists(exportedElements);
        }

        void CreateLists(List<ListElement> exported)
        {
            CbElement.SelectedIndex = -1;
            CbElement.Items.Clear();
            foreach (var listElement in exported)
                CbElement.Items.Add(listElement.Name); //добавили  елементи
        }
        //Коли міняється вибраний обєкт в "Елемент"
        private void CbElement_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            CbType1.SelectedIndex = -1;
            CbType2.SelectedIndex = -1;
            TbIntens1.Clear();
            TbIntens2.Clear();
            if (CbElement.SelectedValue == null) return;
            string selectedElementName = CbElement.SelectedValue.ToString(); //get selected Element name
            CbType1.Items.Clear();

            for (int index = 0; index < exportedElements.Count; index++)
            {
                ListElement element = exportedElements[index];
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
            if (CbType1.SelectedValue == null) return;
            string selectedType1Name = CbType1.SelectedValue.ToString(); //get selected Type1 name
            CbType2.SelectedIndex = -1;
            CbType2.Items.Clear();
            CbType2.IsEnabled = true;
            if (CbType1.SelectedItem != null)
            {
                for (int i = 0; i < exportedElements[selectedElement].Type1s.Count; i++)//получяєм індекс вибраного типа1
                    if (exportedElements[selectedElement].Type1s[i].Name == selectedType1Name)
                    {
                        selectedType1 = i;
                        var listType2s = exportedElements[selectedElement].Type1s[i].Type2s;
                        if (listType2s == null || listType2s.Count <= 0)
                        {
                            TbIntens1.Text = exportedElements[selectedElement].Type1s[i].Intensity1.ToString();
                            TbIntens2.Text = exportedElements[selectedElement].Type1s[i].Intensity2.ToString();
                            CbType2.IsEnabled = true;
                        }
                        else
                        {
                            var type2Ses = exportedElements[selectedElement].Type1s[i].Type2s;
                            if (type2Ses != null)
                                foreach (var type2s in type2Ses)
                                {
                                    CbType2.Items.Add(type2s.Name);
                                }
                        }

                    }

            }
        }
        //Стоврення типів1  для кожного Елементу
        //internal void CreatingCbType1(string elementName)
        //{
        //    ObservableCollection<string> type1 = new ObservableCollection<string>();

        //    switch (elementName)
        //    {
        //        case "Резистор":
        //            {
        //                type1.Add("недротяний потужністю:");
        //                type1.Add("вугільний:");
        //                type1.Add("дротяний потужністю:");
        //                type1.Add("Потенціометри змінні:");
        //                foreach (var item in type1)
        //                {
        //                    CbType1.Items.Add(item);
        //                }
        //            }
        //            break;
        //    }

        //}
        //public void CreatingCbType2(string type1Name)
        //{
        //    //Tuple<string, double, double> tmpPair; // екземпляр пари "Тип2"
        //    switch (type1Name)
        //    {
        //        case "недротяний потужністю:":
        //            {
        //                types2List.Add(new Tuple<string, double, double>("0,5 Вт", 2, 2.5));
        //                types2List.Add(new Tuple<string, double, double>("1 Вт", 3, 4));
        //                types2List.Add(new Tuple<string, double, double>("2 Вт", 3.4, 4));
        //            }
        //            break;
        //        case "вугільний:":
        //            {
        //                types2List.Add(new Tuple<string, double, double>("плівковий", 0.03, 0.03));
        //                types2List.Add(new Tuple<string, double, double>("композиційний", 0.043, 0.043));
        //                types2List.Add(new Tuple<string, double, double>("презиційний", 0.125, 0.125));
        //            }
        //            break;
        //        case "дротяний потужністю:":
        //            {
        //                types2List.Add(new Tuple<string, double, double>("2.5...30 Вт", 2, 4));
        //                types2List.Add(new Tuple<string, double, double>("50 Вт", 14, 14));
        //            }
        //            break;
        //        case "Потенціометри змінні:":
        //            {
        //                types2List.Add(new Tuple<string, double, double>("недротяний", 0.1, 3));
        //                types2List.Add(new Tuple<string, double, double>("дротяний", 5, 15));
        //            }
        //            break;
        //    }
        //    //Заповнюємо "Тип2"
        //    foreach (var item in types2List)
        //    {
        //        CbType2.Items.Add(item.Item1);
        //    }
        //}

        private void CbType2_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (CbType2.SelectedValue != null)
            {
                string selectedType2Name = CbType2.SelectedValue.ToString(); //get selected Type1 name
                for (int i = 0; i < exportedElements[selectedElement].Type1s[selectedType1].Type2s.Count; i++)//получяєм індекс вибраного типа2
                    if (exportedElements[selectedElement].Type1s[selectedType1].Type2s[i].Name == selectedType2Name)
                    {
                        selectedType2 = i;
                    }
            }
            TbIntens1.Text = exportedElements[selectedElement].Type1s[selectedType1].Type2s[selectedType2].Intensity1.ToString();
            TbIntens2.Text = exportedElements[selectedElement].Type1s[selectedType1].Type2s[selectedType2].Intensity2.ToString();

        }


        private void BtnAddElement_Click(object sender, RoutedEventArgs e)
        {
            if (Elements == null || CbElement.SelectedValue == null || CbType1.SelectedItem == null
                || String.IsNullOrWhiteSpace(TbCoefficient.Text)
                || String.IsNullOrWhiteSpace(TbCount.Text) || String.IsNullOrWhiteSpace(TbIntens1.Text)
                || String.IsNullOrWhiteSpace(TbIntens2.Text) || String.IsNullOrWhiteSpace(TbHours.Text))
            {
                MessageBox.Show("Заповніть будь-ласка УСІ поля");
                return;
            }

            Element tmp = new Element(); //тимчасовий список груп подібних елементів 
            for (int i = 0; i < Convert.ToInt32(TbCount.Text); i++)
            {
                if (CbType2.SelectedItem != null)
                {
                    if (TbIntens1.Text == "." || TbIntens2.Text == ".") return;
                    tmp = new Element(Elements.Count + 1,
                                      CbElement.SelectedValue.ToString(),
                                      CbType1.SelectedItem.ToString(),
                                      CbType2.SelectedItem.ToString(),
                                      Convert.ToDouble(TbIntens1.Text.Replace('.', ',')),
                                      Convert.ToDouble(TbIntens2.Text.Replace('.', ',')),
                                      Int32.Parse(TbCoefficient.Text));
                }
                else
                {
                    if (TbIntens1.Text == "." || TbIntens2.Text == ".") return;
                    tmp = new Element(Elements.Count + 1,
                                      CbElement.SelectedValue.ToString(),
                                      CbType1.SelectedItem.ToString(),
                                      null,
                                      Convert.ToDouble(TbIntens1.Text.Replace('.', ',')),
                                      Convert.ToDouble(TbIntens2.Text.Replace('.', ',')),
                                      Int32.Parse(TbCoefficient.Text));
                }

                Elements.Add(tmp); //додаєм елемент в список доданих елементів (стікі разів скікі вказано в "кількість")
                //tmpElemnt = tmp; //додаємо подібні елементи тимчасово (для додавання в группу)
                //tmp.IncrementNumber();//збільшуємо номер елемента
                Table.Items.Add(tmp); //додаєм елемент в таблицю
            }
            ElementsGroupList.Add(new GroupOfElements(tmp, Convert.ToInt32(TbCount.Text))); //додаємо группу поодібних та їх кількість
            //tmpElemntsList.Clear(); //очищаємо тимчасовий списко шоб наступного разу нова група записалась
            Calculating();
        }

        void Calculating()
        {
            int t = Convert.ToInt32(TbHours.Text); //якась константа дана

            //MAX
            double SumMax = 0;
            for (int i = 0; i < ElementsGroupList.Count; i++)
            {
                double tmp = ElementsGroupList[i].Element.Intensity2 * ElementsGroupList[i].Count *
                             ElementsGroupList[i].Element.Coefficient * Math.Pow(10, -6);
                SumMax += tmp;
            }
            decimal roundMax = (decimal)SumMax; //переводимо в десяткове шоб коректно відображалось
            TbMax.Text = roundMax.ToString();

            //MIN
            double SumMin = 0;
            for (int i = 0; i < ElementsGroupList.Count; i++)
            {
                double tmp = ElementsGroupList[i].Element.Intensity1 * ElementsGroupList[i].Count *
                             ElementsGroupList[i].Element.Coefficient * Math.Pow(10, -6);
                SumMax += tmp;
            }
            decimal roundMin = (decimal)SumMax; //переводимо в десяткове шоб коректно відображалось
            TbMin.Text = roundMin.ToString();
            //MIDDLE
            double middle = (SumMax + SumMin) / 2;
            decimal roundMiddle = (decimal)middle;
            TbMiddle.Text = roundMiddle.ToString();

            ////Ймовірність безвідмовної роботи протягом
            //decimal workingHours = 1 / roundMiddle;
            //TbWorkingHours.Text = workingHours.ToString();

            //P(t)
            decimal Pt = (decimal)Math.Exp((-middle * t));
            TbPt.Text = Pt.ToString();

            //Tср
            decimal roundTsr = 1 / roundMiddle;
            TbTsr.Text = ((int)roundTsr).ToString();
        }

        //Перевірка чі текстбос - ціле число
        private void IsDigit_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));

            //try
            //{
            //    Convert.ToInt32(e.Text);
            //}
            //catch
            //{
            //    e.Handled = true;
            //}

            //foreach (char ch in e.Text)
            //    if (!(Char.IsDigit(ch) || ch.Equals('.')))
            //    {
            //        e.Handled =true;
            //    }


        }
        //Перевірка чі текстбос - ДЕСЯТКОВЕ ЧИСЛО число
        private void IsDecimal_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }


        static public void SerializeToXML(List<ListElement> listElements)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ListElement>));
            TextWriter textWriter = new StreamWriter(@"ElementBase.xml");
            serializer.Serialize(textWriter, listElements);
            textWriter.Close();
        }
        static List<ListElement> DeserializeFromXML()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<ListElement>));
            TextReader textReader = new StreamReader(@"ElementBase.xml");
            List<ListElement> list;
            list = (List<ListElement>)deserializer.Deserialize(textReader);
            textReader.Close();

            return list;
        }

        private void BtnAddNewElement_Click_1(object sender, RoutedEventArgs e)
        {
            AddElementWindow wnd = new AddElementWindow();
            wnd.ShowDialog();
            CreateLists(exportedElements);
        }

        private void Table_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            if(e.Key.ToString() == "Delete")
            {
                Table.Items.Clear();
                ElementsGroupList.Clear();
                Elements.Clear();
                TbMax.Clear();
                TbMin.Clear();
                TbMiddle.Clear();
                TbPt.Clear();
                TbTsr.Clear();
            }
        }

        private void BtnDelElement_Click_1(object sender, RoutedEventArgs e)
        {
            DeleteElementWindow wnd = new DeleteElementWindow();
            wnd.ShowDialog();
            CreateLists(exportedElements);
        }
    }
}
