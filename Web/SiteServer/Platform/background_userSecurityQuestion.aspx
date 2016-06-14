<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundUserSecurityQuestion" %>

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
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />

        <script type="text/javascript">
            $(document).ready(function () {
                loopRows(document.getElementById('userSecurityQuestion'), function (cur) { cur.onclick = chkSelect; });
                $(".popover-hover").popover({ trigger: 'hover', html: true });
            });
        </script>

        <div class="well well-small">
            <table class="table table-noborder">
                <tr>
                    <td>关键字：
          <asp:TextBox ID="tbKeyword" MaxLength="500" Size="45" runat="server" />
                        <asp:Button class="btn" OnClick="Search_OnClick" ID="Search" Text="搜 索" runat="server" />
                    </td>
                </tr>
            </table>
        </div>


        <table id="userSecurityQuestion" class="table table-bordered table-hover">
            <tr class="info thead">
                <td width="150">编号</td>
                <td width="60%">问题</td>
                <td width="10%">是否启用</td>
                <td></td>
                <td width="20">
                    <input type="checkbox" onclick="selectRows(document.getElementById('userNoticeTemplate'), this.checked);">
                </td>
            </tr>

            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="center">
                            <asp:Literal ID="ltIndex" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltQuestion" runat="server"></asp:Literal>
                        </td>                 
                        <td class="center">
                            <asp:Literal ID="ltIsEnable" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:LinkButton ID="ltEdit" runat="server"></asp:LinkButton>
                        </td>
                        <td class="center">
                            <input type="checkbox" name="UserSecurityQuestionIDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>

        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

        <ul class="breadcrumb breadcrumb-button">
            <asp:Button class="btn btn-success" ID="btnAdd" Text="新 增" runat="server" />
            <asp:Button class="btn" ID="Delete" Text="删 除" runat="server" />
        </ul>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
