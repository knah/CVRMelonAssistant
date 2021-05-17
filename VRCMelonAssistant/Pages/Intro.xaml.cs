using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace VRCMelonAssistant.Pages
{
    /// <summary>
    /// Interaction logic for Intro.xaml
    /// </summary>
    public partial class Intro : Page
    {
        public static Intro Instance = new Intro();

        public Intro()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Disagree_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ModsButton.IsEnabled = false;
            Properties.Settings.Default.Agreed = false;
            Properties.Settings.Default.Save();
            MessageBox.Show((string)FindResource("Intro:ClosingApp"));
            Application.Current.Shutdown();
        }

        private void Agree_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ModsButton.IsEnabled = true;

            string text = (string) FindResource("Intro:ModsTabEnabled");
            Utils.SendNotify(text);
            MainWindow.Instance.MainText = text;
            Properties.Settings.Default.Agreed = true;
            Properties.Settings.Default.Save();
        }
    }
}
