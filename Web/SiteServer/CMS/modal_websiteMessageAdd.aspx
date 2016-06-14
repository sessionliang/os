<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.WebsiteMessageAdd" Trace="false"%>

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
      <td width="180">留言名称：</td>
      <td>
        <asp:TextBox Columns="25" MaxLength="50" id="WebsiteMessageName" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="WebsiteMessageName" errorMessage=" *" foreColor="red" display="Dynamic"
						runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="WebsiteMessageName"
						ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
    <tr>
      <td>是否需要审核：</td>
      <td>
        <asp:RadioButtonList ID="IsChecked" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="需要审核" Value="False"></asp:ListItem>
          <asp:ListItem Text="不需要审核" Value="True" Selected="true"></asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td>是否需要回复：</td>
      <td>
        <asp:RadioButtonList ID="IsReply" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="需要回复" Value="True"></asp:ListItem>
          <asp:ListItem Text="不需要回复" Value="False" Selected="true"></asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td>提交成功提示信息：</td>
      <td>
        <asp:TextBox Columns="45" Rows="3" TextMode="MultiLine" MaxLength="50" id="MessageSuccess" runat="server" />
      </td>
    </tr>
    <tr>
      <td>提交失败提示信息：</td>
      <td>
        <asp:TextBox Columns="45" Rows="3" TextMode="MultiLine" MaxLength="50" id="MessageFailure" runat="server" />
      </td>
    </tr>
    <tr>
      <td>允许匿名提交：</td>
      <td>
        <asp:RadioButtonList ID="IsAnomynous" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="允许" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="不允许" Value="False"></asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td>显示验证码：</td>
      <td>
        <asp:RadioButtonList ID="IsValidateCode" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="显示" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="不显示" Value="False"></asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td>表单提交成功后是否隐藏：</td>
      <td>
        <asp:RadioButtonList ID="IsSuccessHide" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="隐藏" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="不隐藏" Value="False"></asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td>表单提交成功后是否刷新页面：</td>
      <td>
        <asp:RadioButtonList ID="IsSuccessReload" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="刷新" Value="True"></asp:ListItem>
          <asp:ListItem Text="不刷新" Value="False" Selected="true"></asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td>启用Ctrl+Enter快速提交：</td>
      <td>
        <asp:RadioButtonList ID="IsCtrlEnter" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="启用" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="不启用" Value="False"></asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->