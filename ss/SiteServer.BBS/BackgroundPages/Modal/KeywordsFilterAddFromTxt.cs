using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.BBS.Model;
using System.IO;
using System.Collections;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class KeywordsFilterAddFromTxt : BackgroundBasePage
    {
        protected FileUpload fileTxt;
        protected DropDownList ddlCategory;

        private int id;

        public static string GetOpenWindowString(int publishmentSystemID, int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ID", id.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("导入词库", PageUtils.GetBBSUrl("modal_keywordsFilterAddFromTxt.aspx"), arguments, 400, 300);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.id = base.GetIntQueryString("ID");
            if (!IsPostBack)
            {
                InitCategory();
            }
        }

        public void InitCategory()
        {
            IList<KeywordsCategoryInfo> list = DataProvider.KeywordsCategoryDAO.GetKeywordsCategoryList(base.PublishmentSystemID);
            if (list.Count == 0)
            {
                DataProvider.KeywordsCategoryDAO.CreateDefaultKeywordsCategory(base.PublishmentSystemID);
                list = DataProvider.KeywordsCategoryDAO.GetKeywordsCategoryList(base.PublishmentSystemID);
            }
            ddlCategory.DataSource = list;
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataValueField = "CategoryID";
            ddlCategory.DataBind();
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            if (!string.IsNullOrEmpty(ddlCategory.SelectedValue))
            {
                if (id == 0)
                {
                    int categoryid = ConvertHelper.GetInteger(ddlCategory.SelectedValue);
                    string fileExtend = "";//文件扩展名
                    int fileSize = 0;//文件大小
                    try
                    {
                        if (fileTxt.PostedFile.FileName != string.Empty)
                        {
                            fileSize = fileTxt.PostedFile.ContentLength;//得到文件的大小
                            if (fileSize == 0)
                            {
                                throw new Exception("导入的txt文件大小为0，请检查是否正确！");
                            }
                            fileExtend = fileTxt.PostedFile.FileName.Substring(fileTxt.PostedFile.FileName.LastIndexOf(".") + 1);//得到扩展名
                            if (fileExtend.ToLower() != "txt")
                            {
                                throw new Exception("你选择的文件格式不正确，只能导入txt文件！");
                            }
                            string path = this.Page.Server.MapPath("../../SiteFiles/TemporaryFiles" + "\\" + fileTxt.PostedFile.FileName);
                            fileTxt.SaveAs(path);
                            string strContent = FileUtils.ReadText(path, ECharset.gb2312);
                            File.Delete(path);
                            String[] str = new String[9999];
                            str = strContent.Replace("\r\n", ",").Split(',');
                            for (int i = 0; i < str.Length; i++)
                            {
                                String[] mystr = new String[2];
                                mystr = str[i].Split('|');
                                KeywordsFilterInfo info = new KeywordsFilterInfo(0, base.PublishmentSystemID, categoryid, ConvertHelper.GetInteger(mystr[1]), mystr[0], "******", 0);

                                DataProvider.KeywordsFilterDAO.Insert(base.PublishmentSystemID, info);
                                isChanged = true;
                            }
                        }
                        else
                        {
                            throw new Exception("请选择要导入的 txt文件!");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            else
            {
                isChanged = false;
                base.FailMessage("类别为空，不能添加！");
            }
            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundKeywordsFilting.GetRedirectUrl(base.PublishmentSystemID));
            }
        }

    }
}
