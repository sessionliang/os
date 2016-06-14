using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Web.UI;
using BaiRong.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundFace : BackgroundBasePage
    {
        public DataGrid dgContents;
        public Button AddButton;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_face.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                int id = base.GetIntQueryString("ID");
                try
                {
                    DataProvider.FaceDAO.Delete(base.PublishmentSystemID, id);
                    base.SuccessMessage("成功删除表情");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除表情失败，{0}", ex.Message));
                }
            }

            if (base.GetQueryString("ID") != null && (base.GetQueryString("Up") != null || base.GetQueryString("Down") != null))
            {
                int id = base.GetIntQueryString("ID");
                bool isDown = (base.GetQueryString("Down") != null) ? true : false;
                if (isDown)
                {
                    DataProvider.FaceDAO.UpdateTaxisToDown(base.PublishmentSystemID, id);
                }
                else
                {
                    DataProvider.FaceDAO.UpdateTaxisToUp(base.PublishmentSystemID, id);
                }
                PageUtils.Redirect(BackgroundFace.GetRedirectUrl(base.PublishmentSystemID));
                return;
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Template, "表情管理", AppManager.BBS.Permission.BBS_Template);

                List<FaceInfo> list = DataProvider.FaceDAO.GetFaces(base.PublishmentSystemID);
                if (list.Count == 0)
                {
                    DataProvider.FaceDAO.CreateDefaultFace(base.PublishmentSystemID);
                    list = DataProvider.FaceDAO.GetFaces(base.PublishmentSystemID);
                }

                this.dgContents.DataSource = list;
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                this.AddButton.Attributes.Add("onclick", Modal.FaceAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));
            }
        }

        public void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                FaceInfo faceInfo = e.Item.DataItem as FaceInfo;

                Literal ltlFaceName = e.Item.FindControl("ltlFaceName") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                HyperLink hlUpLinkButton = e.Item.FindControl("hlUpLinkButton") as HyperLink;
                HyperLink hlDownLinkButton = e.Item.FindControl("hlDownLinkButton") as HyperLink;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlFaceName.Text = faceInfo.FaceName;
                ltlTitle.Text = faceInfo.Title;

                string faceUrl = BackgroundFace.GetRedirectUrl(base.PublishmentSystemID);

                hlUpLinkButton.NavigateUrl = string.Format("{0}&id={1}&up=True", faceUrl, faceInfo.FaceName);
                hlDownLinkButton.NavigateUrl = string.Format("{0}&id={1}&down=True", faceUrl, faceInfo.FaceName);

                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.FaceAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, faceInfo.ID));
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}&id={1}&Delete=True"" onClick=""javascript:return confirm('此操作将删除表情“{2}”，确认吗？');"">删除</a>", faceUrl, faceInfo.ID, faceInfo.FaceName);
            }
        }
    }
}
