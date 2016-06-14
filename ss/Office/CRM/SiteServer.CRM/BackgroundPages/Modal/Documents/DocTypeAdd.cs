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
            return JsUtils.OpenWindow.GetOpenWindowString("����ĵ����", "modal_docTypeAdd.aspx", arguments, 450, 300);
        }

        public static string GetShowPopWinStringToEdit(int typeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("TypeID", typeID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("�޸��ĵ����", "modal_docTypeAdd.aspx", arguments, 450, 300);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.docTypeID = TranslateUtils.ToInt(base.Request.QueryString["DocTypeID"]);
            this.typeID = TranslateUtils.ToInt(base.Request.QueryString["TypeID"]);

			if (!IsPostBack)
			{
                ListItem listItem = new ListItem("�޸����", "0");
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
                    base.FailMessage(ex, "�ĵ�����޸�ʧ�ܣ�");
				}
			}
			else
			{
                int parentID = TranslateUtils.ToInt(this.ddlParentID.SelectedValue);
                ArrayList typeNameArrayList = DataProvider.DocTypeDAO.GetTypeNameArrayList(parentID);
                if (typeNameArrayList.IndexOf(this.tbTypeName.Text) != -1)
				{
                    base.FailMessage("�ĵ�������ʧ�ܣ��ĵ���������Ѵ��ڣ�");
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
                        base.FailMessage(ex, "�ĵ�������ʧ�ܣ�");
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
