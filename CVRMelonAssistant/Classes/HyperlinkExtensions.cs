using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using CVRMelonAssistant.Resources;

namespace CVRMelonAssistant
{
    public static class HyperlinkExtensions
    {
        public static bool GetIsExternal(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsExternalProperty);
        }

        public static void SetIsExternal(DependencyObject obj, bool value)
        {
            obj.SetValue(IsExternalProperty, value);
        }

        public static readonly DependencyProperty IsExternalProperty = DependencyProperty.RegisterAttached("IsExternal", typeof(bool), typeof(HyperlinkExtensions), new UIPropertyMetadata(false, OnIsExternalChanged));

        private static void OnIsExternalChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var hyperlink = sender as Hyperlink;

            if ((bool)args.NewValue)
                hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
            else
                hyperlink.RequestNavigate -= Hyperlink_RequestNavigate;
        }

        public static void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        public static void NoAwait(this Task task)
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Utils.ShowErrorMessageBox("Exception in free-floating task", t.Exception);
                }
            });
        }

        public static Hyperlink Create(string uri)
        {
            var link = new Hyperlink(new Run(uri)) { NavigateUri = new Uri(uri) };
            link.RequestNavigate += Hyperlink_RequestNavigate;
            link.MouseRightButtonUp += Hyperlink_RightClick;
            return link;
        }

        public static void Hyperlink_RightClick(object sender, MouseButtonEventArgs e)
        {
            var cm = ContextMenusDictionary.Instance.HyperlinkContextMenu;
            cm.DataContext = sender;
            cm.IsOpen = true;
        }
    }
}
