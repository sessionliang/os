<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundSearchwordSelect" %>

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
        <asp:Literal ID="ltlBreadCrumb" runat="server" />

        <bairong:Alerts Text="允许搜索方式：允许搜索选中栏目的内容，其他栏目内容搜索不到；不允许搜索方式：不允许搜索选中栏目的内容，其他栏目的内容可以被搜索到。" runat="server" />

        <script type="text/javascript" language="javascript">
            function selectAllToAllow(isChecked) {
                for (var i = 0; i < document.getElementById('<%=NodeIDCollectionToAllow.ClientID%>').options.length; i++) {
                    document.getElementById('<%=NodeIDCollectionToAllow.ClientID%>').options[i].selected = isChecked;
                }
            }
            function selectAllToNoAllow(isChecked) {
                for (var i = 0; i < document.getElementById('<%=NodeIDCollectionToNoAllow.ClientID%>').options.length; i++) {
                    document.getElementById('<%=NodeIDCollectionToNoAllow.ClientID%>').options[i].selected = isChecked;
                }
            }
        </script>

        <div class="popover popover-static">
            <h3 class="popover-title">搜索范围设置</h3>
            <div class="popover-content">

                <table class="table noborder">
                    <tr>
                        <td>选择搜索方式：</td>
                        <td>
                            <label class="checkbox inline" style="vertical-align: bottom">
                                <asp:RadioButtonList ID="rbIsAllow" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbIsAllow_SelectedIndexChanged"/>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td width="160">选择搜索范围：<br />
                            <span class="gray">按住Ctrl可多选</span></td>
                        <td style="vertical-align: bottom;">

                            <asp:PlaceHolder ID="phAllow" runat="server">
                                <asp:ListBox ID="NodeIDCollectionToAllow" SelectionMode="Multiple" Rows="19" Style="width: auto" runat="server"></asp:ListBox>&nbsp;&nbsp;
                            <label class="checkbox inline" style="vertical-align: bottom">
                                <input type="checkbox" onclick="selectAllToAllow(this.checked);">
                                全选
                            </label>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="phNoAllow" runat="server">
                                <asp:ListBox ID="NodeIDCollectionToNoAllow" SelectionMode="Multiple" Rows="19" Style="width: auto" runat="server"></asp:ListBox>&nbsp;&nbsp;
                            <label class="checkbox inline" style="vertical-align: bottom">
                                <input type="checkbox" onclick="selectAllToNoAllow(this.checked);">
                                全选
                            </label>
                            </asp:PlaceHolder>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button class="btn btn-primary" ID="SubmitButton" Text="确 定" OnClick="SubmitButton_OnClick" runat="server" /></td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
