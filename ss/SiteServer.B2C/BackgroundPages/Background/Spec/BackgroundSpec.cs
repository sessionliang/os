using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Web.UI;
using BaiRong.Model;

using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;
using System.Collections.Generic;

namespace SiteServer.B2C.BackgroundPages
{
	public class BackgroundSpec : BackgroundBasePage
	{
        public DataGrid dgContents;
        public Button btnAdd;
        public Button btnSync;

        private int channelID;

        public static string GetRedirectUrl(int publishmentSystemID, int channelID)
        {
            return PageUtils.GetB2CUrl(string.Format("background_spec.aspx?PublishmentSystemID={0}&channelID={1}", publishmentSystemID, channelID));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.channelID = base.GetIntQueryString("channelID");

            base.BreadCrumb(AppManager.B2C.LeftMenu.ID_Content, "商品规格管理", string.Empty);

			if(!IsPostBack)
			{
                if (base.GetQueryString("Delete") != null)
                {
                    int specID = base.GetIntQueryString("specID");
                    try
                    {
                        SpecInfo specInfo = SpecManager.GetSpecInfo(base.PublishmentSystemID, specID);
                        if (specInfo != null)
                        {
                            DataProviderB2C.SpecDAO.Delete(base.PublishmentSystemID, this.channelID, specID);
                            StringUtility.AddLog(base.PublishmentSystemID, "删除规格", string.Format("规格:{0}", specInfo.SpecName));
                        }
                        
                        base.SuccessMessage("删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
                }
                else if ((base.GetQueryString("Up") != null || base.GetQueryString("Down") != null) && base.GetQueryString("specID") != null)
                {
                    int specID = base.GetIntQueryString("specID");
                    bool isDown = (base.GetQueryString("Down") != null) ? true : false;
                    if (isDown)
                    {
                        DataProviderB2C.SpecDAO.UpdateTaxisToUp(base.PublishmentSystemID, this.channelID, specID);
                    }
                    else
                    {
                        DataProviderB2C.SpecDAO.UpdateTaxisToDown(base.PublishmentSystemID, this.channelID, specID);
                    }
                }

                this.btnAdd.Attributes.Add("onclick", Modal.SpecAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.channelID));

                this.btnSync.Attributes.Add("onclick", "javascript:return confirm('此操作将删除子栏目的规格并同步本栏目规格到所有子栏目，确认吗？');");

                this.dgContents.DataSource = DataProviderB2C.SpecDAO.GetDataSource(base.PublishmentSystemID, this.channelID);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
			}
		}

        private void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int specID = TranslateUtils.EvalInt(e.Item.DataItem, "SpecID");
                string specName = TranslateUtils.EvalString(e.Item.DataItem, "SpecName");
                bool isIcon = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsIcon"));
                bool isMultiple = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsMultiple"));
                bool isRequired = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsRequired"));

                Literal ltlIsMultiple = (Literal)e.Item.FindControl("ltlIsMultiple");
                Literal ltlIsRequired = (Literal)e.Item.FindControl("ltlIsRequired");
                Literal ltlItem = (Literal)e.Item.FindControl("ltlItem");
                HyperLink hlUpLinkButton = e.Item.FindControl("hlUpLinkButton") as HyperLink;
                HyperLink hlDownLinkButton = e.Item.FindControl("hlDownLinkButton") as HyperLink;
                Literal ltlItemUrl = (Literal)e.Item.FindControl("ltlItemUrl");
                Literal ltlDefaultUrl = (Literal)e.Item.FindControl("ltlDefaultUrl");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");
                Literal ltlDeleteUrl = (Literal)e.Item.FindControl("ltlDeleteUrl");

                ltlIsMultiple.Text = isMultiple ? "多选" : "单选";
                ltlIsRequired.Text = isRequired ? "必选项" : "可选项";
                ltlItem.Text = SpecManager.GetSpecValues(base.PublishmentSystemInfo, specID);

                string urlSpec = BackgroundSpec.GetRedirectUrl(base.PublishmentSystemID, this.channelID);

                hlUpLinkButton.NavigateUrl = urlSpec + string.Format("&specID={0}&Up=True", specID);
                hlDownLinkButton.NavigateUrl = urlSpec + string.Format("&specID={0}&Down=True", specID);

                string urlSpecItem = BackgroundSpecItem.GetRedirectUrl(base.PublishmentSystemID, this.channelID, specID);
                ltlItemUrl.Text = string.Format(@"<a href=""{0}"">设置规格值</a>", urlSpecItem);

                ltlDefaultUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">设置默认项</a>", Modal.SpecDefault.GetOpenWindowString(base.PublishmentSystemID, specID));

                ltlEditUrl.Text = string.Format(@"<a href=""javascript:undefined;"" onclick=""{0}"">编辑</a>", Modal.SpecAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, this.channelID, specID));

                string urlDelete = urlSpec + string.Format("&specID={0}&Delete=True", specID);
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除规格“{1}”及相关数据，确认吗？');"">删除</a>", urlDelete, specName);
            }
        }

        public void Sync_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(this.channelID);
                    List<SpecInfo> specInfoList = DataProviderB2C.SpecDAO.GetSpecInfoList(base.PublishmentSystemID, this.channelID);

                    Dictionary<int, List<SpecItemInfo>> dictionary = new Dictionary<int, List<SpecItemInfo>>();
                    foreach (SpecInfo specInfo in specInfoList)
                    {
                        List<SpecItemInfo> itemInfoList = DataProviderB2C.SpecItemDAO.GetSpecItemInfoList(specInfo.SpecID);
                        dictionary.Add(specInfo.SpecID, itemInfoList);
                    }

                    foreach (int nodeID in nodeIDArrayList)
                    {
                        DataProviderB2C.SpecDAO.DeleteAll(base.PublishmentSystemID, nodeID);

                        foreach (SpecInfo specInfo in specInfoList)
                        {
                            specInfo.ChannelID = nodeID;
                            int specID = DataProviderB2C.SpecDAO.Insert(specInfo);

                            List<SpecItemInfo> itemInfoList = dictionary[specInfo.SpecID];
                            foreach (SpecItemInfo itemInfo in itemInfoList)
                            {
                                itemInfo.SpecID = specID;
                                DataProviderB2C.SpecItemDAO.Insert(base.PublishmentSystemID, itemInfo);
                            }
                        }
                    }

                    base.SuccessMessage("设置同步成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "设置同步失败！");
                }
            }
        }
	}
}
