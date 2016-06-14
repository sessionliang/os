using BaiRong.Core;
using BaiRong.Model;
using SiteServer.API.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace SiteServer.API.Controllers
{
    /// <summary>
    /// by 20151125 sofuny
    /// 学习中心
    /// </summary>
    [RoutePrefix("api/services/organization")]
    public class OrganizationController : ApiController
    {
        private object outPut;

        [HttpPost]
        [Route("action")]
        public HttpResponseMessage Action()
        {
            string type = RequestUtils.GetRequestString("type");
            if (!string.IsNullOrEmpty(type))
            {
                if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_OrganizationQuery))
                {
                    int classifyID = RequestUtils.GetIntRequestString("ClassifyID");
                    int areaPID = RequestUtils.GetIntRequestString("AreaPID");
                    int areaID = RequestUtils.GetIntRequestString("AreaID");

                    this.OrganizationQuery(classifyID, areaPID, areaID);

                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_OrganizationClassifyQuery))
                {
                    this.OrganizationClassifyQuery();

                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_OrganizationAreaByClassifyQuery))
                {
                    int classifyID = RequestUtils.GetIntRequestString("ClassifyID");
                    this.OrganizationAreaByClassifyQuery(classifyID);

                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_OrganizationAreaByParentIDQuery))
                {
                    int parentID = RequestUtils.GetIntRequestString("ParentID");
                    this.OrganizationAreaByParentIDQuery(parentID);

                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_OrganizationShark))
                {
                    int classifyID = RequestUtils.GetIntRequestString("ClassifyID");
                    this.OrganizationShark(classifyID);

                }
            } 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, outPut, new MediaTypeHeaderValue("application/json"));
            return response;
        }


        private void OrganizationQuery(int classifyID, int areaPID, int areaID)
        {
            try
            {
                ArrayList alist = DataProvider.OrganizationInfoDAO.GetInfoList(classifyID, areaPID, areaID);

                List<object> list = new List<object>();
                foreach (OrganizationInfo info in alist)
                {
                    list.Add(new { ID = info.ID, Name = info.OrganizationName, Address = info.OrganizationAddress, Explain = info.Explain, Phone = info.Phone, Longitude = info.Longitude, Latitude = info.Latitude, LogoUrl = info.LogoUrl, ContentNum = info.ContentNum });
                }
                outPut = new { IsSuccess = true, Info = list };

            }
            catch (Exception ex)
            {
                outPut = new { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

        private void OrganizationShark(int classifyID)
        {
            try
            {

                if (RequestUtils.GetRequestString("Lat") != null && RequestUtils.GetRequestString("Lon") != null && RequestUtils.GetRequestString("Raidus") != null)
                {
                    string slat = RequestUtils.GetRequestString("Lat");
                    double lat = Convert.ToDouble(slat);
                    double lon = Convert.ToDouble(RequestUtils.GetRequestString("Lon"));
                    double raidus = Convert.ToInt64(RequestUtils.GetRequestString("Raidus"));

                    double minLat, maxLat, minLng, maxLng;

                    getAround(lat, lon, raidus, out  minLat, out   maxLat, out   minLng, out   maxLng);

                    ArrayList alist = DataProvider.OrganizationInfoDAO.GetInfoList(classifyID, minLat, maxLat, minLng, maxLng);
                    //获取每个点的距离                    

                    List<object> list = new List<object>();
                    foreach (OrganizationInfo info in alist)
                    {
                        double distance = double.Parse(getDistance(lat, lon, double.Parse(info.Latitude), double.Parse(info.Longitude)).ToString());
                        string distanceStr = "距离" + Math.Round((distance / 1000), 1) + "千米";

                        if (Math.Round((distance / 1000), 1) <= 10)
                            list.Add(new { ID = info.ID, Name = info.OrganizationName, Address = info.OrganizationAddress, Explain = info.Explain, Phone = info.Phone, Longitude = info.Longitude, Latitude = info.Latitude, LogoUrl = info.LogoUrl, ContentNum = info.ContentNum, Distance = distanceStr });
                    }
                    outPut = new { IsSuccess = true, Info = list };

                }
            }
            catch (Exception ex)
            {
                outPut = new { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

        private void OrganizationClassifyQuery()
        {
            try
            {
                ArrayList alist = DataProvider.OrganizationClassifyDAO.GetInfoList(string.Empty);

                List<object> list = new List<object>();
                foreach (OrganizationClassifyInfo info in alist)
                {
                    list.Add(new { ItemID = info.ItemID, ItemName = info.ItemName, ContentNum = info.ContentNum, ParentID = info.ParentID });
                }
                outPut = new { IsSuccess = true, ClassifyInfo = list };

            }
            catch (Exception ex)
            {
                outPut = new { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }
        private void OrganizationAreaByClassifyQuery(int classifyID)
        {
            try
            {
                ArrayList alist = DataProvider.OrganizationAreaDAO.getParentArea(classifyID);

                List<object> list = new List<object>();
                foreach (OrganizationAreaInfo info in alist)
                {
                    list.Add(new { ItemID = info.ItemID, ItemName = info.ItemName });
                }
                outPut = new { IsSuccess = true, AreaInfo = list };

            }
            catch (Exception ex)
            {
                outPut = new { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }
        private void OrganizationAreaByParentIDQuery(int parentID)
        {
            try
            {
                ArrayList alist = DataProvider.OrganizationAreaDAO.getChildArea(parentID);

                List<object> list = new List<object>();
                foreach (OrganizationAreaInfo info in alist)
                {
                    list.Add(new { ItemID = info.ItemID, ItemName = info.ItemName });
                }
                outPut = new { IsSuccess = true, AreaInfo = list };

            }
            catch (Exception ex)
            {
                outPut = new { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 查找一定范围内的经纬度值
        /// 传入值：纬度  经度  查找半径(m)
        /// 返回值：最小纬度、经度，最大纬度、经度 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="raidus"></param>
        public void getAround(double lat, double lon, double raidus, out double minLat, out double maxLat, out double minLng, out double maxLng)
        {
            double latitude = lat;
            double longitude = lon;

            double degree = (double)((24901 * 1609) / 360.0);
            double raidusMile = raidus;

            double dpmLat = 1 / degree;
            double radiusLat = dpmLat * raidusMile;
            minLat = latitude - radiusLat;
            maxLat = latitude + radiusLat;

            double mpdLng = (double)(degree * Math.Cos(latitude * (Math.PI / 180)));
            double dpmLng = 1 / mpdLng;
            double radiusLng = dpmLng * raidusMile;
            minLng = longitude - radiusLng;
            maxLng = longitude + radiusLng;
        }


        /// <summary>
        /// 两个坐标之间的实际距离
        /// </summary>
        /// <param name="lat_a"></param>
        /// <param name="lng_a"></param>
        /// <param name="lat_b"></param>
        /// <param name="lng_b"></param>
        /// <returns></returns>
        public double getDistance(double lat_a, double lng_a, double lat_b, double lng_b)
        {
            double pk = (double)(180 / 3.14169);
            double a1 = lat_a / pk;
            double a2 = lng_a / pk;
            double b1 = lat_b / pk;
            double b2 = lng_b / pk;
            double t1 = Math.Cos(a1) * Math.Cos(a2) * Math.Cos(b1) * Math.Cos(b2);
            double t2 = Math.Cos(a1) * Math.Sin(a2) * Math.Cos(b1) * Math.Sin(b2);
            double t3 = Math.Sin(a1) * Math.Sin(b1);
            double tt = Math.Acos(t1 + t2 + t3);
            return 6366000 * tt;
        }
    }
}
