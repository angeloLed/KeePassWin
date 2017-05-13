using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace KeePassWin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListKeys : Page
    {
        public Key selectedKey;
        public ObservableCollection<Key> keys;
        MessageDialog deletedialog;
        public ListKeys()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.keys = (ObservableCollection<Key>)e.Parameter;

            //inizialize dialog
            deletedialog = new Windows.UI.Popups.MessageDialog(
                "You are sure to remove the selected Key?",
                "You are sure?"
            );
            deletedialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
            deletedialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });
            deletedialog.DefaultCommandIndex = 0;
            deletedialog.CancelCommandIndex = 1;
        }

        private void buttonNewKey_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditKey), keys);
        }

        private void EditKey_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditKey), this.selectedKey);
        }
        private async void DeleteKey_Click(object sender, RoutedEventArgs e)
        {
            var result = await this.deletedialog.ShowAsync();
            if (result.Id.ToString() == "0")
            {
                this.keys.Remove(this.selectedKey);
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
    }
}
