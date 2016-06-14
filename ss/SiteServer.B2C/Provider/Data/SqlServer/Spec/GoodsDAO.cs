using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Model;
using SiteServer.B2C.Core;
using System.Collections.Generic;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class GoodsDAO : DataProviderBase, IGoodsDAO
    {
        private const string SQL_UPDATE = "UPDATE b2c_Goods SET GoodsSN = @GoodsSN, Stock = @Stock, PriceMarket = @PriceMarket, PriceSale = @PriceSale, IsOnSale = @IsOnSale WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID AND ComboIDCollection = @ComboIDCollection";

        private const string SQL_DELETE = "DELETE FROM b2c_Goods WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID AND ComboIDCollection = @ComboIDCollection";

        private const string SQL_SELECT_ID = "SELECT GoodsID FROM b2c_Goods WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID AND ComboIDCollection = @ComboIDCollection";

        private const string SQL_SELECT_ALL = "SELECT GoodsID, PublishmentSystemID, ContentID, ComboIDCollection, SpecIDCollection, SpecItemIDCollection, GoodsSN, Stock, PriceMarket, PriceSale, IsOnSale FROM b2c_Goods WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID";

        private const string SQL_SELECT = "SELECT GoodsID, PublishmentSystemID, ContentID, ComboIDCollection, SpecIDCollection, SpecItemIDCollection, GoodsSN, Stock, PriceMarket, PriceSale, IsOnSale FROM b2c_Goods WHERE GoodsID = @GoodsID";

        private const string SQL_SELECT_PRICE_SALE = "SELECT PriceSale FROM b2c_Goods WHERE GoodsID = @GoodsID";

        private const string PARM_GOODS_ID = "@GoodsID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_CONTENT_ID = "@ContentID";
        private const string PARM_COMBO_ID_COLLECTION = "@ComboIDCollection";
        private const string PARM_SPEC_ID_COLLECTION = "@SpecIDCollection";
        private const string PARM_SPEC_ITEM_ID_COLLECTION = "@SpecItemIDCollection";
        private const string PARM_GOODS_SN = "@GoodsSN";
        private const string PARM_STOCK = "@Stock";
        private const string PARM_PRICE_MARKET = "@PriceMarket";
        private const string PARM_PRICE_SALE = "@PriceSale";
        private const string PARM_IS_ON_SALE = "@IsOnSale";

        public void Insert(GoodsInfo goodsInfo)
        {
            string sqlString = "INSERT INTO b2c_Goods (PublishmentSystemID, ContentID, ComboIDCollection, SpecIDCollection, SpecItemIDCollection, GoodsSN, Stock, PriceMarket, PriceSale, IsOnSale) VALUES (@PublishmentSystemID, @ContentID, @ComboIDCollection, @SpecIDCollection, @SpecItemIDCollection, @GoodsSN, @Stock, @PriceMarket, @PriceSale, @IsOnSale)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO b2c_Goods (GoodsID, PublishmentSystemID, ContentID, ComboIDCollection, SpecIDCollection, SpecItemIDCollection, GoodsSN, Stock, PriceMarket, PriceSale, IsOnSale) VALUES (b2c_Goods_SEQ.NEXTVAL, @PublishmentSystemID, @ContentID, @ComboIDCollection, @SpecIDCollection, @SpecItemIDCollection, @GoodsSN, @Stock, @PriceMarket, @PriceSale, @IsOnSale)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, goodsInfo.PublishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, goodsInfo.ContentID),
                this.GetParameter(PARM_COMBO_ID_COLLECTION, EDataType.VarChar, 200, goodsInfo.ComboIDCollection),
                this.GetParameter(PARM_SPEC_ID_COLLECTION, EDataType.VarChar, 200, goodsInfo.SpecIDCollection),
                this.GetParameter(PARM_SPEC_ITEM_ID_COLLECTION, EDataType.VarChar, 200, goodsInfo.SpecItemIDCollection),
                this.GetParameter(PARM_GOODS_SN, EDataType.VarChar, 50, goodsInfo.GoodsSN),
                this.GetParameter(PARM_STOCK, EDataType.Integer, goodsInfo.Stock),
                this.GetParameter(PARM_PRICE_MARKET, EDataType.Decimal, 18, goodsInfo.PriceMarket),
                this.GetParameter(PARM_PRICE_SALE, EDataType.Decimal, 18, goodsInfo.PriceSale),
                this.GetParameter(PARM_IS_ON_SALE, EDataType.VarChar, 18, goodsInfo.IsOnSale.ToString())
            };

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Update(GoodsInfo goodsInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GOODS_SN, EDataType.VarChar, 50, goodsInfo.GoodsSN),
                this.GetParameter(PARM_STOCK, EDataType.Integer, goodsInfo.Stock),
                this.GetParameter(PARM_PRICE_MARKET, EDataType.Decimal, 18, goodsInfo.PriceMarket),
                this.GetParameter(PARM_PRICE_SALE, EDataType.Decimal, 18, goodsInfo.PriceSale),
                this.GetParameter(PARM_IS_ON_SALE, EDataType.VarChar, 18, goodsInfo.IsOnSale.ToString()),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, goodsInfo.PublishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, goodsInfo.ContentID),
                this.GetParameter(PARM_COMBO_ID_COLLECTION, EDataType.VarChar, 200, goodsInfo.ComboIDCollection)
            };

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(int publishmentSystemID, int contentID, string itemIDCollection)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID),
                this.GetParameter(PARM_COMBO_ID_COLLECTION, EDataType.VarChar, 200, itemIDCollection)
            };

            this.ExecuteNonQuery(SQL_DELETE, parms);
        }

        public void DeleteNotInList(int publishmentSystemID, int contentID, ArrayList goodsIDArrayList)
        {
            if (goodsIDArrayList != null && goodsIDArrayList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM b2c_Goods WHERE PublishmentSystemID = {0} AND ContentID = {1} AND GoodsID NOT IN ({2})", publishmentSystemID, contentID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(goodsIDArrayList));

                this.ExecuteNonQuery(sqlString);
            }
        }

        public List<GoodsInfo> GetGoodsInfoList(int publishmentSystemID, int contentID)
        {
            List<GoodsInfo> list = new List<GoodsInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    GoodsInfo goodsInfo = new GoodsInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetDecimal(8), rdr.GetDecimal(9), TranslateUtils.ToBool(rdr.GetValue(10).ToString()));
                    list.Add(goodsInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public GoodsInfo GetGoodsInfo(int goodsID)
        {
            GoodsInfo goodsInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GOODS_ID, EDataType.Integer, goodsID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    goodsInfo = new GoodsInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetDecimal(8), rdr.GetDecimal(9), TranslateUtils.ToBool(rdr.GetValue(10).ToString()));
                }
                rdr.Close();
            }

            return goodsInfo;
        }

        public GoodsInfo GetGoodsInfoForDefault(int goodsID, GoodsContentInfo contentInfo)
        {
            GoodsInfo goodsInfo = this.GetGoodsInfo(goodsID);
            if (goodsInfo == null && contentInfo != null)
            {
                //当商品不存在规格时，默认一个规格
                goodsInfo = new GoodsInfo(0, contentInfo.PublishmentSystemID, contentInfo.ID, string.Empty, contentInfo.SpecIDCollection, contentInfo.SpecItemIDCollection, GoodsManager.GetGoodsSN(), -1, contentInfo.PriceMarket, contentInfo.PriceSale, contentInfo.IsOnSale);
            }

            return goodsInfo;
        }

        public int GetGoodsID(int publishmentSystemID, int contentID, string itemIDCollection)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID),
                this.GetParameter(PARM_COMBO_ID_COLLECTION, EDataType.VarChar, 200, itemIDCollection)
            };

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(SQL_SELECT_ID, parms);
        }

        public Hashtable GetItemIDCollectionHashtable(int publishmentSystemID, int contentID)
        {
            Hashtable hashtable = new Hashtable();

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    string itemIDCollection = rdr.GetValue(0).ToString();
                    int storage = rdr.GetInt32(1);
                    bool isOnSale = TranslateUtils.ToBool(rdr.GetValue(0).ToString());

                    hashtable[itemIDCollection] = storage + "," + isOnSale;
                }
                rdr.Close();
            }

            return hashtable;
        }

        public decimal GetPriceSale(int goodsID)
        {
            decimal priceSale = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GOODS_ID, EDataType.Integer, goodsID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PRICE_SALE, parms))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    priceSale = rdr.GetDecimal(0);
                }
                rdr.Close();
            }

            return priceSale;
        }
    }
}