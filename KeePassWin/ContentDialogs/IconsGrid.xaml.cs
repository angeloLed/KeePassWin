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
        public Emoji SelectedEmoji;
        private List<Emoji> iconSet = new List<Emoji>();
        public IconsGrid()
        {
            this.InitializeComponent();

            // problem with saltuary code of charset
            /*int i = 57345;
            for (int x = i; x < i+100; x++) {
                iconSet.Add(new Emoji(x));
            }*/

            iconSet.Add(new Emoji(57345));
            iconSet.Add(new Emoji(57350));
            iconSet.Add(new Emoji(57351));
            iconSet.Add(new Emoji(57354));
            iconSet.Add(new Emoji(57492));
            iconSet.Add(new Emoji(57604));
            iconSet.Add(new Emoji(57605));
            iconSet.Add(new Emoji(57607));
            iconSet.Add(new Emoji(57615));
            iconSet.Add(new Emoji(57620));
            iconSet.Add(new Emoji(57621));
            iconSet.Add(new Emoji(57622));
            iconSet.Add(new Emoji(57623));
            iconSet.Add(new Emoji(57625));
            iconSet.Add(new Emoji(57629));
            iconSet.Add(new Emoji(57637));
            iconSet.Add(new Emoji(57640));
            iconSet.Add(new Emoji(57641));
            iconSet.Add(new Emoji(57642));
            iconSet.Add(new Emoji(57649));
            iconSet.Add(new Emoji(57650));
            iconSet.Add(new Emoji(57652));
            iconSet.Add(new Emoji(57654));
            iconSet.Add(new Emoji(57658));
            iconSet.Add(new Emoji(57661));
            iconSet.Add(new Emoji(57677));
            iconSet.Add(new Emoji(57694));
            iconSet.Add(new Emoji(57699));
            iconSet.Add(new Emoji(57702));
            iconSet.Add(new Emoji(57703));
            iconSet.Add(new Emoji(57704));
            iconSet.Add(new Emoji(57707));
            iconSet.Add(new Emoji(57708));
            iconSet.Add(new Emoji(57710));
            iconSet.Add(new Emoji(57712));
            iconSet.Add(new Emoji(57713));
            iconSet.Add(new Emoji(57723));
            iconSet.Add(new Emoji(57737));
            iconSet.Add(new Emoji(57746));
            iconSet.Add(new Emoji(57747));
            iconSet.Add(new Emoji(57759));
            iconSet.Add(new Emoji(57760));
            iconSet.Add(new Emoji(57795));
            iconSet.Add(new Emoji(57796));
            iconSet.Add(new Emoji(57803));
            iconSet.Add(new Emoji(57808));
            iconSet.Add(new Emoji(57809));
            iconSet.Add(new Emoji(57810));
            iconSet.Add(new Emoji(57811));
            iconSet.Add(new Emoji(57812));
            iconSet.Add(new Emoji(57813));
            iconSet.Add(new Emoji(57814));
            iconSet.Add(new Emoji(57815));
            iconSet.Add(new Emoji(57822));
            iconSet.Add(new Emoji(57846));
            iconSet.Add(new Emoji(57847));
            iconSet.Add(new Emoji(57873));
            iconSet.Add(new Emoji(57874));
            iconSet.Add(new Emoji(57946));
            iconSet.Add(new Emoji(58004));
            iconSet.Add(new Emoji(58029));
            iconSet.Add(new Emoji(58033));
            iconSet.Add(new Emoji(58102));
            iconSet.Add(new Emoji(59138));
            iconSet.Add(new Emoji(59140));
            iconSet.Add(new Emoji(59145));
            iconSet.Add(new Emoji(59185));
            iconSet.Add(new Emoji(59219));
            iconSet.Add(new Emoji(59222));
            iconSet.Add(new Emoji(59250));
            iconSet.Add(new Emoji(59252));
            iconSet.Add(new Emoji(59265));
            iconSet.Add(new Emoji(59328));
            iconSet.Add(new Emoji(59363));
            iconSet.Add(new Emoji(59372));
            iconSet.Add(new Emoji(59377));
            iconSet.Add(new Emoji(59388));
            iconSet.Add(new Emoji(59398));
            iconSet.Add(new Emoji(59422));
            iconSet.Add(new Emoji(59425));
            iconSet.Add(new Emoji(59448));
            iconSet.Add(new Emoji(59451));
            iconSet.Add(new Emoji(59453));
            iconSet.Add(new Emoji(59534));
            iconSet.Add(new Emoji(59568));
            iconSet.Add(new Emoji(59717));
            iconSet.Add(new Emoji(59728));
            iconSet.Add(new Emoji(59731));
            iconSet.Add(new Emoji(59739));
            iconSet.Add(new Emoji(59740));
            iconSet.Add(new Emoji(59754));
            iconSet.Add(new Emoji(59802));
            iconSet.Add(new Emoji(59858));
            iconSet.Add(new Emoji(59897));
            iconSet.Add(new Emoji(60025));


            iconSet.Add(new Emoji(60454));

            this.GridViewIcons.ItemsSource = iconSet;
        }
        

        private void MyGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gridView = sender as GridView;
            this.SelectedEmoji = iconSet[gridView.SelectedIndex];
            
            this.Hide();
        }
    }
}
