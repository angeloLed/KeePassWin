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

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace KeePassWin
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class EditGroup : Page
    {
        GroupKeys gk;

        public EditGroup()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.gk = (GroupKeys)e.Parameter;

            if (gk != null) {
                name.Text = gk.name;
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (App.currentDb.Groups == null) {
                App.currentDb.Groups = new ObservableCollection<GroupKeys>();
            }

            if (this.gk == null) {
                App.currentDb.Groups.Add(new GroupKeys { name = name.Text, keys = new ObservableCollection<Key>() });
            }
            else {
                gk.name = name.Text;
            }
           
            this.Frame.GoBack();
        }

    }
}
