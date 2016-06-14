<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.STL.BackgroundPages.Modal.StlTemplate.StlEasyChannelEdit" Trace="false"%>

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
<bairong:alerts runat="server"></bairong:alerts>

  <table class="table table-noborder table-hover">
      <tr>
        <td width="120">栏目名称：</td>
        <td>
          <asp:TextBox Columns="45" MaxLength="255" id="tbNodeName" runat="server"/>
          <asp:RequiredFieldValidator id="RequiredFieldValidator" ControlToValidate="tbNodeName" errorMessage=" *" foreColor="red" Display="Dynamic" runat="server"/>
        </td>
      </tr>
      <tr>
        <td>栏目索引：</td>
        <td>
          <asp:TextBox Columns="45" MaxLength="255" id="tbNodeIndexName" runat="server"/>
          <asp:RegularExpressionValidator
            runat="server"
            ControlToValidate="tbNodeIndexName"
            ValidationExpression="[^']+"
            errorMessage=" *" foreColor="red" 
            Display="Dynamic" />
        </td>
      </tr>
      <tr>
        <td>栏目图片地址：</td>
        <td>
          <asp:TextBox ID="tbImageUrl" class="input-xlarge" runat="server"/>
          <asp:Literal id="ltlImageUrlButtonGroup" runat="server" />
        </td>
      </tr>
      <tr>
        <td colspan="2" class="center">
        <site:TextEditorControl id="tecContent" runat="server"></site:TextEditorControl>
        </td>
      </tr>
      <tr>
        <td>外部链接：</td>
        <td>
        <asp:TextBox MaxLength="200" class="input-xlarge" id="tbLinkUrl" runat="server"/>
        <asp:RegularExpressionValidator
          runat="server"
          ControlToValidate="tbLinkUrl"
          ValidationExpression="[^']+"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic" />
        </td>
      </tr>
      <tr>
        <td>链接类型：</td>
      <td>
        <asp:DropDownList id="ddlLinkType" runat="server"></asp:DropDownList>
        <br>
        <span>指示此栏目的链接与栏目下子栏目及内容的关系</span>
      </td>
      </tr>
      <tr>
        <td>栏目组：</td>
        <td>
          <asp:CheckBoxList id="cblNodeGroupNameCollection" DataTextField="NodeGroupName" DataValueField="NodeGroupName" RepeatDirection="Horizontal" class="noborder" RepeatLayout="Flow" runat="server"/>
        </td>
      </tr>
    </table>

  <hr />
  <div class="center">
    <asp:Button class="btn btn-primary" id="btnSubmit" text="确 定"  runat="server" onClick="Submit_OnClick" />
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->