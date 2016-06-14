using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class RelatedFieldItemAdd : BackgroundBasePage
    {
        protected TextBox ItemNames;

        private int relatedFieldID;
        private int parentID;
        private int level;

        public static string GetOpenWindowString(int publishmentSystemID, int relatedFieldID, int parentID, int level)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedFieldID", relatedFieldID.ToString());
            arguments.Add("ParentID", parentID.ToString());
            arguments.Add("Level", level.ToString());
            return PageUtility.GetOpenWindowString("添加字段项", "modal_relatedFieldItemAdd.aspx", arguments, 300, 450);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.relatedFieldID = TranslateUtils.ToInt(base.GetQueryString("RelatedFieldID"));
            this.parentID = TranslateUtils.ToInt(base.GetQueryString("ParentID"));
            this.level = TranslateUtils.ToInt(base.GetQueryString("Level"));
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                string[] itemNameArray = this.ItemNames.Text.Split('\n');
                foreach (string item in itemNameArray)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string itemName = item.Trim();
                        string itemValue = itemName;

                        if (itemName.IndexOf('|') != -1)
                        {
                            itemValue = itemName.Substring(itemName.IndexOf('|') + 1);
                            itemName = itemName.Substring(0, itemName.IndexOf('|'));
                        }

                        RelatedFieldItemInfo itemInfo = new RelatedFieldItemInfo(0, this.relatedFieldID, itemName, itemValue, this.parentID, 0);
                        DataProvider.RelatedFieldItemDAO.Insert(itemInfo);
                    }
                }

                isChanged = true;
            }
            catch
            {
                isChanged = false;
                base.FailMessage("添加字段项出错！");
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundRelatedFieldItem.GetRedirectUrl(base.PublishmentSystemID, this.relatedFieldID, this.parentID, this.level));
            }
        }
    }
}
