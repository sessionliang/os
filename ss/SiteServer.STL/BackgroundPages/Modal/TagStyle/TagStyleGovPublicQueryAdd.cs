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
	public class TagStyleGovPublicQueryAdd : BackgroundBasePage
	{
        protected TextBox StyleName;

		public void Page_Load(object sender, EventArgs e)
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
                        this.StyleName.Text = styleInfo.StyleName;
                    }
                }
				
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
            TagStyleInfo styleInfo = null;
				
			if (base.GetQueryString("StyleID") != null)
			{
				try
				{
                    int styleID = TranslateUtils.ToInt(base.GetQueryString("StyleID"));
                    styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(styleID);
                    if (styleInfo != null)
                    {
                        styleInfo.StyleName = this.StyleName.Text;
                    }
                    DataProvider.TagStyleDAO.Update(styleInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改依申请公开查询样式", string.Format("样式名称:{0}", styleInfo.StyleName));

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "依申请公开查询样式修改失败！");
				}
			}
			else
			{
                ArrayList styleNameArrayList = DataProvider.TagStyleDAO.GetStyleNameArrayList(base.PublishmentSystemID, StlGovPublicQuery.ElementName);
                if (styleNameArrayList.IndexOf(this.StyleName.Text) != -1)
				{
                    base.FailMessage("依申请公开查询样式添加失败，依申请公开查询样式名称已存在！");
				}
				else
				{
					try
					{
                        styleInfo = new TagStyleInfo();

                        styleInfo.StyleName = this.StyleName.Text;
                        styleInfo.ElementName = StlGovPublicQuery.ElementName;
                        styleInfo.PublishmentSystemID = base.PublishmentSystemID;

                        DataProvider.TagStyleDAO.Insert(styleInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "添加依申请公开查询样式", string.Format("样式名称:{0}", styleInfo.StyleName));

						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "依申请公开查询样式添加失败！");
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
