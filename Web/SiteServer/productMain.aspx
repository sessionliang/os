<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.FrameworkProductMain" Trace="False" enableViewState = "false"%>
<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

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
<title><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></title>
<!--[if IE 7]><link href="scripts/lib/font-awesome/css/font_awesome_ie7.css" rel="stylesheet" /><![endif]-->
<!--[if IE]><meta http-equiv="content-type" content="text/html;charset=utf-8">
<![endif]-->
<script language="javascript" src="inc/script.js"></script>
<bairong:Code type="layer" runat="server" />
</head>

<body>
  <div id="navigation">
    <div class="container-fluid">
      <div>
        <asp:Literal id="ltlLogo" runat="server" />
        <a href="javascript:;" target="_self" class="toggle-nav" rel="tooltip" data-placement="bottom" title="显示/隐藏左侧菜单"><i class="icon-arrow-left"></i></a>
      </div>
      <ul class='main-nav'>

      	 <asp:Repeater id="rptTopMenu" runat="server">
      		<itemtemplate>
			      <asp:Literal id="ltlMenuLi" runat="server" />
		            <a href="javascript:;" data-toggle="dropdown" class='dropdown-toggle' data-hover="dropdown">
		                <asp:Literal id="ltlMenuName" runat="server" />
		                <span class="caret"></span>
		            </a>

		            <ul class="dropdown-menu">
		                <asp:Literal id="ltlMenues" runat="server" />
		            </ul>
		        </li>
      		</itemtemplate>
      	 </asp:Repeater>

         <asp:Repeater id="rptTopMenuExternal" runat="server">
            <itemtemplate>
              <asp:Literal id="ltlMenu" runat="server" />
            </itemtemplate>
         </asp:Repeater>

      </ul>

      <div class="user">

        <div class="dropdown">
          <a href="#" class="dropdown-toggle user-name" data-toggle="dropdown">
            <img src="images/avatar.jpg">
            <span class="name"><asp:Literal id="ltlUserName" runat="server" /></span>
            <span class="caret"></span>
          </a>
          <ul class="dropdown-menu pull-right">
            <li><a href="cms/background_right.aspx"><i class="icon-dashboard"></i> 系统面板</a></li>
            <li><a href="platform/framework_userProfile.aspx"><i class="icon-user"></i> 修改资料</a></li>
            <li><a href="platform/framework_userPassword.aspx"><i class="icon-key"></i> 更改密码</a></li>
            <li><a href="platform/framework_userTheme.aspx"><i class="icon-adjust"></i> 界面风格</a></li>
          </ul>
        </div>

        <ul class="icon-nav">
          
          <asp:Literal id="ltlTopApps" runat="server" />

          <li>
            <a href="logout.aspx" target="_self" title="退出系统"><i class="icon-signout"></i></a>
          </li>

        </ul>      

      </div>
    </div>
  </div>
  <div class="container-fluid" id="content">
    <div class="right">
      <div class="main">
        <asp:Literal id="ltlRight" runat="server" />
      </div>
    </div>
    
    <div id="left">

      <form runat="server">
        <table class="table table-condensed table-hover left-table">
          <bairong:NavigationTree ID="leftMenuSystem" runat="server" />
          <tr><td></td></tr>
        </table>
      </form>

    </div>

  </div>
  <asp:Literal ID="ltlScript" runat="server"></asp:Literal>
</body>
</html>

<script type="text/javascript">
$(function(){
    $('#right').height($('#right').height() - 40);
});
</script>