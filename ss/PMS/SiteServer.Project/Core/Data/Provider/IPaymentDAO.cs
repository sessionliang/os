using System.Collections;
using SiteServer.Project.Model;
using BaiRong.Model;
using System.Collections.Generic;

namespace SiteServer.Project.Core
{
	public interface IPaymentDAO
	{
        void Insert(PaymentInfo paymentInfo);

        void Update(PaymentInfo paymentInfo);

        void Delete(int paymentID);

        int GetAmountPaid(int projectID, bool isCashBack);

        int GetAmountPaidPure(int projectID);

        int GetAmountExpect(int projectID, bool isCashBack);

        int GetCount(int projectID, ETriState isPayment);

        PaymentInfo GetPaymentInfo(int paymentID);

        ArrayList GetPaymentInfoArrayList(int projectID, bool isCashBack);

        IEnumerable GetDataSource(int projectID, bool isCashBack);

        IEnumerable GetDataSourceNotPay(string type);

        IEnumerable GetDataSourceCashBack(bool isPayment);

        IEnumerable GetDataSourcePaied(string type);

        int GetMaxPaymentOrder(int projectID);

        int GetAmountPaid(int year, int month);

        int GetAmountNet(int year, int month);

        Dictionary<int, int> GetProjectIDAmountPaidDictionary(int year, int month);
	}
}
