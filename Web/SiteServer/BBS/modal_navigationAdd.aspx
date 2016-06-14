<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.Modal.NavigationAdd" %>

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
      <td width="80">链接标题：</td>
      <td>
        <asp:TextBox ID="tbTitle"  style="height:20px; padding:2px; vertical-align:middle;"  Columns="60" runat="server" Width="200px"></asp:TextBox>
        <asp:RequiredFieldValidator ControlToValidate="tbTitle" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
      </td>
    </tr>
    <tr>
      <td>高亮：</td>
      <td id="highlight_td">
        <asp:CheckBox id="cbIsB" text="加粗" class="checkboxlist" runat="server" />
        <asp:CheckBox id="cbIsI" text="斜体" class="checkboxlist" runat="server" />
        <asp:CheckBox id="cbIsU" text="下划线" class="checkboxlist" runat="server" />
      </td>
    </tr>
    <tr>
      <td>颜色：</td>
      <td>
      	<asp:TextBox ID="tbColor"  style="height:20px; padding:2px; vertical-align:middle;"  Columns="60" runat="server" Width="60px"></asp:TextBox>
      </td>
    </tr>
    <tr>
      <td>链接地址：</td>
      <td>
        <asp:TextBox ID="tbLinkUrl"  style="height:20px; padding:2px; vertical-align:middle;"  Columns="60" runat="server" Width="200px"></asp:TextBox>
        <asp:RequiredFieldValidator ControlToValidate="tbLinkUrl" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
      </td>
    </tr>
    <tr>
      <td>新窗口打开：</td>
      <td>
        <asp:RadioButtonList ID="rblIsBlank" RepeatDirection="Horizontal" runat="server">
          <asp:ListItem Text="是" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="否" Value="False"></asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->