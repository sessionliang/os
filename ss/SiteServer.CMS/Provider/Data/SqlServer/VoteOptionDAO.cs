using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class VoteOptionDAO : DataProviderBase, IVoteOptionDAO
	{
        private const string SQL_SELECT_ALL = "SELECT OptionID, PublishmentSystemID, NodeID, ContentID, Title, ImageUrl, NavigationUrl, VoteNum FROM siteserver_VoteOption WHERE PublishmentSystemID = @PublishmentSystemID AND NodeID = @NodeID AND ContentID = @ContentID";

        private const string SQL_SELECT_VOTE_NUM = "SELECT SUM(VoteNum) AS TotalNum FROM siteserver_VoteOption WHERE PublishmentSystemID = @PublishmentSystemID AND NodeID = @NodeID AND ContentID = @ContentID";

        private const string SQL_UPDATE_VOTE_NUM = "UPDATE siteserver_VoteOption SET VoteNum = VoteNum + 1 WHERE OptionID = @OptionID";

        private const string SQL_DELETE_ALL = "DELETE FROM siteserver_VoteOption WHERE PublishmentSystemID = @PublishmentSystemID AND NodeID = @NodeID AND ContentID = @ContentID";

        private const string PARM_OPTION_ID = "@OptionID";
        private const string PARM_PUBLISHMENTSYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_CONTENT_ID = "@ContentID";
		private const string PARM_TITLE = "@Title";
		private const string PARM_IMAGE_URL = "@ImageUrl";
		private const string PARM_NAVIGATION_URL = "@NavigationUrl";
		private const string PARM_VOTE_NUM = "@VoteNum";

		public void Insert(ArrayList voteOptionInfoArrayList)
		{
            string sqlString = "INSERT INTO siteserver_VoteOption (PublishmentSystemID, NodeID, ContentID, Title, ImageUrl, NavigationUrl, VoteNum) VALUES (@PublishmentSystemID, @NodeID, @ContentID, @Title, @ImageUrl, @NavigationUrl, @VoteNum)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_VoteOption (OptionID, PublishmentSystemID, NodeID, ContentID, Title, ImageUrl, NavigationUrl, VoteNum) VALUES (siteserver_VoteOption_SEQ.NEXTVAL, @PublishmentSystemID, @NodeID, @ContentID, @Title, @ImageUrl, @NavigationUrl, @VoteNum)";
            }

			using (IDbConnection conn = this.GetConnection()) 
			{
				conn.Open();
				using (IDbTransaction trans = conn.BeginTransaction()) 
				{
					try 
					{
                        foreach (VoteOptionInfo optionInfo in voteOptionInfoArrayList)
						{
							IDbDataParameter[] insertItemParms = new IDbDataParameter[]
							{
								this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, optionInfo.PublishmentSystemID),
                                this.GetParameter(PARM_NODE_ID, EDataType.Integer, optionInfo.NodeID),
                                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, optionInfo.ContentID),
								this.GetParameter(PARM_TITLE, EDataType.NVarChar, 255, optionInfo.Title),
								this.GetParameter(PARM_IMAGE_URL, EDataType.VarChar, 200, optionInfo.ImageUrl),
								this.GetParameter(PARM_NAVIGATION_URL, EDataType.VarChar, 200, optionInfo.NavigationUrl),
								this.GetParameter(PARM_VOTE_NUM, EDataType.Integer, optionInfo.VoteNum)
							};

                            this.ExecuteNonQuery(trans, sqlString, insertItemParms);
						}				

						trans.Commit();
					}
					catch
					{
						trans.Rollback();
						throw;
					}
				}
			}
		}

		public void AddVoteNum(int optionID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_OPTION_ID, EDataType.Integer, optionID)
			};
            this.ExecuteNonQuery(SQL_UPDATE_VOTE_NUM, parms);
		}

        public void Delete(int publishmentSystemID, int nodeID, int contentID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID)
			};

            this.ExecuteNonQuery(SQL_DELETE_ALL, parms);
		}

        public ArrayList GetVoteOptionInfoArrayList(int publishmentSystemID, int nodeID, int contentID)
		{
            ArrayList voteOptionInfoArrayList = new ArrayList();

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID)
			};
			
			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms)) 
			{
				while (rdr.Read()) 
				{
                    VoteOptionInfo info = new VoteOptionInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetInt32(7));
                    voteOptionInfoArrayList.Add(info);
				}
				rdr.Close();
			}
            return voteOptionInfoArrayList;
		}

        public void UpdateVoteOptionInfoArrayList(int publishmentSystemID, int nodeID, int contentID, ArrayList voteOptionInfoArrayList)
        {
            ArrayList oldVoteOptionInfoArrayList = this.GetVoteOptionInfoArrayList(publishmentSystemID, nodeID, contentID);
            int i = 0;
            foreach (VoteOptionInfo optionInfo in voteOptionInfoArrayList)
            {
                if (i < oldVoteOptionInfoArrayList.Count)
                {
                    VoteOptionInfo oldOptionInfo = oldVoteOptionInfoArrayList[i++] as VoteOptionInfo;
                    optionInfo.VoteNum = oldOptionInfo.VoteNum;
                }
            }
            this.Delete(publishmentSystemID, nodeID, contentID);
            this.Insert(voteOptionInfoArrayList);
        }

        public int GetTotalVoteNum(int publishmentSystemID, int nodeID, int contentID)
		{
			int totalVoteNum = 0;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_VOTE_NUM, parms))
			{
				if (rdr.Read())
				{
					if (!rdr.IsDBNull(0))
					{
						totalVoteNum = Convert.ToInt32(rdr[0]);
					}
				}
				rdr.Close();
			}
			return totalVoteNum;
		}
	}
}
