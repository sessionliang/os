using BaiRong.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using System.Text;

namespace BaiRong.Model
{
    public abstract class BaseInfo
    {
        public BaseInfo()
        {
        }

        public BaseInfo(object dataItem)
        {
            if (dataItem != null)
            {
                foreach (string name in this.AllAttributes)
                {
                    object value = TranslateUtils.Eval(dataItem, name);
                    if (value != null)
                    {
                        this.SetValueInternal(name, value);
                    }
                }
            }
        }

        public BaseInfo(NameValueCollection form) : this(form, false) 
        {

        }

        public BaseInfo(NameValueCollection form, bool isFilterSqlAndXss)
        {
            if (form != null)
            {
                foreach (string name in this.AllAttributes)
                {
                    string value = form[name];
                    if (value != null)
                    {
                        if (isFilterSqlAndXss)
                        {
                            value = PageUtils.FilterSqlAndXss(value);
                        }
                        this.SetValueInternal(name, value);
                    }
                }
            }
        }

        public BaseInfo(IDataReader rdr)
        {
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                string columnName = rdr.GetName(i);
                this.SetValue(columnName, rdr.GetValue(i));
            }
        }
        public int ID { get; set; }

        protected abstract List<string> AllAttributes { get; }

        public NameValueCollection ToNameValueCollection()
        {
            NameValueCollection attributes = new NameValueCollection();
            foreach (string attributeName in this.AllAttributes)
            {
                object value = this.GetType().GetProperty(attributeName).GetValue(this, null);
                if (value != null)
                {
                    attributes.Add(attributeName, value.ToString());
                }
            }
            return attributes;
        }

        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            foreach (string attributeName in this.AllAttributes)
            {
                object value = this.GetType().GetProperty(attributeName).GetValue(this, null);
                if (value != null)
                {
                    attributes.Add(attributeName, value);
                }
            }
            return attributes;
        }

        public object GetValue(string attributeName)
        {
            foreach (string name in this.AllAttributes)
            {
                if (StringUtils.EqualsIgnoreCase(name, attributeName))
                {
                    return this.GetType().GetProperty(name).GetValue(this, null);
                }
            }
            return null;
        }

        public void SetValue(string attributeName, object value)
        {
            foreach (string name in this.AllAttributes)
            {
                if (StringUtils.EqualsIgnoreCase(name, attributeName))
                {
                    try
                    {
                        this.SetValueInternal(name, value);
                    }
                    catch { }

                    break;
                }
            }
        }

        private void SetValueInternal(string name, object value)
        {
            try
            {
                this.GetType().GetProperty(name).SetValue(this, value, null);
            }
            catch
            {
                if (StringUtils.StartsWithIgnoreCase(name, "Is"))
                {
                    this.GetType().GetProperty(name).SetValue(this, TranslateUtils.ToBool(value.ToString()), null);
                }
                else if (StringUtils.EndsWithIgnoreCase(name, "Date"))
                {
                    this.GetType().GetProperty(name).SetValue(this, TranslateUtils.ToDateTime(value.ToString()), null);
                }
                else if (StringUtils.EndsWithIgnoreCase(name, "ID") || StringUtils.EndsWithIgnoreCase(name, "Num") || StringUtils.EndsWithIgnoreCase(name, "Count"))
                {
                    this.GetType().GetProperty(name).SetValue(this, TranslateUtils.ToInt(value.ToString()), null);
                }
                else if (StringUtils.EndsWithIgnoreCase(name, "Amount"))
                {
                    this.GetType().GetProperty(name).SetValue(this, TranslateUtils.ToDecimal(value.ToString()), null);
                }
            }
        }
    }
}
