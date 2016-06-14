using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;



namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundAdvertisement : BackgroundBasePage
	{
		public DropDownList AdvertisementType;
        public DataGrid dgContents;

		public Button AddAdvertisement;
		public Button Delete;

		public string GetAdvertisementType(string advertisementTypeStr)
		{
			EAdvertisementType adType = EAdvertisementTypeUtils.GetEnumType(advertisementTypeStr);
			return EAdvertisementTypeUtils.GetText(adType);
		}

        public string GetAdvertisementInString(string advertisementName)
		{
            StringBuilder builder = new StringBuilder();
            AdvertisementInfo adInfo = DataProvider.AdvertisementDAO.GetAdvertisementInfo(advertisementName, base.PublishmentSystemID);
            if (!string.IsNullOrEmpty(adInfo.NodeIDCollectionToChannel))
            {
                builder.Append("栏目：");
                ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(adInfo.NodeIDCollectionToChannel);
                foreach (int nodeID in nodeIDArrayList)
                {
                    builder.Append(NodeManager.GetNodeName(base.PublishmentSystemID, nodeID));
                    builder.Append(",");
                }
                builder.Length--;
            }
            if (!string.IsNullOrEmpty(adInfo.NodeIDCollectionToContent))
            {
                if (builder.Length > 0)
                {
                    builder.Append("<br />");
                }
                builder.Append("内容：");
                ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(adInfo.NodeIDCollectionToContent);
                foreach (int nodeID in nodeIDArrayList)
                {
                    builder.Append(NodeManager.GetNodeName(base.PublishmentSystemID, nodeID));
                    builder.Append(",");
                }
                builder.Length--;
            }
            if (!string.IsNullOrEmpty(adInfo.FileTemplateIDCollection))
            {
                if (builder.Length > 0)
                {
                    builder.Append("<br />");
                }
                builder.Append("单页：");
                ArrayList fileTemplateIDArrayList = TranslateUtils.StringCollectionToIntArrayList(adInfo.FileTemplateIDCollection);
                foreach (int fileTemplateID in fileTemplateIDArrayList)
                {
                    builder.Append(TemplateManager.GetCreatedFileFullName(base.PublishmentSystemID, fileTemplateID));
                    builder.Append(",");
                }
                builder.Length--;
            }
            return builder.ToString();
		}

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (base.GetQueryString("Delete") != null)
            {
                string advertisementName = base.GetQueryString("AdvertisementName");
                try
                {
                    DataProvider.AdvertisementDAO.Delete(advertisementName, base.PublishmentSystemID);

                    StringUtility.AddLog(base.PublishmentSystemID, "删除漂浮广告", string.Format("广告名称：{0}", advertisementName));

                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }
            BindGrid();

			if (!Page.IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Advertisement, "漂浮广告管理", AppManager.CMS.Permission.WebSite.Advertisement);

				AdvertisementType.Items.Add(new ListItem("<所有类型>", string.Empty));
				EAdvertisementTypeUtils.AddListItems(AdvertisementType);
				ControlUtils.SelectListItems(AdvertisementType, string.Empty);

				this.Delete.Attributes.Add("onclick","return confirm(\"此操作将删除所选广告，确定吗？\");");
			}
		}

		public void BindGrid()
		{
			try
			{
                if (string.IsNullOrEmpty(this.AdvertisementType.SelectedValue))
                {
                    this.dgContents.DataSource = DataProvider.AdvertisementDAO.GetDataSource(base.PublishmentSystemID);
                }
                else
                {
                    this.dgContents.DataSource = DataProvider.AdvertisementDAO.GetDataSourceByType(EAdvertisementTypeUtils.GetEnumType(this.AdvertisementType.SelectedValue), base.PublishmentSystemID);
                }

                this.dgContents.DataBind();
			}
			catch(Exception ex)
			{
                PageUtils.RedirectToErrorPage(ex.Message);
			}
		}

		public void ReFresh(object sender, EventArgs E)
		{
			BindGrid();
		}


		public void AddAdvertisement_OnClick(object sender, EventArgs E)
		{
            PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_advertisementAdd.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
		}

		public void Delete_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				if (Request.Form["AdvertisementNameCollection"] != null)
				{
					ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.Form["AdvertisementNameCollection"]);
					try
					{
						foreach (string advertisementName in arraylist)
						{
                            DataProvider.AdvertisementDAO.Delete(advertisementName, base.PublishmentSystemID);
						}

                        StringUtility.AddLog(base.PublishmentSystemID, "删除漂浮广告", string.Format("广告名称：{0}", Request.Form["AdvertisementNameCollection"]));

						base.SuccessDeleteMessage();
					}
					catch(Exception ex)
					{
                        base.FailDeleteMessage(ex);
					}
					BindGrid();
				}
				else
				{
                    base.FailMessage("请选择广告后进行操作！");
				}
			}
		}
	}
}
