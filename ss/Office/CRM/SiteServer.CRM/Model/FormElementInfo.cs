using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CRM.Model
{
    public class FormElementInfo
    {
        private int id;
        private int pageID;
        private int groupID;
        private string attributeName;
        private int taxis;
        private string displayName;
        private string helpText;
        private string imageUrl;
        private bool isVisible;
        private bool isVisibleInList;
        private bool isSingleLine;
        private EInputType inputType;
        private string defaultValue;
        private bool isHorizontal;
        private string extendValues;

        public FormElementInfo()
        {
            this.id = 0;
            this.pageID = 0;
            this.groupID = 0;
            this.attributeName = string.Empty;
            this.taxis = 0;
            this.displayName = string.Empty;
            this.helpText = string.Empty;
            this.imageUrl = string.Empty;
            this.isVisible = true;
            this.isVisibleInList = false;
            this.isSingleLine = true;
            this.inputType = EInputType.Text;
            this.defaultValue = string.Empty;
            this.isHorizontal = true;
            this.extendValues = string.Empty;
        }

        public FormElementInfo(int id, int pageID, int groupID, string attributeName, int taxis, string displayName, string helpText, string imageUrl, bool isVisible, bool isVisibleInList, bool isSingleLine, EInputType inputType, string defaultValue, bool isHorizontal, string extendValues)
        {
            this.id = id;
            this.pageID = pageID;
            this.groupID = groupID;
            this.attributeName = attributeName;
            this.taxis = taxis;
            this.displayName = displayName;
            this.helpText = helpText;
            this.imageUrl = imageUrl;
            this.isVisible = isVisible;
            this.isVisibleInList = isVisibleInList;
            this.isSingleLine = isSingleLine;
            this.inputType = inputType;
            this.defaultValue = defaultValue;
            this.isHorizontal = isHorizontal;
            this.extendValues = extendValues;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int PageID
        {
            get { return pageID; }
            set { pageID = value; }
        }

        public int GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }

        public string AttributeName
        {
            get { return attributeName; }
            set { attributeName = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        public string HelpText
        {
            get { return helpText; }
            set { helpText = value; }
        }

        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        public bool IsVisibleInList
        {
            get { return isVisibleInList; }
            set { isVisibleInList = value; }
        }

        public bool IsSingleLine
        {
            get { return isSingleLine; }
            set { isSingleLine = value; }
        }

        public EInputType InputType
        {
            get { return inputType; }
            set { inputType = value; }
        }

        public string DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        public bool IsHorizontal
        {
            get { return isHorizontal; }
            set { isHorizontal = value; }
        }

        public string ExtendValues
        {
            get { return extendValues; }
            set { extendValues = value; }
        }

        FormElementInfoExtend additional;
        public FormElementInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new FormElementInfoExtend(this.extendValues);
                }
                return this.additional;
            }
        }

        private ArrayList styleItems;
        public ArrayList StyleItems
        {
            get { return styleItems; }
            set { styleItems = value; }
        }
    }

    public class FormElementInfoExtend : ExtendedAttributes
    {
        public FormElementInfoExtend(string extendValues)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(extendValues);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public int Height
        {
            get { return base.GetInt("Height", 0); }
            set { base.SetExtendedAttribute("Height", value.ToString()); }
        }

        public string Width
        {
            get
            {
                string width = base.GetExtendedAttribute("Width");
                if (width == "0")
                {
                    return string.Empty;
                }
                return width;
            }
            set { base.SetExtendedAttribute("Width", value); }
        }

        public int Columns
        {
            get { return base.GetInt("Columns", 0); }
            set { base.SetExtendedAttribute("Columns", value.ToString()); }
        }

        public bool IsFormatString
        {
            get { return base.GetBool("IsFormatString", false); }
            set { base.SetExtendedAttribute("IsFormatString", value.ToString()); }
        }

        public string EditorTypeString
        {
            get { return base.GetExtendedAttribute("EditorTypeString"); }
            set { base.SetExtendedAttribute("EditorTypeString", value); }
        }

        public int RelatedFieldID
        {
            get { return base.GetInt("RelatedFieldID", 0); }
            set { base.SetExtendedAttribute("RelatedFieldID", value.ToString()); }
        }

        public string RelatedFieldStyle
        {
            get { return base.GetExtendedAttribute("RelatedFieldStyle"); }
            set { base.SetExtendedAttribute("RelatedFieldStyle", value); }
        }

        public bool IsValidate
        {
            get { return base.GetBool("IsValidate", false); }
            set { base.SetExtendedAttribute("IsValidate", value.ToString()); }
        }

        public bool IsRequired
        {
            get { return base.GetBool("IsRequired", false); }
            set { base.SetExtendedAttribute("IsRequired", value.ToString()); }
        }

        public int MinNum
        {
            get { return base.GetInt("MinNum", 0); }
            set { base.SetExtendedAttribute("MinNum", value.ToString()); }
        }

        public int MaxNum
        {
            get { return base.GetInt("MaxNum", 0); }
            set { base.SetExtendedAttribute("MaxNum", value.ToString()); }
        }

        public EInputValidateType ValidateType
        {
            get { return EInputValidateTypeUtils.GetEnumType(base.GetExtendedAttribute("ValidateType")); }
            set { base.SetExtendedAttribute("ValidateType", EInputValidateTypeUtils.GetValue(value)); }
        }

        public string RegExp
        {
            get { return base.GetExtendedAttribute("RegExp"); }
            set { base.SetExtendedAttribute("RegExp", value); }
        }

        public string ErrorMessage
        {
            get { return base.GetExtendedAttribute("ErrorMessage"); }
            set { base.SetExtendedAttribute("ErrorMessage", value); }
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }
    }
}
