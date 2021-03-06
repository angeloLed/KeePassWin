﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;
using Windows.Data.Json;

namespace KeePassWin
{
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
            this.init();
        }

        public GroupKeys(JsonObject joG)
        {
            this.init();

            this.Description = joG.GetNamedString("Description", "");
            this.Icon = joG.GetNamedString("Icon", "");
            this.Name = joG.GetNamedString("Name", "");
            this.Note = joG.GetNamedString("Note", "");
            this.CreateAt = joG.GetNamedString("CreateAt", "");
            this.UpdateAt = joG.GetNamedString("UpdateAt", "");

            foreach (IJsonValue jsonValueK in joG.GetNamedArray("Keys", new JsonArray()))
            {
                if (jsonValueK.ValueType == JsonValueType.Object)
                {
                    JsonObject joK = jsonValueK.GetObject();
                    this.Keys.Add(new Key(joK));
                }
            }
        }

        private void init()
        {
            if (this.keys == null)
            {
                this.keys = new ObservableCollection<Key>();
            }

            //init event handler
            this.keys.CollectionChanged += K_CollectionChanged;
        }

        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject["Description"] = JsonValue.CreateStringValue(this.Description);
            jsonObject["Icon"] = JsonValue.CreateStringValue(this.Icon);
            jsonObject["Name"] = JsonValue.CreateStringValue(this.Name);
            jsonObject["Note"] = JsonValue.CreateStringValue(this.Note);
            jsonObject["CreateAt"] = JsonValue.CreateStringValue(this.CreateAt);
            jsonObject["UpdateAt"] = JsonValue.CreateStringValue(this.UpdateAt);
            JsonArray jsonKeys = new JsonArray();
            foreach (Key kTemp in this.Keys)
            {
                jsonKeys.Add(kTemp.ToJsonObject());
            }
            jsonObject["Keys"] = jsonKeys;

            return jsonObject;
        }
    }
}