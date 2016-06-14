using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class SeoMetaAdd : BackgroundBasePage
	{
		protected TextBox SeoMetaName;
		protected TextBox PageTitle;
		protected TextBox Keywords;
		protected TextBox Description;
		protected TextBox Copyright;
		protected TextBox Author;
		protected TextBox Email;
		protected DropDownList Language;
		protected DropDownList Charset;
		protected DropDownList Distribution;
		protected DropDownList Rating;
		protected DropDownList Robots;
		protected DropDownList RevisitAfter;
		protected TextBox Expires;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("添加页面元数据", "modal_seoMetaAdd.aspx", arguments, 700, 560);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int seoMetaID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("SeoMetaID", seoMetaID.ToString());
            return PageUtility.GetOpenWindowString("修改页面元数据", "modal_seoMetaAdd.aspx", arguments, 700, 560);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, SeoMetaInfo seoMetaInfo)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("Author", seoMetaInfo.Author);
            arguments.Add("Charset", seoMetaInfo.Charset);
            arguments.Add("Copyright", seoMetaInfo.Copyright);
            arguments.Add("Description", seoMetaInfo.Description);
            arguments.Add("Distribution", seoMetaInfo.Distribution);
            arguments.Add("Email", seoMetaInfo.Email);
            arguments.Add("Expires", seoMetaInfo.Expires);
            arguments.Add("Keywords", seoMetaInfo.Keywords);
            arguments.Add("Language", seoMetaInfo.Language);
            arguments.Add("PageTitle", seoMetaInfo.PageTitle);
            arguments.Add("Rating", seoMetaInfo.Rating);
            arguments.Add("RevisitAfter", seoMetaInfo.RevisitAfter);
            arguments.Add("Robots", seoMetaInfo.Robots);
            return PageUtility.GetOpenWindowString("修改页面元数据", "modal_seoMetaAdd.aspx", arguments, 700, 560);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
				if (base.GetQueryString("SeoMetaID") != null)
				{
					int seoMetaID = TranslateUtils.ToInt(base.GetQueryString("SeoMetaID"));
					SeoMetaInfo seoMetaInfo = DataProvider.SeoMetaDAO.GetSeoMetaInfo(seoMetaID);
					if (seoMetaInfo != null)
					{
						this.SeoMetaName.Text = seoMetaInfo.SeoMetaName;
						this.SeoMetaName.Enabled = false;
						this.PageTitle.Text = seoMetaInfo.PageTitle;
						this.Keywords.Text = seoMetaInfo.Keywords;
						this.Description.Text = seoMetaInfo.Description;
						this.Copyright.Text = seoMetaInfo.Copyright;
						this.Author.Text = seoMetaInfo.Author;
						this.Email.Text = seoMetaInfo.Email;
						ControlUtils.SelectListItems(this.Language, seoMetaInfo.Language);
						ControlUtils.SelectListItems(this.Charset, seoMetaInfo.Charset);
						ControlUtils.SelectListItems(this.Distribution, seoMetaInfo.Distribution);
						ControlUtils.SelectListItems(this.Rating, seoMetaInfo.Rating);
						ControlUtils.SelectListItems(this.Robots, seoMetaInfo.Robots);
						ControlUtils.SelectListItems(this.RevisitAfter, seoMetaInfo.RevisitAfter);
						this.Expires.Text = seoMetaInfo.Expires;
					}
				}
				else
				{
					if (base.GetQueryString("PageTitle") != null) this.PageTitle.Text = base.GetQueryString("PageTitle");
					if (base.GetQueryString("Keywords") != null) this.Keywords.Text = base.GetQueryString("Keywords");
					if (base.GetQueryString("Description") != null) this.Description.Text = base.GetQueryString("Description");
					if (base.GetQueryString("Copyright") != null) this.Copyright.Text = base.GetQueryString("Copyright");
					if (base.GetQueryString("Author") != null) this.Author.Text = base.GetQueryString("Author");
					if (base.GetQueryString("Email") != null) this.Email.Text = base.GetQueryString("Email");
					if (base.GetQueryString("Language") != null) ControlUtils.SelectListItems(this.Language, base.GetQueryString("Language"));
					if (base.GetQueryString("Charset") != null) ControlUtils.SelectListItems(this.Charset, base.GetQueryString("Charset"));
					if (base.GetQueryString("Distribution") != null) ControlUtils.SelectListItems(this.Distribution, base.GetQueryString("Distribution"));
					if (base.GetQueryString("Rating") != null) ControlUtils.SelectListItems(this.Rating, base.GetQueryString("Rating"));
					if (base.GetQueryString("Robots") != null) ControlUtils.SelectListItems(this.Robots, base.GetQueryString("Robots"));
					if (base.GetQueryString("RevisitAfter") != null) ControlUtils.SelectListItems(this.RevisitAfter, base.GetQueryString("RevisitAfter"));
					if (base.GetQueryString("Expires") != null) this.Expires.Text = base.GetQueryString("Expires");
				}
				
			}
			else
			{
				this.PageTitle.Text = TrimString(this.PageTitle.Text, 80);
				this.Keywords.Text = TrimString(this.Keywords.Text, 100);
				this.Description.Text = TrimString(this.Description.Text, 200);
			}
		}

		private static string TrimString(string inputString, int count)
		{
			if (!string.IsNullOrEmpty(inputString))
			{
				if (inputString.Length > count)
				{
					inputString = inputString.Substring(0, count);
				}
			}
			return inputString;
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

			SeoMetaInfo seoMetaInfo = null;

			if (base.GetQueryString("SeoMetaID") != null)
			{
				int seoMetaID = TranslateUtils.ToInt(base.GetQueryString("SeoMetaID"));
				seoMetaInfo = DataProvider.SeoMetaDAO.GetSeoMetaInfo(seoMetaID);
			}
			else
			{
				seoMetaInfo = new SeoMetaInfo();
				seoMetaInfo.IsDefault = false;
			}

			seoMetaInfo.PublishmentSystemID = base.PublishmentSystemID;
			seoMetaInfo.SeoMetaName = this.SeoMetaName.Text;
			seoMetaInfo.PageTitle = this.PageTitle.Text;
			seoMetaInfo.Keywords = this.Keywords.Text;
			seoMetaInfo.Description = this.Description.Text;
			seoMetaInfo.Copyright = this.Copyright.Text;
			seoMetaInfo.Author = this.Author.Text;
			seoMetaInfo.Email = this.Email.Text;
			seoMetaInfo.Language = this.Language.SelectedValue;
			seoMetaInfo.Charset = this.Charset.SelectedValue;
			seoMetaInfo.Distribution = this.Distribution.SelectedValue;
			seoMetaInfo.Rating = this.Rating.SelectedValue;
			seoMetaInfo.Robots = this.Robots.SelectedValue;
			seoMetaInfo.RevisitAfter = this.RevisitAfter.SelectedValue;
			seoMetaInfo.Expires = this.Expires.Text;
				
			if (base.GetQueryString("SeoMetaID") != null)
			{
				try
				{
					DataProvider.SeoMetaDAO.Update(seoMetaInfo);
                    StringUtility.AddLog(base.PublishmentSystemID, "修改页面元数据", string.Format("元数据:{0}", seoMetaInfo.SeoMetaName));
					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, ex.Message);
				}
			}
			else
			{
                ArrayList seoMetaNameArrayList = DataProvider.SeoMetaDAO.GetSeoMetaNameArrayList(base.PublishmentSystemID);
				if (seoMetaNameArrayList.IndexOf(this.SeoMetaName.Text) != -1)
				{
                    base.FailMessage("页面元数据添加失败，名称已存在！");
				}
				else
				{
					try
					{
						DataProvider.SeoMetaDAO.Insert(seoMetaInfo);
                        StringUtility.AddLog(base.PublishmentSystemID, "添加页面元数据", string.Format("元数据:{0}", seoMetaInfo.SeoMetaName));
						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, ex.Message);
					}
				}
			}

			if (isChanged)
			{
				if (base.GetQueryString("PageTitle") == null)
				{
					JsUtils.OpenWindow.CloseModalPage(Page);
				}
				else
				{
                    JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetCMSUrl("background_seoMetaList.aspx?PublishmentSystemID=" + base.PublishmentSystemID));
				}
			}
		}
	}
}
