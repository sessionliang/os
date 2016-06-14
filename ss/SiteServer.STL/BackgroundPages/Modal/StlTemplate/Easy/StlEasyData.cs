using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using System.Collections.Generic;
using SiteServer.STL.Parser.TemplateDesign;
using SiteServer.CMS.BackgroundPages;
using BaiRong.Controls;
using BaiRong.Core.Data.Provider;

namespace SiteServer.STL.BackgroundPages.Modal.StlTemplate
{
    public class StlEasyData : BackgroundBasePage
    {
        public Repeater rptContents;

        private string elementName;
        private TemplateInfo templateInfo;
        private string includeUrl;
        private bool isStlInsert;
        private int stlIndex;
        private string stlElement;

        private int channelID;
        private int contentID;

        protected override bool IsSinglePage { get { return true; } }

        public static string GetOpenWindowStringByStlChannels(int publishmentSystemID, int templateID, string includeUrl, int stlIndex, string stlEncryptElement)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("stlIndex", stlIndex.ToString());
            arguments.Add("stlElement", stlEncryptElement);
            arguments.Add("elementName", StlChannels.ElementName);
            return JsUtils.Layer.GetOpenLayerString("信息编辑", PageUtils.GetSTLUrl("modal_stlEasyData.aspx"), arguments);
        }

        public static string GetOpenWindowStringByStlContents(int publishmentSystemID, int templateID, string includeUrl, int stlIndex, string stlEncryptElement)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("stlIndex", stlIndex.ToString());
            arguments.Add("stlElement", stlEncryptElement);
            arguments.Add("elementName", StlContents.ElementName);
            return JsUtils.Layer.GetOpenLayerString("信息编辑", PageUtils.GetSTLUrl("modal_stlEasyData.aspx"), arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            int templateID = base.GetIntQueryString("templateID");
            this.templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, templateID);
            this.includeUrl = base.GetQueryString("includeUrl");
            this.isStlInsert = TranslateUtils.ToBool(base.GetQueryString("isStlInsert"));
            this.stlIndex = TranslateUtils.ToInt(base.GetQueryString("stlIndex"));
            this.stlElement = RuntimeUtils.DecryptStringByTranslate(base.GetQueryString("stlElement"));
            this.elementName = base.GetQueryString("elementName");

            if (!IsPostBack)
            {
                this.rptContents.DataSource = StlDataUtility.GetDataSourceByStlElement(base.PublishmentSystemInfo, templateID, this.elementName, this.stlElement);
                
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();

                if (this.rptContents.Items.Count == 1)
                {
                    if (this.elementName == StlChannels.ElementName)
                    {
                        string editUrl = StlEasyChannelEdit.GetRedirectUrl(base.PublishmentSystemID, this.channelID);
                        PageUtils.Redirect(editUrl);
                    }
                    else if (this.elementName == StlContents.ElementName)
                    {
                        string editUrl = StlEasyContentEdit.GetRedirectUrl(base.PublishmentSystemID, this.channelID, this.contentID);
                        PageUtils.Redirect(editUrl);
                    }
                }
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                if (this.elementName == StlChannels.ElementName)
                {
                    this.channelID = TranslateUtils.EvalInt(e.Item.DataItem, "NodeID");
                    string nodeName = NodeManager.GetNodeName(base.PublishmentSystemID, this.channelID);
                    if (!string.IsNullOrEmpty(nodeName))
                    {
                        string editUrl = StlEasyChannelEdit.GetRedirectUrl(base.PublishmentSystemID, this.channelID);

                        ltlTitle.Text = nodeName;
                        ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", editUrl);
                    }
                    else
                    {
                        e.Item.Visible = false;
                    }
                }
                else if (this.elementName == StlContents.ElementName)
                {
                    this.contentID = TranslateUtils.EvalInt(e.Item.DataItem, "ID");
                    this.channelID = TranslateUtils.EvalInt(e.Item.DataItem, "NodeID");
                    string title = TranslateUtils.EvalString(e.Item.DataItem, "Title");
                    if (!string.IsNullOrEmpty(title))
                    {
                        string editUrl = StlEasyContentEdit.GetRedirectUrl(base.PublishmentSystemID, this.channelID, this.contentID);

                        ltlTitle.Text = title;
                        ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", editUrl);
                    }
                }
            }
        }
    }
}
