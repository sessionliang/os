using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using SiteServer.WCM.Core;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.WCM.BackgroundPages.Modal
{
	public class GovInteractTypeAdd : BackgroundBasePage
	{
        protected TextBox tbTypeName;

        private int nodeID;
        private int typeID;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            return PageUtilityWCM.GetOpenWindowString("添加办件类型", "modal_govInteractTypeAdd.aspx", arguments, 450, 220);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int nodeID, int typeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("TypeID", typeID.ToString());
            return PageUtilityWCM.GetOpenWindowString("修改办件类型", "modal_govInteractTypeAdd.aspx", arguments, 450, 220);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.nodeID = TranslateUtils.ToInt(base.Request.QueryString["NodeID"]);
            this.typeID = TranslateUtils.ToInt(base.Request.QueryString["TypeID"]);

			if (!IsPostBack)
			{
                if (this.typeID > 0)
                {
                    GovInteractTypeInfo typeInfo = DataProvider.GovInteractTypeDAO.GetTypeInfo(this.typeID);
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
            GovInteractTypeInfo typeInfo = null;
				
			if (this.typeID > 0)
			{
				try
				{
                    typeInfo = DataProvider.GovInteractTypeDAO.GetTypeInfo(this.typeID);
                    if (typeInfo != null)
                    {
                        typeInfo.TypeName = this.tbTypeName.Text;
                    }
                    DataProvider.GovInteractTypeDAO.Update(typeInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改办件类型", string.Format("办件类型:{0}", typeInfo.TypeName));

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "办件类型修改失败！");
				}
			}
			else
			{
                ArrayList typeNameArrayList = DataProvider.GovInteractTypeDAO.GetTypeNameArrayList(this.nodeID);
                if (typeNameArrayList.IndexOf(this.tbTypeName.Text) != -1)
				{
                    base.FailMessage("办件类型添加失败，办件类型名称已存在！");
                }
				else
				{
					try
					{
                        typeInfo = new GovInteractTypeInfo(0, this.tbTypeName.Text, this.nodeID, base.PublishmentSystemID, 0);

                        DataProvider.GovInteractTypeDAO.Insert(typeInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "添加办件类型", string.Format("办件类型:{0}", typeInfo.TypeName));

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
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetWCMUrl(string.Format("background_govInteractType.aspx?PublishmentSystemID={0}&NodeID={1}", base.PublishmentSystemID, this.nodeID)));
			}
		}
	}
}
