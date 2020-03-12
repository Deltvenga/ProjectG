using System.Windows;
using System.Windows.Controls;

namespace Interface.Views
{
    /// <summary>
    /// Логика взаимодействия для PreviousRace.xaml
    /// </summary>
    public partial class PreviousRace : UserControl
    {
        public PreviousRace()
        {
            var serv = new ServiceReference1.Service1Client();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = (MainWindow)Window.GetWindow(this);
            win.ChangeTab("MainScreen");
        }
    }
}
