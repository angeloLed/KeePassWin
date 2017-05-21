using System;
using System.Collections.Generic;
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

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace KeePassWin
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class EditDb : Page
    {
        private Db db = new Db();
        public EditDb()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (App.currentDb != null) {
                db = App.currentDb;
                title.Text = db.Title;
                password.Password = db.Password;
                deleteButton.Visibility = Visibility.Visible;
            }
        }

        private async void orangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!checkPassword())
            {
                ContentDialogs.WrongPassword dialog = new KeePassWin.ContentDialogs.WrongPassword();
                await dialog.ShowAsync();

                return;
            }

            db.Title = title.Text;
            db.Password = password.Password;
            if (db.Groups == null) {
                db.Groups = new System.Collections.ObjectModel.ObservableCollection<GroupKeys>();
            }
            
            db.save();

            App.currentDb = db;
            this.Frame.Navigate(typeof(Home), db);
        }

        private void showPassword_Click(object sender, RoutedEventArgs e)
        {
            password.PasswordRevealMode = (password.PasswordRevealMode == PasswordRevealMode.Visible) ? PasswordRevealMode.Hidden : PasswordRevealMode.Visible;
        }

        private bool checkPassword()
        {
            bool ok = true;

            if (password.Password != db.Password)
            {
                if(passwordRep.Password != password.Password)
                {
                    ok = false;
                }
            }

            if (String.IsNullOrEmpty(password.Password)) {
                ok = false;
            }

            return ok;
        }

        private async void delteButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Popups.MessageDialog deletedialog = new Windows.UI.Popups.MessageDialog(
                "You are sure to remove entire Db? The process is irreversible!",
                "You are sure?"
            );
            deletedialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
            deletedialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });
            deletedialog.DefaultCommandIndex = 0;
            deletedialog.CancelCommandIndex = 1;

            var result = await deletedialog.ShowAsync();
            if (result.Id.ToString() == "0")
            {
                await Storage.deleteFile(App.currentDb.FileName);
                this.Frame.Navigate(typeof(ListDb), db);
            }
        }
    }
}
