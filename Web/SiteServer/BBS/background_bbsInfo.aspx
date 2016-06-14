<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundBBSInfo" %>

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
  <h3 class="popover-title">论坛信息</h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td width="170"><bairong:help HelpText="显示在浏览器窗口标题等位置" Text="论坛名称：" runat="server" ></bairong:help></td>
        <td>
          <asp:TextBox  Columns="40" id="txtBBSName" runat="server" />  
            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtBBSName"
                ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
        </td>
      </tr>
    <tr>
        <td width="170"><bairong:help HelpText="显示在页面底部的联系方式处" Text="网站名称：" runat="server" ></bairong:help></td>
        <td>
          <asp:TextBox  Columns="40" MaxLength="200" id="txtSiteName" runat="server" />  
            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtSiteName"
                ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
        </td>
      </tr>
      <tr>
        <td width="170"><bairong:help HelpText="网站URL" Text="网站URL：" runat="server" ></bairong:help></td>
        <td>
          <asp:TextBox  Columns="40" MaxLength="200" id="txtSiteUrl" runat="server" /> 
            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtSiteUrl"
                ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
        </td>
      </tr>  
      <tr>
        <td width="170"><bairong:help HelpText="管理员邮箱" Text="管理员邮箱：" runat="server" ></bairong:help></td>
        <td>
          <asp:TextBox  Columns="40" MaxLength="200" id="txtAdminEmail" runat="server" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtAdminEmail"
                ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
        </td>
      </tr>
      <tr>
        <td width="170"><bairong:help HelpText="网站第三方统计代码" Text="网站第三方统计代码：" runat="server" ></bairong:help></td>
        <td>
          <asp:TextBox  TextMode="MultiLine" Columns="60" Rows="5" MaxLength="1000" id="txtCountCode" runat="server" />     <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCountCode"
                ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
        </td>
      </tr>
      
       <tr>
        <td width="170"><bairong:help HelpText="是否开启日志功能" Text="是否开启日志功能：" runat="server" ></bairong:help></td>
        <td width="900"><asp:RadioButtonList ID="IsLogBBS" AutoPostBack="false"  RepeatDirection="Horizontal" runat="server">
          <asp:ListItem Text="开启" Value="True" Selected="true"></asp:ListItem>
            <asp:ListItem Text="不开启" Value="False"></asp:ListItem>
        </asp:RadioButtonList></td>
      </tr>
       <tr>
        <td width="170"><bairong:help HelpText="暂时将论坛关闭，其他人无法访问，但不影响管理员访问" Text="是否关闭论坛：" runat="server" ></bairong:help></td>
        <td width="900"><asp:RadioButtonList ID="IsCloseBBS" AutoPostBack="true" OnSelectedIndexChanged="IsCloseBBS_SelectedIndexChanged" RepeatDirection="Horizontal" runat="server">
          <asp:ListItem Text="关闭" Value="True" Selected="true"></asp:ListItem>
            <asp:ListItem Text="不关闭" Value="False"></asp:ListItem>
        </asp:RadioButtonList></td>
      </tr>
      <asp:PlaceHolder ID="phCheck" runat="server">
      <tr>
        <td width="170"><bairong:help HelpText="站点关闭时出现的提示信" Text="关闭站点的原因：" runat="server" ></bairong:help></td>
        <td>
          <asp:TextBox   TextMode="MultiLine" Columns="60" Rows="5" MaxLength="500" id="txtCloseBBSReason" runat="server" />  
            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCloseBBSReason"
                ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
        </td>
      </tr>
      </asp:PlaceHolder>
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