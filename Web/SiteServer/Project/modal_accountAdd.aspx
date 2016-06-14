<%@ Page Language="C#" validateRequest="false" Inherits="BRS.BackgroundPages.Modal.AccountAdd" Trace="false"%>
<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>


<HTML>
	<HEAD>
<link rel="stylesheet" href="../../../../../SiteServer/style.css" type="text/css" />
<bairong:custom type="style" runat="server"></bairong:custom>
<title><asp:Literal ID="MyTitle" runat="server"></asp:Literal></title>		
		
<meta http-equiv="content-type" content="text/html;charset=gb2312">
	</HEAD>
	<body>
		<form id="myForm" runat="server">
<bairong:Message runat="server"></bairong:Message>
		  <table cellpadding="2" width="95%" align="center">
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
					<nobr><bairong:help HelpText="密码" Text="密码：" runat="server" ></bairong:help></nobr>
				</td>
				<td align="left">
                     <asp:TextBox class="colorblur" TextMode="Password" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="35" MaxLength="50" id="Password" runat="server"/>
                    <asp:RequiredFieldValidator
						ControlToValidate="Password"
						ErrorMessage="*"
						Display="Dynamic"
						runat="server"/>
				</td>
			</tr>
            <tr>
				<td align="left">
					<nobr><bairong:help HelpText="MD5加密码" Text="MD5加密码：" runat="server" ></bairong:help></nobr>
				</td>
				<td align="left">
                    <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="35" MaxLength="50" id="MD5String" runat="server"/>
                    <asp:RequiredFieldValidator
						ControlToValidate="MD5String"
						ErrorMessage="*"
						Display="Dynamic"
						runat="server"/>
				</td>
			</tr>
            <tr>
				<td align="left">
					<nobr><bairong:help HelpText="真实姓名" Text="真实姓名：" runat="server" ></bairong:help></nobr>
				</td>
				<td align="left">
                    <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="35" MaxLength="50" id="RealName" runat="server"/>
				</td>
			</tr>
            <tr>
				<td align="left">
					<nobr><bairong:help HelpText="公司名称" Text="公司名称：" runat="server" ></bairong:help></nobr>
				</td>
				<td align="left">
                    <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="35" MaxLength="50" id="Company" runat="server"/>
				</td>
			</tr>
            <tr>
				<td align="left">
					<nobr><bairong:help HelpText="手机号码" Text="手机号码：" runat="server" ></bairong:help></nobr>
				</td>
				<td align="left">
                    <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="35" MaxLength="50" id="Mobile" runat="server"/>
				</td>
			</tr>
            <tr>
				<td align="left">
					<nobr><bairong:help HelpText="邮箱" Text="邮箱：" runat="server" ></bairong:help></nobr>
				</td>
				<td align="left">
                    <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="35" MaxLength="50" id="Email" runat="server"/>
				</td>
			</tr>
            <tr>
				<td align="left">
					<nobr><bairong:help HelpText="QQ号码" Text="QQ号码：" runat="server" ></bairong:help></nobr>
				</td>
				<td align="left">
                    <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="35" MaxLength="50" id="QQ" runat="server"/>
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
