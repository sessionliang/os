using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CRM.Model
{
    public class FormElementItemInfo
    {
        private int id;
        private int formElementID;
        private string itemTitle;
        private string itemValue;
        private bool isSelected;

        public FormElementItemInfo()
        {
            this.id = 0;
            this.formElementID = 0;
            this.itemTitle = string.Empty;
            this.itemValue = string.Empty;
            this.isSelected = false;
        }

        public FormElementItemInfo(int id, int formElementID, string itemTitle, string itemValue, bool isSelected)
        {
            this.id = id;
            this.formElementID = formElementID;
            this.itemTitle = itemTitle;
            this.itemValue = itemValue;
            this.isSelected = isSelected;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int FormElementID
        {
            get { return formElementID; }
            set { formElementID = value; }
        }

        public string ItemTitle
        {
            get { return itemTitle; }
            set { itemTitle = value; }
        }

        public string ItemValue
        {
            get { return itemValue; }
            set { itemValue = value; }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }
    }
}
