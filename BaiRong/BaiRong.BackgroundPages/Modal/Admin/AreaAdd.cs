using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;


using BaiRong.Core.Data.Provider;

namespace BaiRong.BackgroundPages.Modal
{
	public class AreaAdd : BackgroundBasePage
	{
        public TextBox AreaName;
        public PlaceHolder phParentID;
        public DropDownList ParentID;

        private int areaID = 0;
        private string returnUrl = string.Empty;
        private bool[] isLastNodeArray;

        public static string GetOpenWindowStringToAdd(string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtilityPF.GetOpenWindowString("�������", "modal_areaAdd.aspx", arguments, 460, 360);
        }

        public static string GetOpenWindowStringToEdit(int areaID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("AreaID", areaID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtilityPF.GetOpenWindowString("�޸�����", "modal_areaAdd.aspx", arguments, 460, 360);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.areaID = base.GetIntQueryString("AreaID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = PageUtils.GetPlatformUrl("background_area.aspx");
            }

			if (!IsPostBack)
			{
                if (this.areaID == 0)
                {
                    this.ParentID.Items.Add(new ListItem("<���ϼ�����>", "0"));

                    ArrayList areaIDArrayList = AreaManager.GetAreaIDArrayList();
                    int count = areaIDArrayList.Count;
                    this.isLastNodeArray = new bool[count];
                    foreach (int theAreaID in areaIDArrayList)
                    {
                        AreaInfo areaInfo = AreaManager.GetAreaInfo(theAreaID);
                        ListItem listitem = new ListItem(this.GetTitle(areaInfo.AreaID, areaInfo.AreaName, areaInfo.ParentsCount, areaInfo.IsLastNode), theAreaID.ToString());
                        this.ParentID.Items.Add(listitem);
                    }
                }
                else
                {
                    this.phParentID.Visible = false;
                }

                if (this.areaID != 0)
                {
                    AreaInfo areaInfo = AreaManager.GetAreaInfo(this.areaID);

                    this.AreaName.Text = areaInfo.AreaName;
                    this.ParentID.SelectedValue = areaInfo.ParentID.ToString();
                }
			}
		}

        public string GetTitle(int areaID, string areaName, int parentsCount, bool isLastNode)
        {
            string str = "";
            if (isLastNode == false)
            {
                this.isLastNodeArray[parentsCount] = false;
            }
            else
            {
                this.isLastNodeArray[parentsCount] = true;
            }
            for (int i = 0; i < parentsCount; i++)
            {
                if (this.isLastNodeArray[i])
                {
                    str = String.Concat(str, "��");
                }
                else
                {
                    str = String.Concat(str, "��");
                }
            }
            if (isLastNode)
            {
                str = String.Concat(str, "��");
            }
            else
            {
                str = String.Concat(str, "��");
            }
            str = String.Concat(str, areaName);
            return str;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {
                if (this.areaID == 0)
                {
                    AreaInfo areaInfo = new AreaInfo();
                    areaInfo.AreaName = this.AreaName.Text;
                    areaInfo.ParentID = TranslateUtils.ToInt(this.ParentID.SelectedValue);

                    BaiRongDataProvider.AreaDAO.Insert(areaInfo);
                }
                else
                {
                    AreaInfo areaInfo = AreaManager.GetAreaInfo(this.areaID);

                    areaInfo.AreaName = this.AreaName.Text;
                    areaInfo.ParentID = TranslateUtils.ToInt(this.ParentID.SelectedValue);

                    BaiRongDataProvider.AreaDAO.Update(areaInfo);
                }

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "ά��������Ϣ");

                base.SuccessMessage("�������óɹ���");
                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "��������ʧ�ܣ�");
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
            }
        }
	}
}
