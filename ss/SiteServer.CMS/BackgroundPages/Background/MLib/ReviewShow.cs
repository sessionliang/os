using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Model.MLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.BackgroundPages.MLib
{
    public class ReviewShow : MLibBackgroundBasePage
    {
        public Literal ltlPageTitle;

        public Literal ltlTabAction;
        public string ReturnUrl;
        private ETableStyle tableStyle;
        private string tableName;
        ArrayList relatedIdentities;
        private int contentID;
        private ContentInfo contentInfo;
        private NodeInfo nodeInfo;

        public Button btnSubmit;

        public Button btnSubmit1;




        public Literal ltlTitle;
        public Literal ltlPostTime;
        public Literal ltlAuthor;
        public Literal ltlSource;
        public Literal ltlContent;

        public Literal ltlRecord;

        public PlaceHolder phSHYJ;
        public TextBox tbSHYJ;


        private int MaxCheckLevel;

        public void Page_Load(object sender, EventArgs E)
        {
            int nodeID = TranslateUtils.ToInt(Request.QueryString["nodeid"]);
            this.contentID = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.ReturnUrl = "ReviewList.aspx?PublishmentSystemID=" + this.PublishmentSystemID + "&NodeID=" + nodeID;

            MaxCheckLevel = TranslateUtils.ToInt(DataProvider.MlibDAO.GetConfigAttr("CheckLevel"));

            nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);

            this.contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
            SubmissionInfo submissionInfo = DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);

            if (submissionInfo.CheckedLevel > MaxCheckLevel || (submissionInfo.CheckedLevel == MaxCheckLevel && submissionInfo.IsChecked))
            {
                btnSubmit.Text = "终审通过";
                btnSubmit1.Text = "终审不通过";
            }
            else
            {
                //审核选项
                if (contentInfo.IsChecked)
                {
                    btnSubmit.Text =Number2Chinese(submissionInfo.CheckedLevel + 1) + "审通过";
                    btnSubmit1.Text = Number2Chinese(submissionInfo.CheckedLevel + 1) + "审不通过";

                }
                else
                {
                    btnSubmit.Text =Number2Chinese(submissionInfo.CheckedLevel - 1) + "审通过";
                    btnSubmit1.Text =Number2Chinese(submissionInfo.CheckedLevel - 1) + "审不通过";
                }
            }

            #region 其他阶段稿件链接

            var contentIDs = DataProvider.MlibDAO.GetContentIDsBySubmissionID(contentInfo.ReferenceID);
            for (int i = 0; i < contentIDs.Tables[0].Rows.Count; i++)
            {
                int itemCheckLevel = TranslateUtils.ToInt(contentIDs.Tables[0].Rows[i]["checkedLevel"].ToString());

                string statusText = "";
                if (submissionInfo.PassDate != null && contentIDs.Tables[0].Rows[i]["IsChecked"].ToString() == "True" && itemCheckLevel == MaxCheckLevel && i == contentIDs.Tables[0].Rows.Count - 1)
                {
                    statusText = "终审稿";
                }
                else
                {
                    if (itemCheckLevel >= MaxCheckLevel)
                    {

                        statusText = Number2Chinese(itemCheckLevel - 1) + "审稿";
                    }
                    else
                    {
                        statusText = Number2Chinese(itemCheckLevel) + "审稿";
                    }
                }

                ltlTabAction.Text += string.Format("<input type=\"button\" class=\"{4}\" onclick=\"location.href='ReviewShow.aspx?PublishmentSystemID={0}&nodeid={1}&id={2}';\" value=\"{3}\"/>",
                    base.PublishmentSystemID,
                    TranslateUtils.ToInt(contentIDs.Tables[0].Rows[i]["NodeID"].ToString()),
                    TranslateUtils.ToInt(contentIDs.Tables[0].Rows[i]["ID"].ToString()),
                    itemCheckLevel == 0 ? "草稿" : statusText,
                    contentInfo.ID == TranslateUtils.ToInt(contentIDs.Tables[0].Rows[i]["ID"].ToString()) ? "btn btn-info" : "btn"
                    );

            }

            #region 是否显示编辑按钮,审核按钮
            bool showEditLink = false;


            //如果没有权限 不显示编辑按钮

            if (submissionInfo.CheckedLevel > MaxCheckLevel || (submissionInfo.CheckedLevel == MaxCheckLevel && submissionInfo.IsChecked))
            {
                showEditLink = this.HasNodePermissions((MaxCheckLevel).ToString(), nodeID.ToString());
            }
            else
            {
                if (contentInfo.IsChecked)
                {
                    showEditLink = this.HasNodePermissions((submissionInfo.CheckedLevel + 1).ToString(), nodeID.ToString());
                }
                else
                {
                    showEditLink = this.HasNodePermissions((submissionInfo.CheckedLevel - 1).ToString(), nodeID.ToString());
                }
            }

            //如果终审通过 不显示编辑按钮
            showEditLink = !showEditLink ? false : submissionInfo.PassDate == null;


            if (showEditLink)
            {
                ltlTabAction.Text += string.Format("<input type=\"button\" class=\"btn\" onclick=\"location.href='ReviewEdit.aspx?PublishmentSystemID={0}&nodeid={1}&id={2}';\" value=\"编辑\"/>",
                    base.PublishmentSystemID,
                        TranslateUtils.ToInt(contentIDs.Tables[0].Rows[contentIDs.Tables[0].Rows.Count - 1]["NodeID"].ToString()),
                        TranslateUtils.ToInt(contentIDs.Tables[0].Rows[contentIDs.Tables[0].Rows.Count - 1]["ID"].ToString())
                    );
            }

            btnSubmit.Visible = showEditLink;
            btnSubmit1.Visible = showEditLink;
            phSHYJ.Visible = showEditLink;
            #endregion

            #endregion



            if (!IsPostBack)
            {
                this.ltlPageTitle.Text = "稿件查看";

                ltlTitle.Text = contentInfo.Title;
                ltlPostTime.Text = contentInfo.AddDate.ToString("yyyy-MM-dd");
                ltlAuthor.Text = contentInfo.GetExtendedAttribute("Author");
                ltlSource.Text = contentInfo.GetExtendedAttribute("Source");
                ltlContent.Text = contentInfo.GetExtendedAttribute("Content");






                var contentStatusRecord = DataProvider.MlibDAO.GetContentIDsBySubmissionID1(contentInfo.ReferenceID);
                StringBuilder sb = new StringBuilder();

                var dv = new DataView(contentStatusRecord.Tables[0]);
                for (int i = 0; i < dv.Count; i++)
                {
                    var cInfo = new ContentInfo(dv[i]);
                    sb.AppendLine("<tr>");
                    if (submissionInfo.PassDate != null & cInfo.IsChecked & cInfo.CheckedLevel == MaxCheckLevel && i == dv.Count - 1)
                    {
                        sb.AppendLine("    <td>终审通过</td>");
                    }
                    else
                    {
                        sb.AppendLine("    <td>" + GetStatusText(cInfo.IsChecked, cInfo.CheckedLevel) + "</td>");
                    }

                    sb.AppendLine("    <td>" + cInfo.LastEditDate.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                    sb.AppendLine("    <td>" + cInfo.LastEditUserName + "</td>");
                    sb.AppendLine("    <td>" + cInfo.GetExtendedAttribute("teseeee") + "</td>");
                    sb.AppendLine("</tr>");
                }

                ltlRecord.Text = sb.ToString();
            }
        }


        private string GetStatusText(bool isChecked, int CheckedLevel)
        {
            string returnVal;
            if (CheckedLevel == 0)
            {
                returnVal = "未审核";
            }
            else
            {
                string StageText, PassText, nextStage;
                if (CheckedLevel > MaxCheckLevel)
                {
                    StageText = Number2Chinese(MaxCheckLevel - 1) + "审";
                    PassText = "通过";
                    nextStage = ",等待" + Number2Chinese(MaxCheckLevel) + "审";

                }
                else if (CheckedLevel == MaxCheckLevel)
                {
                    if (isChecked)
                    {
                        StageText = Number2Chinese(MaxCheckLevel - 1) + "审";
                        PassText = "通过";
                        nextStage = ",等待" + Number2Chinese(MaxCheckLevel) + "审";
                    }
                    else
                    {

                        StageText = Number2Chinese(CheckedLevel) + "审";
                        PassText = "不通过";
                        nextStage = ",等待" + Number2Chinese(CheckedLevel - 1) + "审重审";
                    }
                }
                else
                {
                    StageText = Number2Chinese(CheckedLevel) + "审";
                    if (isChecked)
                    {
                        PassText = "通过";
                        nextStage = ",等待" + Number2Chinese(CheckedLevel + 1) + "审";
                    }
                    else
                    {
                        PassText = "<span style=\"color:#f00;\">退稿</span>";
                        nextStage = "";
                    }
                }

                returnVal = StageText + "" + PassText + nextStage;
            }
            return returnVal;
        }



        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, contentID);

                try
                {
                    contentInfo.LastEditUserName = AdminManager.Current.UserName;
                    contentInfo.LastEditDate = DateTime.Now;
                    if (contentInfo.IsChecked)
                    {
                        contentInfo.CheckedLevel++;
                    }
                    else
                    {
                        contentInfo.CheckedLevel--;
                    }
                    contentInfo.SetExtendedAttribute("teseeee", tbSHYJ.Text);
                    contentInfo.IsChecked = true;
                    DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfo);

                    SubmissionInfo submissionInfo = DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);
                    submissionInfo.CheckedLevel = contentInfo.CheckedLevel;
                    submissionInfo.IsChecked = contentInfo.IsChecked;
                    if (submissionInfo.CheckedLevel >= MaxCheckLevel)
                    {
                        submissionInfo.PassDate = DateTime.Now;
                    }
                    DataProvider.MlibDAO.Update(submissionInfo);

                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("内容审核失败：{0}", ex.Message));
                    //LogUtils.SystemErrorLogWrite(ex);
                    return;
                }

                PageUtils.Redirect("ReviewList.aspx?PublishmentSystemID=" + this.PublishmentSystemID + "&NodeID=" + nodeInfo.NodeID);
            }
        }
        public void Submit1_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, contentID);

                try
                {
                    contentInfo.LastEditUserName = AdminManager.Current.UserName;
                    contentInfo.LastEditDate = DateTime.Now;
                    contentInfo.SetExtendedAttribute("teseeee", tbSHYJ.Text);
                    if (contentInfo.CheckedLevel > MaxCheckLevel)
                    {
                        contentInfo.CheckedLevel = MaxCheckLevel;
                    }
                    else
                    {
                        if (contentInfo.IsChecked)
                        {
                            contentInfo.CheckedLevel++;
                        }
                        else
                        {
                            contentInfo.CheckedLevel--;
                        }
                    }
                    contentInfo.IsChecked = false;
                    DataProvider.ContentDAO.Insert(this.tableName, base.PublishmentSystemInfo, contentInfo);

                    SubmissionInfo submissionInfo = DataProvider.MlibDAO.GetSubmissionInfo(contentInfo.ReferenceID);
                    submissionInfo.CheckedLevel = contentInfo.CheckedLevel;
                    submissionInfo.IsChecked = contentInfo.IsChecked;
                    DataProvider.MlibDAO.Update(submissionInfo);

                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("内容审核失败：{0}", ex.Message));
                    //LogUtils.SystemErrorLogWrite(ex);
                    return;
                }

                PageUtils.Redirect("ReviewList.aspx?PublishmentSystemID=" + this.PublishmentSystemID + "&NodeID=" + nodeInfo.NodeID);
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
