using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using KodiRemote.Core;
using KodiRemote.Uwp.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp
{
    public sealed partial class PageSettings : Page
    {
        class SettingsViewModel : INotifyPropertyChanged
        {
            #region -_ Properties _-

            public string ConnectionName
            {
                get { return KodiConnection.Name; }
                set
                {
                    if (KodiConnection.Name == value) return;
                    KodiConnection.Name = value; NotifyPropertyChanged();
                }
            }
            public string GetName()
            {
                return KodiConnection.Name;
            }
            public string Address
            {
                get { return KodiConnection.Kodi.Address; }
                set
                {
                    if (KodiConnection.Kodi.Address == value) return;
                    KodiConnection.Kodi.Address = value;
                    NotifyPropertyChanged();
                }
            }

            public string Port
            {
                get { return KodiConnection.Kodi.Port; }
                set
                {
                    if (KodiConnection.Kodi.Port == value) return;
                    KodiConnection.Kodi.Port = value; NotifyPropertyChanged();
                }
            }

            public string Login
            {
                get { return KodiConnection.Kodi.Login; }
                set
                {
                    if (KodiConnection.Kodi.Login == value) return;
                    KodiConnection.Kodi.Login = value; NotifyPropertyChanged();
                }
            }

            public string Password
            {
                get { return KodiConnection.Kodi.Password; }
                set
                {
                    if (KodiConnection.Kodi.Password == value) return;
                    KodiConnection.Kodi.Password = value; NotifyPropertyChanged();
                }
            }

            public string MacAddress
            {
                get { return KodiConnection.Kodi.MacAddress; }
                set
                {
                    if (KodiConnection.Kodi.MacAddress == value) return;
                    KodiConnection.Kodi.MacAddress = value; NotifyPropertyChanged();
                }
            }
            bool isLoading;
            public bool IsLoading
            {
                get { return isLoading; }
                set
                {
                    if (isLoading == value) return;
                    isLoading = value; NotifyPropertyChanged();
                }
            }

            public KodiConnection KodiConnection { get; }
            #endregion

            public SettingsViewModel(KodiConnection connection)
            {
                KodiConnection = connection;
            }

            public event PropertyChangedEventHandler PropertyChanged;
            void NotifyPropertyChanged([CallerMemberName] string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


        SettingsViewModel vm;

        public PageSettings()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            KodiConnection newConnection = e.Parameter as KodiConnection;
            var _connection = App.Context.Connections.FirstOrDefault(c => c.Id == newConnection?.Id) ?? new KodiConnection { Kodi = Connection.Default() };
            DataContext = vm = new SettingsViewModel(_connection);
        }

        private async void TestButtonClick(object sender, RoutedEventArgs e)
        {
            vm.IsLoading = true;

            try
            {
                var result = await vm.KodiConnection.TestConnectionAsync();
                Bindings.Update();

                var resourceLoader = ResourceLoader.GetForCurrentView();
                string message = result ? resourceLoader.GetString("/settings/TestGood") : resourceLoader.GetString("/settings/TestBad");
                var dialog = new MessageDialog(message, resourceLoader.GetString("/settings/ConnectivityTest.Content"));
                await dialog.ShowAsync();

            }
            finally
            {
                vm.IsLoading = false;
            }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {

            if (!App.Context.Connections.Any(c=>c.Id == vm.KodiConnection.Id))
                App.Context.Connections.Add(vm.KodiConnection);

            App.Context.Save();

            if (Frame.CanGoBack)
                Frame.GoBack();
        }


    }
}
