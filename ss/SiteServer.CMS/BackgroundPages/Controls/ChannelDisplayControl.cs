using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using BaiRong.Core;

namespace SiteServer.CMS.BackgroundPages
{
	public class ChannelDisplayControl : Control
	{
		private int nodeID = 0;
		private string auxiliaryTableENName = "";

		public int NodeID
		{
			get { return nodeID; }
			set { nodeID = value; }
		}

		public string AuxiliaryTableENName
		{
			get { return auxiliaryTableENName; }
			set { auxiliaryTableENName = value; }
		}

		protected override void OnInit(EventArgs e)
		{
			string sCode = @"
<script language='javascript'>
function DisplaySpan(box,str)
{
	var span = eval('window.'+str);
	if(box.checked)
	{
		span.style.visibility = 'visible';
		span.all['ValidateAuxiliaryFields'].disabled = false;
		span.all['DisplayNameAuxiliaryFields'].disabled = false;
	}
	else
	{
		span.style.visibility = 'hidden';
		span.all['ValidateAuxiliaryFields'].disabled = true;
		span.all['DisplayNameAuxiliaryFields'].disabled = true;
	}
	
}
</script>
";
			JsManager.RegisterClientScriptBlock(Page, "DisplaySpanForChannelDisplay",sCode);
		}


		protected override void Render(HtmlTextWriter output)
		{
            IEnumerable ienumerable = BaiRongDataProvider.TableMetadataDAO.GetDataSource(auxiliaryTableENName);
			foreach (IDataRecord record in ienumerable)
			{
				string AttributeName = record.GetString(2);
				string DisplayName = record.GetString(9);
				output.Write("\r\n\t\t<tr> \r\n\t\t  <td align=\"center\"> ");
				output.Write(DisplayName);
				output.Write("(");
				output.Write(AttributeName);
				output.Write(")： </td>\r\n\t\t  <td align=\"left\">\r\n\t\t\t<label for=\"ShowAuxiliaryFields\">显示</label>\r\n" +
					"\t\t\t<input name=\"ShowAuxiliaryFields\" type=\"checkbox\" onClick=\"DisplaySpan(this,\'" +
					"");
				output.Write(AttributeName);
				output.Write("\');\" value=\"");
				output.Write(AttributeName);
				output.Write("\" ");
				output.Write("checked");
				output.Write(">\r\n\t\t\t<span id=\"");
				output.Write(AttributeName);
				output.Write("\" style=\"visibility:");
				output.Write("visible");
				output.Write(";\">\r\n\t\t\t\t<label for=\"ValidateAuxiliaryFields\">不可为空</label>\r\n\t\t\t\t<input name=\"Vali" +
					"dateAuxiliaryFields\" type=\"checkbox\" value=\"");
				output.Write(AttributeName);
				output.Write("\" ");
				output.Write(" ");
				output.Write(">\r\n\t\t\t\t显示名称\r\n\t\t\t\t<input name=\"DisplayNameAuxiliaryFields\" type=\"text\" id=\"classNa" +
					"me\" value=\"");
				output.Write(DisplayName);
				output.Write("\" size=\"15\" maxlength=\"15\" ");
				output.Write(" />\r\n\t\t\t</span>\r\n\t\t  </td>\r\n\t\t</tr>\r\n");
			}
		}
	}
}
