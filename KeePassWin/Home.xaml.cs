using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
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
    public sealed partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();
            this.splitView.Content = new Frame();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.Session = new Session();

            App.Session.PropertyChanged += (sender, ev) =>
            {
                HeadTitlePage.Text = App.Session.CurrentPageTitle;
            };

            App.CurrentDb.PropertyChanged += (sender, ev) =>
            {
                App.Session.PengingSave = true;
                DbSaveIcon.Foreground = new SolidColorBrush(Colors.Red);
                DbSaveText.Foreground = new SolidColorBrush(Colors.Red);
            };

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            if (App.IsMobile)
            {
                splitView.DisplayMode = SplitViewDisplayMode.Inline;
            }


            this.navigateTo(typeof(ListGroups));
        }

        private void buttonSpitter_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void buttonGroupes_Click(object sender, RoutedEventArgs e)
        {
            this.navigateTo(typeof(ListGroups));
        }

        private void navigateTo(Type page)
        {
            this.navigateTo(page, null);
        }

        private void navigateTo(Type page, string parameter)
        {
            splitView.IsPaneOpen = false;
            if (splitView.Content != null)
            {
                ((Frame)splitView.Content).Navigate(page, parameter);
            }

            /*var frame = this.Frame;
            Page page = frame?.Content as Page;
            if (page?.GetType() != typeof(EditGroup))
            {
                frame.Navigate(typeof(EditGroup));
            }*/
        }

        private void buttonDbSave_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentDb.save();

            App.Session.PengingSave = false;
            splitView.IsPaneOpen = false;


            DbSaveIcon.Foreground = new SolidColorBrush(Colors.Black);
            DbSaveText.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void buttonDbEdit_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditDb));
        }

        private async void buttomDbList_Click(object sender, RoutedEventArgs e)
        {
            bool exit = true;

            if (App.Session.PengingSave) {
                ContentDialogs.PendingSaveDialog dialog = new KeePassWin.ContentDialogs.PendingSaveDialog();
                await dialog.ShowAsync();

                exit = dialog.skipSave;

            }

            if (exit)
            {
                this.Frame.Navigate(typeof(ListDb));
            }

        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(textSearch.Text))
            {
                splitView.IsPaneOpen = true;
                textSearch.Focus(FocusState.Keyboard);
            }
            else {

                this.navigateTo(typeof(SearchKeys), textSearch.Text);
            }
        }

        private async void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame splitviewFrame = ((Frame)splitView.Content);
            e.Handled = true;

            if (splitviewFrame.CanGoBack)
            {
                splitviewFrame.GoBack();
            }
            else if (App.Session.PengingSave)
            {
                ContentDialogs.PendingSaveDialog dialog = new KeePassWin.ContentDialogs.PendingSaveDialog();
                await dialog.ShowAsync();

                if (dialog.skipSave) {
                    Application.Current.Exit();
                }

            }
            else {
                Application.Current.Exit();
            }
        }

        private void textSearch_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                this.buttonSearch_Click(sender, e);
            }
        }
    }
}
