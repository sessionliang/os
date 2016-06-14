<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.WeiXin.BackgroundPages.Modal.FunctionSelect" Trace="false"%>

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
<bairong:alerts runat="server" />

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          <asp:DropDownList ID="ddlKeywordType" runat="server" OnSelectedIndexChanged="ReFresh" AutoPostBack="true"></asp:DropDownList>
         </td>
      </tr>
    </table>
  </div>

  <asp:PlaceHolder id="phFunction" runat="server" visible="false">

  <table id="contents" class="table table-bordered">
    <tr class="info thead">
      <td>点击选择</td>
    </tr>
    <tr><td>
      <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
      </itemtemplate>
    </asp:Repeater>
    </td></tr>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  </asp:PlaceHolder>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->