
using System;
using System.Collections;
using System.Text;
using SiteServer.CMS.Model;
using BaiRong.Model;
using SiteServer.CMS.Core;
using BaiRong.Core;
using SiteServer.CMS.Core.Security;


using SiteServer.CMS.BackgroundPages.Modal;
using System.Collections.Specialized;

namespace SiteServer.CMS.BackgroundPages
{
    public class WebUtils
    {
        public static string GetContentTitle(PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo, string pageUrl)
        {
            string url;
            string displayString;
            string title = ContentUtility.FormatTitle(contentInfo.Attributes[BackgroundContentAttribute.TitleFormatString], contentInfo.Title);

            if (TranslateUtils.ToBool(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.IsColor)))
            {
                displayString = string.Format("<span style='color:#ff0000;text-decoration:none' title='醒目'>{0}</span>", title);
            }
            else
            {
                displayString = title;
            }

            if (contentInfo.NodeID < 0)
            {
                url = displayString;
            }
            else if (contentInfo.IsChecked)
            {
                url = string.Format("<a href='{0}' target='blank'>{1}</a>", PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, true, publishmentSystemInfo.Additional.VisualType), displayString);
            }
            else
            {
                url = string.Format("<a href='{0}'>{1}</a>", BackgroundContentView.GetContentViewUrl(publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, pageUrl), displayString);
            }

