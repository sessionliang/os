﻿using System;
using System.Collections;
using System.Web.UI.WebControls;


using BaiRong.Core;
using BaiRong.Controls;
using BaiRong.BackgroundPages;
using BaiRong.Model;

namespace BaiRong.BackgroundPages
{
    public class BackgroundUserAnalysis : BackgroundBasePage
    {
        #region 控件
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

            this.ExportTracking.Visible = false;
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.TopMenu.ID_Statistics, "会员新增数据统计", AppManager.Platform.Permission.Platform_Statistics);

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
                //用户添加量统计
                Hashtable trackingDayHashtable = BaiRongDataProvider.UserDAO.GetTrackingHashtable( TranslateUtils.ToDateTime(base.GetQueryString("DateFrom")), TranslateUtils.ToDateTime(base.GetQueryString("DateTo"), DateTime.Now), EStatictisXTypeUtils.GetValue(this.xType));

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
                    _pageUrl = PageUtils.GetPlatformUrl(string.Format("background_userAnalysis.aspx?DateFrom={0}&DateTo={1}&XType={2}", this.dateFrom.Text, this.dateTo.Text, this.ddlXType.SelectedValue));
                }
                return _pageUrl;
            }
        }
    }
}
