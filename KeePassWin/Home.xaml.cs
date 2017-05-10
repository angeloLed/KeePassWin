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
            this.MySplitView.Content = new Frame();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void buttonSpitter_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void buttonGroupes_Click(object sender, RoutedEventArgs e)
        {
            this.navigateTo(typeof(ListGroups));
        }

        private void navigateTo(Type page)
        {
            MySplitView.IsPaneOpen = false;
            if (MySplitView.Content != null) {
                ((Frame)MySplitView.Content).Navigate(page);
            }

            /*var frame = this.Frame;
            Page page = frame?.Content as Page;
            if (page?.GetType() != typeof(EditGroup))
            {
                frame.Navigate(typeof(EditGroup));
            }*/
        }
    }
}
