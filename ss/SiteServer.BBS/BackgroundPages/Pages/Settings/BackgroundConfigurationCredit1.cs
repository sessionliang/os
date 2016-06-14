using System;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core.Configuration;
using BaiRong.Model;
using BaiRong.Core;
using System.Web.UI;
using System.Collections;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundConfigurationCredit1 : BackgroundBasePage
	{
        public Literal ltlCreditCalculate;
        public Button SetButton;

        public DataGrid MyDataGrid1;
        public DataGrid MyDataGrid2;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_configurationCredit1.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Settings, "积分类型设置", AppManager.BBS.Permission.BBS_Settings);

                this.ltlCreditCalculate.Text = CreditRuleManager.GetCreditCalculate(base.PublishmentSystemID);
				BindGrid();

                this.SetButton.Attributes.Add("onclick", Modal.CreditCalculate.GetOpenWindowString(base.PublishmentSystemID));
			}
		}

        public void BindGrid()
		{
            ArrayList creditsArrayList = new ArrayList();
            ArrayList othersArrayList = new ArrayList();

            creditsArrayList.Add(ECreditType.Prestige);
            creditsArrayList.Add(ECreditType.Contribution);
            creditsArrayList.Add(ECreditType.Currency);

            othersArrayList.Add(ECreditType.ExtCredit1);
            othersArrayList.Add(ECreditType.ExtCredit2);
            othersArrayList.Add(ECreditType.ExtCredit3);

            MyDataGrid1.DataSource = creditsArrayList;
            MyDataGrid1.ItemDataBound += new DataGridItemEventHandler(MyDataGrid_ItemDataBound);
            MyDataGrid1.DataBind();

            MyDataGrid2.DataSource = othersArrayList;
            MyDataGrid2.ItemDataBound += new DataGridItemEventHandler(MyDataGrid_ItemDataBound);
            MyDataGrid2.DataBind();
		}

        public void MyDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ECreditType creditType = (ECreditType)e.Item.DataItem;

                Literal ltlCreditID = e.Item.FindControl("ltlCreditID") as Literal;
                Literal ltlCreditName = e.Item.FindControl("ltlCreditName") as Literal;
                Literal ltlCreditUnit = e.Item.FindControl("ltlCreditUnit") as Literal;
                Literal ltlInitial = e.Item.FindControl("ltlInitial") as Literal;
                Literal ltlUsing = e.Item.FindControl("ltlUsing") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlCreditID.Text = ECreditTypeUtils.GetCreditID(creditType);
                ltlCreditName.Text = ECreditTypeUtils.GetCreditName(base.PublishmentSystemID, creditType);
                ltlCreditUnit.Text = ECreditTypeUtils.GetCreditUnit(base.PublishmentSystemID, creditType);
                ltlInitial.Text = ECreditTypeUtils.GetCreditInitial(base.PublishmentSystemID, creditType).ToString();
                ltlUsing.Text = StringUtilityBBS.GetTrueOrFalseImageHtml(ECreditTypeUtils.GetIsUsing(base.PublishmentSystemID, creditType));

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.CreditEdit.GetOpenWindowString(base.PublishmentSystemID, creditType));
            }
        }       
	}
}
