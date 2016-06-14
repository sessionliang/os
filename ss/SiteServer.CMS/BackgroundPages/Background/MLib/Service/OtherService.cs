using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using System.Text;
using System.Web.UI;

namespace SiteServer.CMS.BackgroundPages.MLib.Service
{
    public class OtherService : Page
    {

        public void Page_Load(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["action"]))
            {
                this.GetType().GetMethod(Request["action"]).Invoke(this, null);

            }
            else
            {

                MLibBackgroundBasePage mbbp = new MLibBackgroundBasePage();
                int MaxCheckLevel = TranslateUtils.ToInt(DataProvider.MlibDAO.GetConfigAttr("CheckLevel"));
                var CheckLevelArr = new ArrayList();


                var idds = DataProvider.MlibDAO.GetContentIDsAll();
                string contentids = "0";
                for (int i = 0; i < idds.Tables[0].Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(idds.Tables[0].Rows[i]["ID"].ToString()))
                    {
                        contentids += "," + idds.Tables[0].Rows[i]["ID"].ToString();
                    }
                }
                int publishmentSystemId = TranslateUtils.ToInt(Request.QueryString["publishmentSystemId"]);

                StringBuilder sb = new StringBuilder();
                var nodelist = DataProvider.NodeDAO.GetNodeInfoArrayListByParentID(publishmentSystemId, publishmentSystemId);
                foreach (NodeInfo nodeInfo in nodelist)
                {
                    for (int i = 0; i < MaxCheckLevel; i++)
                    {
                        if (mbbp.HasNodePermissions((i + 1).ToString(), nodeInfo.NodeID.ToString()))
                        {
                            CheckLevelArr.Add((i + 1).ToString());
                        }
                    }
                    if (CheckLevelArr.Count != 0 || mbbp.HasNodePermissions("0", nodeInfo.NodeID.ToString()))
                    {
                        string cmd = " and  ml_Submission.PassDate is null and ( 1=0 ";
                        foreach (string item in CheckLevelArr)
                        {
                            cmd += " or (ml_Submission.CheckedLevel=" + (int.Parse(item) - 1) + " and ml_Submission.IsChecked='True')";
                            cmd += " or (ml_Submission.CheckedLevel=" + (int.Parse(item) + 1) + " and ml_Submission.IsChecked='False')";

                        }
                        cmd += ") ";

                        var count = DataProvider.MlibDAO.GetSubmissionCount(" ml_Content.ID in (" + contentids + ") and ml_Submission.CheckedLevel>0 and ml_Content.NodeID=" + nodeInfo.NodeID + cmd).ToString();



                        sb.AppendLine("<tr treeItemLevel=\"3\">");
                        sb.AppendLine("    <td align=\"left\" nowrap>");
                        sb.AppendLine("        <img align=\"absmiddle\" src=\"/SiteFiles/bairong/icons/tree/empty.gif\" /><img align=\"absmiddle\" src=\"/SiteFiles/bairong/icons/tree/empty.gif\" /><img align=\"absmiddle\" src=\"/SiteFiles/bairong/icons/tree/empty.gif\" /><a href=\"javascript:;\" ><img align=\"absmiddle\" border=\"0\" src=\"/SiteFiles/bairong/icons/tree/folder.gif\" /></a>&nbsp;<a href='ReviewList.aspx?PublishmentSystemID=" + publishmentSystemId + "&NodeID=" + nodeInfo.NodeID + "' isLink='true' onclick='fontWeightLink(this)' target='content'>" + nodeInfo.NodeName + "</a>&nbsp;&nbsp;<span style=\"font-size:8pt;font-family:arial\" class=\"gray\">(" + count + ")</span>");
                        sb.AppendLine("    </td>");
                        sb.AppendLine("</tr>");
                    }
                    CheckLevelArr.Clear();
                }


                Response.Write(sb.ToString());
            }
        }

        public void GetDraftCount()
        {
            MLibBackgroundBasePage mbbp = new MLibBackgroundBasePage();

            if (mbbp.HasCheckLevelPermissions("1"))
            {
                var count = DataProvider.MlibDAO.GetSubmissionCount("ml_Submission.CheckedLevel=0 and ml_Submission.IsChecked='True'");

                Response.Write(count.ToString());
                Response.End();
            }
            else
            {

                Response.Write("-1");
                Response.End();
            }

        }
    }
}
