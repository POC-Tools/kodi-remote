using KodiRemote.Uwp.Core;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp
{
    public sealed partial class PageWelcome : Page
    {
        public PageWelcome()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;

            Loaded += PageWelcome_Loaded;
        }

        private void PageWelcome_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Context.Connections.Any())
            {
                Frame.Navigate(typeof(PageServers));
                return;
            }
        }

        private void AddButton_Click(object o, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageSettings), new KodiConnection());
        }
    }
}
