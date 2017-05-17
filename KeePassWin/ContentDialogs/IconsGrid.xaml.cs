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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace KeePassWin.ContentDialogs
{
    public sealed partial class IconsGrid : ContentDialog
    {
        public string Icon;
        private List<Emoji> iconSet = new List<Emoji>();
        public IconsGrid()
        {
            this.InitializeComponent();
            

            int i = 57345;
            for (int x = i; x < i+100; x++) {
                iconSet.Add(new Emoji{ Text = Char.ConvertFromUtf32(x)});
            }

            this.GridViewIcons.ItemsSource = iconSet;
        }
        

        private void MyGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gridView = sender as GridView;
            string asd = iconSet[gridView.SelectedIndex].Text;
            
            this.Hide();
        }
    }
}
