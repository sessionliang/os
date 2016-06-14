using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;


namespace BaiRong.BackgroundPages.Modal
{
    public class UserLevelAdd : BackgroundBasePage
	{
        protected PlaceHolder phCreditsAddTips;
        protected PlaceHolder phCreditsEditTips;
        protected PlaceHolder phCredits;

        protected TextBox LevelName;
        protected TextBox MinNum;
        protected TextBox MaxNum;
        protected TextBox Stars;
        protected TextBox Color;

        private int levelID = 0;
        private bool isCredits = false;
        private bool isAdd = false;

        public static string GetOpenWindowStringToEdit(int levelID, bool isCredits)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("LevelID", levelID.ToString());
            arguments.Add("IsCredits", isCredits.ToString());
            arguments.Add("IsAdd", false.ToString());
            return PageUtilityPF.GetOpenWindowString("�޸��û��ȼ�", "modal_userLevelAdd.aspx", arguments, 620, 550);
        }

        public static string GetOpenWindowStringToAdd(int levelID, bool isCredits)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("LevelID", levelID.ToString());
            arguments.Add("IsCredits", isCredits.ToString());
            arguments.Add("IsAdd", true.ToString());
            return PageUtilityPF.GetOpenWindowString("����û��ȼ�", "modal_userLevelAdd.aspx", arguments, 620, 550);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.levelID = TranslateUtils.ToInt(base.GetQueryString("LevelID"));
            this.isCredits = TranslateUtils.ToBool(base.GetQueryString("IsCredits"));
            this.isAdd = TranslateUtils.ToBool(base.GetQueryString("IsAdd"));

            if (!IsPostBack)
            {
                if (this.isCredits)
                {
                    this.phCredits.Visible = true;
                    if (!this.isAdd)
                    {
                        this.phCreditsAddTips.Visible = false;
                        this.phCreditsEditTips.Visible = true;
                    }
                    else
                    {
                        this.phCreditsAddTips.Visible = true;
                        this.phCreditsEditTips.Visible = false;
                    }
                }
                else
                {
                    this.phCreditsAddTips.Visible = this.phCreditsEditTips.Visible = this.phCredits.Visible = false;
                }

                if (!this.isAdd)
                {
                    UserLevelInfo levelInfo = UserLevelManager.GetLevelInfo("", this.levelID);
                    if (levelInfo != null)
                    {
                        this.LevelName.Text = levelInfo.LevelName;
                        this.MinNum.Text = levelInfo.MinNum.ToString();
                        this.MaxNum.Text = levelInfo.MaxNum.ToString();
                        this.Stars.Text = levelInfo.Stars.ToString();
                        this.Color.Text = levelInfo.Color;
                    }
                }
                else
                {
                    if (this.levelID > 0)
                    {
                        UserLevelInfo levelInfo = UserLevelManager.GetLevelInfo("", this.levelID);
                        if (levelInfo != null)
                        {
                            this.MinNum.Text = levelInfo.MinNum.ToString();
                            this.MaxNum.Text = levelInfo.MaxNum.ToString();
                            this.Stars.Text = levelInfo.Stars.ToString();
                            this.Color.Text = levelInfo.Color;
                        }
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (!this.isAdd)
                {
                    try
                    {
                        UserLevelInfo levelInfo = UserLevelManager.GetLevelInfo("", this.levelID);
                        if (levelInfo.LevelName != this.LevelName.Text && BaiRongDataProvider.UserLevelDAO.IsExists("", this.LevelName.Text))
                        {
                            base.FailMessage("�û��ȼ��޸�ʧ�ܣ��û��ȼ������Ѵ��ڣ�");
                            return;
                        }

                        int oldMinNum = levelInfo.MinNum;
                        int oldMaxNum = levelInfo.MaxNum;
                        if (this.isCredits)
                        {
                            int minNum = TranslateUtils.ToInt(this.MinNum.Text);
                            int maxNum = TranslateUtils.ToInt(this.MaxNum.Text);
                            if (minNum < oldMinNum || maxNum > oldMaxNum)
                            {
                                base.FailMessage(string.Format("�û��ȼ��޸�ʧ�ܣ����ַ�Χ������ {0} �� {1} ֮�䣡", levelInfo.MinNum, levelInfo.MaxNum));
                                return;
                            }
                            if (minNum == 0 && minNum != 0)
                            {
                                base.FailMessage("�û��ȼ��޸�ʧ�ܣ����뱣��һ����ͻ���Ϊ 0 ���û��ȼ���");
                                return;
                            }
                            levelInfo.MinNum = minNum;
                            levelInfo.MaxNum = maxNum;
                        }

                        levelInfo.LevelName = this.LevelName.Text;
                        levelInfo.Stars = TranslateUtils.ToInt(this.Stars.Text);
                        levelInfo.Color = this.Color.Text;

                        if (this.isCredits)
                        {
                            BaiRongDataProvider.UserLevelDAO.UpdateWithCredits("", levelInfo, oldMinNum, oldMaxNum);
                        }
                        else
                        {
                            BaiRongDataProvider.UserLevelDAO.UpdateWithoutCredits("", levelInfo);
                        }

                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "�޸��û��ȼ�", string.Format("�û��ȼ�:{0}", this.LevelName.Text));

                        JsUtils.OpenWindow.CloseModalPage(Page);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "�û��ȼ��޸�ʧ�ܣ�");
                    }
                }
                else
                {
                    if (BaiRongDataProvider.UserLevelDAO.IsExists("", this.LevelName.Text))
                    {
                        base.FailMessage("�û��ȼ����ʧ�ܣ��û��ȼ������Ѵ��ڣ�");
                        return;
                    }
                    else
                    {
                        int minNum = TranslateUtils.ToInt(this.MinNum.Text);
                        int maxNum = TranslateUtils.ToInt(this.MaxNum.Text);

                        if (this.isCredits && !BaiRongDataProvider.UserLevelDAO.IsCreditsValid("", minNum, maxNum))
                        {
                            base.FailMessage("�û��ȼ����ʧ�ܣ����ַ�Χ���������е�ĳ���û��ȼ����ַ�Χ֮�ڣ�");
                            return;
                        }

                        UserLevelInfo levelInfo = new UserLevelInfo(0, "", this.LevelName.Text, EUserLevelType.Credits, minNum, maxNum, TranslateUtils.ToInt(this.Stars.Text), this.Color.Text, string.Empty);

                        try
                        {
                            BaiRongDataProvider.UserLevelDAO.Insert(levelInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "����û��ȼ�", string.Format("�û��ȼ�:{0}", this.LevelName.Text));

                            JsUtils.OpenWindow.CloseModalPage(Page);
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(ex, "�û��ȼ����ʧ�ܣ�" + ex.ToString());
                        }
                    }
                }

            }
        }

	}
}
