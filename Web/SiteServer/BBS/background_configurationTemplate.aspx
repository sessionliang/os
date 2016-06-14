<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundConfigurationTemplate" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

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
  <bairong:alerts runat="server" />

  <div class="popover popover-static">
  <h3 class="popover-title">模板生成设置</h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td width="170">是否开启双击生成：</td>
        <td width="900"><asp:RadioButtonList ID="IsCreateDoubleClick" AutoPostBack="false"  RepeatDirection="Horizontal" runat="server">
          <asp:ListItem Text="开启" Value="True" Selected="true"></asp:ListItem>
            <asp:ListItem Text="不开启" Value="False"></asp:ListItem>
        </asp:RadioButtonList></td>
      </tr>
    </table>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <asp:Button class="btn btn-primary" id="Submit" text="修 改" onclick="Submit_OnClick" runat="server" />
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->