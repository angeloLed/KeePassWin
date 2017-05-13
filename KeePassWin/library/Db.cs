using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KeePassWin
{
    public class Db
    {
        public string FileName { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Password { get; set; }
        public string Note { get; set; }
        public ObservableCollection<GroupKeys> Groups { get; set; }

        public static Db getFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Db>(json);
        }

        public void save()
        {
            if (String.IsNullOrEmpty(this.FileName)) {
                this.FileName = this.Title;
            }

            string body = JsonConvert.SerializeObject(this);
            Storage.saveFile(this.FileName, Crypto.Encrypt(body, this.Password));
        }
    }
}