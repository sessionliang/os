<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundMain" Trace="False" EnableViewState="false" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent" />
    <base target="right" />
    <link rel="stylesheet" type="text/css" href="scripts/lib/bootstrap/css/bootstrap.min.css" media="all" />
    <link href="scripts/lib/font-awesome/css/font-awesome.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="css/style.css" media="all" />
    <script type="text/javascript" src="scripts/lib/jquery/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="scripts/js/main.js"></script>
    <script type="text/javascript" src="scripts/lib/bootstrap/js/bootstrap.min.js"></script>
    <title>
        <asp:Literal ID="ltlTitle" runat="server"></asp:Literal></title>
    <!--[if IE 7]><link href="scripts/lib/font-awesome/css/font_awesome_ie7.css" rel="stylesheet" /><![endif]-->
    <asp:Literal ID="ltlFavicon" runat="server"></asp:Literal>
    <!--[if IE]><meta http-equiv="content-type" content="text/html;charset=utf-8">
<![endif]-->
    <script language="javascript" src="inc/script.js"></script>
    <bairong:Code Type="layer" runat="server" />
</head>

<body>
    <div id="navigation">
        <div class="container-fluid">
            <div>
                <asp:Literal ID="ltlLogo" runat="server" />
                <a href="javascript:;" target="_self" class="toggle-nav" rel="tooltip" data-placement="bottom" title="显示/隐藏左侧菜单"><i class="icon-arrow-left"></i></a>
            </div>
            <ul class='main-nav'>

                <asp:Repeater ID="rptTopMenu" runat="server">
                    <ItemTemplate>
                        <asp:Literal ID="ltlMenuLi" runat="server" />

                        <a href='javascript:;' data-toggle='dropdown' class='dropdown-toggle' data-hover='dropdown' style='<%#Container.ItemIndex ==2 ?"padding-top: 0;": ""%>'>
                            <asp:Literal ID='ltlMenuName' runat='server' />
                            <%#Container.ItemIndex <=1 ?
                            "<span class='caret'></span>"
                                :string.Empty %>
                        </a>

                        <!--<ul class="dropdown-menu">-->
                        <asp:Literal ID="ltlMenues" runat="server" />
                        <!--</ul>-->
                        </li>
      	
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Repeater ID="rptTopMenuExternal" runat="server">
                    <ItemTemplate>
                        <asp:Literal ID="ltlMenu" runat="server" />
                    </ItemTemplate>
                </asp:Repeater>

            </ul>

            <div class="user">

                <div class="dropdown">
                    <a href="#" class="dropdown-toggle user-name" data-toggle="dropdown">
                        <img src="images/avatar.jpg">
                        <span class="name">
                            <asp:Literal ID="ltlUserName" runat="server" /></span>
                        <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu pull-right">
                        <li><a href="cms/background_right.aspx"><i class="icon-dashboard"></i>系统面板</a></li>
                        <li><a href="platform/framework_userProfile.aspx"><i class="icon-user"></i>修改资料</a></li>
                        <li><a href="platform/framework_userPassword.aspx"><i class="icon-key"></i>更改密码</a></li>
                        <li><a href="platform/framework_userTheme.aspx"><i class="icon-adjust"></i>界面风格</a></li>
                    </ul>
                </div>

                <ul class="icon-nav">

                    <asp:Literal ID="ltlTopApps" runat="server" />

                    <li>
                        <form action="logout.aspx" method="post" target="_self">
                            <input type="hidden" value="<%=t%>" name="t" />
                            <a href="javascript:;" title="退出系统" onclick="$(this).parent().submit();"><i class="icon-signout"></i></a>
                            <style>
                                .icon-nav li > form {
                                    display: inline;
                                }                        
                                .icon-nav li > form > a {
                                    padding:11px 10px 9px 10px;display:block;color:#fff;position:relative;
                                }
                                .icon-nav li > form > a:hover {
                                    background:#1b67af;text-decoration:none;
                                }
                            </style>
                        </form>
                    </li>

                </ul>

            </div>
        </div>
    </div>
    <div class="container-fluid" id="content">
        <div class="right">
            <div class="main">
                <asp:Literal ID="ltlRight" runat="server" />
            </div>
        </div>

        <div id="left">

            <form runat="server">
                <table class="table table-condensed table-hover left-table">
                    <site:NodeNaviTree ID="leftMenuSite" runat="server" />
                    <bairong:NavigationTree ID="leftMenuSystem" runat="server" />
                    <tr>
                        <td></td>
                    </tr>
                </table>
            </form>

        </div>

    </div>
</body>
</html>

<script type="text/javascript">
    $(function () {
        $('#right').height($('#right').height() - 40);
    });
</script>
