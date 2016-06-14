<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.MachineAdd" Trace="false"%>

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
      <td ><bairong:help HelpText="服务器名称" Text="服务器名称：" runat="server" ></bairong:help></td>
      <td><asp:TextBox Columns="35" MaxLength="200" id="MachineName" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="MachineName" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="MachineName"
						ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td ><bairong:help HelpText="服务器连接方式" Text="连接方式：" runat="server" ></bairong:help></td>
      <td><asp:DropDownList ID="ConnectionType" AutoPostBack="true" OnSelectedIndexChanged="ConnectionType_SelectedIndexChanged" runat="server"></asp:DropDownList></td>
    </tr>
    <asp:PlaceHolder ID="PlaceHolder_Ftp" runat="server">
      <tr>
        <td ><bairong:help HelpText="FTP服务器地址" Text="FTP服务器地址：" runat="server" ></bairong:help></td>
        <td><asp:TextBox Columns="35" MaxLength="200" id="FtpServer" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="FtpServer" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="FtpServer"
						ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      </tr>
      <tr>
        <td ><bairong:help HelpText="FTP服务器端口" Text="FTP服务器端口：" runat="server" ></bairong:help></td>
        <td><asp:TextBox Columns="10" MaxLength="50" Text="21" id="FtpPort" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="FtpPort" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator
						ControlToValidate="FtpPort"
						ValidationExpression="\d+"
						Display="Dynamic"
						ErrorMessage="FTP端口必须为大于零的整数"
						runat="server"/>
          <asp:CompareValidator 
						ControlToValidate="FtpPort" 
						Operator="GreaterThan" 
						ValueToCompare="0" 
						Display="Dynamic"
						ErrorMessage="FTP端口必须为大于零的整数"
						runat="server"/></td>
      </tr>
      <tr>
        <td ><bairong:help HelpText="FTP用户名" Text="FTP用户名：" runat="server" ></bairong:help></td>
        <td><asp:TextBox Columns="35" MaxLength="200" id="FtpUserName" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="FtpUserName" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="FtpUserName"
						ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      </tr>
      <tr>
        <td ><bairong:help HelpText="FTP密码" Text="FTP密码：" runat="server" ></bairong:help></td>
        <td><asp:TextBox Columns="35" MaxLength="200" TextMode="Password" id="FtpPassword" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="FtpPassword" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="FtpPassword"
						ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      </tr>
      <tr>
        <td ><bairong:help HelpText="FTP根目录" Text="FTP根目录：" runat="server" ></bairong:help></td>
        <td><asp:TextBox Columns="35" MaxLength="200" id="FtpHomeDirectory" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="FtpHomeDirectory"
						ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="PlaceHolder_Network" runat="server">
      <tr>
        <td ><bairong:help HelpText="网上邻居文件夹路径" Text="网上邻居文件夹路径：" runat="server" ></bairong:help></td>
        <td><asp:TextBox Columns="35" MaxLength="200" id="NetworkDirectoryPath" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="NetworkDirectoryPath" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="NetworkDirectoryPath"
						ValidationExpression="^\\\\\w+\\[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
          <br>
          <span class="gray">示例：\\machine\share</span> </td>
      </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="PlaceHolder_Local" runat="server">
      <tr>
        <td ><bairong:help HelpText="本机文件夹路径" Text="本机文件夹路径：" runat="server" ></bairong:help></td>
        <td><asp:TextBox Columns="35" MaxLength="200" id="LocalDirectoryPath" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="LocalDirectoryPath" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="NetworkDirectoryPath"
						ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
          <br>
          <span class="gray">示例：D:\Utils\Backup</span> </td>
      </tr>
    </asp:PlaceHolder>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->