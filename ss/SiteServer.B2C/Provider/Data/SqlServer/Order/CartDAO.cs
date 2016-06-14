using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using BaiRong.Model;
using System.Data;
using System.Collections;
using BaiRong.Core;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class CartDAO : DataProviderBase, ICartDAO
    {
        private const string SQL_UPDATE_CART = "UPDATE b2c_Cart SET UserName = @UserName, SessionID = @SessionID, ChannelID = @ChannelID, ContentID = @ContentID, GoodsID = @GoodsID, PurchaseNum = @PurchaseNum, AddDate = @AddDate WHERE CartID = @CartID";

        private const string SQL_UPDATE_PURCHASE_NUM = "UPDATE b2c_Cart SET PurchaseNum = @PurchaseNum WHERE CartID = @CartID";

        private const string SQL_UPDATE_USER_NAME = "UPDATE b2c_Cart SET UserName = @UserName, SessionID = '' WHERE SessionID = @SessionID AND UserName = ''";

        private const string SQL_DELETE_CART = "DELETE FROM b2c_Cart WHERE CartID = @CartID";

        private const string SQL_SELECT_CART = "SELECT CartID, PublishmentSystemID, UserName, SessionID, ChannelID, ContentID, GoodsID, PurchaseNum, AddDate FROM b2c_Cart WHERE CartID = @CartID";

        private const string SQL_SELECT_BY_USERNAME = "SELECT CartID, PublishmentSystemID, UserName, SessionID, ChannelID, ContentID, GoodsID, PurchaseNum, AddDate FROM b2c_Cart WHERE PublishmentSystemID = @PublishmentSystemID AND UserName = @UserName AND ChannelID = @ChannelID AND ContentID = @ContentID AND GoodsID = @GoodsID";

        private const string SQL_SELECT_BY_SESSIONID = "SELECT CartID, PublishmentSystemID, UserName, SessionID, ChannelID, ContentID, GoodsID, PurchaseNum, AddDate FROM b2c_Cart WHERE PublishmentSystemID = @PublishmentSystemID AND SessionID = @SessionID AND ChannelID = @ChannelID AND ContentID = @ContentID AND GoodsID = @GoodsID";

        private const string SQL_SELECT_ALL_BY_USERNAME = "SELECT CartID, PublishmentSystemID, UserName, SessionID, ChannelID, ContentID, GoodsID, PurchaseNum, AddDate FROM b2c_Cart WHERE PublishmentSystemID = @PublishmentSystemID AND (UserName = @UserName OR SessionID = @SessionID) ORDER BY CartID DESC";

        private const string SQL_SELECT_ALL_BY_USERNAME_FOR_USERCENTER = "SELECT CartID, PublishmentSystemID, UserName, SessionID, ChannelID, ContentID, GoodsID, PurchaseNum, AddDate FROM b2c_Cart WHERE UserName = @UserName OR SessionID = @SessionID ORDER BY CartID DESC";

        private const string SQL_SELECT_ALL_BY_SESSIONID = "SELECT CartID, PublishmentSystemID, UserName, SessionID, ChannelID, ContentID, GoodsID, PurchaseNum, AddDate FROM b2c_Cart WHERE PublishmentSystemID = @PublishmentSystemID AND SessionID = @SessionID ORDER BY CartID DESC";

        public const string PARM_CART_ID = "@CartID";
        public const string PARM_PUBLISHMENTSYSTEM_ID = "@PublishmentSystemID";
        public const string PARM_USER_NAME = "@UserName";
        public const string PARM_SESSION_ID = "@SessionID";
        public const string PARM_CHANNEL_ID = "@ChannelID";
        public const string PARM_CONTENT_ID = "@ContentID";
        public const string PARM_GOODS_ID = "@GoodsID";
        public const string PARM_PURCHASE_NUM = "@PurchaseNum";
        public const string PARM_ADD_DATE = "@AddDate";

        public int Insert(CartInfo cartInfo)
        {
            int cartID = 0;
            string sqlString = "INSERT INTO b2c_Cart(PublishmentSystemID, UserName, SessionID, ChannelID, ContentID, GoodsID, PurchaseNum, AddDate) VALUES(@PublishmentSystemID, @UserName, @SessionID, @ChannelID, @ContentID, @GoodsID, @PurchaseNum, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO b2c_Cart(CartID, PublishmentSystemID, UserName, SessionID, ChannelID, ContentID, GoodsID, PurchaseNum, AddDate) VALUES(b2c_Cart_SEQ.NEXTVAL, @PublishmentSystemID, @UserName, @SessionID, @ChannelID, @ContentID, @GoodsID, @PurchaseNum, @AddDate)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, cartInfo.PublishmentSystemID),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, cartInfo.UserName),
                this.GetParameter(PARM_SESSION_ID, EDataType.NVarChar, 255, cartInfo.SessionID),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, cartInfo.ChannelID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, cartInfo.ContentID),
                this.GetParameter(PARM_GOODS_ID, EDataType.Integer, cartInfo.GoodsID),
                this.GetParameter(PARM_PURCHASE_NUM, EDataType.Integer, cartInfo.PurchaseNum),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, cartInfo.AddDate)
            };
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        cartID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "b2c_Cart");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return cartID;
        }

        public void InsertOrUpdate(CartInfo cartInfo)
        {
            CartInfo cartInfoExists = this.GetCartInfo(cartInfo.PublishmentSystemID, cartInfo.UserName, cartInfo.SessionID, cartInfo.ChannelID, cartInfo.ContentID, cartInfo.GoodsID);
            if (cartInfoExists != null)
            {
                cartInfoExists.PurchaseNum += cartInfo.PurchaseNum;
                cartInfoExists.AddDate = DateTime.Now;
                this.Update(cartInfoExists);
            }
            else
            {
                this.Insert(cartInfo);
            }
        }

        public void Update(CartInfo cartInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, cartInfo.UserName),
                this.GetParameter(PARM_SESSION_ID, EDataType.NVarChar, 255, cartInfo.SessionID),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, cartInfo.ChannelID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, cartInfo.ContentID),
                this.GetParameter(PARM_GOODS_ID, EDataType.Integer, cartInfo.GoodsID),
                this.GetParameter(PARM_PURCHASE_NUM, EDataType.Integer, cartInfo.PurchaseNum),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, cartInfo.AddDate),
                this.GetParameter(PARM_CART_ID, EDataType.Integer, cartInfo.CartID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_CART, parms);
        }

        public void Update(int cartID, int purchaseNum)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PURCHASE_NUM, EDataType.Integer, purchaseNum),
                this.GetParameter(PARM_CART_ID, EDataType.Integer, cartID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_PURCHASE_NUM, parms);
        }

        private void UpdateUserNameBySessionID(string userName, string sessionID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName),
                this.GetParameter(PARM_SESSION_ID, EDataType.NVarChar, 255, sessionID),
            };

            this.ExecuteNonQuery(SQL_UPDATE_USER_NAME, parms);
        }

        private CartInfo GetCartInfo(int publishmentSystemID, string userName, string sessionID, int channelID, int contentID, int goodsID)
        {
            CartInfo cartInfo = null;

            if (!string.IsNullOrEmpty(userName))
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, publishmentSystemID),
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName),
                    this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, channelID),
                    this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID),
                    this.GetParameter(PARM_GOODS_ID, EDataType.Integer, goodsID),
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_USERNAME, parms))
                {
                    if (rdr.Read())
                    {
                        cartInfo = new CartInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetDateTime(8));
                    }
                    rdr.Close();
                }
            }
            else
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, publishmentSystemID),
                    this.GetParameter(PARM_SESSION_ID, EDataType.NVarChar, 255, sessionID),
                    this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, channelID),
                    this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID),
                    this.GetParameter(PARM_GOODS_ID, EDataType.Integer, goodsID),
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_SESSIONID, parms))
                {
                    if (rdr.Read())
                    {
                        cartInfo = new CartInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetDateTime(8));
                    }
                    rdr.Close();
                }
            }

            return cartInfo;
        }

        public void Delete(int cartID)
        {
            IDbDataParameter[] deleteParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_CART_ID, EDataType.Integer, cartID)
            };

            this.ExecuteNonQuery(SQL_DELETE_CART, deleteParms);
        }

        public void Delete(IDbTransaction trans, int cartID)
        {
            if (trans != null)
            {
                IDbDataParameter[] deleteParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_CART_ID, EDataType.Integer, cartID)
                };

                this.ExecuteNonQuery(SQL_DELETE_CART, deleteParms);
            }
        }

        public void Delete(List<int> cartIDList)
        {
            if (cartIDList != null && cartIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM b2c_Cart WHERE CartID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(cartIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(IDbTransaction trans, List<int> cartIDList)
        {
            if (trans != null)
            {
                if (cartIDList != null && cartIDList.Count > 0)
                {
                    string sqlString = string.Format("DELETE FROM b2c_Cart WHERE CartID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(cartIDList));
                    this.ExecuteNonQuery(trans, sqlString);
                }
            }
        }

        public CartInfo GetCartInfo(int cartID)
        {
            CartInfo cartInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_CART_ID, EDataType.Integer, cartID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_CART, parms))
            {
                if (rdr.Read())
                {
                    cartInfo = new CartInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetDateTime(8));
                }
                rdr.Close();
            }

            return cartInfo;
        }

        public List<CartInfo> GetCartInfoList(int publishmentSystemID, string sessionID, string userName)
        {
            List<CartInfo> list = new List<CartInfo>();

            if (!string.IsNullOrEmpty(userName))
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, publishmentSystemID),
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName),
                    this.GetParameter(PARM_SESSION_ID, EDataType.NVarChar, 255, sessionID)
                };

                List<CartInfo> anonymousCarts = new List<CartInfo>();

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_BY_USERNAME, parms))
                {
                    while (rdr.Read())
                    {
                        CartInfo cartInfo = new CartInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetDateTime(8));

                        if (string.IsNullOrEmpty(cartInfo.UserName))
                        {
                            anonymousCarts.Add(cartInfo);
                            cartInfo.UserName = userName;
                        }

                        list.Add(cartInfo);
                    }
                    rdr.Close();
                }

                if (anonymousCarts.Count > 0)
                {
                    this.UpdateUserNameBySessionID(userName, sessionID);
                }
            }
            else
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, publishmentSystemID),
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName),
                    this.GetParameter(PARM_SESSION_ID, EDataType.NVarChar, 255, sessionID)
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_BY_SESSIONID, parms))
                {
                    while (rdr.Read())
                    {
                        CartInfo cartInfo = new CartInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetDateTime(8));
                        list.Add(cartInfo);
                    }
                    rdr.Close();
                }
            }

            return list;
        }



        public List<CartInfo> GetCartInfoList(string sessionID, string userName)
        {
            List<CartInfo> list = new List<CartInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName),
                this.GetParameter(PARM_SESSION_ID, EDataType.NVarChar, 255, sessionID)
            };

            List<CartInfo> anonymousCarts = new List<CartInfo>();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_BY_USERNAME_FOR_USERCENTER, parms))
            {
                while (rdr.Read())
                {
                    CartInfo cartInfo = new CartInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetDateTime(8));

                    if (string.IsNullOrEmpty(cartInfo.UserName))
                    {
                        anonymousCarts.Add(cartInfo);
                        cartInfo.UserName = userName;
                    }

                    list.Add(cartInfo);
                }
                rdr.Close();
            }

            if (anonymousCarts.Count > 0)
            {
                this.UpdateUserNameBySessionID(userName, sessionID);
            }

            return list;
        }
    }
}
