﻿using System;
using System.Collections;
using System.Web.UI.WebControls;


using BaiRong.Core;
using BaiRong.Controls;
using BaiRong.BackgroundPages;
using BaiRong.Model;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;
using SiteServer.CMS.Controls;


namespace SiteServer.CMS.BackgroundPages
{
    public class ConsoleMLibAnalysis : BackgroundBasePage
    {
        #region 控件
        protected DropDownList ddlPublishmentSystem;
        protected DateTimeTextBox dateFrom;
        protected DateTimeTextBox dateTo;

        protected DropDownList ddlXType;
        public Button ExportTracking;
        #endregion

        //用户数量
        readonly Hashtable userNumHashtable = new Hashtable();
        int maxUserNum = 0;

        public int count = 30;
        public EStatictisXType xType = EStatictisXType.Day;

        public double GetAccessNum(int index)
        {
            double accessNum = 0;
            if (this.maxUserNum > 0)
            {
                accessNum = (Convert.ToDouble(this.maxUserNum) * Convert.ToDouble(index)) / 8;
                accessNum = Math.Round(accessNum, 2);
            }
            return accessNum;
        }

        public string GetGraphicHtml(int index)
        {
            if (index <= 0 || index > this.count) return string.Empty;
            int accessNum = (int)userNumHashtable[index];
            DateTime datetime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            int xNum = 0;
            if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Day))
            {
                datetime = datetime.AddDays(-(this.count - index));
                xNum = datetime.Day;
            }
            else if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Month))
            {
                datetime = datetime.AddMonths(-(this.count - index));
                xNum = datetime.Month;
            }
            else if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Year))
            {
                datetime = datetime.AddYears(-(this.count - index));
                xNum = datetime.Year;
            }

            double height = 0;
            if (this.maxUserNum > 0)
            {
                height = (Convert.ToDouble(accessNum) / Convert.ToDouble(this.maxUserNum)) * 200.0;
            }
            string html = string.Format("<IMG title=访问量：{0} height={1} style=height:{1}px src=../pic/tracker_bar.gif width=16><BR>{2}", accessNum, height, xNum);
            return html;
        }

        public string GetGraphicX(int index)
        {
            int xNum = 0;
            DateTime datetime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Day))
            {
                datetime = datetime.AddDays(-(this.count - index));
                xNum = datetime.Day;
            }
            else if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Month))
            {
                datetime = datetime.AddMonths(-(this.count - index));
                xNum = datetime.Month;
            }
            else if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Year))
            {
                datetime = datetime.AddYears(-(this.count - index));
                xNum = datetime.Year;
            }
            return xNum.ToString();
        }

        public string GetGraphicY(int index)
        {
            if (index <= 0 || index > this.count) return string.Empty;
            int accessNum = (int)userNumHashtable[index];
            return accessNum.ToString();
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (ConfigManager.Additional.IsUseMLib == false)
            {
                PageUtils.RedirectToErrorPage("投稿中心尚未开启，无法统计稿件.请在投稿基本设置中启用投稿");
                return;
            }

            this.ExportTracking.Visible = false;

            #region 获取稿件发布都有权限的站点和栏目
            if (this.ddlPublishmentSystem.Items.Count == 0)
            {
                this.ddlPublishmentSystem.Items.Clear();
                foreach (PublishmentSystemInfo info in PublishmentSystemManager.GetPublishmentSystem(UserManager.CurrentNewGroupMLibAddUser))
                {
                    ListItem item = new ListItem(info.PublishmentSystemName, info.PublishmentSystemID.ToString());
                    this.ddlPublishmentSystem.Items.Add(item);
                }
            }
            #endregion
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.TopMenu.ID_Statistics, "稿件新增数据统计", AppManager.Platform.Permission.Platform_Statistics);

                EStatictisXTypeUtils.AddListItems(this.ddlXType);

                this.xType = EStatictisXTypeUtils.GetEnumType(base.GetQueryString("XType"));

                if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Day))
                {
                    this.count = 30;
                }
                else if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Month))
                {
                    this.count = 12;
                }
                else if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Year))
                {
                    this.count = 10;
                }


                #region 控件赋值
                this.dateFrom.Text = base.GetQueryString("DateFrom");
                this.dateTo.Text = base.GetQueryString("DateTo");
                this.ddlXType.SelectedValue = EStatictisXTypeUtils.GetValue(this.xType);
                #endregion


                ddlPublishmentSystem_OnSelectedIndexChanged(sender, E);
            }
        }

        public void ContentBind(int publishmentSystemId, string xtype)
        {
            this.xType = EStatictisXTypeUtils.GetEnumType(xtype);
            PublishmentSystemInfo pinfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemId);

            string tableName = PublishmentSystemManager.GetAuxiliaryTableNameList(pinfo)[0];

            //用户添加量统计
            Hashtable trackingDayHashtable = DataProvider.ContentDAO.GetTrackingHashtable(publishmentSystemId, tableName, TranslateUtils.ToDateTime(base.GetQueryString("DateFrom")), TranslateUtils.ToDateTime(base.GetQueryString("DateTo"), DateTime.Now), xtype);

            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            for (int i = 0; i < this.count; i++)
            {
                DateTime datetime = now.AddDays(-i);
                if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Day))
                {
                    now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    datetime = now.AddDays(-i);
                }
                else if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Month))
                {
                    now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                    datetime = now.AddMonths(-i);
                }
                else if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Year))
                {
                    now = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
                    datetime = now.AddYears(-i);
                }

                int accessNum = 0;
                if (trackingDayHashtable[datetime] != null)
                {
                    accessNum = (int)trackingDayHashtable[datetime];
                }
                this.userNumHashtable.Add(this.count - i, accessNum);
                if (accessNum > this.maxUserNum)
                {
                    this.maxUserNum = accessNum;
                }
            }
        }


        public void ddlPublishmentSystem_OnSelectedIndexChanged(object sender, EventArgs E)
        {
            int publishmentSystemId = TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue);

            ContentBind(publishmentSystemId, this.ddlXType.SelectedValue);
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("console_mlibAnalysis.aspx?PublishmentSystemID={3}&DateFrom={0}&DateTo={1}&XType={2}", this.dateFrom.Text, this.dateTo.Text, this.ddlXType.SelectedValue, this.ddlPublishmentSystem.SelectedValue));
                }
                return _pageUrl;
            }
        }
    }
}
