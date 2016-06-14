using SiteServer.Project.Model;
using System.Collections;

namespace SiteServer.Project.Core
{
	public interface IReplyDAO
	{
        void Insert(ReplyInfo replyInfo);

        void Delete(int replyID);

        void DeleteByApplyID(int applyID);

        ReplyInfo GetReplyInfo(int replyID);

        ReplyInfo GetReplyInfoByApplyID(int applyID);
	}
}
