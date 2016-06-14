using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;


using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using SiteServer.CMS.BackgroundPages;
using System.Web.UI.HtmlControls;

namespace SiteServer.B2C.BackgroundPages.Modal
{
    public class UserConsigneeAdd : BackgroundBasePage
    {
        public TextBox tbConsignee;
        //public DropDownList ddlProvice;
        //public DropDownList ddlCity;
        //public DropDownList ddlArea;
        public TextBox tbAddress;
        public TextBox tbZipcode;
        public TextBox tbMobile;
        public TextBox tbTel;
        public TextBox tbEmail;

        public HtmlInputHidden provinceValue;

        private string userName;
        private int consigneeID = 0;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID,string userName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("userName", userName);
            return JsUtils.OpenWindow.GetOpenWindowString("添加收货方式", "modal_userConsigneeAdd.aspx", arguments);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, string userName, int consigneeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("userName", userName);
            arguments.Add("consigneeID", consigneeID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("修改收货方式", "modal_userConsigneeAdd.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.userName = base.GetQueryString("userName");
            this.consigneeID = base.GetIntQueryString("consigneeID");

            if (!IsPostBack)
            {
                if (this.consigneeID != 0)
                {
                    ConsigneeInfo consigneeInfo = DataProviderB2C.ConsigneeDAO.GetConsigneeInfo(this.consigneeID);

                    this.tbConsignee.Text = consigneeInfo.Consignee;
                    this.tbAddress.Text = consigneeInfo.Address;
                    this.tbZipcode.Text = consigneeInfo.Zipcode;
                    this.tbMobile.Text = consigneeInfo.Mobile;
                    this.tbTel.Text = consigneeInfo.Tel;
                    this.tbEmail.Text = consigneeInfo.Email;
                    this.provinceValue.Value = consigneeInfo.Province + "," + consigneeInfo.City + "," + consigneeInfo.Area;
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {

                string provice = string.Empty, city = string.Empty, area = string.Empty;
                if (!string.IsNullOrEmpty(this.provinceValue.Value))
                {
                    string[] location = this.provinceValue.Value.Split(new char[] { ',' });
                    provice = location[0];
                    city = location[1];
                    area = location[2];
                }

                if (this.consigneeID == 0)
                {
                    ConsigneeInfo consigneeInfo = new ConsigneeInfo { GroupSN = base.PublishmentSystemInfo.GroupSN, UserName = this.userName, IsOrder = false, IPAddress = PageUtils.GetIPAddress(), IsDefault = false, Consignee = this.tbConsignee.Text, Province = provice, City = city, Area = area, Address = this.tbAddress.Text, Zipcode = this.tbZipcode.Text, Mobile = this.tbMobile.Text, Tel = this.tbTel.Text, Email = this.tbEmail.Text };

                    DataProviderB2C.ConsigneeDAO.Insert(consigneeInfo);
                }
                else
                {
                    ConsigneeInfo consigneeInfo = DataProviderB2C.ConsigneeDAO.GetConsigneeInfo(this.consigneeID);

                    consigneeInfo.Consignee = this.tbConsignee.Text;
                    consigneeInfo.Province = provice;
                    consigneeInfo.City = city;
                    consigneeInfo.Area = area;
                    consigneeInfo.Address = this.tbAddress.Text;
                    consigneeInfo.Zipcode = this.tbZipcode.Text;
                    consigneeInfo.Mobile = this.tbMobile.Text;
                    consigneeInfo.Tel = this.tbTel.Text;
                    consigneeInfo.Email = this.tbEmail.Text;

                    DataProviderB2C.ConsigneeDAO.Update(consigneeInfo);
                }

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "维护收货方式信息");

                base.SuccessMessage("收货方式设置成功！");
                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "收货方式设置失败！");
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(this.Page);
            }
        }
    }
}
