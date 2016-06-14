<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundUrlAdd" %>
<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>


<HTML>
<HEAD>
<link rel="stylesheet" href="../../../../../SiteServer/style.css" type="text/css" />
<bairong:custom type="style" runat="server"></bairong:custom>

<meta http-equiv="content-type" content="text/html;charset=utf-8">
<bairong:Code type="Prototype" runat="server" />
<bairong:Code type="Scriptaculous" runat="server" />
</HEAD>
<body>
<bairong:custom type="showpopwin" runat="server" />
<form id="myForm" runat="server">
<bairong:Message runat="server"></bairong:Message>
<div class="column">
<div class="columntitle">添加网站</div>
<table width="98%" align="center" cellpadding="3" cellspacing="3">
	<tr><td width="170" align="left">
	<bairong:help HelpText="使用的产品" Text="使用的产品：" runat="server" ></bairong:help>
			</td>
	  <td align="left">
      	<asp:DropDownList ID="ProductID" runat="server"></asp:DropDownList>
      </td>
	  </tr>
      <tr><td width="170" align="left">
	<bairong:help HelpText="域名" Text="域名：" runat="server" ></bairong:help>
			</td>
	  <td align="left">
      	<asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" style="width:250px;" id="Domain" runat="server"/>
      </td>
	  </tr>
      <asp:PlaceHolder ID="phOEM" Visible="false" runat="server">      </asp:PlaceHolder>
      <tr><td width="170" align="left">
        <bairong:help HelpText="站点名称" Text="站点名称：" runat="server" ></bairong:help>
        </td>
        <td align="left">
          <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" style="width:250px;" id="SiteName" runat="server"/>
          <asp:RequiredFieldValidator
            ControlToValidate="SiteName"
            ErrorMessage="*"
            Display="Dynamic"
            runat="server"/>
        </td>
      </tr>
      <tr><td width="170" align="left">
	<bairong:help HelpText="客户名称" Text="客户名称：" runat="server" ></bairong:help>
			</td>
	  <td align="left">
      	<asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" style="width:250px;" id="ClientName" runat="server"/>
      </td>
	  </tr>
	</table>
    <br>
    <table width="95%" align="center" cellspacing="0" cellpadding="0" >
<tr>
<td align="center">
	   <asp:Button class="button" id="Submit" text="保 存" onclick="Submit_OnClick" runat="server"/>
</td>
</tr>
</table>
</div>
<br />
</form>
</body>
</HTML>
