<%@ Page Language="c#" Inherits="BaiRong.BackgroundPages.FrameworkForgetPassword" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>找回密码</title>
<bairong:Code type="JQuery" runat="server" />
<bairong:Code type="bootstrap" runat="server" />
<bairong:Code type="html5shiv" runat="server" />
<script language="JavaScript">
if (window.top != self){
	window.top.location = self.location;
}
$(document).ready(function () { $('#UserName').focus(); });
</script>
<link href="css/login.css" rel="stylesheet" type="text/css" />
</head>
<style type="text/css">
body {margin:20px auto auto 6px;text-align:center;padding:0;line-height:22px;}
img {border:0}
.tip {padding:4px 0 6px 46px;color:#999;}
#Logo {width:760px;margin:auto;text-align:left;height:50px;}
#Logo .lg {position:absolute;top:22px}
#Logo .nav {float:right;margin-right:5px;color:#1A82D2}
#Main {width:770px;margin:auto;text-align:center;}
#Bot {width:750px;clear:both;margin:auto;text-align:center;line-height:18px;border-top:1px solid #DADADA;padding-top:13px;margin-top:25px;}
#Bot a {color:#494949;}
#Banner {width:503px;height:170px;float:left;margin-top:30px;}
#Banner .conn_left{float:left;width:3px;height:170px;background:#208DE1}
#Banner .conn_img {margin-top:164px;}
#Banner .index_banner {float:left;background:#208DE1;width:216px;height:170px;}
#Banner .index_bg {float:left;background:url(pic/index_login_bg.gif) repeat-y;width:280px;height:145px!important; height /**/: 170px;padding:25px 0 0 4px;font:normal 12px tahoma;line-height:24px;text-align:left;color:#fff;}
#Banner .color {clear:both;width:503px;height:16px;background:#D4ECFF;border-top:2px solid #fff;}
#Banner ul {list-style:none;margin:12px 0 0 6px;}
#Banner ul li {text-align:left;height:48px;}
.txt {color:#1274BA;}
.txt_ {font-family:tahoma;}
.txt1 {color:#F86B2D;}
.txt2 {font-size:11px!important; font-size /**/:8pt;font-family:tahoma;}
.left {float:left;}
.right {float:right;}
#Login {width:255px;float:left;font-family:tahoma;color:#494949}
#Login .top {height:4px;background:url(pic/login_top_bg.gif) repeat-x;}
#Login .login_bg {height:280px;background:#F9F9F9;border-right:1px solid #8A8A8A;border-left:1px solid #8A8A8A;}
#Login .lg_title {text-align:left;height:23px;margin:0 11px 0px 11px;padding-top:3px;padding-left:4px;border-bottom:1px solid #B5B5B5;}
#Login .lg_title1 {text-align:left;margin:5px 11px;padding-top:3px;padding-left:4px;border-bottom:1px solid #B5B5B5;}
#Login .lg_title2 {text-align:left;margin:0 11px;padding-top:10px;padding-left:0;color:#ff0000;}
#Login .input_id {text-align:left;margin:0px 0 0 26px;}
#Login .input_pwd {text-align:left;margin:6px 0 0 26px;}
#Login .input_vc {text-align:left;margin:6px 0 0 14px;}
#Login .input_post {text-align:left;margin:8px 0 0 75px;}
#Login .input_fpwd {text-align:left;margin:5px 0 0 32px;}
#Login .bottom {height:4px;background:url(pic/login_bottom_bg.gif) repeat-x;}
#Login .intro_txt {color:#959595;text-align:left;margin-left:62px;}
#Login .txt3 {text-align:left;margin:10px 0 0 22px; clear:both}
.input_n {width:110px;height:15px !important; height /**/:20px;border-style:inset;padding:2px 0 0 2px;font:normal 12px tahoma;}
.input_s {width:62px;height:27px;padding-top:2px;font-weight:bold;border-style:outset;}
#Right {margin-top:30px;float:left;width:12px;height:170px;background:#A5D3F7;}
#Right .color {margin-top:170px;width:12px;height:16px;background:#D4ECFF;border-top:2px solid #fff;}
</style>
<body>
<div id="Logo">
	<div style="float:left"><div class="lg"><a href="<%=GetProductUrl()%>" target="_blank"><img src="pic/company/logo.gif" border="0" /></a></div></div>
	<div class="nav"><a href="<%=GetProductUrl()%>" target="_blank">产品网站</a>&nbsp;-&nbsp;<a href="http://help.siteserver.cn" target="_blank">系统帮助</a></div>
    <div style="clear:both"></div>
</div>
<div id="Main">
	<div id="Banner">
		<div class="conn_left"><img src="pic/index_conn_left.gif" width="3" height="3" /><img src="pic/index_conn_left_bottom.gif" width="3" height="3" class="conn_img" /></div>
		<div class="index_banner"><img src="pic/Company/login.gif"/></div>
		<div class="index_bg">
全静态发布及面向搜索引擎设计<br/>
可视化及拖拽式的模板制作<br/>
支持多站点，网站群管理<br />
精确化建设、协作化管理、流程化控制<br />
降低开发周期、降低总体成本、降低实施风险
	  </div>
		<div class="color"></div>
		<div style="margin-top:6px;text-align:left;">&nbsp;&nbsp;&nbsp;</div>
	</div>
	<div id="Login">
		<div class="top">
			<div class="left"><img src="pic/login_conn_left.gif" width="4" height="4" /></div>
			<div class="right"><img src="pic/login_conn_right.gif" width="4" height="4" /></div>
		</div>
		<div class="login_bg">
			<form class="form-inline" runat="server">
			<div class="lg_title"><b class="txt">找回密码</b></div>	
			<div class="lg_title2">&nbsp;</div>
			<asp:PlaceHolder ID="phUserNameTemplate" runat="server">
				<div class="input_id">用户名：
					<asp:TextBox id="UserName" class="input-medium" type="text" runat="server"></asp:TextBox>
					<asp:RequiredFieldValidator ControlToValidate="UserName" ErrorMessage=" *" foreColor="red"  Display="Static" runat="server" />
				</div>
				<div class="input_post"><asp:Button class="btn btn-primary" CommandName="userName" oncommand="Button_Command" text="下一步" runat="server"/>
				</div>			
			</asp:PlaceHolder>
            <asp:PlaceHolder ID="phQuestionTemplate" runat="server">
			  <div class="input_id">请回答问题：
					<asp:Literal ID="ltlQuestion" runat="server"></asp:Literal>
			  </div>
				<div class="input_id">答案：
					<asp:TextBox id="Answer" class="input-medium" type="text" runat="server"></asp:TextBox>
					<asp:RequiredFieldValidator ControlToValidate="Answer" ErrorMessage=" *" foreColor="red"  Display="Static" runat="server" />
				</div>
			  <div class="input_post"><asp:Button class="btn btn-primary" CommandName="question" oncommand="Button_Command" text="下一步" runat="server"/>
				</div>
              </asp:PlaceHolder>
			  <asp:PlaceHolder ID="phSuccessTemplate" runat="server">
				<div class="lg_title2"><span style="width:100%; text-align:center;">
					您好，<asp:Literal ID="ltlUserName" runat="server"></asp:Literal><br />
					您的密码是：<strong><asp:Literal ID="ltlPassword" runat="server"></asp:Literal></strong>，请牢记。
				</span></div>
			  </asp:PlaceHolder>
              <asp:PlaceHolder ID="phFailureTemplate" runat="server">
			  	<div class="lg_title2"><span style="width:100%; text-align:center; color:red">获取密码失败，<asp:Literal ID="ltlErrorMessage" runat="server"></asp:Literal></span></div>
              </asp:PlaceHolder>
			
			</form>
			<br />
			<div class="input_fpwd"><a href="login.aspx">返回登录</a></div>
			<div class="lg_title1"></div>
		  <div class="txt3">
				系统版本：<asp:Literal ID="Version" runat="server"></asp:Literal><br />
				.NET 版本：<asp:Literal ID="NetVersion" runat="server"></asp:Literal><br />
				数据库：<asp:Literal ID="Database" runat="server"></asp:Literal>
			</div>
		</div>
		<div class="bottom">
			<div class="left"><img src="pic/login_conn_left_b.gif" width="4" height="4"  /></div>
			<div class="right"><img src="pic/login_conn_right_b.gif" width="4" height="4" /></div>
		</div>
	</div>
	<div id="Right">
		<div class="right"><img src="pic/index_conn_right.gif" width="3" height="3" /></div>
		<div class="color"></div>
  </div>
  <div style="clear:both"></div>
</div>

<div id="Bot">
<A HREF="<%=GetProductUrl()%>" target="_blank" style="text-decoration:none"><span>Copyright &copy;<script>document.write(new Date().getFullYear());</script> <asp:Literal ID="CompanyName" runat="server"></asp:Literal>. All Rights Reserved</span></A></div>
</body>
</html>

