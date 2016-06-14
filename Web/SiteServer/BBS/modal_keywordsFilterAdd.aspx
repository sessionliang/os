<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.Modal.KeywordsFilterAdd" %>

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

  <script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $("#snewcategory").click(function () {
            $("#ddlCategory").hide();
            $("#snewcategory").hide();
            $("#Ok").hide();
            $("#tbCategory").show();
            $("#schoicecategory").show();
            $("#btnNewCategory").show();
        });
        $("#schoicecategory").click(function () {
            $("#ddlCategory").show();
            $("#snewcategory").show();
            $("#Ok").show();
            $("#tbCategory").hide();
            $("#schoicecategory").hide();
            $("#btnNewCategory").hide();
        });
    });
  </script>

    <table class="table table-noborder table-hover">
        <tr>
            <td width="50">敏感词：</td>
            <td>
                <asp:TextBox Width="220" ID="tbName" runat="server" />
            </td>
        </tr>
        <tr>
            <td>等级：</td>
            <td>
                <asp:DropDownList ID="ddlGrade" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlGrade_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Text="禁用" Value="1"></asp:ListItem>
                    <asp:ListItem Text="审核" Value="2"></asp:ListItem>
                    <asp:ListItem Text="替换" Value="3"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="trReplacement" runat="server">
            <td>分类：</td>
            <td>
                <asp:DropDownList ID="ddlCategory" runat="server">
                </asp:DropDownList>
                <asp:TextBox ID="tbCategory" runat="server" MaxLength="18" Width="220" Style="display: none"></asp:TextBox>
                <span id="snewcategory" style="cursor: hand" runat="server">新建分类</span> <span id="schoicecategory" style="display: none; cursor: hand" runat="server">选择分类</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltlReplacement" runat="server">替换为：</asp:Literal> 
            </td>
            <td>
                <asp:TextBox ID="tbReplacement" runat="server" MaxLength="18" Width="220" Text="******"></asp:TextBox>
            </td>
        </tr>
    </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->