using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;

using BaiRong.Model;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundAdArea : BackgroundBasePage
    {
        public TextBox AdAreaName;
        public DataGrid dgContents;

        public Button AddAdArea;
        public Button Delete;
        private bool[] isLastNodeArray;
         
        public string GetIsEnabled(string isEnabledStr)
        {
            return StringUtils.GetTrueOrFalseImageHtml(isEnabledStr);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (base.GetQueryString("Delete") != null)
            {
                string adAreaName = base.GetQueryString("AdAreaName");
                try
                {
                    DataProvider.AdAreaDAO.Delete(adAreaName, base.PublishmentSystemID);

                    StringUtility.AddLog(base.PublishmentSystemID, "删除广告位", string.Format("广告名称：{0}", adAreaName ));

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
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Advertisement, "固定广告管理", AppManager.CMS.Permission.WebSite.Advertisement);
                
                this.Delete.Attributes.Add("onclick", "return confirm(\"此操作将删除所选广告位，确定吗？\");");
            }
        }

        public void BindGrid()
        {
            try
            {
                if (string.IsNullOrEmpty(this.AdAreaName.Text))
                {
                    this.dgContents.DataSource = DataProvider.AdAreaDAO.GetDataSource(base.PublishmentSystemID);
                }
                else
                {
                    this.dgContents.DataSource = DataProvider.AdAreaDAO.GetDataSourceByName(this.AdAreaName.Text, base.PublishmentSystemID);
                }
                this.dgContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        public void ReFresh(object sender, EventArgs E)
        {
            BindGrid();
        }


        public void AddAdArea_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_adAreaAdd.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
        }

        public void Delete_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (Request.Form["AdAreaNameCollection"] != null)
                {
                    ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.Form["AdAreaNameCollection"]);
                    try
                    {
                        foreach (string adAreaName in arraylist)
                        {
                            DataProvider.AdAreaDAO.Delete(adAreaName, base.PublishmentSystemID);
                        }

                        StringUtility.AddLog(base.PublishmentSystemID, "删除广告位", string.Format("广告名称：{0}", Request.Form["AdAreaNameCollection"]));

                        base.SuccessDeleteMessage();
                    }
                    catch (Exception ex)
                    {
                        base.FailDeleteMessage(ex);
                    }
                    BindGrid();
                }
                else
                {
                    base.FailMessage("请选择广告位后进行操作！");
                }
            }
        }
    }
}
