using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;

namespace SiteServer.Project.Model
{
    public class ProjectInfoExtend : ExtendedAttributes
    {
        public ProjectInfoExtend(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public int PageSize
        {
            get { return base.GetInt("PageSize", 30); }
            set { base.SetExtendedAttribute("PageSize", value.ToString()); }
        }

        /****************文件上传设置********************/

        public string UploadImageTypeCollection
        {
            get { return base.GetString("UploadImageTypeCollection", "gif|jpg|jpeg|bmp|png|swf|flv"); }
            set { base.SetExtendedAttribute("UploadImageTypeCollection", value); }
        }

        public int UploadImageTypeMaxSize
        {
            get { return base.GetInt("UploadImageTypeMaxSize", 2048); }
            set { base.SetExtendedAttribute("UploadImageTypeMaxSize", value.ToString()); }
        }

        public string UploadMediaTypeCollection
        {
            get { return base.GetString("UploadMediaTypeCollection", "rm|rmvb|mp3|flv|wav|mid|midi|ra|avi|mpg|mpeg|asf|asx|wma|mov"); }
            set { base.SetExtendedAttribute("UploadMediaTypeCollection", value); }
        }

        public int UploadMediaTypeMaxSize
        {
            get { return base.GetInt("UploadMediaTypeMaxSize", 5120); }
            set { base.SetExtendedAttribute("UploadMediaTypeMaxSize", value.ToString()); }
        }

        public string UploadFileTypeCollectionForbidden
        {
            get { return base.GetString("UploadFileTypeCollectionForbidden", "ade,adp,app,asa,asp,asmx,aspx,bas,bat,cdx,cer,cmd,com,cpl,crt,dll,exe,fxp,hlp,hta,htr,ins,isp,jse,jsp,lnk,mda,mdb,mde,mdt,mdw,mdz,msc,msi,msp,mst,ops,pcd,pif,prf,prg,reg,scf,scr,sct,shb,shs,url,vb,vbe,vbs,wsc,wsf,wsh"); }
            set { base.SetExtendedAttribute("UploadFileTypeCollectionForbidden", value); }
        }

        public int UploadFileTypeMaxSize
        {
            get { return base.GetInt("UploadFileTypeMaxSize", 5120); }
            set { base.SetExtendedAttribute("UploadFileTypeMaxSize", value.ToString()); }
        }

        public string UploadDirectoryName
        {
            get { return base.GetString("UploadDirectoryName", "upload"); }
            set { base.SetExtendedAttribute("UploadDirectoryName", value); }
        }

        public string UploadDateFormatString
        {
            get { return base.GetString("UploadDateFormatString", EDateFormatTypeUtils.GetValue(EDateFormatType.Month)); }
            set { base.SetExtendedAttribute("UploadDateFormatString", value); }
        }

        public bool IsUploadChangeFileName
        {
            get { return base.GetBool("IsUploadChangeFileName", true); }
            set { base.SetExtendedAttribute("IsUploadChangeFileName", value.ToString()); }
        }

        /****************其他********************/

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }
    }
}
