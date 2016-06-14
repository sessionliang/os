using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

using BaiRong.Model;
using System.Collections;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundGatherDatabaseRule : BackgroundBasePage
	{
		public DataGrid dgContents;
        public Button Start;

		public string GetDatabaseInfo(string relatedTableName)
		{
            return relatedTableName;
		}

		public string GetEidtLink(string gatherRuleName)
		{
            string urlEdit = PageUtils.GetCMSUrl(string.Format("background_gatherDatabaseRuleAdd.aspx?PublishmentSystemID={0}&GatherRuleName={1}", base.PublishmentSystemID, gatherRuleName));
            return string.Format("<a href=\"{0}\">编辑</a>", urlEdit);
		}

        public string GetAutoCreateClickString()
        {
            return JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("background_gatherDatabaseRule.aspx?PublishmentSystemID={0}&Auto=True", base.PublishmentSystemID), "GatherDatabaseRuleNameCollection", "GatherDatabaseRuleNameCollection", "请选择需要打开自动生成的规则！", "确认要设置所选规则为自动生成吗？");
        }

        public string GetNoAutoCreateClickString()
        {
            return JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("background_gatherDatabaseRule.aspx?PublishmentSystemID={0}&NoAuto=True", base.PublishmentSystemID), "GatherDatabaseRuleNameCollection", "GatherDatabaseRuleNameCollection", "请选择需要打开自动生成的规则！", "确认要设置所选规则为自动生成吗？");
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
            string showPopWinString = Modal.GatherDatabaseSet.GetOpenWindowString(base.PublishmentSystemID, gatherRuleName);
			return string.Format(@"<a href=""javascript:;"" onclick=""{0}"">开始采集</a>", showPopWinString);
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
                    DataProvider.GatherDatabaseRuleDAO.Delete(gatherRuleName, base.PublishmentSystemID);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除数据库采集规则", string.Format("采集规则:{0}", gatherRuleName));
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
                    GatherDatabaseRuleInfo gatherDatabaseRuleInfo = DataProvider.GatherDatabaseRuleDAO.GetGatherDatabaseRuleInfo(gatherRuleName, base.PublishmentSystemID);
                    gatherDatabaseRuleInfo.GatherRuleName = gatherDatabaseRuleInfo.GatherRuleName + "_复件";
                    gatherDatabaseRuleInfo.LastGatherDate = DateUtils.SqlMinValue;

                    DataProvider.GatherDatabaseRuleDAO.Insert(gatherDatabaseRuleInfo);
                    StringUtility.AddLog(base.PublishmentSystemID, "复制数据库采集规则", string.Format("采集规则:{0}", gatherRuleName));
                    base.SuccessMessage("采集规则复制成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "采集规则复制失败！");
                }
            }

            if (base.GetQueryString("Auto") != null && base.GetQueryString("GatherDatabaseRuleNameCollection") != null)
            {
                ArrayList gatherDatabaseRuleNameCollection = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("GatherDatabaseRuleNameCollection"));
                try
                {
                    DataProvider.GatherDatabaseRuleDAO.OpenAuto(base.PublishmentSystemID,gatherDatabaseRuleNameCollection);
                    StringUtility.AddLog(base.PublishmentSystemID, "开启采集规则自动生成成功", string.Format("采集规则:{0}", base.GetQueryString("GatherDatabaseRuleNameCollection")));
                    base.SuccessMessage("开启采集规则自动生成成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "开启采集规则自动生成失败！");
                }
            }

            if (base.GetQueryString("NoAuto") != null && base.GetQueryString("GatherDatabaseRuleNameCollection") != null)
            {
                ArrayList gatherDatabaseRuleNameCollection = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("GatherDatabaseRuleNameCollection"));
                try
                {
                    DataProvider.GatherDatabaseRuleDAO.CloseAuto(base.PublishmentSystemID, gatherDatabaseRuleNameCollection);
                    StringUtility.AddLog(base.PublishmentSystemID, "关闭采集规则自动生成成功", string.Format("采集规则:{0}", base.GetQueryString("GatherDatabaseRuleNameCollection")));
                    base.SuccessMessage("关闭采集规则自动生成成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "关闭采集规则自动生成失败！");
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Gather, "数据库采集", AppManager.CMS.Permission.WebSite.Gather);

                string showPopWinString = Modal.ProgressBar.GetOpenWindowStringWithGatherDatabase(base.PublishmentSystemID);
                this.Start.Attributes.Add("onclick", showPopWinString);

                this.dgContents.DataSource = DataProvider.GatherDatabaseRuleDAO.GetDataSource(base.PublishmentSystemID);
                this.dgContents.DataBind();
            }
		}
	}
}
