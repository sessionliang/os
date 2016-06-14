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

namespace SiteServer.B2C.BackgroundPages
{
	public class BackgroundSpecItem : BackgroundBasePage
	{
        public DataGrid dgContents;
        public Button btnAdd;
        public Button btnReturn;

        private int channelID;
        private SpecInfo specInfo;

        public static string GetRedirectUrl(int publishmentSystemID, int channelID, int specID)
        {
            return PageUtils.GetB2CUrl(string.Format("background_specItem.aspx?PublishmentSystemID={0}&channelID={1}&specID={2}", publishmentSystemID, channelID, specID));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "channelID", "specID");

            this.channelID = base.GetIntQueryString("channelID");

            base.BreadCrumb(AppManager.B2C.LeftMenu.ID_Content, "���ֵ����", string.Empty);

            int specID = base.GetIntQueryString("SpecID");
            this.specInfo = SpecManager.GetSpecInfo(base.PublishmentSystemID, specID);


            if (base.GetQueryString("ItemID") != null && (base.GetQueryString("Up") != null || base.GetQueryString("Down") != null))
            {
                int itemID = base.GetIntQueryString("ItemID");
                bool isDown = (base.GetQueryString("Down") != null) ? true : false;
                if (isDown)
                {
                    DataProviderB2C.SpecItemDAO.UpdateTaxisToDown(specID, itemID);
                }
                else
                {
                    DataProviderB2C.SpecItemDAO.UpdateTaxisToUp(specID, itemID);
                }

                StringUtility.AddLog(base.PublishmentSystemID, "���ֵ����" + (isDown ? "�½�" : "����"));

                PageUtils.Redirect(BackgroundSpecItem.GetRedirectUrl(base.PublishmentSystemID, this.channelID, this.specInfo.SpecID));
                return;
            }

			if(!IsPostBack)
			{
                if (base.GetQueryString("Delete") != null)
                {
                    int itemID = base.GetIntQueryString("ItemID");
                    try
                    {
                        SpecItemInfo itemInfo = SpecItemManager.GetSpecItemInfo(base.PublishmentSystemID, itemID);
                        if (itemInfo != null)
                        {
                            DataProviderB2C.SpecItemDAO.Delete(base.PublishmentSystemID, itemID);
                            StringUtility.AddLog(base.PublishmentSystemID, "ɾ�����ֵ", string.Format("���ֵ:{0}", itemInfo.Title));
                        }
                        
                        base.SuccessMessage("ɾ���ɹ���");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "ɾ��ʧ�ܣ�");
                    }
                }

                this.btnAdd.Attributes.Add("onclick", Modal.SpecItemAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.channelID, specID));

                string urlReturn = BackgroundSpec.GetRedirectUrl(base.PublishmentSystemID, this.channelID);
                this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", urlReturn));

                if (!this.specInfo.IsIcon)
                {
                    this.dgContents.Columns[2].Visible = false;
                }
                this.dgContents.DataSource = DataProviderB2C.SpecItemDAO.GetDataSource(this.specInfo.SpecID);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
			}
		}

        private void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int itemID = TranslateUtils.EvalInt(e.Item.DataItem, "ItemID");
                string title = TranslateUtils.EvalString(e.Item.DataItem, "Title");
                string iconUrl = TranslateUtils.EvalString(e.Item.DataItem, "IconUrl");

                Literal ltlIconUrl = (Literal)e.Item.FindControl("ltlIconUrl");
                Literal upLink = (Literal)e.Item.FindControl("UpLink");
                Literal downLink = (Literal)e.Item.FindControl("DownLink");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");
                Literal ltlDeleteUrl = (Literal)e.Item.FindControl("ltlDeleteUrl");

                if (!string.IsNullOrEmpty(iconUrl))
                {
                    ltlIconUrl.Text = string.Format(@"<img src=""{0}"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, iconUrl));
                }

                string url = BackgroundSpecItem.GetRedirectUrl(base.PublishmentSystemID, this.channelID, this.specInfo.SpecID);

                string urlUp = url + string.Format("&ItemID={0}&Up=True", itemID);
                upLink.Text = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""����"" /></a>", urlUp);

                string urlDown = url + string.Format("ItemID={0}&Down=True", itemID);
                downLink.Text = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""�½�"" /></a>", urlDown);

                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">�༭</a>", Modal.SpecItemAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, this.channelID, this.specInfo.SpecID, itemID));

                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}&Delete=True&ItemID={1}"" onClick=""javascript:return confirm('�˲�����ɾ�����ֵ��{2}����������ݣ�ȷ����');"">ɾ��</a>", url, itemID, title);
            }
        }
	}
}
