using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;

using BaiRong.Core;

namespace BaiRong.Core.Configuration
{
	public class PermissionConfig
	{
		string name;
		string text;

        public PermissionConfig(XmlAttributeCollection attributes) 
		{
            this.name = attributes["name"].Value;
            this.text = attributes["text"].Value;
		}

        public PermissionConfig(string name, string text)
        {
            this.name = name;
            this.text = text;
        }

		public string Name 
		{
			get 
			{
				return name;
			}
            set
            {
                name = value;
            }
		}

		public string Text 
		{
			get 
			{
				return text;
			}
            set
            {
                text = value;
            }
		}
	}
}
