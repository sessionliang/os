using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using SiteServer.Project.Model;
using System.Text;
using BaiRong.Model;

namespace SiteServer.Project.BackgroundPages.Modal
{
    public class FormPageAdd : BackgroundBasePage
    {
        protected TextBox tbTitle;

        private int mobanID;
        private int id;

        public static string GetOpenWindowStringToAdd(int mobanID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("MobanID", mobanID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加分页", "modal_formPageAdd.aspx", arguments, 330, 260);
        }

        public static string GetOpenWindowStringToEdit(int mobanID, int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("MobanID", mobanID.ToString());
            arguments.Add("ID", id.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("修改分页", "modal_formPageAdd.aspx", arguments, 330, 260);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.mobanID = TranslateUtils.ToInt(base.Request.QueryString["MobanID"]);
            this.id = TranslateUtils.ToInt(base.Request.QueryString["ID"]);

            if (!IsPostBack)
            {
                if (this.id > 0)
                {
                    FormPageInfo formPageInfo = DataProvider.FormPageDAO.GetFormPageInfo(this.id);

                    if (formPageInfo != null)
                    {
                        this.tbTitle.Text = formPageInfo.Title;
                    }
                }


            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            FormPageInfo formPageInfo = null;

            if (this.id > 0)
            {
                try
                {
                    formPageInfo = DataProvider.FormPageDAO.GetFormPageInfo(this.id);
                    if (formPageInfo != null)
                    {
                        formPageInfo.Title = this.tbTitle.Text;
                    }
                    DataProvider.FormPageDAO.Update(formPageInfo);

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "分页修改失败！");
                }
            }
            else
            {
                try
                {
                    formPageInfo = new FormPageInfo();
                    formPageInfo.MobanID = this.mobanID;
                    formPageInfo.Title = this.tbTitle.Text;

                    DataProvider.FormPageDAO.Insert(formPageInfo);

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "分页添加失败！");
                }
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, string.Format("background_formPage.aspx?MobanID={0}", this.mobanID));
            }
        }
    }
}
