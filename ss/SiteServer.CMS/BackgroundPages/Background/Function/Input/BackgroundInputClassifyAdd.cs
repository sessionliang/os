using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundInputClassifyAdd : BackgroundBasePage
    {
        public DropDownList ParentItemID;
        public TextBox ItemName;
        public TextBox ItemIndexName;
        public Button TypeButton;

        private int itemID;
        private string returnUrl;
        private string typeName;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ItemID", "ReturnUrl");
            this.itemID = base.GetIntQueryString("ItemID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            string parentItemID = base.GetQueryString("parentItemID");

            if (this.itemID == 0)
            {
                this.typeName = "添加";
            }
            else
                this.typeName = "修改";
            // this.TypeButton.Text = this.typeName;
            InputClissifyInfo itemInfo = DataProvider.InputClassifyDAO.GetInputClassifyInfo(this.itemID);

            if (!base.IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, this.typeName + "分类", string.Empty);
                 
                TreeManager.AddListItems(this.ParentItemID.Items, base.PublishmentSystemID, this.itemID, true, true, "InputClassify", 2);
                ControlUtils.SelectListItems(this.ParentItemID, parentItemID);

                if (this.itemID > 0)
                {
                    this.ItemName.Text = itemInfo.ItemName;
                    this.ItemIndexName.Text = itemInfo.ItemIndexName;
                    ControlUtils.SelectListItems(this.ParentItemID, string.IsNullOrEmpty(parentItemID) ? itemInfo.ParentID.ToString() : parentItemID);
                }
            }
        }

        public void ParentItemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int theItemID = TranslateUtils.ToInt(this.ParentItemID.SelectedValue);
            if (theItemID == 0)
            {
                theItemID = this.itemID;
            }
            PageUtils.Redirect(BackgroundInputClassifyAdd.GetRedirectUrl(base.PublishmentSystemID, TranslateUtils.ToInt(base.GetQueryString("itemID")), theItemID, base.GetQueryString("ReturnUrl")));
        }

        public static string GetRedirectUrl(int publishmentSystemID, int itemID, int parentID, string returnUrl)
        {
            return PageUtils.GetCMSUrl(string.Format("background_inputClassifyAdd.aspx?PublishmentSystemID={0}&ItemID={1}&parentItemID={2}&ReturnUrl={3}", publishmentSystemID, itemID, parentID, StringUtils.ValueToUrl(returnUrl)));
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                int insertItemID = 0;

                try
                {

                    if (this.itemID == 0)
                    {
                        int parentItemID = TranslateUtils.ToInt(base.Request.Form["parentItemID"]);
                        InputClissifyInfo itemInfo = new InputClissifyInfo();
                        itemInfo.ParentID = parentItemID;
                        itemInfo.PublishmentSystemID = base.PublishmentSystemID;
                        itemInfo.ItemName = this.ItemName.Text;
                        itemInfo.ItemIndexName = this.ItemIndexName.Text;
                        itemInfo.Enabled = true;

                        if (ItemIndexName.Text.Length != 0)
                        {
                            ArrayList itemIndexNameArrayList = DataProvider.InputClassifyDAO.GetItemIndexNameArrayList(base.PublishmentSystemID);
                            if (itemIndexNameArrayList.IndexOf(ItemIndexName.Text) != -1)
                            {
                                base.FailMessage("分类" + this.typeName + "失败，分类索引已存在！");
                                return;
                            }
                        }

                        itemInfo.AddDate = DateTime.Now;

                        insertItemID = DataProvider.InputClassifyDAO.InsertInputClassifyInfo(itemInfo);
                    }
                    else
                    {
                        if (this.ParentItemID.SelectedValue == this.itemID.ToString())
                        {
                            base.FailMessage("分类" + this.typeName + "失败，不成为自己的下级！");
                            return;
                        }
                        int parentItemID = TranslateUtils.ToInt(base.Request.Form["parentItemID"]);
                        InputClissifyInfo itemInfo = DataProvider.InputClassifyDAO.GetInputClassifyInfo(this.itemID);
                        if (itemInfo.ParentID > 0)
                            itemInfo.ParentID = parentItemID;
                        itemInfo.ItemName = this.ItemName.Text;


                        if (ItemIndexName.Text.Length != 0 && ItemIndexName.Text != itemInfo.ItemIndexName)
                        {
                            ArrayList itemIndexNameArrayList = DataProvider.KeywordClassifyDAO.GetItemIndexNameArrayList(base.PublishmentSystemID);
                            if (itemIndexNameArrayList.IndexOf(ItemIndexName.Text) != -1)
                            {
                                base.FailMessage("分类修改失败，分类索引已存在！");
                                return;
                            }
                        }

                        itemInfo.ItemIndexName = this.ItemIndexName.Text;
                        DataProvider.InputClassifyDAO.UpdateInputClassifyInfo(itemInfo);
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("分类" + this.typeName + "失败：{0}", ex.Message));
                    LogUtils.AddErrorLog(ex);
                    return;
                }

                base.SuccessMessage("分类" + this.typeName + "成功！");
                base.AddWaitAndRedirectScript(this.returnUrl);
            }
        }

        public string ReturnUrl { get { return this.returnUrl; } }
    }
}
