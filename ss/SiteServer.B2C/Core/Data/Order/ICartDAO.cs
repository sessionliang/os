using System.Collections;
using SiteServer.B2C.Model;
using System.Collections.Generic;
using System.Data;

namespace SiteServer.B2C.Core
{
    public interface ICartDAO
    {
        int Insert(CartInfo cartInfo);

        void InsertOrUpdate(CartInfo cartInfo);

        void Update(CartInfo cartInfo);

        void Update(int cartID, int purchaseNum);

        void Delete(int cartID);

        void Delete(IDbTransaction trans, int cartID);

        void Delete(List<int> cartIDList);

        void Delete(IDbTransaction trans, List<int> cartID);

        CartInfo GetCartInfo(int cartID);

        List<CartInfo> GetCartInfoList(int publishmentSystemID, string sessionID, string userName);

        List<CartInfo> GetCartInfoList(string sessionID, string userName);
    }
}
