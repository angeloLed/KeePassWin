using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace KeePassWin
{
    [Serializable]
    public class GroupKeys : INotifyPropertyChanged
    {
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        // keys
        internal ObservableCollection<Key> Keys { get; set; }
       
        //name
        private string name;
        internal string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("name");
            }
        }

        // description
        private string description;
        internal string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("description");
            }
        }

        //note
        private string note { get; set; }
        internal string Note
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

        //contructor
        public GroupKeys()
        {
           
        }


    }
}