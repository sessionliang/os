using System.Collections;
using SiteServer.CMS.Model;
using BaiRong.Model;

namespace SiteServer.CMS.Core
{
	public interface IGovPublicIdentifierSeqDAO
	{
        int GetSequence(int publishmentSystemID, int nodeID, int departmentID, int addYear, int ruleSequence);
	}
}
