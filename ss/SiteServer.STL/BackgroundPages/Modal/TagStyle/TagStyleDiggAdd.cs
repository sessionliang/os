using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal
{
	public class TagStyleDiggAdd : BackgroundBasePage
	{
        protected TextBox StyleName;

        protected RadioButtonList DiggType;
        protected TextBox GoodText;
        protected TextBox BadText;
        protected DropDownList ddlTheme;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                EDiggTypeUtils.AddListItems(this.DiggType);

                for (int i = 1; i <= 4; i++)
                {
                    ListItem listItem = new ListItem("��ʽ" + i, i.ToString());
                    this.ddlTheme.Items.Add(listItem);
                }

                if (base.GetQueryString("StyleID") != null)
                {
                    int styleID = TranslateUtils.ToInt(base.GetQueryString("StyleID"));
                    TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(styleID);
                    if (styleInfo != null)
                    {
                        TagStyleDiggInfo diggInfo = new TagStyleDiggInfo(styleInfo.SettingsXML);
                        this.StyleName.Text = styleInfo.StyleName;
                        ControlUtils.SelectListItems(this.DiggType, EDiggTypeUtils.GetValue(diggInfo.DiggType));
                        this.GoodText.Text = diggInfo.GoodText;
                        this.BadText.Text = diggInfo.BadText;
                        ControlUtils.SelectListItems(this.ddlTheme, diggInfo.Theme);
                    }
                }
                else
                {
                    TagStyleDiggInfo diggInfo = new TagStyleDiggInfo(string.Empty);
                    ControlUtils.SelectListItems(this.DiggType, EDiggTypeUtils.GetValue(diggInfo.DiggType));
                    this.GoodText.Text = diggInfo.GoodText;
                    this.BadText.Text = diggInfo.BadText;
                    ControlUtils.SelectListItems(this.ddlTheme, diggInfo.Theme);
                }
				
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
			if (base.GetQueryString("StyleID") != null)
			{
				try
				{
                    int styleID = TranslateUtils.ToInt(base.GetQueryString("StyleID"));
                    TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(styleID);
                    if (styleInfo != null)
                    {
                        TagStyleDiggInfo diggInfo = new TagStyleDiggInfo(styleInfo.SettingsXML);

                        styleInfo.StyleName = this.StyleName.Text;

                        diggInfo.DiggType = EDiggTypeUtils.GetEnumType(this.DiggType.SelectedValue);
                        diggInfo.GoodText = this.GoodText.Text;
                        diggInfo.BadText = this.BadText.Text;
                        diggInfo.Theme = this.ddlTheme.SelectedValue;

                        styleInfo.SettingsXML = diggInfo.ToString();
                    }
                    DataProvider.TagStyleDAO.Update(styleInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "�޸ľ��(digg)��ʽ", string.Format("��ʽ����:{0}", styleInfo.StyleName));

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "���(digg)��ʽ�޸�ʧ�ܣ�");
				}
			}
			else
			{
                ArrayList styleNameArrayList = DataProvider.TagStyleDAO.GetStyleNameArrayList(base.PublishmentSystemID, StlDigg.ElementName);
                if (styleNameArrayList.IndexOf(this.StyleName.Text) != -1)
				{
                    base.FailMessage("���(digg)��ʽ���ʧ�ܣ���ʽ�����Ѵ��ڣ�");
				}
				else
				{
					try
					{
                        TagStyleInfo styleInfo = new TagStyleInfo();
                        TagStyleDiggInfo diggInfo = new TagStyleDiggInfo(string.Empty);

                        styleInfo.StyleName = this.StyleName.Text;
                        styleInfo.ElementName = StlDigg.ElementName;
                        styleInfo.PublishmentSystemID = base.PublishmentSystemID;

                        diggInfo.DiggType = EDiggTypeUtils.GetEnumType(this.DiggType.SelectedValue);
                        diggInfo.GoodText = this.GoodText.Text;
                        diggInfo.BadText = this.BadText.Text;
                        diggInfo.Theme = this.ddlTheme.SelectedValue;

                        styleInfo.SettingsXML = diggInfo.ToString();

                        DataProvider.TagStyleDAO.Insert(styleInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "��Ӿ��(digg)��ʽ", string.Format("��ʽ����:{0}", styleInfo.StyleName));

						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "���(digg)��ʽ���ʧ�ܣ�");
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
