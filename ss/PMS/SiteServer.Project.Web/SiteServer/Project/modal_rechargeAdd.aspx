<%@ Page Language="C#" validateRequest="false" Inherits="BRS.BackgroundPages.Modal.RechargeAdd" Trace="false"%>
<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>


<HTML>
	<HEAD>
<link rel="stylesheet" href="../../../../../SiteServer/style.css" type="text/css" />
<bairong:custom type="style" runat="server"></bairong:custom>
<title>�˺ų�ֵ</title>		
		
<meta http-equiv="content-type" content="text/html;charset=gb2312">
	</HEAD>
	<body>
		<form id="myForm" runat="server">
<bairong:Message runat="server"></bairong:Message>
		  <table cellpadding="2" cellspacing="2" width="98%" align="center">
            <tr>
				<td align="left">
					<nobr><bairong:help HelpText="�û���" Text="�û�����" runat="server" ></bairong:help></nobr>
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
					<nobr><bairong:help HelpText="��ֵ���" Text="��ֵ��" runat="server" ></bairong:help></nobr>
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
					<nobr><bairong:help HelpText="��ö�����" Text="��ö�������" runat="server" ></bairong:help></nobr>
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
					<nobr><bairong:help HelpText="֧����ʽ" Text="֧����ʽ��" runat="server" ></bairong:help></nobr>
				</td>
				<td align="left">
                    <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="35" MaxLength="50" id="PaymentType" runat="server"/>
				</td>
			</tr>
            <tr>
				<td align="left">
					<nobr><bairong:help HelpText="˵��" Text="˵����" runat="server" ></bairong:help></nobr>
				</td>
				<td align="left">
                    <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="35" Rows="4" MaxLength="50" id="Summary" TextMode="MultiLine" runat="server"/>
				</td>
			</tr>
			<tr>
				<td align="center" colspan="2">
					<asp:Button class="button" id="Ok" text="ȷ ��" OnClick="Ok_OnClick" runat="server" />&nbsp;&nbsp;
					<asp:Button class="button" id="Cancel" text="ȡ ��" runat="server" />
				</td>
			</tr>
	  </table>
		</form>
	</body>
</HTML>
