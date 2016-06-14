using System.Collections;
using BaiRong.Model;
using SiteServer.CMS.Model;
using BaiRong.Core;
using System.Collections.Generic;

namespace SiteServer.CMS.Core
{
	public interface ITemplateDAO
	{
		int Insert(TemplateInfo templateInfo, string templateContent);

		void Update(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo, string templateContent);

		void SetDefault(int publishmentSystemID, int templateID);

		void Delete(int publishmentSystemID, int templateID);

		void CreateDefaultTemplateInfo(int publishmentSystemID);

		string GetImportTemplateName(int publishmentSystemID, string templateName);

        Dictionary<ETemplateType, int> GetCountDictionary(int publishmentSystemID);

		IEnumerable GetDataSourceByType(int publishmentSystemID, ETemplateType type);

		IEnumerable GetDataSource(int publishmentSystemID, string searchText, string templateTypeString);

		ArrayList GetTemplateIDArrayListByType(int publishmentSystemID, ETemplateType type);

		ArrayList GetTemplateInfoArrayListByType(int publishmentSystemID, ETemplateType type);

        ArrayList GetTemplateInfoArrayListOfFile(int publishmentSystemID);

		ArrayList GetTemplateInfoArrayListByPublishmentSystemID(int publishmentSystemID);

		ArrayList GetTemplateNameArrayList(int publishmentSystemID, ETemplateType templateType);

        ArrayList GetLowerRelatedFileNameArrayList(int publishmentSystemID, ETemplateType templateType);

        int GetTemplateUseCount(int publishmentSystemID, int templateID, ETemplateType templateType, bool isDefault);

        ArrayList GetNodeIDArrayList(TemplateInfo templateInfo);

        Dictionary<int, TemplateInfo> GetTemplateInfoDictionaryByPublishmentSystemID(int publishmentSystemID);

        //by sofuny 20151106
        TemplateInfo GetTemplateByURLType(int publishmentSystemID, ETemplateType type, string createdFileFullName);

        /// <summary>
        /// by sofuny 20151119
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="type">模板类型</param>
        /// <param name="tID">模板ID</param>
        /// <returns></returns>
        TemplateInfo GetTemplateByTemplateID(int publishmentSystemID, ETemplateType type, string tID);
	}
}
