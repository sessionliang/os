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
            return PageUtilityPF.GetOpenWindowString("修改用户等级", "modal_userLevelAdd.aspx", arguments, 620, 550);
        }

        public static string GetOpenWindowStringToAdd(int levelID, bool isCredits)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("LevelID", levelID.ToString());
            arguments.Add("IsCredits", isCredits.ToString());
            arguments.Add("IsAdd", true.ToString());
            return PageUtilityPF.GetOpenWindowString("添加用户等级", "modal_userLevelAdd.aspx", arguments, 620, 550);
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
                            base.FailMessage("用户等级修改失败，用户等级名称已存在！");
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
                                base.FailMessage(string.Format("用户等级修改失败，积分范围必须在 {0} 和 {1} 之间！", levelInfo.MinNum, levelInfo.MaxNum));
                                return;
                            }
                            if (minNum == 0 && minNum != 0)
                            {
                                base.FailMessage("用户等级修改失败，必须保留一个最低积分为 0 的用户等级！");
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

                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改用户等级", string.Format("用户等级:{0}", this.LevelName.Text));

                        JsUtils.OpenWindow.CloseModalPage(Page);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "用户等级修改失败！");
                    }
                }
                else
                {
                    if (BaiRongDataProvider.UserLevelDAO.IsExists("", this.LevelName.Text))
                    {
                        base.FailMessage("用户等级添加失败，用户等级名称已存在！");
                        return;
                    }
                    else
                    {
                        int minNum = TranslateUtils.ToInt(this.MinNum.Text);
                        int maxNum = TranslateUtils.ToInt(this.MaxNum.Text);

                        if (this.isCredits && !BaiRongDataProvider.UserLevelDAO.IsCreditsValid("", minNum, maxNum))
                        {
                            base.FailMessage("用户等级添加失败，积分范围必须在已有的某个用户等级积分范围之内！");
                            return;
                        }

                        UserLevelInfo levelInfo = new UserLevelInfo(0, "", this.LevelName.Text, EUserLevelType.Credits, minNum, maxNum, TranslateUtils.ToInt(this.Stars.Text), this.Color.Text, string.Empty);

                        try
                        {
                            BaiRongDataProvider.UserLevelDAO.Insert(levelInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加用户等级", string.Format("用户等级:{0}", this.LevelName.Text));

                            JsUtils.OpenWindow.CloseModalPage(Page);
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(ex, "用户等级添加失败！" + ex.ToString());
                        }
                    }
                }

            }
        }

	}
}
