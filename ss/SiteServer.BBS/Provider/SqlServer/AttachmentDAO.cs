using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using BaiRong.Core.Data.Provider;
using System.Collections;
using SiteServer.BBS.Core;
using BaiRong.Model;
using BaiRong.Core;
using SiteServer.CMS.Core;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class AttachmentDAO : DataProviderBase, IAttachmentDAO
    {
        public int Insert(AttachmentInfo info)
        {
            int attachmentID = 0;

            string sqlString = "INSERT INTO bbs_Attachment(PublishmentSystemID, ThreadID, PostID, UserName, IsInContent, IsImage, Price, FileName, FileType, FileSize, AttachmentUrl, ImageUrl, Downloads, AddDate, Description) VALUES(@PublishmentSystemID, @ThreadID, @PostID, @UserName, @IsInContent, @IsImage, @Price, @FileName, @FileType, @FileSize, @AttachmentUrl, @ImageUrl, @Downloads, @AddDate, @Description)";

            IDbDataParameter[] param = new IDbDataParameter[]
            {
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter("@ThreadID", EDataType.Integer, info.ThreadID),
                this.GetParameter("@PostID", EDataType.Integer, info.PostID),
                this.GetParameter("@UserName", EDataType.NVarChar, 50,  info.UserName),
                this.GetParameter("@IsInContent", EDataType.VarChar, 18,  info.IsInContent.ToString()),
                this.GetParameter("@IsImage", EDataType.VarChar, 18,  info.IsImage.ToString()),
                this.GetParameter("@Price", EDataType.Integer,  info.Price),
                this.GetParameter("@FileName", EDataType.NVarChar, 50,  info.FileName),
                this.GetParameter("@FileType", EDataType.VarChar, 50,  info.FileType),
                this.GetParameter("@FileSize", EDataType.Integer, info.FileSize),
                this.GetParameter("@AttachmentUrl", EDataType.VarChar, 200,  info.AttachmentUrl),
                this.GetParameter("@ImageUrl", EDataType.VarChar, 200,  info.ImageUrl),
                this.GetParameter("@Downloads", EDataType.Integer, info.Downloads),
                this.GetParameter("@AddDate", EDataType.DateTime, info.AddDate),
                this.GetParameter("@Description", EDataType.NVarChar, 200,  info.Description)
            };

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, param);

                        attachmentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bbs_Attachment");

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return attachmentID;
        }

        public int ImportAttachment(AttachmentInfo info)
        {
            int attachmentID = 0;

            string sqlString = "INSERT INTO bbs_Attachment(PublishmentSystemID, ThreadID, PostID, UserName, IsInContent, IsImage, Price, FileName, FileType, FileSize, AttachmentUrl, ImageUrl, Downloads, AddDate, Description) VALUES(@PublishmentSystemID, @ThreadID, @PostID, @UserName, @IsInContent, @IsImage, @Price, @FileName, @FileType, @FileSize, @AttachmentUrl, @ImageUrl, @Downloads, @AddDate, @Description)";

            IDbDataParameter[] param = new IDbDataParameter[]
            {
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter("@ThreadID", EDataType.Integer, info.ThreadID),
                this.GetParameter("@PostID", EDataType.Integer, info.PostID),
                this.GetParameter("@UserName", EDataType.NVarChar, 50,  info.UserName),
                this.GetParameter("@IsInContent", EDataType.VarChar, 18,  info.IsInContent.ToString()),
                this.GetParameter("@IsImage", EDataType.VarChar, 18,  info.IsImage.ToString()),
                this.GetParameter("@Price", EDataType.Integer,  info.Price),
                this.GetParameter("@FileName", EDataType.NVarChar, 50,  info.FileName),
                this.GetParameter("@FileType", EDataType.VarChar, 50,  info.FileType),
                this.GetParameter("@FileSize", EDataType.Integer, info.FileSize),
                this.GetParameter("@AttachmentUrl", EDataType.VarChar, 200,  info.AttachmentUrl),
                this.GetParameter("@ImageUrl", EDataType.VarChar, 200,  info.ImageUrl),
                this.GetParameter("@Downloads", EDataType.Integer, info.Downloads),
                this.GetParameter("@AddDate", EDataType.DateTime, info.AddDate),
                this.GetParameter("@Description", EDataType.NVarChar, 200,  info.Description)
            };

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, param);

                        attachmentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bbs_Attachment");

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return attachmentID;
        }

        public void Update(AttachmentInfo info)
        {
            string sqlString = "UPDATE bbs_Attachment SET ThreadID = @ThreadID, PostID = @PostID, UserName = @UserName, IsInContent = @IsInContent, IsImage = @IsImage, Price = @Price, FileName = @FileName, FileType = @FileType, FileSize = @FileSize, AttachmentUrl = @AttachmentUrl, ImageUrl = @ImageUrl, Downloads = @Downloads, AddDate = @AddDate, Description = @Description WHERE ID= @ID";
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter("@ThreadID", EDataType.Integer, info.ThreadID),
                this.GetParameter("@PostID", EDataType.Integer, info.PostID),
                this.GetParameter("@UserName", EDataType.NVarChar, 50,  info.UserName),
                this.GetParameter("@IsInContent", EDataType.VarChar, 18,  info.IsInContent.ToString()),
                this.GetParameter("@IsImage", EDataType.VarChar, 18,  info.IsImage.ToString()),
                this.GetParameter("@Price", EDataType.Integer,  info.Price),
                this.GetParameter("@FileName", EDataType.NVarChar, 50,  info.FileName),
                this.GetParameter("@FileType", EDataType.VarChar, 50,  info.FileType),
                this.GetParameter("@FileSize", EDataType.Integer, info.FileSize),
                this.GetParameter("@AttachmentUrl", EDataType.VarChar, 200,  info.AttachmentUrl),
                this.GetParameter("@ImageUrl", EDataType.VarChar, 200,  info.ImageUrl),
                this.GetParameter("@Downloads", EDataType.Integer, info.Downloads),
                this.GetParameter("@AddDate", EDataType.DateTime, info.AddDate),
                this.GetParameter("@Description", EDataType.NVarChar, 200,  info.Description),
                this.GetParameter("@ID", EDataType.Integer,  info.ID)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Update(int id, int threadID, int postID, bool isInContent, int price, string description)
        {
            string sqlString = "UPDATE bbs_Attachment SET ThreadID = @ThreadID, PostID = @PostID, IsInContent = @IsInContent, Price = @Price, Description = @Description WHERE ID= @ID";
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter("@ThreadID", EDataType.Integer, threadID),
                this.GetParameter("@PostID", EDataType.Integer, postID),
                this.GetParameter("@IsInContent", EDataType.VarChar, 18,  isInContent.ToString()),
                this.GetParameter("@Price", EDataType.Integer,  price),
                this.GetParameter("@Description", EDataType.NVarChar, 200,  description),
                this.GetParameter("@ID", EDataType.Integer,  id)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void AddDownloadCount(int id)
        {
            string sqlString = "UPDATE bbs_Attachment SET Downloads = Downloads + 1 WHERE ID = " + id;

            this.ExecuteNonQuery(sqlString);
        }

        public int GetDownloadCount(int id)
        {
            string sqlString = "SELECT Downloads FROM bbs_Attachment WHERE ID = " + id;

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public void DeleteByPostIDList(int publishmentSystemID, List<int> postIDList)
        {
            if (postIDList != null && postIDList.Count > 0)
            {
                string sqlString = string.Format("SELECT AttachmentUrl FROM bbs_Attachment WHERE PublishmentSystemID = {0} AND PostID IN ({1})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(postIDList));

                ArrayList attachmentUrlArrayList = new ArrayList();

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        attachmentUrlArrayList.Add(rdr.GetValue(0).ToString());
                    }
                    rdr.Close();
                }

                if (attachmentUrlArrayList.Count > 0)
                {
                    sqlString = string.Format("DELETE FROM bbs_Attachment WHERE PublishmentSystemID = {0} AND PostID IN ({1})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(postIDList));

                    this.ExecuteNonQuery(sqlString);

                    foreach (string attachmentUrl in attachmentUrlArrayList)
                    {
                        if (!string.IsNullOrEmpty(attachmentUrl))
                        {
                            string filePath = PathUtility.GetPublishmentSystemPath(publishmentSystemID, attachmentUrl);
                            FileUtils.DeleteFileIfExists(filePath);
                        }
                    }
                }
            }
        }

        public void DeleteByThreadIDList(int publishmentSystemID, List<int> threadIDList)
        {
            if (threadIDList != null && threadIDList.Count > 0)
            {
                string sqlString = string.Format("SELECT AttachmentUrl FROM bbs_Attachment WHERE PublishmentSystemID = {0} AND ThreadID IN ({1})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDList));

                ArrayList attachmentUrlArrayList = new ArrayList();

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        attachmentUrlArrayList.Add(rdr.GetValue(0).ToString());
                    }
                    rdr.Close();
                }

                if (attachmentUrlArrayList.Count > 0)
                {
                    sqlString = string.Format("DELETE FROM bbs_Attachment WHERE PublishmentSystemID = {0} AND ThreadID IN ({1})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDList));

                    this.ExecuteNonQuery(sqlString);

                    foreach (string attachmentUrl in attachmentUrlArrayList)
                    {
                        if (!string.IsNullOrEmpty(attachmentUrl))
                        {
                            string filePath = PathUtility.GetPublishmentSystemPath(publishmentSystemID, attachmentUrl);
                            FileUtils.DeleteFileIfExists(filePath);
                        }
                    }
                }
            }
        }
       
    
        public void Delete(int publishmentSystemID, List<int> attachIDList)
        {
            if (attachIDList != null && attachIDList.Count > 0)
            {
                string sqlString = string.Format("SELECT AttachmentUrl FROM bbs_Attachment WHERE PublishmentSystemID = {0} AND ID IN ({1})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(attachIDList));

                ArrayList attachmentUrlArrayList = new ArrayList();

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        attachmentUrlArrayList.Add(rdr.GetValue(0).ToString());
                    }
                    rdr.Close();
                }

                if (attachmentUrlArrayList.Count > 0)
                {
                    sqlString = string.Format("DELETE FROM bbs_Attachment WHERE PublishmentSystemID = {0} AND ID IN ({1})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(attachIDList));

                    this.ExecuteNonQuery(sqlString);

                    foreach (string attachmentUrl in attachmentUrlArrayList)
                    {
                        if (!string.IsNullOrEmpty(attachmentUrl))
                        {
                            string filePath = PathUtility.GetPublishmentSystemPath(publishmentSystemID, attachmentUrl);
                            FileUtils.DeleteFileIfExists(filePath);
                        }
                    }
                }
            }
        }

        public void DeleteByThreadAll(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT AttachmentUrl FROM bbs_Attachment WHERE PostID < 0 AND PublishmentSystemID = {0}", publishmentSystemID);

            ArrayList attachmentUrlArrayList = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    attachmentUrlArrayList.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            if (attachmentUrlArrayList.Count > 0)
            {
                sqlString = string.Format("DELETE FROM bbs_Attachment WHERE PostID < 0 AND PublishmentSystemID = {0}", publishmentSystemID);

                this.ExecuteNonQuery(sqlString);

                foreach (string attachmentUrl in attachmentUrlArrayList)
                {
                    if (!string.IsNullOrEmpty(attachmentUrl))
                    {
                        string filePath = PathUtility.GetPublishmentSystemPath(publishmentSystemID, attachmentUrl);
                        FileUtils.DeleteFileIfExists(filePath);
                    }
                }
            }
        }

        public AttachmentInfo GetAttachmentInfo(int id)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, ThreadID, PostID, UserName, IsInContent, IsImage, Price, FileName, FileType, FileSize, AttachmentUrl, ImageUrl, Downloads, AddDate, Description FROM bbs_Attachment WHERE ID = {0}", id);

            AttachmentInfo attachmentInfo = null;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    attachmentInfo = new AttachmentInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetValue(4).ToString(), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetInt32(7), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetInt32(10), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString(), rdr.GetInt32(13), rdr.GetDateTime(14), rdr.GetValue(15).ToString());
                }
                rdr.Close();
            }
            return attachmentInfo;
        }

        public List<int> GetAttachIDList(int publishmentSystemID, int threadID, int postID)
        {
            string sqlString = string.Format("SELECT ID FROM bbs_Attachment WHERE PublishmentSystemID = {0} AND ThreadID = {1} AND PostID = {2}", publishmentSystemID, threadID, postID);

            List<int> list = new List<int>();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }
            return list;
        }

        public List<AttachmentInfo> GetList(int publishmentSystemID, int threadID, int postID)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, ThreadID, PostID, UserName, IsInContent, IsImage, Price, FileName, FileType, FileSize, AttachmentUrl, ImageUrl, Downloads, AddDate, Description FROM bbs_Attachment WHERE PublishmentSystemID = {0} AND ThreadID = {1} AND PostID = {2}", publishmentSystemID, threadID, postID);

            List<AttachmentInfo> list = new List<AttachmentInfo>();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    AttachmentInfo attachmentInfo = new AttachmentInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetValue(4).ToString(), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetInt32(7), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetInt32(10), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString(), rdr.GetInt32(13), rdr.GetDateTime(14), rdr.GetValue(15).ToString());
                    list.Add(attachmentInfo);
                }
                rdr.Close();
            }
            return list;
        }

        public string GetSqlString(int publishmentSystemID, int threadID, int postID)
        {
            return string.Format("SELECT ID, PublishmentSystemID, ThreadID, PostID, UserName, IsInContent, IsImage, Price, FileName, FileType, FileSize, AttachmentUrl, ImageUrl, Downloads, AddDate, Description FROM bbs_Attachment WHERE PublishmentSystemID = {0} AND ThreadID = {1} AND PostID = {2}", publishmentSystemID, threadID, postID);
        }
    }
}