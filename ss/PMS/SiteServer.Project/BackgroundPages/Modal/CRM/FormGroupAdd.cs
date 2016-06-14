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
    public class FormGroupAdd : BackgroundBasePage
    {
        protected TextBox tbTitle;
        protected TextBox tbIconUrl;

        private int pageID;
        private int id;

        public static string GetOpenWindowStringToAdd(int pageID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("pageID", pageID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加组", "modal_formGroupAdd.aspx", arguments, 400, 320);
        }

        public static string GetOpenWindowStringToEdit(int pageID, int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("pageID", pageID.ToString());
            arguments.Add("ID", id.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("修改组", "modal_formGroupAdd.aspx", arguments, 400, 320);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.pageID = TranslateUtils.ToInt(base.Request.QueryString["pageID"]);
            this.id = TranslateUtils.ToInt(base.Request.QueryString["ID"]);

            if (!IsPostBack)
            {
                if (this.id > 0)
                {
                    FormGroupInfo formGroupInfo = DataProvider.FormGroupDAO.GetFormGroupInfo(this.id);

                    if (formGroupInfo != null)
                    {
                        this.tbTitle.Text = formGroupInfo.Title;
                        this.tbIconUrl.Text = formGroupInfo.IconUrl;
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            FormGroupInfo formGroupInfo = null;

            if (this.id > 0)
            {
                try
                {
                    formGroupInfo = DataProvider.FormGroupDAO.GetFormGroupInfo(this.id);
                    if (formGroupInfo != null)
                    {
                        formGroupInfo.Title = this.tbTitle.Text;
                        formGroupInfo.IconUrl = this.tbIconUrl.Text;
                    }
                    DataProvider.FormGroupDAO.Update(formGroupInfo);

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "组修改失败！");
                }
            }
            else
            {
                try
                {
                    formGroupInfo = new FormGroupInfo();
                    formGroupInfo.PageID = this.pageID;
                    formGroupInfo.Title = this.tbTitle.Text;
                    formGroupInfo.IconUrl = this.tbIconUrl.Text;

                    DataProvider.FormGroupDAO.Insert(formGroupInfo);

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "组添加失败！");
                }
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, string.Format("background_form.aspx?PageID={0}", this.pageID));
            }
        }
    }
}
