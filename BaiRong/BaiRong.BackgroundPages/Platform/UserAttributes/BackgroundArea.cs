using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;


using BaiRong.Core.Data.Provider;

namespace BaiRong.BackgroundPages
{
    public class BackgroundArea : BackgroundBasePage
    {
        public Repeater rptContents;

        public Button btnAdd;
        public Button btnDelete;

        private int currentAreaID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.GetQueryString("Delete")) && !string.IsNullOrEmpty(base.GetQueryString("AreaIDCollection")))
            {
                ArrayList areaIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("AreaIDCollection"));
                foreach (int areaID in areaIDArrayList)
                {
                    BaiRongDataProvider.AreaDAO.Delete(areaID);
                }
                base.SuccessMessage("成功删除所选区域");
            }
            else if (!string.IsNullOrEmpty(base.GetQueryString("AreaID")) && (!string.IsNullOrEmpty(base.GetQueryString("Subtract")) || !string.IsNullOrEmpty(base.GetQueryString("Add"))))
            {
                int areaID = int.Parse(base.GetQueryString("AreaID"));
                bool isSubtract = (!string.IsNullOrEmpty(base.GetQueryString("Subtract"))) ? true : false;
                BaiRongDataProvider.AreaDAO.UpdateTaxis(areaID, isSubtract);

                PageUtils.Redirect(BackgroundArea.GetRedirectUrl(areaID));
                return;
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_AdminAttributes, "所在区域管理", AppManager.Platform.Permission.Platform_AdminAttributes);

                base.RegisterClientScriptBlock("NodeTreeScript", AreaTreeItem.GetScript(EAreaLoadingType.Management, null));

                if (!string.IsNullOrEmpty(base.GetQueryString("CurrentAreaID")))
                {
                    this.currentAreaID = TranslateUtils.ToInt(base.GetQueryString("CurrentAreaID"));
                    string onLoadScript = this.GetScriptOnLoad(this.currentAreaID);
                    if (!string.IsNullOrEmpty(onLoadScript))
                    {
                        Page.RegisterClientScriptBlock("NodeTreeScriptOnLoad", onLoadScript);
                    }
                }

                NameValueCollection arguments = new NameValueCollection();
                string showPopWinString = string.Empty;

                this.btnAdd.Attributes.Add("onclick", Modal.AreaAdd.GetOpenWindowStringToAdd(BackgroundArea.GetRedirectUrl(0)));

                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetPlatformUrl("background_area.aspx?Delete=True"), "AreaIDCollection", "AreaIDCollection", "请选择需要删除的区域！", "此操作将删除对应区域以及所有下级区域，确认删除吗？"));

                BindGrid();
            }
        }

        public static string GetRedirectUrl(int currentAreaID)
        {
            if (currentAreaID != 0)
            {
                return PageUtils.GetPlatformUrl(string.Format("background_area.aspx?CurrentAreaID={0}", currentAreaID));
            }
            return PageUtils.GetPlatformUrl("background_area.aspx");
        }

        public string GetScriptOnLoad(int currentAreaID)
        {
            if (currentAreaID != 0)
            {
                AreaInfo areaInfo = AreaManager.GetAreaInfo(currentAreaID);
                if (areaInfo != null)
                {
                    string path = string.Empty;
                    if (areaInfo.ParentsCount <= 1)
                    {
                        path = currentAreaID.ToString();
                    }
                    else
                    {
                        path = areaInfo.ParentsPath.Substring(areaInfo.ParentsPath.IndexOf(",") + 1) + "," + currentAreaID.ToString();
                    }
                    return AreaTreeItem.GetScriptOnLoad(path);
                }
            }
            return string.Empty;
        }

        public void BindGrid()
        {
            try
            {
                this.rptContents.DataSource = BaiRongDataProvider.AreaDAO.GetAreaIDArrayListByParentID(0);
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int areaID = (int)e.Item.DataItem;

            AreaInfo areaInfo = AreaManager.GetAreaInfo(areaID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            ltlHtml.Text = BackgroundArea.GetAreaRowHtml(areaInfo, EAreaLoadingType.Management, null);
        }

        public static string GetAreaRowHtml(AreaInfo areaInfo, EAreaLoadingType loadingType, NameValueCollection additional)
        {
            AreaTreeItem treeItem = AreaTreeItem.CreateInstance(areaInfo);
            string title = treeItem.GetItemHtml(loadingType, additional, false);

            string rowHtml = string.Empty;

            if (loadingType == EAreaLoadingType.Management)
            {
                string editUrl = string.Empty;
                string upLink = string.Empty;
                string downLink = string.Empty;
                string checkBoxHtml = string.Empty;

                editUrl = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.AreaAdd.GetOpenWindowStringToEdit(areaInfo.AreaID, BackgroundArea.GetRedirectUrl(areaInfo.AreaID)));

                string urlUp = PageUtils.GetPlatformUrl(string.Format("background_area.aspx?Subtract=True&AreaID={0}", areaInfo.AreaID));
                upLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlUp);

                string urlDown = PageUtils.GetPlatformUrl(string.Format("background_area.aspx?Add=True&AreaID={0}", areaInfo.AreaID));
                downLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlDown);

                checkBoxHtml = string.Format("<input type='checkbox' name='AreaIDCollection' value='{0}' />", areaInfo.AreaID);

                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
    <td>{1}</td>
    <td class=""center"">{2}</td>
    <td class=""center"">{3}</td>
    <td class=""center"">{4}</td>
    <td class=""center"">{5}</td>
    <td class=""center"">{6}</td>
</tr>
", areaInfo.ParentsCount + 1, title, areaInfo.CountOfAdmin, upLink, downLink, editUrl, checkBoxHtml);
            }
            return rowHtml;
        }
    }
}
