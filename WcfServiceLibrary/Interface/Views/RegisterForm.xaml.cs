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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Interface.Views
{
    /// <summary>
    /// Логика взаимодействия для RegisterForm.xaml
    /// </summary>
    public partial class RegisterForm : UserControl
    {

        private ServiceReference1.Service1Client serv = new ServiceReference1.Service1Client();


        public RegisterForm()
        {

            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = (MainWindow)Window.GetWindow(this);
            win.ChangeTab("MainScreen");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow win = (MainWindow)Window.GetWindow(this);
            win.ChangeTab("MainScreen");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            AddValuesComboBox(serv.GetGender(true).ToList(), GenderComboBox);
            AddValuesComboBox(serv.GetCountry().ToList(), CountryComboBox);
    
        }

        private void AddValuesComboBox(List<string> genderList, ComboBox comboBox)
        {
            int i = 0;
            foreach (string obj in genderList)
            {
                FillCmBox(comboBox, obj, i);
                i++;
            }
        }

        private void FillCmBox(ComboBox comboBox, string obj, int i)
        {
            comboBox.Items.Add(GetNewItem(obj, i));
        }

        private ComboBoxItem GetNewItem(string itemName, dynamic i)
        {
            ComboBoxItem item = new ComboBoxItem();
            item.Content = itemName;
            item.Tag = i;
            return item;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
