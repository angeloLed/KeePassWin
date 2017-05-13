using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace KeePassWin
{
    public class GroupKeys
    {
        internal ObservableCollection<Key> keys { get; set; }
        internal string name { get; set; }
        internal string description { get; set; }
        internal string Note { get; set; }


        public GroupKeys()
        {
            this.keys.CollectionChanged += (sender, ev) =>
            {
            };
        }
    }
}