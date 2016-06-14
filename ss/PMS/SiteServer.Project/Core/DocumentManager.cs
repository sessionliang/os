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
	public class DocumentManager
	{
        public static string GetDirectoryPathByCategory(int typeID)
        {
            DocTypeInfo typeInfo = DataProvider.DocTypeDAO.GetTypeInfo(typeID);
            string directoryPath = PathUtils.MapPath(string.Format("~/type_documents/{0}", typeInfo.TypeName.Trim()));
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            return directoryPath;
        }

        public static string GetDirectoryPathByContract(int contractID)
        {
            ContractInfo contractInfo = DataProvider.ContractDAO.GetContractInfo(contractID);
            string directoryPath = PathUtils.MapPath(string.Format("~/contract_documents/{0}", contractInfo.SN.Trim()));
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            return directoryPath;
        }

        public static string GetFilePath(int documentID)
        {
            string filePath = string.Empty;
            DocumentInfo documentInfo = DataProvider.DocumentDAO.GetDocumentInfo(documentID);
            if (documentInfo != null)
            {
                if (documentInfo.DocumentType == EDocumentType.Category)
                {
                    filePath = PathUtils.Combine(DocumentManager.GetDirectoryPathByCategory(documentInfo.TypeID), documentInfo.FileName);
                }
                else if (documentInfo.DocumentType == EDocumentType.Contract)
                {
                    filePath = PathUtils.Combine(DocumentManager.GetDirectoryPathByContract(documentInfo.ContractID), documentInfo.FileName);
                }
                
            }
            return filePath;
        }

        public static string GetDirectoryUrlByCategory(int typeID)
        {
            DocTypeInfo typeInfo = DataProvider.DocTypeDAO.GetTypeInfo(typeID);
            if (typeInfo != null)
            {
                return PageUtils.ParseNavigationUrl(string.Format("~/type_documents/{0}", typeInfo.TypeName.Trim()));
            }
            return string.Empty;
        }

        public static string GetDirectoryUrlByContract(int contractID)
        {
            ContractInfo contractInfo = DataProvider.ContractDAO.GetContractInfo(contractID);
            if (contractInfo != null)
            {
                return PageUtils.ParseNavigationUrl(string.Format("~/contract_documents/{0}", contractInfo.SN.Trim()));
            }
            return string.Empty;
        }

        public static void Delete(int documentID)
        {
            string filePath = DocumentManager.GetFilePath(documentID);
            FileUtils.DeleteFileIfExists(filePath);
            DataProvider.DocumentDAO.Delete(documentID);
        }
	}
}
