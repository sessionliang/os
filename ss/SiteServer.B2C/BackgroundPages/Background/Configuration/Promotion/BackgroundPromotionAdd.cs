using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages.Modal;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundPromotionAdd : BackgroundBasePage
    {
        public TextBox tbPromotionName;
        public TextBox tbTags;
        public DateTimeTextBox tbStartDate;
        public DateTimeTextBox tbEndDate;
        public RadioButtonList rblTarget;

        public PlaceHolder phTargetChannel;
        public Literal ltlChannelIDCollection;
        public Button btnSelectChannel;

        public PlaceHolder phTargetContent;
        public Literal ltlIDsCollection;
        public Button btnSelectContent;

        public Literal ltlExcludeChannelIDCollection;
        public Button btnSelectExcludeChannel;
        public Literal ltlExcludeIDsCollection;
        public Button btnSelectExcludeContent;
        public TextBox tbIfAmount;
        public TextBox tbIfCount;
        public TextBox tbDiscount;
        public TextBox tbReturnAmount;
        public CheckBox cbIsReturnMultiply;
        public CheckBox cbIsShipmentFree;
        public CheckBox cbIsGift;
        public TextBox tbGiftName;
        public TextBox tbGiftUrl;
        public TextBox tbDescription;

        private PromotionInfo promotionInfo;

        public static string GetRedirectUrlToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtils.AddQueryString(PageUtils.GetB2CUrl("background_promotionAdd.aspx"), arguments);
        }

        public static string GetRedirectUrlToEdit(int publishmentSystemID, int promotionID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("promotionID", promotionID.ToString());
            return PageUtils.AddQueryString(PageUtils.GetB2CUrl("background_promotionAdd.aspx"), arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.B2C.LeftMenu.ID_ConfigrationB2C, "打折促销设置", string.Empty);

                this.tbStartDate.DateTime = DateTime.Now;
                this.tbEndDate.DateTime = this.tbStartDate.DateTime.AddMonths(1);

                EPromotionTargetUtils.AddListItems(this.rblTarget);
                ControlUtils.SelectListItems(this.rblTarget, EPromotionTargetUtils.GetValue(EPromotionTarget.Site));

                this.ltlChannelIDCollection.Text = @"<input id=""channelIDCollection"" name=""channelIDCollection"" value="""" type=""hidden"">";
                this.btnSelectChannel.Attributes.Add("onclick", ChannelMultipleSelect.GetOpenWindowString(base.PublishmentSystemID, false, "channelAdd"));

                this.ltlIDsCollection.Text = @"<input id=""idsCollection"" name=""idsCollection"" value="""" type=""hidden"">";
                this.btnSelectContent.Attributes.Add("onclick", ContentMultipleSelect.GetOpenWindowString(base.PublishmentSystemID, "idsAdd"));

                this.ltlExcludeChannelIDCollection.Text = @"<input id=""excludeChannelIDCollection"" name=""excludeChannelIDCollection"" value="""" type=""hidden"">";
                this.btnSelectExcludeChannel.Attributes.Add("onclick", ChannelMultipleSelect.GetOpenWindowString(base.PublishmentSystemID, false, "excludeChannelAdd"));

                this.ltlExcludeIDsCollection.Text = @"<input id=""excludeIDsCollection"" name=""excludeIDsCollection"" value="""" type=""hidden"">";
                this.btnSelectExcludeContent.Attributes.Add("onclick", ContentMultipleSelect.GetOpenWindowString(base.PublishmentSystemID, "excludeIDsAdd"));

                if (base.GetQueryString("promotionID") != null)
                {
                    int promotionID = base.GetIntQueryString("promotionID");
                    this.promotionInfo = DataProviderB2C.PromotionDAO.GetPromotionInfo(promotionID);

                    if (this.promotionInfo != null)
                    {
                        this.ltlExcludeChannelIDCollection.Text = string.Format(@"<input id=""excludeChannelIDCollection"" name=""excludeChannelIDCollection"" value=""{0}"" type=""hidden"">", this.promotionInfo.ExcludeChannelIDCollection);
                        foreach (int channelID in TranslateUtils.StringCollectionToIntList(this.promotionInfo.ExcludeChannelIDCollection))
                        {
                            if (channelID > 0)
                            {
                                this.ltlExcludeChannelIDCollection.Text += string.Format(@"<div id=""excludeChannel_{0}"" class=""addr_base addr_normal""><b>{1}</b> <a class=""addr_del"" href=""javascript:;"" onclick=""excludeChannelRemove('{0}')""></a></div>", channelID, NodeManager.GetNodeName(base.PublishmentSystemID, channelID));
                            }
                        }

                        this.ltlChannelIDCollection.Text = string.Format(@"<input id=""channelIDCollection"" name=""channelIDCollection"" value=""{0}"" type=""hidden"">", this.promotionInfo.ChannelIDCollection);
                        foreach (int channelID in TranslateUtils.StringCollectionToIntList(this.promotionInfo.ChannelIDCollection))
                        {
                            if (channelID > 0)
                            {
                                this.ltlChannelIDCollection.Text += string.Format(@"<div id=""channel_{0}"" class=""addr_base addr_normal""><b>{1}</b> <a class=""addr_del"" href=""javascript:;"" onclick=""channelRemove('{0}')""></a></div>", channelID, NodeManager.GetNodeName(base.PublishmentSystemID, channelID));
                            }
                        }

                        this.ltlExcludeIDsCollection.Text = string.Format(@"<input id=""excludeIDsCollection"" name=""excludeIDsCollection"" value=""{0}"" type=""hidden"">", this.promotionInfo.ExcludeIDsCollection);
                        foreach (var value in TranslateUtils.StringCollectionToStringList(this.promotionInfo.ExcludeIDsCollection))
                        {
                            var idTmp = value.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                            if (idTmp.Length > 1)
                            {
                                int channelID = TranslateUtils.ToInt(idTmp[0]);
                                int contentID = TranslateUtils.ToInt(idTmp[1]);

                                if (channelID > 0 && contentID > 0)
                                {
                                    string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, channelID);

                                    this.ltlExcludeIDsCollection.Text += string.Format(@"<div id=""excludeIDs_{0}_{1}"" class=""addr_base addr_normal""><b>{2}</b> <a class=""addr_del"" href=""javascript:;"" onclick=""excludeIDsRemove('{0}_{1}')""></a></div>", channelID, contentID, BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.Title));
                                }
                            }
                        }

                        this.ltlIDsCollection.Text = string.Format(@"<input id=""idsCollection"" name=""idsCollection"" value=""{0}"" type=""hidden"">", this.promotionInfo.IDsCollection);
                        foreach (var value in TranslateUtils.StringCollectionToStringList(this.promotionInfo.IDsCollection))
                        {
                            var idTmp = value.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                            if (idTmp.Length > 1)
                            {
                                int channelID = TranslateUtils.ToInt(idTmp[0]);
                                int contentID = TranslateUtils.ToInt(idTmp[1]);

                                if (channelID > 0 && contentID > 0)
                                {
                                    string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, channelID);

                                    this.ltlIDsCollection.Text += string.Format(@"<div id=""ids_{0}_{1}"" class=""addr_base addr_normal""><b>{2}</b> <a class=""addr_del"" href=""javascript:;"" onclick=""idsRemove('{0}_{1}')""></a></div>", channelID, contentID, BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.Title));
                                }
                            }
                        }

                        this.tbPromotionName.Text = this.promotionInfo.PromotionName;
                        this.tbTags.Text = this.promotionInfo.Tags;
                        this.tbStartDate.DateTime = this.promotionInfo.StartDate;
                        this.tbEndDate.DateTime = this.promotionInfo.EndDate;
                        ControlUtils.SelectListItems(this.rblTarget, this.promotionInfo.Target);
                        this.tbIfAmount.Text = this.promotionInfo.IfAmount.ToString();
                        this.tbIfCount.Text = this.promotionInfo.IfCount.ToString();
                        this.tbDiscount.Text = this.promotionInfo.Discount.ToString();
                        this.tbReturnAmount.Text = this.promotionInfo.ReturnAmount.ToString();
                        this.cbIsReturnMultiply.Checked = this.promotionInfo.IsReturnMultiply;
                        this.cbIsShipmentFree.Checked = this.promotionInfo.IsShipmentFree;
                        this.cbIsGift.Checked = this.promotionInfo.IsGift;
                        this.tbGiftName.Text = this.promotionInfo.GiftName;
                        this.tbGiftUrl.Text = this.promotionInfo.GiftUrl;
                        this.tbDescription.Text = this.promotionInfo.Description;
                    }
                }

                this.rblTarget_OnSelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        public void rblTarget_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            EPromotionTarget target = EPromotionTargetUtils.GetEnumType(this.rblTarget.SelectedValue);
            if (target == EPromotionTarget.Site)
            {
                this.phTargetChannel.Visible = this.phTargetContent.Visible = false;
            }
            else if (target == EPromotionTarget.Channels)
            {
                this.phTargetChannel.Visible = true;
                this.phTargetContent.Visible = false;
            }
            else if (target == EPromotionTarget.Contents)
            {
                this.phTargetChannel.Visible = false;
                this.phTargetContent.Visible = true;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            if (base.GetQueryString("promotionID") != null)
            {
                try
                {
                    int promotionID = base.GetIntQueryString("promotionID");
                    this.promotionInfo = DataProviderB2C.PromotionDAO.GetPromotionInfo(promotionID);
                    if (this.promotionInfo != null)
                    {
                        this.promotionInfo.PromotionName = this.tbPromotionName.Text;
                        this.promotionInfo.Tags = this.tbTags.Text;
                        this.promotionInfo.StartDate = this.tbStartDate.DateTime;
                        this.promotionInfo.EndDate = this.tbEndDate.DateTime;
                        this.promotionInfo.Target = this.rblTarget.SelectedValue;
                        this.promotionInfo.ChannelIDCollection = base.Request.Form["channelIDCollection"];
                        this.promotionInfo.IDsCollection = base.Request.Form["idsCollection"];
                        this.promotionInfo.ExcludeChannelIDCollection = base.Request.Form["excludeChannelIDCollection"];
                        this.promotionInfo.ExcludeIDsCollection = base.Request.Form["excludeIDsCollection"];
                        this.promotionInfo.IfAmount = TranslateUtils.ToDecimal(this.tbIfAmount.Text);
                        this.promotionInfo.IfCount = TranslateUtils.ToInt(this.tbIfCount.Text);
                        this.promotionInfo.Discount = TranslateUtils.ToDecimal(this.tbDiscount.Text);
                        this.promotionInfo.ReturnAmount = TranslateUtils.ToDecimal(this.tbReturnAmount.Text);
                        this.promotionInfo.IsReturnMultiply = this.cbIsReturnMultiply.Checked;
                        this.promotionInfo.IsShipmentFree = this.cbIsShipmentFree.Checked;
                        this.promotionInfo.IsGift = this.cbIsGift.Checked;
                        this.promotionInfo.GiftName = this.tbGiftName.Text;
                        this.promotionInfo.GiftUrl = this.tbGiftUrl.Text;
                        this.promotionInfo.Description = this.tbDescription.Text;
                    }
                    DataProviderB2C.PromotionDAO.Update(this.promotionInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改打折促销", string.Format("打折促销:{0}", this.promotionInfo.PromotionName));

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "打折促销修改失败！");
                }
            }
            else
            {
                try
                {
                    this.promotionInfo = new PromotionInfo { AddDate = DateTime.Now, PublishmentSystemID = base.PublishmentSystemID, IsEnabled = true };

                    this.promotionInfo.PromotionName = this.tbPromotionName.Text;
                    this.promotionInfo.Tags = this.tbTags.Text;
                    this.promotionInfo.StartDate = this.tbStartDate.DateTime;
                    this.promotionInfo.EndDate = this.tbEndDate.DateTime;
                    this.promotionInfo.Target = this.rblTarget.SelectedValue;
                    this.promotionInfo.ChannelIDCollection = base.Request.Form["channelIDCollection"];
                    this.promotionInfo.IDsCollection = base.Request.Form["idsCollection"];
                    this.promotionInfo.ExcludeChannelIDCollection = base.Request.Form["excludeChannelIDCollection"];
                    this.promotionInfo.ExcludeIDsCollection = base.Request.Form["excludeIDsCollection"];
                    this.promotionInfo.IfAmount = TranslateUtils.ToDecimal(this.tbIfAmount.Text);
                    this.promotionInfo.IfCount = TranslateUtils.ToInt(this.tbIfCount.Text);
                    this.promotionInfo.Discount = TranslateUtils.ToDecimal(this.tbDiscount.Text);
                    this.promotionInfo.ReturnAmount = TranslateUtils.ToDecimal(this.tbReturnAmount.Text);
                    this.promotionInfo.IsReturnMultiply = this.cbIsReturnMultiply.Checked;
                    this.promotionInfo.IsShipmentFree = this.cbIsShipmentFree.Checked;
                    this.promotionInfo.IsGift = this.cbIsGift.Checked;
                    this.promotionInfo.GiftName = this.tbGiftName.Text;
                    this.promotionInfo.GiftUrl = this.tbGiftUrl.Text;
                    this.promotionInfo.Description = this.tbDescription.Text;

                    DataProviderB2C.PromotionDAO.Insert(this.promotionInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "添加打折促销", string.Format("打折促销:{0}", this.promotionInfo.PromotionName));

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "打折促销添加失败！");
                }
            }

            if (isChanged)
            {
                PageUtils.Redirect(PageUtils.GetB2CUrl(string.Format("background_promotion.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
            }
        }
    }
}
