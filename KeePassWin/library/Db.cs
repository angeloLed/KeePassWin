using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace KeePassWin
{
    public class Db
    {
        public string FileName { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Password { get; set; }
        public string Note { get; set; }
        public List<KeyGroup> Groups { get; set; }

        public async void save()
        {
            if (this.FileName == "") {
                this.FileName = this.Title;
            }

            Storage.saveFile(this.FileName, JsonConvert.SerializeObject(this));
        }
    }
}