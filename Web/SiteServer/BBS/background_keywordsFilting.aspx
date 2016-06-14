<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundKeywordsFilting" %>

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
    $(document).ready(function () {
        $("#tbkeywords").focus(function () {
            if ($("#tbkeywords").val() == "敏感词关键字") {
                $("#tbkeywords").val("");
            }
        });
        $("#tbkeywords").blur(function () {
            if ($("#tbkeywords").val() == "") {
                $("#tbkeywords").val("敏感词关键字");
            }
        });
    });
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

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          <asp:DropDownList ID="ddlGrade" runat="server">
              <asp:ListItem Selected="True" Text="所有级别" Value="0"></asp:ListItem>
              <asp:ListItem Text="禁用" Value="1"></asp:ListItem>
              <asp:ListItem Text="审核" Value="2"></asp:ListItem>
              <asp:ListItem Text="替换" Value="3"></asp:ListItem>
          </asp:DropDownList>
          <asp:DropDownList ID="ddlCategory" runat="server">
          </asp:DropDownList>
          <asp:TextBox ID="tbkeywords" runat="server" MaxLength="50" Text="敏感词关键字"></asp:TextBox>
          <asp:Button ID="btnquery" class="btn" runat="server" Text="搜索" OnClick="btnquery_Click" />
        </td>
      </tr>
    </table>
  </div>

  <table class="table table-bordered table-hover">
    <tr class="info thead">
      <td class="center">序号</td>
      <td class="center">敏感词</td>
      <td class="center">敏感级别</td>
      <td class="center">分类</td>
      <td></td>
      <td></td>
      <td></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center" style="width: 30px;">
              <asp:Literal ID="ltlNum" runat="server"></asp:Literal>
              <asp:Literal ID="ltlID" runat="server" Visible="false"></asp:Literal>
          </td>
          <td class="center">
              <asp:Literal ID="ltlName" runat="server"></asp:Literal>
          </td>
          <td class="center">
              <asp:Label ID="lblGrade" runat="server"></asp:Label>
          </td>
          <td class="center">
              <asp:Literal ID="ltlCategory" runat="server"></asp:Literal>
          </td>
          <td class="center">
              <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
          </td>
          <td class="center" style="width: 80px;">
              <asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
          </td>
          <td class="center" style="width: 50px;">
              <asp:CheckBox ID="chk_ID" runat="server" />
          </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button ID="btnAdd" Cssclass="btn btn-success" runat="server" Text="添 加" />
    <asp:Button ID="btnDelAll" Cssclass="btn" runat="server" Text="删 除" OnClick="btnDelAll_Click" />
    <asp:Button ID="btnImport" runat="server" Text="导入词库" class="btn" />
    <asp:Button ID="btnExport" runat="server" Text="导出词库" class="btn" OnClick="btnExport_Click" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->