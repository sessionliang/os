using System;
using System.Collections;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using BaiRong.Core.Cryptography;


using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Xml;
using System.Web;
using System.IO;
using BaiRong.Core.Data;

namespace BaiRong.BackgroundPages
{
    public class Upgrade : Page
    {
        public Literal ltlVersionInfo;
        public Literal ltlStepTitle;

        public Literal ltlErrorMessage;

        public PlaceHolder phStep1;
        public CheckBox chkIAgree;

        public PlaceHolder phStep2;

        public PlaceHolder phStep3;
        public Literal ltlSuccessMessage;

        private string GetSetpTitleString(int step)
        {
            this.phStep1.Visible = this.phStep2.Visible = this.phStep3.Visible = false;
            if (step == 1)
            {
                this.phStep1.Visible = true;
            }
            else if (step == 2)
            {
                this.phStep2.Visible = true;
            }
            else if (step == 3)
            {
                this.phStep3.Visible = true;
            }

            StringBuilder builder = new StringBuilder();

            for (int i = 1; i <= 3; i++)
            {
                string liClass = string.Empty;
                if (i == step)
                {
                    liClass = @" class=""current""";
                }
                string imageUrl = string.Format("../installer/images/step{0}{1}.gif", i, (i <= step) ? "a" : "b");
                string title = string.Empty;
                if (i == 1)
                {
                    title = "许可协议";
                }
                else if (i == 2)
                {
                    title = "系统升级";
                }
                else if (i == 3)
                {
                    title = "升级完成";
                }
                builder.AppendFormat(@"<li{0}><img src=""{1}"" />{2}</li>", liClass, imageUrl, title);
            }

            return builder.ToString();
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            if (TranslateUtils.ToBool(base.Request.QueryString["done"]))
            {
                this.ltlStepTitle.Text = this.GetSetpTitleString(3);
            }
            else
            {
                if (IsNeedUpgrade(this.Page))
                {
                    if (!IsPostBack)
                    {
                        this.ltlVersionInfo.Text = string.Format("SITESERVER {0}", ProductManager.GetFullVersion());
                        this.ltlStepTitle.Text = this.GetSetpTitleString(1);
                    }
                }
            }
        }

        public bool IsNeedUpgrade(Page page)
        {
            CacheUtils.Clear();

            if (ProductManager.Version != "4.1" && BaiRongDataProvider.ConfigDAO.GetDatabaseVersion() == ProductManager.Version)
            {
                page.Response.Write(string.Format("<h2>当前版本为“{0}”,数据库版本与系统版本一致，无需升级</h2>", ProductManager.Version));
                page.Response.End();
                return false;
            }

            return true;
        }

        protected void btnStep1_Click(object sender, System.EventArgs e)
        {
            if (this.chkIAgree.Checked)
            {
                this.ltlErrorMessage.Text = string.Empty;

                string errorMessage = string.Empty;
                AppManager.Upgrade(false, ProductManager.Version, out errorMessage);
                this.ltlStepTitle.Text = this.GetSetpTitleString(2);
                this.ltlErrorMessage.Text = errorMessage;
            }
            else
            {
                this.ShowErrorMessage("您必须同意软件许可协议才能进行升级！");
            }
        }

        public string GetSiteServerUrl()
        {
            return PageUtils.GetAdminDirectoryUrl(string.Empty);
        }

        private void ShowErrorMessage(string errorMessage)
        {
            this.ltlErrorMessage.Text = string.Format(@"<img src=""../installer/images/check_error.gif"" /> {0}", errorMessage);
        }
    }
}
