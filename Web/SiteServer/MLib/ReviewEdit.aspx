<%@ Page Language="C#" AutoEventWireup="true" Inherits="SiteServer.CMS.BackgroundPages.MLib.ReviewEdit" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
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
                        <td>版本：</td>
                        <td colspan="3">
                            <asp:Literal ID="ltlTabAction" runat="server"></asp:Literal>
                        </td>
                    </tr>

                    <site:AuxiliaryControl ID="acAttributes" runat="server" />

			<script>
				$('td').map(function(){
					if($(this).html()=='推荐：'||$(this).html()=='热点：'||$(this).html()=='醒目：'||$(this).html()=='置顶：'){
						$(this).parent().hide();
					}
				});
                        </script>
                    <asp:PlaceHolder ID="phStatus" runat="server">
                        <tr>
                            <td>状态：</td>
                            <td colspan="3">
                                <asp:RadioButtonList CssClass="radiobuttonlist" ID="ContentLevel" RepeatDirection="Horizontal" class="noborder" runat="server">
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <td>转移到：</td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlNodeID" runat="server">
                            </asp:DropDownList>
                            转移方式:
                            <asp:DropDownList ID="ddlTrunslateType" runat="server">
                                <asp:ListItem Value="Copy">复制</asp:ListItem>
                                <asp:ListItem Value="Cut">剪切</asp:ListItem>
                            </asp:DropDownList>

                        </td>
                    </tr>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" itemIndex="1" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                            <input class="btn btn-info" type="button" onClick="submitPreview();" value="预 览" />
                            <input class="btn" type="button" onclick="history.go(-1);" value="返 回" />

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
