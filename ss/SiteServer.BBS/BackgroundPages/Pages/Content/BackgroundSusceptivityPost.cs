using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;
using System.Collections;
using BaiRong.Controls;
using System.Web.UI.WebControls;
using System.Data;
using BaiRong.Core;
using System.Web.UI;
using SiteServer.BBS.Model;


namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundSusceptivityPost : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;

        protected Button btnPass;//通过按钮
        protected Button btnDel;//删除按钮

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_susceptivityPost.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            this.spContents.ItemsPerPage = 20;
            this.spContents.ConnectionString = DataProvider.ConnectionString;

            this.spContents.SelectCommand = GetSelectCommend();
            this.spContents.SortField = "AddDate";
            this.spContents.SortMode = SortMode.DESC;

            btnPass.Attributes.Add("onclick", "return checkstate('myform','审核通过');");
            btnDel.Attributes.Add("onclick", "return checkstate('myform','删除');");

            if (base.GetQueryString("Delete") != null)
            {
                int id = base.GetIntQueryString("ID");
                try
                {
                    PostInfo postInfo = DataProvider.PostDAO.GetPostInfo(base.PublishmentSystemID, id);
                    UserMessageManager.SendSystemMessage(postInfo.UserName, "【敏感词】您发表的帖子因包含敏感内容已被管理员删除。");
                    DataProvider.PostDAO.Delete(base.PublishmentSystemID, id);
                    base.SuccessMessage("成功删除敏感贴");
                    base.AddWaitAndRedirectScript(BackgroundSusceptivityPost.GetRedirectUrl(base.PublishmentSystemID));
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除敏感贴失败，{0}", ex.Message));
                }
            }
            if (base.GetQueryString("Pass") != null)
            {
                int id = base.GetIntQueryString("ID");
                try
                {
                    DataProvider.PostDAO.Pass(base.PublishmentSystemID, id);
                    PostInfo postInfo = DataProvider.PostDAO.GetPostInfo(base.PublishmentSystemID, id);
                    UserMessageManager.SendSystemMessage(postInfo.UserName, "【敏感词】您发表的帖子已通过审核。");
                    base.SuccessMessage("审核通过");
                    base.AddWaitAndRedirectScript(BackgroundSusceptivityPost.GetRedirectUrl(base.PublishmentSystemID));
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("审核失败，{0}", ex.Message));
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Content, "敏感贴审核", AppManager.BBS.Permission.BBS_Content);

                spContents.DataBind();
            }
        }

        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlNum = e.Item.FindControl("ltlNum") as Literal;
                Literal ltlID = e.Item.FindControl("ltlID") as Literal;
                Literal ltlThread = e.Item.FindControl("ltlThread") as Literal;
                Literal ltlKeyWords = e.Item.FindControl("ltlKeyWords") as Literal;
                Literal ltlUser = e.Item.FindControl("ltlUser") as Literal;
                Literal ltlTime = e.Item.FindControl("ltlTime") as Literal;
                Literal ltlLookUp = e.Item.FindControl("ltlLookUp") as Literal;
                Literal ltlPassUrl = e.Item.FindControl("ltlPassUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                int id = (int)DataBinder.Eval(e.Item.DataItem, "ID");

                ltlID.Text = ConvertHelper.GetString(DataBinder.Eval(e.Item.DataItem, "ID"));
                ltlNum.Text = ConvertHelper.GetString(e.Item.ItemIndex + 1);
                ltlThread.Text = GetThreadTitleByID((int)DataBinder.Eval(e.Item.DataItem, "ThreadID"));
                ltlKeyWords.Text = StringUtils.MaxLengthText(GetKeyWords((int)DataBinder.Eval(e.Item.DataItem, "ID")), 15);
                ltlUser.Text = (string)DataBinder.Eval(e.Item.DataItem, "UserName");
                ltlTime.Text = ConvertHelper.GetString(DataBinder.Eval(e.Item.DataItem, "AddDate"));
                ltlLookUp.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">查看</a>", Modal.LookupTheKeywordsPost.GetOpenWindowString(base.PublishmentSystemID, id, "Look"));

                ltlPassUrl.Text = string.Format(@"<a href=""{0}&ID={1}&Pass=True"" onclick=""javascript:return confirm('确认结果为通过吗？');"">通过</a>", BackgroundSusceptivityPost.GetRedirectUrl(base.PublishmentSystemID), id);
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}&ID={1}&Delete=True"" onclick=""javascript:return confirm('确认要删除吗？');"">删除</a>", BackgroundSusceptivityPost.GetRedirectUrl(base.PublishmentSystemID), id);
            }
        }

        public string GetSelectCommend()
        {
            StringBuilder sbString = new StringBuilder();
            sbString.Append("SELECT * FROM bbs_Post WHERE ID=0 OR");
            IList<PostInfo> postList = DataProvider.PostDAO.GetNotPassedPost(base.PublishmentSystemID);
            if (postList.Count > 0)
            {
                foreach (PostInfo postInfo in postList)
                {
                    sbString.Append(" ID=" + postInfo.ID);
                    sbString.Append(" OR");
                }
            }
            string sqlString = sbString.ToString().Replace("OR", ",").TrimEnd(',').Replace(",", "OR");
            return sqlString;
        }
        public string GetKeyWords(int ID)
        {
            int auditGrade = 2;
            StringBuilder sbKeyword = new StringBuilder();
            PostInfo info = DataProvider.PostDAO.GetPostInfo(base.PublishmentSystemID, ID);
            string strContent = info.Content;
            IList<KeywordsFilterInfo> List = DataProvider.KeywordsFilterDAO.GetKeywordsByGrade(base.PublishmentSystemID, auditGrade);
            foreach (KeywordsFilterInfo infoAudit in List)
            {
                if (strContent.Contains(infoAudit.Name))
                {
                    sbKeyword.Append(infoAudit.Name);
                    sbKeyword.Append(",");
                }
            }
            return sbKeyword.ToString().TrimEnd(',');
        }

        public string GetThreadTitleByID(int ID)
        {
            ThreadInfo info = DataProvider.ThreadDAO.GetThreadInfo(base.PublishmentSystemID, ID);
            return info.Title;
        }

        protected void btnDel_Click(object sender, EventArgs e)//删除
        {
            for (int i = 0; i < this.rptContents.Items.Count; i++)
            {
                CheckBox cbSelect = (CheckBox)rptContents.Items[i].FindControl("chk_ID");
                if (cbSelect.Checked)
                {
                    Literal ltlID = (Literal)rptContents.Items[i].FindControl("ltlID");
                    PostInfo postInfo = DataProvider.PostDAO.GetPostInfo(base.PublishmentSystemID, int.Parse(ltlID.Text));
                    UserMessageManager.SendSystemMessage(postInfo.UserName, "【敏感词】您发表的帖子因包含敏感内容已被管理员删除。");
                    DataProvider.PostDAO.Delete(base.PublishmentSystemID, int.Parse(ltlID.Text));
                }
            }
            base.SuccessMessage("删除成功!");
            base.AddWaitAndRedirectScript(BackgroundSusceptivityPost.GetRedirectUrl(base.PublishmentSystemID));
        }
        protected void btnPass_Click(object sender, EventArgs e)//通过
        {
            for (int i = 0; i < rptContents.Items.Count; i++)
            {
                CheckBox cbSelect = (CheckBox)rptContents.Items[i].FindControl("chk_ID");
                if (cbSelect.Checked)
                {
                    Literal ltlID = (Literal)rptContents.Items[i].FindControl("ltlID");
                    DataProvider.PostDAO.Pass(base.PublishmentSystemID, TranslateUtils.ToInt(ltlID.Text));
                    PostInfo postInfo = DataProvider.PostDAO.GetPostInfo(base.PublishmentSystemID, TranslateUtils.ToInt(ltlID.Text));
                    UserMessageManager.SendSystemMessage(postInfo.UserName, "【敏感词】您发表的帖子已通过审核。");
                }
            }
            base.SuccessMessage("审核成功!");
            base.AddWaitAndRedirectScript(BackgroundSusceptivityPost.GetRedirectUrl(base.PublishmentSystemID));
        }
    }
}
