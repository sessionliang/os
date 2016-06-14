using System;
using System.Data;
using System.Collections;
using SiteServer.BBS.Model;

namespace SiteServer.BBS
{
	public interface IPollDAO
	{
        int Insert(PollInfo info, ArrayList pollItems);

        void Update(PollInfo info, ArrayList pollItems);

        void InsertPollUser(PollUserInfo pollUser);

        void AddPollNum(ArrayList pollItemIDArrayList);

        void DeletePollInfo(int pollID);

        void DeletePollItems(int pollID);

        PollInfo GetPollInfo(int threadID);

        ArrayList GetPollItemInfoArrayList(int pollID);

        PollItemInfo GetPollItemInfo(DataRow dataRow);

        int GetTotalPollNum(int pollID);

        bool IsUserExists(int pollID, string userName);

        ArrayList GetPollItemIDArrayList(int pollID, string userName);
	}
}
