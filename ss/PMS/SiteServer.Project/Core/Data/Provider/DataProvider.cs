using System;
using System.Configuration;
using System.Reflection;

using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.Project.Core
{
    public class DataProvider
    {
        private static string assemblyString;
        private static string namespaceString;

        private static string connectionString;

        public static void SetDatabaseType()
        {
            if (BaiRongDataProvider.DatabaseType == EDatabaseType.SqlServer)
            {
                assemblyString = "SiteServer.Project";
                namespaceString = "SiteServer.Project.Provider.Data.SqlServer";
            }
        }

        static DataProvider()
        {
            connectionString = ConfigurationSettings.AppSettings.Get("ConnectionString");
            DataProvider.SetDatabaseType();
        }

        public static string ConnectionString
        {
            set { connectionString = value; }
            get { return connectionString; }
        }

        private static IApplyDAO applyDAO;
        public static IApplyDAO ApplyDAO
        {
            get
            {
                if (applyDAO == null)
                {
                    string className = namespaceString + ".ApplyDAO";
                    applyDAO = (IApplyDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return applyDAO;
            }
        }

        private static IProjectLogDAO projectLogDAO;
        public static IProjectLogDAO ProjectLogDAO
        {
            get
            {
                if (projectLogDAO == null)
                {
                    string className = namespaceString + ".ProjectLogDAO";
                    projectLogDAO = (IProjectLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return projectLogDAO;
            }
        }

        private static IProjectDAO projectDAO;
        public static IProjectDAO ProjectDAO
        {
            get
            {
                if (projectDAO == null)
                {
                    string className = namespaceString + ".ProjectDAO";
                    projectDAO = (IProjectDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return projectDAO;
            }
        }

        private static IRemarkDAO remarkDAO;
        public static IRemarkDAO RemarkDAO
        {
            get
            {
                if (remarkDAO == null)
                {
                    string className = namespaceString + ".RemarkDAO";
                    remarkDAO = (IRemarkDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return remarkDAO;
            }
        }

        private static IReplyDAO replyDAO;
        public static IReplyDAO ReplyDAO
        {
            get
            {
                if (replyDAO == null)
                {
                    string className = namespaceString + ".ReplyDAO";
                    replyDAO = (IReplyDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return replyDAO;
            }
        }

        private static ITypeDAO typeDAO;
        public static ITypeDAO TypeDAO
        {
            get
            {
                if (typeDAO == null)
                {
                    string className = namespaceString + ".TypeDAO";
                    typeDAO = (ITypeDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return typeDAO;
            }
        }

        private static IPaymentDAO paymentDAO;
        public static IPaymentDAO PaymentDAO
        {
            get
            {
                if (paymentDAO == null)
                {
                    string className = namespaceString + ".PaymentDAO";
                    paymentDAO = (IPaymentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return paymentDAO;
            }
        }

        private static IProjectDocumentDAO projectDocumentDAO;
        public static IProjectDocumentDAO ProjectDocumentDAO
        {
            get
            {
                if (projectDocumentDAO == null)
                {
                    string className = namespaceString + ".ProjectDocumentDAO";
                    projectDocumentDAO = (IProjectDocumentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return projectDocumentDAO;
            }
        }

        private static IDocTypeDAO docTypeDAO;
        public static IDocTypeDAO DocTypeDAO
        {
            get
            {
                if (docTypeDAO == null)
                {
                    string className = namespaceString + ".DocTypeDAO";
                    docTypeDAO = (IDocTypeDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return docTypeDAO;
            }
        }

        private static IDocumentDAO documentDAO;
        public static IDocumentDAO DocumentDAO
        {
            get
            {
                if (documentDAO == null)
                {
                    string className = namespaceString + ".DocumentDAO";
                    documentDAO = (IDocumentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return documentDAO;
            }
        }

        //brs

        private static IDBLicenseDAO dbLicenseDAO;
        public static IDBLicenseDAO DBLicenseDAO
        {
            get
            {
                if (dbLicenseDAO == null)
                {
                    string className = namespaceString + ".DBLicenseDAO";
                    dbLicenseDAO = (IDBLicenseDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return dbLicenseDAO;
            }
        }

        private static IUrlDAO urlDAO;
        public static IUrlDAO UrlDAO
        {
            get
            {
                if (urlDAO == null)
                {
                    string className = namespaceString + ".UrlDAO";
                    urlDAO = (IUrlDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return urlDAO;
            }
        }

        //private static IAccountDAO accountDAO;
        //public static IAccountDAO AccountDAO
        //{
        //    get
        //    {
        //        if (accountDAO == null)
        //        {
        //            string className = namespaceString + ".AccountDAO";
        //            accountDAO = (IAccountDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return accountDAO;
        //    }
        //}

        //private static IMessageLogDAO messageLogDAO;
        //public static IMessageLogDAO MessageLogDAO
        //{
        //    get
        //    {
        //        if (messageLogDAO == null)
        //        {
        //            string className = namespaceString + ".MessageLogDAO";
        //            messageLogDAO = (IMessageLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return messageLogDAO;
        //    }
        //}

        //private static IRechargeLogDAO rechargeLogDAO;
        //public static IRechargeLogDAO RechargeLogDAO
        //{
        //    get
        //    {
        //        if (rechargeLogDAO == null)
        //        {
        //            string className = namespaceString + ".RechargeLogDAO";
        //            rechargeLogDAO = (IRechargeLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return rechargeLogDAO;
        //    }
        //}

        private static IHotfixDAO hotfixDAO;
        public static IHotfixDAO HotfixDAO
        {
            get
            {
                if (hotfixDAO == null)
                {
                    string className = namespaceString + ".HotfixDAO";
                    hotfixDAO = (IHotfixDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return hotfixDAO;
            }
        }

        private static IHotfixDownloadDAO hotfixDownloadDAO;
        public static IHotfixDownloadDAO HotfixDownloadDAO
        {
            get
            {
                if (hotfixDownloadDAO == null)
                {
                    string className = namespaceString + ".HotfixDownloadDAO";
                    hotfixDownloadDAO = (IHotfixDownloadDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return hotfixDownloadDAO;
            }
        }

        private static IApplicationDAO applicationDAO;
        public static IApplicationDAO ApplicationDAO
        {
            get
            {
                if (applicationDAO == null)
                {
                    string className = namespaceString + ".ApplicationDAO";
                    applicationDAO = (IApplicationDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return applicationDAO;
            }
        }

        private static IUrlActivityDAO urlActivityDAO;
        public static IUrlActivityDAO UrlActivityDAO
        {
            get
            {
                if (urlActivityDAO == null)
                {
                    string className = namespaceString + ".UrlActivityDAO";
                    urlActivityDAO = (IUrlActivityDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return urlActivityDAO;
            }
        }

        private static ILeadDAO leadDAO;
        public static ILeadDAO LeadDAO
        {
            get
            {
                if (leadDAO == null)
                {
                    string className = namespaceString + ".LeadDAO";
                    leadDAO = (ILeadDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return leadDAO;
            }
        }

        private static IAccountDAO accountDAO;
        public static IAccountDAO AccountDAO
        {
            get
            {
                if (accountDAO == null)
                {
                    string className = namespaceString + ".AccountDAO";
                    accountDAO = (IAccountDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return accountDAO;
            }
        }

        private static IContactDAO contactDAO;
        public static IContactDAO ContactDAO
        {
            get
            {
                if (contactDAO == null)
                {
                    string className = namespaceString + ".ContactDAO";
                    contactDAO = (IContactDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return contactDAO;
            }
        }

        private static IRequestDAO requestDAO;
        public static IRequestDAO RequestDAO
        {
            get
            {
                if (requestDAO == null)
                {
                    string className = namespaceString + ".RequestDAO";
                    requestDAO = (IRequestDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return requestDAO;
            }
        }

        private static IRequestAnswerDAO requestAnswerDAO;
        public static IRequestAnswerDAO RequestAnswerDAO
        {
            get
            {
                if (requestAnswerDAO == null)
                {
                    string className = namespaceString + ".RequestAnswerDAO";
                    requestAnswerDAO = (IRequestAnswerDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return requestAnswerDAO;
            }
        }

        private static IConfigurationDAO configurationDAO;
        public static IConfigurationDAO ConfigurationDAO
        {
            get
            {
                if (configurationDAO == null)
                {
                    string className = namespaceString + ".ConfigurationDAO";
                    configurationDAO = (IConfigurationDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return configurationDAO;
            }
        }

        private static IOrderDAO orderDAO;
        public static IOrderDAO OrderDAO
        {
            get
            {
                if (orderDAO == null)
                {
                    string className = namespaceString + ".OrderDAO";
                    orderDAO = (IOrderDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return orderDAO;
            }
        }

        private static IMobanDAO mobanDAO;
        public static IMobanDAO MobanDAO
        {
            get
            {
                if (mobanDAO == null)
                {
                    string className = namespaceString + ".MobanDAO";
                    mobanDAO = (IMobanDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return mobanDAO;
            }
        }

        private static IFormPageDAO formPageDAO;
        public static IFormPageDAO FormPageDAO
        {
            get
            {
                if (formPageDAO == null)
                {
                    string className = namespaceString + ".FormPageDAO";
                    formPageDAO = (IFormPageDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return formPageDAO;
            }
        }

        private static IFormGroupDAO formGroupDAO;
        public static IFormGroupDAO FormGroupDAO
        {
            get
            {
                if (formGroupDAO == null)
                {
                    string className = namespaceString + ".FormGroupDAO";
                    formGroupDAO = (IFormGroupDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return formGroupDAO;
            }
        }

        private static IFormElementDAO formElementDAO;
        public static IFormElementDAO FormElementDAO
        {
            get
            {
                if (formElementDAO == null)
                {
                    string className = namespaceString + ".FormElementDAO";
                    formElementDAO = (IFormElementDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return formElementDAO;
            }
        }

        private static IOrderFormDAO orderFormDAO;
        public static IOrderFormDAO OrderFormDAO
        {
            get
            {
                if (orderFormDAO == null)
                {
                    string className = namespaceString + ".OrderFormDAO";
                    orderFormDAO = (IOrderFormDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return orderFormDAO;
            }
        }

        private static IOrderRefundDAO orderRefundDAO;
        public static IOrderRefundDAO OrderRefundDAO
        {
            get
            {
                if (orderRefundDAO == null)
                {
                    string className = namespaceString + ".OrderRefundDAO";
                    orderRefundDAO = (IOrderRefundDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return orderRefundDAO;
            }
        }

        private static IInvoiceDAO invoiceDAO;
        public static IInvoiceDAO InvoiceDAO
        {
            get
            {
                if (invoiceDAO == null)
                {
                    string className = namespaceString + ".InvoiceDAO";
                    invoiceDAO = (IInvoiceDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return invoiceDAO;
            }
        }

        private static IOrderFormSEMDAO orderFormSEMDAO;
        public static IOrderFormSEMDAO OrderFormSEMDAO
        {
            get
            {
                if (orderFormSEMDAO == null)
                {
                    string className = namespaceString + ".OrderFormSEMDAO";
                    orderFormSEMDAO = (IOrderFormSEMDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return orderFormSEMDAO;
            }
        }

        private static IContractDAO contractDAO;
        public static IContractDAO ContractDAO
        {
            get
            {
                if (contractDAO == null)
                {
                    string className = namespaceString + ".ContractDAO";
                    contractDAO = (IContractDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return contractDAO;
            }
        }

        private static ITouchDAO touchDAO;
        public static ITouchDAO TouchDAO
        {
            get
            {
                if (touchDAO == null)
                {
                    string className = namespaceString + ".TouchDAO";
                    touchDAO = (ITouchDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return touchDAO;
            }
        }

        private static IOrderMessageDAO orderMessageDAO;
        public static IOrderMessageDAO OrderMessageDAO
        {
            get
            {
                if (orderMessageDAO == null)
                {
                    string className = namespaceString + ".OrderMessageDAO";
                    orderMessageDAO = (IOrderMessageDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return orderMessageDAO;
            }
        }
    }
}
