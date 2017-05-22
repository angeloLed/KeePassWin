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

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace KeePassWin
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class SearchKeys : Page
    {

        public Key selectedKey;
        public ObservableCollection<Key> keys = new ObservableCollection<Key>();
        public SearchKeys()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!String.IsNullOrEmpty((string)e.Parameter)) {
                this.search((string)e.Parameter);
            }

            Utils.SetTitlepage("Search on Db for '"+ (string)e.Parameter + "'");

            //TODO: call method "checkNoItem"; if call now, listitem lost the databinding :/
            if (this.keys.Count == 0)
            {
                listViewNoItems.Visibility = Visibility.Visible;
            }
        }

        private void search(string searchString)
        {
            searchString = searchString.ToLower();
            keys.Clear();

            foreach (GroupKeys gk in App.CurrentDb.Groups)
            {
                IEnumerable<Key> keysTemp = gk.Keys.Where(x =>
                    x.Note.ToLower().Contains(searchString)
                    || x.Title.ToLower().Contains(searchString)
                    || x.Username.ToLower().Contains(searchString)
                    || x.Url.ToLower().Contains(searchString)
                );

                foreach (Key k in keysTemp) {
                    keys.Add(k);
                }
            }
        }

        private void gridElements_ItemClick(object sender, ItemClickEventArgs e)
        {
            Key key = (Key)e.ClickedItem;
            this.Frame.Navigate(typeof(EditKey), key);
        }

        private void gridElements_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            this.selectedKey = senderElement.DataContext as Key;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void gridElements_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            this.selectedKey = senderElement.DataContext as Key;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void checkNoItem()
        {
            if (gridElements.Items.Count == 0)
            {
                listViewNoItems.Visibility = Visibility.Visible;
                gridElements.Visibility = Visibility.Collapsed;
            }
            else
            {
                listViewNoItems.Visibility = Visibility.Collapsed;
                gridElements.Visibility = Visibility.Visible;
            }
        }

        private void gridElements_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            this.checkNoItem();
        }
    }
}
