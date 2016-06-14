using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Web.UI;


namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundForm : BackgroundBasePage
    {
        public HyperLink hlAddGroup;
        public HyperLink hlAddElement;
        public HyperLink hlSetting;

        public DataGrid dgContents;
        public Repeater rptContents;

        private int mobanID;
        private int pageID;

        public void Page_Load(object sender, EventArgs E)
        {
            this.mobanID = TranslateUtils.ToInt(base.GetQueryString("MobanID"));
            this.pageID = TranslateUtils.ToInt(base.GetQueryString("PageID"));

            if (base.Request.QueryString["Delete"] != null && base.Request.QueryString["ID"] != null)
            {
                try
                {
                    DataProvider.FormGroupDAO.Delete(TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["ID"]));
                    base.SuccessMessage("成功删除组");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除组失败，{0}", ex.Message));
                }
            }
            else if ((base.Request.QueryString["Up"] != null || base.Request.QueryString["Down"] != null) && base.Request.QueryString["ID"] != null)
            {
                int id = TranslateUtils.ToInt(base.Request.QueryString["ID"]);
                bool isDown = (base.Request.QueryString["Down"] != null) ? true : false;
                if (isDown)
                {
                    DataProvider.FormGroupDAO.UpdateTaxisToUp(id, this.pageID);
                }
                else
                {
                    DataProvider.FormGroupDAO.UpdateTaxisToDown(id, this.pageID);
                }
            }
            else if (Request.QueryString["deleteFormElement"] != null)
            {
                int elementID = TranslateUtils.ToInt(Request.QueryString["formElementID"]);
                DataProvider.FormElementDAO.Delete(elementID);
            }
            else if (Request.QueryString["setTaxisFormElement"] != null)
            {
                int elementID = TranslateUtils.ToInt(Request.QueryString["formElementID"]);

                string direction = Request.QueryString["Direction"];

                switch (direction.ToUpper())
                {
                    case "UP":
                        DataProvider.FormElementDAO.TaxisDown(elementID);
                        break;
                    case "DOWN":
                        DataProvider.FormElementDAO.TaxisUp(elementID);
                        break;
                    default:
                        break;
                }
            }

            if (!IsPostBack)
            {
                this.dgContents.DataSource = DataProvider.FormElementDAO.GetFormElementInfoList(this.pageID, 0);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                this.rptContents.DataSource = DataProvider.FormGroupDAO.GetDataSource(this.pageID);
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();

                this.hlAddGroup.Attributes.Add("onclick", Modal.FormGroupAdd.GetOpenWindowStringToAdd(this.pageID));

                this.hlAddElement.Attributes.Add("onclick", Modal.FormElementAdd.GetOpenWindowStringToAdd(this.mobanID, this.pageID, 0));

                this.hlSetting.Attributes.Add("onclick", Modal.MobanSetting.GetShowPopWinString());
            }
        }

        private void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                FormElementInfo elementInfo = e.Item.DataItem as FormElementInfo;

                Literal ltlAttributeName = (Literal)e.Item.FindControl("ltlAttributeName");
                Literal ltlDataType = (Literal)e.Item.FindControl("ltlDataType");
                Literal ltlDisplayName = (Literal)e.Item.FindControl("ltlDisplayName");
                Literal ltlInputType = (Literal)e.Item.FindControl("ltlInputType");
                Literal ltlIsVisible = (Literal)e.Item.FindControl("ltlIsVisible");
                Literal ltlIsValidate = (Literal)e.Item.FindControl("ltlIsValidate");
                Literal ltlEditStyle = (Literal)e.Item.FindControl("ltlEditStyle");
                Literal ltlEditValidate = (Literal)e.Item.FindControl("ltlEditValidate");
                Literal ltlDeleteStyle = (Literal)e.Item.FindControl("ltlDeleteStyle");
                HyperLink upLinkButton = e.Item.FindControl("UpLinkButton") as HyperLink;
                HyperLink downLinkButton = e.Item.FindControl("DownLinkButton") as HyperLink;

                ltlAttributeName.Text = elementInfo.AttributeName;

                ltlDisplayName.Text = elementInfo.DisplayName;
                ltlInputType.Text = EInputTypeUtils.GetText(elementInfo.InputType);

                ltlIsVisible.Text = UserUIUtils.GetTrueOrFalseImageHtml(elementInfo.IsVisible.ToString());
                ltlIsValidate.Text = UserUIUtils.GetTrueImageHtml(elementInfo.Additional.IsValidate);

                ltlEditStyle.Text = string.Format("<a href=\"javascript:;\" onClick=\"{0}\">修改</a>", Modal.FormElementAdd.GetOpenWindowStringToEdit(elementInfo.ID, this.mobanID, elementInfo.PageID, elementInfo.GroupID));

                ltlEditValidate.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">设置</a>", Modal.FormElementValidateAdd.GetOpenWindowString(elementInfo.ID));

                ltlDeleteStyle.Text = string.Format(@"<a href=""background_form.aspx?pageID={0}&groupID={1}&deleteFormElement=True&formElementID={2}"" onClick=""javascript:return confirm('此操作将删除对应显示样式，确认吗？');"">删除</a>", elementInfo.PageID, elementInfo.GroupID, elementInfo.ID);

                upLinkButton.NavigateUrl = string.Format("background_form.aspx?pageID={0}&groupID={1}&setTaxisFormElement=True&formElementID={2}&Direction=UP", elementInfo.PageID, elementInfo.GroupID, elementInfo.ID);
                downLinkButton.NavigateUrl = string.Format("background_form.aspx?pageID={0}&groupID={1}&setTaxisFormElement=True&formElementID={2}&Direction=DOWN", elementInfo.PageID, elementInfo.GroupID, elementInfo.ID);
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                FormGroupInfo groupInfo = new FormGroupInfo(e.Item.DataItem);

                Literal ltlIndex = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlIconUrl = e.Item.FindControl("ltlIconUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlUpUrl = e.Item.FindControl("ltlUpUrl") as Literal;
                Literal ltlDownUrl = e.Item.FindControl("ltlDownUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                HyperLink hlAdd = e.Item.FindControl("hlAdd") as HyperLink;

                DataGrid dgGroupContents = e.Item.FindControl("dgGroupContents") as DataGrid;

                ltlIndex.Text = Convert.ToString(e.Item.ItemIndex + 1);
                ltlTitle.Text = groupInfo.Title;
                ltlIconUrl.Text = groupInfo.IconUrl;

                ltlUpUrl.Text = string.Format(@"<a href=""?pageID={0}&id={1}&up=true""><img src=""../../SiteFiles/bairong/icons/up.gif"" border=""0"" alt=""上升"" /></a>", this.pageID, groupInfo.ID);
                ltlDownUrl.Text = string.Format(@"<a href=""?pageID={0}&id={1}&down=true""><img src=""../../SiteFiles/bairong/icons/down.gif"" border=""0"" alt=""下降"" /></a>", this.pageID, groupInfo.ID);

                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑组</a>", Modal.FormGroupAdd.GetOpenWindowStringToEdit(this.pageID, groupInfo.ID));
                ltlDeleteUrl.Text = string.Format(@"<a href=""?pageID={0}&id={1}&delete=true"">删除组</a>", this.pageID, groupInfo.ID);

                hlAdd.Attributes.Add("onclick", Modal.FormElementAdd.GetOpenWindowStringToAdd(this.mobanID, this.pageID, groupInfo.ID));

                dgGroupContents.DataSource = DataProvider.FormElementDAO.GetFormElementInfoList(this.pageID, groupInfo.ID);
                dgGroupContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                dgGroupContents.DataBind();
            }
        }
    }
}
