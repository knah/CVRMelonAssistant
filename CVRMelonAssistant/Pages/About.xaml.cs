using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using static CVRMelonAssistant.Http;

namespace CVRMelonAssistant.Pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class About : Page
    {
        public static About Instance = new About();

        public About()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private async void HeadpatsButton_Click(object sender, RoutedEventArgs e)
        {
            PatButton.IsEnabled = false;
            await Task.Run(async () => await HeadPat());
            PatUp.IsOpen = true;
        }

        private async void HugsButton_Click(object sender, RoutedEventArgs e)
        {
            HugButton.IsEnabled = false;
            await Task.Run(async () => await Hug());
            HugUp.IsOpen = true;
        }

        private async Task<string> WeebCDN(string type)
        {
            var resp = await HttpClient.GetAsync(Utils.Constants.WeebCDNAPIURL + type + "/random");
            var body = await resp.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<Utils.WeebCDNRandomResponse>(body);
            return response.url;
        }

        private async Task HeadPat()
        {
            try
            {
                PatImage.Load(await WeebCDN("pats"));
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Utils.ShowErrorMessageBox("Oops! Can't get headpats right now!", ex);
                });
            }
        }

        private async Task Hug()
        {
            try
            {
                HugImage.Load(await WeebCDN("hugs"));
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Utils.ShowErrorMessageBox("Oops! Can't get hugs right now!", ex);
                });
            }
        }
    }
}
