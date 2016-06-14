using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundGatherRule : BackgroundBasePage
    {
        public DataGrid dgContents;
        public Button Start;
        public Button Export;

        public string GetImportClickString()
        {
            return PageUtility.ModalSTL.Import.GetOpenWindowString(base.PublishmentSystemID, PageUtility.ModalSTL.Import.TYPE_GATHERRULE);
        }

        public string GetAutoCreateClickString()
        {
            return JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("background_gatherRule.aspx?PublishmentSystemID={0}&Auto=True", base.PublishmentSystemID), "GatherRuleNameCollection", "GatherRuleNameCollection", "请选择需要打开自动生成的规则！", "确认要设置所选规则为自动生成吗？");
        }

        public string GetNoAutoCreateClickString()
        {
            return JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("background_gatherRule.aspx?PublishmentSystemID={0}&NoAuto=True", base.PublishmentSystemID), "GatherRuleNameCollection", "GatherRuleNameCollection", "请选择需要打开自动生成的规则！", "确认要设置所选规则为自动生成吗？");
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
                    DataProvider.GatherRuleDAO.Delete(gatherRuleName, base.PublishmentSystemID);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除Web页面采集规则", string.Format("采集规则:{0}", gatherRuleName));
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
                    GatherRuleInfo gatherRuleInfo = DataProvider.GatherRuleDAO.GetGatherRuleInfo(gatherRuleName, base.PublishmentSystemID);
                    gatherRuleInfo.GatherRuleName = gatherRuleInfo.GatherRuleName + "_复件";
                    gatherRuleInfo.LastGatherDate = DateUtils.SqlMinValue;

                    DataProvider.GatherRuleDAO.Insert(gatherRuleInfo);
                    StringUtility.AddLog(base.PublishmentSystemID, "复制Web页面采集规则", string.Format("采集规则:{0}", gatherRuleName));
                    base.SuccessMessage("采集规则复制成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "采集规则复制失败！");
                }
            }

            if (base.GetQueryString("Auto") != null && base.GetQueryString("GatherRuleNameCollection") != null)
            {
                ArrayList gatherRuleNameCollection = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("GatherRuleNameCollection"));
                try
                {
                    foreach (string item in gatherRuleNameCollection)
                    {
                        GatherRuleInfo gatherRuleInfoTmp = DataProvider.GatherRuleDAO.GetGatherRuleInfo(item, base.PublishmentSystemID);
                        gatherRuleInfoTmp.Additional.IsAutoCreate = true;
                        DataProvider.GatherRuleDAO.Update(gatherRuleInfoTmp);
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, "开启采集规则自动生成成功", string.Format("采集规则:{0}", gatherRuleNameCollection));
                    base.SuccessMessage("开启采集规则自动生成成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "开启采集规则自动生成失败！");
                }
            }

            if (base.GetQueryString("NoAuto") != null && base.GetQueryString("GatherRuleNameCollection") != null)
            {
                ArrayList gatherRuleNameCollection = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("GatherRuleNameCollection"));
                try
                {
                    foreach (string item in gatherRuleNameCollection)
                    {
                        GatherRuleInfo gatherRuleInfoTmp = DataProvider.GatherRuleDAO.GetGatherRuleInfo(item, base.PublishmentSystemID);
                        gatherRuleInfoTmp.Additional.IsAutoCreate = false;
                        DataProvider.GatherRuleDAO.Update(gatherRuleInfoTmp);
                    }
                    StringUtility.AddLog(base.PublishmentSystemID, "关闭采集规则自动生成成功", string.Format("采集规则:{0}", gatherRuleNameCollection));
                    base.SuccessMessage("关闭采集规则自动生成成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "关闭采集规则自动生成失败！");
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Gather, "Web页面信息采集", AppManager.CMS.Permission.WebSite.Gather);

                string showPopWinString = Modal.ProgressBar.GetOpenWindowStringWithGather(base.PublishmentSystemID);
                this.Start.Attributes.Add("onclick", showPopWinString);

                showPopWinString = PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToGatherRule(base.PublishmentSystemID, "GatherRuleNameCollection", "请选择需要导出的规则！");
                this.Export.Attributes.Add("onclick", showPopWinString);

                this.dgContents.DataSource = DataProvider.GatherRuleDAO.GetGatherRuleInfoArrayList(base.PublishmentSystemID);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GatherRuleInfo gatherRuleInfo = e.Item.DataItem as GatherRuleInfo;

                Literal ltlGatherRuleName = (Literal)e.Item.FindControl("ltlGatherRuleName");
                Literal ltlGatherUrl = (Literal)e.Item.FindControl("ltlGatherUrl");
                Literal ltlLastGatherDate = (Literal)e.Item.FindControl("ltlLastGatherDate");
                Literal ltlIsAutoCreate = (Literal)e.Item.FindControl("ltlIsAutoCreate");
                Literal ltlTestGatherUrl = (Literal)e.Item.FindControl("ltlTestGatherUrl");
                Literal ltlStartGatherUrl = (Literal)e.Item.FindControl("ltlStartGatherUrl");
                Literal ltlEditLink = (Literal)e.Item.FindControl("ltlEditLink");
                Literal ltlCopyLink = (Literal)e.Item.FindControl("ltlCopyLink");
                Literal ltlDeleteLink = (Literal)e.Item.FindControl("ltlDeleteLink");

                ltlGatherRuleName.Text = gatherRuleInfo.GatherRuleName;
                ArrayList gatherUrlArrayList = GatherUtility.GetGatherUrlArrayList(gatherRuleInfo);
                if (gatherUrlArrayList != null && gatherUrlArrayList.Count > 0)
                {
                    string url = (string)gatherUrlArrayList[0];
                    ltlGatherUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"" title=""{0}"">{1}</a>", PageUtils.AddProtocolToUrl(url), StringUtils.MaxLengthText(url, 25));
                }
                if (!DateUtils.SqlMinValue.Equals(gatherRuleInfo.LastGatherDate))
                {
                    ltlLastGatherDate.Text = DateUtils.GetDateAndTimeString(gatherRuleInfo.LastGatherDate);
                }
                if (gatherRuleInfo.Additional.IsAutoCreate)
                    ltlIsAutoCreate.Text = "是";
                else
                    ltlIsAutoCreate.Text = "否";
                string showPopWinString = Modal.GatherTest.GetOpenWindowString(base.PublishmentSystemID, gatherRuleInfo.GatherRuleName, false);
                ltlTestGatherUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">测试</a>", showPopWinString);

                showPopWinString = Modal.GatherSet.GetOpenWindowString(base.PublishmentSystemID, gatherRuleInfo.GatherRuleName);
                ltlStartGatherUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">开始采集</a>", showPopWinString);

                string urlEdit = PageUtils.GetCMSUrl(string.Format("background_gatherRuleAdd.aspx?PublishmentSystemID={0}&GatherRuleName={1}", base.PublishmentSystemID, gatherRuleInfo.GatherRuleName));
                ltlEditLink.Text = string.Format("<a href=\"{0}\">编辑</a>", urlEdit);

                string urlCopy = PageUtils.GetCMSUrl(string.Format("background_gatherRule.aspx?PublishmentSystemID={0}&GatherRuleName={1}&Copy=True", base.PublishmentSystemID, gatherRuleInfo.GatherRuleName));
                ltlCopyLink.Text = string.Format(@"<a href=""{0}"">复制</a>", urlCopy);

                string urlDelete = PageUtils.GetCMSUrl(string.Format("background_gatherRule.aspx?PublishmentSystemID={0}&GatherRuleName={1}&Delete=True", base.PublishmentSystemID, gatherRuleInfo.GatherRuleName));
                ltlDeleteLink.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除采集规则“{1}”，确认吗？');"">删除</a>", urlDelete, gatherRuleInfo.GatherRuleName);
            }
        }

    }
}
