using System.Collections;
using System.Collections.Generic;
using SiteServer.B2C.Model;

namespace SiteServer.B2C.Core
{
	public interface IPaymentDAO
	{
		int Insert(PaymentInfo paymentInfo);

        void Update(PaymentInfo paymentInfo);

		void Delete(int paymentID);

        PaymentInfo GetPaymentInfo(int paymentID);

        bool IsExists(int publishmentSystemID, string paymentName);

        bool IsExists(int publishmentSystemID, EPaymentType paymentType);

		IEnumerable GetDataSource(int publishmentSystemID);

        List<PaymentInfo> GetPaymentInfoList(int publishmentSystemID);

        bool UpdateTaxisToUp(int publishmentSystemID, int paymentID);

        bool UpdateTaxisToDown(int publishmentSystemID, int paymentID);
	}
}
