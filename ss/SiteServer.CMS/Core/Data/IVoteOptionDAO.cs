using System;
using System.Data;
using System.Collections;

using BaiRong.Model;

namespace SiteServer.CMS.Core
{
	public interface IVoteOptionDAO
	{
        void Insert(ArrayList voteOptionInfoArrayList);

		void AddVoteNum(int optionID);

		void Delete(int publishmentSystemID, int nodeID, int contentID);

        void UpdateVoteOptionInfoArrayList(int publishmentSystemID, int nodeID, int contentID, ArrayList voteOptionInfoArrayList);

        ArrayList GetVoteOptionInfoArrayList(int publishmentSystemID, int nodeID, int contentID);

        int GetTotalVoteNum(int publishmentSystemID, int nodeID, int contentID);
	}
}
