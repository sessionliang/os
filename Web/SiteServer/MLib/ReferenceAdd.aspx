<%@ Page Language="C#" AutoEventWireup="true" Inherits="SiteServer.CMS.BackgroundPages.MLib.ReferenceAdd" %>


<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <!--#include file="../inc/header.aspx"-->
    <!--[if lte IE 6]>
   <script type="text/javascript">
       $(function(){
           $('input[type=button]').width(24);
       });
   </script>
<![endif]-->

</head>
<body>
    <!--#include file="../inc/openWindow.html"-->
    <form id="myForm" class="form-inline" enctype="multipart/form-data" runat="server">
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />

        <script type="text/javascript" charset="utf-8" src="../../sitefiles/bairong/scripts/independent/validate.js"></script>
        <script language="javascript" type="text/javascript">
            $(document).keypress(function (e) {
                if (e.ctrlKey && e.which == 13 || e.which == 10) {
                    e.preventDefault();
                    $("#Submit").click();
                } else if (e.shiftKey && e.which == 13 || e.which == 10) {
                    e.preventDefault();
                    $("#Submit").click();
                }
            })

            $(function () {
                window.parent.document.getElementById('openWindowBtn').onclick = function () {
                    Submit.click();
                };
            });
        </script>

        <div class="popover popover-static">
            <h3 class="popover-title">
                <asp:Literal ID="ltlPageTitle" runat="server" /></h3>
            <div class="popover-content">

                <table class="table table-fixed noborder" style="position: relative; top: -30px;">
                    <tr>
                        <td width="100">&nbsp;</td>
                        <td></td>
                        <td width="100"></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>引用方：</td>
                        <td colspan="3">
                            <asp:TextBox ID="tbRTName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" itemIndex="1" Style="display: none;" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                            <br>
                            <span class="gray">提示：按CTRL+回车可以快速提交</span>
                        </td>
                    </tr>
                </table>

            </div>
        </div>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
