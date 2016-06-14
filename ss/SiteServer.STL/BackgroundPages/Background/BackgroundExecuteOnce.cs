using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.STL.Parser.TemplateDesign;
using SiteServer.CMS.Model;
using SiteServer.CMS.Services;
using SiteServer.STL.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
    public class BackgroundExecuteOnce : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            int count = 0;
            try
            {
                string publishmentSystemDir = base.Request.QueryString["publishmentSystemDir"];
                int dataAppID = Convert.ToInt32(base.Request.QueryString["dataAppID"]);
                int publishmentSystemID = DataProvider.PublishmentSystemDAO.GetPublishmentSystemIDByPublishmentSystemDir(publishmentSystemDir);

                if (publishmentSystemID > 0)
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    publishmentSystemInfo.Additional.ZDH_DataAppID = dataAppID;
                    publishmentSystemInfo.PublishmentSystemID = publishmentSystemID;
                    DataProvider.PublishmentSystemDAO.Update(publishmentSystemInfo);

                    base.Response.Write(string.Format("UpdateSucess"));

                    FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                    FSO.CreateAll();
                    count++;
                }
            }
            catch { }

            base.Response.Write(string.Format("executed over, total count: {0}, executed count: {1}", "1", count));

            base.Response.End();
        }
    }
}
