using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.IO;
using System.Text;

using System.Web.UI.HtmlControls;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;
using SiteServer.B2C.Core;

namespace SiteServer.B2C.BackgroundPages
{
	public class BackgroundContentTranslate : BackgroundBasePage
	{
        public Literal ltlContents;
        public HtmlControl divTranslateAdd;
        public RadioButtonList ddlTranslateType;

        private Hashtable idsHashtable = new Hashtable();
        private string returnUrl;

        public static string GetRedirectClickStringForMultiChannels(int publishmentSystemID, string returnUrl)
        {
            string redirectUrl = PageUtils.GetB2CUrl(string.Format("background_contentTranslate.aspx?PublishmentSystemID={0}&ReturnUrl={1}", publishmentSystemID, StringUtils.ValueToUrl(returnUrl)));
            return JsUtils.GetRedirectStringWithCheckBoxValue(redirectUrl, "IDsCollection", "IDsCollection", "请选择需要转移的内容！");
        }

        public static string GetRedirectClickString(int publishmentSystemID, int nodeID, string returnUrl)
        {
            string redirectUrl = PageUtils.GetB2CUrl(string.Format("background_contentTranslate.aspx?PublishmentSystemID={0}&NodeID={1}&ReturnUrl={2}", publishmentSystemID, nodeID, StringUtils.ValueToUrl(returnUrl)));
            return JsUtils.GetRedirectStringWithCheckBoxValue(redirectUrl, "ContentIDCollection", "ContentIDCollection", "请选择需要转移的内容！");
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			PageUtils.CheckRequestParameter("PublishmentSystemID", "ReturnUrl");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            //if (!base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.ContentTranslate))
            //{
            //    PageUtils.RedirectToErrorPage("您没有此栏目的内容转移权限！");
            //    return;
            //}

            //bool isCut = base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.ContentDelete);
            bool isCut = true;
            this.idsHashtable = ContentUtility.GetIDsHashtable(base.Request.QueryString);

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "内容转移", string.Empty);

                StringBuilder builder = new StringBuilder();
                foreach (int nodeID in this.idsHashtable.Keys)
                {
                    ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeID);
                    string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
                    ArrayList contentIDArrayList = this.idsHashtable[nodeID] as ArrayList;
                    foreach (int contentID in contentIDArrayList)
                    {
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                        if (contentInfo != null)
                        {
                            builder.AppendFormat(@"{0}<br />", WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, this.returnUrl));
                        }
                    }
                }
                this.ltlContents.Text = builder.ToString();

                this.divTranslateAdd.Attributes.Add("onclick", ChannelMultipleSelect.GetOpenWindowString(base.PublishmentSystemID, true));

                ETranslateContentTypeUtils.AddListItems(this.ddlTranslateType, isCut);
                ControlUtils.SelectListItems(this.ddlTranslateType, ETranslateContentTypeUtils.GetValue(ETranslateContentType.Copy));
			}
		}

        private void AddSite(ListControl listControl, PublishmentSystemInfo publishmentSystemInfo, Hashtable parentWithChildren, int level)
        {
            string padding = string.Empty;
            for (int i = 0; i < level; i++)
            {
                padding += "　";
            }
            if (level > 0)
            {
                padding += "└ ";
            }

            if (parentWithChildren[publishmentSystemInfo.PublishmentSystemID] != null)
            {
                ArrayList children = (ArrayList)parentWithChildren[publishmentSystemInfo.PublishmentSystemID];

                ListItem listitem = new ListItem(padding + publishmentSystemInfo.PublishmentSystemName + string.Format("({0})", children.Count), publishmentSystemInfo.PublishmentSystemID.ToString());
                if (publishmentSystemInfo.PublishmentSystemID == base.PublishmentSystemID) listitem.Selected = true;

                listControl.Items.Add(listitem);
                level++;
                foreach (PublishmentSystemInfo subSiteInfo in children)
                {
                    AddSite(listControl, subSiteInfo, parentWithChildren, level);
                }
            }
            else
            {
                ListItem listitem = new ListItem(padding + publishmentSystemInfo.PublishmentSystemName, publishmentSystemInfo.PublishmentSystemID.ToString());
                if (publishmentSystemInfo.PublishmentSystemID == base.PublishmentSystemID) listitem.Selected = true;

                listControl.Items.Add(listitem);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (!string.IsNullOrEmpty(base.Request.Form["translateCollection"]))
                {
                    try
                    {
                        ETranslateContentType translateType = ETranslateContentTypeUtils.GetEnumType(this.ddlTranslateType.SelectedValue);

                        foreach (int nodeID in this.idsHashtable.Keys)
                        {
                            ArrayList contentIDArrayList = this.idsHashtable[nodeID] as ArrayList;
                            contentIDArrayList.Reverse();
                            if (contentIDArrayList.Count > 0)
                            {
                                StringBuilder builder = new StringBuilder();
                                foreach (int contentID in contentIDArrayList)
                                {
                                    builder.AppendFormat("{0}_{1},", nodeID, contentID);
                                }
                                builder.Length = builder.Length - 1;

                                ContentUtilityB2C.Translate(base.PublishmentSystemInfo, builder.ToString(), base.Request.Form["translateCollection"], translateType);
                            }
                        }

                        base.SuccessMessage("内容转移成功！");
                        base.AddWaitAndRedirectScript(this.returnUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "内容转移失败！");
                        LogUtils.AddErrorLog(ex);
                    }
                }
                else
                {
                    base.FailMessage("请选择需要转移到的栏目！");
                }
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.returnUrl);
        }
	}
}
