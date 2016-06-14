<%@ Page Language="C#" AutoEventWireup="true" Inherits="SiteServer.CMS.BackgroundPages.MLib.ReviewShow" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <!--#include file="../inc/header.aspx"-->
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <form class="form-inline" id="myForm" runat="server">
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />
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
                    <tr style="height: 0px;">
                        <td width="150">版本:</td>
                        <td colspan="2">
                            <asp:Literal runat="server" ID="ltlTabAction"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div id="content" style="width: 100%; font-size: 20px">
                                <h2 style="text-align: center;">
                                    <asp:Literal runat="server" ID="ltlTitle"></asp:Literal></h2>
                                <br />
                                <center><span style="font-size:18px">发布时间:<asp:Literal runat="server" ID="ltlPostTime"></asp:Literal>　</span>
                                <span style="font-size:18px">作者:<asp:Literal runat="server" ID="ltlAuthor"></asp:Literal>　</span>
                                <span style="font-size:18px">来源:<asp:Literal runat="server" ID="ltlSource"></asp:Literal>　</span>
                                </center>
                                <br />
                                <asp:Literal runat="server" ID="ltlContent"></asp:Literal>
                            </div>
                            <script>
                                $(function () {
                                    $('#content p').css('font-size', '20px');
                                });

                            </script>
                        </td>
                    </tr>                    
                </table>

                <hr />
                <table class="table noborder">
                    <asp:PlaceHolder runat="server" ID="phSHYJ">
                    <tr style="height: 0px;">
                        <td width="150">审核意见:</td>
                        <td colspan="2">
                            <asp:TextBox TextMode="MultiLine" runat="server" ID="tbSHYJ" class="textarea" style="width:550px;height:80px;"></asp:TextBox>
                        </td>
                    </tr>
                    </asp:PlaceHolder>
                    <tr>

                        <td class="center" colspan="3">
                            <asp:Button class="btn btn-primary" itemIndex="1" ID="btnSubmit" Text="" OnClick="Submit_OnClick" runat="server" />
                            <asp:Button class="btn btn-primary" itemIndex="1" ID="btnSubmit1" Text="" OnClick="Submit1_OnClick" runat="server" />
                            <input class="btn" type="button" onclick="history.go(-1);" value="返 回" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <div class="popover popover-static">
            <h3 class="popover-title">审核记录</h3>
            <div class="popover-content">

                <table class="table table-fixed noborder" style="position: relative; top: -30px;">
                    <tr style="height: 0px;">
                        <td width="100">&nbsp;</td>
                        <td width="100"></td>
                        <td width="100"></td>
                        <td width="200"></td>
                    </tr>
                    <tr style="height: 0px;">
                        <td>阶段</td>
                        <td>审核时间</td>
                        <td>操作人</td>
                        <td>审核意见</td>
                    </tr>
                    <asp:Literal runat="server" ID="ltlRecord"></asp:Literal>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
