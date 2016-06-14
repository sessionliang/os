using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.Pages.MLibManage
{

    public class DraftContent : SystemBasePage
    {
        public TextBox Keyword;
        public HtmlInputText start;
        public HtmlInputText end;
        public PlaceHolder phContents;
        public Repeater dlContents;
        public SqlPager spContents;
        public Literal ltlCount;
        public PlaceHolder phNoData;
        public Literal ltlContentType;
        public string homeUrl;

        public void Page_Load(object sender, EventArgs e)
        {
            if (ConfigManager.Additional.IsUseMLib == false)
            {
                Response.Write("<script>alert('投稿中心尚未开启.');history.go(-1);</script>");
                Response.End();
            }
            //if (string.IsNullOrEmpty(ConfigManager.Additional.MLibPublishmentSystemIDs))
            //{
            //    Response.Write("<script>alert('投稿中心尚未开启.');history.go(-1);</script>");
            //    Response.End();
            //    return;
            //}

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetUniqueUserCenter();
            if (publishmentSystemInfo != null)
                homeUrl = publishmentSystemInfo.PublishmentSystemDir;

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
                {
                    ArrayList idsArrayList = TranslateUtils.StringCollectionToArrayList(Request.QueryString["IDsCollection"]);
                    if (idsArrayList.Count > 0)
                        //删除草稿
                        DataProvider.MLibDraftContentDAO.Delete(idsArrayList);
                }
                ContentBind();
            }
        }

        public void ContentBind()
        {
            #region
            string keyWord = Keyword.Text.Trim();
            string startInfo = start.Value;
            string endInfo = end.Value;

            this.spContents.ControlToPaginate = this.dlContents;
            this.dlContents.ItemDataBound += new RepeaterItemEventHandler(dlContents_ItemDataBound);

            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            StringBuilder whereString = new StringBuilder();

            whereString.AppendFormat(" WHERE AddUserName = '{0}'", UserManager.Current.UserName);
            if (!string.IsNullOrEmpty(keyWord))
            {
                whereString.AppendFormat(" AND Title like '%{0}%'", keyWord);
            }
            if (!string.IsNullOrEmpty(startInfo) && !string.IsNullOrEmpty(endInfo))
            {
                whereString.Append(" AND (AddDate>='" + startInfo + "' AND AddDate<='" + endInfo + "') ");
            }

            this.spContents.SelectCommand = "SELECT * FROM bairong_MLibDraftContent " + whereString.ToString();
            this.spContents.SortField = ContentAttribute.AddDate;
            this.spContents.SortMode = SortMode.DESC;
            #endregion


            this.spContents.DataBind();

            this.ltlCount.Text = this.spContents.TotalCount.ToString();

            if (this.spContents.TotalCount > 0)
            {
                this.phContents.Visible = true;
                this.phNoData.Visible = false;
            }
            else
            {
                this.phContents.Visible = false;
                this.phNoData.Visible = true;
            }

        }

        private void dlContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Literal ltlID = e.Item.FindControl("ltlID") as Literal;
                Literal ltlContent = e.Item.FindControl("ltlContent") as Literal;
                Literal ltlChannel = e.Item.FindControl("ltlChannel") as Literal;
                Literal ltlState = e.Item.FindControl("ltlState") as Literal;
                Literal ltlDateTime = e.Item.FindControl("ltlDateTime") as Literal;
                Literal ltlOperate = e.Item.FindControl("ltlOperate") as Literal;


                ContentInfo contentInfo = new ContentInfo(e.Item.DataItem);

                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(contentInfo.PublishmentSystemID);

                if (contentInfo != null)
                {
                    ltlContent.Text = contentInfo.Title;

                    string nodeName = publishmentSystemInfo.PublishmentSystemName + " > " + NodeManager.GetNodeNameNavigation(contentInfo.PublishmentSystemID, contentInfo.NodeID);

                    ltlChannel.Text = nodeName;

                    ltlState.Text = "草稿";

                    ltlDateTime.Text = contentInfo.AddDate.ToString();
                    ltlOperate.Text = GetOperateText(contentInfo.IsChecked, contentInfo.CheckedLevel, contentInfo.ID);
                }
                else
                {
                    e.Item.Visible = false;
                }
            }
        }

        public string GetOperateText(bool isChecked, int CheckedLevel, int id)
        {
            string returnVal = "";


            returnVal = "<a href='submissionEdit.aspx?ID=" + id + "'>编辑</a>&nbsp;<a href='draftcontent.aspx?Delete=True&IDsCollection=" + id + "'>删除</a>";


            return returnVal;
        }

        private string GetStatusText(bool isChecked, int CheckedLevel)
        {


            //0,false 草稿未提交
            //0,true  草稿已提交,初审待审核
            //1,false 初审不通过
            //1,true  初审通过,二审待审核
            //2,false 二审不通过
            //2,true  二审通过,三审待审核
            //3,false 三审不通过
            //3,true  三审通过..............................

            string returnVal;
            if (CheckedLevel == 0)
            {
                returnVal = "草稿已提交,初审待审核";
            }
            else
            {
                string StageText = Number2Chinese(CheckedLevel) + "审";
                string PassText = isChecked ? "已通过" : "<span style=\"color:#f00;\">未通过</span>";
                returnVal = StageText + "" + PassText;
            }
            return returnVal;
        }


        public string GetDeleteUrl(string idsCollection)
        {
            return string.Format(@"contents.aspx?type={1}&Delete=True&IDsCollection={2}", "", idsCollection);
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                // PageUtils.Redirect(string.Format("contents.aspx?PublishmentSystemID={0}&Keyword={1}&start={2}&end={3}", this.ddlPublishmentSystemID.SelectedValue, this.tbKeyword.Text, this.start.Value, this.end.Value));
                ContentBind();
            }
        }



        public string Number2Chinese(int n)
        {

            int MaxCheckLevel = TranslateUtils.ToInt(DataProvider.MlibDAO.GetConfigAttr("CheckLevel"));
            if (n == MaxCheckLevel)
            {
                return "终";
            }
            var chinese = new string[] { "", "初", "二", "三", "四", "五", "六", "七", "八", "九" };
            return chinese[n];
        }
    }
}
