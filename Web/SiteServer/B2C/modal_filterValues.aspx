<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.B2C.BackgroundPages.Modal.FilterValues" Trace="false"%>

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
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

  <table class="table table-noborder table-hover">
    <tr>
      <td width="110">筛选属性：</td>
      <td><asp:Literal ID="ltlAttributeName" runat="server"></asp:Literal></td>
    </tr>
    <tr>
      <td>默认筛选值：</td>
      <td><asp:Literal ID="ltlValues" runat="server"></asp:Literal></td>
    </tr>
    <tr>
      <td>筛选值类型：</td>
      <td><asp:RadioButtonList ID="rblIsDefaultValues" AutoPostBack="true" OnSelectedIndexChanged="rblIsDefaultValues_OnSelectedIndexChanged" RepeatDirection="Horizontal" runat="server">
          <asp:ListItem Text="采用默认值" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="自定义筛选值" Value="False"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <asp:PlaceHolder ID="phValues" runat="server">
    <tr>
      <td colspan="2">
        <span class="gray">在下框中填写自定义筛选值，项与项之间用回车分隔，名称与值之间用“|”分隔(没有“|”代表名称与值相同)。</span>
      </td>
    </tr>
    <tr>
      <td class="center" colspan="2">
          <asp:TextBox ID="tbValues"  runat="server" TextMode="MultiLine" Width="500" Height="200"></asp:TextBox>
      </td>
    </tr>
    </asp:PlaceHolder>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->