<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundConfigurationStorageVideo" %>

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
  <bairong:alerts text="采用独立空间存储视频需要在服务管理菜单中设置好对应的存储空间并在服务器中安装SiteServer Service服务组件" runat="server" />

  <div class="popover popover-static">
    <h3 class="popover-title">视频存储空间设置</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="200">视频存储方式：</td>
          <td>
            <asp:RadioButtonList ID="rblIsVideoStorage" AutoPostBack="true" OnSelectedIndexChanged="rblIsVideoStorage_SelectedIndexChanged" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList>
            <span>设置视频的存储方式</span>
          </td>
        </tr>
        <asp:PlaceHolder ID="phVideoStorage" runat="server">
          <tr>
            <td>选择视频存储空间：</td>
            <td>
              <asp:DropDownList id="ddlVideoStorageID" runat="server"></asp:DropDownList>
              <br><span>在此选择视频存储空间</span>
            </td>
          </tr>
          <tr>
            <td>存储空间路径：</td>
            <td>
              <asp:TextBox Columns="25" MaxLength="50" id="tbVideoStoragePath" runat="server" />
              <asp:RequiredFieldValidator ControlToValidate="tbVideoStoragePath" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
              <asp:RegularExpressionValidator runat="server" ControlToValidate="tbVideoStoragePath" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
              <br><span>设置路径后，视频文件将存储在对应路径之下</span>
            </td>
          </tr>
        </asp:PlaceHolder>
      </table>
  
        <hr />
        <table class="table noborder">
          <tr>
            <td class="center">
              <asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server" />
            </td>
          </tr>
        </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->