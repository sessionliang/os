<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.Modal.StorageAdd" Trace="false"%>

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
      <td width="120">空间名称：</td>
      <td><asp:TextBox Columns="35" MaxLength="200" id="tbStorageName" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbStorageName" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbStorageName" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
    <tr>
      <td>空间域名：</td>
      <td><asp:TextBox Columns="35" MaxLength="200" id="tbStorageUrl" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbStorageUrl" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbStorageUrl" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
    <tr>
      <td>空间类型：</td>
      <td>
        <asp:DropDownList ID="ddlStorageType" AutoPostBack="true" OnSelectedIndexChanged="ddlStorageType_SelectedIndexChanged" runat="server"></asp:DropDownList>
      </td>
    </tr>
    <asp:PlaceHolder ID="phFtp" runat="server">
      <tr>
        <td>FTP服务器地址：</td>
        <td>
          <asp:TextBox Columns="35" MaxLength="200" id="tbFtpServer" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="tbFtpServer" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="tbFtpServer" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
        </td>
      </tr>
      <tr>
        <td>FTP服务器端口：</td>
        <td>
          <asp:TextBox Columns="10" MaxLength="50" Text="21" id="tbFtpPort" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="tbFtpPort" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator
						ControlToValidate="tbFtpPort"
						ValidationExpression="\d+"
						Display="Dynamic"
						ErrorMessage="FTP端口必须为大于零的整数"
						runat="server"/>
          <asp:CompareValidator 
						ControlToValidate="tbFtpPort" 
						Operator="GreaterThan" 
						ValueToCompare="0" 
						Display="Dynamic"
						ErrorMessage="FTP端口必须为大于零的整数"
						runat="server"/>
        </td>
      </tr>
      <tr>
        <td>FTP用户名：</td>
        <td>
          <asp:TextBox Columns="35" MaxLength="200" id="tbFtpUserName" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="tbFtpUserName" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="tbFtpUserName"
						ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
        </td>
      </tr>
      <tr>
        <td>FTP密码：</td>
        <td>
          <asp:TextBox Columns="35" MaxLength="200" TextMode="Password" id="tbFtpPassword" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="tbFtpPassword" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="tbFtpPassword" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
        </td>
      </tr>
      <tr>
        <td>传输模式：</td>
        <td>
          <asp:RadioButtonList id="rblIsPassiveMode" runat="server" repeatDirection="Horizontal" class="radiobuttonlist" />
        </td>
      </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phLocal" runat="server">
      <tr>
        <td>本机文件夹路径：</td>
        <td>
          <asp:TextBox Columns="35" MaxLength="200" id="tbLocalDirectoryPath" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="tbLocalDirectoryPath" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="tbLocalDirectoryPath"
						ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
          <br>
          <span class="gray">示例：D:\Utils\Backup</span>
        </td>
      </tr>
    </asp:PlaceHolder>
    <tr>
      <td>空间备注：</td>
      <td>
        <asp:TextBox TextMode="MultiLine" Columns="50" Rows="3" id="tbDescription" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbDescription" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbDescription" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->