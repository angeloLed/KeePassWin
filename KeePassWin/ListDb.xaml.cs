using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
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
    public sealed partial class ListDb : Page
    {
        public List<string> filesDb = new List<string>();
        public ListDb()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            App.CurrentDb = null;
            IReadOnlyList<StorageFile> files = await Storage.getFiles();
            this.filesDb = files.Select(file => file.Name).ToList();

            foreach (string file in this.filesDb) {
                listDbGrid.Items.Add(file);
            }
            listDbGrid.Items.Add("+");


            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        private void listDbGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridView gw = (GridView)sender;
            if (gw.SelectedIndex != gw.Items.Count - 1)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["filename"] = (string)((GridView)sender).SelectedItem;
                this.Frame.Navigate(typeof(LoginDb), parameters);
            }
            else {
                this.Frame.Navigate(typeof(EditDb), null);
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            Application.Current.Exit();
        }
    }
}
