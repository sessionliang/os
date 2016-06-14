using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.API.Model.WX;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;

namespace SiteServer.API.Controllers.WX
{
    public class WX_SearchController : ApiController
    {
        [HttpGet]
        [ActionName("GetSearchParameter")]
        public IHttpActionResult GetSearchParameter(int id)
        {
            try
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

                DataProviderWX.SearchDAO.AddPVCount(id);

                SearchInfo searchInfo = DataProviderWX.SearchDAO.GetSearchInfo(id);

                List<SearchNavigationInfo> navigationInfoList = DataProviderWX.SearchNavigationDAO.GetSearchNavigationInfoList(publishmentSystemInfo.PublishmentSystemID, id);
                foreach (SearchNavigationInfo navigationInfo in navigationInfoList)
                {
                    navigationInfo.Url = NavigationUtils.GetNavigationUrl(publishmentSystemInfo, ENavigationTypeUtils.GetEnumType(navigationInfo.NavigationType), EKeywordTypeUtils.GetEnumType(navigationInfo.KeywordType), navigationInfo.FunctionID, navigationInfo.ChannelID, navigationInfo.ContentID, navigationInfo.Url);
                }

                searchInfo.ImageUrl = SearchManager.GetImageUrl(publishmentSystemInfo, searchInfo.ImageUrl);
                searchInfo.ContentImageUrl = SearchManager.GetContentImageUrl(publishmentSystemInfo, searchInfo.ContentImageUrl);

                List<SearchContentInfo> imageContentInfoList = new List<SearchContentInfo>();
                List<SearchContentInfo> textContentInfoList = new List<SearchContentInfo>();

                if (searchInfo.ImageAreaChannelID > 0 && NodeManager.IsExists(publishmentSystemInfo.PublishmentSystemID, searchInfo.ImageAreaChannelID))
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, searchInfo.ImageAreaChannelID);
                    ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                    ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, searchInfo.ImageAreaChannelID, 3, "ORDER BY ID DESC", " AND ImageUrl <> ''", EScopeType.All);

