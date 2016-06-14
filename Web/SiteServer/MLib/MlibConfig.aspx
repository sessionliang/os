<%@ Page Language="C#" AutoEventWireup="true" Inherits="SiteServer.CMS.BackgroundPages.MLib.MlibConfig" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <!--#include file="../inc/header.aspx"-->


    <link rel="stylesheet" type="text/css" href="jeasyui/themes/gray/easyui.css">
    <link rel="stylesheet" type="text/css" href="jeasyui/themes/icon.css">
    <script type="text/javascript" src="jeasyui/jquery.easyui.min.js"></script>

    <script>
        function getChecked() {
            var nodes = $('#tt').tree('getChecked');
            var s = '';
            for (var i = 0; i < nodes.length; i++) {
                if (s != '') s += ',';
                s += nodes[i].id;
            }
            $('#tbCheckedValues').val(s);
            $('#Submit').click();
        }

        $(function () {
            $('#tt').tree({
                url: 'MlibConfig.aspx?action=GetTreeData&publishmentsystemid=<%=base.PublishmentSystemID%>',
                method: 'get',
                animate: true,
                checkbox: true,
                onlyLeafCheck: false
            });

            $('#tbCheckLevel').keyup(function () {
                $('#tt').tree({
                    url: 'MlibConfig.aspx?action=GetTreeData&publishmentsystemid=<%=base.PublishmentSystemID%>&checklevel=' + $('#tbCheckLevel').val(),
                    method: 'get',
                    animate: true,
                    checkbox: true,
                    onlyLeafCheck: false
                });
            });
        });
    </script>
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <form class="form-inline" runat="server">
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />

        <div class="popover popover-static">
            <h3 class="popover-title">站点配置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">

                    <tr>
                        <td>审核级别：</td>
                        <td>
                            <asp:TextBox Columns="2" Text="3" MaxLength="2" Style="width: 30px;" ID="tbCheckLevel" runat="server" />
                            <asp:RequiredFieldValidator
                                ControlToValidate="tbCheckLevel"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                            <asp:TextBox ID="tbCheckedValues" Style="display: none;" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>审核级别权限：</td>
                        <td>
                            <div class="easyui-panel" style="padding: 5px; height: 400px;">
                                <ul id="tt"></ul>
                            </div>
                        </td>
                    </tr>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button ID="Submit" Style="display: none;" OnClick="Submit_OnClick" runat="server" />
                            <input type="button" value="确 定" onclick="getChecked();" class="btn btn-primary">
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
