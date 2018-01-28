using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace KodiRemote.Uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationPage : Page, INotifyPropertyChanged
    {
        private string selectedMenu;

        public string SelectedMenu
        {
            get { return selectedMenu; }
            set
            {
                if (selectedMenu == value) return;
                selectedMenu = value;
                NotifyChange();
            }
        }

        public bool IsDesktop
        {
            get
            {
                return !Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyChange([CallerMemberName] string propName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        public NavigationPage()
        {
            this.InitializeComponent();
            string appName = Windows.ApplicationModel.Package.Current.DisplayName;
            appTitle.Text = appName;
            this.SizeChanged += NavigationPage_SizeChanged;
        }

        private void NavigationPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth < 501)
            {
                menuSplitView.DisplayMode = SplitViewDisplayMode.Overlay;
                menuSplitView.IsPaneOpen = false;
            }
            else if (this.ActualWidth < 1000)
            {
                menuSplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
                menuSplitView.IsPaneOpen = false;
            }
            else
            {
                menuSplitView.DisplayMode = SplitViewDisplayMode.Inline;
                menuSplitView.IsPaneOpen = true;
            }
        }

        private void hamburgerMenu_Click(object sender, RoutedEventArgs e)
        {
            menuSplitView.IsPaneOpen = !menuSplitView.IsPaneOpen;
        }

        private void menuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            var selectedItem = listView.SelectedItem as ListViewItem;
            if (selectedItem != null)
            {
                SelectedMenu = selectedItem?.Tag.ToString();
                if (listView.Name == "menuList")
                {
                    foreach (ListViewItem item in footerListView.Items)
                    {
                        item.IsSelected = false;
                    }
                }
                else
                {
                    foreach (ListViewItem item in menuList.Items)
                    {
                        item.IsSelected = false;
                    }
                }
            }
            if (this.ActualWidth < 1000 && menuSplitView.IsPaneOpen) menuSplitView.IsPaneOpen = false;
        }


    }
}
