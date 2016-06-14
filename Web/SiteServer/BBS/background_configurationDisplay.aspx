<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundConfigurationDisplay" %>

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
  <h3 class="popover-title">界面设置</h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td width="170"><bairong:help HelpText="设置宽版/窄版显示" Text="宽版/窄版显示：" runat="server" ></bairong:help></td>
        <td colspan="2"><asp:RadioButtonList id="rblDisplayFullScreen" RepeatDirection="Horizontal" runat="server">
            <asp:ListItem Text="窄版" Value="False" ></asp:ListItem>
            <asp:ListItem Text="宽版" Value="True"></asp:ListItem>
          </asp:RadioButtonList></td>
      </tr>
      <tr>
        <td width="170"><bairong:help HelpText="设置下级子版块横排时每行版块数量" Text="下级子版块横排：" runat="server" ></bairong:help></td>
        <td colspan="2"><asp:DropDownList id="ddlDisplayColumns" runat="server">
            <asp:ListItem Text="1" Value="1" ></asp:ListItem>
            <asp:ListItem Text="2" Value="2"></asp:ListItem>
            <asp:ListItem Text="3" Value="3"></asp:ListItem>
            <asp:ListItem Text="4" Value="4"></asp:ListItem>
            <asp:ListItem Text="5" Value="5"></asp:ListItem>
          </asp:DropDownList></td>
      </tr>
      <tr>
        <td><bairong:help HelpText="每页显示主题数目" Text="每页显示主题数目：" runat="server" ></bairong:help></td>
        <td colspan="2"><asp:TextBox  Columns="10" id="tbThreadPageNum" runat="server" />  
            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbThreadPageNum"
                ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      </tr>
      <tr>
        <td><bairong:help HelpText="每页显示帖子数目" Text="每页显示帖子数目：" runat="server" ></bairong:help></td>
        <td colspan="2"><asp:TextBox  Columns="10" id="tbPostPageNum" runat="server" />  
            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbPostPageNum"
                ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      </tr>
      <tr>
        <td width="170"><bairong:help HelpText="显示在线用户" Text="显示在线用户：" runat="server" ></bairong:help></td>
        <td colspan="2"><asp:RadioButtonList id="rblIsOnlineInIndexPage" RepeatDirection="Horizontal" runat="server">
            <asp:ListItem Text="首页显示" Value="True"></asp:ListItem>
            <asp:ListItem Text="不显示" Value="False" ></asp:ListItem>
          </asp:RadioButtonList></td>
      </tr>
      <tr>
        <td width="170"><bairong:help HelpText="显示在线用户类型" Text="显示在线用户类型：" runat="server" ></bairong:help></td>
        <td colspan="2"><asp:RadioButtonList id="rblIsOnlineUserOnly" RepeatDirection="Horizontal" runat="server">
            <asp:ListItem Text="显示用户与游客" Value="False"></asp:ListItem>
            <asp:ListItem Text="仅显示注册用户" Value="True" ></asp:ListItem>
          </asp:RadioButtonList></td>
      </tr>
      <tr>
        <td><bairong:help HelpText="最多显示在线人数" Text="最多显示在线人数：" runat="server" ></bairong:help></td>
        <td colspan="2"><asp:TextBox  Columns="10" id="tbOnlineMaxInIndexPage" runat="server" />  
            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbOnlineMaxInIndexPage"
                ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      </tr>
      <tr>
        <td><bairong:help HelpText="无动作离线时间" Text="无动作离线时间：" runat="server" ></bairong:help></td>
        <td colspan="2"><asp:TextBox  Columns="10" id="tbOnlineTimeout" runat="server" />  
            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbOnlineTimeout" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />分钟</td>
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