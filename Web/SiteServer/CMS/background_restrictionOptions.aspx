<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundRestrictionOptions" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts text="在此设置各栏目页面访问限制规则，修改设置后需要重新生成页面才能使规则生效。" runat="server" />

  <div class="popover popover-static">
    <h3 class="popover-title">访问限制设置</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="220">是否启用页面访问限制功能：</td>
          <td><asp:RadioButtonList ID="IsRestriction" runat="server" RepeatDirection="Horizontal" class="noborder" AutoPostBack="true" OnSelectedIndexChanged="IsRestriction_SelectedIndexChanged"></asp:RadioButtonList></td>
        </tr>
      </table>
  
    </div>
  </div>

  <asp:PlaceHolder ID="PlaceHolder_Options" Visible="false" runat="server">
    <table class="table table-bordered table-hover">
      <tr class="info thead">
        <td>栏目名</td>
        <td width="100">栏目页访问限制</td>
        <td width="100">内容页访问限制</td>
        <td width="50">&nbsp;</td>
      </tr>
      <asp:Repeater ID="rptContents" runat="server">
        <itemtemplate>
          <bairong:NoTagText id="TrHtml" runat="server" />
            <td><nobr>
              <bairong:NoTagText id="NodeTitle" runat="server" />
              </nobr></td>
            <td style="width:100px;"><nobr>
              <bairong:NoTagText id="RestrictionTypeOfChannel" runat="server" />
              </nobr></td>
            <td style="width:100px;"><nobr>
              <bairong:NoTagText id="RestrictionTypeOfContent" runat="server" />
              </nobr></td>
            <td class="center" style="width:50px;"><bairong:NoTagText id="EditLink" runat="server" /></td>
          </tr>
        </itemtemplate>
      </asp:Repeater>
    </table>
  </asp:PlaceHolder>

  <br>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->