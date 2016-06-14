
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
                displayString = string.Format("<span style='color:#ff0000;text-decoration:none' title='��Ŀ'>{0}</span>", title);
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
                image += "&nbsp;<img src='../pic/icon/recommend.gif' title='�Ƽ�' align='absmiddle' border=0 />";
            }
            if (TranslateUtils.ToBool(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.IsHot)))
            {
                image += "&nbsp;<img src='../pic/icon/hot.gif' title='�ȵ�' align='absmiddle' border=0 />";
            }
            if (TranslateUtils.ToBool(contentInfo.GetExtendedAttribute(ContentAttribute.IsTop)))
            {
                image += "&nbsp;<img src='../pic/icon/top.gif' title='�ö�' align='absmiddle' border=0 />";
            }
            if (contentInfo.ReferenceID > 0)
            {
                if (contentInfo.GetExtendedAttribute(ContentAttribute.TranslateContentType) == ETranslateContentType.ReferenceContent.ToString())
                {
                    image += "&nbsp;<img src='../pic/icon/reference.png' title='��������' align='absmiddle' border=0 />(��������)";
                }
                else if (contentInfo.GetExtendedAttribute(ContentAttribute.TranslateContentType) != ETranslateContentType.ReferenceContent.ToString())
                {
                    image += "&nbsp;<img src='../pic/icon/reference.png' title='���õ�ַ' align='absmiddle' border=0 />(���õ�ַ)";
                }
            }
            if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.LinkUrl)))
            {
                image += "&nbsp;<img src='../pic/icon/link.png' title='�ⲿ����' align='absmiddle' border=0 />";
            }
            if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl)))
            {
                string imageUrl = PageUtility.ParseNavigationUrl(publishmentSystemInfo, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl));
                string openWindowString = Modal.Message.GetOpenWindowString("Ԥ��ͼƬ", string.Format("<img src='{0}' />", imageUrl), 500, 500);
                image += string.Format(@"&nbsp;<a href=""javascript:;"" onclick=""{0}""><img src='../../sitefiles/bairong/icons/img.gif' title='Ԥ��ͼƬ' align='absmiddle' border=0 /></a>", openWindowString);
            }
            if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.VideoUrl)))
            {
                string openWindowString = Modal.Message.GetOpenWindowStringToPreviewVideoByUrl(publishmentSystemInfo.PublishmentSystemID, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.VideoUrl));
                image += string.Format(@"&nbsp;<a href=""javascript:;"" onclick=""{0}""><img src='../pic/icon/video.png' title='Ԥ����Ƶ' align='absmiddle' border=0 /></a>", openWindowString);
            }
            if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl)))
            {
                image += "&nbsp;<img src='../pic/icon/attachment.gif' title='����' align='absmiddle' border=0 />";
                if (publishmentSystemInfo.Additional.IsCountDownload)
                {
                    int count = CountManager.GetCount(AppManager.CMS.AppID, publishmentSystemInfo.AuxiliaryTableForContent, contentInfo.ID.ToString(), ECountType.Download);
                    image += string.Format("���ش���:<strong>{0}</strong>", count);
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
                    str = String.Concat(str, "��");
                }
                else
                {
                    str = String.Concat(str, "��");
                }
            }
            if (isLastNode)
            {
                str = String.Concat(str, "��");
            }
            else
            {
                str = String.Concat(str, "��");
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
                return PageUtility.GetOpenWindowStringWithCheckBoxValue("������Ʒ����", "../b2c/modal_GoodsContentAttributes.aspx", arguments, "ContentIDCollection", "��ѡ����Ҫ�������Ե���Ʒ��", 360, 240);
            }
            else
            {
                return PageUtility.GetOpenWindowStringWithCheckBoxValue("������������", "modal_ContentAttributes.aspx", arguments, "ContentIDCollection", "��ѡ����Ҫ�������Ե����ݣ�", 300, 240);
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
            //�������
            if (!isCheckPage && AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentAdd) && nodeInfo.Additional.IsContentAddable)
            {
                string redirectUrl = WebUtils.GetContentAddAddUrl(publishmentSystemInfo.PublishmentSystemID, nodeInfo, pageUrl);
                string title = "�������";
                if (modelType == EContentModelType.GovPublic)
                {
                    title = "�ɼ���Ϣ";
                }
                else if (modelType == EContentModelType.GovInteract)
                {
                    title = "�������";
                }
                else if (modelType == EContentModelType.Goods)
                {
                    title = "�����Ʒ";
                }
                else if (modelType == EContentModelType.Brand)
                {
                    title = "���Ʒ��";
                }
                else if (modelType == EContentModelType.Photo)
                {
                    title = "���ͼƬ";
                }
                else if (modelType == EContentModelType.Teleplay)
                {
                    title = "��ӵ��Ӿ�";
                }
                else if (modelType == EContentModelType.Vote)
                {
                    title = "����ͶƱ";
                }

                builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" /><lan>{2}</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", redirectUrl, iconUrl, title);

                //if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.GovPublic))
                //{
                //    builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" /><lan>�ɼ���Ϣ</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", BackgroundGovPublicContentAdd.GetRedirectUrlOfAdd(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl), iconUrl);
                //}
                //else if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.GovInteract))
                //{
                //    builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" /><lan>�������</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", BackgroundContentAdd.GetRedirectUrlOfAdd(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl), iconUrl);
                //}
                //else if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.Goods))
                //{
                //    builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" /><lan>�����Ʒ</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", WebUtils.GetContentAddAddUrl(publishmentSystemInfo.PublishmentSystemID, nodeInfo, pageUrl), iconUrl);
                //}
                //else if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.Brand))
                //{
                //    builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" /><lan>���Ʒ��</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", BackgroundContentAdd.GetRedirectUrlOfAdd(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl), iconUrl);
                //}
                //else if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.Vote))
                //{
                //    builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" /><lan>����ͶƱ</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", BackgroundVoteContentAdd.GetRedirectUrlOfAdd(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl), iconUrl);
                //}
                //else
                //{
                //    builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" /><lan>�������</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", BackgroundContentAdd.GetRedirectUrlOfAdd(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl), iconUrl);
                //}

                builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>��������</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", PageUtility.ModalSTL.ContentImport_GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));

                if (modelType != EContentModelType.UserDefined && modelType != EContentModelType.Vote && modelType != EContentModelType.Job && modelType != EContentModelType.GovInteract)
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>����Word</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.UploadWord.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, StringUtils.ValueToUrl(pageUrl)));
                }
            }
            //ɾ ��
            if (nodeInfo.ContentNum > 0 && AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentDelete))
            {
                builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>ɾ ��</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", BackgroundContentDelete.GetRedirectClickStringForSingleChannel(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, false, pageUrl));
            }

            if (nodeInfo.ContentNum > 0)
            {
                builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>�� ��</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", PageUtility.ModalSTL.ContentExport_GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));
                //����
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentEdit))
                {
                    if (EContentModelTypeUtils.Equals(EContentModelType.Goods, nodeInfo.ContentModelID))
                    {
                        builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>��������</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", GetContentAttributesOpenWindowUrl(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, EContentModelType.Goods));
                    }
                    else
                    {
                        builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>��������</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", GetContentAttributesOpenWindowUrl(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, EContentModelType.Content));
                    }
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>����������</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.AddToGroup.GetOpenWindowStringToContent(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));
                }
                //ת ��
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

                    string clickString = JsUtils.GetRedirectStringWithCheckBoxValue(redirectUrl, "ContentIDCollection", "ContentIDCollection", "��ѡ����Ҫת�Ƶ����ݣ�");

                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>ת ��</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", clickString);
                }
                //�� ��
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentEdit))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>�� ��</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.ContentTaxis.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl));
                }
                //����
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentOrder))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>�� ��</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.ContentTidyUp.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl));
                }
                //�� ��
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentCheck))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>�� ��</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.ContentCheck.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl));
                }
                //�� ��
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentArchive))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>�� ��</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.ContentArchive.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl));
                }
                //��վת��
                if (CrossSiteTransUtility.IsTranslatable(publishmentSystemInfo, nodeInfo))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>��վת��</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.ContentCrossSiteTrans.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));
                }
                //�� ��
                if (!isCheckPage && (AdminUtility.HasWebsitePermissions(publishmentSystemInfo.PublishmentSystemID, AppManager.CMS.Permission.WebSite.Create) || AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.CreatePage)))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>�� ��</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.ProgressBar.GetOpenWindowStringWithCreateContentsOneByOne(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));
                }
                //�� ��
                if (!isCheckPage && publishmentSystemInfo.Additional.IsSiteStorage)
                {
                    if (AdminUtility.HasWebsitePermissions(publishmentSystemInfo.PublishmentSystemID, AppManager.CMS.Permission.WebSite.AllPublish) || AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.PublishPage))
                    {
                        builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>�� ��</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.PublishPages.GetOpenWindowStringByContents(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));
                    }
                }
                #region by 20160308 sofuny �¼����ù��ܵĹ��ܰ�ť
                if (nodeInfo.Additional.IsUseTrial)
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>��������</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.TrialApplySetting.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl));
                } 
                if (nodeInfo.Additional.IsUseSurvey)
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>�����ʾ�����</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.SurveySetting.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl));
                }
                #endregion
            }

            //ѡ����ʾ��
            //if (nodeInfo.NodeType != ENodeType.BackgroundImageNode)
            //{
            if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelEdit))
            {
                builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>��ʾ��</lan></a> &nbsp; &nbsp; ", Modal.SelectColumns.GetOpenWindowStringToContent(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, true));
            }
            //}

            if (!isCheckPage && nodeInfo.ContentNum > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Length = builder.Length - 15;
                }

                //builder.Append(GetContentLinks(publishmentSystemInfo, nodeInfo, contentType, currentFileName));

                builder.AppendFormat(@"&nbsp; <a href=""javascript:;;"" onClick=""$('#contentSearch').toggle(); return false""><img src=""{0}/search.gif"" align=""absMiddle"" alt=""���ٲ���"" /></a>", iconUrl);
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
            //�����Ŀ
            if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelAdd) && nodeInfo.Additional.IsChannelAddable)
            {
                builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/add.gif"" align=""absMiddle"" />�����Ŀ</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", BackgroundChannelAdd.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl), iconUrl);
                builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">�������</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.ChannelAdd.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, pageUrl));
            }
            if (nodeInfo.ChildrenCount > 0)
            {
                //ɾ����Ŀ
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelDelete))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">ɾ����Ŀ</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.GetCMSUrl(string.Format("background_channelDelete.aspx?PublishmentSystemID={0}&ReturnUrl={1}", publishmentSystemInfo.PublishmentSystemID, StringUtils.ValueToUrl(pageUrl))), "ChannelIDCollection", "ChannelIDCollection", "��ѡ����Ҫɾ������Ŀ��"));
                }
                //�������
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentDelete))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">�������</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.GetCMSUrl(string.Format("background_channelDelete.aspx?PublishmentSystemID={0}&DeleteContents=True&ReturnUrl={1}", publishmentSystemInfo.PublishmentSystemID, StringUtils.ValueToUrl(pageUrl))), "ChannelIDCollection", "ChannelIDCollection", "��ѡ����Ҫɾ�����ݵ���Ŀ��"));
                }

                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelAdd))
                {
                    //�� ��
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">�� ��</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", PageUtility.ModalSTL.ChannelImport_GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));
                    //�� ��
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">�� ��</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToChannel(publishmentSystemInfo.PublishmentSystemID, "ChannelIDCollection", "��ѡ����Ҫ��������Ŀ��"));
                }

                //������Ŀ��
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelEdit))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">������Ŀ��</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.AddToGroup.GetOpenWindowStringToChannel(publishmentSystemInfo.PublishmentSystemID));
                }
                //ת ��
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelTranslate))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">ת ��</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.GetCMSUrl(string.Format("background_channelTranslate.aspx?PublishmentSystemID={0}&NodeID={1}&ReturnUrl={2}", publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, StringUtils.ValueToUrl(pageUrl))), "ChannelIDCollection", "ChannelIDCollection", "��ѡ����Ҫת�Ƶ���Ŀ��"));
                }

                //�� ��
                if (AdminUtility.HasWebsitePermissions(publishmentSystemInfo.PublishmentSystemID, AppManager.CMS.Permission.WebSite.Create) || AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.CreatePage))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>�� ��</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.CreateChannels.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID));
                }

                //�� ��
                if (publishmentSystemInfo.Additional.IsSiteStorage)
                {
                    if (AdminUtility.HasWebsitePermissions(publishmentSystemInfo.PublishmentSystemID, AppManager.CMS.Permission.WebSite.AllPublish) || AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.PublishPage))
                    {
                        builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}""><lan>�� ��</lan></a> <span class=""gray"">&nbsp;|&nbsp;</span> ", Modal.PublishPages.GetOpenWindowStringByChannels(publishmentSystemInfo.PublishmentSystemID));
                    }
                }
            }
            else
            {
                //�� ��
                if (AdminUtility.HasChannelPermissions(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelAdd))
                {
                    builder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">�� ��</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", PageUtility.ModalSTL.ChannelImport_GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID));
                }
            }
            if (publishmentSystemInfo.PublishmentSystemID != nodeInfo.NodeID)
            {
                builder.AppendFormat(@"<a href=""{0}""><img style=""MARGIN-RIGHT: 3px"" src=""{1}/upfolder.gif"" align=""absMiddle"" />�� ��</a> <span class=""gray"">&nbsp;|&nbsp;</span> ", string.Format("{0}?PublishmentSystemID={1}&NodeID={2}", currentFileName, publishmentSystemInfo.PublishmentSystemID, nodeInfo.ParentID), iconUrl);
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
                builder.AppendFormat(@"<div class=""btn_word"" onclick=""{0}""><lan>����Word</lan></div>", TextEditorImportWord.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, editorType, attributeName));
            }

            if (ETextEditorTypeUtils.IsInsertVideo(editorType))
            {
                builder.AppendFormat(@"<div class=""btn_video"" onclick=""{0}""><lan>������Ƶ</lan></div>", TextEditorInsertVideo.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, editorType, attributeName));
            }

            if (ETextEditorTypeUtils.IsInsertAudio(editorType))
            {
                builder.AppendFormat(@"<div class=""btn_audio"" onclick=""{0}""><lan>������Ƶ</lan></div>", TextEditorInsertAudio.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, editorType, attributeName));
            }

            if (ETextEditorTypeUtils.IsGetPureText(editorType))
            {
                string command = @"<div class=""btn_keywords"" onclick=""getWordSpliter();""><lan>��ȡ�ؼ���</lan></div>
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
                command = command.Replace("[tips]", JsUtils.OpenWindow.GetOpenTipsString("�Բ������ݲ��㣬�޷���ȡ�ؼ���", JsUtils.OpenWindow.TIPS_ERROR));

                builder.Append(command);
            }

            if (ETextEditorTypeUtils.IsGetPureText(editorType) && ETextEditorTypeUtils.IsGetContent(editorType) && ETextEditorTypeUtils.IsSetContent(editorType))
            {
                string command = @"<div class=""btn_detection"" onclick=""detection();""><lan>���дʼ��</lan></div>
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
                command = command.Replace("[tips_warn]", JsUtils.OpenWindow.GetOpenTipsString("����⵽' + i + '�����дʣ��������û�ɫ����������������ܰ�����'+keyword+'"
                   , JsUtils.OpenWindow.TIPS_WARN));
                command = command.Replace("[tips_success]", JsUtils.OpenWindow.GetOpenTipsString("���ɹ���û�м�⵽�κ����д�", JsUtils.OpenWindow.TIPS_SUCCESS));
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

    //����
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
            //�������û�����д�
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
                command = command.Replace("[tips_warn]", JsUtils.OpenWindow.GetOpenTipsString("�����й���⵽' + i + '�����дʣ����û�ɫ��������", JsUtils.OpenWindow.TIPS_WARN, false, "��������", "autoReplaceKeywords"));
                command = command.Replace("[tips_warn2]", JsUtils.OpenWindow.GetOpenTipsString("����⵽' + i + '�����дʣ��������û�ɫ�������������⽫�����滻", JsUtils.OpenWindow.TIPS_WARN, false, "��������", "autoReplaceKeywords"));
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
            command = command.Replace("[tips_warn]", JsUtils.OpenWindow.GetOpenTipsString("�����й���⵽' + i + '�����д�", JsUtils.OpenWindow.TIPS_WARN));
            builder.Append(command);

            return builder.ToString();
        }
    }
}
