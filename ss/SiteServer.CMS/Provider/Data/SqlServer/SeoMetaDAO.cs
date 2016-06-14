using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
	public class SeoMetaDAO : DataProviderBase, ISeoMetaDAO
	{
		// Static constants
        private const string SQL_SELECT_SEO_META = "SELECT SeoMetaID, PublishmentSystemID, SeoMetaName, [IsDefault], [PageTitle], [Keywords], [Description], [Copyright], [Author], [Email], [Language], [Charset], [Distribution], [Rating], [Robots], [RevisitAfter], [Expires] FROM siteserver_SeoMeta WHERE SeoMetaID = @SeoMetaID";

		private const string SQL_SELECT_ALL_SEO_META = "SELECT SeoMetaID, PublishmentSystemID, SeoMetaName, [IsDefault], [PageTitle], [Keywords], [Description], [Copyright], [Author], [Email], [Language], [Charset], [Distribution], [Rating], [Robots], [RevisitAfter], [Expires] FROM siteserver_SeoMeta WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY SeoMetaID";

        private const string SQL_SELECT_SEO_META_BY_SEO_META_NAME = "SELECT SeoMetaID, PublishmentSystemID, SeoMetaName, [IsDefault], [PageTitle], [Keywords], [Description], [Copyright], [Author], [Email], [Language], [Charset], [Distribution], [Rating], [Robots], [RevisitAfter], [Expires] FROM siteserver_SeoMeta WHERE PublishmentSystemID = @PublishmentSystemID AND SeoMetaName = @SeoMetaName";

        private const string SQL_SELECT_DEFAULT_SEO_META = "SELECT SeoMetaID, PublishmentSystemID, SeoMetaName, [IsDefault], [PageTitle], [Keywords], [Description], [Copyright], [Author], [Email], [Language], [Charset], [Distribution], [Rating], [Robots], [RevisitAfter], [Expires] FROM siteserver_SeoMeta WHERE PublishmentSystemID = @PublishmentSystemID AND [IsDefault] = @IsDefault";

        private const string SQL_SELECT_DEFAULT_SEO_META_ID = "SELECT SeoMetaID FROM siteserver_SeoMeta WHERE PublishmentSystemID = @PublishmentSystemID AND [IsDefault] = @IsDefault";

        private const string SQL_SELECT_ALL_SEO_META_BY_PUBLISHMENT_SYSTEM_ID = "SELECT SeoMetaID, PublishmentSystemID, SeoMetaName, [IsDefault], [PageTitle], [Keywords], [Description], [Copyright], [Author], [Email], [Language], [Charset], [Distribution], [Rating], [Robots], [RevisitAfter], [Expires] FROM siteserver_SeoMeta WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY SeoMetaID";

		private const string SQL_SELECT_SEO_META_NAMES = "SELECT SeoMetaName FROM siteserver_SeoMeta WHERE PublishmentSystemID = @PublishmentSystemID";

		private const string SQL_SELECT_SEO_META_COUNT = "SELECT COUNT(SeoMetaID) FROM siteserver_SeoMeta WHERE PublishmentSystemID = @PublishmentSystemID";

		private const string SQL_SELECT_SEO_META_NAME = "SELECT SeoMetaName FROM siteserver_SeoMeta WHERE SeoMetaID = @SeoMetaID";

		private const string SQL_SELECT_SEO_META_ID_BY_SEO_META_NAME = "SELECT SeoMetaID FROM siteserver_SeoMeta WHERE PublishmentSystemID = @PublishmentSystemID AND SeoMetaName = @SeoMetaName";

        private const string SQL_UPDATE_SEO_META = "UPDATE siteserver_SeoMeta SET SeoMetaName = @SeoMetaName, [IsDefault] = @IsDefault, [PageTitle] = @PageTitle, [Keywords] = @Keywords, [Description] = @Description, [Copyright] = @Copyright, [Author] = @Author, [Email] = @Email, [Language] = @Language, [Charset] = @Charset, [Distribution] = @Distribution, [Rating] = @Rating, [Robots] = @Robots, [RevisitAfter] = @RevisitAfter, [Expires] = @Expires WHERE SeoMetaID = @SeoMetaID";

		private const string SQL_UPDATE_ALL_IS_DEFAULT = "UPDATE siteserver_SeoMeta SET [IsDefault] = @IsDefault WHERE PublishmentSystemID = @PublishmentSystemID";

		private const string SQL_UPDATE_IS_DEFAULT = "UPDATE siteserver_SeoMeta SET [IsDefault] = @IsDefault WHERE SeoMetaID = @SeoMetaID";

		private const string SQL_DELETE_SEO_META = "DELETE FROM siteserver_SeoMeta WHERE SeoMetaID = @SeoMetaID";

		//siteserver_SeoMetasInNodes
		private const string SQL_INSERT_MATCH = "INSERT INTO siteserver_SeoMetasInNodes (NodeID, IsChannel, SeoMetaID, PublishmentSystemID) VALUES (@NodeID, @IsChannel, @SeoMetaID, @PublishmentSystemID)";

		private const string SQL_DELETE_MATCH_BY_NODE_ID = "DELETE FROM siteserver_SeoMetasInNodes WHERE NodeID = @NodeID AND IsChannel = @IsChannel";

		private const string SQL_SELECT_SEO_META_ID_BY_NODE_ID = "SELECT SeoMetaID FROM siteserver_SeoMetasInNodes WHERE NodeID = @NodeID AND IsChannel = @IsChannel";

		private const string PARM_SEO_META_ID = "@SeoMetaID";
		private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
		private const string PARM_SEO_META_NAME = "@SeoMetaName";
		private const string PARM_IS_DEFAULT = "@IsDefault";
		private const string PARM_PAGE_TITLE = "@PageTitle";
		private const string PARM_KEYWORDS = "@Keywords";
		private const string PARM_DESCRIPTION = "@Description";
		private const string PARM_COPYRIGHT = "@Copyright";
        private const string PARM_AUTHOR = "@Author";
        private const string PARM_EMAIL = "@Email";
		private const string PARM_LANGUAGE = "@Language";
		private const string PARM_CHARSET = "@Charset";
		private const string PARM_DISTRIBUTION = "@Distribution";
		private const string PARM_RATING = "@Rating";
		private const string PARM_ROBOTS = "@Robots";
		private const string PARM_REVISIT_AFTER = "@RevisitAfter";
		private const string PARM_EXPIRES = "@Expires";

		//siteserver_SeoMetasInNodes
		private const string PARM_NODE_ID = "@NodeID";
		private const string PARM_IS_CHANNEL = "@IsChannel";

		public void Insert(SeoMetaInfo info)
		{
			if (info.IsDefault)
			{
				this.SetAllIsDefaultToFalse(info.PublishmentSystemID);
			}

            string sqlString = "INSERT INTO siteserver_SeoMeta (PublishmentSystemID, SeoMetaName, [IsDefault], [PageTitle], [Keywords], [Description], [Copyright], [Author], [Email], [Language], [Charset], [Distribution], [Rating], [Robots], [RevisitAfter], [Expires]) VALUES (@PublishmentSystemID, @SeoMetaName, @IsDefault, @PageTitle, @Keywords, @Description, @Copyright, @Author, @Email, @Language, @Charset, @Distribution, @Rating, @Robots, @RevisitAfter, @Expires)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_SeoMeta (SeoMetaID, PublishmentSystemID, SeoMetaName, [IsDefault], [PageTitle], [Keywords], [Description], [Copyright], [Author], [Email], [Language], [Charset], [Distribution], [Rating], [Robots], [RevisitAfter], [Expires]) VALUES (siteserver_SeoMeta_SEQ.NEXTVAL, @PublishmentSystemID, @SeoMetaName, @IsDefault, @PageTitle, @Keywords, @Description, @Copyright, @Author, @Email, @Language, @Charset, @Distribution, @Rating, @Robots, @RevisitAfter, @Expires)";
            }

			IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, info.PublishmentSystemID),
				this.GetParameter(PARM_SEO_META_NAME, EDataType.VarChar, 50, info.SeoMetaName),
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, info.IsDefault.ToString()),
				this.GetParameter(PARM_PAGE_TITLE, EDataType.NVarChar, 80, info.PageTitle),
				this.GetParameter(PARM_KEYWORDS, EDataType.NVarChar, 100, info.Keywords),
				this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 200, info.Description),
				this.GetParameter(PARM_COPYRIGHT, EDataType.NVarChar, 255, info.Copyright),
				this.GetParameter(PARM_AUTHOR, EDataType.NVarChar, 50, info.Author),
				this.GetParameter(PARM_EMAIL, EDataType.NVarChar, 50, info.Email),
				this.GetParameter(PARM_LANGUAGE, EDataType.VarChar, 50, info.Language),
				this.GetParameter(PARM_CHARSET, EDataType.VarChar, 50, info.Charset),
				this.GetParameter(PARM_DISTRIBUTION, EDataType.VarChar, 50, info.Distribution),
				this.GetParameter(PARM_RATING, EDataType.VarChar, 50, info.Rating),
				this.GetParameter(PARM_ROBOTS, EDataType.VarChar, 50, info.Robots),
				this.GetParameter(PARM_REVISIT_AFTER, EDataType.VarChar, 50, info.RevisitAfter),
				this.GetParameter(PARM_EXPIRES, EDataType.VarChar, 50, info.Expires)
			};

            this.ExecuteNonQuery(sqlString, insertParms);
		}

		public void Update(SeoMetaInfo info)
		{
			if (info.IsDefault)
			{
				this.SetAllIsDefaultToFalse(info.PublishmentSystemID);
			}

			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SEO_META_NAME, EDataType.VarChar, 50, info.SeoMetaName),
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, info.IsDefault.ToString()),
				this.GetParameter(PARM_PAGE_TITLE, EDataType.NVarChar, 80, info.PageTitle),
				this.GetParameter(PARM_KEYWORDS, EDataType.NVarChar, 100, info.Keywords),
				this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 200, info.Description),
				this.GetParameter(PARM_COPYRIGHT, EDataType.NVarChar, 255, info.Copyright),
				this.GetParameter(PARM_AUTHOR, EDataType.NVarChar, 50, info.Author),
				this.GetParameter(PARM_EMAIL, EDataType.NVarChar, 50, info.Email),
				this.GetParameter(PARM_LANGUAGE, EDataType.VarChar, 50, info.Language),
				this.GetParameter(PARM_CHARSET, EDataType.VarChar, 50, info.Charset),
				this.GetParameter(PARM_DISTRIBUTION, EDataType.VarChar, 50, info.Distribution),
				this.GetParameter(PARM_RATING, EDataType.VarChar, 50, info.Rating),
				this.GetParameter(PARM_ROBOTS, EDataType.VarChar, 50, info.Robots),
				this.GetParameter(PARM_REVISIT_AFTER, EDataType.VarChar, 50, info.RevisitAfter),
				this.GetParameter(PARM_EXPIRES, EDataType.VarChar, 50, info.Expires),
				this.GetParameter(PARM_SEO_META_ID, EDataType.Integer, info.SeoMetaID)
			};
							
			this.ExecuteNonQuery(SQL_UPDATE_SEO_META, updateParms);
		}

		private void SetAllIsDefaultToFalse(int publishmentSystemID)
		{
			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, false.ToString()),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

			this.ExecuteNonQuery(SQL_UPDATE_ALL_IS_DEFAULT, updateParms);
		}

        public void SetDefault(int publishmentSystemID, int seoMetaID, bool defaultValue)
		{
			this.SetAllIsDefaultToFalse(publishmentSystemID);

			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, defaultValue.ToString()),
				this.GetParameter(PARM_SEO_META_ID, EDataType.Integer, seoMetaID)
			};
							
			this.ExecuteNonQuery(SQL_UPDATE_IS_DEFAULT, updateParms);
		}

		public void Delete(int seoMetaID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SEO_META_ID, EDataType.Integer, seoMetaID)
			};
							
			this.ExecuteNonQuery(SQL_DELETE_SEO_META, parms);
		}

		public SeoMetaInfo GetSeoMetaInfo(int seoMetaID)
		{
			SeoMetaInfo info = null;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SEO_META_ID, EDataType.Integer, seoMetaID)
			};
			
			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SEO_META, parms)) 
			{
				if (rdr.Read()) 
				{
                    info = new SeoMetaInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString());
				}
				rdr.Close();
			}

			return info;
		}

		public SeoMetaInfo GetSeoMetaInfoBySeoMetaName(int publishmentSystemID, string seoMetaName)
		{
			SeoMetaInfo info = null;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_SEO_META_NAME, EDataType.VarChar, 50, seoMetaName)
			};
			
			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SEO_META_BY_SEO_META_NAME, parms))
			{
				if (rdr.Read())
				{
                    info = new SeoMetaInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString());
				}
				rdr.Close();
			}

			return info;
		}

		public string GetImportSeoMetaName(int publishmentSystemID, string seoMetaName)
		{
			string importSeoMetaName = "";
			if (seoMetaName.IndexOf("_") != -1)
			{
				int seoMetaName_Count = 0;
				string lastSeoMetaName = seoMetaName.Substring(seoMetaName.LastIndexOf("_") + 1);
				string firstSeoMetaName = seoMetaName.Substring(0, seoMetaName.Length - lastSeoMetaName.Length);
				try
				{				
					seoMetaName_Count = int.Parse(lastSeoMetaName);
				}
				catch { }
				seoMetaName_Count++;
				importSeoMetaName = firstSeoMetaName + seoMetaName_Count;
			}
			else
			{
				importSeoMetaName = seoMetaName + "_1";
			}

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_SEO_META_NAME, EDataType.VarChar, 50, importSeoMetaName)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SEO_META_BY_SEO_META_NAME, parms))
			{
				if (rdr.Read())
				{
					importSeoMetaName = GetImportSeoMetaName(publishmentSystemID, importSeoMetaName);
				}
				rdr.Close();
			}

			return importSeoMetaName;
		}

		public int GetCount(int publishmentSystemID)
		{
			int count = 0;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SEO_META_COUNT, parms)) 
			{
				if (rdr.Read()) 
				{
					if (!rdr.IsDBNull(0))
					{
						count = Convert.ToInt32(rdr[0]);
					}
				}
				rdr.Close();
			}

			return count;
		}

		public SeoMetaInfo GetDefaultSeoMetaInfo(int publishmentSystemID)
		{
			SeoMetaInfo info = null;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, true.ToString())
			};
			
			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_DEFAULT_SEO_META, parms)) 
			{
				if (rdr.Read()) 
				{
                    info = new SeoMetaInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString());
				}
				rdr.Close();
			}

			return info;
		}

        public int GetDefaultSeoMetaID(int publishmentSystemID)
        {
            int seoMetaID = 0;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, true.ToString())
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_DEFAULT_SEO_META_ID, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        seoMetaID = Convert.ToInt32(rdr[0]);
                    }
                }
				rdr.Close();
            }

            return seoMetaID;
        }

		public IEnumerable GetDataSource(int publishmentSystemID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

			IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_SEO_META, parms);
			return enumerable;
		}

		public ArrayList GetSeoMetaInfoArrayListByPublishmentSystemID(int publishmentSystemID)
		{
			ArrayList arraylist = new ArrayList();

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_SEO_META_BY_PUBLISHMENT_SYSTEM_ID, parms))
			{
				while (rdr.Read())
				{
                    SeoMetaInfo info = new SeoMetaInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString());
					arraylist.Add(info);
				}
				rdr.Close();
			}
			return arraylist;
		}

		public string GetSeoMetaName(int seoMetaID)
		{
			string seoMetaName = string.Empty;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SEO_META_ID, EDataType.Integer, seoMetaID)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SEO_META_NAME, parms)) 
			{
				if (rdr.Read()) 
				{					
					seoMetaName = rdr.GetValue(0).ToString();
				}
				rdr.Close();
			}

			return seoMetaName;
		}

		public int GetSeoMetaIDBySeoMetaName(int publishmentSystemID, string seoMetaName)
		{
			int seoMetaID = 0;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_SEO_META_NAME, EDataType.VarChar, 50, seoMetaName)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SEO_META_ID_BY_SEO_META_NAME, parms)) 
			{
				if (rdr.Read())
				{
					if (!rdr.IsDBNull(0))
					{
						seoMetaID = Convert.ToInt32(rdr[0]);
					}
				}
				rdr.Close();
			}

			return seoMetaID;
		}

		public ArrayList GetSeoMetaNameArrayList(int publishmentSystemID)
		{
			ArrayList list = new ArrayList();

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SEO_META_NAMES, parms)) 
			{
				while (rdr.Read()) 
				{
                    list.Add(rdr.GetValue(0).ToString());
				}
				rdr.Close();
			}

			return list;
		}

		//siteserver_SeoMetasInNodes
        public void InsertMatch(int publishmentSystemID, int nodeID, int seoMetaID, bool isChannel)
		{
			int lastSeoMetaID = this.GetSeoMetaIDByNodeID(nodeID, isChannel);
			if (lastSeoMetaID != 0)
			{
                this.DeleteMatch(publishmentSystemID, nodeID, isChannel);
			}

			IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID),
				this.GetParameter(PARM_IS_CHANNEL, EDataType.VarChar, 18, isChannel.ToString()),
				this.GetParameter(PARM_SEO_META_ID, EDataType.Integer, seoMetaID),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
			};
							
			this.ExecuteNonQuery(SQL_INSERT_MATCH, insertParms);
            SeoManager.RemoveCache(publishmentSystemID);
		}

        public void DeleteMatch(int publishmentSystemID, int nodeID, bool isChannel)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID),
				this.GetParameter(PARM_IS_CHANNEL, EDataType.VarChar, 18, isChannel.ToString()),
			};
							
			this.ExecuteNonQuery(SQL_DELETE_MATCH_BY_NODE_ID, parms);
            SeoManager.RemoveCache(publishmentSystemID);
		}


        public int GetSeoMetaIDByNodeID(int nodeID, bool isChannel)
		{
			int seoMetaID = 0;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID),
				this.GetParameter(PARM_IS_CHANNEL, EDataType.VarChar, 18, isChannel.ToString())
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SEO_META_ID_BY_NODE_ID, parms))
			{
				if (rdr.Read())
				{
					if (!rdr.IsDBNull(0))
					{
						seoMetaID = Convert.ToInt32(rdr[0]);
					}
				}
				rdr.Close();
			}

			return seoMetaID;
		}

        public ArrayList[] GetSeoMetaArrayLists(int publishmentSystemID)
        {
            string sqlString = "SELECT NodeID, IsChannel FROM siteserver_SeoMetasInNodes WHERE PublishmentSystemID = " + publishmentSystemID;

            ArrayList arraylist1 = new ArrayList();
            ArrayList arraylist2 = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int nodeID = rdr.GetInt32(0);
                    bool isChannel = TranslateUtils.ToBool(rdr.GetValue(1).ToString());

                    if (isChannel)
                    {
                        if (!arraylist1.Contains(nodeID))
                        {
                            arraylist1.Add(nodeID);
                        }
                    }
                    else
                    {
                        if (!arraylist2.Contains(nodeID))
                        {
                            arraylist2.Add(nodeID);
                        }
                    }
                }
                rdr.Close();
            }

            return new ArrayList[] { arraylist1, arraylist2 };
        }

	}
}
