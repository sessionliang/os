using System;
using System.Collections;
using System.Text;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Xml;

namespace BaiRong.Core.Configuration
{
    public class ProviderConfig
    {
        // Fields
        private string _type;
        private string _description;
        private bool _Initialized;
        private string _name;
        private NameValueCollection _attributes;

        // Methods
        public ProviderConfig(XmlAttributeCollection xmlAttributes)
        {
            NameValueCollection attributes = new NameValueCollection();
            if (xmlAttributes != null && xmlAttributes.Count > 0)
            {
                foreach (XmlAttribute xmlAttribute in xmlAttributes)
                {
                    attributes.Add(xmlAttribute.Name.ToLower(), xmlAttribute.Value);
                }
            }
            this.Initialize(attributes);
        }

        public virtual void Initialize(NameValueCollection config)
        {
            lock (this)
            {
                if (this._Initialized)
                {
                    throw new InvalidOperationException("Provider Already Initialized");
                }
                this._Initialized = true;
            }
            if (string.IsNullOrEmpty(config["name"]))
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrEmpty(config["type"]))
            {
                throw new ArgumentNullException("type");
            }
            this._name = config["name"];
            this._type = config["type"];
            this._description = config["description"];
            config.Remove("name");
            config.Remove("type");
            config.Remove("description");
            this._attributes = config;
        }

        // Properties
        public virtual string Type
        {
            get
            {
                return this._type;
            }
        }

        public virtual string Description
        {
            get
            {
                if (!string.IsNullOrEmpty(this._description))
                {
                    return this._description;
                }
                return this.Name;
            }
        }

        public virtual string Name
        {
            get
            {
                return this._name;
            }
        }

        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(this.Attributes["connectionstring"]))
                {
                    return ConfigUtils.Instance.GetAppSettings(this.Attributes["connectionstringname"]);
                }
                else
                {
                    return this.Attributes["connectionstring"];
                }
            }
            set
            {
                this.Attributes["connectionstring"] = value;
            }
        }

        public NameValueCollection Attributes { get { return this._attributes; } }
    }
}
