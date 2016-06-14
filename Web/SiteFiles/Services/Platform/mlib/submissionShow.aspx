<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.CMS.Pages.Mlib.SubmissionShow" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <!--#include file="inc/header.aspx"-->
    <style>
        table tr {
        
         border:1px solid #CCC;
        }
    </style>
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
        <p>当前位置：投稿系统<span>&gt;</span>查看稿件</p>
    </div>
    <div class="main-cont">
        <div class="tab-box">
            <h5 class="tab-nav tab-nav-s1 clear"><asp:Literal runat="server" ID="ltlTabAction"></asp:Literal></h5>
            <div class="tab-con-s1">
                <table class="table table-bordered table-striped">
                    <tr style="height:0px;">
                        <td width="150">审核信息</td>
                        <td colspan="2"><asp:Literal runat="server" ID="ltlStatus"></asp:Literal></td>
                    </tr>
                    <asp:Repeater ID="MyRepeater" runat="server">
                        <ItemTemplate>
                            <asp:Literal ID="ltlHtml" runat="server" />
                        </ItemTemplate>
                    </asp:Repeater>
                    <div class="btn-area">
                    </div>
                </table>
            </div>
        </div>
    </div>
</body>
</html>