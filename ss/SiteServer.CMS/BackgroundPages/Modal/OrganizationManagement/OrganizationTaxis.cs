﻿using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class OrganizationTaxis : BackgroundBasePage
    {
        protected RadioButtonList TaxisType;
        protected TextBox TaxisNum;
         
        private int classifyID;
        private string returnUrl;
        private ArrayList contentIDArrayList;

        public static string GetOpenWindowString(int publishmentSystemID,   int classifyID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString()); 
            arguments.Add("ClassifyID", classifyID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtility.GetOpenWindowStringWithCheckBoxValue("分支机构排序", "modal_organizationTaxis.aspx", arguments, "ContentIDCollection", "请选择需要排序的内容！", 300, 220);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID",  "ReturnUrl"); 
            this.classifyID = TranslateUtils.ToInt(base.GetQueryStringNoSqlAndXss("ClassifyID"));
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ContentIDCollection"));

            if (!IsPostBack)
            {
                this.TaxisType.Items.Add(new ListItem("上升", "Up"));
                this.TaxisType.Items.Add(new ListItem("下降", "Down"));
                ControlUtils.SelectListItems(this.TaxisType, "Up");


            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isUp = (this.TaxisType.SelectedValue == "Up");
            int taxisNum = int.Parse(this.TaxisNum.Text);

            if (isUp == false)
            {
                this.contentIDArrayList.Reverse();
            }

            foreach (int contentID in this.contentIDArrayList)
            {
                for (int i = 1; i <= taxisNum; i++)
                {
                    if (isUp)
                    {
                        if (DataProvider.OrganizationInfoDAO.UpdateTaxisToUp( this.classifyID, contentID))
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (DataProvider.OrganizationInfoDAO.UpdateTaxisToDown(this.classifyID, contentID))
                        {
                            break;
                        }
                    }
                }
            }

            JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
        }

    }
}
