﻿using System;
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

                title.Text = key.title;

            } else if (e.Parameter.GetType() == typeof(ObservableCollection<Key>)) {
                this.keys = (ObservableCollection<Key>)e.Parameter;
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (key != null)
            {
                key.title = title.Text;
            }
            else {
                this.key = new Key();
                this.key.title = title.Text;
                this.keys.Add(this.key);
            }

            this.Frame.GoBack();
        }
    }
}
