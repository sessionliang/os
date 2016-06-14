<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.CMS.Pages.Mlib.SubmissionEdit" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <!--#include file="inc/header.aspx"-->
</head>

<body class="main-body">
    <!--#include file="inc/openWindow.html"-->
    <script type="text/javascript" charset="utf-8" src="../../sitefiles/bairong/scripts/independent/validate.js"></script>
    <script language="javascript">
        function selectChannel(nodeName, nodeID) {
            $('#channelName').html(nodeName);
            $('#channelID').val(nodeID);
        }
    </script>
    <div class="path">
        <p>当前位置：投稿系统<span>&gt;</span>修改草稿</p>
    </div>
    <div class="main-cont">
        <bairong:Alerts runat="server"></bairong:Alerts>
        <h3 class="title">
            <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
        </h3>
        <div class="set-area">
            <div class="form">
                <form id="myForm" enctype="multipart/form-data" runat="server">

                    <site:AuxiliaryControl ID="acAttributes" runat="server" />

                    <div class="btn-area">
                        <asp:Button ID="btnSubmit" class="btn btn-primary" OnClick="btnSubmit_OnClick" Text="提 交" runat="server"></asp:Button>
                    </div>
                </form>
            </div>
        </div>
    </div>
</body>
</html>
