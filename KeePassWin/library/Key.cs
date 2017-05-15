using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace KeePassWin
{
    [Serializable]
    public class Key
    {
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        private string icon { get; set; }
        [DataMember]
        public string Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                OnPropertyChanged("icon");
            }
        }

        private string title { get; set; }
        [DataMember]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("title");
            }
        }

        private string username { get; set; }
        [DataMember]
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged("username");
            }
        }

        private string password { get; set; }
        [DataMember]
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("password");
            }
        }

        private string url { get; set; }
        [DataMember]
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                OnPropertyChanged("url");
            }
        }

        private string note { get; set; }
        [DataMember]
        public string Note
        {
            get { return note; }
            set
            {
                note = value;
                OnPropertyChanged("note");
            }
        }
        #endregion

        #region Event handlers
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}