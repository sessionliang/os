using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using System.Collections;
using SiteServer.CMS.Model;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal
{
	public class ContentExport : BackgroundBasePage
	{
        public RadioButtonList rblExportType;
        public DropDownList ddlPeriods;
        public DateTimeTextBox tbStartDate;
        public DateTimeTextBox tbEndDate;
        public PlaceHolder phDisplayAttributes;
        public CheckBoxList cblDisplayAttributes;
        public DropDownList ddlIsChecked;

        private int nodeID;

        private void LoadDisplayAttributeCheckBoxList()
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.nodeID);
            ContentModelInfo modelInfo = ContentModelManager.GetContentModelInfo(base.PublishmentSystemInfo, nodeInfo.ContentModelID);
            ETableStyle tableStyle = EAuxiliaryTableTypeUtils.GetTableStyle(modelInfo.TableType);
            ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, modelInfo.TableName, relatedIdentities);
            tableStyleInfoArrayList = ContentUtility.GetAllTableStyleInfoArrayList(base.PublishmentSystemInfo, tableStyle, tableStyleInfoArrayList);

            foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
            {
                ListItem listItem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);
                listItem.Selected = styleInfo.IsVisible;
                this.cblDisplayAttributes.Items.Add(listItem);
            }
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"), base.PublishmentSystemID);
			if (!IsPostBack)
			{
                this.LoadDisplayAttributeCheckBoxList();
                this.ConfigSettings(true);
			}
		}

        private void ConfigSettings(bool isLoad)
        {
            if (isLoad)
            {
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.Config_ExportType))
                {
                    this.rblExportType.SelectedValue = base.PublishmentSystemInfo.Additional.Config_ExportType;
                }
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.Config_ExportPeriods))
                {
                    this.ddlPeriods.SelectedValue = base.PublishmentSystemInfo.Additional.Config_ExportPeriods;
                }
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.Config_ExportDisplayAttributes))
                {
                    ArrayList displayAttributes = TranslateUtils.StringCollectionToArrayList(base.PublishmentSystemInfo.Additional.Config_ExportDisplayAttributes);
                    ControlUtils.SelectListItems(this.cblDisplayAttributes, displayAttributes);
                }
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.Config_ExportIsChecked))
                {
                    this.ddlIsChecked.SelectedValue = base.PublishmentSystemInfo.Additional.Config_ExportIsChecked;
                }
            }
            else
            {
                base.PublishmentSystemInfo.Additional.Config_ExportType = this.rblExportType.SelectedValue;
                base.PublishmentSystemInfo.Additional.Config_ExportPeriods = this.ddlPeriods.SelectedValue;
                base.PublishmentSystemInfo.Additional.Config_ExportDisplayAttributes = ControlUtils.GetSelectedListControlValueCollection(this.cblDisplayAttributes);
                base.PublishmentSystemInfo.Additional.Config_ExportIsChecked = this.ddlIsChecked.SelectedValue;
                DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
            }
        }

        public void rblExportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phDisplayAttributes.Visible = this.rblExportType.SelectedValue != "ContentZip";
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            string displayAttributes = ControlUtils.GetSelectedListControlValueCollection(this.cblDisplayAttributes);
            if (this.phDisplayAttributes.Visible && string.IsNullOrEmpty(displayAttributes))
            {
                base.FailMessage("必须至少选择一项！");
                return;
            }

            this.ConfigSettings(false);

            bool isPeriods = false;
            string startDate = string.Empty;
            string endDate = string.Empty;
            if (this.ddlPeriods.SelectedValue != "0")
            {
                isPeriods = true;
                if (this.ddlPeriods.SelectedValue == "-1")
                {
                    startDate = this.tbStartDate.Text;
                    endDate = this.tbEndDate.Text;
                }
                else
                {
                    int days = int.Parse(this.ddlPeriods.SelectedValue);
                    startDate = DateUtils.GetDateString(DateTime.Now.AddDays(-days));
                    endDate = DateUtils.GetDateString(DateTime.Now);
                }
            }
            ETriState checkedState = ETriStateUtils.GetEnumType(this.ddlPeriods.SelectedValue);
            string redirectUrl = PageUtility.ModalSTL.ExportMessage.GetRedirectUrlStringToExportContent(base.PublishmentSystemID, this.nodeID, this.rblExportType.SelectedValue, base.GetQueryString("ContentIDCollection"), displayAttributes, isPeriods, startDate, endDate, checkedState);
            PageUtils.Redirect(redirectUrl);
		}
	}
}