            string image = string.Empty;
            if (TranslateUtils.ToBool(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.IsRecommend)))
            {
                image += "&nbsp;<img src='../pic/icon/recommend.gif' title='推荐' align='absmiddle' border=0 />";
            }
            if (TranslateUtils.ToBool(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.IsHot)))
            {
                image += "&nbsp;<img src='../pic/icon/hot.gif' title='热点' align='absmiddle' border=0 />";
            }
            if (TranslateUtils.ToBool(contentInfo.GetExtendedAttribute(ContentAttribute.IsTop)))
            {
                image += "&nbsp;<img src='../pic/icon/top.gif' title='置顶' align='absmiddle' border=0 />";
            }
            if (contentInfo.ReferenceID > 0)
            {
                if (contentInfo.GetExtendedAttribute(ContentAttribute.TranslateContentType) == ETranslateContentType.ReferenceContent.ToString())
                {
                    image += "&nbsp;<img src='../pic/icon/reference.png' title='引用内容' align='absmiddle' border=0 />(引用内容)";
                }
                else if (contentInfo.GetExtendedAttribute(ContentAttribute.TranslateContentType) != ETranslateContentType.ReferenceContent.ToString())
                {
                    image += "&nbsp;<img src='../pic/icon/reference.png' title='引用地址' align='absmiddle' border=0 />(引用地址)";
                }
            }
            if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.LinkUrl)))
            {
                image += "&nbsp;<img src='../pic/icon/link.png' title='外部链接' align='absmiddle' border=0 />";
            }
            if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl)))
            {
                string imageUrl = PageUtility.ParseNavigationUrl(publishmentSystemInfo, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl));
                string openWindowString = Modal.Message.GetOpenWindowString("预览图片", string.Format("<img src='{0}' />", imageUrl), 500, 500);
                image += string.Format(@"&nbsp;<a href=""javascript:;"" onclick=""{0}""><img src='../../sitefiles/bairong/icons/img.gif' title='预览图片' align='absmiddle' border=0 /></a>", openWindowString);
            }
            if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.VideoUrl)))
            {
                string openWindowString = Modal.Message.GetOpenWindowStringToPreviewVideoByUrl(publishmentSystemInfo.PublishmentSystemID, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.VideoUrl));
                image += string.Format(@"&nbsp;<a href=""javascript:;"" onclick=""{0}""><img src='../pic/icon/video.png' title='预览视频' align='absmiddle' border=0 /></a>", openWindowString);
            }
            if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl)))
            {
                image += "&nbsp;<img src='../pic/icon/attachment.gif' title='附件' align='absmiddle' border=0 />";
                if (publishmentSystemInfo.Additional.IsCountDownload)
                {
                    int count = CountManager.GetCount(AppManager.CMS.AppID, publishmentSystemInfo.AuxiliaryTableForContent, contentInfo.ID.ToString(), ECountType.Download);
                    image += string.Format("下载次数:<strong>{0}</strong>", count);
                }
            }
            return url + image;
        }

        public static string GetChannelListBoxTitle(int publishmentSystemID, int nodeID, string nodeName, ENodeType nodeType, int parentsCount, bool isLastNode, bool[] isLastNodeArray)
        {
            string str = string.Empty;
            if (nodeID == publishmentSystemID)
            {
                isLastNode = true;
            }
            if (isLastNode == false)
            {
                isLastNodeArray[parentsCount] = false;
            }
            else
            {
                isLastNodeArray[parentsCount] = true;
            }
            for (int i = 0; i < parentsCount; i++)
            {
                if (isLastNodeArray[i])
                {
                    str = String.Concat(str, "　");
                }
                else
                {
                    str = String.Concat(str, "│");
                }
            }
            if (isLastNode)
            {
                str = String.Concat(str, "└");
            }
            else
            {
                str = String.Concat(str, "├");
            }
            str = String.Concat(str, StringUtils.MaxLengthText(nodeName, 8));

            return str;
        }

        private static string GetContentAddUrl(EContentModelType modelType)
        {
            if (modelType == EContentModelType.GovPublic)
            {
                return PageUtils.GetWCMUrl("background_govPublicContentAdd.aspx");
            }
            else if (modelType == EContentModelType.Goods)
            {
                return PageUtils.GetB2CUrl("background_contentAddGoods.aspx");
            }
            else if (modelType == EContentModelType.Vote)
            {
                return PageUtils.GetCMSUrl("background_voteContentAdd.aspx");
            }
            else
            {
                return PageUtils.GetCMSUrl("background_contentAdd.aspx");
            }
        }

        private static string GetContentAttributesOpenWindowUrl(int publishmentSystemID, int nodeID, EContentModelType modelType)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("nodeID", nodeID.ToString());


            if (modelType == EContentModelType.Goods)
            {
                return PageUtility.GetOpenWindowStringWithCheckBoxValue("设置商品属性", "../b2c/modal_GoodsContentAttributes.aspx", arguments, "ContentIDCollection", "请选择需要设置属性的商品！", 360, 240);
            }
            else
            {
                return PageUtility.GetOpenWindowStringWithCheckBoxValue("设置内容属性", "modal_ContentAttributes.aspx", arguments, "ContentIDCollection", "请选择需要设置属性的内容！", 300, 240);
            }
        }

        public static string GetContentAddUploadWordUrl(int publishmentSystemID, NodeInfo nodeInfo, bool isFirstLineTitle, bool isFirstLineRemove, bool isClearFormat, bool isFirstLineIndent, bool isClearFontSize, bool isClearFontFamily, bool isClearImages, int contentLevel, string fileName, string returnUrl)
        {
            EContentModelType modelType = EContentModelTypeUtils.GetEnumType(nodeInfo.ContentModelID);
            return string.Format("{0}?publishmentSystemID={1}&nodeID={2}&isUploadWord=True&isFirstLineTitle={3}&isFirstLineRemove={4}&isClearFormat={5}&isFirstLineIndent={6}&isClearFontSize={7}&isClearFontFamily={8}&isClearImages={9}&contentLevel={10}&fileName={11}&returnUrl={12}", WebUtils.GetContentAddUrl(modelType), publishmentSystemID, nodeInfo.NodeID, isFirstLineTitle, isFirstLineRemove, isClearFormat, isFirstLineIndent, isClearFontSize, isClearFontFamily, isClearImages, contentLevel, fileName, returnUrl);
        }

        public static string GetContentAddAddUrl(int publishmentSystemID, NodeInfo nodeInfo, string returnUrl)
        {
            EContentModelType modelType = EContentModelTypeUtils.GetEnumType(nodeInfo.ContentModelID);
            return string.Format("{0}?publishmentSystemID={1}&nodeID={2}&returnUrl={3}", WebUtils.GetContentAddUrl(modelType), publishmentSystemID, nodeInfo.NodeID, StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetContentAddEditUrl(int publishmentSystemID, NodeInfo nodeInfo, int id, string returnUrl)
        {
            EContentModelType modelType = EContentModelTypeUtils.GetEnumType(nodeInfo.ContentModelID);
            return string.Format("{0}?publishmentSystemID={1}&nodeID={2}&id={3}&returnUrl={4}", WebUtils.GetContentAddUrl(modelType), publishmentSystemID, nodeInfo.NodeID, id, StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetContentCommands(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, string pageUrl, string currentFileName, bool isCheckPage)
        {
            string iconUrl = PageUtils.GetIconUrl(string.Empty);
            EContentModelType modelType = EContentModelTypeUtils.GetEnumType(nodeInfo.ContentModelID);

            StringBuilder builder = new StringBuilder();
            //添加内容
            if (!isCheckPage && AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentAdd) && nodeInfo.Additional.IsContentAddable)
            {
                string redirectUrl = WebUtils.GetContentAddAddUrl(publishmentSystemInfo.PublishmentSystemID, nodeInfo, pageUrl);
                string title = "添加内容";
                if (modelType == EContentModelType.GovPublic)
                {
                    title = "采集信息";
                }
                else if (modelType == EContentModelType.GovInteract)
                {
                    title = "新增办件";
                }
                else if (modelType == EContentModelType.Goods)
                {
                    title = "添加商品";
                }
                else if (modelType == EContentModelType.Brand)
                {
                    title = "添加品牌";
                }
                else if (modelType == EContentModelType.Photo)
                {
                    title = "添加图片";
                }
                else if (modelType == EContentModelType.Teleplay)
                {
                    title = "添加电视剧";
                }
                else if (modelType == EContentModelType.Vote)
                {
                    title = "发起投票";
                }

                builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" /><lan>{2}</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", redirectUrl, iconUrl, title);

                //if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.GovPublic))
                //{
                //    builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" /><lan>采集信息</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", BackgroundGovPublicContentAdd.GetRedirectUrlOfAdd(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl), iconUrl);
                //}
                //else if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.GovInteract))
                //{
                //    builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" /><lan>新增办件</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", BackgroundContentAdd.GetRedirectUrlOfAdd(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl), iconUrl);
                //}
                //else if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.Goods))
                //{
                //    builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" /><lan>添加商品</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", WebUtils.GetContentAddAddUrl(publishmentSystemInfo.PublishmentSystemID, nodeInfo, pageUrl), iconUrl);
                //}
                //else if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.Brand))
                //{
                //    builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" /><lan>添加品牌</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", BackgroundContentAdd.GetRedirectUrlOfAdd(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl), iconUrl);
                //}
                //else if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.Vote))
                //{
                //    builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" /><lan>发起投票</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", BackgroundVoteContentAdd.GetRedirectUrlOfAdd(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl), iconUrl);
                //}
                //else
                //{
                //    builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" /><lan>添加内容</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", BackgroundContentAdd.GetRedirectUrlOfAdd(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl), iconUrl);
                //}

                builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>导入内容</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", PageUtility.ModalSTL.ContentImport_GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));

                if (modelType != EContentModelType.UserDefined && modelType != EContentModelType.Vote && modelType != EContentModelType.Job && modelType != EContentModelType.GovInteract)
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>导入Word</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.UploadWord.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, StringUtils.ValueToUrl(pageUrl)));
                }
            }
            //删 除
            if (nodeInfo.ContentNum > 0 && AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentDelete))
            {
                builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>删 除</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", BackgroundContentDelete.GetRedirectClickStringForSingleChannel(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, false, pageUrl));
            }

            if (nodeInfo.ContentNum > 0)
            {
                builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>导 出</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", PageUtility.ModalSTL.ContentExport_GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));
                //设置
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentEdit))
                {
                    if (EContentModelTypeUtils.Equals(EContentModelType.Goods, nodeInfo.ContentModelID))
                    {
                        builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>设置属性</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", GetContentAttributesOpenWindowUrl(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, EContentModelType.Goods));
                    }
                    else
                    {
                        builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>设置属性</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", GetContentAttributesOpenWindowUrl(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, EContentModelType.Content));
                    }
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>设置内容组</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.AddToGroup.GetOpenWindowStringToContent(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));
                }
                //转 移
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentTranslate))
                {
                    string redirectUrl = string.Format("background_contentTranslate.aspx?PublishmentSystemID={0}&NodeID={1}&ReturnUrl={2}", publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, StringUtils.ValueToUrl(pageUrl));

                    if (EPublishmentSystemTypeUtils.IsB2C(publishmentSystemInfo.PublishmentSystemType))
                    {
                        redirectUrl = PageUtils.GetB2CUrl(redirectUrl);
                    }
                    else
                    {
                        redirectUrl = PageUtils.GetCMSUrl(redirectUrl);
                    }

                    string clickString = JsUtils.GetRedirectStringWithCheckBoxValue(redirectUrl, "ContentIDCollection", "ContentIDCollection", "请选择需要转移的内容！");

                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>转 移</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", clickString);
                }
                //排 序
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentEdit))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>排 序</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.ContentTaxis.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl));
                }
                //整理
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentOrder))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>整 理</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.ContentTidyUp.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl));
                }
                //审 核
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentCheck))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>审 核</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.ContentCheck.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl));
                }
                //归 档
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentArchive))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>归 档</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.ContentArchive.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl));
                }
                //跨站转发
                if (CrossSiteTransUtility.IsTranslatable(publishmentSystemInfo, nodeInfo))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>跨站转发</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.ContentCrossSiteTrans.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));
                }
                //生 成
                if (!isCheckPage && (AdminUtility.HasWebsitePermissions(publishmentSystemInfo.PublishmentSystemID, AppManager.CMS.Permission.WebSite.Create) || AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.CreatePage)))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>生 成</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.ProgressBar.GetOpenWindowStringWithCreateContentsOneByOne(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));
                }
                //发 布
                if (!isCheckPage && publishmentSystemInfo.Additional.IsSiteStorage)
                {
                    if (AdminUtility.HasWebsitePermissions(publishmentSystemInfo.PublishmentSystemID, AppManager.CMS.Permission.WebSite.AllPublish) || AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.PublishPage))
                    {
                        builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>发 布</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.PublishPages.GetOpenWindowStringByContents(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));
                    }
                }
                #region by 20160308 sofuny 新加试用功能的功能按钮
                if (nodeInfo.Additional.IsUseTrial)
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>试用设置</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.TrialApplySetting.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl));
                } 
                if (nodeInfo.Additional.IsUseSurvey)
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>调查问卷设置</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.SurveySetting.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl));
                }
                #endregion
            }

            //选择显示项
            //if (nodeInfo.NodeType != ENodeType.BackgroundImageNode)
            //{
            if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelEdit))
            {
                builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>显示项</lan></a> &nbsp; &nbsp; ", Modal.SelectColumns.GetOpenWindowStringToContent(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, true));
            }
            //}

            if (!isCheckPage && nodeInfo.ContentNum > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Length = builder.Length - 15;
                }

                //builder.Append(GetContentLinks(publishmentSystemInfo, nodeInfo, contentType, currentFileName));

                builder.AppendFormat(@"&nbsp; <a href=""javascript:;;"" onClick=""$('#contentSearch').toggle(); return false""><img src=""{0}/search.gif"" align=""absMiddle"" alt=""快速查找"" /></a>", iconUrl);
            }


            //if (builder.Length > 0)
            //{
            //    builder.Length = builder.Length - 16;
            //}
            return builder.ToString();
        }

        public static string GetChannelCommands(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, string pageUrl, string currentFileName)
        {
            string iconUrl = PageUtils.GetIconUrl(string.Empty);
            StringBuilder builder = new StringBuilder();
            //添加栏目
            if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelAdd) && nodeInfo.Additional.IsChannelAddable)
            {
                builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" />添加栏目</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", BackgroundChannelAdd.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl), iconUrl);
                builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">快速添加</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.ChannelAdd.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl));
            }
            if (nodeInfo.ChildrenCount > 0)
            {
                //删除栏目
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelDelete))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">删除栏目</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.GetCMSUrl(string.Format("background_channelDelete.aspx?PublishmentSystemID={0}&ReturnUrl={1}", publishmentSystemInfo.PublishmentSystemID, StringUtils.ValueToUrl(pageUrl))), "ChannelIDCollection", "ChannelIDCollection", "请选择需要删除的栏目！"));
                }
                //清空内容
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentDelete))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">清空内容</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.GetCMSUrl(string.Format("background_channelDelete.aspx?PublishmentSystemID={0}&DeleteContents=True&ReturnUrl={1}", publishmentSystemInfo.PublishmentSystemID, StringUtils.ValueToUrl(pageUrl))), "ChannelIDCollection", "ChannelIDCollection", "请选择需要删除内容的栏目！"));
                }

                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelAdd))
                {
                    //导 入
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">导 入</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", PageUtility.ModalSTL.ChannelImport_GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));
                    //导 出
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">导 出</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToChannel(publishmentSystemInfo.PublishmentSystemID, "ChannelIDCollection", "请选择需要导出的栏目！"));
                }

                //设置栏目组
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelEdit))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">设置栏目组</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.AddToGroup.GetOpenWindowStringToChannel(publishmentSystemInfo.PublishmentSystemID));
                }
                //转 移
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelTranslate))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">转 移</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.GetCMSUrl(string.Format("background_channelTranslate.aspx?PublishmentSystemID={0}&NodeID={1}&ReturnUrl={2}", publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, StringUtils.ValueToUrl(pageUrl))), "ChannelIDCollection", "ChannelIDCollection", "请选择需要转移的栏目！"));
                }

                //生 成
                if (AdminUtility.HasWebsitePermissions(publishmentSystemInfo.PublishmentSystemID, AppManager.CMS.Permission.WebSite.Create) || AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.CreatePage))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>生 成</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.CreateChannels.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID));
                }

                //发 布
                if (publishmentSystemInfo.Additional.IsSiteStorage)
                {
                    if (AdminUtility.HasWebsitePermissions(publishmentSystemInfo.PublishmentSystemID, AppManager.CMS.Permission.WebSite.AllPublish) || AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.PublishPage))
                    {
                        builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>发 布</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.PublishPages.GetOpenWindowStringByChannels(publishmentSystemInfo.PublishmentSystemID));
                    }
                }
            }
            else
            {
                //导 入
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelAdd))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">导 入</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", PageUtility.ModalSTL.ChannelImport_GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));
                }
            }
            if (publishmentSystemInfo.PublishmentSystemID != nodeInfo.NodeID)
            {
                builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/upfolder.gif"" align=""absMiddle"" />向 上</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", string.Format("{0}?PublishmentSystemID={1}&NodeID={2}", currentFileName, publishmentSystemInfo.PublishmentSystemID, nodeInfo.ParentID), iconUrl);
            }
            if (builder.Length > 0)
            {
                builder.Length = builder.Length - 15;
            }
            return builder.ToString();
        }

        public static string GetTextEditorCommands(PublishmentSystemInfo publishmentSystemInfo, ETextEditorType editorType, string attributeName)
        {
            StringBuilder builder = new StringBuilder();

            if (ETextEditorTypeUtils.IsInsertHtml(editorType))
            {
                builder.AppendFormat(@"<div class=""btn_word"" onclick=""{0}""><lan>导入Word</lan></div>", TextEditorImportWord.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, editorType, attributeName));
            }

            if (ETextEditorTypeUtils.IsInsertVideo(editorType))
            {
                builder.AppendFormat(@"<div class=""btn_video"" onclick=""{0}""><lan>插入视频</lan></div>", TextEditorInsertVideo.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, editorType, attributeName));
            }

            if (ETextEditorTypeUtils.IsInsertAudio(editorType))
            {
                builder.AppendFormat(@"<div class=""btn_audio"" onclick=""{0}""><lan>插入音频</lan></div>", TextEditorInsertAudio.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, editorType, attributeName));
            }

            if (ETextEditorTypeUtils.IsGetPureText(editorType))
            {
                string command = @"<div class=""btn_keywords"" onclick=""getWordSpliter();""><lan>提取关键字</lan></div>
<script type=""text/javascript"">
function getWordSpliter(){
    var pureText = [getPureText]
	$.post('[url]&r=' + Math.random(), {content:pureText}, function(data) {
		if(data !=''){
			$('#Tags').val(data).focus();
		}else{
            [tips]
        }
	});	
}
</script>
";
                command = command.Replace("[url]", BackgroundService.GetWordSpliterUrl(publishmentSystemInfo.PublishmentSystemID));
                command = command.Replace("[getPureText]", ETextEditorTypeUtils.GetPureTextScript(editorType, attributeName));
                command = command.Replace("[tips]", JsUtils.OpenWindow.GetOpenTipsString("对不起，内容不足，无法提取关键字", JsUtils.OpenWindow.TIPS_ERROR));

                builder.Append(command);
            }

            if (ETextEditorTypeUtils.IsGetPureText(editorType) && ETextEditorTypeUtils.IsGetContent(editorType) && ETextEditorTypeUtils.IsSetContent(editorType))
            {
                string command = @"<div class=""btn_detection"" onclick=""detection();""><lan>敏感词检测</lan></div>
<script type=""text/javascript"">
function detection(){
    var pureText = [getPureText]
    var htmlContent = [getContent]
    var titleContent = [getTitleContent]
    var oldTitleContent = titleContent;
    var subTitleContent = [getSubTitleContent]
    var oldSubTitleContent = subTitleContent;
    var keyword = '';
	$.post('[url]&r=' + Math.random(), {content:pureText+titleContent+subTitleContent}, function(data) {
		if(data !=''){
			var arr = data.split(',');
            var i=0;
			for(;i<arr.length;i++)
			{
                var reg = new RegExp(arr[i], 'gi');
				htmlContent = htmlContent.replace(reg,'<span style=""background-color:#ffff00;"">' + arr[i] + '</span>');
			}
            keyword=data;
			[setContent]
            [tips_warn]
		}else{
            [tips_success]
        }
	});	
}
</script>
";
                command = command.Replace("[url]", BackgroundService.GetDetectionUrl(publishmentSystemInfo.PublishmentSystemID));
                command = command.Replace("[getPureText]", ETextEditorTypeUtils.GetPureTextScript(editorType, attributeName));
                command = command.Replace("[getContent]", ETextEditorTypeUtils.GetContentScript(editorType, attributeName));
                command = command.Replace("[setContent]", ETextEditorTypeUtils.GetSetContentScript(editorType, attributeName, "htmlContent"));
                command = command.Replace("[getTitleContent]", string.Format(@"$('#{0}').val();", "Title"));
                command = command.Replace("[getSubTitleContent]", string.Format(@"$('#{0}').val();", "SubTitle"));
                command = command.Replace("[tips_warn]", JsUtils.OpenWindow.GetOpenTipsString("共检测到' + i + '个敏感词，内容已用黄色背景标明，标题可能包含：'+keyword+'"
                   , JsUtils.OpenWindow.TIPS_WARN));
                command = command.Replace("[tips_success]", JsUtils.OpenWindow.GetOpenTipsString("检测成功，没有检测到任何敏感词", JsUtils.OpenWindow.TIPS_SUCCESS));
                builder.Append(command);
            }

            return builder.ToString();
        }

        public static string GetAutoCheckKeywordsCommands(PublishmentSystemInfo publishmentSystemInfo, ETextEditorType editorType, string attributeName)
        {
            StringBuilder builder = new StringBuilder();

            if (ETextEditorTypeUtils.IsGetPureText(editorType) && ETextEditorTypeUtils.IsGetContent(editorType) && ETextEditorTypeUtils.IsSetContent(editorType))
            {
                string command = @"
<script type=""text/javascript"">
var bairongKeywordArray;
function autoCheckKeywords(){
    if($('#openWaitModal').length > 0){
        $('#openWaitModal').modal();
    }
    if([isAutoCheckKeywords]){

    var pureText = [getPureText]
    var htmlContent = [getContent]
    var titleContent = [getTitleContent]
    var oldTitleContent = titleContent;
    var subTitleContent = [getSubTitleContent]
    var oldSubTitleContent = subTitleContent; 

    //内容
	$.post('[url]&r=' + Math.random(), {content:pureText+titleContent+subTitleContent}, function(data) {
		if(data !=''){
            bairongKeywordArray = data;
			var arr = data.split(',');
            var i=0;
			for(;i<arr.length;i++)
			{
                var tmpArr = arr[i].split('|');
                var keyword = tmpArr[0];
                var replace = tmpArr.length==2?tmpArr[1]:'';
                var reg = new RegExp(keyword, 'gi');
				htmlContent = htmlContent.replace(reg,'<span style=""background-color:#ffff00;"">' + keyword + '</span>');
				titleContent = htmlContent.replace(reg, keyword);
				subTitleContent = htmlContent.replace(reg, keyword);
			}
			//[setTitleContent]
			//[setSubTitleContent]
			[setContent]

            if($('#openWaitModal').length > 0){
                $('#openWaitModal').hide();
            }
            //如果标题没有敏感词
            if(oldTitleContent==titleContent && subTitleContent==oldSubTitleContent){
                [tips_warn]
            }else{
                [tips_warn2]
            }
		}else{
            $('#Submit').attr('onclick', '').click();
        }
	});
    return false;	
    }
}
function autoReplaceKeywords(){
    if(!!bairongKeywordArray){
		var arr = bairongKeywordArray.split(',');
        var i=0;
        var htmlContent = [getContent]
		for(;i<arr.length;i++)
		{
            var tmpArr = arr[i].split('|');
            var keyword = tmpArr[0];
            var replace = tmpArr.length==2?tmpArr[1]:'';
            var reg = new RegExp('<span style=""background-color:#ffff00;"">' + keyword + '</span>', 'gi');
			htmlContent = htmlContent.replace(reg, replace);
            //IE8
            reg = new RegExp('<span style=""background-color:#ffff00"">' + keyword + '</span>', 'gi');
			htmlContent = htmlContent.replace(reg, replace);
		}
        [setContent]
        autoReplaceKeywordsTitle();
        autoReplaceKeywordsSubTitle();
    }
    return true;
}
</script>
";
                command = command.Replace("[isAutoCheckKeywords]", string.Format("{0}", publishmentSystemInfo.Additional.IsAutoCheckKeywords.ToString().ToLower()));
                command = command.Replace("[url]", BackgroundService.GetDetectionReplaceUrl(publishmentSystemInfo.PublishmentSystemID));
                command = command.Replace("[getPureText]", ETextEditorTypeUtils.GetPureTextScript(editorType, attributeName));
                command = command.Replace("[getContent]", ETextEditorTypeUtils.GetContentScript(editorType, attributeName));
                command = command.Replace("[setContent]", ETextEditorTypeUtils.GetSetContentScript(editorType, attributeName, "htmlContent"));
                command = command.Replace("[getTitleContent]", string.Format(@"$('#{0}').val();", "Title"));
                command = command.Replace("[setTitleContent]", string.Format(@"$('#{0}').val({1});", "Title", "titleContent"));
                command = command.Replace("[getSubTitleContent]", string.Format(@"$('#{0}').val();", "SubTitle"));
                command = command.Replace("[setSubTitleContent]", string.Format(@"$('#{0}').val({1});", "SubTitle", "subTitleContent"));
                command = command.Replace("[tips_warn]", JsUtils.OpenWindow.GetOpenTipsString("内容中共检测到' + i + '个敏感词，已用黄色背景标明", JsUtils.OpenWindow.TIPS_WARN, false, "继续发布", "autoReplaceKeywords"));
                command = command.Replace("[tips_warn2]", JsUtils.OpenWindow.GetOpenTipsString("共检测到' + i + '个敏感词，内容已用黄色背景标明，标题将进行替换", JsUtils.OpenWindow.TIPS_WARN, false, "继续发布", "autoReplaceKeywords"));
                builder.Append(command);
            }
            else
            {
                string command = @"
<script type=""text/javascript"">return true;</script>";
                builder.Append(command);
            }
            return builder.ToString();
        }



        public static string GetAutoCheckKeywordsCommandsByInput(PublishmentSystemInfo publishmentSystemInfo, string attributeName)
        {
            StringBuilder builder = new StringBuilder();

            string command = @"
<script type=""text/javascript""> 
function autoReplaceKeywords[attributeName](){
    if(!!bairongKeywordArray){
		var arr = bairongKeywordArray.split(',');
        var i=0;
        var htmlContent = [getInputContent]
		for(;i<arr.length;i++)
		{
            var tmpArr = arr[i].split('|');
            var keyword = tmpArr[0];
            var replace = tmpArr.length==2?tmpArr[1]:'';
            var reg = new RegExp( keyword , 'gi');
			htmlContent = htmlContent.replace(reg, replace);
            //IE8
            reg = new RegExp( keyword , 'gi');
			htmlContent = htmlContent.replace(reg, replace);
		}
        [setInputContent]
    }
    return true;
}
</script>
";
            command = command.Replace("[isAutoCheckKeywords]", string.Format("{0}", publishmentSystemInfo.Additional.IsAutoCheckKeywords.ToString().ToLower()));
            command = command.Replace("[url]", BackgroundService.GetDetectionReplaceUrl(publishmentSystemInfo.PublishmentSystemID));
            command = command.Replace("[attributeName]", attributeName);
            command = command.Replace("[getInputContent]", string.Format(@"$('#{0}').val();", attributeName));
            command = command.Replace("[setInputContent]", string.Format(@"$('#{0}').val({1});", attributeName, "htmlContent"));
            command = command.Replace("[tips_warn]", JsUtils.OpenWindow.GetOpenTipsString("标题中共检测到' + i + '个敏感词", JsUtils.OpenWindow.TIPS_WARN));
            builder.Append(command);

            return builder.ToString();
        }
    }
}
