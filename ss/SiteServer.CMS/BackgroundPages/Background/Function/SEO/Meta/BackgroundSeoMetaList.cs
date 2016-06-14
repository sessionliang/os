using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;

using SiteServer.CMS.Model;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundSeoMetaList : BackgroundBasePage
	{
        public DataGrid dgContents;
		public Button AddSeoMeta;

		public bool IsSetDefaultable(string isDefault)
		{
            return !TranslateUtils.ToBool(isDefault);
		}

		public bool IsDefault(string isDefault)
		{
            return TranslateUtils.ToBool(isDefault);
		}

		public string GetPageTitle(string pageTitle)
		{
			return StringUtils.MaxLengthText(pageTitle, 30);
		}

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (base.GetQueryString("Delete") != null)
			{
				int seoMetaID = base.GetIntQueryString("SeoMetaID");

				try
				{
                    SeoMetaInfo metaInfo = DataProvider.SeoMetaDAO.GetSeoMetaInfo(seoMetaID);
                    if (metaInfo != null)
                    {
                        DataProvider.SeoMetaDAO.Delete(seoMetaID);
                        StringUtility.AddLog(base.PublishmentSystemID, "删除页面元数据", string.Format("页面元数据:{0}", metaInfo.SeoMetaName));
                    }
					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
                    base.FailDeleteMessage(ex);
				}
			}
			else if (base.GetQueryString("SetDefault") != null)
			{
				int seoMetaID = base.GetIntQueryString("SeoMetaID");
                bool isSetDefault = TranslateUtils.ToBool(base.GetQueryString("SetDefault"));
			
				try
				{
					if (isSetDefault)
					{
						DataProvider.SeoMetaDAO.SetDefault(base.PublishmentSystemID, seoMetaID, true);
					}
					else
					{
                        DataProvider.SeoMetaDAO.SetDefault(base.PublishmentSystemID, seoMetaID, false);
					}
					base.SuccessMessage();
				}
				catch(Exception ex)
				{
					base.FailMessage(ex, "操作失败");
				}
			}

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_SEO, "页面元数据设置", AppManager.CMS.Permission.WebSite.SEO);

                this.dgContents.DataSource = DataProvider.SeoMetaDAO.GetDataSource(base.PublishmentSystemID);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

				this.AddSeoMeta.Attributes.Add("onclick", Modal.SeoMetaAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));
			}
		}

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int seoMetaID = TranslateUtils.EvalInt(e.Item.DataItem, "SeoMetaID");
                string seoMetaName = TranslateUtils.EvalString(e.Item.DataItem, "SeoMetaName");
                string pageTitle = TranslateUtils.EvalString(e.Item.DataItem, "PageTitle");
                bool isDefault = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsDefault"));

                Literal ltlSeoMetaName = e.Item.FindControl("ltlSeoMetaName") as Literal;
                Literal ltlPageTitle = e.Item.FindControl("ltlPageTitle") as Literal;
                Literal ltlIsDefault = e.Item.FindControl("ltlIsDefault") as Literal;
                HyperLink hlViewLink = e.Item.FindControl("hlViewLink") as HyperLink;
                HyperLink hlEditLink = e.Item.FindControl("hlEditLink") as HyperLink;
                Literal ltlDefaultUrl = e.Item.FindControl("ltlDefaultUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlSeoMetaName.Text = seoMetaName;
                ltlPageTitle.Text = pageTitle;
                ltlIsDefault.Text = StringUtils.GetTrueImageHtml(isDefault);
                hlViewLink.Attributes.Add("onclick", Modal.SeoMetaView.GetOpenWindowString(base.PublishmentSystemID, seoMetaID));
                hlEditLink.Attributes.Add("onclick", Modal.SeoMetaAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, seoMetaID));
                if (!isDefault)
                {
                    string urlDefault = PageUtils.GetCMSUrl(string.Format("background_seoMetaList.aspx?PublishmentSystemID={0}&SeoMetaID={1}&SetDefault=True", base.PublishmentSystemID, seoMetaID));
                    ltlDefaultUrl.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将把此项设为默认，确认吗？');"">设为默认</a>", urlDefault);
                }
                else
                {
                    string urlDefault = PageUtils.GetCMSUrl(string.Format("background_seoMetaList.aspx?PublishmentSystemID={0}&SeoMetaID={1}&SetDefault=False", base.PublishmentSystemID, seoMetaID));
                    ltlDefaultUrl.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将取消默认，确认吗？');"">取消默认</a>", urlDefault);
                }

                string urlDelete = PageUtils.GetCMSUrl(string.Format("background_seoMetaList.aspx?PublishmentSystemID={0}&Delete=True&SeoMetaID={1}", base.PublishmentSystemID, seoMetaID, seoMetaName));
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除页面元数据“{1}”，确认吗？');"">删除</a>", urlDelete, seoMetaName);
            }
		}
	}
}
