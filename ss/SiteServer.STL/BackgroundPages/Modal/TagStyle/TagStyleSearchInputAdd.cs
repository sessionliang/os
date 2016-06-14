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
	public class TagStyleSearchInputAdd : BackgroundBasePage
	{
        protected TextBox StyleName;

        protected TextBox SearchUrl;
        protected TextBox InputWidth;
        protected RadioButtonList OpenWin;
        protected RadioButtonList IsType;
        protected RadioButtonList IsChannel;
        protected RadioButtonList IsChannelRadio;
        protected RadioButtonList IsDate;
        protected RadioButtonList IsDateFrom;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                if (base.GetQueryString("StyleID") != null)
                {
                    int styleID = TranslateUtils.ToInt(base.GetQueryString("StyleID"));
                    TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(styleID);
                    if (styleInfo != null)
                    {
                        TagStyleSearchInputInfo inputInfo = new TagStyleSearchInputInfo(styleInfo.SettingsXML);
                        this.StyleName.Text = styleInfo.StyleName;

                        this.SearchUrl.Text = inputInfo.SearchUrl;
                        this.InputWidth.Text = inputInfo.InputWidth;
                        ControlUtils.SelectListItems(this.OpenWin, inputInfo.OpenWin.ToString());
                        ControlUtils.SelectListItems(this.IsType, inputInfo.IsType.ToString());
                        ControlUtils.SelectListItems(this.IsChannel, inputInfo.IsChannel.ToString());
                        ControlUtils.SelectListItems(this.IsChannelRadio, inputInfo.IsChannelRadio.ToString());
                        ControlUtils.SelectListItems(this.IsDate, inputInfo.IsDate.ToString());
                        ControlUtils.SelectListItems(this.IsDateFrom, inputInfo.IsDateFrom.ToString());
                    }
                }
                else
                {
                    TagStyleSearchInputInfo inputInfo = new TagStyleSearchInputInfo(string.Empty);

                    this.SearchUrl.Text = inputInfo.SearchUrl;
                    this.InputWidth.Text = inputInfo.InputWidth;
                    ControlUtils.SelectListItems(this.OpenWin, inputInfo.OpenWin.ToString());
                    ControlUtils.SelectListItems(this.IsType, inputInfo.IsType.ToString());
                    ControlUtils.SelectListItems(this.IsChannel, inputInfo.IsChannel.ToString());
                    ControlUtils.SelectListItems(this.IsChannelRadio, inputInfo.IsChannelRadio.ToString());
                    ControlUtils.SelectListItems(this.IsDate, inputInfo.IsDate.ToString());
                    ControlUtils.SelectListItems(this.IsDateFrom, inputInfo.IsDateFrom.ToString());
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
                        TagStyleSearchInputInfo inputInfo = new TagStyleSearchInputInfo(styleInfo.SettingsXML);

                        styleInfo.StyleName = this.StyleName.Text;

                        inputInfo.SearchUrl = this.SearchUrl.Text;
                        inputInfo.InputWidth = this.InputWidth.Text;
                        inputInfo.OpenWin = TranslateUtils.ToBool(this.OpenWin.SelectedValue);
                        inputInfo.IsChannel = TranslateUtils.ToBool(this.IsChannel.SelectedValue);
                        inputInfo.IsType = TranslateUtils.ToBool(this.IsType.SelectedValue);
                        inputInfo.IsChannelRadio = TranslateUtils.ToBool(this.IsChannelRadio.SelectedValue);
                        inputInfo.IsDate = TranslateUtils.ToBool(this.IsDate.SelectedValue);
                        inputInfo.IsDateFrom = TranslateUtils.ToBool(this.IsDateFrom.SelectedValue);

                        styleInfo.SettingsXML = inputInfo.ToString();
                    }
                    DataProvider.TagStyleDAO.Update(styleInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改搜索框样式", string.Format("样式名称:{0}", styleInfo.StyleName));

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "搜索框样式修改失败！");
				}
			}
			else
			{
                ArrayList styleNameArrayList = DataProvider.TagStyleDAO.GetStyleNameArrayList(base.PublishmentSystemID, StlSearchInput.ElementName);
                if (styleNameArrayList.IndexOf(this.StyleName.Text) != -1)
				{
                    base.FailMessage("搜索框样式添加失败，搜索框样式名称已存在！");
				}
				else
				{
					try
					{
                        TagStyleInfo styleInfo = new TagStyleInfo();
                        TagStyleSearchInputInfo inputInfo = new TagStyleSearchInputInfo(string.Empty);

                        styleInfo.StyleName = this.StyleName.Text;
                        styleInfo.ElementName = StlSearchInput.ElementName;
                        styleInfo.PublishmentSystemID = base.PublishmentSystemID;

                        inputInfo.SearchUrl = this.SearchUrl.Text;
                        inputInfo.InputWidth = this.InputWidth.Text;
                        inputInfo.OpenWin = TranslateUtils.ToBool(this.OpenWin.SelectedValue);
                        inputInfo.IsType = TranslateUtils.ToBool(this.IsType.SelectedValue);
                        inputInfo.IsChannel = TranslateUtils.ToBool(this.IsChannel.SelectedValue);
                        inputInfo.IsChannelRadio = TranslateUtils.ToBool(this.IsChannelRadio.SelectedValue);
                        inputInfo.IsDate = TranslateUtils.ToBool(this.IsDate.SelectedValue);
                        inputInfo.IsDateFrom = TranslateUtils.ToBool(this.IsDateFrom.SelectedValue);

                        styleInfo.SettingsXML = inputInfo.ToString();

                        DataProvider.TagStyleDAO.Insert(styleInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "添加搜索框样式", string.Format("样式名称:{0}", styleInfo.StyleName));

						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "搜索框样式添加失败！");
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
