using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace KeePassWin
{
    [Serializable]
    public class GroupKeys : INotifyPropertyChanged
    {
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        // keys
        private ObservableCollection<Key> keys { get; set; }
        [DataMember]
        public ObservableCollection<Key> Keys
        {
            get
            {
                return keys;
            }
            set
            {
                keys = value;
            }
        }

        //name
        private string name;
        [DataMember]
        public string Name
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
        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("description");
            }
        }

        // icon
        private string icon;
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

        //note
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

        private String createAt { get; set; }
        [DataMember]
        public String CreateAt
        {
            get { return createAt; }
            set
            {
                createAt = value;
                OnPropertyChanged("createDate");
            }
        }

        private String updateAt { get; set; }
        [DataMember]
        public String UpdateAt
        {
            get { return updateAt; }
            set
            {
                updateAt = value;
                OnPropertyChanged("updateAt");
            }
        }

        #endregion

        #region Event handlers
        private void K_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Key item in e.NewItems)
                {
                    item.PropertyChanged += OnPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (Key item in e.OldItems)
                {
                    item.PropertyChanged -= OnPropertyChanged;
                }
            }

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs("key"));
            }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(e.PropertyName));
            }
        }
        #endregion

        //contructor
        public GroupKeys()
        {
            if (this.keys == null)
            {
                this.keys = new ObservableCollection<Key>();
            }

            //init event handler
            this.keys.CollectionChanged += K_CollectionChanged;
        }


    }
}