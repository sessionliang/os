using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using System.Web.UI;
using SiteServer.Project.Model;
using System.Collections;
using System.Text;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundDocumentLeft : BackgroundBasePage
    {
        public Repeater rptContents;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                this.rptContents.DataSource = DataProvider.DocTypeDAO.GetTypeInfoArrayList(0);
                this.rptContents.ItemDataBound += rptContents_ItemDataBound;
                this.rptContents.DataBind();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DocTypeInfo typeInfo = e.Item.DataItem as DocTypeInfo;

                Literal ltlTypeName = e.Item.FindControl("ltlTypeName") as Literal;
                Literal ltlSubTypes = e.Item.FindControl("ltlSubTypes") as Literal;

                ltlTypeName.Text = typeInfo.TypeName;

                ArrayList subTypeInfoArrayList = DataProvider.DocTypeDAO.GetTypeInfoArrayList(typeInfo.TypeID);

                StringBuilder builder = new StringBuilder();
                foreach (DocTypeInfo subTypeInfo in subTypeInfoArrayList)
                {
                    string url = string.Format("background_document.aspx?TypeID={0}", subTypeInfo.TypeID);
                    builder.AppendFormat(@"
<tr>
    <td>
        
    </td>
    <td>
        <a href=""{0}"" target=""document"">{1}</a>
    </td>
</tr>
", url, subTypeInfo.TypeName);
                }

                ltlSubTypes.Text = builder.ToString();
            }
        }
    }
}
