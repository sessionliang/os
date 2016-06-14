using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Web.UI;
using SiteServer.CMS.Model;

namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovPublicCategoryClass : BackgroundGovPublicBasePage
    {
        public DataGrid dgContents;
        public Button AddButton;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.Request.QueryString["Delete"] != null && base.Request.QueryString["ClassCode"] != null)
            {
                string classCode = base.Request.QueryString["ClassCode"];
                try
                {
                    DataProvider.GovPublicCategoryClassDAO.Delete(classCode, base.PublishmentSystemID);
                    base.SuccessMessage("成功删除分类法");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除分类法失败，{0}", ex.Message));
                }
            }
            else if ((base.Request.QueryString["Up"] != null || base.Request.QueryString["Down"] != null) && base.Request.QueryString["ClassCode"] != null)
            {
                string classCode = base.Request.QueryString["ClassCode"];
                bool isDown = (base.Request.QueryString["Down"] != null) ? true : false;
                if (isDown)
                {
                    DataProvider.GovPublicCategoryClassDAO.UpdateTaxisToUp(classCode, base.PublishmentSystemID);
                }
                else
                {
                    DataProvider.GovPublicCategoryClassDAO.UpdateTaxisToDown(classCode, base.PublishmentSystemID);
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicContentConfiguration, "分类法管理", AppManager.CMS.Permission.WebSite.GovPublicContentConfiguration);

                BindGrid();

                this.AddButton.Attributes.Add("onclick", Modal.GovPublicCategoryClassAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));
            }
        }

        public void BindGrid()
        {
            this.dgContents.DataSource = DataProvider.GovPublicCategoryClassDAO.GetDataSource(base.PublishmentSystemID);
            this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
            this.dgContents.DataBind();
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string classCode = TranslateUtils.EvalString(e.Item.DataItem, "ClassCode");
                string className = TranslateUtils.EvalString(e.Item.DataItem, "ClassName");
                bool isSystem = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsSystem"));
                bool isEnabled = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsEnabled"));

                Literal ltlClassName = e.Item.FindControl("ltlClassName") as Literal;
                Literal ltlClassCode = e.Item.FindControl("ltlClassCode") as Literal;
                HyperLink hlUpLinkButton = e.Item.FindControl("hlUpLinkButton") as HyperLink;
                HyperLink hlDownLinkButton = e.Item.FindControl("hlDownLinkButton") as HyperLink;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                if (!isSystem)
                {
                    ltlClassName.Text = string.Format(@"<a href=""{0}"" target=""category"">{1}分类</a>", PageUtils.GetWCMUrl(string.Format(@"background_govPublicCategory.aspx?PublishmentSystemID={0}&ClassCode={1}", base.PublishmentSystemID, classCode)), className);
                }
                else if (EGovPublicCategoryClassTypeUtils.Equals(EGovPublicCategoryClassType.Channel, classCode))
                {
                    ltlClassName.Text = string.Format(@"<a href=""{0}"" target=""category"">{1}分类</a>", PageUtils.GetWCMUrl(string.Format(@"background_govPublicChannel.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)), className);
                }
                else if (EGovPublicCategoryClassTypeUtils.Equals(EGovPublicCategoryClassType.Department, classCode))
                {
                    ltlClassName.Text = string.Format(@"<a href=""{0}"" target=""category"">{1}分类</a>", PageUtils.GetPlatformUrl(string.Format(@"background_department.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)), className);
                }
                
                ltlClassCode.Text = classCode;
                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(isEnabled);

                hlUpLinkButton.NavigateUrl = PageUtils.GetWCMUrl(string.Format("background_govPublicCategoryClass.aspx?PublishmentSystemID={0}&ClassCode={1}&Up=True", base.PublishmentSystemID, classCode));

                hlDownLinkButton.NavigateUrl = PageUtils.GetWCMUrl(string.Format("background_govPublicCategoryClass.aspx?PublishmentSystemID={0}&ClassCode={1}&Down=True", base.PublishmentSystemID, classCode));

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.GovPublicCategoryClassAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, classCode));

                if (!isSystem)
                {
                    string urlDelete = PageUtils.GetWCMUrl(string.Format("background_govPublicCategoryClass.aspx?PublishmentSystemID={0}&Delete=True&ClassCode={1}", base.PublishmentSystemID, classCode));
                    ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除分类法“{1}”及其分类项，确认吗？');"">删除</a>", urlDelete, className);
                }
            }
        }
    }
}
