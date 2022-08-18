using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CVRMelonAssistant.Resources {
    public partial class ContextMenusDictionary {
        static ContextMenusDictionary _instance = null;
        ContextMenu _hyperlinkContextMenu;

        public ContextMenu HyperlinkContextMenu => _hyperlinkContextMenu;

        public static ContextMenusDictionary Instance {
            get {
                if(_instance is null)
                    _instance = new ContextMenusDictionary();
                return _instance;
            }
        }

        public ContextMenusDictionary() {
            InitializeComponent();
            _hyperlinkContextMenu = this["cmHyperlink"] as ContextMenu;
        }
        private void HyperLinkContextMenu_Open(object sender, RoutedEventArgs e) {
            var menuItem = sender as MenuItem;
            var link = menuItem.DataContext as Hyperlink;
            link.DoClick();
        }

        private void HyperLinkContextMenu_Copy(object sender, RoutedEventArgs e) {
            var menuItem = sender as MenuItem;
            var link = menuItem.DataContext as Hyperlink;
            Clipboard.SetText(link.NavigateUri.ToString());
        }
    }
}
