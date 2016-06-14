<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.Installer" %>
<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta http-equiv="X-UA-Compatible" content="IE=7" />
<title>SiteServer 系列产品安装向导</title>
<link href="style/step.css" rel="stylesheet" type="text/css" />
<bairong:Code type="JQuery" runat="server" />
<script src="js/check_data.js"></script>
<link href="../../SiteFiles/bairong/jquery/showLoading/css/showLoading.css" rel="stylesheet" media="screen" /> 
<script type="text/javascript" src="../../SiteFiles/bairong/jquery/showLoading/js/jquery.showLoading.js"></script>
<script>
$(function() {
	$('.byellow').click(function(){
		$('#main').showLoading();
		return true;
	});
});
</script>
</head>
<body>
<div class="wrap">
  <DIV class="top">
    <DIV class="top-logo"> </DIV>
    <DIV class="top-link">
      <UL>
        <LI> <A href="http://www.siteserver.cn/" target="_blank">官方网站</A> </LI>
        <LI> <A href="http://bbs.siteserver.cn/" target="_blank">技术论坛</A> </LI>
        <LI> <A href="http://cms.siteserver.cn/" target="_blank">系统帮助</A> </LI>
      </UL>
    </DIV>
    <DIV class="top-version">
      <H2>
        <asp:Literal ID="ltlVersionInfo" runat="server"></asp:Literal>
        安装向导 </H2>
    </DIV>
  </DIV>
  <div id="main">
    <div class="box">
      <h2>安装进度</h2>
      <ul class="list_step">
        <asp:Literal ID="ltlStepTitle" runat="server"></asp:Literal>
      </ul>
    </div>
    <div class="box noline">
      <form id="installForm" runat="server">
        <div class="form_detail">
          <div class="error">
            <asp:Literal ID="ltlErrorMessage" runat="server"></asp:Literal>
          </div>
          <asp:PlaceHolder ID="phStep1" runat="server">
            <table cellpadding="0" cellspacing="0" width="660" border="0">
              <TBODY>
                <tr>
                  <td><H3>SiteServer 系列产品许可协议</H3></td>
                  <td nowrap align="right"><img src="../Pic/Installer/printerIcon.gif"> <a href="eula.html" target="new"> 可打印版本</a></td>
                <tr>
                  <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                  <td valign="top" class="center" colspan="2"><iframe style="border-color:#999999; border-width:1px;" scrolling="yes" src="eula.html" height="264" width="660"></iframe></td>
                </tr>
                <tr>
                  <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                  <td valign="top" align="right" colspan="2"><span class="im">我已经阅读并同意此协议</span>
                    <asp:Checkbox id="chkIAgree" runat="server" Checked="true" />
                    &nbsp;
                    <asp:button OnClick="btnStep1_Click" class="btn byellow" Text="继 续" runat="server"></asp:button></td>
                </tr>
              </tbody>
            </table>
          </asp:PlaceHolder>
          <asp:PlaceHolder ID="phStep2" runat="server">
            <DIV>
              <DIV class=pr-title>
                <H3>服务器信息</H3>
              </DIV>
              <TABLE class=twbox border=0 cellSpacing=0 cellPadding=0 align=center>
                <TBODY>
                  <TR>
                    <TH align=middle><STRONG>参数</STRONG></TH>
                    <TH><STRONG>值</STRONG></TH>
                  </TR>
                  <TR>
                    <TD>服务器域名</TD>
                    <TD><asp:Literal ID="ltlDomain" runat="server"></asp:Literal></TD>
                  </TR>
                  <TR>
                    <TD>SiteServer 版本</TD>
                    <TD><asp:Literal ID="ltlVersion" runat="server"></asp:Literal></TD>
                  </TR>
                  <TR>
                    <TD>.NET版本</TD>
                    <TD><asp:Literal ID="ltlNETVersion" runat="server"></asp:Literal></TD>
                  </TR>
                  <TR>
                    <TD>系统根目录</TD>
                    <TD><asp:Literal ID="ltlPhysicalApplicationPath" runat="server"></asp:Literal></TD>
                  </TR>
                </TBODY>
              </TABLE>
              <DIV class=pr-title>
                <H3>目录权限检测</H3>
              </DIV>
              <DIV style="PADDING-BOTTOM: 0px; LINE-HEIGHT: 33px; PADDING-LEFT: 8px; PADDING-RIGHT: 8px; HEIGHT: 23px; COLOR: #666; OVERFLOW: hidden; PADDING-TOP: 2px">系统要求必须满足下列所有的目录权限全部可读写的需求才能使用，如果没有相关权限请添加。 </DIV>
              <TABLE class=twbox border=0 cellSpacing=0 cellPadding=0 width=512 align=center>
                <TBODY>
                  <TR>
                    <TH width=300 align=middle><STRONG>目录名</STRONG></TH>
                    <TH width=212><strong>读写权限</strong></TH>
                  </TR>
                  <TR>
                    <TD>/*</TD>
                    <TD><asp:Literal ID="ltlRootWrite" runat="server"></asp:Literal></TD>
                  </TR>
                  <TR>
                    <TD>/SiteFiles/*</TD>
                    <TD><asp:Literal ID="ltlSiteFielsWrite" runat="server"></asp:Literal></TD>
                  </TR>
                </TBODY>
              </TABLE>
              
              <P style="text-align:right; padding-right:50px;">
                <asp:button OnClick="btnPrevious_Click" class="btn bnormal" Text="后 退" runat="server"></asp:button>
                <asp:button ID="btnStep2" OnClick="btnStep2_Click" class="btn byellow" Text="下一步" runat="server"></asp:button>
              </P>
            </DIV>
          </asp:PlaceHolder>
          <asp:PlaceHolder ID="phStep3" runat="server">
            <DIV class=pr-title>
              <H3>选择数据库类型</H3>
            </DIV>
            <p>
              <label>数据库类型：</label>
              <asp:DropDownList ID="ddlDatabaseType" OnSelectedIndexChanged="ddlDatabaseType_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
              <span id="server_msg" class="error" style="display:none"></span> <span class="info">选择支持 SiteServer 系统的数据库类型</span> </p>
            <asp:PlaceHolder ID="phSqlServer" runat="server">
              <DIV class=pr-title>
                <H3>SQL SERVER 数据库设置</H3>
              </DIV>
              <DIV style="PADDING-BOTTOM: 10px; LINE-HEIGHT: 33px; PADDING-LEFT: 8px; PADDING-RIGHT: 8px; HEIGHT: 23px; COLOR: #666; OVERFLOW: hidden; PADDING-TOP: 2px">在此设置 SQL SERVER 数据库的连接字符串。 </DIV>
              <asp:PlaceHolder ID="phSqlServer1" runat="server">
                <p>
                  <label>数据库主机：</label>
                  <asp:TextBox style="width:285px" class="ipt_tx" ID="tbSqlServer" onblur="checkData(this, 'sqlserver_msg', '数据库主机');" Text="(local)" runat="server"/>
                  <span id="sqlserver_msg" class="error" style="display:none"></span> <span class="info">IP地址或者服务器名，本机为"(local)"</span> </p>
                <p>
                  <label>数据库用户：</label>
                  <asp:TextBox style="width:285px" class="ipt_tx" id="tbSqlUserName" onblur="checkData(this, 'sqlusername_msg', '数据库用户');" runat="server"/>
                  <span id="sqlusername_msg" class="error" style="display:none"></span> <span class="info">连接数据库的用户名</span> </p>
                <p>
                  <label>数据库密码：</label>
                  <asp:TextBox style="width:285px" TextMode="Password" class="ipt_tx" id="tbSqlPassword" onblur="checkData(this, 'sqlpassword_msg', '数据库密码');" runat="server"/>
                  <input type="hidden" runat="server" id="hihSqlHiddenPassword" />
                  <span id="sqlpassword_msg" class="error" style="display:none"></span> <span class="info">连接数据库的密码</span> </p>
              </asp:PlaceHolder>
              <asp:PlaceHolder ID="phSqlServer2" runat="server" Visible="false">
                <p>
                  <label>选择数据库：</label>
                  <asp:DropDownList ID="ddlSqlDatabaseName" runat="server"></asp:DropDownList>
                  <span class="info">选择安装的数据库</span> </p>
              </asp:PlaceHolder>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phOracle" runat="server">
              <DIV class=pr-title>
                <H3>ORACLE 数据库设置</H3>
              </DIV>
              <DIV style="PADDING-BOTTOM: 10px; LINE-HEIGHT: 33px; PADDING-LEFT: 8px; PADDING-RIGHT: 8px; HEIGHT: 23px; COLOR: #666; OVERFLOW: hidden; PADDING-TOP: 2px">在此设置 ORACLE 数据库的连接字符串。 </DIV>
              <p>
                <label>主机名称/IP地址：</label>
                <asp:TextBox style="width:285px" class="ipt_tx" ID="tbOraHost" onblur="checkData(this, 'oraHost_msg', '主机名称/IP地址');" runat="server"/>
                <span id="oraHost_msg" class="error" style="display:none"></span> <span class="info">ORACLE 数据库主机名/IP地址</span> </p>
              <p>
                <label>端口号：</label>
                <asp:TextBox style="width:85px" class="ipt_tx" ID="tbOraPort" Text="1521" onblur="checkData(this, 'oraPort_msg', '端口号');" runat="server"/>
                <span id="oraPort_msg" class="error" style="display:none"></span> <span class="info">ORACLE 数据库端口号</span> </p>
              <p>
                <label>数据库名称：</label>
                <asp:TextBox style="width:285px" class="ipt_tx" ID="tbOraServiceName" onblur="checkData(this, 'oraServiceName_msg', '数据库名称');" runat="server"/>
                <span id="oraServiceName_msg" class="error" style="display:none"></span> <span class="info">ORACLE 数据库名称</span> </p>
              <p>
                <label>数据库用户：</label>
                <asp:TextBox style="width:285px" class="ipt_tx" id="tbOraUserName" onblur="checkData(this, 'oraUserName_msg', '数据库用户');" runat="server"/>
                <span id="oraUserName_msg" class="error" style="display:none"></span> <span class="info">连接数据库的用户名</span> </p>
              <p>
                <label>数据库密码：</label>
                <asp:TextBox style="width:285px" TextMode="Password" class="ipt_tx" id="tbOraPassword" onblur="checkData(this, 'oraPassword_msg', '数据库密码');" runat="server"/>
                <input type="hidden" runat="server" id="hihOraHiddenPassword" />
                <span id="oraPassword_msg" class="error" style="display:none"></span> <span class="info">连接数据库的密码</span> </p>
            </asp:PlaceHolder>
            <P style="text-align:right; padding-right:50px;">
              <asp:button OnClick="btnPrevious_Click" class="btn bnormal" Text="后 退" runat="server"></asp:button>
              <asp:button OnClick="btnStep3_Click" class="btn byellow" Text="下一步" runat="server"></asp:button>
            </P>
          </asp:PlaceHolder>
          <asp:PlaceHolder ID="phStep4" runat="server">
              <DIV class=pr-title>
                <H3>管理员初始密码</H3>
              </DIV>
              <DIV style="PADDING-BOTTOM: 10px; LINE-HEIGHT: 33px; PADDING-LEFT: 8px; PADDING-RIGHT: 8px; HEIGHT: 23px; COLOR: #666; OVERFLOW: hidden; PADDING-TOP: 2px">在此设置总管理员的登录用户名与密码。</DIV>
              <p>
                <label>管理员用户名：</label>
                <asp:TextBox style="width:285px" class="ipt_tx" ID="tbAdminName" onblur="checkAdminName();" runat="server"/>
                <span id="adminname_msg" class="error" style="display:none"></span> <span class="info">登录后台使用的用户名</span> </p>
              <p>
                <label>管理员密码：</label>
                <asp:TextBox style="width:285px" TextMode="Password" class="ipt_tx" id="tbAdminPassword" onblur="checkPassword();" runat="server"/>
                <span id="password_msg" class="error" style="display:none"></span> <span class="rank_info">密码强度：
                <input type="text" id="passwordLevel" class="rank r0" disabled="disabled" />
                </span> </p>
              <p>
                <label>确认密码：</label>
                <asp:TextBox style="width:285px" TextMode="Password" class="ipt_tx" id="tbComfirmAdminPassword" onblur="checkConfirmPassword();" runat="server"/>
                <span id="confirmPassword_msg" class="error" style="display:none"></span> <span class="info">6-16个字符，支持大小写字母、数字和符号</span> </p>
            <P style="text-align:right; padding-right:50px;">
              <asp:button OnClick="btnPrevious_Click" class="btn bnormal" Text="后 退" runat="server"></asp:button>
              <asp:button OnClick="btnStep4_Click" class="btn byellow" Text="下一步" runat="server"></asp:button>
            </P>
          </asp:PlaceHolder>
          <asp:PlaceHolder ID="phStep5" runat="server">
            <p class="success" style="background-repeat:no-repeat; padding:15px; padding-left:50px;margin-right:100px;"> 恭喜，您已经完成了 SiteServer 系列产品的安装，并已正常运行，<A href='<%=GetSiteServerUrl()%>'>进入后台</A>。 </p>
          </asp:PlaceHolder>
        </div>
      </form>
    </div>
  </div>
</div>
<div id="ft">
  <p> 北京百容千域软件技术开发有限公司 版权所有 </p>
</div>
</div>
</body>
</html>
