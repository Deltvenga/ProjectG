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
        ComboBoxItem currentMarathonItem;
        ComboBoxItem currentEventRaceItem;

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

            FillDeffinetlyCmBox(serv.GetMarathon().ToList(), MarathonCmBox);
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
        private ComboBoxItem GetNewItem(string itemName, dynamic i)
        {
            ComboBoxItem item = new ComboBoxItem();
            item.Content = itemName;
            item.Tag = i;
            return item;
        }
  

        private void MarathonCmBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaceEventCmBox.Items.Clear();
            var keks = serv.GetEventType(MarathonCmBox.SelectedIndex);
            foreach(string[] obj in keks)
            {
                RaceEventCmBox.Items.Add(GetNewItem(obj[1], obj[0]));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            currentMarathonItem = (ComboBoxItem)MarathonCmBox.SelectedItem;
            currentEventRaceItem = (ComboBoxItem)RaceEventCmBox.SelectedItem;
            var GridData = serv.GetPreviousResult(18, 30, Convert.ToInt32(currentMarathonItem.Tag), currentEventRaceItem.Tag.ToString(), "Male");
        }

    }
}
