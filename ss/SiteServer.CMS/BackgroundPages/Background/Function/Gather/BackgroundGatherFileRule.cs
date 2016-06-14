using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundGatherFileRule : BackgroundBasePage
	{
		public DataGrid dgContents;
        public Button Start;

        public string GetGatherUrl(string gatherUrl)
		{
            return string.Format("<a href=\"{0}\" target='_blank' title=\"{0}\">{1}</a>", PageUtils.AddProtocolToUrl(gatherUrl), StringUtils.MaxLengthText(gatherUrl, 25));
		}

        public string GetEditLink(string gatherRuleName)
		{
            string urlEdit = PageUtils.GetCMSUrl(string.Format("background_gatherFileRuleAdd.aspx?PublishmentSystemID={0}&GatherRuleName={1}", base.PublishmentSystemID, gatherRuleName));
            return string.Format("<a href=\"{0}\">编辑</a>", urlEdit);
		}

        public string GetAutoCreateClickString()
        {
            return JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("background_gatherFileRule.aspx?PublishmentSystemID={0}&Auto=True", base.PublishmentSystemID), "GatherFileRuleNameCollection", "GatherFileRuleNameCollection", "请选择需要打开自动生成的规则！", "确认要设置所选规则为自动生成吗？");
        }

        public string GetNoAutoCreateClickString()
        {
            return JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("background_gatherFileRule.aspx?PublishmentSystemID={0}&NoAuto=True", base.PublishmentSystemID), "GatherFileRuleNameCollection", "GatherFileRuleNameCollection", "请选择需要打开自动生成的规则！", "确认要设置所选规则为自动生成吗？");
        }

        public string GetLastGatherDate(DateTime lastGatherDate)
		{
			if (DateUtils.SqlMinValue.Equals(lastGatherDate))
			{
				return string.Empty;
			}
			else
			{
                return DateUtils.GetDateAndTimeString(lastGatherDate);
			}
		}

        public string GetIsAutoCreate(bool isAutoCreate)
        {
            if (isAutoCreate)
            {
                return "是";
            }
            else
            {
                return "否";
            }
        }

        public string GetStartGatherUrl(string gatherRuleName)
		{
            string showPopWinString = Modal.GatherFileSet.GetOpenWindowString(base.PublishmentSystemID, gatherRuleName);
			return string.Format(@"<a href=""javascript:;"" onclick=""{0}"">开始采集</a>", showPopWinString);
		}

		public string GetTestGatherUrl(string gatherRuleName)
		{
            string showPopWinString = Modal.GatherTest.GetOpenWindowString(base.PublishmentSystemID, gatherRuleName, true);
			return string.Format(@"<a href=""javascript:;"" onclick=""{0}"">测试</a>", showPopWinString);
		}

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (base.GetQueryString("Delete") != null)
            {
                string gatherRuleName = base.GetQueryString("GatherRuleName");
                try
                {
                    DataProvider.GatherFileRuleDAO.Delete(gatherRuleName, base.PublishmentSystemID);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除单文件页采集规则", string.Format("采集规则:{0}", gatherRuleName));
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            if (base.GetQueryString("Copy") != null)
            {
                string gatherRuleName = base.GetQueryString("GatherRuleName");
                try
                {
                    GatherFileRuleInfo gatherFileRuleInfo = DataProvider.GatherFileRuleDAO.GetGatherFileRuleInfo(gatherRuleName, base.PublishmentSystemID);
                    gatherFileRuleInfo.GatherRuleName = gatherFileRuleInfo.GatherRuleName + "_复件";
                    gatherFileRuleInfo.LastGatherDate = DateUtils.SqlMinValue;

                    DataProvider.GatherFileRuleDAO.Insert(gatherFileRuleInfo);
                    StringUtility.AddLog(base.PublishmentSystemID, "复制单文件页采集规则", string.Format("采集规则:{0}", gatherRuleName));
                    base.SuccessMessage("采集规则复制成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "采集规则复制失败！");
                }
            }

            if (base.GetQueryString("Auto") != null && base.GetQueryString("GatherFileRuleNameCollection") != null)
            {
                ArrayList gatherFileRuleNameCollection = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("GatherFileRuleNameCollection"));
                try
                {
                    DataProvider.GatherFileRuleDAO.OpenAuto(base.PublishmentSystemID, gatherFileRuleNameCollection);
                    StringUtility.AddLog(base.PublishmentSystemID, "开启采集规则自动生成成功", string.Format("采集规则:{0}", base.GetQueryString("GatherFileRuleNameCollection")));
                    base.SuccessMessage("开启采集规则自动生成成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "开启采集规则自动生成失败！");
                }
            }

            if (base.GetQueryString("NoAuto") != null && base.GetQueryString("GatherFileRuleNameCollection") != null)
            {
                ArrayList gatherFileRuleNameCollection = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("GatherFileRuleNameCollection"));
                try
                {
                    DataProvider.GatherFileRuleDAO.CloseAuto(base.PublishmentSystemID, gatherFileRuleNameCollection);
                    StringUtility.AddLog(base.PublishmentSystemID, "关闭采集规则自动生成成功", string.Format("采集规则:{0}", base.GetQueryString("GatherFileRuleNameCollection")));
                    base.SuccessMessage("关闭采集规则自动生成成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "关闭采集规则自动生成失败！");
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Gather, "单文件页采集", AppManager.CMS.Permission.WebSite.Gather);

                string showPopWinString = Modal.ProgressBar.GetOpenWindowStringWithGatherFile(base.PublishmentSystemID);
                this.Start.Attributes.Add("onclick", showPopWinString);

                this.dgContents.DataSource = DataProvider.GatherFileRuleDAO.GetDataSource(base.PublishmentSystemID);
                this.dgContents.DataBind();
            }
		}
	}
}
