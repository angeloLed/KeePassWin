using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeePassWin
{
    public class Emoji
    {
        public int Code { get; set; }
        public string Icon { get; set; }

        public Emoji(int code)
        {
            this.Code = code;
            this.Icon = this.GetIcon();
        }

        public string GetIcon()
        {
            return Char.ConvertFromUtf32(this.Code);
        }

        public int GetCode()
        {
            return this.Code;
        }
    }
}
