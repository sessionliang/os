using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using System.Collections;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.Project.Model;
using System.Text;

using System;

namespace SiteServer.Project.Core
{
	public class ProjectManager
	{
        public static void AddDefaultTypeInfos(int projectID)
        {
            TypeInfo typeInfo = new TypeInfo(0, "求决", projectID, 0);
            DataProvider.TypeDAO.Insert(typeInfo);
            typeInfo = new TypeInfo(0, "举报", projectID, 0);
            DataProvider.TypeDAO.Insert(typeInfo);
            typeInfo = new TypeInfo(0, "投诉", projectID, 0);
            DataProvider.TypeDAO.Insert(typeInfo);
            typeInfo = new TypeInfo(0, "咨询", projectID, 0);
            DataProvider.TypeDAO.Insert(typeInfo);
            typeInfo = new TypeInfo(0, "建议", projectID, 0);
            DataProvider.TypeDAO.Insert(typeInfo);
            typeInfo = new TypeInfo(0, "感谢", projectID, 0);
            DataProvider.TypeDAO.Insert(typeInfo);
            typeInfo = new TypeInfo(0, "其他", projectID, 0);
            DataProvider.TypeDAO.Insert(typeInfo);
        }

        public static ArrayList GetFirstDepartmentIDArrayList()
        {
            return BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByParentID(0);
        }

        public static string GetDirectoryPath(int projectID)
        {
            ProjectInfo projectInfo = DataProvider.ProjectDAO.GetProjectInfo(projectID);
            string directoryPath = PathUtils.MapPath(string.Format("~/project_documents/{0}/{1}/{2}", projectInfo.AddDate.Year, projectInfo.AddDate.Month, projectInfo.ProjectName.Trim()));
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            return directoryPath;
        }

        public static string GetDirectoryUploadPath(int projectID)
        {
            ProjectInfo projectInfo = DataProvider.ProjectDAO.GetProjectInfo(projectID);
            string directoryPath = PathUtils.MapPath(string.Format("~/project_documents/{0}/{1}/{2}/upload", projectInfo.AddDate.Year, projectInfo.AddDate.Month, projectInfo.ProjectName.Trim()));
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            return directoryPath;
        }

        public static string GetDirectoryUrl(int projectID)
        {
            ProjectInfo projectInfo = DataProvider.ProjectDAO.GetProjectInfo(projectID);
            return PageUtils.ParseNavigationUrl(string.Format("~/project_documents/{0}/{1}/{2}", projectInfo.AddDate.Year, projectInfo.AddDate.Month, projectInfo.ProjectName.Trim()));
        }

        public static string GetDirectoryUploadUrl(int projectID)
        {
            ProjectInfo projectInfo = DataProvider.ProjectDAO.GetProjectInfo(projectID);
            return PageUtils.ParseNavigationUrl(string.Format("~/project_documents/{0}/{1}/{2}/upload", projectInfo.AddDate.Year, projectInfo.AddDate.Month, projectInfo.ProjectName.Trim()));
        }

        public static string GetTypeName(int typeID)
        {
            if (typeID > 0)
            {
                return DataProvider.TypeDAO.GetTypeName(typeID);
            }
            return string.Empty;
        }

        public static string GetTypeName(int typeID, Hashtable typeHashtable)
        {
            string typeName = typeHashtable[typeID] as string;
            if (string.IsNullOrEmpty(typeName))
            {
                typeName = ProjectManager.GetTypeName(typeID);
                typeHashtable[typeID] = typeName;
            }
            return typeName;
        }

        public static string GetProjectName(int projectID)
        {
            ProjectInfo projectInfo = DataProvider.ProjectDAO.GetProjectInfo(projectID);
            if (projectInfo != null)
            {
                return projectInfo.ProjectName;
            }
            return string.Empty;
        }

        public static void RegisterClientScriptBlock(Page page, string key, string script)
        {
            if (!IsStartupScriptRegistered(page, key))
            {
                page.RegisterClientScriptBlock(key, script);
            }
        }

        public static bool IsStartupScriptRegistered(Page page, string key)
        {
            return page.IsStartupScriptRegistered(key);
        }

        public static string GetUserNameCollection(string userNameAM, string userNamePM, string userNameCollection)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(userNameAM))
            {
                builder.Append("<code>AM:" + AdminManager.GetDisplayName(userNameAM, false) + "</code>,");
            }
            if (!string.IsNullOrEmpty(userNamePM))
            {
                builder.Append("<code>PM:" + AdminManager.GetDisplayName(userNamePM, false) + "</code>,");
            }

            foreach (string userName in userNameCollection.Split(','))
            {
                if (!string.IsNullOrEmpty(userName) && userName != userNameAM && userName != userNamePM)
                {
                    builder.Append(AdminManager.GetDisplayName(userName, false) + ",");
                }
            }

            if (builder.Length > 0)
            {
                builder.Length--;
            }
            return builder.ToString();
        }

        public static string GetCleanDomain(string url)
        {
            string domain = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                domain = url.Trim().ToLower();
                domain = StringUtils.ReplaceStartsWith(domain, "http://", string.Empty);
            }
            return domain;
        }

        public static string GetInvoiceSN(EInvoiceType invoiceType)
        {
            return string.Format("INV-{0}{1}-{2}", invoiceType == EInvoiceType.SiteServer ? "S" : "Y", DateTime.Now.ToString("yyyyMMdd"), StringUtils.GetRandomInt(1000, 9999));
        }

        public static string GetOrderSN(EOrderType orderType)
        {
            string t = string.Empty;
            if (orderType == EOrderType.Aliyun_Moban)
            {
                t = "Y";
            }
            else if (orderType == EOrderType.Aliyun_Software)
            {
                t = "P";
            }
            else if (orderType == EOrderType.Taobao_Service)
            {
                t = "S";
            }
            return string.Format("ORD-{0}{1}-{2}", t, DateTime.Now.ToString("yyyyMMdd"), StringUtils.GetRandomInt(1000, 9999));
        }

        public static string GetAccountSN(EAccountType accountType)
        {
            string t = string.Empty;
            if (accountType == EAccountType.SiteYun_Partner)
            {
                t = "Y";
            }
            else
            {
                t = "S";
            }
            return string.Format("ACC-{0}{1}-{2}", t, DateTime.Now.ToString("yyyyMMdd"), StringUtils.GetRandomInt(1000, 9999));
        }

        public static string GetContractSN(EContractType contractType)
        {
            string t = string.Empty;
            if (contractType == EContractType.SiteYun_Order)
            {
                t = "Y";
            }
            else
            {
                t = "S";
            }
            return string.Format("CNR-{0}{1}-{2}", t, DateTime.Now.ToString("yyyyMMdd"), StringUtils.GetRandomInt(1000, 9999));
        }
	}
}
