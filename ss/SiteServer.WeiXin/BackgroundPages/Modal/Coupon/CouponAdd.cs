using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Net;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using SiteServer.CMS.Core;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class CouponAdd : BackgroundBasePage
    {
        public TextBox tbTitle;
        public TextBox tbTotalNum;
        public CheckBox cbIsEnabled;

        private int couponID;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityWX.GetOpenWindowString("添加优惠劵", "modal_couponAdd.aspx", arguments, 400, 300);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int couponID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("couponID", couponID.ToString());
            return PageUtilityWX.GetOpenWindowString("编辑优惠劵", "modal_couponAdd.aspx", arguments, 400, 300);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.couponID = TranslateUtils.ToInt(base.GetQueryString("couponID"));

            if (!IsPostBack)
            {
                if (this.couponID > 0)
                {
                    CouponInfo couponInfo = DataProviderWX.CouponDAO.GetCouponInfo(this.couponID);

                    this.tbTitle.Text = couponInfo.Title;
                    this.tbTotalNum.Text = couponInfo.TotalNum.ToString();
                    this.tbTotalNum.Enabled = false;
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {
                if (this.couponID == 0)
                {
                    int totalNum = TranslateUtils.ToInt(this.tbTotalNum.Text);

                    if (totalNum > 1000)
                    {
                        base.FailMessage("添加失败，一次最多只能新增1000张优惠劵");
                    }
                    else
                    {
                        CouponInfo couponInfo = new CouponInfo { ID = 0, PublishmentSystemID = base.PublishmentSystemID, ActID = 0, Title = this.tbTitle.Text, TotalNum = totalNum, AddDate = DateTime.Now };

                        int couponID = DataProviderWX.CouponDAO.Insert(couponInfo);

                        if (this.cbIsEnabled.Checked == false)
                        {
                            DataProviderWX.CouponSNDAO.Insert(base.PublishmentSystemID, couponID, totalNum);
                        }
                        StringUtility.AddLog(base.PublishmentSystemID, "添加优惠劵", string.Format("优惠劵:{0}", this.tbTitle.Text));

                        isChanged = true;
                    }
                }
                else
                {
                    CouponInfo couponInfo = DataProviderWX.CouponDAO.GetCouponInfo(this.couponID);
                    couponInfo.Title = this.tbTitle.Text;

                    DataProviderWX.CouponDAO.Update(couponInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "编辑优惠劵", string.Format("优惠劵:{0}", this.tbTitle.Text));

                    isChanged = true;
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "失败：" + ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
    }
}
