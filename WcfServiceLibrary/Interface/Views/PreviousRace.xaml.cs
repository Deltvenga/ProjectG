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
        public PreviousRace()
        {
            InitializeComponent();
            
            int i = 0;
            serv.GetMarathon().ToList().ForEach(obj =>
            {
                MarathonCmBox.Items.Add(GetNewItem(obj, i));
                i++;
            });
            
            
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
            serv.GetEventType(MarathonCmBox.SelectedIndex).ToList().ForEach(obj => 
            {
                RaceEventCmBox.Items.Add(GetNewItem(obj[0],Convert.ToInt32(obj[1])));
            });
        }
    }
}
