using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.BBS.Model;
using System.Data;
using BaiRong.Model;
using System.Collections;
using SiteServer.BBS.Core;
using BaiRong.Core;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class IdentifyDAO : DataProviderBase, IIdentifyDAO
    {
        //取主题鉴定表中的信息
        public List<IdentifyInfo> GetIdentifyList(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, Title, IconUrl, StampUrl, Taxis FROM bbs_Identify WHERE PublishmentSystemID = {0} ORDER BY Taxis DESC", publishmentSystemID);

            List<IdentifyInfo> list = new List<IdentifyInfo>();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    IdentifyInfo info = new IdentifyInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5));
                    list.Add(info);
                }
                rdr.Close();
            }

            return list;
        }

        //根据ID取主题鉴定表中的信息
        public IdentifyInfo GetIdentifyInfo(int id)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, Title, IconUrl, StampUrl, Taxis FROM bbs_Identify WHERE ID = {0}", id);

            IdentifyInfo identifyInfo = null;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    identifyInfo = new IdentifyInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5));
                }
                rdr.Close();
            }
            return identifyInfo;
        }

        //后台主题鉴定的更新
        public void Update(int publishmentSystemID, IdentifyInfo info)
        {
            string sqlString = "UPDATE bbs_Identify SET Title=@Title, IconUrl=@IconUrl,StampUrl=@StampUrl WHERE ID=@ID";

            IDbDataParameter[] param = new IDbDataParameter[]
			{
                this.GetParameter("@Title", EDataType.NVarChar, 50, info.Title),
                this.GetParameter("@IconUrl", EDataType.VarChar, 200, info.IconUrl),
                this.GetParameter("@StampUrl", EDataType.VarChar, 200, info.StampUrl),
                this.GetParameter("@ID",EDataType.Integer, info.ID)
            };

            this.ExecuteNonQuery(sqlString, param);

            IdentifyManager.RemoveCache(publishmentSystemID);
        }

        //根据ID进行主题鉴定的删除
        public void Delete(int publishmentSystemID, int id)
        {
            string sqlString = string.Format("DELETE FROM bbs_Identify WHERE ID = {0}", id);
            this.ExecuteNonQuery(sqlString);

            IdentifyManager.RemoveCache(publishmentSystemID);
        }

        //添加记录
        public void Insert(int publishmentSystemID, IdentifyInfo info)
        {
            int maxTaxis = this.GetMaxTaxis(info.PublishmentSystemID);
            info.Taxis = maxTaxis + 1;

            string sqlString = "INSERT INTO bbs_Identify(PublishmentSystemID, Title, IconUrl, StampUrl, Taxis) VALUES (@PublishmentSystemID, @Title, @IconUrl, @StampUrl, @Taxis)";
            IDbDataParameter[] param = new IDbDataParameter[]
            {
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter("@Title", EDataType.NVarChar, 50, info.Title),
                this.GetParameter("@IconUrl", EDataType.VarChar, 200, info.IconUrl),
                this.GetParameter("@StampUrl", EDataType.VarChar, 200, info.StampUrl),
                this.GetParameter("@Taxis", EDataType.Integer, info.Taxis)
            };

            this.ExecuteNonQuery(sqlString, param);

            IdentifyManager.RemoveCache(publishmentSystemID);
        }
        //取最大的Taxis
        private int GetMaxTaxis(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM bbs_Identify WHERE PublishmentSystemID = {0}", publishmentSystemID);
            
            int maxTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        maxTaxis = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return maxTaxis;
        }

        private int GetTaxis(int id)
        {
            string sqlString = string.Format("SELECT Taxis FROM bbs_Identify WHERE ID = {0}", id);
            int taxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    taxis = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int id, int taxis)
        {
            string sqlString = string.Format("UPDATE bbs_Identify SET Taxis = {0} WHERE ID = {1}", taxis, id);

            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateTaxisToUp(int publishmentSystemID, int id)
        {
            //Get Higher Taxis and ClassID
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM bbs_Identify WHERE Taxis > (SELECT Taxis FROM bbs_Identify WHERE ID = {0}) AND PublishmentSystemID = {1} ORDER BY Taxis", id, publishmentSystemID);

            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = rdr.GetInt32(0);
                    higherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            if (higherID > 0)
            {
                int selectedTaxis = GetTaxis(id);
                SetTaxis(id, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
            }
        }

        public void UpdateTaxisToDown(int publishmentSystemID, int id)
        {
            //Get Lower Taxis and ClassID
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM bbs_Identify WHERE Taxis < (SELECT Taxis FROM bbs_Identify WHERE ID = {0}) AND PublishmentSystemID = {1} ORDER BY Taxis DESC", id, publishmentSystemID);

            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = rdr.GetInt32(0);
                    lowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            if (lowerID > 0)
            {
                int selectedTaxis = GetTaxis(id);
                SetTaxis(id, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
            }
        }

        public string GetSelectCommend(int publishmentSystemID)
        {
            return string.Format("SELECT ID, PublishmentSystemID, Title, IconUrl, StampUrl, Taxis FROM bbs_Identify WHERE PublishmentSystemID = {0}", publishmentSystemID);
        }

        public Hashtable GetIdentifyInfoHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string sqlString = string.Format("SELECT ID, PublishmentSystemID, Title, IconUrl, StampUrl, Taxis FROM bbs_Identify WHERE PublishmentSystemID = {0} ORDER BY Taxis DESC", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    IdentifyInfo info = new IdentifyInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5));

                    hashtable.Add(info.ID, info);
                }
                rdr.Close();
            }

            return hashtable;
        }

        public void CreateDefaultIdentify(int publishmentSystemID)
        {
            bool isExists = false;

            string sqlString = string.Format("SELECT ID FROM bbs_Identify WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    isExists = true;
                }
                rdr.Close();
            }

            if (!isExists)
            {
                IdentifyInfo identifyInfo = new IdentifyInfo(0, publishmentSystemID, "变态贴", "images/identify/s_bt.gif", "images/identify/bt.gif", 0);
                Insert(publishmentSystemID, identifyInfo);

                identifyInfo = new IdentifyInfo(0, publishmentSystemID, "找抽贴", "images/identify/s_zc.gif", "images/identify/zc.gif", 0);
                Insert(publishmentSystemID, identifyInfo);

                identifyInfo = new IdentifyInfo(0, publishmentSystemID, "扯淡贴", string.Empty, "images/identify/cd.gif", 0);
                Insert(publishmentSystemID, identifyInfo);

                identifyInfo = new IdentifyInfo(0, publishmentSystemID, "猥琐贴", "images/identify/s_ws.gif", "images/identify/ws.gif", 0);
                Insert(publishmentSystemID, identifyInfo);

                identifyInfo = new IdentifyInfo(0, publishmentSystemID, "好图贴", "images/identify/s_ht.gif", "images/identify/ht.gif", 0);
                Insert(publishmentSystemID, identifyInfo);

                identifyInfo = new IdentifyInfo(0, publishmentSystemID, "精华贴", "images/identify/s_jh.gif", "images/identify/jh.gif", 0);
                Insert(publishmentSystemID, identifyInfo);

                identifyInfo = new IdentifyInfo(0, publishmentSystemID, "雷人贴", "images/identify/s_lr.gif", "images/identify/lr.gif", 0);
                Insert(publishmentSystemID, identifyInfo);

                identifyInfo = new IdentifyInfo(0, publishmentSystemID, "牛贴", "images/identify/s_nt.gif", "images/identify/nt.gif", 0);
                Insert(publishmentSystemID, identifyInfo);

                identifyInfo = new IdentifyInfo(0, publishmentSystemID, "推荐贴", "images/identify/s_tj.gif", "images/identify/tj.gif", 0);
                Insert(publishmentSystemID, identifyInfo);

                identifyInfo = new IdentifyInfo(0, publishmentSystemID, "原创贴", "images/identify/s_yc.gif", "images/identify/yc.gif", 0);
                Insert(publishmentSystemID, identifyInfo);

                identifyInfo = new IdentifyInfo(0, publishmentSystemID, "优质贴", "images/identify/s_yz.gif", "images/identify/yz.gif", 0);
                Insert(publishmentSystemID, identifyInfo);

                identifyInfo = new IdentifyInfo(0, publishmentSystemID, "置顶贴", "images/identify/s_zd.gif", "images/identify/zd.gif", 0);
                Insert(publishmentSystemID, identifyInfo);
            }
        }
    }
}
