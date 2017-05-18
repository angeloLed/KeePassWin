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
    public sealed partial class ListGroups : Page
    {
        public Db db;
        public GroupKeys selectedGroup;
        MessageDialog deletedialog;
        public ListGroups()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.db = App.currentDb;

            //inizialize dialog
            deletedialog = new Windows.UI.Popups.MessageDialog(
                "You are sure to remove the selected group?",
                "You are sure?"
            );

            deletedialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
            deletedialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });

            /*if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
            {
                deletedialog.Commands.Add(new Windows.UI.Popups.UICommand("Maybe later") { Id = 2 });
            }*/

            deletedialog.DefaultCommandIndex = 0;
            deletedialog.CancelCommandIndex = 1;

            Utils.SetTitlepage("Groups");

            //TODO: call method "checkNoItem"; if call now, listitem lost the databinding :/
            if (App.currentDb.Groups.Count == 0) {
                listViewNoItems.Visibility = Visibility.Visible;
            }
        }

        private void buttonNewGroup_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditGroup));
        }
        private void EditGroup_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditGroup), this.selectedGroup);
        }
        private async void DeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            var result = await this.deletedialog.ShowAsync();
            if (result.Id.ToString() == "0") {
                this.db.Groups.Remove(this.selectedGroup);
            }
        }

        private void gridElements_ItemClick(object sender, ItemClickEventArgs e)
        {
            GroupKeys gk = (GroupKeys)e.ClickedItem;
            this.Frame.Navigate(typeof(ListKeys), gk.Keys);

        }

        private void gridElements_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            this.selectedGroup = senderElement.DataContext as GroupKeys;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void gridElements_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            this.selectedGroup = senderElement.DataContext as GroupKeys;
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
