using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class TypeAdd : BackgroundBasePage
	{
        protected TextBox tbTypeName;

        private int projectID;
        private int typeID;

        public static string GetShowPopWinStringToAdd(int projectID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加办件类型", "modal_typeAdd.aspx", arguments, 450, 300);
        }

        public static string GetShowPopWinStringToEdit(int projectID, int typeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            arguments.Add("TypeID", typeID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("修改办件类型", "modal_typeAdd.aspx", arguments, 450, 300);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.projectID = TranslateUtils.ToInt(base.Request.QueryString["ProjectID"]);
            this.typeID = TranslateUtils.ToInt(base.Request.QueryString["TypeID"]);

			if (!IsPostBack)
			{
                if (this.typeID > 0)
                {
                    TypeInfo typeInfo = DataProvider.TypeDAO.GetTypeInfo(this.typeID);
                    if (typeInfo != null)
                    {
                        this.tbTypeName.Text = typeInfo.TypeName;
                    }
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            TypeInfo typeInfo = null;
				
			if (this.typeID > 0)
			{
				try
				{
                    typeInfo = DataProvider.TypeDAO.GetTypeInfo(this.typeID);
                    if (typeInfo != null)
                    {
                        typeInfo.TypeName = this.tbTypeName.Text;
                    }
                    DataProvider.TypeDAO.Update(typeInfo);

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "办件类型修改失败！");
				}
			}
			else
			{
                ArrayList typeNameArrayList = DataProvider.TypeDAO.GetTypeNameArrayList(this.projectID);
                if (typeNameArrayList.IndexOf(this.tbTypeName.Text) != -1)
				{
                    base.FailMessage("办件类型添加失败，办件类型名称已存在！");
                }
				else
				{
					try
					{
                        typeInfo = new TypeInfo(0, this.tbTypeName.Text, this.projectID, 0);

                        DataProvider.TypeDAO.Insert(typeInfo);

						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "办件类型添加失败！");
					}
				}
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, string.Format("background_type.aspx?ProjectID={0}", this.projectID));
			}
		}
	}
}
