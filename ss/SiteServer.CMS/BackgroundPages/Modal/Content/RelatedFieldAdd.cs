using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class RelatedFieldAdd : BackgroundBasePage
	{
        protected TextBox RelatedFieldName;
        protected DropDownList TotalLevel;

        protected TextBox Prefix1;
        protected TextBox Suffix1;
        protected PlaceHolder phFix2;
        protected TextBox Prefix2;
        protected TextBox Suffix2;
        protected PlaceHolder phFix3;
        protected TextBox Prefix3;
        protected TextBox Suffix3;
        protected PlaceHolder phFix4;
        protected TextBox Prefix4;
        protected TextBox Suffix4;
        protected PlaceHolder phFix5;
        protected TextBox Prefix5;
        protected TextBox Suffix5;

        public static string GetOpenWindowString(int publishmentSystemID, int relatedFieldID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedFieldID", relatedFieldID.ToString());
            return PageUtility.GetOpenWindowString("修改联动字段", "modal_relatedFieldAdd.aspx", arguments, 420, 450);
        }

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("添加联动字段", "modal_relatedFieldAdd.aspx", arguments, 420, 450);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                if (base.GetQueryString("RelatedFieldID") != null)
				{
                    int relatedFieldID = TranslateUtils.ToInt(base.GetQueryString("RelatedFieldID"));
                    RelatedFieldInfo relatedFieldInfo = DataProvider.RelatedFieldDAO.GetRelatedFieldInfo(relatedFieldID);
                    if (relatedFieldInfo != null)
					{
                        RelatedFieldName.Text = relatedFieldInfo.RelatedFieldName;
                        ControlUtils.SelectListItemsIgnoreCase(this.TotalLevel, relatedFieldInfo.TotalLevel.ToString());

                        if (!string.IsNullOrEmpty(relatedFieldInfo.Prefixes))
                        {
                            StringCollection collection = TranslateUtils.StringCollectionToStringCollection(relatedFieldInfo.Prefixes);
                            this.Prefix1.Text = collection[0];
                            this.Prefix2.Text = collection[1];
                            this.Prefix3.Text = collection[2];
                            this.Prefix4.Text = collection[3];
                            this.Prefix5.Text = collection[4];
                        }
                        if (!string.IsNullOrEmpty(relatedFieldInfo.Suffixes))
                        {
                            StringCollection collection = TranslateUtils.StringCollectionToStringCollection(relatedFieldInfo.Suffixes);
                            this.Suffix1.Text = collection[0];
                            this.Suffix2.Text = collection[1];
                            this.Suffix3.Text = collection[2];
                            this.Suffix4.Text = collection[3];
                            this.Suffix5.Text = collection[4];
                        }
					}
				}
                this.TotalLevel_SelectedIndexChanged(null, EventArgs.Empty);
				
			}
		}

        public void TotalLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phFix2.Visible = this.phFix3.Visible = this.phFix4.Visible = this.phFix5.Visible = false;

            int totalLevel = TranslateUtils.ToInt(this.TotalLevel.SelectedValue);
            if (totalLevel >= 2)
            {
                this.phFix2.Visible = true;
            }
            if (totalLevel >= 3)
            {
                this.phFix3.Visible = true;
            }
            if (totalLevel >= 4)
            {
                this.phFix4.Visible = true;
            }
            if (totalLevel >= 5)
            {
                this.phFix5.Visible = true;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

            RelatedFieldInfo relatedFieldInfo = new RelatedFieldInfo();
            relatedFieldInfo.RelatedFieldName = RelatedFieldName.Text;
            relatedFieldInfo.PublishmentSystemID = base.PublishmentSystemID;
            relatedFieldInfo.TotalLevel = TranslateUtils.ToInt(TotalLevel.SelectedValue);
            ArrayList prefix = new ArrayList();
            prefix.Add(this.Prefix1.Text);
            prefix.Add(this.Prefix2.Text);
            prefix.Add(this.Prefix3.Text);
            prefix.Add(this.Prefix4.Text);
            prefix.Add(this.Prefix5.Text);
            relatedFieldInfo.Prefixes = TranslateUtils.ObjectCollectionToString(prefix);
            ArrayList suffix = new ArrayList();
            suffix.Add(this.Suffix1.Text);
            suffix.Add(this.Suffix2.Text);
            suffix.Add(this.Suffix3.Text);
            suffix.Add(this.Suffix4.Text);
            suffix.Add(this.Suffix5.Text);
            relatedFieldInfo.Suffixes = TranslateUtils.ObjectCollectionToString(suffix);
				
			if (base.GetQueryString("RelatedFieldID") != null)
			{
				try
				{
                    relatedFieldInfo.RelatedFieldID = TranslateUtils.ToInt(base.GetQueryString("RelatedFieldID"));
                    DataProvider.RelatedFieldDAO.Update(relatedFieldInfo);
                    StringUtility.AddLog(base.PublishmentSystemID, "修改联动字段", string.Format("联动字段:{0}", relatedFieldInfo.RelatedFieldName));
					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "联动字段修改失败！");
				}
			}
			else
			{
                ArrayList relatedFieldNameArrayList = DataProvider.RelatedFieldDAO.GetRelatedFieldNameArrayList(base.PublishmentSystemID);
                if (relatedFieldNameArrayList.IndexOf(this.RelatedFieldName.Text) != -1)
				{
                    base.FailMessage("联动字段添加失败，联动字段名称已存在！");
				}
				else
				{
					try
					{
                        DataProvider.RelatedFieldDAO.Insert(relatedFieldInfo);
                        StringUtility.AddLog(base.PublishmentSystemID, "添加联动字段", string.Format("联动字段:{0}", relatedFieldInfo.RelatedFieldName));
						isChanged = true;
					}
					catch(Exception ex)
					{
						base.FailMessage(ex, "联动字段添加失败！");
					}
				}
			}

			if (isChanged)
			{
				JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
