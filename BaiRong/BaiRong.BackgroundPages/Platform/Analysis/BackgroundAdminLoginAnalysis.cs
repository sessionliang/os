﻿using System;
using System.Collections;
using System.Web.UI.WebControls;


using BaiRong.Core;
using BaiRong.Controls;
using BaiRong.BackgroundPages;
using BaiRong.Model;

namespace BaiRong.BackgroundPages
{
    public class BackgroundAdminLoginAnalysis : BackgroundBasePage
    {
        #region 控件
        protected DateTimeTextBox dateFrom;
        protected DateTimeTextBox dateTo;

        protected DropDownList ddlXType;
        public Button ExportTracking;
        #endregion

        //管理员登录（按日期）
        readonly Hashtable adminNumHashtableDay = new Hashtable();

        //管理员登录（按用户）
        protected Hashtable adminNumHashtableName = new Hashtable();

        int maxAdminNum = 0;

        public int count = 30;
        public EStatictisXType xType = EStatictisXType.Day;

        public double GetAccessNum(int index)
        {
            double accessNum = 0;
            if (this.maxAdminNum > 0)
            {
                accessNum = (Convert.ToDouble(this.maxAdminNum) * Convert.ToDouble(index)) / 8;
                accessNum = Math.Round(accessNum, 2);
            }
            return accessNum;
        }

        public string GetGraphicHtml(int index)
        {
            if (index <= 0 || index > this.count) return string.Empty;
            int accessNum = (int)adminNumHashtableDay[index];
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
            if (this.maxAdminNum > 0)
            {
                height = (Convert.ToDouble(accessNum) / Convert.ToDouble(this.maxAdminNum)) * 200.0;
            }
            string html = string.Format("<IMG title=登录次数：{0} height={1} style=height:{1}px src=../pic/tracker_bar.gif width=16><BR>{2}", accessNum, height, xNum);
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
            int accessNum = (int)adminNumHashtableDay[index];
            return accessNum.ToString();
        }

        public string GetGraphicYUser(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            int accessNum = (int)adminNumHashtableName[key];
            return accessNum.ToString();
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.ExportTracking.Visible = false;
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.TopMenu.ID_Statistics, "管理员登录统计", AppManager.Platform.Permission.Platform_Statistics);

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
                //管理员登录量统计，按照日期
                Hashtable trackingDayHashtable = BaiRongDataProvider.LogDAO.GetAdminLoginHashtableByDate(TranslateUtils.ToDateTime(base.GetQueryString("DateFrom")), TranslateUtils.ToDateTime(base.GetQueryString("DateTo"), DateTime.Now), EStatictisXTypeUtils.GetValue(this.xType), LogInfo.ADMIN_LOGIN);

                //管理员登录量统计，按照用户名
                adminNumHashtableName = BaiRongDataProvider.LogDAO.GetAdminLoginHashtableByName(TranslateUtils.ToDateTime(base.GetQueryString("DateFrom")), TranslateUtils.ToDateTime(base.GetQueryString("DateTo"), DateTime.Now), LogInfo.ADMIN_LOGIN);

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
                    this.adminNumHashtableDay.Add(this.count - i, accessNum);
                    if (accessNum > this.maxAdminNum)
                    {
                        this.maxAdminNum = accessNum;
                    }
                }

            }
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
                    _pageUrl = PageUtils.GetPlatformUrl(string.Format("background_adminLoginAnalysis.aspx?DateFrom={0}&DateTo={1}&XType={2}", this.dateFrom.Text, this.dateTo.Text, this.ddlXType.SelectedValue));
                }
                return _pageUrl;
            }
        }
    }
}
