using System;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SiteServer.Project.Model;
using SiteServer.Project.Core;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Cryptography;

namespace SiteServer.Project.BackgroundPages.Modal
{
    public class LicenseView : BackgroundBasePage
    {
        //public Literal ltlProductID;
        public Literal ltlDomain;
        public Literal ltlMacAddress;

        public Literal ltlMaxSiteNumber;
        public Literal ltlLicenseDate;
        public Literal ltlSiteName;
        public Literal ltlClientName;
        public PlaceHolder phExpireDate;
        public Literal ltlExpireDate;

        private int id;

        public static string GetShowPopWinString(int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ID", id.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("许可证查看", "modal_product_licenseView.aspx", arguments, 400, 560, true);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.id = int.Parse(base.Request.QueryString["ID"]);

            if (!IsPostBack)
            {
                DBLicenseInfo licenseInfo = DataProvider.DBLicenseDAO.GetLicenseInfo(this.id);

                //this.ltlProductID.Text = ProductManager.GetProductName(licenseInfo.ProductID, true);
                this.ltlDomain.Text = licenseInfo.Domain;
                this.ltlMacAddress.Text = licenseInfo.MacAddress;
                
                this.ltlMaxSiteNumber.Text = licenseInfo.MaxSiteNumber.ToString();
                if (licenseInfo.MaxSiteNumber == 0)
                {
                    this.ltlMaxSiteNumber.Text = "不限制";
                }

                this.ltlLicenseDate.Text = DateUtils.GetDateString(licenseInfo.LicenseDate);
                this.ltlSiteName.Text = licenseInfo.SiteName;
                this.ltlClientName.Text = licenseInfo.ClientName;
                this.phExpireDate.Visible = false;
                this.ltlExpireDate.Text = DateUtils.GetDateString(licenseInfo.ExpireDate);
            }
        }

        public void btnDownload_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                DBLicenseInfo licenseInfo = DataProvider.DBLicenseDAO.GetLicenseInfo(this.id);
                string filePathXML = PathUtils.GetTemporaryFilesPath("license.xml");
                string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<registration>
	<computer>
		<domain>{0}</domain>
		<macAddress>{1}</macAddress>
		<processorID>{2}</processorID>
		<columnSerialNumber>{3}</columnSerialNumber>
	</computer>
	
	<product>
		<owner>{4}</owner>
		<maxSiteNumber>{5}</maxSiteNumber>
        <expireDate>{6}</expireDate>
	</product>
</registration>
", licenseInfo.Domain, licenseInfo.MacAddress, licenseInfo.ProcessorID, licenseInfo.ColumnSerialNumber, licenseInfo.ClientName, licenseInfo.MaxSiteNumber.ToString(), licenseInfo.ExpireDate);

                FileUtils.DeleteFileIfExists(filePathXML);
                FileUtils.WriteText(filePathXML, ECharset.gb2312, xml);

                string filePathLicense = PathUtils.GetTemporaryFilesPath("license.lic");
                FileUtils.DeleteFileIfExists(filePathLicense);

                LicenseView.CreateLicense(filePathXML, filePathLicense);

                PageUtils.Download(base.Response, filePathLicense);
            }
            catch
            {
                isChanged = false;
            }

            if (isChanged)
            {
                //JsUtils.SubModal.CloseModalPageAndRedirect(Page, string.Format("../background/background_catalog.aspx?ParentID={0}&Level={1}", this.parentID, this.level));
            }
        }

        private static void CreateLicense(string filePathXML, string filePathLicense)
        {
            new DESEncryptor { InputFilePath = filePathXML, OutFilePath = filePathLicense, EncryptKey = "kesgf4zb" }.FileDesEncrypt();
        }

        private bool Encrypt(string filePath, string licenseXML)
        {
            bool isSuccess = false;
            try
            {
                FileUtils.DeleteFileIfExists(filePath);
                DESEncryptor encryptor = new DESEncryptor();
                encryptor.InputString = licenseXML;
                encryptor.EncryptKey = "kesgf4zb";
                encryptor.DesEncrypt();

                FileUtils.WriteText(filePath, ECharset.utf_8, encryptor.OutString);

                isSuccess = true;
                //encryptor.InputString = FileUtils.ReadBase64StringFromFile(filePath);
                //encryptor.DecryptKey = "kesgf4zb";
                //encryptor.DesDecrypt();
                //string xmlContent = encryptor.OutString.Substring(encryptor.OutString.IndexOf("\r\n") + 2);
                //xmlDocument.LoadXml(xmlContent);
            }
            catch { }

            return isSuccess;
        }
    }
}
