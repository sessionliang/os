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
    public class RelatedFieldItemEdit : BackgroundBasePage
    {
        protected TextBox ItemName;
        protected TextBox ItemValue;

        private int relatedFieldID;
        private int parentID;
        private int level;
        private int id;

        public static string GetOpenWindowString(int publishmentSystemID, int relatedFieldID, int parentID, int level, int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedFieldID", relatedFieldID.ToString());
            arguments.Add("ParentID", parentID.ToString());
            arguments.Add("Level", level.ToString());
            arguments.Add("ID", id.ToString());
            return PageUtility.GetOpenWindowString("编辑字段项", "modal_relatedFieldItemEdit.aspx", arguments, 320, 260);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.relatedFieldID = base.GetIntQueryString("RelatedFieldID");
            this.parentID = base.GetIntQueryString("ParentID");
            this.level = base.GetIntQueryString("Level");
            this.id = base.GetIntQueryString("ID");

            if (!IsPostBack)
            {
                RelatedFieldItemInfo itemInfo = DataProvider.RelatedFieldItemDAO.GetRelatedFieldItemInfo(this.id);
                this.ItemName.Text = itemInfo.ItemName;
                this.ItemValue.Text = itemInfo.ItemValue;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                RelatedFieldItemInfo itemInfo = DataProvider.RelatedFieldItemDAO.GetRelatedFieldItemInfo(this.id);
                itemInfo.ItemName = this.ItemName.Text;
                itemInfo.ItemValue = this.ItemValue.Text;
                DataProvider.RelatedFieldItemDAO.Update(itemInfo);

                isChanged = true;
            }
            catch(Exception ex)
            {
                isChanged = false;
                base.FailMessage(ex, ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundRelatedFieldItem.GetRedirectUrl(base.PublishmentSystemID, this.relatedFieldID, this.parentID, this.level));
            }
        }
    }
}
