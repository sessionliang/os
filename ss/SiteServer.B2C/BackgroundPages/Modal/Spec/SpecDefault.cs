using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Collections.Specialized;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using System.Collections.Generic;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.B2C.BackgroundPages.Modal
{
    public class SpecDefault : BackgroundBasePage
    {
        protected Literal ltlSpec;
        protected CheckBoxList cblDefault;
        protected RadioButtonList rblDefault;

        private int specID;

        public static string GetOpenWindowString(int publishmentSystemID, int specID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("SpecID", specID.ToString());
            return PageUtilityB2C.GetOpenWindowString("设置默认项", "modal_specDefault.aspx", arguments, 500, 400);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.specID = base.GetIntQueryString("SpecID");

            if (!IsPostBack)
            {
                SpecInfo specInfo = SpecManager.GetSpecInfo(base.PublishmentSystemID, this.specID);

                this.ltlSpec.Text = specInfo.SpecName;

                List<SpecItemInfo> specItemInfoList = SpecItemManager.GetSpecItemInfoList(base.PublishmentSystemID, this.specID);
                foreach (SpecItemInfo itemInfo in specItemInfoList)
                {
                    ListItem listItem = new ListItem(itemInfo.Title, itemInfo.ItemID.ToString());
                    listItem.Selected = itemInfo.IsDefault;
                    if (specInfo.IsMultiple)
                    {
                        this.cblDefault.Visible = true;
                        this.rblDefault.Visible = false;
                        this.cblDefault.Items.Add(listItem);
                    }
                    else
                    {
                        this.rblDefault.Visible = true;
                        this.cblDefault.Visible = false;
                        this.rblDefault.Items.Add(listItem);
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                SpecInfo specInfo = SpecManager.GetSpecInfo(base.PublishmentSystemID, this.specID);
                ArrayList itemIDArrayList = new ArrayList();
                if (specInfo.IsMultiple)
                {
                    itemIDArrayList = ControlUtils.GetSelectedListControlValueIntArrayList(this.cblDefault);
                }
                else
                {
                    itemIDArrayList = ControlUtils.GetSelectedListControlValueIntArrayList(this.rblDefault);
                }
                List<SpecItemInfo> specItemInfoList = SpecItemManager.GetSpecItemInfoList(base.PublishmentSystemID, this.specID);
                foreach (SpecItemInfo itemInfo in specItemInfoList)
                {
                    itemInfo.IsDefault = itemIDArrayList.Contains(itemInfo.ItemID);
                    DataProviderB2C.SpecItemDAO.Update(base.PublishmentSystemID, itemInfo);
                }

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "设置默认项失败！");
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
    }
}