                    foreach (int contentID in contentIDArrayList)
                    {
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                        SearchContentInfo sContentInfo = new SearchContentInfo { ID = contentInfo.ID, Title = contentInfo.Title, ImageUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl))), NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, EVisualType.Static), AddDate = contentInfo.AddDate };

                        imageContentInfoList.Add(sContentInfo);
                    }
                }

                if (searchInfo.TextAreaChannelID > 0 && NodeManager.IsExists(publishmentSystemInfo.PublishmentSystemID, searchInfo.TextAreaChannelID))
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, searchInfo.TextAreaChannelID);
                    ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                    ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, searchInfo.TextAreaChannelID, 6, "ORDER BY ID DESC", string.Empty, EScopeType.All);

                    foreach (int contentID in contentIDArrayList)
                    {
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                        SearchContentInfo sContentInfo = new SearchContentInfo { ID = contentInfo.ID, Title = StringUtils.MaxLengthText(contentInfo.Title, 10, string.Empty), NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, EVisualType.Static), AddDate = contentInfo.AddDate };

                        textContentInfoList.Add(sContentInfo);
                    }
                }

                SearchParameter parameter = new SearchParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, SearchInfo = searchInfo, SearchNavigationInfoList = navigationInfoList, ImageContentInfoList = imageContentInfoList, TextContentInfoList = textContentInfoList };
                
                return Ok(parameter);
            }
            catch (Exception ex)
            {
                SearchParameter parameter = new SearchParameter { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };
                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("GetRefreshParameter")]
        public IHttpActionResult GetRefreshParameter(int id)
        {
            try
            {
                string imageIDCollection = RequestUtils.GetQueryString("imageIDCollection");
                string textIDCollection = RequestUtils.GetQueryString("textIDCollection");

                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

                SearchInfo searchInfo = DataProviderWX.SearchDAO.GetSearchInfo(id);

                List<SearchContentInfo> imageContentInfoList = null;
                List<SearchContentInfo> textContentInfoList = null;

                if (!string.IsNullOrEmpty(imageIDCollection))
                {
                    imageContentInfoList = new List<SearchContentInfo>();

                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, searchInfo.ImageAreaChannelID);
                    ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                    ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, searchInfo.ImageAreaChannelID, 3, "ORDER BY ID DESC", string.Format(" AND ImageUrl <> '' AND ID NOT IN ({0})", imageIDCollection), EScopeType.All);

                    foreach (int contentID in contentIDArrayList)
                    {
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                        SearchContentInfo sContentInfo = new SearchContentInfo { ID = contentInfo.ID, Title = contentInfo.Title, ImageUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl))), NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, EVisualType.Static), AddDate = contentInfo.AddDate };

                        imageContentInfoList.Add(sContentInfo);
                    }
                }
                else if (!string.IsNullOrEmpty(textIDCollection))
                {
                    textContentInfoList = new List<SearchContentInfo>();

                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, searchInfo.TextAreaChannelID);
                    ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                    ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, searchInfo.TextAreaChannelID, 6, "ORDER BY ID DESC", string.Format(" AND ID NOT IN ({0})", textIDCollection), EScopeType.All);

                    foreach (int contentID in contentIDArrayList)
                    {
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                        SearchContentInfo sContentInfo = new SearchContentInfo { ID = contentInfo.ID, Title = StringUtils.MaxLengthText(contentInfo.Title, 10, string.Empty), NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, EVisualType.Static), AddDate = contentInfo.AddDate };

                        textContentInfoList.Add(sContentInfo);
                    }
                }

                SearchParameter parameter = new SearchParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, ImageContentInfoList = imageContentInfoList, TextContentInfoList = textContentInfoList };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                SearchParameter parameter = new SearchParameter { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };
                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("GetSearchResultParameter")]
        public IHttpActionResult GetSearchResultParameter(int id)
        {
            try
            {
                string keywords = RequestUtils.GetQueryString("keywords");

                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);
                  
                SearchInfo searchInfo = DataProviderWX.SearchDAO.GetSearchInfo(id);
                List<SearchNavigationInfo> navigationInfoList = DataProviderWX.SearchNavigationDAO.GetSearchNavigationInfoList(publishmentSystemInfo.PublishmentSystemID, id);
                foreach (SearchNavigationInfo navigationInfo in navigationInfoList)
                {
                    navigationInfo.Url = NavigationUtils.GetNavigationUrl(publishmentSystemInfo, ENavigationTypeUtils.GetEnumType(navigationInfo.NavigationType), EKeywordTypeUtils.GetEnumType(navigationInfo.KeywordType), navigationInfo.FunctionID, navigationInfo.ChannelID, navigationInfo.ContentID, navigationInfo.Url);
                }

                searchInfo.ImageUrl = SearchManager.GetImageUrl(publishmentSystemInfo, searchInfo.ImageUrl);
                searchInfo.ContentImageUrl = SearchManager.GetContentImageUrl(publishmentSystemInfo, searchInfo.ContentImageUrl);

                List<SearchContentInfo> contentInfoList = new List<SearchContentInfo>();

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.PublishmentSystemID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, publishmentSystemInfo.PublishmentSystemID, 10, "ORDER BY ID DESC", string.Format(" AND TITLE LIKE '%{0}%'", keywords), EScopeType.All);

                foreach (int contentID in contentIDArrayList)
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                    string summary = StringUtils.MaxLengthText(StringUtils.StripTags(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Content)), 55);

                    SearchContentInfo sContentInfo = new SearchContentInfo { ID = contentInfo.ID, Title = StringUtils.MaxLengthText(contentInfo.Title, 13, string.Empty), NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, EVisualType.Static), Summary = summary, AddDate = contentInfo.AddDate };

                    contentInfoList.Add(sContentInfo);
                }

                SearchParameter parameter = new SearchParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, SearchInfo = searchInfo, SearchNavigationInfoList = navigationInfoList, ContentInfoList = contentInfoList };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                SearchParameter parameter = new SearchParameter { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };
                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("GetMoreResultParameter")]
        public IHttpActionResult GetMoreResultParameter(int id)
        {
            try
            {
                string keywords = RequestUtils.GetQueryString("keywords");
                string idCollection = RequestUtils.GetQueryString("idCollection");

                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

                List<SearchContentInfo> contentInfoList = new List<SearchContentInfo>();

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.PublishmentSystemID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, publishmentSystemInfo.PublishmentSystemID, 10, "ORDER BY ID DESC", string.Format(" AND TITLE LIKE '%{0}%' AND ID NOT IN ({1})", keywords, idCollection), EScopeType.All);

                foreach (int contentID in contentIDArrayList)
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                    string summary = StringUtils.MaxLengthText(StringUtils.StripTags(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Content)), 55);

                    SearchContentInfo sContentInfo = new SearchContentInfo { ID = contentInfo.ID, Title = StringUtils.MaxLengthText(contentInfo.Title, 13, string.Empty), NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, EVisualType.Static), Summary = summary, AddDate = contentInfo.AddDate };

                    contentInfoList.Add(sContentInfo);
                }

                SearchParameter parameter = new SearchParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, ContentInfoList = contentInfoList };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                SearchParameter parameter = new SearchParameter { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };
                return Ok(parameter);
            }
        }
    }
}
