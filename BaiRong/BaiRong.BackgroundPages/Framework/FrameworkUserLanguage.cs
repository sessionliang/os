using System;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;



namespace BaiRong.BackgroundPages
{
    public class FrameworkUserLanguage : BackgroundBasePage
    {
        public DataGrid MyDataGrid;

        public string GetChecked(string language)
        {
            if (string.IsNullOrEmpty(AdminManager.Current.Language) && string.IsNullOrEmpty(language))
            {
                return "checked";
            }
            else
            {
                if (StringUtils.EqualsIgnoreCase(language, AdminManager.Current.Language))
                {
                    return "checked";
                }
            }
            return string.Empty;
        }

        public string GetLanguageName(string language)
        {
            string retval = string.Empty;
            if (StringUtils.EqualsIgnoreCase(language, "en"))
            {
                retval = "English";
            }
            else if (StringUtils.EqualsIgnoreCase(language, "zh-cn"))
            {
                retval = "中文简体";
            }
            else if (StringUtils.EqualsIgnoreCase(language, "zh"))
            {
                retval = "中文繁體";
            }
            else
            {
                retval = "<lan>与浏览器一致</lan>";
            }
            return retval;
        }

        public string GetCurrentLanguage()
        {
            return AdminManager.Current.Language;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(base.GetQueryString("SetLanguage")))
                {
                    string language = PageUtils.FilterXSS(base.GetQueryString("Language"));
                    try
                    {
                        AdministratorInfo adminInfo = AdminManager.Current;
                        adminInfo.Language = language;
                        BaiRongDataProvider.AdministratorDAO.Update(adminInfo);
                        base.AddScript("setTimeout(\"gotoMain()\", 1000);");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "语言设置失败！");
                    }
                }
                BindGrid();
            }
        }

        public void BindGrid()
        {
            try
            {
                string[] fileNames = DirectoryUtils.GetFileNames(PathUtils.MapPath(string.Format("~/{0}/Themes/Language", FileConfigManager.Instance.AdminDirectoryName)));
                ArrayList languages = new ArrayList();
                languages.Add(string.Empty);

                ArrayList list = TranslateUtils.StringArrayToArrayList(fileNames);
                foreach (string lan in list)
                {
                    languages.Add(lan.Replace(".xml", string.Empty));
                }

                MyDataGrid.DataSource = languages;
                MyDataGrid.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }
    }
}
