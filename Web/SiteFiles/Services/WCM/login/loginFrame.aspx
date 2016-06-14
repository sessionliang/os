<%@ Page language="c#" trace="false" enableViewState="false" Inherits="SiteServer.WCM.Services.LoginFrame" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<link href="../resource/frame.css" rel="stylesheet" type="text/css" />

</head>
<body onload="document.getElementById('UserName').focus()">
<div class="main" id=login>
  <h4><span><a href="javascript:parent.stlCloseWindow();"> <img src="../resource/frame/login_close.gif" width="17" height="17" name="close" border="0" id="close"/></a></span><u>用户登录</u></h4>
  <div id="web_login" >
    <form runat="server" target="_self" style="margin:0px;">
      <ul>
        <li><span><u id="label_uin">帐 号：</u></span>
          <asp:TextBox type="text" class="inputstyle" id="UserName" tabindex="1" runat="server"></asp:TextBox>
        </li>
        <li><span><u id="label_pwd">密 码：</u></span>
          <asp:TextBox class="inputstyle" id="Password" TextMode="Password" maxlength="16" tabindex="2" runat="server"/>
            
          <label><a href="<%=GetForgetUrl()%>" target="_blank" tabindex="7">忘了密码？</a></label>
        </li>
        <asp:PlaceHolder ID="phValidateCode" runat="server">
        <li><span for="code"><u id="label_vcode">验证码：</u></span>
          <asp:TextBox tabindex="3" style="ime-mode: disabled;" autocomplete="off" maxlength=4 class="inputstyle" id="ValidateCode" runat="server"/>
        </li>
        <li id="verifytip"><span>&nbsp;</span> <u id="label_vcode_tip">输入下图中的字符，不区分大小写</u></li>
        <li><span for="pic">&nbsp;</span>
          <asp:Literal ID="ltlValidateCodeImage" runat="server"></asp:Literal>
        </li>
        </asp:PlaceHolder>
      </ul>
      <div class="login_button">
        <asp:Button class="btn" id="LoginSubmit" text="登 录" onclick="Submit_OnClick" runat="server"/>
        &nbsp;&nbsp;
        <input type="submit" tabindex="5" value="注 册" onclick="window.open('<%=GetRegisterUrl()%>');return false;" class="btn" />
      </div>
    </form>
  </div>
</div>
<script>
<asp:Literal ID="ltlError" runat="server"></asp:Literal>
</script>
</body>
</html>
