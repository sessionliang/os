<%@ Page Language="C#" validateRequest="false" Inherits="BRS.BackgroundPages.Modal.RechargeAdd" Trace="false"%>
<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>


<HTML>
	<HEAD>
<link rel="stylesheet" href="../../../../../SiteServer/style.css" type="text/css" />
<bairong:custom type="style" runat="server"></bairong:custom>
<title>账号充值</title>		
		
<meta http-equiv="content-type" content="text/html;charset=gb2312">
	</HEAD>
	<body>
		<form id="myForm" runat="server">
<bairong:Message runat="server"></bairong:Message>
		  <table cellpadding="2" cellspacing="2" width="98%" align="center">
            <tr>
				<td align="left">
					<nobr><bairong:help HelpText="用户名" Text="用户名：" runat="server" ></bairong:help></nobr>
				</td>
				<td align="left">
                    <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="35" MaxLength="50" id="UserName" runat="server"/>
                    <asp:RequiredFieldValidator
						ControlToValidate="UserName"
						ErrorMessage="*"
						Display="Dynamic"
						runat="server"/>
				</td>
			</tr>
            <tr>
				<td align="left">
					<nobr><bairong:help HelpText="充值金额" Text="充值金额：" runat="server" ></bairong:help></nobr>
				</td>
				<td align="left">
                     <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="35" MaxLength="50" id="Cost" runat="server"/>
                    <asp:RequiredFieldValidator
						ControlToValidate="Cost"
						ErrorMessage="*"
						Display="Dynamic"
						runat="server"/>
                    <asp:RegularExpressionValidator
					ControlToValidate="Cost"
					ValidationExpression="\d+"
					Display="Dynamic"
					ErrorMessage="*"
					runat="server"/>
				</td>
			</tr>
            <tr>
				<td align="left">
					<nobr><bairong:help HelpText="获得短信数" Text="获得短信数：" runat="server" ></bairong:help></nobr>
				</td>
				<td align="left">
                    <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="35" MaxLength="50" id="Messages" runat="server"/>
                    <asp:RequiredFieldValidator
						ControlToValidate="Messages"
						ErrorMessage="*"
						Display="Dynamic"
						runat="server"/>
                    <asp:RegularExpressionValidator
					ControlToValidate="Messages"
					ValidationExpression="\d+"
					Display="Dynamic"
					ErrorMessage="*"
					runat="server"/>
				</td>
			</tr>
            <tr>
				<td align="left">
					<nobr><bairong:help HelpText="支付方式" Text="支付方式：" runat="server" ></bairong:help></nobr>
				</td>
				<td align="left">
                    <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="35" MaxLength="50" id="PaymentType" runat="server"/>
				</td>
			</tr>
            <tr>
				<td align="left">
					<nobr><bairong:help HelpText="说明" Text="说明：" runat="server" ></bairong:help></nobr>
				</td>
				<td align="left">
                    <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="35" Rows="4" MaxLength="50" id="Summary" TextMode="MultiLine" runat="server"/>
				</td>
			</tr>
			<tr>
				<td align="center" colspan="2">
					<asp:Button class="button" id="Ok" text="确 定" OnClick="Ok_OnClick" runat="server" />&nbsp;&nbsp;
					<asp:Button class="button" id="Cancel" text="取 消" runat="server" />
				</td>
			</tr>
	  </table>
		</form>
	</body>
</HTML>
