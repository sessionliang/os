<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.MLib.ContentReferenceSelect1" Trace="false" %>

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
        $(function () {
            $('#tabs').tabs({
                onSelect: function (title) {
                    if (title == '引用到站点') {
                        $('#tbReferenceType').val(0);
                    }
                    else {
                        $('#tbReferenceType').val(1);
                    }
                }
            });
            $('#tt').tree({
                url: 'modal_contentReferenceSelect.aspx?action=getchannel&PublishmentSystemID=<%= base.PublishmentSystemID%>&TargetPublishmentSystemID=<%= this.ddlPublishmentSystemID.SelectedValue%>',
                method: 'get',
                animate: true,
                checkbox: true,
                cascadeCheck:false,
                onlyLeafCheck: false,
                onCheck: function () {
                    var nodes = $('#tt').tree('getChecked');
                    var s = '';
                    for (var i = 0; i < nodes.length; i++) {
                        if (s != '') s += ',';
                        s += nodes[i].id;
                    }
                    $('#tbCheckedValues').val(s);
                }
            });
            var submitBtn = parent.document.getElementById('openWindowBtn');
            $(submitBtn).css('display', 'inline');
        });

    </script>
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <form class="form-inline" runat="server">
        <div id="tabs" style="width: 575px; height: 450px">
            <div title="引用到站点" style="padding: 10px">
                <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
                <bairong:Alerts runat="server" Text="点击栏目名称进行选择"></bairong:Alerts>

                <div class="well well-small">
                    引用到站点：
                    <asp:DropDownList ID="ddlPublishmentSystemID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PublishmentSystemID_OnSelectedIndexChanged"></asp:DropDownList>
                </div>
                <ul id="tt"></ul>
                <asp:TextBox ID="tbCheckedValues" Style="display: none;" runat="server" />
                <asp:TextBox ID="tbReferenceType" Text="0" Style="display: none;" runat="server" />
            </div>
            <div title="引用到第三方平台" style="padding: 10px">
                引用到第三方平台:
                <asp:RadioButtonList runat="server" ID="rblThirdPlatform" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
            </div>
        </div>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
