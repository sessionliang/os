<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundSystemMessage" %>

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
        <bairong:Alerts runat="server" Text="点击“编辑”可以设置公告状态"></bairong:Alerts>
        <script type="text/javascript">
            $(document).ready(function () {
                loopRows(document.getElementById('systemMessages'), function (cur) { cur.onclick = chkSelect; });
                $(".popover-hover").popover({ trigger: 'hover', html: true });
            });
        </script>

        <div class="well well-small">

            <div>
                <span>注意：</span><asp:TextBox ID="tbDays" Width="30" runat="server" ></asp:TextBox>天内的信息属于最新信息。
                <asp:Button class="btn" ID="btnUpdateDays" OnClick="UpdateDays_OnClick" Text="修改" runat="server" />
            </div>

            <table class="table table-noborder">
                <tr>
                    <td>发布时间：
          <asp:DropDownList ID="ddlCreationDate" class="input-medium" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server">
              <asp:ListItem Text="全部时间" Value="0" Selected="true"></asp:ListItem>
              <asp:ListItem Text="1天内" Value="1"></asp:ListItem>
              <asp:ListItem Text="2天内" Value="2"></asp:ListItem>
              <asp:ListItem Text="3天内" Value="3"></asp:ListItem>
              <asp:ListItem Text="1周内" Value="7"></asp:ListItem>
              <asp:ListItem Text="1个月内" Value="30"></asp:ListItem>
              <asp:ListItem Text="3个月内" Value="90"></asp:ListItem>
              <asp:ListItem Text="半年内" Value="180"></asp:ListItem>
              <asp:ListItem Text="1年内" Value="365"></asp:ListItem>
          </asp:DropDownList>
                        显示条数：
          <asp:DropDownList ID="ddlPageNum" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server">
              <asp:ListItem Text="默认" Value="15" Selected="true"></asp:ListItem>
              <asp:ListItem Text="30" Value="30"></asp:ListItem>
              <asp:ListItem Text="50" Value="50"></asp:ListItem>
              <asp:ListItem Text="100" Value="100"></asp:ListItem>
              <asp:ListItem Text="200" Value="200"></asp:ListItem>
              <asp:ListItem Text="300" Value="300"></asp:ListItem>
          </asp:DropDownList>
                        关键字：
          <asp:TextBox ID="tbKeyword" MaxLength="500" Size="45" runat="server" />
                        <asp:Button class="btn" OnClick="Search_OnClick" ID="Search" Text="搜 索" runat="server" />
                    </td>
                </tr>
            </table>
        </div>


        <table id="systemMessages" class="table table-bordered table-hover">
            <tr class="info thead">
                <td width="65%">通知标题</td>
                <td width="100">发布时间</td>
                <td></td>
                <td width="20">
                    <input type="checkbox" onclick="selectRows(document.getElementById('systemMessages'), this.checked);">
                </td>
            </tr>

            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:LinkButton ID="ltTitle" runat="server"></asp:LinkButton>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltAddDate" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:LinkButton ID="ltEdit" runat="server"></asp:LinkButton>
                        </td>
                        <td class="center">
                            <input type="checkbox" name="SystemMessageIDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' />
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
