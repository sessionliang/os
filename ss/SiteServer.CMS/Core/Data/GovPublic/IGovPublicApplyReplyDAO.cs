using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
	public interface IGovPublicApplyReplyDAO
	{
        void Insert(GovPublicApplyReplyInfo replyInfo);

        void Delete(int replyID);

        void DeleteByApplyID(int applyID);

        GovPublicApplyReplyInfo GetReplyInfo(int replyID);

        GovPublicApplyReplyInfo GetReplyInfoByApplyID(int applyID);
	}
}
