using System;
using System.Text;

namespace BaiRong.Core
{
    public class Pair
    {
        private string key;
        private object value;

        public Pair(string key, object value)
        {
            this.key = key;
            this.value = value;
        }

        public string Key
        {
            get
            {
                return this.key;
            }
        }

        public object Value
        {
            get
            {
                return this.value;
            }
        }
    }
}
