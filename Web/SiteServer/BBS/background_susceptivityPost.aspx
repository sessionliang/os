<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundSusceptivityPost" %>

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

  <script language="JavaScript" type="text/JavaScript">
    function checkstate(frm, state) {
        frm = eval(frm);
        var a = 0;
        var messon = "确定要" + state + "所选中的问题吗？";
        var messoff = "您还没有选择操作的对象！";
        for (var i = 0; i < frm.length; i++) {
            var cb = frm.elements[i];
            if (cb.value == "on") {
                if (cb.checked) { a = a + 1; }
            }
        }
        if (a == 0) { alert(messoff); return false; }
        else {
            if (confirm(messon))
            { return true; }
            else { return false; }
        }
        return true;
    }
  </script>

  <table class="table table-bordered table-hover">
    <tr class="info thead">
      <td class="center">
          序号
      </td>
      <td class="center">
          主题
      </td>
      <td class="center">
          敏感词
      </td>
      <td class="center">
          发布者
      </td>
      <td class="center">
          发布时间
      </td>
      <td></td>
      <td></td>
      <td></td>
      <td width="50"></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center" style="width: 30px;">
              <asp:Literal ID="ltlNum" runat="server"></asp:Literal>
              <asp:Literal ID="ltlID" runat="server" Visible="false"></asp:Literal>
          </td>
          <td class="center">
              <asp:Literal ID="ltlThread" runat="server"></asp:Literal>
          </td>
          <td class="center">
              <asp:Literal ID="ltlKeyWords" runat="server"></asp:Literal>
          </td>
          <td class="center">
              <asp:Literal ID="ltlUser" runat="server"></asp:Literal>
          </td>
          <td class="center">
              <asp:Literal ID="ltlTime" runat="server"></asp:Literal>
          </td>
          <td class="center">
              <asp:Literal ID="ltlLookUp" runat="server"></asp:Literal>
          </td>
          <td class="center">
              <asp:Literal id="ltlPassUrl" runat="server" />
          </td>
          <td class="center" style="width: 80px;">
              <asp:Literal id="ltlDeleteUrl" runat="server" />
          </td>
          <td class="center">
              <asp:CheckBox ID="chk_ID" runat="server" />
          </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button ID="btnPass" Cssclass="btn btn-success" runat="server" Text="通 过" OnClick="btnPass_Click" />
    <asp:Button ID="btnDel" Cssclass="btn" runat="server" Text="删 除" OnClick="btnDel_Click" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->