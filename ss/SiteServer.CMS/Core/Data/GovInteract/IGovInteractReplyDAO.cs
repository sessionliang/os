using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
	public interface IGovInteractReplyDAO
	{
        void Insert(GovInteractReplyInfo replyInfo);

        void Delete(int replyID);

        void DeleteByContentID(int publishmentSystemID, int contentID);

        GovInteractReplyInfo GetReplyInfo(int replyID);

        GovInteractReplyInfo GetReplyInfoByContentID(int publishmentSystemID, int contentID);
	}
}
