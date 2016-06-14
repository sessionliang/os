using System;
using System.Collections.Specialized;
using System.Collections;
using BaiRong.Core;

namespace BaiRong.Model
{
    /// <summary>
    /// Provides standard implementation for simple extendent data storage
    /// </summary>
    [Serializable]
    public class ExtendedAttributes : Copyable
    {
        private readonly object dataItem;
        NameValueCollection extendedAttributes = new NameValueCollection();

        public ExtendedAttributes()
        {
        }

        public ExtendedAttributes(object dataItem)
        {
            this.dataItem = dataItem;
            this.AfterExecuteReader();
        }

        public virtual string GetExtendedAttribute(string name)
        {
            name = name.ToLower();
            string returnValue = extendedAttributes[name];

            if (returnValue == null && this.dataItem != null)
            {
                object obj = TranslateUtils.Eval(this.dataItem, name);
                if (obj != null)
                {
                    if (obj is string)
                    {
                        returnValue = extendedAttributes[name] = obj as string;
                    }
                    else
                    {
                        returnValue = extendedAttributes[name] = obj.ToString();
                    }
                }
            }

            return (returnValue == null) ? string.Empty : returnValue;
        }

        public bool ContainsKey(string name)
        {
            name = name.ToLower();
            string returnValue = extendedAttributes[name];

            if (returnValue == null && this.dataItem != null)
            {
                object obj = TranslateUtils.Eval(this.dataItem, name);
                if (obj != null)
                {
                    if (obj is string)
                    {
                        returnValue = extendedAttributes[name] = obj as string;
                    }
                    else
                    {
                        returnValue = extendedAttributes[name] = obj.ToString();
                    }
                }
            }

            return (returnValue == null) ? false : true;
        }

        public virtual void SetExtendedAttribute(string name, string value)
        {
            name = name.ToLower();

            if (value == null)
                extendedAttributes.Remove(name);
            else
                extendedAttributes[name] = value;
        }

        public static void SetExtendedAttribute(NameValueCollection extendedAttributes, string name, string value)
        {
            name = name.ToLower();

            if (value == null)
                extendedAttributes.Remove(name);
            else
                extendedAttributes[name] = value;
        }

        public void SetExtendedAttribute(NameValueCollection attributes)
        {
            if (attributes != null)
            {
                foreach (string key in attributes)
                {
                    SetExtendedAttribute(key.ToLower(), attributes[key]);
                }
            }
        }

        public virtual NameValueCollection Attributes
        {
            get
            {
                return extendedAttributes;
            }
        }

        public int ExtendedAttributesCount
        {
            get { return extendedAttributes.Count; }
        }

        protected bool GetBool(string name, bool defaultValue)
        {
            name = name.ToLower();
            string b = GetExtendedAttribute(name);
            if (b == null || b.Trim().Length == 0)
                return defaultValue;
            try
            {
                return bool.Parse(b);
            }
            catch { }
            return defaultValue;
        }

        protected int GetInt(string name, int defaultValue)
        {
            name = name.ToLower();
            string i = GetExtendedAttribute(name);
            if (i == null || i.Trim().Length == 0)
                return defaultValue;

            int retval = defaultValue;
            try
            {
                retval = int.Parse(i);
            }
            catch { }
            return retval;
        }

        protected decimal GetDecimal(string name, decimal defaultValue)
        {
            name = name.ToLower();
            string i = GetExtendedAttribute(name);
            if (i == null || i.Trim().Length == 0)
                return defaultValue;

            decimal retval = defaultValue;
            try
            {
                retval = decimal.Parse(i);
            }
            catch { }
            return retval;
        }

        protected DateTime GetDateTime(string name, DateTime defaultValue)
        {
            name = name.ToLower();
            string d = GetExtendedAttribute(name);
            if (d == null || d.Trim().Length == 0)
                return defaultValue;

            DateTime retval = defaultValue;
            try
            {
                retval = DateTime.Parse(d);
            }
            catch { }
            return retval;
        }

        protected string GetString(string name, string defaultValue)
        {
            name = name.ToLower();
            string v = GetExtendedAttribute(name);
            return (string.IsNullOrEmpty(v)) ? defaultValue : v;
        }

        public override object Copy()
        {
            ExtendedAttributes ea = (ExtendedAttributes)this.CreateNewInstance();
            ea.extendedAttributes = new NameValueCollection(this.extendedAttributes);
            return ea;
        }

        private string SettingsXML
        {
            get { return this.GetExtendedAttribute("SettingsXML"); }
            set { this.SetExtendedAttribute("SettingsXML", value); }
        }

        protected virtual ArrayList GetDefaultAttributesNames()
        {
            return new ArrayList();
        }

        //将数据保存至数据库前执行
        public void BeforeExecuteNonQuery()
        {
            NameValueCollection attributes = new NameValueCollection(this.Attributes);

            foreach (string attributeName in this.GetDefaultAttributesNames())
            {
                attributes.Remove(attributeName.ToLower());
            }

            this.SettingsXML = TranslateUtils.NameValueCollectionToString(attributes);
        }


        public string GetSettingsXML()
        {
            this.BeforeExecuteNonQuery();
            return this.SettingsXML;
        }


        //从数据库获取数据后执行
        public void AfterExecuteReader()
        {
            NameValueCollection attributes = TranslateUtils.ToNameValueCollection(this.SettingsXML);
            if (attributes != null)
            {
                foreach (string key in attributes)
                {
                    if (string.IsNullOrEmpty(this.GetExtendedAttribute(key)))
                    {
                        SetExtendedAttribute(key.ToLower(), attributes[key]);
                    }
                }
            }
            //this.SetExtendedAttribute(TranslateUtils.ToNameValueCollection(this.SettingsXML));
        }

        #region Serialization

        public SerializerData GetSerializerData()
        {
            SerializerData data = new SerializerData();

            string keys = null;
            string values = null;

            Serializer.ConvertFromNameValueCollection(this.extendedAttributes, ref keys, ref values);
            data.Keys = keys;
            data.Values = values;

            return data;
        }

        public void SetSerializerData(SerializerData data)
        {
            if (this.extendedAttributes == null || this.extendedAttributes.Count == 0)
            {
                this.extendedAttributes = Serializer.ConvertToNameValueCollection(data.Keys, data.Values);
            }

            if (this.extendedAttributes == null)
                extendedAttributes = new NameValueCollection();
        }

        public override string ToString()
        {
            if (this.extendedAttributes != null && this.extendedAttributes.Count > 0)
            {
                return TranslateUtils.NameValueCollectionToString(this.extendedAttributes);
            }
            return string.Empty;
        }

        public static ExtendedAttributes Parse(string str)
        {
            ExtendedAttributes eas = new ExtendedAttributes();
            eas.extendedAttributes = TranslateUtils.ToNameValueCollection(str);
            return eas;
        }
        #endregion
    }
}
