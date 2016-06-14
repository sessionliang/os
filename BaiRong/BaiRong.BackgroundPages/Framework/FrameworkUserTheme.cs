using System;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;



namespace BaiRong.BackgroundPages
{
    public class FrameworkUserTheme : BackgroundBasePage
    {
        public DataGrid dgContents;

        public string GetCurrentTheme()
        {
            return AdminManager.Current.Theme;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(base.GetQueryString("SetTheme")) && !string.IsNullOrEmpty(base.GetQueryString("ThemeName")))
                {
                    string themeName = PageUtils.FilterXSS(base.GetQueryString("ThemeName"));
                    try
                    {
                        AdministratorInfo adminInfo = AdminManager.Current;
                        adminInfo.Theme = themeName;
                        BaiRongDataProvider.AdministratorDAO.Update(adminInfo);
                        base.AddScript("setTimeout(\"gotoMain()\", 1000);");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "÷˜Ã‚…Ë÷√ ß∞‹£°");
                    }
                }

                this.dgContents.DataSource = EThemeUtils.GetArrayList();
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ETheme themeType = (ETheme)e.Item.DataItem;
                string theme = EThemeUtils.GetValue(themeType);

                Literal ltlSelect = e.Item.FindControl("ltlSelect") as Literal;
                Literal ltlName = e.Item.FindControl("ltlName") as Literal;

                string checkedStr = string.Empty;
                if ((themeType == ETheme.Default && string.IsNullOrEmpty(AdminManager.Current.Theme)) || StringUtils.EqualsIgnoreCase(theme, AdminManager.Current.Theme))
                {
                    checkedStr = "checked";
                }

                ltlSelect.Text = string.Format(@"<input type=""radio"" name=""choose"" id=""choose"" onClick=""document.getElementById('CurrentTheme').value=this.value;"" value=""{0}"" {1} />", theme, checkedStr);
                ltlName.Text = EThemeUtils.GetText(themeType);
            }
        }
    }
}
