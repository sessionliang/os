using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Text;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.BackgroundPages.MLib
{
    public class MlibConfig : MLibBackgroundBasePage
    {
        public Literal ltlAdminRoles;
        public TextBox tbCheckLevel;
        public TextBox tbCheckedValues;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!string.IsNullOrEmpty(Request["action"]))
            {
                this.GetType().GetMethod(Request["action"]).Invoke(this, null);
            }

            if (!IsPostBack)
            {
                tbCheckLevel.Text = DataProvider.MlibDAO.GetConfigAttr("CheckLevel");
            }

        }


        public bool HasCheckLevelPermissions(string roleName, int checkLevel)
        {
            string where = " RoleName='" + roleName + "' and CheckLevel=" + checkLevel;

            var RC = DataProvider.MlibDAO.GetRoleCheckLevel(where);
            if (RC.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool HasNodePermissions(string roleName, int checkLevel, int nodeId)
        {
            string where = " RoleName='" + roleName + "' and CheckLevel=" + checkLevel;

            var RC = DataProvider.MlibDAO.GetRoleCheckLevel(where);
            if (RC.Tables[0].Rows.Count > 0)
            {
                string where1 = " RCID=" + RC.Tables[0].Rows[0]["ID"].ToString() + " and nodeid=" + nodeId;

                var nodeIds = DataProvider.MlibDAO.GetRCNode(where1);
                if (nodeIds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void GetTreeData()
        {
            var adminNames = RoleManager.GetAllRoles();


            var Nodes = DataProvider.NodeDAO.GetNodeInfoArrayListByParentID(base.PublishmentSystemID, base.PublishmentSystemID);
            int maxCheckLevel = TranslateUtils.ToInt(Request.QueryString["checklevel"]);
            if (maxCheckLevel == 0)
            {
                maxCheckLevel = TranslateUtils.ToInt(DataProvider.MlibDAO.GetConfigAttr("CheckLevel"));
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            for (int i = 0; i < adminNames.Length; i++)
            {
                string roleName = adminNames[i];
                if (roleName == "Administrator" || roleName == "ConsoleAdministrator" || roleName == "SystemAdministrator")
                {
                    continue;
                }
                if (sb.Length != 1)
                {
                    sb.AppendLine(" ,");
                }
                sb.AppendLine(" {");
                sb.AppendLine("    \"id\":\"" + roleName + "\",");
                sb.AppendLine("    \"text\":\"" + roleName + "\",");
                sb.AppendLine("    \"state\":\"closed\",");
                sb.AppendLine("    \"children\":[");

                for (int j = 0; j <= maxCheckLevel; j++)
                {
                    if (j != 0)
                    {
                        sb.AppendLine("                      ,");
                    }
                    sb.AppendLine("                     {\"id\":\"" + roleName + "_" + j + "\",");
                    if (j == 0)
                    {
                        sb.AppendLine("                     \"text\":\"引用\",");
                    }
                    else
                    {
                        sb.AppendLine("                     \"text\":\"" + Number2Chinese(j) + "审\",");
                    }
                    //if (HasCheckLevelPermissions(roleName, j))
                    //{
                    //    sb.AppendLine("                     \"checked\":\"true\",");
                    //}
                    sb.AppendLine("                     \"state\":\"closed\",");
                    sb.AppendLine("                     \"children\":[");
                    for (int k = 0; k < Nodes.Count; k++)
                    {
                        NodeInfo nodeInfo = (NodeInfo)Nodes[k];
                        if (k != 0)
                        {
                            sb.AppendLine("                                     ,");
                        }
                        sb.AppendLine("                                     {\"id\":\"" + roleName + "_" + j + "_" + nodeInfo.NodeID + "\",");
                        if (HasNodePermissions(roleName, j, nodeInfo.NodeID))
                        {
                            sb.AppendLine("                                      \"checked\":\"true\",");
                        }
                        sb.AppendLine("                                      \"text\":\"" + nodeInfo.NodeName + "\"}");

                    }
                    sb.AppendLine("                                  ]");
                    sb.AppendLine("                     }");
                }
                sb.AppendLine("        ]");
                sb.AppendLine(" }");

            }
            sb.AppendLine("]");

            Response.Write(sb.ToString());
            Response.End();
        }


        public override void Submit_OnClick(object sender, EventArgs E)
        {
            int checkLevel = TranslateUtils.ToInt(tbCheckLevel.Text);
            DataProvider.MlibDAO.UpdateConfigAttr("CheckLevel", checkLevel.ToString());

            var selections = tbCheckedValues.Text.Split(',');


            var adminNames = RoleManager.GetAllRoles();
            var Nodes = DataProvider.NodeDAO.GetNodeInfoArrayListByParentID(base.PublishmentSystemID, base.PublishmentSystemID);
            int maxCheckLevel = TranslateUtils.ToInt(tbCheckLevel.Text);

            foreach (var roleName in adminNames)
            {
                if (!HasStrInArr(selections, roleName))
                {

                    DataProvider.MlibDAO.UpdateRoleCheckLevel(roleName, new string[0]);
                }
                string checkedCLs = "";
                for (int i = 0; i <= maxCheckLevel; i++)
                {
                    if (HasStrInArr(selections, roleName + "_" + i))
                    {
                        checkedCLs += "," + i;
                    }
                }
                if (checkedCLs.Length > 0)
                {
                    checkedCLs = checkedCLs.Substring(1);
                    DataProvider.MlibDAO.UpdateRoleCheckLevel(roleName, checkedCLs.Split(','));
                }

                for (int i = 0; i <= maxCheckLevel; i++)
                {
                    if (HasStrInArr(selections, roleName + "_" + i))
                    {

                        var RC = DataProvider.MlibDAO.GetRoleCheckLevel(" RoleName='" + roleName + "' and CheckLevel=" + i);
                        int rcid = TranslateUtils.ToInt(RC.Tables[0].Rows[0]["ID"].ToString());

                        string checkedNodeIds = "";
                        foreach (NodeInfo nodeInfo in Nodes)
                        {
                            if (HasStrInArr(selections, roleName + "_" + i + "_" + nodeInfo.NodeID))
                            {
                                checkedNodeIds += "," + nodeInfo.NodeID;
                            }
                        }

                        if (checkedNodeIds.Length > 0)
                        {
                            checkedNodeIds = checkedNodeIds.Substring(1);
                            DataProvider.MlibDAO.UpdateNodeAdminRoles(rcid, checkedNodeIds.Split(','));
                        }
                    }
                }

            }
        }

        private bool HasStrInArr(string[] selections, string startWith)
        {
            foreach (var item in selections)
            {
                if (item.StartsWith(startWith))
                {
                    return true;
                }
            }
            return false;
        }

        public void SaveRole(string[] adminNames)
        {

            foreach (string item in adminNames)
            {
                var checklevels = Request.Form["ddlCheckLevel" + item];
                if (!string.IsNullOrEmpty(checklevels))
                {
                    var checklevelArray = checklevels.Split(',');
                    DataProvider.MlibDAO.UpdateRoleCheckLevel(item, checklevelArray);

                    foreach (var cl in checklevelArray)
                    {
                        var nodeids = Request.Form["ddlNode" + item + "_" + cl];
                        if (!string.IsNullOrEmpty(nodeids))
                        {
                            var nodeidArray = nodeids.Split(',');

                            var ds = DataProvider.MlibDAO.GetRoleCheckLevel("RoleName='" + item + "' and CheckLevel=" + cl);

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataProvider.MlibDAO.UpdateNodeAdminRoles(TranslateUtils.ToInt(ds.Tables[0].Rows[0]["ID"].ToString()), nodeidArray);
                            }
                        }

                    }
                }

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
