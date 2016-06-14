using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using System.Collections;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.Project.Model;
using System.Text;

using System;

namespace SiteServer.Project.Core
{
	public class PaymentManager
	{
        public static void GetPaymentError(int amountTotal, int amountExpect, int amountRemain, bool isClosed, out string amountTotalError, out string amountRemainError)
        {
            bool boolAmountTotalError = false;
            bool boolAmountRemainError = false;

            amountTotalError = string.Empty;
            amountRemainError = string.Empty;

            if (amountExpect != amountTotal)
            {
                boolAmountTotalError = true;
            }
            if (amountRemain < 0)
            {
                boolAmountRemainError = true;
            }
            if (isClosed && amountRemain > 0)
            {
                boolAmountRemainError = true;
            }

            if (boolAmountTotalError)
            {
                amountTotalError = "合同金额与预计回款总额不符，请核实！";
            }
            if (boolAmountRemainError)
            {
                amountRemainError = "合同金额与实际回款总额不符，请核实！";
            }
        }
	}
}
