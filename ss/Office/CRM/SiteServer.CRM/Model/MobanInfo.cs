using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CRM.Model
{
    public class MobanAttribute
    {
        protected MobanAttribute()
        {
        }

        //hidden
        public const string ID = "ID";

        //basic
        public const string Category = "Category";
        public const string SN = "SN";
        public const string IsAliyun = "IsAliyun";
        public const string IsInitializationForm = "IsInitializationForm";
        public const string Industry = "Industry";
        public const string Color = "Color";
        public const string AddDate = "AddDate";
        public const string Summary = "Summary";

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arraylist = new ArrayList(HiddenAttributes);
                arraylist.AddRange(BasicAttributes);
                return arraylist;
            }
        }

        private static ArrayList hiddenAttributes;
        public static ArrayList HiddenAttributes
        {
            get
            {
                if (hiddenAttributes == null)
                {
                    hiddenAttributes = new ArrayList();
                    hiddenAttributes.Add(ID.ToLower());
                }

                return hiddenAttributes;
            }
        }

        private static ArrayList basicAttributes;
        public static ArrayList BasicAttributes
        {
            get
            {
                if (basicAttributes == null)
                {
                    basicAttributes = new ArrayList();
                    basicAttributes.Add(Category.ToLower());
                    basicAttributes.Add(SN.ToLower());
                    basicAttributes.Add(IsAliyun.ToLower());
                    basicAttributes.Add(IsInitializationForm.ToLower());
                    basicAttributes.Add(Industry.ToLower());
                    basicAttributes.Add(Color.ToLower());
                    basicAttributes.Add(AddDate.ToLower());
                    basicAttributes.Add(Summary.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class MobanInfo : ExtendedAttributes
    {
        public const string TableName = "crm_Moban";

        public MobanInfo()
        {
            this.ID = 0;
        }

        public MobanInfo(object dataItem)
            : base(dataItem)
        {
        }

        public MobanInfo(int id)
        {
            this.ID = id;
        }

        public int ID
        {
            get { return base.GetInt(MobanAttribute.ID, 0); }
            set { base.SetExtendedAttribute(MobanAttribute.ID, value.ToString()); }
        }

        public string Category
        {
            get { return base.GetExtendedAttribute(MobanAttribute.Category); }
            set { base.SetExtendedAttribute(MobanAttribute.Category, value); }
        }

        public string SN
        {
            get { return base.GetExtendedAttribute(MobanAttribute.SN); }
            set { base.SetExtendedAttribute(MobanAttribute.SN, value); }
        }

        public bool IsAliyun
        {
            get { return base.GetBool(MobanAttribute.IsAliyun, false); }
            set { base.SetExtendedAttribute(MobanAttribute.IsAliyun, value.ToString()); }
        }

        public bool IsInitializationForm
        {
            get { return base.GetBool(MobanAttribute.IsInitializationForm, false); }
            set { base.SetExtendedAttribute(MobanAttribute.IsInitializationForm, value.ToString()); }
        }

        public string Industry
        {
            get { return base.GetExtendedAttribute(MobanAttribute.Industry); }
            set { base.SetExtendedAttribute(MobanAttribute.Industry, value); }
        }

        public string Color
        {
            get { return base.GetExtendedAttribute(MobanAttribute.Color); }
            set { base.SetExtendedAttribute(MobanAttribute.Color, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(MobanAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(MobanAttribute.AddDate, value.ToString()); }
        }

        public string Summary
        {
            get { return base.GetExtendedAttribute(MobanAttribute.Summary); }
            set { base.SetExtendedAttribute(MobanAttribute.Summary, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return MobanAttribute.AllAttributes;
        }
    }
}
