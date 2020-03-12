using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;

namespace Interface.Views
{
    /// <summary>
    /// Логика взаимодействия для PreviousRace.xaml
    /// </summary>
    public partial class PreviousRace : UserControl
    {
        public PreviousRace()
        {
            InitializeComponent();
            var serv = new ServiceReference1.Service1Client();
            MarathonCmBox.ItemsSource = serv.GetMarathon();
            serv.GetEventType().ToList().ForEach(item => RaceEventCmBox.Items.Add(item[1]));
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = (MainWindow)Window.GetWindow(this);
            win.ChangeTab("MainScreen");
        }
    }
}
