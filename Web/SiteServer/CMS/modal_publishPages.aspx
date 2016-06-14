<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.PublishPages" Trace="false"%>

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
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

  <table class="table table-noborder table-hover">
    <tr>
      <td width="140">生成后发布：</td>
      <td><asp:RadioButtonList ID="IsCreate" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="是" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="否" Value="False"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <asp:PlaceHolder ID="IsChannelPlaceHolder" runat="server">
      <tr>
        <td width="140">是否发布下级栏目：</td>
        <td><asp:RadioButtonList ID="IsIncludeChildren" RepeatDirection="Horizontal" class="noborder" runat="server">
            <asp:ListItem Text="发布下级栏目" Value="True"></asp:ListItem>
            <asp:ListItem Text="仅发布选中栏目" Value="False" Selected="true"></asp:ListItem>
          </asp:RadioButtonList></td>
      </tr>
      <tr>
        <td>是否发布内容页：</td>
        <td><asp:RadioButtonList ID="IsIncludeContents" RepeatDirection="Horizontal" class="noborder" runat="server">
            <asp:ListItem Text="发布内容页" Value="True"></asp:ListItem>
            <asp:ListItem Text="不发布内容页" Value="False" Selected="true"></asp:ListItem>
          </asp:RadioButtonList></td>
      </tr>
    </asp:PlaceHolder>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->