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

        // constructors
        public Db()
        {
            this.init();
        }

        public Db(string json)
        {
            this.init();

            JsonObject jsonObject = JsonObject.Parse(json);
            this.FileName = jsonObject.GetNamedString("FileName", "");
            //loadedDb.Icon = jsonObject.GetNamedString("Icon", "");
            this.Password = jsonObject.GetNamedString("Password", "");
            //loadedDb.Note = jsonObject.GetNamedString("Note", "");
            this.Title = jsonObject.GetNamedString("Title", "");
            foreach (IJsonValue jsonValue in jsonObject.GetNamedArray("Groups", new JsonArray()))
            {
                if (jsonValue.ValueType == JsonValueType.Object)
                {
                    JsonObject joG = jsonValue.GetObject();
                    this.Groups.Add(new GroupKeys(joG));
                }
            }
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

        private void init()
        {
            if (this.Groups == null)
            {
                this.Groups = new ObservableCollection<GroupKeys>();
            }

            //init event handler
            this.Groups.CollectionChanged += GK_CollectionChanged;
        }

        public string ToJson()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject["FileName"] = JsonValue.CreateStringValue(this.FileName);
            jsonObject["Password"] = JsonValue.CreateStringValue(this.Password);
            jsonObject["Title"] = JsonValue.CreateStringValue(this.Title);
            JsonArray jsonGroups = new JsonArray();
            foreach (GroupKeys gTemp in this.Groups) {
                jsonGroups.Add(gTemp.ToJsonObject());
            }
            jsonObject["Groups"] = jsonGroups;


            return jsonObject.Stringify();
        }  

        public void save()
        {
            //TODO: hash file name
            if (String.IsNullOrEmpty(this.FileName)) {

                this.FileName = Path.GetInvalidFileNameChars().Aggregate(this.Title, (current, c) => current.Replace(c.ToString(), string.Empty));
            }


            //OLD (with json.net)
            //string body = JsonConvert.SerializeObject(this);

            //NEW
            string body = this.ToJson();

            Storage.saveFile(this.FileName, Crypto.Encrypt(body, this.Password));
        }

        public static Db GetFromJson(string json)
        {
            //OLD (with json.net)
            return JsonConvert.DeserializeObject<Db>(json);
        }
    }
}