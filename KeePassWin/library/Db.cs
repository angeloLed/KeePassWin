using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace KeePassWin
{
    [Serializable]
    public class Db
    {
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        public string FileName { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Password { get; set; }
        public string Note { get; set; }

        [JsonProperty("groups")]
        public ObservableCollection<GroupKeys> Groups { get; set; }

        #endregion

        // constructor
        public Db()
        {
            if (this.Groups == null) {
                this.Groups = new ObservableCollection<GroupKeys>();
            }
            
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

        public static Db getFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Db>(json);
        }

        public void save()
        {
            //TODO: hash file name
            if (String.IsNullOrEmpty(this.FileName)) {
                this.FileName = this.Title;
            }

            string body = JsonConvert.SerializeObject(this);
            Storage.saveFile(this.FileName, Crypto.Encrypt(body, this.Password));
        }
    }
}