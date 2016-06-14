using System;
using System.Data;
using System.Collections;

using BaiRong.Model;

namespace SiteServer.CMS.Core
{
	public interface IVoteOperationDAO
	{
        void Insert(VoteOperationInfo operationInfo);

        int GetCount(int publishmentSystemID, int nodeID, int contentID);

        bool IsUserExists(int publishmentSystemID, int nodeID, int contentID, string userName);

        bool IsIPAddressExists(int publishmentSystemID, int nodeID, int contentID, string ipAddress);

        DataSet GetDataSet(int publishmentSystemID, int nodeID, int contentID);

        string GetCookieName(int publishmentSystemID, int nodeID, int contentID);
	}
}
