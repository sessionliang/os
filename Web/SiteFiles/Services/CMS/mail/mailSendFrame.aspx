<%@ Page language="c#" trace="false" enableViewState="false" Inherits="SiteServer.CMS.Services.MailSendFrame" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<link href="../resource/frame.css" rel="stylesheet" type="text/css" />

</head>
<body onload="document.getElementById('Receiver').focus()">
<div class="main" id=login >
  <h4><span><a href="javascript:parent.stlCloseWindow();"> <img src="../resource/frame/login_close.gif" width="17" height="17" name="close" border="0" id="close"/></a></span><u>推荐给好友</u></h4>
  <div id="web_login" >
    <form runat="server" target="_self" style="margin:0px;">
      <ul>
        <li><span><u id="label_pwd">好友姓名：</u></span>
          <asp:TextBox class="inputstyle" id="Receiver" maxlength="16" tabindex="1" runat="server"/>
        </li>
        <li><span><u id="label_uin">好友邮箱地址：</u></span>
          <asp:TextBox type="text" class="inputstyle" id="Mail" tabindex="2" runat="server"></asp:TextBox>
        </li>
        <li id="verifytip"><span>&nbsp;</span> <u id="label_vcode_tip">若有多个邮件地址，请用(半角)分号隔开。 </u></li>
        <li><span><u id="label_pwd">您的姓名：</u></span>
          <asp:TextBox class="inputstyle" id="Sender" maxlength="16" tabindex="3" runat="server"/>
        </li>
        <asp:PlaceHolder ID="phValidateCode" runat="server">
        <li><span for="code"><u id="label_vcode">验证码：</u></span>
          <asp:TextBox tabindex="4" style="ime-mode: disabled;" autocomplete="off" maxlength=4 class="inputstyle" id="ValidateCode" runat="server"/>
        </li>
        <li id="verifytip"><span>&nbsp;</span> <u id="label_vcode_tip">输入下图中的字符，不区分大小写</u></li>
        <li><span for="pic">&nbsp;</span>
          <asp:Literal ID="ltlValidateCodeImage" runat="server"></asp:Literal>
        </li>
        </asp:PlaceHolder>
      </ul>
      <div class="login_button">
        <asp:Button class="btn" TabIndex="5" id="Submit" text="发 送" onclick="Submit_OnClick" runat="server"/>
        &nbsp;&nbsp;
        <input type="submit" tabindex="6" value="关 闭" onclick="parent.stlCloseWindow();return false;" class="btn" />
      </div>
    </form>
  </div>
</div>
<script>
<asp:Literal ID="ltlError" runat="server"></asp:Literal>
</script>
</body>
</html>
