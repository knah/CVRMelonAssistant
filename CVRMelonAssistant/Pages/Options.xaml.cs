using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;

namespace CVRMelonAssistant.Pages
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Page
    {
        public static Options Instance = new Options();

        public string InstallDirectory { get; set; }
        public string InstallType { get; set; }
        public bool CloseWindowOnFinish { get; set; }
        public string LogURL { get; private set; }

        public Options()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void SelectDirButton_Click(object sender, RoutedEventArgs e)
        {
            Utils.GetManualDir();
            DirectoryTextBlock.Text = InstallDirectory;
            GameTypeTextBlock.Text = InstallType;
        }

        private void OpenDirButton_Click(object sender, RoutedEventArgs e)
        {
            Utils.OpenFolder(InstallDirectory);
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Utils.GetSteamDir());
        }

        private void CloseWindowOnFinish_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.CloseWindowOnFinish = true;
            App.CloseWindowOnFinish = true;
            CloseWindowOnFinish = true;
            Properties.Settings.Default.Save();
        }

        private void CloseWindowOnFinish_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.CloseWindowOnFinish = false;
            App.CloseWindowOnFinish = false;
            CloseWindowOnFinish = false;
            Properties.Settings.Default.Save();
        }

        private void OpenAppDataButton_Click(object sender, RoutedEventArgs e)
        {
            string location = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "AppData", "LocalLow", "Alpha Blend Interactive", "ChilloutVR");
            if (Directory.Exists(location))
            {
                Utils.OpenFolder(location);
            }
            else
            {
                MessageBox.Show((string)Application.Current.FindResource("Options:AppDataNotFound"));
            }
        }

        private async void YeetModsButton_Click(object sender, RoutedEventArgs e)
        {
            string title = (string)Application.Current.FindResource("Options:YeetModsBox:Title");
            string line1 = (string)Application.Current.FindResource("Options:YeetModsBox:RemoveAllMods");
            string line2 = (string)Application.Current.FindResource("Options:YeetModsBox:CannotBeUndone");

            var resp = System.Windows.Forms.MessageBox.Show($"{line1}\n{line2}", title, System.Windows.Forms.MessageBoxButtons.YesNo);
            if (resp == System.Windows.Forms.DialogResult.Yes)
            {
                var modsDir = Path.Combine(App.ChilloutInstallDirectory, "Mods");
                if (Directory.Exists(modsDir))
                    Directory.Delete(modsDir, true);
                var pluginsDir = Path.Combine(App.ChilloutInstallDirectory, "Plugins");
                if (Directory.Exists(pluginsDir))
                    Directory.Delete(pluginsDir, true);

                Directory.CreateDirectory(modsDir);
                Directory.CreateDirectory(pluginsDir);

                MainWindow.Instance.MainText = $"{Application.Current.FindResource("Options:AllModsUninstalled")}...";
            }
        }

        private async void YeetMelonLoaderButton_Click(object sender, RoutedEventArgs e)
        {
            string title = (string)Application.Current.FindResource("Options:YeetMLBox:Title");
            string line1 = (string)Application.Current.FindResource("Options:YeetMLBox:RemoveAllMods");
            string line2 = (string)Application.Current.FindResource("Options:YeetMLBox:CannotBeUndone");

            var resp = System.Windows.Forms.MessageBox.Show($"{line1}\n{line2}", title, System.Windows.Forms.MessageBoxButtons.YesNo);
            if (resp == System.Windows.Forms.DialogResult.Yes)
            {
                InstallHandlers.RemoveMelonLoader();

                MainWindow.Instance.MainText = $"{Application.Current.FindResource("Options:MLUninstalled")}...";
            }
        }

        private async void InstallMelonLoaderButton_Click(object sender, RoutedEventArgs e)
        {
            await InstallHandlers.InstallMelonLoader();

            MainWindow.Instance.MainText = $"{Application.Current.FindResource("Options:MLInstalled")}...";
        }

        private void ApplicationThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem == null)
            {
                Themes.ApplyWindowsTheme();
                MainWindow.Instance.MainText = (string)Application.Current.FindResource("Options:CurrentThemeRemoved");
            }
            else
            {
                Themes.ApplyTheme((sender as ComboBox).SelectedItem.ToString());
            }
        }

        public void LanguageSelectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem == null)
            {
                // Apply default language
                Console.WriteLine("Applying default language");
                Languages.LoadLanguage("en");
            }
            else
            {
                // Get the matching language from the LoadedLanguages array, then try and use it
                var languageName = (sender as ComboBox).SelectedItem.ToString();
                var selectedLanguage = Languages.LoadedLanguages.Find(language => language.NativeName.CompareTo(languageName) == 0);
                if (Languages.LoadLanguage(selectedLanguage.Name))
                {
                    Properties.Settings.Default.LanguageCode = selectedLanguage.Name;
                    Properties.Settings.Default.Save();
                    if (Languages.FirstRun)
                    {
                        Languages.FirstRun = false;
                    }
                    else
                    {
                        Process.Start(Utils.ExePath, App.Arguments);
                        Application.Current.Dispatcher.Invoke(() => { Application.Current.Shutdown(); });
                    }
                }
            }
        }

        private void ApplicationThemeExportTemplate_Click(object sender, RoutedEventArgs e)
        {
            Themes.WriteThemeToDisk("Ugly Kulu-Ya-Ku");
            Themes.LoadThemes();
        }

        private void ApplicationThemeOpenThemesFolder_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Themes.ThemeDirectory))
            {
                Utils.OpenFolder(Themes.ThemeDirectory);
            }
            else
            {
                MessageBox.Show((string)Application.Current.FindResource("Options:ThemeFolderNotFound"));
            }
        }
    }
}
