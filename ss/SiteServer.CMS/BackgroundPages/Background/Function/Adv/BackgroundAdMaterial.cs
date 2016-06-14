using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;

using BaiRong.Model;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundAdMaterial : BackgroundBasePage
    {
        public DataGrid dgContents;

        public Button AddAdMaterial;
        public Button Delete;

        private int advID = 0;

        public string GetAdvName(int advID)
        {
            string advName = string.Empty;
            AdvInfo advInfo = DataProvider.AdvDAO.GetAdvInfo(advID,base.PublishmentSystemID);
            if (advInfo != null)
            {
                advName = advInfo.AdvName;
            }
            return advName;
        }

        public string GetAdMaterialType(string adTypeStr)
        {
            EAdvType adType = EAdvTypeUtils.GetEnumType(adTypeStr);
            return EAdvTypeUtils.GetText(adType);
        }
        
        public string GetIsEnabled(string isEnabledStr)
        {
            return StringUtils.GetTrueOrFalseImageHtml(isEnabledStr);
        }

        public string GetEditUrl(int adMaterialID)
        {
            return  string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.AdMaterialAdd.GetOpenWindowStringToEdit(adMaterialID, base.PublishmentSystemID, this.advID));
         }

        public void Page_Load(object sender, EventArgs E)
        {
            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.advID = TranslateUtils.ToInt(base.GetQueryString("AdvID"));

            if (base.GetQueryString("Delete") != null)
            {
                int adMaterialID = TranslateUtils.ToInt(base.GetQueryString("AdMaterialID"));
                try
                {
                    DataProvider.AdMaterialDAO.Delete(adMaterialID, base.PublishmentSystemID);

                     StringUtility.AddLog(base.PublishmentSystemID, "删除广告物料", string.Format("广告物料名称：{0}","" ));

                     base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }
            BindGrid();

            if (!Page.IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Advertisement, "固定广告物料管理", AppManager.CMS.Permission.WebSite.Advertisement);
                
                this.AddAdMaterial.Attributes.Add("onclick",Modal.AdMaterialAdd.GetOpenWindowStringToAdd(0, base.PublishmentSystemID, this.advID));
                this.Delete.Attributes.Add("onclick", "return confirm(\"此操作将删除所选广告物料，确定吗？\");");
            }
        }

        public void BindGrid()
        {
            try
            {
                this.dgContents.DataSource = DataProvider.AdMaterialDAO.GetDataSource(this.advID,base.PublishmentSystemID);
                this.dgContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        public void ReFresh(object sender, EventArgs E)
        {
            BindGrid();
        }
         
        public void Delete_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (Request.Form["AdMaterialIDCollection"] != null)
                {
                    ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.Form["AdMaterialIDCollection"]);
                    try
                    {
                        foreach (string adMaterialID in arraylist)
                        {
                            DataProvider.AdMaterialDAO.Delete(TranslateUtils.ToInt(adMaterialID), base.PublishmentSystemID);
                        }

                        StringUtility.AddLog(base.PublishmentSystemID, "删除广告物料", string.Format("广告物料名称：{0}", Request.Form["AdMaterialIDCollection"]));

                        base.SuccessDeleteMessage();
                    }
                    catch (Exception ex)
                    {
                        base.FailDeleteMessage(ex);
                    }
                    BindGrid();
                }
                else
                {
                    base.FailMessage("请选择广告后进行操作！");
                }
            }
        }
    }
}
