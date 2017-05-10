using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class BootPage : Page
    {
        public BootPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //string asd3 = Crypto.Encrypt("abcdef", "password");
            //string asd4 = Crypto.Decrypt(asd3, "password");

            IReadOnlyList<StorageFile> files = await Storage.getFiles();
            if (files.Count() == 0)
            {
                this.Frame.Navigate(typeof(EditDb), null);
            }
            else if (files.Count() == 1)
            {
                //to login db
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["filename"] = files[0].Name;
                this.Frame.Navigate(typeof(LoginDb), parameters);
            }
            else {

                //to choise db
                this.Frame.Navigate(typeof(MainPage), null);
            }
        }
    }
}
