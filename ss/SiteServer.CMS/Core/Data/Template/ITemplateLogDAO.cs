using SiteServer.CMS.Model;
using System.Collections;
using System.Collections.Generic;

namespace SiteServer.CMS.Core
{
	public interface ITemplateLogDAO
	{
        void Insert(TemplateLogInfo logInfo);

        string GetSelectCommend(int publishmentSystemID, int templateID);

        string GetTemplateContent(int logID);

        Dictionary<int, string> GetLogIDWithNameDictionary(int publishmentSystemID, int templateID);

        void Delete(ArrayList logIDArrayList);
	}
}
