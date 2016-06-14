using BaiRong.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

namespace SiteServer.WeiXin.Model
{
    public class KeyWordAttribute
    {
        protected KeyWordAttribute()
        {
        }

        public const string KeywordID = "KeywordID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string Keywords = "Keywords";
        public const string IsDisabled = "IsDisabled";
        public const string KeywordType = "KeywordType";
        public const string MatchType = "MatchType";
        public const string Reply = "Reply";
        public const string AddDate = "AddDate";
        public const string Taxis = "Taxis";

        private static List<string> allAttributes;
        public static List<string> AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new List<string>();
                    allAttributes.Add(KeywordID);
                    allAttributes.Add(PublishmentSystemID);
                    allAttributes.Add(Keywords);
                    allAttributes.Add(IsDisabled);
                    allAttributes.Add(KeywordType);
                    allAttributes.Add(MatchType);
                    allAttributes.Add(Reply);
                    allAttributes.Add(AddDate);
                    allAttributes.Add(Taxis);
                }

                return allAttributes;
            }
        }
    }

    public class KeywordInfo
    {
        private int keywordID;
        private int publishmentSystemID;
        private string keywords;
        private bool isDisabled;
        private EKeywordType keywordType;
        private EMatchType matchType;
        private string reply;
        private DateTime addDate;
        private int taxis;

        public KeywordInfo()
        {
            this.keywordID = 0;
            this.publishmentSystemID = 0;
            this.keywords = string.Empty;
            this.isDisabled = false;
            this.keywordType = EKeywordType.Text;
            this.matchType = EMatchType.Exact;
            this.reply = string.Empty;
            this.addDate = DateTime.Now;
            this.taxis = 0;
        }

        public KeywordInfo(int keywordID, int publishmentSystemID, string keywords, bool isDisabled, EKeywordType keywordType, EMatchType matchType, string reply, DateTime addDate, int taxis)
        {
            this.keywordID = keywordID;
            this.publishmentSystemID = publishmentSystemID;
            this.keywords = keywords;
            this.isDisabled = isDisabled;
            this.keywordType = keywordType;
            this.matchType = matchType;
            this.reply = reply;
            this.addDate = addDate;
            this.taxis = taxis;
        }

        public KeywordInfo(object dataItem)
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

        public KeywordInfo(NameValueCollection form, bool isFilterSqlAndXss)
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

        public object GetValue(string attributeName)
        {
            foreach (string name in this.AllAttributes)
            {
                if (StringUtils.EqualsIgnoreCase(name, attributeName))
                {
                    object nameVlaue = this.GetType().GetProperty(name).GetValue(this, null);

                    if (attributeName == "KeywordType")
                    {
                        return EKeywordTypeUtils.GetEnumType(nameVlaue.ToString());
                    }
                    if (attributeName == "MatchType")
                    {
                        return EMatchTypeUtils.GetEnumType(nameVlaue.ToString());
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
                return KeyWordAttribute.AllAttributes;
            }
        }

        public int KeywordID
        {
            get { return keywordID; }
            set { keywordID = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public string Keywords
        {
            get { return keywords; }
            set { keywords = value; }
        }

        public bool IsDisabled
        {
            get { return isDisabled; }
            set { isDisabled = value; }
        }

        public EKeywordType KeywordType
        {
            get { return keywordType; }
            set { keywordType = value; }
        }

        public EMatchType MatchType
        {
            get { return matchType; }
            set { matchType = value; }
        }

        public string Reply
        {
            get { return reply; }
            set { reply = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
    }
}
