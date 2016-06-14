using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using SiteServer.Project.Model;
using System.Text;
using BaiRong.Model;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class MobanSetting : BackgroundBasePage
	{
        protected DropDownList ddlIsAliyun;
        protected DropDownList ddlIsInitializationForm;

        private ArrayList idArrayList;

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public static string GetShowPopWinString()
        {
            NameValueCollection arguments = new NameValueCollection();
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("��������", "modal_mobanSetting.aspx", arguments, "IDCollection", "��ѡ����Ҫ���õ�ģ�壡", 500, 350);
        }

        public static string GetShowPopWinString(int mobanID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("IDCollection", mobanID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("��������", "modal_mobanSetting.aspx", arguments, 500, 350);
        }
        
		public void Page_Load(object sender, EventArgs E)
		{
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["IDCollection"]);

			if (!IsPostBack)
			{
                EBooleanUtils.AddListItems(this.ddlIsAliyun, "����������ģ��", "δ����������ģ��");
                this.ddlIsAliyun.Items.Insert(0, new ListItem("<���ֲ���>", string.Empty));

                EBooleanUtils.AddListItems(this.ddlIsInitializationForm, "�ѿ�ͨ��ʼ����", "δ��ͨ��ʼ����");
                this.ddlIsInitializationForm.Items.Insert(0, new ListItem("<���ֲ���>", string.Empty));
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
            try
            {
                foreach (int mobanID in this.idArrayList)
                {
                    MobanInfo mobanInfo = DataProvider.MobanDAO.GetMobanInfo(mobanID);
                    if (!string.IsNullOrEmpty(this.ddlIsAliyun.SelectedValue))
                    {
                        mobanInfo.IsAliyun = TranslateUtils.ToBool(this.ddlIsAliyun.SelectedValue);
                    }
                    if (!string.IsNullOrEmpty(this.ddlIsInitializationForm.SelectedValue))
                    {
                        mobanInfo.IsInitializationForm = TranslateUtils.ToBool(this.ddlIsInitializationForm.SelectedValue);
                    }
                    DataProvider.MobanDAO.Update(mobanInfo);
                }

                isChanged = true;
            }
			catch(Exception ex)
			{
                base.FailMessage(ex, ex.Message);
			    isChanged = false;
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}

	}
}
