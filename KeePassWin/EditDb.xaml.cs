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

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace KeePassWin
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class EditDb : Page
    {
        public EditDb()
        {
            this.InitializeComponent();
        }

        private void orangeButton_Click(object sender, RoutedEventArgs e)
        {
            Db db = new Db();

            db.Title = title.Text;
            db.Password = password.Password;
            db.Groups = new System.Collections.ObjectModel.ObservableCollection<GroupKeys>();
            db.save();

            this.Frame.Navigate(typeof(Home), db);
        }
    }
}
