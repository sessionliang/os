<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.StlTemplate.StlTemplateSelect" %>

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
<bairong:alerts text="选择模板类型及模板名称后可直接进入页面或者开始可视化编辑操作。" runat="server"></bairong:alerts>

  <table class="table table-noborder">
    <tr>
      <td>模版类型：</td>
      <td><asp:DropDownList ID="ddlTemplateType" class="input-medium" AutoPostBack="true" runat="server"></asp:DropDownList></td>
      <td>模版名称：</td>
      <td>
        <asp:DropDownList ID="ddlTemplateID" class="input-medium" AutoPostBack="true" runat="server"></asp:DropDownList>
      </td>
    </tr>
    <asp:PlaceHolder ID="phNodeID" runat="server">
      <tr>
        <td>选择栏目：</td>
        <td colspan="3">
          <asp:DropDownList ID="ddlNodeID" style="width:95%" AutoPostBack="true"  runat="server"></asp:DropDownList>
        </td>
      </tr>
    </asp:PlaceHolder>
    <tr>
      <td colspan="4">

        <table class="table table-bordered table-hover">
          <tr class="info thead">
            <td>页面(点击查看) </td>
            <td width="100">编辑</td>
          </tr>
          <asp:Literal id="ltlSingle" runat="server" />
          <asp:Repeater ID="rptContents" runat="server">
            <itemtemplate>
              <tr>
                <td>
                  <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
                </td>
                <td class="center">
                  <asp:Literal ID="ltlDesignUrl" runat="server"></asp:Literal>
                </td>
              </tr>
            </itemtemplate>
          </asp:Repeater>
        </table>

        <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

      </td>
    </tr>
  </table>

  <hr />
  <table class="table noborder">
    <tr>
      <td class="center">
        <asp:Button class="btn btn-primary" id="btnSubmit" text="确 定"  runat="server" onClick="Submit_OnClick" />
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->