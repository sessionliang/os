<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundUserConfigWaterMark" %>

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
  <h3 class="popover-title">图片水印设置</h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td width="160">是否启用水印功能：</td>
        <td><asp:RadioButtonList ID="IsWaterMark" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="IsWaterMark_SelectedIndexChanged"></asp:RadioButtonList></td>
      </tr>
      <tr id="WaterMarkPositionRow" runat="server">
        <td width="160">添加水印位置：</td>
        <td><asp:literal id="WaterMarkPosition" runat="server"></asp:literal></td>
      </tr>
      <tr id="WaterMarkTransparencyRow" runat="server">
        <td width="160">水印透明度：</td>
        <td><asp:DropDownList ID="WaterMarkTransparency" runat="server"></asp:DropDownList></td>
      </tr>
      <tr id="WaterMarkMinRow" runat="server">
        <td width="160">图片最小尺寸：</td>
        <td> 宽：
          <asp:TextBox ID="WaterMarkMinWidth" class="input-mini" MaxLength="50" runat="server"></asp:TextBox>
          <asp:RequiredFieldValidator ControlToValidate="WaterMarkMinWidth" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator ControlToValidate="WaterMarkMinWidth" ValidationExpression="\d+" Display="Dynamic" ErrorMessage="此项必须为数字" runat="server"/>
          <span class="gray">0代表不限制</span>
          高：
          <asp:TextBox ID="WaterMarkMinHeight" class="input-mini" MaxLength="50" runat="server"></asp:TextBox>
          <asp:RequiredFieldValidator ControlToValidate="WaterMarkMinHeight" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator ControlToValidate="WaterMarkMinHeight" ValidationExpression="\d+" Display="Dynamic" ErrorMessage="此项必须为数字" runat="server"/>
          <span class="gray">0代表不限制</span>
        </td>
      </tr>
      <tr id="IsImageWaterMarkRow" runat="server">
        <td>水印类型：</td>
        <td><asp:RadioButtonList ID="IsImageWaterMark" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="IsWaterMark_SelectedIndexChanged"></asp:RadioButtonList></td>
      </tr>
      <tr id="WaterMarkFormatStringRow" runat="server">
        <td>文字型水印的内容：</td>
        <td><asp:TextBox Columns="25" MaxLength="50" id="WaterMarkFormatString" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="WaterMarkFormatString" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="WaterMarkFormatString" ValidationExpression="[^']+" ErrorMessage="此项不能为空" Display="Dynamic" />
          <br />
          <span class="gray">可以使用替换变量: {1}表示当前日期 {2}表示当前时间 例如:&quot;上传于{1} {2}&quot; </span>
          </td>
      </tr>
      <tr id="WaterMarkFontNameRow" runat="server">
        <td>文字水印的字体：</td>
        <td><asp:DropDownList ID="WaterMarkFontName" runat="server"></asp:DropDownList></td>
      </tr>
      <tr id="WaterMarkFontSizeRow" runat="server">
        <td>文字水印的大小：</td>
        <td><asp:TextBox class="input-mini" MaxLength="50" id="WaterMarkFontSize" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="WaterMarkFontSize" ErrorMessage="此项不能为空" Display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator
            ControlToValidate="WaterMarkFontSize"
            ValidationExpression="\d+"
            Display="Dynamic"
            ErrorMessage="此项必须为数字"
            runat="server"/></td>
      </tr>
      <tr id="WaterMarkImagePathRow" runat="server">
        <td>图片型水印文件：</td>
        <td><asp:TextBox Columns="35" MaxLength="200" id="WaterMarkImagePath" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="WaterMarkImagePath" ErrorMessage="此项不能为空" Display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="WaterMarkImagePath" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
      </tr>
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