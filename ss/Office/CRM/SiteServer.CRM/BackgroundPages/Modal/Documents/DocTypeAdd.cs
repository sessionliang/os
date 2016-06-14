using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CRM.Core;
using SiteServer.CRM.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages.Modal
{
	public class DocTypeAdd : BackgroundBasePage
	{
        public DropDownList ddlParentID;
        public TextBox tbTypeName;
        public TextBox tbDescription;

        private int docTypeID;
        private int typeID;

        public static string GetShowPopWinStringToAdd(int docTypeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("DocTypeID", docTypeID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加文档类别", "modal_docTypeAdd.aspx", arguments, 450, 300);
        }

        public static string GetShowPopWinStringToEdit(int typeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("TypeID", typeID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("修改文档类别", "modal_docTypeAdd.aspx", arguments, 450, 300);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.docTypeID = TranslateUtils.ToInt(base.Request.QueryString["DocTypeID"]);
            this.typeID = TranslateUtils.ToInt(base.Request.QueryString["TypeID"]);

			if (!IsPostBack)
			{
                ListItem listItem = new ListItem("无父类别", "0");
                this.ddlParentID.Items.Add(listItem);
                ArrayList typeInfoArrayList = DataProvider.DocTypeDAO.GetTypeInfoArrayList(0);
                foreach (DocTypeInfo typeInfo in typeInfoArrayList)
                {
                    listItem = new ListItem(typeInfo.TypeName, typeInfo.TypeID.ToString());
                    this.ddlParentID.Items.Add(listItem);
                }
                ControlUtils.SelectListItems(this.ddlParentID, this.docTypeID.ToString());

                if (this.typeID > 0)
                {
                    this.ddlParentID.Visible = false;
                    DocTypeInfo typeInfo = DataProvider.DocTypeDAO.GetTypeInfo(this.typeID);
                    if (typeInfo != null)
                    {
                        this.tbTypeName.Text = typeInfo.TypeName;
                        this.tbDescription.Text = typeInfo.Description;
                    }
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            DocTypeInfo typeInfo = null;
				
			if (this.typeID > 0)
			{
				try
				{
                    typeInfo = DataProvider.DocTypeDAO.GetTypeInfo(this.typeID);
                    if (typeInfo != null)
                    {
                        typeInfo.TypeName = this.tbTypeName.Text;
                        typeInfo.Description = this.tbDescription.Text;
                    }
                    DataProvider.DocTypeDAO.Update(typeInfo);

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "文档类别修改失败！");
				}
			}
			else
			{
                int parentID = TranslateUtils.ToInt(this.ddlParentID.SelectedValue);
                ArrayList typeNameArrayList = DataProvider.DocTypeDAO.GetTypeNameArrayList(parentID);
                if (typeNameArrayList.IndexOf(this.tbTypeName.Text) != -1)
				{
                    base.FailMessage("文档类别添加失败，文档类别名称已存在！");
                }
				else
				{
					try
					{
                        typeInfo = new DocTypeInfo(0, parentID, this.tbTypeName.Text, 0, this.tbDescription.Text, DateTime.Now);

                        DataProvider.DocTypeDAO.Insert(typeInfo);

						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "文档类别添加失败！");
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
