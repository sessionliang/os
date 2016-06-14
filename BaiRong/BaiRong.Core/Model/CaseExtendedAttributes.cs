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
    public class CaseExtendedAttributes : Copyable
    {
        private readonly object dataItem;
        Hashtable extendedAttributes = new Hashtable();

        public CaseExtendedAttributes()
        {
        }

        public CaseExtendedAttributes(object dataItem)
        {
            this.dataItem = dataItem;
        }

        public virtual object GetExtendedAttribute(string name)
        {
            object returnValue = extendedAttributes[name];

            if (returnValue == null && this.dataItem != null)
            {
                returnValue = TranslateUtils.Eval(this.dataItem, name);
            }

            return (returnValue == null) ? string.Empty : returnValue;
        }

        public bool ContainsKey(string name)
        {
            object returnValue = extendedAttributes[name];

            if (returnValue == null && this.dataItem != null)
            {
                returnValue = TranslateUtils.Eval(this.dataItem, name);
            }

            return (returnValue == null) ? false : true;
        }

        public virtual void SetExtendedAttribute(string name, object value)
        {
            if (value == null)
                extendedAttributes.Remove(name);
            else
                extendedAttributes[name] = value;
        }

        public static void SetExtendedAttribute(Hashtable extendedAttributes, string name, object value)
        {
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
                    SetExtendedAttribute(key, attributes[key]);
                }
            }
        }

        public virtual Hashtable Attributes
        {
            get
            {
                return extendedAttributes;
            }
        }

        public int CaseExtendedAttributesCount
        {
            get { return extendedAttributes.Count; }
        }

        protected bool GetBool(string name, bool defaultValue)
        {
            object b = GetExtendedAttribute(name);
            if (b == null)
                return defaultValue;
            try
            {
                return bool.Parse(b.ToString());
            }
            catch { }
            return defaultValue;
        }

        protected int GetInt(string name, int defaultValue)
        {
            object i = GetExtendedAttribute(name);
            if (i == null)
                return defaultValue;

            int retval = defaultValue;
            try
            {
                retval = int.Parse(i.ToString());
            }
            catch { }
            return retval;
        }

        protected decimal GetDecimal(string name, decimal defaultValue)
        {
            object i = GetExtendedAttribute(name);
            if (i == null)
                return defaultValue;

            decimal retval = defaultValue;
            try
            {
                retval = decimal.Parse(i.ToString());
            }
            catch { }
            return retval;
        }

        protected DateTime GetDateTime(string name, DateTime defaultValue)
        {
            object d = GetExtendedAttribute(name);
            if (d == null)
                return defaultValue;

            DateTime retval = defaultValue;
            try
            {
                retval = DateTime.Parse(d.ToString());
            }
            catch { }
            return retval;
        }

        protected string GetString(string name, string defaultValue)
        {
            object v = GetExtendedAttribute(name);
            return (v == null) ? defaultValue : v.ToString();
        }

        public override object Copy()
        {
            CaseExtendedAttributes ea = (CaseExtendedAttributes)this.CreateNewInstance();
            ea.extendedAttributes = new Hashtable(this.extendedAttributes);
            return ea;
        }

        protected virtual ArrayList GetDefaultAttributesNames()
        {
            return new ArrayList();
        }
    }
}
