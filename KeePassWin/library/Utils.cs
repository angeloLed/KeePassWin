using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace KeePassWin
{
    class Utils
    {
        public string currentPageTitle = "";


        /*
         * 
         * puuffff method
        public static T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            int childsCount = VisualTreeHelper.GetChildrenCount(dependencyObject);
            for (int i = 0; i < childsCount; i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);
                T childType = child as T;
            }

           
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            if (parent == null) return null;
            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }

        public static void SetTitlepage(DependencyObject dependencyObject, string title)
        {
            var test = FindParent<Frame>(dependencyObject);

        }
        */
        public static void SetTitlepage( string title)
        {
            App.Session.CurrentPageTitle = title;
        }
    }
}
