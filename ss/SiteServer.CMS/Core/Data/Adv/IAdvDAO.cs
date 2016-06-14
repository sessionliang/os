using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
    public interface IAdvDAO
	{
        void Insert(AdvInfo advInfo);

        void Update(AdvInfo advInfo);

        void Delete(int advID, int publishmentSystemID);

        void Delete(ArrayList advIDArrayList, int publishmentSystemID);

        AdvInfo GetAdvInfo(int advID, int publishmentSystemID);

        bool IsExists(string advName, int publishmentSystemID);

        IEnumerable GetDataSource(int publishmentSystemID);

        IEnumerable GetDataSourceByAdAreaID(int adAreaID, int publishmentSystemID);

        ArrayList GetAdvNameArrayList(int publishmentSystemID);

        ArrayList GetAdvIDArrayList(int adAreaIDint,int publishmentSystemID);

        ArrayList GetAdvInfoArrayList(ETemplateType templateType, int adAreaID, int publishmentSystemID, int nodeID, int fileTemplateID);
	}
}
