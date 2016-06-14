using BaiRong.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

namespace SiteServer.WeiXin.Model
{
    public class CountAttribute
    {
        protected CountAttribute()
        {
        }

        public const string CountID = "CountID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string CountYear = "CountYear";
        public const string CountMonth = "CountMonth";
        public const string CountDay = "CountDay";
        public const string CountType = "CountType";
        public const string Count = "Count";

        private static List<string> allAttributes;
        public static List<string> AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new List<string>();
                    allAttributes.Add(CountID);
                    allAttributes.Add(PublishmentSystemID);
                    allAttributes.Add(CountYear);
                    allAttributes.Add(CountMonth);
                    allAttributes.Add(CountDay);
                    allAttributes.Add(CountType);
                    allAttributes.Add(Count);

                }

                return allAttributes;
            }
        }
    }

    public class CountInfo
    {
        private int countID;
        private int publishmentSystemID;
        private int countYear;
        private int countMonth;
        private int countDay;
        private ECountType countType;
        private int count;

        public CountInfo()
        {
            this.countID = 0;
            this.publishmentSystemID = 0;
            this.countYear = 0;
            this.countMonth = 0;
            this.countDay = 0;
            this.countType = ECountType.UserSubscribe;
            this.count = 0;
        }

        public CountInfo(int countID, int publishmentSystemID, int countYear, int countMonth, int countDay, ECountType countType, int count)
        {
            this.countID = countID;
            this.publishmentSystemID = publishmentSystemID;
            this.countYear = countYear;
            this.countMonth = countMonth;
            this.countDay = countDay;
            this.countType = countType;
            this.count = count;
        }


        public CountInfo(NameValueCollection form, bool isFilterSqlAndXss)
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

        public CountInfo(object dataItem)
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

        public object GetValue(string attributeName)
        {
            foreach (string name in this.AllAttributes)
            {
                if (StringUtils.EqualsIgnoreCase(name, attributeName))
                {
                    object nameVlaue = this.GetType().GetProperty(name).GetValue(this, null);

                    if (attributeName == "CountType")
                    {
                        return ECountTypeUtils.GetEnumType(nameVlaue.ToString());
                    }

                    return nameVlaue;
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
                if (StringUtils.ContainsIgnoreCase(name, "Is"))
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

        protected List<string> AllAttributes
        {
            get
            {
                return CountAttribute.AllAttributes;
            }
        }

        public int CountID
        {
            get { return countID; }
            set { countID = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public int CountYear
        {
            get { return countYear; }
            set { countYear = value; }
        }

        public int CountMonth
        {
            get { return countMonth; }
            set { countMonth = value; }
        }

        public int CountDay
        {
            get { return countDay; }
            set { countDay = value; }
        }

        public ECountType CountType
        {
            get { return countType; }
            set { countType = value; }
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
    }
}
