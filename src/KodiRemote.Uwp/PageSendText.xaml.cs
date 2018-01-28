using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KodiRemote.Uwp
{
    public sealed partial class PageSendText : Page
    {
        public PageSendText()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = this;
            Loaded += PageSendText_Loaded;
        }

        #region TextToSend

        public string TextToSend
        {
            get { return (string)GetValue(TextToSendProperty); }
            set { SetValue(TextToSendProperty, value); }
        }

        public static readonly DependencyProperty TextToSendProperty =
            DependencyProperty.Register(nameof(TextToSend), typeof(string), typeof(PageSendText), new PropertyMetadata(null));

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        
        }

        private void PageSendText_Loaded(object sender, RoutedEventArgs e)
        {
            TxtTextToSend.Focus(FocusState.Keyboard);
        }

        public async void ButtonSendClick(object sender, RoutedEventArgs e)
        {
            if ( string.IsNullOrWhiteSpace(TextToSend)) return;

            await App.Context.Connection.Kodi.Input.SendTextAsync(TextToSend);
        }
    }
}
