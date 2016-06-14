using System;
namespace SiteServer.Project.Model
{
	public class PaymentInfo
	{
        private int paymentID;
        private int projectID;
        private bool isCashBack;
        private string paymentType;
        private int paymentOrder;
        private string premise;
        private DateTime expectDate;
        private int amountExpect;
        private bool isInvoice;
        private string invoiceNO;
        private DateTime invoiceDate;
        private bool isPayment;
        private int amountPaid;
        private DateTime paymentDate;

		public PaymentInfo()
		{
            this.paymentID = 0;
            this.projectID = 0;
            this.isCashBack = false;
            this.paymentType = string.Empty;
            this.paymentOrder = 0;
            this.premise = string.Empty;
            this.expectDate = DateTime.Now;
            this.amountExpect = 0;
            this.isInvoice = false;
            this.invoiceNO = string.Empty;
            this.invoiceDate = DateTime.Now;
            this.isPayment = false;
            this.amountPaid = 0;
            this.paymentDate = DateTime.Now;
		}

        public PaymentInfo(int paymentID, int projectID, bool isCashBack, string paymentType, int paymentOrder, string premise, DateTime expectDate, int amountExpect, bool isInvoice, string invoiceNO, DateTime invoiceDate, bool isPayment, int amountPaid, DateTime paymentDate)
		{
            this.paymentID = paymentID;
            this.projectID = projectID;
            this.isCashBack = isCashBack;
            this.paymentType = paymentType;
            this.paymentOrder = paymentOrder;
            this.premise = premise;
            this.expectDate = expectDate;
            this.amountExpect = amountExpect;
            this.isInvoice = isInvoice;
            this.invoiceNO = invoiceNO;
            this.invoiceDate = invoiceDate;
            this.isPayment = isPayment;
            this.amountPaid = amountPaid;
            this.paymentDate = paymentDate;
		}

        public int PaymentID
        {
            get { return paymentID; }
            set { paymentID = value; }
        }

        public int ProjectID
        {
            get { return projectID; }
            set { projectID = value; }
        }

        public bool IsCashBack
        {
            get { return isCashBack; }
            set { isCashBack = value; }
        }

        public string PaymentType
        {
            get { return paymentType; }
            set { paymentType = value; }
        }

        public int PaymentOrder
        {
            get { return paymentOrder; }
            set { paymentOrder = value; }
        } 

        public string Premise
        {
            get { return premise; }
            set { premise = value; }
        }

        public DateTime ExpectDate
        {
            get { return expectDate; }
            set { expectDate = value; }
        }

        public int AmountExpect
        {
            get { return amountExpect; }
            set { amountExpect = value; }
        }

        public bool IsInvoice
        {
            get { return isInvoice; }
            set { isInvoice = value; }
        }

        public string InvoiceNO
        {
            get { return invoiceNO; }
            set { invoiceNO = value; }
        }

        public DateTime InvoiceDate
        {
            get { return invoiceDate; }
            set { invoiceDate = value; }
        }

        public bool IsPayment
        {
            get { return isPayment; }
            set { isPayment = value; }
        }

        public int AmountPaid
        {
            get { return amountPaid; }
            set { amountPaid = value; }
        }

        public DateTime PaymentDate
        {
            get { return paymentDate; }
            set { paymentDate = value; }
        }
	}
}
