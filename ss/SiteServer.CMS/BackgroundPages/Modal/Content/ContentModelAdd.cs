using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class ContentModelAdd : BackgroundBasePage
	{
        protected TextBox tbModelID;
        protected TextBox tbModelName;
        protected TextBox tbIconUrl;
        protected RadioButtonList rblTableType;
        protected DropDownList ddlTableName;
        protected TextBox tbDescription;

        protected override bool IsSaasForbidden { get { return true; } }

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("添加内容模型", "modal_contentModelAdd.aspx", arguments, 530, 480);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, string modelID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ModelID", modelID);
            return PageUtility.GetOpenWindowString("修改内容模型", "modal_contentModelAdd.aspx", arguments, 530, 480);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			PageUtils.CheckRequestParameter("PublishmentSystemID");
			if (!IsPostBack)
			{
                this.rblTableType.Items.Add(EAuxiliaryTableTypeUtils.GetListItem(EAuxiliaryTableType.BackgroundContent, true));
                this.rblTableType.Items.Add(EAuxiliaryTableTypeUtils.GetListItem(EAuxiliaryTableType.UserDefined, false));

                if (base.GetQueryString("ModelID") != null)
                {
                    this.tbModelID.Enabled = false;
                    string modelID = base.GetQueryString("ModelID");
                    ContentModelInfo modelInfo = BaiRongDataProvider.ContentModelDAO.GetContentModelInfo(modelID, AppManager.CMS.AppID, base.PublishmentSystemID);
                    if (modelInfo != null)
                    {
                        this.tbModelID.Text = modelID;
                        this.tbModelName.Text = modelInfo.ModelName;
                        this.tbIconUrl.Text = modelInfo.IconUrl;
                        ControlUtils.SelectListItemsIgnoreCase(this.rblTableType, EAuxiliaryTableTypeUtils.GetValue(modelInfo.TableType));
                        this.rblTableType_SelectedIndexChanged(null, null);
                        ControlUtils.SelectListItemsIgnoreCase(this.ddlTableName, modelInfo.TableName);
                        this.tbDescription.Text = modelInfo.Description;
                    }
                }
                else
                {
                    this.rblTableType_SelectedIndexChanged(null, null);
                }
				
			}
		}

        public void rblTableType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlTableName.Items.Clear();
            EAuxiliaryTableType tableType = EAuxiliaryTableTypeUtils.GetEnumType(this.rblTableType.SelectedValue);
            ArrayList tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(tableType);
            foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
            {
                ListItem li = new ListItem(tableInfo.TableCNName + "(" + tableInfo.TableENName + ")", tableInfo.TableENName);
                this.ddlTableName.Items.Add(li);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

			ContentModelInfo modelInfo = null;

			if (base.GetQueryString("ModelID") != null)
			{
                string modelID = base.GetQueryString("ModelID");
                modelInfo = BaiRongDataProvider.ContentModelDAO.GetContentModelInfo(modelID, AppManager.CMS.AppID, base.PublishmentSystemID);

                modelInfo.ModelName = this.tbModelName.Text;
                modelInfo.TableName = this.ddlTableName.SelectedValue;
                modelInfo.TableType = EAuxiliaryTableTypeUtils.GetEnumType(this.rblTableType.SelectedValue);
                modelInfo.IconUrl = this.tbIconUrl.Text;
                modelInfo.Description = this.tbDescription.Text;
			}
			else
			{
                modelInfo = new ContentModelInfo();
                modelInfo.ModelID = this.tbModelID.Text;
                modelInfo.ProductID = AppManager.CMS.AppID;
                modelInfo.SiteID = base.PublishmentSystemID;
                modelInfo.ModelName = this.tbModelName.Text;
                modelInfo.IsSystem = false;
                modelInfo.TableName = this.ddlTableName.SelectedValue;
                modelInfo.TableType = EAuxiliaryTableTypeUtils.GetEnumType(this.rblTableType.SelectedValue);
                modelInfo.IconUrl = this.tbIconUrl.Text;
                modelInfo.Description = this.tbDescription.Text;
			}

            if (base.GetQueryString("ModelID") != null)
			{
				try
				{
                    BaiRongDataProvider.ContentModelDAO.Update(modelInfo);
                    StringUtility.AddLog(base.PublishmentSystemID, "修改内容模型", string.Format("内容模型:{0}", modelInfo.ModelName));
					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "内容模型修改失败！");
				}
			}
			else
			{
                bool isFail = false;
                ArrayList modelInfoArrayList = ContentModelManager.GetContentModelArrayList(base.PublishmentSystemInfo);
                foreach (ContentModelInfo contentModelInfo in modelInfoArrayList)
                {
                    if (contentModelInfo.ModelID == this.tbModelID.Text)
                    {
                        base.FailMessage("内容模型添加失败，模型标识已存在！");
                        isFail = true;
                        break;
                    }
                    else if (contentModelInfo.ModelName == this.tbModelName.Text)
                    {
                        base.FailMessage("内容模型添加失败，模型名称已存在！");
                        isFail = true;
                        break;
                    }
                }
                if (!isFail)
				{
                    try
                    {
                        BaiRongDataProvider.ContentModelDAO.Insert(modelInfo);
                        StringUtility.AddLog(base.PublishmentSystemID, "添加内容模型", string.Format("内容模型:{0}", modelInfo.ModelName));
                        isChanged = true;
                    }
                    catch(Exception ex)
                    {
                        base.FailMessage(ex, "内容模型添加失败！");
                    }
				}
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
