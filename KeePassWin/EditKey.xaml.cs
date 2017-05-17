using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

namespace KeePassWin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditKey : Page
    {
        private Key key = null;
        private ObservableCollection<Key> keys = null;
        public EditKey()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter.GetType() == typeof(Key)) {
                this.key = (Key)e.Parameter;

                previewIcon.Text = key.Icon ?? "&#xE192;";
                title.Text = key.Title ?? "";
                note.Text = key.Note ?? "";
                password.Password = key.Password ?? "";
                url.Text = key.Url ?? "";
                username.Text = key.Username ?? "";

            } else if (e.Parameter.GetType() == typeof(ObservableCollection<Key>)) {
                this.key = new Key();
                this.keys = (ObservableCollection<Key>)e.Parameter;

            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            key.Title = title.Text;
            key.Note = note.Text;
            key.Password = password.Password;
            key.Url = url.Text;
            key.Username = username.Text;
            key.Icon = previewIcon.Text;

            if (this.keys != null) {
                this.keys.Add(this.key);
            }

            this.Frame.GoBack();
        }

        private async void icon_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogs.IconsGrid dialog = new KeePassWin.ContentDialogs.IconsGrid();
            await dialog.ShowAsync();
            previewIcon.Text = dialog.SelectedEmoji.GetIcon();
        }
    }
}
