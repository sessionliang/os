<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundUserNoticeTemplate" %>

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
                loopRows(document.getElementById('systemMessages'), function (cur) { cur.onclick = chkSelect; });
                $(".popover-hover").popover({ trigger: 'hover', html: true });
            });
        </script>

        <div class="well well-small">
            <table class="table table-noborder">
                <tr>
                    <td>提醒信息类型：
          <asp:DropDownList ID="ddlUserNoticeType" class="input-medium" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server">
          </asp:DropDownList>
                        信息模板类型：
          <asp:DropDownList ID="ddlUserNoticeTemplateType" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server">
          </asp:DropDownList>
                        关键字：
          <asp:TextBox ID="tbKeyword" MaxLength="500" Size="45" runat="server" />
                        <asp:Button class="btn" OnClick="Search_OnClick" ID="Search" Text="搜 索" runat="server" />
                    </td>
                </tr>
            </table>
        </div>


        <table id="userNoticeTemplate" class="table table-bordered table-hover">
            <tr class="info thead">
                <td width="200">模板名称</td>
                <td width="10%">标题模板</td>
                <td width="30%">内容模板</td>
                <td width="100">提醒信息类型</td>
                <td width="100">信息模板类型</td>
                <td width="100">是否可用</td>
                <td></td>
                <td width="20">
                    <input type="checkbox" onclick="selectRows(document.getElementById('userNoticeTemplate'), this.checked);">
                </td>
            </tr>

            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="center">
                            <asp:Literal ID="ltName" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltContent" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltUserNoticetType" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltUesrNoticeTemplateType" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltIsEnable" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:LinkButton ID="ltEdit" runat="server"></asp:LinkButton>
                        </td>
                        <td class="center">
                            <input type="checkbox" name="UserNoticeTemplateIDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' />
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
