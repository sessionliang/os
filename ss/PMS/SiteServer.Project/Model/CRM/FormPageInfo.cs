using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.Project.Model
{
    public class FormPageAttribute
    {
        protected FormPageAttribute()
        {
        }

        //hidden
        public const string ID = "ID";

        //basic
        public const string MobanID = "MobanID";
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
                    basicAttributes.Add(MobanID.ToLower());
                    basicAttributes.Add(Title.ToLower());
                    basicAttributes.Add(Taxis.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class FormPageInfo : ExtendedAttributes
    {
        public const string TableName = "crm_FormPage";

        public FormPageInfo()
        {
            this.ID = 0;
        }

        public FormPageInfo(object dataItem)
            : base(dataItem)
        {
        }

        public FormPageInfo(int id)
        {
            this.ID = id;
        }

        public int ID
        {
            get { return base.GetInt(FormPageAttribute.ID, 0); }
            set { base.SetExtendedAttribute(FormPageAttribute.ID, value.ToString()); }
        }

        public int MobanID
        {
            get { return base.GetInt(FormPageAttribute.MobanID, 0); }
            set { base.SetExtendedAttribute(FormPageAttribute.MobanID, value.ToString()); }
        }

        public string Title
        {
            get { return base.GetExtendedAttribute(FormPageAttribute.Title); }
            set { base.SetExtendedAttribute(FormPageAttribute.Title, value); }
        }

        public int Taxis
        {
            get { return base.GetInt(FormPageAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(FormPageAttribute.Taxis, value.ToString()); }
        }
        protected override ArrayList GetDefaultAttributesNames()
        {
            return FormPageAttribute.AllAttributes;
        }
    }
}
