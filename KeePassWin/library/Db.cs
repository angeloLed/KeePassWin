using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Windows.Data.Json;

namespace KeePassWin
{
    [Serializable]
    public class Db
    {
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string Icon { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Note { get; set; }
        
        private ObservableCollection<GroupKeys> groups { get; set; }
        [DataMember]
        public ObservableCollection<GroupKeys> Groups
        {
            get
            {
                return groups;
            }
            set
            {
                groups = value;
            }
        }

        #endregion

        // constructor
        public Db()
        {
            if (this.Groups == null) {
                this.Groups = new ObservableCollection<GroupKeys>();
            }
            
            //init event handler
            this.Groups.CollectionChanged += GK_CollectionChanged;
        }

        #region event handlers
        private void GK_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null) {
                foreach (GroupKeys item in e.NewItems) {
                    item.PropertyChanged += OnPropertyChanged;
                }
            }

            if (e.OldItems != null) {
                foreach (GroupKeys item in e.OldItems) {
                    item.PropertyChanged -= OnPropertyChanged;
                }
            }

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs("Groups"));
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

        public void save()
        {
            //TODO: hash file name
            if (String.IsNullOrEmpty(this.FileName)) {

                this.FileName = Path.GetInvalidFileNameChars().Aggregate(this.Title, (current, c) => current.Replace(c.ToString(), string.Empty));
            }

            JsonObject jsonObject = new JsonObject();
            jsonObject["FileName"] = JsonValue.CreateStringValue(this.FileName);
            jsonObject["Password"] = JsonValue.CreateStringValue(this.Password);
            jsonObject["Title"] = JsonValue.CreateStringValue(this.Title);
            string we = jsonObject.Stringify();

            string body = JsonConvert.SerializeObject(this);
            Storage.saveFile(this.FileName, Crypto.Encrypt(body, this.Password));
        }

        public static Db getFromJson(string json)
        {
            JsonObject jsonObject = JsonObject.Parse(json);
            Db loadedDb = new Db();
            loadedDb.FileName = jsonObject.GetNamedString("FileName", "");
            //loadedDb.Icon = jsonObject.GetNamedString("Icon", "");
            loadedDb.Password = jsonObject.GetNamedString("Password", "");
            //loadedDb.Note = jsonObject.GetNamedString("Note", "");
            loadedDb.Title = jsonObject.GetNamedString("Title", "");
            foreach (IJsonValue jsonValue in jsonObject.GetNamedArray("Groups", new JsonArray()))
            {
                if (jsonValue.ValueType == JsonValueType.Object)
                {
                    JsonObject joG = jsonValue.GetObject();

                    GroupKeys gk = new GroupKeys();
                    gk.Description = joG.GetNamedString("Description", "");
                    gk.Icon = joG.GetNamedString("Icon", "");
                    gk.Name = joG.GetNamedString("Name", "");
                    gk.Note = joG.GetNamedString("Note", "");
                    gk.CreateAt = joG.GetNamedString("CreateAt", "");
                    gk.UpdateAt = joG.GetNamedString("UpdateAt", "");

                    foreach (IJsonValue jsonValueK in joG.GetNamedArray("Keys", new JsonArray()))
                    {
                        if (jsonValueK.ValueType == JsonValueType.Object)
                        {
                            JsonObject joK = jsonValueK.GetObject();

                            Key key = new Key();
                            key.CreateAt = joK.GetNamedString("CreateAt", "");
                            key.Icon = joK.GetNamedString("Icon", "");
                            key.Note = joK.GetNamedString("Note", "");
                            key.Password = joK.GetNamedString("Password", "");
                            key.Title = joK.GetNamedString("Title", "");
                            key.UpdateAt = joK.GetNamedString("UpdateAt", "");
                            key.Url = joK.GetNamedString("Url", "");
                            key.Username = joK.GetNamedString("Username", "");

                            gk.Keys.Add(key);
                        }
                    }

                    loadedDb.Groups.Add(gk);
                }
            }
            return loadedDb;

            //OLD (with json.net)
            //return JsonConvert.DeserializeObject<Db>(json);
        }
    }
}