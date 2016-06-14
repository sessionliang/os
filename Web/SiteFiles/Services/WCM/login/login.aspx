<%@ Page language="c#" trace="false" enableViewState="false" Inherits="SiteServer.WCM.Services.Login" %>
<asp:PlaceHolder ID="phNoTemplateString" runat="server">
<TABLE cellPadding="4" cellspacing="0" width="<%=Width%>" border="0">
<style type="text/css">
.is_button {
	padding:0px 4px;
	height:20px;
	cursor:pointer;
	margin-top:2px;
	border:1px #93b9dc solid;
	background:url(<%=IconUrl%>/button_bg.gif) repeat;
	font-size:12px;
	padding-top:2px;
	color:#000000
}
.colorfocus {
	border: 1px #86a1ba double;
	background-color: #fff8e6;
}
.colorblur {
	border: 1px #86a1ba double;
	background-color: #ffffff;
}
</style>
    <TBODY>
    <asp:PlaceHolder ID="phAnonymousTemplate" runat="server">
      <TR>
        <TD id="errorMessage" style="COLOR: red;" align="center" colSpan="2"><asp:Literal ID="ltlErrorMessage" runat="server"></asp:Literal></TD>
      </TR>
      <TR>
        <TD width="60">用户名:</TD>
        <TD>
        <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
        </TD>
      </TR>
      <TR>
        <TD>密　码:</TD>
        <TD>
        <asp:Literal ID="ltlPassword" runat="server"></asp:Literal>
        </TD>
      </TR>
      <asp:PlaceHolder ID="phValidateCode" runat="server">
      <TR>
        <TD>验证码:</TD>
        <TD>
        <asp:Literal ID="ltlValidateCode" runat="server"></asp:Literal>
        </TD>
      </TR>
      </asp:PlaceHolder>
      <TR>
        <TD colspan="2" align="center">
          <input type="checkbox" id="isRemember" /><label for="isRemember">记住我的登录状态</label>&nbsp;&nbsp;<A href="<%=UserCenterUrl%>/forget.aspx" target="usercenter">忘记密码？</A>
        </TD>
      </TR>
      <TR>
        <TD colspan="2" align="center">
            <INPUT type="button" class="is_button" onclick="<%=LoginClickString%>" value="登 录" />&nbsp;&nbsp;
            <INPUT type="button" class="is_button" onclick="<%=RegisterClickString%>" value="注 册" />
        </TD>
      </TR>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phLoggedInTemplate" runat="server">
      <TR>
        <TD colspan="2" style="line-height:22px;">
            欢迎您，<asp:Literal ID="ltlDisplayName" runat="server"></asp:Literal><br />
            <A href="<%=UserCenterUrl%>" target="usercenter">用户中心</A>&nbsp;&nbsp;<A href="javascript:;" onclick="<%=LogoutClickString%>">退 出</A>
        </TD>
      </TR>
    </asp:PlaceHolder>
    </TBODY>
</TABLE>
</asp:PlaceHolder>
<asp:Literal ID="ltlTemplate" runat="server"></asp:Literal>
