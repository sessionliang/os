using BaiRong.Core;
using BaiRong.Model;
using Senparc.Weixin.MP;
using SiteServer.API.Model.WX;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;


using MessageManager = SiteServer.WeiXin.Core.MessageManager;

namespace SiteServer.API.Controllers.WX
{
    public class WX_View360Controller : ApiController
    {
        [HttpGet]
        [ActionName("GetConfigXML")]
        public HttpResponseMessage GetConfigXML()
        {
            int publishmentSystemID = RequestUtils.GetIntQueryString("publishmentSystemID");
            int view360ID = RequestUtils.GetIntQueryString("view360ID");

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            View360Info viewInfo = DataProviderWX.View360DAO.GetView360Info(view360ID);

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(@"tile0url=""{0}"" ", View360Manager.GetContentImageUrl(publishmentSystemInfo, viewInfo.ContentImageUrl1, 1));
            builder.AppendFormat(@"tile1url=""{0}"" ", View360Manager.GetContentImageUrl(publishmentSystemInfo, viewInfo.ContentImageUrl2, 2));
            builder.AppendFormat(@"tile2url=""{0}"" ", View360Manager.GetContentImageUrl(publishmentSystemInfo, viewInfo.ContentImageUrl3, 3));
            builder.AppendFormat(@"tile3url=""{0}"" ", View360Manager.GetContentImageUrl(publishmentSystemInfo, viewInfo.ContentImageUrl4, 4));
            builder.AppendFormat(@"tile4url=""{0}"" ", View360Manager.GetContentImageUrl(publishmentSystemInfo, viewInfo.ContentImageUrl5, 5));
            builder.AppendFormat(@"tile5url=""{0}"" ", View360Manager.GetContentImageUrl(publishmentSystemInfo, viewInfo.ContentImageUrl6, 6));
            
            string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<panorama id="""">
  <view fovmode=""0"" pannorth=""0"">
    <start pan=""0"" fov=""70"" tilt=""0""/>
    <min pan=""0"" fov=""5"" tilt=""-90""/>
    <max pan=""360"" fov=""120"" tilt=""90""/>
  </view>
  <userdata title=""dddddddd"" datetime=""2011:11:03 09:41:07"" description=""description"" copyright=""copyright"" tags=""tags"" author=""author"" source=""source"" comment=""comment"" info=""info"" longitude=""0"" latitude=""""/>
  <hotspots width=""180"" height=""20"" wordwrap=""1"">
    <label width=""180"" backgroundalpha=""0.5"" enabled=""1"" height=""20"" backgroundcolor=""0xffffff"" bordercolor=""0x000000"" border=""0"" textcolor=""0x000000"" borderalpha=""0.5"" borderradius=""1"" wordwrap=""1"" textalpha=""1""/>
    <polystyle mode=""0"" backgroundalpha=""0.2509803921568627"" backgroundcolor=""0x0000ff"" bordercolor=""0x0000ff"" borderalpha=""1""/>
   </hotspots>
  <media/>
  <input tilesize=""685"" tilescale=""1"" {0}/>
  <autorotate speed=""0.200"" nodedelay=""0.00"" startloaded=""1"" returntohorizon=""0.000"" delay=""5.00""/>
  <control simulatemass=""1"" lockedmouse=""0"" lockedkeyboard=""0"" dblclickfullscreen=""0"" invertwheel=""0"" lockedwheel=""0"" invertcontrol=""1"" speedwheel=""1"" sensitivity=""8""/>
</panorama>", builder);

            return new HttpResponseMessage()
            {
                Content = new StringContent(xml, Encoding.UTF8, "application/xml")
            };
        }
    }
}
