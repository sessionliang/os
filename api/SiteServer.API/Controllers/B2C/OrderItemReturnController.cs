using BaiRong.Core;
using BaiRong.Model;
using SiteServer.API.Model;
using SiteServer.API.Model.B2C;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;



namespace SiteServer.API.Controllers.B2C
{
    public class OrderItemReturnController : ApiController
    {

        public const string UserOrderItemReturnImageFileName = "OrderItemReturn";

        /// <summary>
        /// ��ȡ���е��˻���¼����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetOrderItemReturnRecordList")]
        public IHttpActionResult GetOrderItemReturnRecordList()
        {
            #region ��ҳ����
            int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
            int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));
            int orderTime = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["orderTime"], 0);
            string keywords = PageUtils.FilterSql(HttpContext.Current.Request.QueryString["keywords"]);

            string SQL_WHERE = string.Format(" ApplyUser = '{0}' ", RequestUtils.CurrentUserName);

            if (orderTime > 0)
            {
                SQL_WHERE += string.Format(" AND ApplyDate > getdate() - {0} ", orderTime);
            }


            if (!string.IsNullOrEmpty(keywords))
            {
                SQL_WHERE += string.Format(" AND (Title like '%{0}%' OR GoodsSN like '%{0}%') ", keywords);
            }

            int total = DataProviderB2C.OrderItemReturnDAO.GetCount(SQL_WHERE);
            string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);
            #endregion
            List<OrderItemReturnInfo> OrderItemReturnList = DataProviderB2C.OrderItemReturnDAO.GetItemReturnInfoList(SQL_WHERE, pageIndex, prePageNum);
            List<OrderItemReturnParameter> OrderItemReturnParameterList = new List<OrderItemReturnParameter>();
            foreach (OrderItemReturnInfo orderItemReturnInfo in OrderItemReturnList)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(orderItemReturnInfo.PublishmentSystemID);
                PublishmentSystemParameter publishmentSystemParameter = new PublishmentSystemParameter() { PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID, PublishmentSystemName = publishmentSystemInfo.PublishmentSystemName, PublishmentSystemUrl = publishmentSystemInfo.PublishmentSystemUrl };

                OrderItemInfo orderItemInfo = DataProviderB2C.OrderItemDAO.GetItemInfo(orderItemReturnInfo.OrderItemID);

                OrderItemReturnParameter OrderItemReturnParameterInfo = new OrderItemReturnParameter(orderItemReturnInfo);

                OrderItemReturnParameterInfo.NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, orderItemInfo.ChannelID), orderItemInfo.ContentID, publishmentSystemInfo.Additional.VisualType);
    

                if (EReturnMoneyStatusUtils.Equals(orderItemReturnInfo.ReturnMoneyStatus, EReturnMoneyStatus.Refund))
                {
                    OrderItemReturnParameterInfo.DetailStatus = EReturnMoneyStatusUtils.GetText(EReturnMoneyStatus.Refund);
                }
                else if (EReturnOrderStatusUtils.Equals(orderItemReturnInfo.ReturnOrderStatus, EReturnOrderStatus.Received))
                {
                    OrderItemReturnParameterInfo.DetailStatus = EReturnOrderStatusUtils.GetText(EReturnOrderStatus.Received);
                }
                else if (EAuditStatusUtils.Equals(orderItemReturnInfo.AuditStatus, EAuditStatus.Pass))
                {
                    OrderItemReturnParameterInfo.DetailStatus = EAuditStatusUtils.GetText(EAuditStatus.Pass);
                }
                else if (EAuditStatusUtils.Equals(orderItemReturnInfo.AuditStatus, EAuditStatus.UnPass))
                {
                    OrderItemReturnParameterInfo.DetailStatus = EAuditStatusUtils.GetText(EAuditStatus.UnPass);
                }
                else
                {
                    OrderItemReturnParameterInfo.DetailStatus = EAuditStatusUtils.GetText(EAuditStatus.Wait);
                }

                OrderItemReturnParameterList.Add(OrderItemReturnParameterInfo);
            }
            var orderInfoListParms = new { pageJson = pageJson, orderItemReturnRecordList = OrderItemReturnParameterList };
            return Ok(orderInfoListParms);
        }


        /// <summary>
        /// �ϴ��˻���Ƭ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UploadReturnImage")]
        public IHttpActionResult UploadReturnImage()
        {
            try
            {
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    UserInfo user = RequestUtils.Current;
                    int orderItemID = RequestUtils.GetIntQueryString("orderItemID");
                    OrderItemInfo orderItemInfo = DataProviderB2C.OrderItemDAO.GetItemInfo(orderItemID);


                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    string fileName = PathUtils.GetFileName(file.FileName);
                    string fileExtend = PathUtils.GetExtension(fileName).Trim('.');
                    EImageType imageType = EImageTypeUtils.GetEnumType(fileExtend);
                    //������ļ���
                    string localFileName = PathUtility.GetUploadFileName(RequestUtils.PublishmentSystemInfo, fileName);
                    //������ļ�����
                    string localDirectoryPath = PathUtility.GetUploadDirectoryPath(RequestUtils.PublishmentSystemInfo, fileExtend);
                    string localFilePath = PathUtils.Combine(localDirectoryPath, UserOrderItemReturnImageFileName, orderItemID.ToString(), localFileName);

                    localFilePath = APIPageUtils.ParseUrl(localFilePath);

                    if (!PathUtility.IsImageExtenstionAllowed(RequestUtils.PublishmentSystemInfo, fileExtend))
                    {
                        var errorParm = new { isSuccess = false, errorMessage = "�ϴ�ʧ�ܣ��ϴ�ͼƬ��ʽ����ȷ��" };
                        return Ok(errorParm);
                    }
                    if (!PathUtility.IsImageSizeAllowed(RequestUtils.PublishmentSystemInfo, file.ContentLength))
                    {
                        var errorParm = new { isSuccess = false, errorMessage = "�ϴ�ʧ�ܣ��ϴ�ͼƬ�����涨�ļ���С��" };
                        return Ok(errorParm);
                    }

                    bool isImage = EFileSystemTypeUtils.IsImage(fileExtend);

                    if (isImage)
                    {
                        //�����˻�ͼƬ
                        DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(localFilePath));
                        file.SaveAs(localFilePath);

                        string imageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(RequestUtils.PublishmentSystemInfo, localFilePath);

                        return Ok(new { isSuccess = true, imageUrl = PageUtils.AddProtocolToUrl(imageUrl) });
                    }

                }
                return Ok(new { isSuccess = false });
            }
            catch (Exception ex)
            {
                var errorParm = new { isSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        /// <summary>
        /// ���涩���˻�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("SaveOrderItemReturn")]
        public IHttpActionResult SaveOrderItemReturn()
        {
            try
            {
                int orderItemID = RequestUtils.GetIntQueryString("orderItemID");
                string returnType = RequestUtils.GetQueryString("returnType");
                int returnCount = RequestUtils.GetIntQueryString("returnCount");
                bool inspectReport = RequestUtils.GetBoolQueryString("insectReport");
                string description = RequestUtils.GetQueryString("description");
                string imageUrl = RequestUtils.GetQueryString("imageUrl");
                string contact = RequestUtils.GetQueryString("contact");
                string contactPhone = RequestUtils.GetQueryString("contactPhone");
                OrderItemInfo orderItemInfo = DataProviderB2C.OrderItemDAO.GetItemInfo(orderItemID);

                #region ������У��
                if (orderItemInfo == null)
                {
                    var errorParm = new { isSuccess = false, errorMessage = "�ö��������ڣ�" };
                    return Ok(errorParm);
                }
                if (orderItemInfo.PurchaseNum < returnCount)
                {
                    var errorParm = new { isSuccess = false, errorMessage = "�˻���������������Ʒ��������" + orderItemInfo.PurchaseNum };
                    return Ok(errorParm);
                }
                if (string.IsNullOrEmpty(contact))
                {
                    var errorParm = new { isSuccess = false, errorMessage = "��ϵ�˱�����д��" };
                    return Ok(errorParm);
                }
                if (string.IsNullOrEmpty(contactPhone))
                {
                    var errorParm = new { isSuccess = false, errorMessage = "�ֻ����������д��" };
                    return Ok(errorParm);
                }
                #endregion

                OrderItemReturnInfo orderItemReturnInfoExist = DataProviderB2C.OrderItemReturnDAO.GetItemReturnInfoByOrderItemID(orderItemID);

                OrderItemReturnInfo orderItemReturnInfo = new OrderItemReturnInfo(HttpContext.Current.Request.QueryString, true);
                if (orderItemReturnInfoExist != null)
                    orderItemReturnInfo.ID = orderItemReturnInfoExist.ID;

                //����ͼƬ
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    StringBuilder sbImageUrl = new StringBuilder();
                    ArrayList arrayList = TranslateUtils.StringCollectionToArrayList(imageUrl);
                    foreach (string image in arrayList)
                    {
                        string[] iamgeArr = image.Split(new string[] { RequestUtils.PublishmentSystemInfo.PublishmentSystemDir }, StringSplitOptions.RemoveEmptyEntries);
                        if (iamgeArr.Length < 1)
                            continue;
                        string virtualImage = iamgeArr[1];
                        virtualImage = PageUtility.GetPublishmentSystemUrlByPhysicalPath(RequestUtils.PublishmentSystemInfo, virtualImage);
                        virtualImage = PageUtility.GetVirtualUrl(RequestUtils.PublishmentSystemInfo, virtualImage);
                        sbImageUrl.Append(virtualImage + ",");
                    }
                    if (sbImageUrl.Length > 0)
                        sbImageUrl.Length--;
                    orderItemReturnInfo.ImageUrl = sbImageUrl.ToString();
                }
                orderItemReturnInfo.ApplyDate = DateTime.Now;
                orderItemReturnInfo.ApplyUser = RequestUtils.CurrentUserName;
                orderItemReturnInfo.AuditStatus = EAuditStatus.Wait.ToString();
                orderItemReturnInfo.Status = EStatus.ToDo.ToString();
                orderItemReturnInfo.Title = orderItemInfo.Title;
                orderItemReturnInfo.GoodsSN = orderItemInfo.GoodsSN;

                if (orderItemReturnInfo.ID == 0)
                {
                    DataProviderB2C.OrderItemReturnDAO.Insert(orderItemReturnInfo);
                }
                else
                {
                    var errorParm = new { isSuccess = false, errorMessage = "�ö����Ѿ��������˻��������Եȴ�" };
                    return Ok(errorParm);
                }

                return Ok(new { isSuccess = true });
            }
            catch (Exception ex)
            {
                var errorParm = new { isSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }
    }
}
