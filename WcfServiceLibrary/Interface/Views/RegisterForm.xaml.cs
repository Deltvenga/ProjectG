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


        //проверка регистрации по кнопке
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
          //  MessageBox.Show(FirstPasswordBox.Password);
            if (EmailTextBox.Text != "" && FirstPasswordBox.Password != "" && SecondPasswordBox.Password != "" && FirstNameTextBox.Text != "" && LastNameTextBox.Text != "" && GenderComboBox.Text != "" && DateOfBirthDatePicker.Text != "" && CountryComboBox.Text != "")
            {
                //подготовка к проверке
                bool isFill = true;
                CheckRegisterForm formCheck = new CheckRegisterForm();
                FillCheckRegisterFormClass(formCheck);

                //выполняю проверку в классе
                bool[] checkForm = formCheck.CheckForm();
                string errorMessage = "Неправильное заполнение:\n ";

                //проверка проверки в классе
                FillingCheck(ref isFill, checkForm, ref errorMessage);

                //проверка
                if (isFill)
                {
                    MessageBox.Show("Ураааааа");
                }
                else MessageBox.Show(errorMessage);
            }
            else MessageBox.Show("Проверьте заполнение всех полей");
        }

        private void FillCheckRegisterFormClass(CheckRegisterForm formCheck)
        {
            formCheck.email = EmailTextBox.Text.ToString();
            formCheck.pass = FirstPasswordBox.Password.ToString();
            formCheck.confpass = SecondPasswordBox.Password.ToString();
            formCheck.firstname = FirstNameTextBox.Text.ToString();
            formCheck.lastname = LastNameTextBox.Text.ToString();
            formCheck.dateOfBirth = Convert.ToDateTime(DateOfBirthDatePicker.SelectedDate.Value.ToShortDateString());
        }

        private static void FillingCheck(ref bool isFill, bool[] checkForm, ref string errorMessage)
        {
            if (!checkForm[0])
            {
                errorMessage += "Email\n ";
                isFill = false;
            }
            if (!checkForm[1])
            {
                errorMessage += "Пароли не совпадают или длина меньше 6\n";
                isFill = false;
            }
            if (!checkForm[2])
            {
                errorMessage += "Имя\n ";
                isFill = false;
            }
            if (!checkForm[3])
            {
                errorMessage += "Фамилия\n ";
                isFill = false;
            }
            if (!checkForm[4])
            {
                errorMessage += "Дата рождения указана неверно";
                isFill = false;
            }
        }



        //добавление данных в комбобоксы
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
