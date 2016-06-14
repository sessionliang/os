using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using BaiRong.Core;

namespace BaiRong.Core
{
	public class LowerNameValueCollection : NameValueCollection
	{
        public override void Add(string name, string value)
        {
            if (!string.IsNullOrEmpty(name))
            {
                base.Add(name.ToLower(), value);
            }
        }

        public override void Set(string name, string value)
        {
            if (!string.IsNullOrEmpty(name))
            {
                base.Set(name.ToLower(), value);
            }
        }

        public override string Get(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return base.Get(name.ToLower());
            }
            return null;
        }

        public override void Remove(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                base.Remove(name.ToLower());
            }
        }

        public override string[] GetValues(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                base.GetValues(name.ToLower());
            }
            return null;
        }
	}
}
