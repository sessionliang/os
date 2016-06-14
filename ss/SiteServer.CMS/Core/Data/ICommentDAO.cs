using System;
using System.Collections;
using System.Data;
using BaiRong.Model;
using SiteServer.CMS.Model;
using System.Collections.Generic;

namespace SiteServer.CMS.Core
{
	public interface ICommentDAO
	{
        int Insert(int publishmentSystemID, CommentInfo commentInfo);

        void Delete(int publishmentSystemID, int nodeID, int contentID, List<int> commentIDList);

        void Delete(List<int> commentIDList);

        void Update(CommentInfo commentInfo, int publishmentSystemID);

        void AddGood(int publishmentSystemID, int commentID);

        void Check(List<int> commentIDList, int publishmentSystemID);

        void Recommend(List<int> commentIDList, bool isRecommend, int publishmentSystemID);

        CommentInfo GetCommentInfo(int commentContentID);

        List<CommentInfo> GetCommentInfoListChecked(int publishmentSystemID, int nodeID, int contentID);

        int GetCountChecked(int publishmentSystemID, int nodeID, int contentID);

        int GetCountChecked(int publishmentSystemID, DateTime begin, DateTime end);

        int GetCountChecked(int publishmentSystemID, int nodeID, DateTime begin, DateTime end);

        string GetSortFieldName();

        string GetSelectSqlString(int publishmentSystemID, int nodeID, int contentID);

        string GetSelectSqlString(int publishmentSystemID, List<int> channelIDList, string keyword, int searchDate, ETriState checkedState, ETriState channelState);

        string GetSelectSqlStringWithChecked(int publishmentSystemID, int nodeID, int contentID, int startNum, int totalNum, bool isRecommend, string whereString, string orderByString);

        string GetSelectIDStringWithChecked(int publishmentSystemID, int nodeID, int contentID);

        List<int> GetCommentIDListWithChecked(int publishmentSystemID, int contentID);

        IEnumerable GetDataSourceByUserName(int publishmentSystemID, string userName);

        ArrayList GetContentIDArrayListByCount(int publishmentSystemID);
	}
}
