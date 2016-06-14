using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.Parser.StlEntity;

using SiteServer.STL.Parser.Model;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundTemplateReference : BackgroundBasePage
	{
        private bool isLocal;

		public DataGrid DataGrid;

		public Literal ltlTemplateElements;
		public Literal ltlTemplateEntities;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.isLocal = FileConfigManager.Instance.OEMConfig.IsOEM;

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Template, "模板语言参考", AppManager.CMS.Permission.WebSite.Template);

                IDictionary elementsDictionary = StlAll.StlElements.GetElementsNameDictionary();
                IDictionary attributesDictionary = StlAll.StlElements.GetElementsAttributesDictionary();
                this.ltlTemplateElements.Text = this.GetElementsString(elementsDictionary, attributesDictionary, "StlElements", false);

                elementsDictionary = StlAll.StlEntities.GetEntitiesNameDictionary();
                attributesDictionary = StlAll.StlEntities.GetEntitiesAttributesDictionary();
                this.ltlTemplateEntities.Text = this.GetElementsString(elementsDictionary, attributesDictionary, "StlEntities", true);

                //this.TemplateEntities.Text = this.GetEntitiesString(StlEntities.GetStlEntitiesDictionary(), "通用实体");
                //this.TemplateEntities.Text += this.GetEntitiesString(StlContentEntities.GetContentEntitiesDirectory(base.PublishmentSystemInfo), "内容实体");
                //this.TemplateEntities.Text += this.GetEntitiesString(StlChannelEntities.GetChannelEntitiesDirectory(base.PublishmentSystemInfo), "栏目实体");
                //this.TemplateEntities.Text += this.GetEntitiesString(StlCommentEntities.GetCommentEntitiesDirectory(base.PublishmentSystemInfo), "评论实体");
                //this.TemplateEntities.Text += this.GetEntitiesString(StlSqlEntities.GetSqlEntitiesDirectory(), "Sql实体");
			}
		}


        private string GetElementsString(IDictionary elementsDictionary, IDictionary attributesDictionary, string elementsName, bool isEntities)
		{
			string retval = string.Empty;

			if (elementsDictionary != null)
			{
				StringBuilder builder = new StringBuilder();
				string tr = "<tr>";
				foreach (string label in elementsDictionary.Keys)
				{
					string labelName = (string)elementsDictionary[label];
                    string helpUrl = string.Empty;
                    string target = string.Empty;
                    if (this.isLocal)
                    {
                        helpUrl = PageUtils.UNCLICKED_URL;
                    }
                    else
                    {
                        helpUrl = StringUtils.Constants.GetStlUrl(isEntities, label);
                        target = " target=\"_blank\"";
                    }
					string tdName = string.Format("<td width=\"0\" align=\"Left\" ><a href=\"{0}\"{1}>&lt;{2}&gt;</a></td><td width=\"0\" align=\"Left\" >{3}&nbsp;</td>", helpUrl, target, label, labelName);
                    if (isEntities)
                    {
                        tdName = string.Format("<td width=\"0\" align=\"Left\" ><a href=\"{0}\"{1}>{{{2}}}</a></td><td width=\"0\" align=\"Left\" >{3}&nbsp;</td>", helpUrl, target, label, labelName);
                    }

					IDictionary attributesList = (IDictionary)attributesDictionary[label];
					StringBuilder attributesBuilder = new StringBuilder();
                    if (attributesList != null)
                    {
                        foreach (string attributeName in attributesList.Keys)
                        {
                            attributesBuilder.AppendFormat("{0}({1})&nbsp;&nbsp;", attributeName, attributesList[attributeName]);
                        }
                    }

					string tdAttributes = string.Format("<td width=\"0\" align=\"Left\" >{0}<br /></td>", attributesBuilder);

					builder.Append(tr + tdName + tdAttributes);
				}
				retval = builder.ToString();
			}

			return retval;
		}
	}
}
