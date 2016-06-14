using System;
using System.Text;
using System.Net;
using System.Web.UI;
using System.Collections.Specialized;
using System.Web;
using System.Data;
using BaiRong.Core;
using System.Collections;
using BaiRong.Text.LitJson;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages
{
    public class Test : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                StringBuilder builder = new StringBuilder();
                ArrayList sqlArrayList = new ArrayList();
                using (IDataReader rdr = BaiRongDataProvider.DatabaseDAO.GetDataReader("server=(local);uid=sa;pwd=bairong;database=siteserver.cn", "select id, fileurl, SettingsXML from cms_Content where fileurl <> ''"))
                {
                    while (rdr.Read())
                    {
                        int id = rdr.GetInt32(0);
                        string fileUrl = rdr.GetString(1);

                        if (fileUrl.Contains("/V"))
                        {
                            string uid = fileUrl.Substring(fileUrl.IndexOf("/V") + 1);
                            uid = uid.Substring(0, uid.LastIndexOf("/"));

                            string fileName = fileUrl.Substring(fileUrl.LastIndexOf("/T_") + 3);
                            fileName = fileName.Substring(0, fileName.IndexOf("."));

                            uid = uid + fileName.ToUpper();

                            string sqlString = string.Format(@"update cms_Content set UID = '{0}' where id = {1}", uid, id);
                            sqlArrayList.Add(sqlString);

                            builder.AppendFormat("fileUrl:{0},UID:{1},fileName:{2}<br />", fileUrl, uid, fileName);
                        }
                    }
                    rdr.Close();
                }

                BaiRongDataProvider.DatabaseDAO.ExecuteSql("server=(local);uid=sa;pwd=bairong;database=siteserver.cn", sqlArrayList);

                Response.Write(builder.ToString());
                Response.End();
            }
        }
    }
}
