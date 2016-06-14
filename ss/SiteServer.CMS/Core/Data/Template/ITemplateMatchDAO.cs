using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
    public interface ITemplateMatchDAO
	{
        void Insert(TemplateMatchInfo info);

        void Update(TemplateMatchInfo info);

        void Delete(int nodeID);

        TemplateMatchInfo GetTemplateMatchInfo(int nodeID);

        bool IsExists(int nodeID);

        Hashtable GetChannelTemplateIDHashtable(int publishmentSystemID);

        Hashtable GetContentTemplateIDHashtable(int publishmentSystemID);

        int GetChannelTemplateID(int nodeID);

        int GetContentTemplateID(int nodeID);

        string GetFilePath(int nodeID);

        string GetChannelFilePathRule(int nodeID);

        string GetContentFilePathRule(int nodeID);

        ArrayList GetAllFilePathByPublishmentSystemID(int publishmentSystemID);
	}
}
