using System.Web.UI;
using BaiRong.Core;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
    public class StarsManager
    {
        private StarsManager()
        {
        }

        public static string GetStarString(int publishmentSystemID, int channelID, int contentID)
        {
            int[] counts = DataProvider.StarDAO.GetCount(publishmentSystemID, channelID, contentID);
            int totalCount = counts[0];
            int totalPoint = counts[1];

            object[] totalCountAndPointAverage = DataProvider.StarSettingDAO.GetTotalCountAndPointAverage(publishmentSystemID, contentID);
            int settingTotalCount = (int)totalCountAndPointAverage[0];
            decimal settingPointAverage = (decimal)totalCountAndPointAverage[1];
            if (settingTotalCount > 0 || settingPointAverage > 0)
            {
                totalCount += settingTotalCount;
                totalPoint += Convert.ToInt32(settingPointAverage * settingTotalCount);
            }

            decimal num = 0;
            string display = string.Empty;
            if (totalCount > 0)
            {
                num = Convert.ToDecimal(totalPoint) / Convert.ToDecimal(totalCount);
                string numString = num.ToString();
                if (numString.IndexOf('.') == -1)
                {
                    display = numString + ".0";
                }
                else
                {
                    string first = numString.Substring(0, numString.IndexOf('.'));
                    string second = numString.Substring(numString.IndexOf('.') + 1, 1);
                    display = first + "." + second;
                }

                display = string.Format("评分：{0} 人数：{1}", display, totalCount);
            }
            else
            {
                display = "未设置";
            }

            return display;
        }

        public static void SetStarSetting(int publishmentSystemID, int channelID, int contentID, int totalCount, decimal pointAverage)
        {
            DataProvider.StarSettingDAO.SetStarSetting(publishmentSystemID, channelID, contentID, totalCount, pointAverage);
        }

        public static string GetStarSettingToExport(int publishmentSystemID, int channelID, int contentID)
        {
            int[] counts = DataProvider.StarDAO.GetCount(publishmentSystemID, channelID, contentID);
            int totalCount = counts[0];
            int totalPoint = counts[1];

            object[] totalCountAndPointAverage = DataProvider.StarSettingDAO.GetTotalCountAndPointAverage(publishmentSystemID, contentID);
            int settingTotalCount = (int)totalCountAndPointAverage[0];
            decimal settingPointAverage = (decimal)totalCountAndPointAverage[1];
            if (settingTotalCount > 0 || settingPointAverage > 0)
            {
                totalCount += settingTotalCount;
                totalPoint += Convert.ToInt32(settingPointAverage * settingTotalCount);
            }

            if (totalCount > 0)
            {
                string display = string.Empty;
                decimal num = Convert.ToDecimal(totalPoint) / Convert.ToDecimal(totalCount);
                string numString = num.ToString();
                if (numString.IndexOf('.') == -1)
                {
                    display = numString + ".0";
                }
                else
                {
                    string first = numString.Substring(0, numString.IndexOf('.'));
                    string second = numString.Substring(numString.IndexOf('.') + 1, 1);
                    display = first + "." + second;
                }
                return string.Format("{0}_{1}", totalCount, display);
            }

            return string.Empty;
        }
    }
}
