using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Interface.Views
{
    /// <summary>
    /// Логика взаимодействия для PreviousRace.xaml
    /// </summary>
    public partial class PreviousRace : UserControl
    {
        private ServiceReference1.Service1Client serv = new ServiceReference1.Service1Client();

        Dictionary<int, string> Ages = new Dictionary<int, string>(6)
        {
            {0, "Under 18" },
            {1, "18 to 29" },
            {2, "30 to 39" },
            {3, "40 to 55" },
            {4, "56 to 70" },
            {5, "Over 70" }
        };
        
        
        public PreviousRace()
        {
            InitializeComponent();
            FillCmBoxes();
            
        }
        private void FillCmBoxes()
        {

            FillDeffinetlyCmBox(serv.GetMarathon().ToList(),MarathonCmBox);
            FillDeffinetlyCmBox(serv.GetGender(true).ToList(), GenderCmBox);
            FillDeffinetlyCmBox(Ages.Values.ToList(), AgeCmBox);
           
        }
        private void FillDeffinetlyCmBox (List<string> servList, ComboBox comboBox)
        {
            int i = 0;
            foreach (string obj in servList)
            {
                FillCmBox(comboBox, obj, i);
                i++;
                if (i == servList.Count)
                {
                    i = 0;
                }
            }
        }

        private void FillCmBox(ComboBox comboBox, string obj, int i)
        {
            comboBox.Items.Add(GetNewItem(obj, i));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = (MainWindow)Window.GetWindow(this);
            win.ChangeTab("MainScreen");
        }
        private ComboBoxItem GetNewItem(string itemName, int i)
        {
            ComboBoxItem item = new ComboBoxItem();
            item.Content = itemName;
            item.Tag = i;
            return item;
        }

        private void MarathonCmBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaceEventCmBox.Items.Clear();
            serv.GetEventType(MarathonCmBox.SelectedIndex).ToList().ForEach(obj => 
            {
                RaceEventCmBox.Items.Add(GetNewItem(obj[0],Convert.ToInt32(obj[1])));
            });
        }
    }
}
