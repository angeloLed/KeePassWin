using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace KeePassWin
{
    public class Session
    {
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        private string currentPageTitle { get; set; }
        public string CurrentPageTitle {
            get { return this.currentPageTitle; }
            set {
                currentPageTitle = value;
                OnPropertyChanged("currentPageTitle");
            }
        }

        public bool PengingSave { get; set; }
        #endregion

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public void ChooseTheme(string filename)
        {
            ResourceDictionary rd = new ResourceDictionary
                {
                    Source = new Uri("ms-appx:///Styles/"+ filename + ".xaml", UriKind.Absolute)
                };

            Application.Current.Resources = rd;
        }
    }
}