using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Windows.Data.Json;

namespace KeePassWin
{
    public class Key
    {
        #region CONTRUCTORS

        public Key()
        {
        }

        public Key(JsonObject joK)
        {
            this.CreateAt = joK.GetNamedString("CreateAt", "");
            this.Icon = joK.GetNamedString("Icon", "");
            this.Note = joK.GetNamedString("Note", "");
            this.Password = joK.GetNamedString("Password", "");
            this.Title = joK.GetNamedString("Title", "");
            this.UpdateAt = joK.GetNamedString("UpdateAt", "");
            this.Url = joK.GetNamedString("Url", "");
            this.Username = joK.GetNamedString("Username", "");
        }

        #endregion


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
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject["Url"] = JsonValue.CreateStringValue(this.Url);
            jsonObject["Username"] = JsonValue.CreateStringValue(this.Username);
            jsonObject["Password"] = JsonValue.CreateStringValue(this.Password);
            jsonObject["Icon"] = JsonValue.CreateStringValue(this.Icon);
            jsonObject["Title"] = JsonValue.CreateStringValue(this.Title);
            jsonObject["Note"] = JsonValue.CreateStringValue(this.Note);
            jsonObject["CreateAt"] = JsonValue.CreateStringValue(this.CreateAt);
            jsonObject["UpdateAt"] = JsonValue.CreateStringValue(this.UpdateAt);

            return jsonObject;
        }
    }
}