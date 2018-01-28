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
using Windows.UI.ViewManagement;
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

            string name;
            public string ConnectionName
            {
                get
                {
                    return name;
                    //if (string.IsNullOrEmpty(KodiConnection.Name))
                    //{
                    //    var resourceLoader = ResourceLoader.GetForCurrentView();
                    //    string message = resourceLoader.GetString("/settings/ConnectionName.Text");
                    //    return message;
                    //}

                    //return KodiConnection.Name;
                }
                set
                {

                    if (name == value) return;
                    name = value;
                    NotifyPropertyChanged();
                }

            }

            string address;
            public string Address
            {
                get { return address; }
                set
                {
                    if (address == value) return;
                    address = value;
                    NotifyPropertyChanged();
                }
            }

            string port;
            public string Port
            {
                get { return port; }
                set
                {
                    if (port == value) return;
                    port = value; NotifyPropertyChanged();
                }
            }

            string login;
            public string Login
            {
                get { return login; }
                set
                {
                    if (login == value) return;
                    login = value; NotifyPropertyChanged();
                }
            }

            string password;
            public string Password
            {
                get { return password; }
                set
                {
                    if (password == value) return;
                    password = value;
                    NotifyPropertyChanged();
                }
            }

            string macAddress;
            public string MacAddress
            {
                get { return macAddress; }
                set
                {
                    if (macAddress == value) return;
                    macAddress = value;
                    NotifyPropertyChanged();
                }
            }

            bool isLoading;
            public bool IsLoading
            {
                get { return isLoading; }
                set
                {
                    if (isLoading == value) return;
                    isLoading = value;
                    NotifyPropertyChanged();
                }
            }

            bool valid;
            public bool Valid
            {
                get { return valid; }
                set
                {
                    if (valid == value) return;
                    valid = value;
                    NotifyPropertyChanged();
                }
            }

            private bool isDefault;

            public bool IsDefault
            {
                get { return isDefault; }
                set
                {
                    if (isDefault == value) return;
                    isDefault = value;
                    foreach (var conn in App.Context.Connections)
                    {
                        conn.IsDefault = false;
                    }
                    NotifyPropertyChanged();
                }
            }



            public KodiConnection KodiConnection
            {
                get
                {
                    return new KodiConnection(Login, Password, Address, Port, MacAddress) { IsDefault = this.IsDefault };
                }
            }

            #endregion

            public SettingsViewModel(KodiConnection connection)
            {
                var resourceLoader = ResourceLoader.GetForCurrentView();
                string name = resourceLoader.GetString("/settings/ConnectionName.Text");

                Address = connection.Kodi.Address;
                Port = connection.Kodi.Port;
                Login = connection.Kodi.Login;
                Password = connection.Kodi.Password;
                MacAddress = connection.Kodi.MacAddress;
                IsDefault = connection.IsDefault;
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


        Task loadingTask = Task.CompletedTask;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string address = e.Parameter as string;
            var _connection = App.Context.Connections.FirstOrDefault(c => c.Kodi.Address == address) ?? new KodiConnection { Kodi = Connection.Default() };
            DataContext = vm = new SettingsViewModel(_connection);

            await loadingTask.ContinueWith(async t => { vm.Valid = await vm.KodiConnection.TestConnectionAsync(); }).ContinueWith(async t => { vm.ConnectionName = await vm.KodiConnection.GetName(); });


        }

        private async void TestButtonClick(object sender, RoutedEventArgs e)
        {
            vm.IsLoading = true;
            try
            {
                vm.Valid = await vm.KodiConnection.TestConnectionAsync();
                vm.ConnectionName = await vm.KodiConnection.GetName();
            }
            finally
            {
                vm.IsLoading = false;
                Bindings.Update();
                scrollView.ChangeView(null, 0, null, false);
            }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            var conn = App.Context.Connections.FirstOrDefault(c => c.Kodi.Address == vm.Address);
            if (conn == null)
                App.Context.Connections.Add(vm.KodiConnection);
            else
                conn = vm.KodiConnection;

            App.Context.Save();

            if (Frame.CanGoBack)
                Frame.GoBack();
        }


    }
}
