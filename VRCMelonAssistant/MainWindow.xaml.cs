using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VRCMelonAssistant.Pages;
using static VRCMelonAssistant.Http;

namespace VRCMelonAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        public static bool ModsOpened = false;
        public static bool ModsLoading = false;

        public string MainText
        {
            get
            {
                return MainTextBlock.Text;
            }
            set
            {
                Dispatcher.Invoke(new Action(() => { Instance.MainTextBlock.Text = value; }));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            const int ContentWidth = 1280;
            const int ContentHeight = 720;

            double ChromeWidth = SystemParameters.WindowNonClientFrameThickness.Left + SystemParameters.WindowNonClientFrameThickness.Right;
            double ChromeHeight = SystemParameters.WindowNonClientFrameThickness.Top + SystemParameters.WindowNonClientFrameThickness.Bottom;
            double ResizeBorder = SystemParameters.ResizeFrameVerticalBorderWidth;

            Width = ChromeWidth + ContentWidth + 2 * ResizeBorder;
            Height = ChromeHeight + ContentHeight + 2 * ResizeBorder;

            VersionText.Text = App.Version;

            Themes.LoadThemes();
            Themes.FirstLoad(Properties.Settings.Default.SelectedTheme);

            if (Properties.Settings.Default.Agreed)
                Instance.ModsButton.IsEnabled = true;

            if (!Properties.Settings.Default.Agreed || string.IsNullOrEmpty(Properties.Settings.Default.LastTab))
            {
                Main.Content = Intro.Instance;
            }
            else
            {
                switch (Properties.Settings.Default.LastTab)
                {
                    case "Intro":
                        Main.Content = Intro.Instance;
                        break;
                    case "Mods":
                        ShowModsPage().NoAwait();
                        break;
                    case "About":
                        Main.Content = About.Instance;
                        break;
                    case "Options":
                        Main.Content = Options.Instance;
                        Themes.LoadThemes();
                        break;
                    default:
                        Main.Content = Intro.Instance;
                        break;
                }
            }
        }

        /* Force the app to shutdown when The main window is closed.
         *
         * Explaination:
         * OneClickStatus is initialized as a static object,
         * so the window will exist, even if it is unused.
         * This would cause VRChat Melon Assistant to not shutdown,
         * because technically a window was still open.
         */
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        internal void MarkModsPageForRefresh()
        {
            ModsOpened = false;
            InstallButton.IsEnabled = false;
        }

        public async Task ShowModsPage()
        {
            void OpenModsPage()
            {
                Main.Content = Mods.Instance;
                Properties.Settings.Default.LastTab = "Mods";
                Properties.Settings.Default.Save();
                Mods.Instance.RefreshColumns();
            }

            if (ModsOpened && !Mods.Instance.PendingChanges)
            {
                OpenModsPage();
                return;
            }

            Main.Content = Loading.Instance;

            if (ModsLoading) return;
            ModsLoading = true;
            await Mods.Instance.LoadMods();
            ModsLoading = false;

            if (ModsOpened == false) ModsOpened = true;
            if (Mods.Instance.PendingChanges == true) Mods.Instance.PendingChanges = false;

            if (Main.Content == Loading.Instance)
            {
                OpenModsPage();
            }
        }

        private void ModsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowModsPage().NoAwait();
        }

        private void IntroButton_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = Intro.Instance;
            Properties.Settings.Default.LastTab = "Intro";
            Properties.Settings.Default.Save();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = About.Instance;
            Properties.Settings.Default.LastTab = "About";
            Properties.Settings.Default.Save();
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = Options.Instance;
            Themes.LoadThemes();
            Properties.Settings.Default.LastTab = "Options";
            Properties.Settings.Default.Save();
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            Mods.Instance.InstallMods();
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            if ((Mods.ModListItem)Mods.Instance.ModsListView.SelectedItem == null)
            {
                MessageBox.Show((string)Application.Current.FindResource("MainWindow:NoModSelected"));
                return;
            }
            Mods.ModListItem mod = ((Mods.ModListItem)Mods.Instance.ModsListView.SelectedItem);
            ShowModInfoWindow(mod.ModInfo);
        }

        internal static void ShowModInfoWindow(Mod mod)
        {
            var infoWindow = new ModInfoWindow();
            infoWindow.SetMod(mod);
            infoWindow.Owner = Instance;
            infoWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            infoWindow.ShowDialog();
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (About.Instance.PatUp.IsOpen)
            {
                About.Instance.PatUp.IsOpen = false;
                About.Instance.PatButton.IsEnabled = true;
            }

            if (About.Instance.HugUp.IsOpen)
            {
                About.Instance.HugUp.IsOpen = false;
                About.Instance.HugButton.IsEnabled = true;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Main.Content == Mods.Instance)
            {
                Mods.Instance.RefreshColumns();
            }
        }

        private void BackgroundVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundVideo.Position = TimeSpan.Zero;
            BackgroundVideo.Play();
        }
    }
}
