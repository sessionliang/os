using System;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Web.UI;
using BaiRong.Controls;

using System.Web.UI.HtmlControls;
using SiteServer.BBS.Core;
using BaiRong.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundLink : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Button btnAdd;
        public Button btnDelete;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_link.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (Request.QueryString["SetTaxis"] != null)
            {
                int id = base.GetIntQueryString("ID");
                string direction = Request.QueryString["Direction"];

                switch (direction.ToUpper())
                {
                    case "UP":
                        DataProvider.LinkDAO.UpdateTaxisToDown(base.PublishmentSystemID, id);
                        break;
                    case "DOWN":
                        DataProvider.LinkDAO.UpdateTaxisToUp(base.PublishmentSystemID, id);
                        break;
                    default:
                        break;
                }
                base.SuccessMessage("排序成功！");
                base.AddWaitAndRedirectScript(BackgroundLink.GetRedirectUrl(base.PublishmentSystemID));
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            this.spContents.ItemsPerPage = 20;
            this.spContents.ConnectionString = DataProvider.ConnectionString;

            this.spContents.SelectCommand = DataProvider.LinkDAO.GetSelectCommend(base.PublishmentSystemID);
            this.spContents.SortField = "Taxis";
            this.spContents.SortMode = SortMode.ASC;

            btnDelete.Attributes.Add("onclick", "return checkstate('myform','删除');");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Template, "友情链接", AppManager.BBS.Permission.BBS_Template);

                spContents.DataBind();

                this.btnAdd.Attributes.Add("onclick", Modal.LinkAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));
            }
        }

        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlNum = e.Item.FindControl("ltlNum") as Literal;
                Literal ltlID = e.Item.FindControl("ltlID") as Literal;
                Literal ltlLinkName = e.Item.FindControl("ltlLinkName") as Literal;
                Literal ltlLinkUrl = e.Item.FindControl("ltlLinkUrl") as Literal;
                Literal ltlIconUrl = e.Item.FindControl("ltlIconUrl") as Literal;
                HyperLink hlUpLink = e.Item.FindControl("hlUpLink") as HyperLink;
                HyperLink hlDownLink = e.Item.FindControl("hlDownLink") as HyperLink;
                Literal ltlEdit = e.Item.FindControl("ltlEdit") as Literal;

                int id = (int)DataBinder.Eval(e.Item.DataItem, "ID");
                int ID = e.Item.ItemIndex + 1;
                string linkName = (string)DataBinder.Eval(e.Item.DataItem, "LinkName");
                string linkUrl = (string)DataBinder.Eval(e.Item.DataItem, "LinkUrl");
                string iconUrl = (string)DataBinder.Eval(e.Item.DataItem, "IconUrl");

                ltlNum.Text = ID.ToString();
                ltlID.Text = id.ToString();

                ltlLinkName.Text = linkName;
                ltlLinkUrl.Text = linkUrl;
                if (!string.IsNullOrEmpty(iconUrl))
                {
                    ltlIconUrl.Text = string.Format(@"<img src=""{0}"" />", iconUrl);
                }

                string backgroundUrl = BackgroundLink.GetRedirectUrl(base.PublishmentSystemID);

                hlUpLink.NavigateUrl = string.Format("{0}&SetTaxis=True&ID={1}&Direction=UP", backgroundUrl, id);
                hlDownLink.NavigateUrl = string.Format("{0}&SetTaxis=True&ID={1}&Direction=DOWN", backgroundUrl, id);

                ltlEdit.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.LinkAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, id));
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.rptContents.Items.Count; i++)
            {
                CheckBox cbSelect = (CheckBox)this.rptContents.Items[i].FindControl("chk_ID");
                if (cbSelect.Checked)
                {
                    Literal ltlID = (Literal)this.rptContents.Items[i].FindControl("ltlID");
                    DataProvider.LinkDAO.Delete(int.Parse(ltlID.Text));
                }
            }
            base.SuccessMessage("删除成功!");
            base.AddWaitAndRedirectScript(BackgroundLink.GetRedirectUrl(base.PublishmentSystemID));
        }
    }
}
