using System;
using System.Data;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;



namespace BaiRong.BackgroundPages
{
    public class BackgroundSMSServer : BackgroundBasePage
    {
        public DataGrid dgContents;
        public LinkButton pageFirst;
        public LinkButton pageLast;
        public LinkButton pageNext;
        public LinkButton pagePrevious;
        public Label currentPage;

        protected string configSMSServerType = ConfigManager.Additional.SMSServerType;


        public static string GetRedirectUrl(int SMSServerID)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_SMSServer.aspx"));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                string SMSServerEName = base.GetQueryString("SMSServerEName");
                try
                {

                    BaiRongDataProvider.SMSServerDAO.Delete(SMSServerEName);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除短信服务商", string.Format("短信服务商:{0}", SMSServerEName));

                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            if (base.GetQueryString("Open") != null)
            {
                string SMSServerEName = base.GetQueryString("SMSServerEName");
                try
                {
                    SMSServerInfo smsServerInfo = BaiRongDataProvider.SMSServerDAO.GetSMSServerInfo(SMSServerEName);
                    ConfigManager.Additional.SMSServerType = smsServerInfo.SMSServerEName;
                    BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);
                    ConfigManager.IsChanged = true;
                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "开启短信服务商", string.Format("短信服务商:{0}", SMSServerEName));

                    base.SuccessMessage("操作成功！");
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_SMS, "短信服务商管理", AppManager.Platform.Permission.platform_SMS);

                BindGrid();
            }
        }

        public void dgContents_Page(object sender, DataGridPageChangedEventArgs e)
        {
            this.dgContents.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }

        public DataSet GetDataSetBySMSServers(string[] SMSServers)
        {
            DataSet dataset = new DataSet();

            DataTable dataTable = new DataTable("AllSMSServers");

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "SMSServerEName";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "SMSServerName";
            dataTable.Columns.Add(column);

            foreach (string SMSServerEName in SMSServers)
            {
                DataRow dataRow = dataTable.NewRow();

                SMSServerInfo smsServerInfo = BaiRongDataProvider.SMSServerDAO.GetSMSServerInfo(SMSServerEName);
                dataRow["SMSServerEName"] = SMSServerEName;
                dataRow["SMSServerName"] = smsServerInfo.SMSServerName;

                dataTable.Rows.Add(dataRow);
            }

            dataset.Tables.Add(dataTable);
            return dataset;
        }

        public void BindGrid()
        {
            try
            {
                this.dgContents.PageSize = StringUtils.Constants.PageSize;

                this.dgContents.DataSource = BaiRongDataProvider.SMSServerDAO.GetSMSServerInfoArrayList();


                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                if (this.dgContents.CurrentPageIndex > 0)
                {
                    pageFirst.Enabled = true;
                    pagePrevious.Enabled = true;
                }
                else
                {
                    pageFirst.Enabled = false;
                    pagePrevious.Enabled = false;
                }

                if (this.dgContents.CurrentPageIndex + 1 == this.dgContents.PageCount)
                {
                    pageLast.Enabled = false;
                    pageNext.Enabled = false;
                }
                else
                {
                    pageLast.Enabled = true;
                    pageNext.Enabled = true;
                }

                currentPage.Text = string.Format("{0}/{1}", this.dgContents.CurrentPageIndex + 1, this.dgContents.PageCount);
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }

        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //string SMSServerEName = (string)this.dgContents.DataKeys[e.Item.ItemIndex];
                //e.Item.Visible = !EPredefinedSMSServerUtils.IsPredefinedSMSServer(SMSServerName);
                Literal ltlStatue = e.Item.FindControl("ltlStatue") as Literal;

                SMSServerInfo smsServerInfo = e.Item.DataItem as SMSServerInfo;
                if (smsServerInfo != null)
                {
                    ltlStatue.Text = smsServerInfo.SMSServerEName == configSMSServerType ? "正在使用" : "开启";
                }
            }
        }

        protected void NavigationButtonClick(object sender, System.EventArgs e)
        {
            LinkButton button = (LinkButton)sender;
            string direction = button.CommandName;

            switch (direction.ToUpper())
            {
                case "FIRST":
                    this.dgContents.CurrentPageIndex = 0;
                    break;
                case "PREVIOUS":
                    this.dgContents.CurrentPageIndex =
                        Math.Max(this.dgContents.CurrentPageIndex - 1, 0);
                    break;
                case "NEXT":
                    this.dgContents.CurrentPageIndex =
                        Math.Min(this.dgContents.CurrentPageIndex + 1,
                        this.dgContents.PageCount - 1);
                    break;
                case "LAST":
                    this.dgContents.CurrentPageIndex = this.dgContents.PageCount - 1;
                    break;
                default:
                    break;
            }
            BindGrid();
        }


    }
}
