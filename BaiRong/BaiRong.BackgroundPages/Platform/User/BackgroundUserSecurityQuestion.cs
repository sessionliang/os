using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;


using System.Collections.Generic;

namespace BaiRong.BackgroundPages
{
    public class BackgroundUserSecurityQuestion : BackgroundBasePage
    {

        public TextBox tbKeyword;

        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button Delete;

        public string keyWord;

        public static string GetRedirectUrl()
        {
            return PageUtils.GetPlatformUrl(string.Format("background_UserSecurityQuestion.aspx"));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                List<int> UserSecurityQuestionIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserSecurityQuestionIDCollection"));
                try
                {
                    foreach (int userSecurityQuestionID in UserSecurityQuestionIDList)
                    {
                        BaiRongDataProvider.UserSecurityQuestionDAO.Delete(userSecurityQuestionID);
                    }

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "ɾ���ܱ�����", string.Empty);

                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.SortField = "ID";
            this.spContents.SortMode = SortMode.DESC;
            this.spContents.ItemsPerPage = 15;

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserMessage, "�ܱ��������", AppManager.User.Permission.Usercenter_Setting);

                keyWord = PageUtils.FilterSql(base.GetQueryString("Keyword"));
                this.tbKeyword.Text = keyWord;

                string backgroundUrl = BackgroundUserSecurityQuestion.GetRedirectUrl();

                this.btnAdd.Attributes.Add("onclick", Modal.UserSecurityQuestionAdd.GetRedirectUrlToAdd(this.PageUrl));


                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}?Delete=True", backgroundUrl), "UserSecurityQuestionIDCollection", "UserSecurityQuestionIDCollection", "��ѡ����Ҫɾ������Ϣģ�壡", "�˲�����ɾ����ѡ��Ϣģ�壬ȷ����"));

                this.spContents.SelectCommand = BaiRongDataProvider.UserSecurityQuestionDAO.GetSqlString(keyWord);
                this.spContents.DataBind();
            }
        }

        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                Literal ltIndex = (Literal)e.Item.FindControl("ltIndex");
                Literal ltQuestion = (Literal)e.Item.FindControl("ltQuestion");
                Literal ltIsEnable = (Literal)e.Item.FindControl("ltIsEnable");


                ltIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltQuestion.Text = TranslateUtils.EvalString(e.Item.DataItem, "Question");
                ltIsEnable.Text = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsEnable")) ? "����" : "������";


                LinkButton ltEdit = (LinkButton)e.Item.FindControl("ltEdit");

                ltEdit.Text = "�༭";
                ltEdit.OnClientClick = Modal.UserSecurityQuestionAdd.GetRedirectUrlToEdit(TranslateUtils.EvalInt(e.Item.DataItem, "ID"), BackgroundUserSecurityQuestion.GetRedirectUrl());
            }
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = string.Format("{0}?keyword={1}", BackgroundUserSecurityQuestion.GetRedirectUrl(), this.tbKeyword.Text);
                }
                return this._pageUrl;
            }
        }
    }
}
