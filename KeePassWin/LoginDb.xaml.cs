using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class LoginDb : Page
    {
        private string bodyFile;
        public LoginDb()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Dictionary<string, string> parameters = (Dictionary<string, string>)e.Parameter;

            labelLogin.Text = "Opening : " + parameters["filename"];
            this.bodyFile = await Storage.getContentFile(parameters["filename"]);
        }

        private async void orangeButton_Click(object sender, RoutedEventArgs e)
        {
            string decryptedBody = "";

            try {
                decryptedBody = Crypto.Decrypt(this.bodyFile, password.Password);
            }
            catch(Exception ex) {
                string messageError = "";
                if (ex.Message.Contains("Exception from HRESULT: 0x80070017")) {
                    messageError = "Wrong Password";
                }
                else {
                    messageError = "Error to login :" + ex.Message;
                }
                var dialog = new MessageDialog(messageError);
                await dialog.ShowAsync();
            }

            if (!String.IsNullOrEmpty(decryptedBody)) {

                Db db = Db.getFromJson(decryptedBody);
                App.CurrentDb = db;
                this.Frame.Navigate(typeof(Home));
            }
        }

        private void homeButtom_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ListDb));
        }
    }
}
