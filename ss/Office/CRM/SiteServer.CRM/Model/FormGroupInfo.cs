using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CRM.Model
{
    public class FormGroupAttribute
    {
        protected FormGroupAttribute()
        {
        }

        //hidden
        public const string ID = "ID";

        //basic
        public const string PageID = "PageID";
        public const string IconUrl = "IconUrl";
        public const string Title = "Title";
        public const string Taxis = "Taxis";

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
                    basicAttributes.Add(PageID.ToLower());
                    basicAttributes.Add(IconUrl.ToLower());
                    basicAttributes.Add(Title.ToLower());
                    basicAttributes.Add(Taxis.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class FormGroupInfo : ExtendedAttributes
    {
        public const string TableName = "crm_FormGroup";

        public FormGroupInfo()
        {
            this.ID = 0;
        }

        public FormGroupInfo(object dataItem)
            : base(dataItem)
        {
        }

        public FormGroupInfo(int id)
        {
            this.ID = id;
        }

        public int ID
        {
            get { return base.GetInt(FormGroupAttribute.ID, 0); }
            set { base.SetExtendedAttribute(FormGroupAttribute.ID, value.ToString()); }
        }

        public int PageID
        {
            get { return base.GetInt(FormGroupAttribute.PageID, 0); }
            set { base.SetExtendedAttribute(FormGroupAttribute.PageID, value.ToString()); }
        }

        public string IconUrl
        {
            get { return base.GetExtendedAttribute(FormGroupAttribute.IconUrl); }
            set { base.SetExtendedAttribute(FormGroupAttribute.IconUrl, value); }
        }

        public string Title
        {
            get { return base.GetExtendedAttribute(FormGroupAttribute.Title); }
            set { base.SetExtendedAttribute(FormGroupAttribute.Title, value); }
        }

        public int Taxis
        {
            get { return base.GetInt(FormGroupAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(FormGroupAttribute.Taxis, value.ToString()); }
        }
        protected override ArrayList GetDefaultAttributesNames()
        {
            return FormGroupAttribute.AllAttributes;
        }
    }
}
