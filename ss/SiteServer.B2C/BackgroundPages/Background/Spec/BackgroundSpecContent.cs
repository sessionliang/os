using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;

using System.Text;
using System.Collections.Generic;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundSpecContent : BackgroundBasePage
    {
        public DataGrid dgSpec;
        protected Repeater rptGoods;

        private string returnUrl = string.Empty;
        private int nodeID = 0;
        private int contentID = 0;
        private string sn = string.Empty;
        private List<int> specIDList = new List<int>();
        private Dictionary<int, SpecComboInfo> comboDictionary = new Dictionary<int, SpecComboInfo>();
        private B2CConfigurationInfo configurationInfo;

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID, int contentID, string sn, string returnUrl)
        {
            return PageUtils.GetB2CUrl(string.Format("background_specContent.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&SN={3}&ReturnUrl={4}", publishmentSystemID, nodeID, contentID, sn, StringUtils.ValueToUrl(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ReturnUrl");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.nodeID = base.GetIntQueryString("NodeID");
            this.contentID = base.GetIntQueryString("ContentID");
            this.sn = base.GetQueryString("SN");

            this.configurationInfo = B2CConfigurationManager.GetInstance(this.nodeID);

            if (!IsPostBack)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);

                this.dgSpec.DataSource = DataProviderB2C.SpecDAO.GetSpecInfoList(base.PublishmentSystemID, this.nodeID);
                this.dgSpec.ItemDataBound += new DataGridItemEventHandler(dgSpec_ItemDataBound);
                this.dgSpec.DataBind();

                this.rptGoods.DataSource = SpecManager.GetGoodsInfoList(base.PublishmentSystemID, nodeInfo, this.contentID, this.sn, out this.comboDictionary, out this.specIDList);
                this.rptGoods.ItemDataBound += new RepeaterItemEventHandler(rptGoods_ItemDataBound);
                this.rptGoods.DataBind();
            }
        }

        private void dgSpec_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SpecInfo specInfo = (SpecInfo)e.Item.DataItem;

                Literal ltlSpecName = (Literal)e.Item.FindControl("ltlSpecName");
                Literal ltlItem = (Literal)e.Item.FindControl("ltlItem");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");

                List<SpecComboInfo> comboInfoArrayList = DataProviderB2C.SpecComboDAO.GetSpecComboInfoList(base.PublishmentSystemID, this.contentID, specInfo.SpecID);

                ltlSpecName.Text = specInfo.SpecName;
                StringBuilder builder = new StringBuilder();
                foreach (SpecComboInfo comboInfo in comboInfoArrayList)
                {
                    builder.Append(SpecManager.GetSpecValue(base.PublishmentSystemInfo, comboInfo));
                }

                if (builder.Length == 0)
                {
                    ltlItem.Text = "无";
                }
                else
                {
                    ltlItem.Text = builder.ToString();
                }
                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">设置规格项</a>", Modal.SpecSelect.GetOpenWindowString(base.PublishmentSystemID, this.nodeID, this.contentID, this.sn, specInfo.SpecID, this.returnUrl));
            }
        }

        private void rptGoods_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GoodsInfo goodsInfo = e.Item.DataItem as GoodsInfo;
                GoodsContentInfo goodsContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(base.PublishmentSystemInfo, this.nodeID, goodsInfo.ContentID);
                Literal ltlTr = e.Item.FindControl("ltlTr") as Literal;
                Literal ltlSpec = e.Item.FindControl("ltlSpec") as Literal;
                Literal ltlSN = e.Item.FindControl("ltlSN") as Literal;
                PlaceHolder phStock = e.Item.FindControl("phStock") as PlaceHolder;
                Literal ltlStock = e.Item.FindControl("ltlStock") as Literal;
                Literal ltlPriceMarket = e.Item.FindControl("ltlPriceMarket") as Literal;
                Literal ltlPriceSale = e.Item.FindControl("ltlPriceSale") as Literal;
                Literal ltlIsOnSale = e.Item.FindControl("ltlIsOnSale") as Literal;

                ArrayList comboIDCollection = TranslateUtils.StringCollectionToIntArrayList(goodsInfo.ComboIDCollection);

                if (goodsInfo.IsOnSale)
                {
                    ltlTr.Text = string.Format(@"<tr class=""specIcon success"">");
                }
                else
                {
                    ltlTr.Text = string.Format(@"<tr class=""specIcon"">");
                }

                List<int> comboIDList = TranslateUtils.StringCollectionToIntList(goodsInfo.ComboIDCollection);

                foreach (int specID in this.specIDList)
                {
                    foreach (int comboID in comboIDList)
                    {
                        SpecComboInfo comboInfo = null;
                        if (this.comboDictionary.ContainsKey(comboID))
                        {
                            comboInfo = this.comboDictionary[comboID];
                        }

                        if (comboInfo != null && comboInfo.SpecID == specID)
                        {
                            ltlSpec.Text += string.Format(@"<td align=""center"">{0}</td>", SpecManager.GetSpecValue(base.PublishmentSystemInfo, comboInfo));
                            break;
                        }
                    }
                }

                if (this.configurationInfo.IsVirtualGoods)
                {
                    phStock.Visible = false;
                }
                else
                {
                    ltlStock.Text = string.Format(@"
<input name=""stock"" type=""text"" value=""{0}"" style=""width:50px;"" />", goodsInfo.Stock == -1 ? "-1" : goodsInfo.Stock.ToString());
                }

                ltlSN.Text = string.Format(@"
<input name=""sn"" type=""text"" value=""{0}"" style=""width:180px;"" />", goodsInfo.GoodsSN);
                ltlPriceMarket.Text = string.Format(@"
<input name=""priceMarket"" type=""text"" value=""{0}"" style=""width:50px;"" />", goodsInfo.PriceMarket == -1 ? goodsContentInfo.PriceMarket.ToString() : goodsInfo.PriceMarket.ToString());
                ltlPriceSale.Text = string.Format(@"
<input name=""priceSale"" type=""text"" value=""{0}"" style=""width:50px;"" />", goodsInfo.PriceSale == -1 ? goodsContentInfo.PriceSale.ToString() : goodsInfo.PriceSale.ToString());

                ltlIsOnSale.Text = string.Format(@"<label class=""checkbox""><input name=""isOnSale_{0}"" type=""checkbox"" {1} /> 上架销售</label>", goodsInfo.ComboIDCollection.Replace(",", "_"), goodsInfo.IsOnSale ? @"checked=""checked""" : string.Empty);

                ltlIsOnSale.Text += string.Format(@"<input name=""goodsID"" type=""hidden"" value=""{0}"" />
<input name=""comboIDCollection"" type=""hidden"" value=""{1}"" />", goodsInfo.GoodsID.ToString(), goodsInfo.ComboIDCollection.Replace(",", "_"));
            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                Literal ltlSpec = e.Item.FindControl("ltlSpec") as Literal;
                PlaceHolder phStock = e.Item.FindControl("phStock") as PlaceHolder;

                phStock.Visible = !this.configurationInfo.IsVirtualGoods;

                foreach (int specID in this.specIDList)
                {
                    SpecInfo specInfo = SpecManager.GetSpecInfo(base.PublishmentSystemID, specID);
                    ltlSpec.Text += string.Format(@"<td>{0}</td>", specInfo.SpecName);
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ArrayList goodsIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.Form["goodsID"]);
                if (goodsIDArrayList.Count == 0) goodsIDArrayList.Add(string.Empty);
                ArrayList comboIDCollectionArrayList = TranslateUtils.StringCollectionToArrayList(base.Request.Form["comboIDCollection"]);
                if (comboIDCollectionArrayList.Count == 0) comboIDCollectionArrayList.Add(string.Empty);
                ArrayList snArrayList = TranslateUtils.StringCollectionToArrayList(base.Request.Form["sn"]);
                if (snArrayList.Count == 0) snArrayList.Add(string.Empty);
                ArrayList stockArrayList = TranslateUtils.StringCollectionToArrayList(base.Request.Form["stock"]);
                if (stockArrayList.Count == 0) stockArrayList.Add(string.Empty);
                ArrayList priceMarketArrayList = TranslateUtils.StringCollectionToArrayList(base.Request.Form["priceMarket"]);
                if (priceMarketArrayList.Count == 0) priceMarketArrayList.Add(string.Empty);
                ArrayList priceSaleArrayList = TranslateUtils.StringCollectionToArrayList(base.Request.Form["priceSale"]);
                if (priceSaleArrayList.Count == 0) priceSaleArrayList.Add(string.Empty);

                try
                {
                    DataProviderB2C.GoodsDAO.DeleteNotInList(base.PublishmentSystemID, this.contentID, goodsIDArrayList);

                    List<int> usedSpecIDList = new List<int>();
                    List<int> usedSpecItemIDList = new List<int>();
                    Dictionary<string, GoodsInfo> goodsInfoDic = new Dictionary<string, GoodsInfo>();
                    int goodsContentStock = 0;
                    for (int i = 0; i < goodsIDArrayList.Count; i++)
                    {
                        int goodsID = (int)goodsIDArrayList[i];
                        string comboIDCollection = (string)comboIDCollectionArrayList[i];
                        bool isOnSale = base.Request.Form[string.Format("isOnSale_{0}", comboIDCollection)] == "on";
                        if (!string.IsNullOrEmpty(comboIDCollection))
                        {
                            comboIDCollection = comboIDCollection.Replace("_", ",");
                        }
                        string sn = (string)snArrayList[i];
                        int stock = 0;
                        if (!this.configurationInfo.IsVirtualGoods)
                        {
                            stock = TranslateUtils.ToIntWithNagetive((string)stockArrayList[i], -1);
                            if (stock > 0)
                                goodsContentStock = goodsContentStock + stock;
                        }
                        decimal priceMarket = TranslateUtils.ToDecimalWithNagetive((string)priceMarketArrayList[i], -1);
                        decimal priceSale = TranslateUtils.ToDecimalWithNagetive((string)priceSaleArrayList[i], -1);

                        List<int> specIDList = new List<int>();
                        List<int> specItemIDList = new List<int>();
                        DataProviderB2C.SpecComboDAO.GetSpec(comboIDCollection, out specIDList, out specItemIDList);

                        GoodsInfo goodsInfo = new GoodsInfo(goodsID, base.PublishmentSystemID, this.contentID, comboIDCollection, TranslateUtils.ObjectCollectionToString(specIDList), TranslateUtils.ObjectCollectionToString(specItemIDList), sn, stock, priceMarket, priceSale, isOnSale);

                        if (goodsID > 0)
                        {
                            DataProviderB2C.GoodsDAO.Update(goodsInfo);
                        }
                        else
                        {
                            DataProviderB2C.GoodsDAO.Insert(goodsInfo);
                        }

                        goodsInfoDic.Add(goodsInfo.ComboIDCollection, goodsInfo);

                        if (goodsInfo.IsOnSale)
                        {
                            foreach (int specID in specIDList)
                            {
                                if (!usedSpecIDList.Contains(specID))
                                {
                                    usedSpecIDList.Add(specID);
                                }
                            }
                            foreach (int specItemID in specItemIDList)
                            {
                                if (!usedSpecItemIDList.Contains(specItemID))
                                {
                                    usedSpecItemIDList.Add(specItemID);
                                }
                            }
                        }
                    }

                    string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeID);
                    DataProviderB2C.GoodsContentDAO.UpdateSpec(tableName, base.PublishmentSystemID, contentID, usedSpecIDList, usedSpecItemIDList);

                    //更新商品的Stock
                    DataProviderB2C.GoodsContentDAO.UpdateGoodContentCount(base.PublishmentSystemInfo.AuxiliaryTableForGoods, contentID, goodsContentStock);

                    #region 更新引用该商品内容的规格
                    //更新引用该商品内容的规格
                    ArrayList sourceContentIDList = new ArrayList();
                    sourceContentIDList.Add(contentID);
                    ArrayList tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.GoodsContent);
                    foreach (AuxiliaryTableInfo table in tableArrayList)
                    {
                        ArrayList targetContentIDList = BaiRongDataProvider.ContentDAO.GetReferenceIDArrayList(table.TableENName, sourceContentIDList);
                        foreach (int targetContentID in targetContentIDList)
                        {
                            GoodsContentInfo targetGoodsContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(table.TableENName, targetContentID);

                            //删除原有的Goods
                            ArrayList noDeleteIDCollection = new ArrayList();
                            noDeleteIDCollection.Add(0);
                            DataProviderB2C.GoodsDAO.DeleteNotInList(targetGoodsContentInfo.PublishmentSystemID, targetContentID, noDeleteIDCollection);

                            List<int> targetUsedSpecIDList = new List<int>();
                            List<int> targetUsedSpecItemIDList = new List<int>();
                            Dictionary<int, List<SpecComboInfo>> dictionary = new Dictionary<int, List<SpecComboInfo>>();
                            foreach (int specID in DataProviderB2C.SpecDAO.GetSpecIDList(targetGoodsContentInfo.PublishmentSystemID, targetGoodsContentInfo.NodeID))
                            {
                                List<SpecComboInfo> comboInfoList = DataProviderB2C.SpecComboDAO.GetSpecComboInfoList(targetGoodsContentInfo.PublishmentSystemID, targetContentID, specID);
                                if (comboInfoList.Count > 0)
                                {
                                    foreach (SpecComboInfo comboInfo in comboInfoList)
                                    {
                                        comboDictionary[comboInfo.ComboID] = comboInfo;
                                    }
                                    dictionary[specID] = comboInfoList;
                                    specIDList.Add(specID);
                                }
                            }
                            //targetComboIDCollectionArrayList
                            ArrayList targetComboIDCollectionArrayList = SpecManager.GetComboIDCollectionArrayList(targetGoodsContentInfo.PublishmentSystemID, dictionary);
                            int i = 0;
                            foreach (string key in goodsInfoDic.Keys)
                            {
                                goodsInfoDic[key].PublishmentSystemID = targetGoodsContentInfo.PublishmentSystemID;
                                goodsInfoDic[key].ContentID = targetContentID;
                                goodsInfoDic[key].ComboIDCollection = targetComboIDCollectionArrayList[i].ToString();
                                List<int> specIDList = new List<int>();
                                List<int> specItemIDList = new List<int>();
                                DataProviderB2C.SpecComboDAO.GetSpec(targetComboIDCollectionArrayList[i].ToString(), out specIDList, out specItemIDList);
                                goodsInfoDic[key].SpecIDCollection = TranslateUtils.ObjectCollectionToString(specIDList);
                                goodsInfoDic[key].SpecItemIDCollection = TranslateUtils.ObjectCollectionToString(specItemIDList);
                                DataProviderB2C.GoodsDAO.Insert(goodsInfoDic[key]);

                                if (goodsInfoDic[key].IsOnSale)
                                {
                                    foreach (int specID in specIDList)
                                    {
                                        if (!targetUsedSpecIDList.Contains(specID))
                                        {
                                            targetUsedSpecIDList.Add(specID);
                                        }
                                    }
                                    foreach (int specItemID in specItemIDList)
                                    {
                                        if (!targetUsedSpecItemIDList.Contains(specItemID))
                                        {
                                            targetUsedSpecItemIDList.Add(specItemID);
                                        }
                                    }
                                }

                                i++;
                            }

                            DataProviderB2C.GoodsContentDAO.UpdateSpec(table.TableENName, targetGoodsContentInfo.PublishmentSystemID, targetContentID, targetUsedSpecIDList, targetUsedSpecItemIDList);
                            //更新商品的Stock
                            DataProviderB2C.GoodsContentDAO.UpdateGoodContentCount(base.PublishmentSystemInfo.AuxiliaryTableForGoods, targetContentID, goodsContentStock);
                        }
                    }
                    #endregion


                    PageUtils.Redirect(this.returnUrl);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "设置规格项失败！");
                }
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.returnUrl);
        }
    }
}
