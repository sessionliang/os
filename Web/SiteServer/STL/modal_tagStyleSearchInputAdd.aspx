<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.TagStyleSearchInputAdd" Trace="false"%>

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
      <td width="150">样式名称：</td>
      <td><asp:TextBox Columns="25" MaxLength="50" id="StyleName" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="StyleName" errorMessage=" *" foreColor="red" display="Dynamic"
						runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="StyleName"
						ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td>搜索结果页地址：</td>
      <td><asp:TextBox style="width:220px" Columns="25" MaxLength="50" id="SearchUrl" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="SearchUrl" errorMessage=" *" foreColor="red" display="Dynamic"
						runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="SearchUrl"
						ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td>搜索结果显示：</td>
      <td><asp:RadioButtonList ID="OpenWin" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="新窗口" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="本窗口" Value="False"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <tr>
      <td>搜索框宽度：</td>
      <td><asp:TextBox class="input-mini" MaxLength="50" id="InputWidth" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="InputWidth" errorMessage=" *" foreColor="red" display="Dynamic"
						runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="InputWidth"
						ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td>显示搜索的字段类型：</td>
      <td><asp:RadioButtonList ID="IsType" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="显示" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="不显示" Value="False"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <tr>
      <td>显示需要搜索的栏目：</td>
      <td><asp:RadioButtonList ID="IsChannel" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="显示" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="不显示" Value="False"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <tr>
      <td>栏目显示方式：</td>
      <td><asp:RadioButtonList ID="IsChannelRadio" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="下拉列表" Value="False" Selected="true"></asp:ListItem>
          <asp:ListItem Text="单选框" Value="True"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <tr>
      <td>显示日期下拉框：</td>
      <td><asp:RadioButtonList ID="IsDate" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="显示" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="不显示" Value="False"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <tr>
      <td>显示日期范围：</td>
      <td><asp:RadioButtonList ID="IsDateFrom" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="显示" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="不显示" Value="False"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->