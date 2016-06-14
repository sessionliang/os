using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;


namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundVoteLog : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnDelete;
        public Button btnReturn;

        private Dictionary<int, string> idTitleMap;
        private int voteID;
        private string returnUrl;

        public static string GetRedirectUrl(int publishmentSystemID, int voteID, string returnUrl)
        {
            return PageUtils.GetWXUrl(string.Format("background_voteLog.aspx?publishmentSystemID={0}&voteID={1}&returnUrl={2}", publishmentSystemID, voteID, StringUtils.ValueToUrl(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            this.voteID = TranslateUtils.ToInt(base.Request.QueryString["voteID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["returnUrl"]);

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.VoteLogDAO.Delete(list);//删除投票记录
                        List<VoteLogInfo> voteLogInfoList = new List<VoteLogInfo>();
                        voteLogInfoList = DataProviderWX.VoteLogDAO.GetVoteLogInfoListByVoteID(base.PublishmentSystemID, this.voteID);//根据投票编号获取投票记录表中所有投票记录集合
                        string voteLogCollection = string.Empty;
                        int allCount = 0;
                        if (voteLogInfoList != null && voteLogInfoList.Count > 0)
                        {
                            allCount = voteLogInfoList.Count;
                            foreach (var vlist in voteLogInfoList)
                            {
                                voteLogCollection = voteLogCollection + vlist.ItemIDCollection + ",";//获取该次投票的所有的投票项目并拼接字符串
                            }
                            int strlength = voteLogCollection.Length;
                            voteLogCollection = voteLogCollection.Substring(0, strlength - 1);

                            Dictionary<int, int> dict = new Dictionary<int, int>();
                            string[] arr = voteLogCollection.Split(',');
                            for (int i = 0; i < arr.Length; i++)
                            {
                                if (dict.ContainsKey(TranslateUtils.ToInt(arr[i])))
                                {
                                    dict[TranslateUtils.ToInt(arr[i])] += 1;//统计该次投票的每个项目的投票次数，重复的投票次数增1
                                }
                                else
                                {
                                    dict[TranslateUtils.ToInt(arr[i])] = 1;
                                }
                            }
                            List<int> otherItemList = new List<int>();
                            foreach (var item in dict)
                            {
                                otherItemList.Add(TranslateUtils.ToInt(item.Key.ToString()));
                                DataProviderWX.VoteItemDAO.UpdateVoteNumByID(TranslateUtils.ToInt(item.Value.ToString()), TranslateUtils.ToInt(item.Key.ToString()));//修改该次投票的每个项目的投票次数
                            }
                            DataProviderWX.VoteItemDAO.UpdateOtherVoteNumByIDList(otherItemList, 0, this.voteID);
                            DataProviderWX.VoteDAO.UpdateUserCountByID(allCount, this.voteID);//修改该次投票的总投票次数

                        }
                        else
                        {
                            DataProviderWX.VoteItemDAO.UpdateAllVoteNumByVoteID(allCount, this.voteID);//修改该次投票的每个项目的投票次数为0
                            DataProviderWX.VoteDAO.UpdateUserCountByID(allCount, this.voteID);//修改该次投票的总投票次数为0
                        }


                        base.SuccessMessage("删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.VoteLogDAO.GetSelectString(this.voteID);
            this.spContents.SortField = VoteLogAttribute.ID;
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {

                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Vote, "投票记录设置", AppManager.WeiXin.Permission.WebSite.Vote);
                List<VoteItemInfo> itemInfoList = DataProviderWX.VoteItemDAO.GetVoteItemInfoList(this.voteID);
                this.idTitleMap = new Dictionary<int, string>();
                foreach (VoteItemInfo itemInfo in itemInfoList)
                {
                    this.idTitleMap[itemInfo.ID] = itemInfo.Title;
                }

                this.spContents.DataBind();

                string urlDelete = PageUtils.AddQueryString(BackgroundVoteLog.GetRedirectUrl(base.PublishmentSystemID, this.voteID, this.returnUrl), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的投票项", "此操作将删除所选投票项，确认吗？"));
                this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false", this.returnUrl));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                VoteLogInfo logInfo = new VoteLogInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlItemIDCollection = e.Item.FindControl("ltlItemIDCollection") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlIPAddress = e.Item.FindControl("ltlIPAddress") as Literal;
                Literal ltlWXOpenID = e.Item.FindControl("ltlWXOpenID") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();

                StringBuilder builder = new StringBuilder();
                foreach (int itemID in TranslateUtils.StringCollectionToIntList(logInfo.ItemIDCollection))
                {
                    if (this.idTitleMap.ContainsKey(itemID))
                    {
                        builder.Append(this.idTitleMap[itemID]).Append(",");
                    }
                }
                if (builder.Length > 0) builder.Length -= 1;
                ltlItemIDCollection.Text = builder.ToString();
                ltlUserName.Text = logInfo.UserName;
                ltlIPAddress.Text = logInfo.IPAddress;
                ltlWXOpenID.Text = logInfo.WXOpenID;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(logInfo.AddDate);
            }
        }

    }
}
