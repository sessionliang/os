<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundShareSet" EnableEventValidation="false" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <style type="text/css" >
    table{background-color:#CFCFCF}
  table tr td{background-color:#ffffff}
  </style>
  <script language="javascript" type="text/javascript">
   $(function () {
       $("#selectStyle").click(function () {
           
           var posLeft = window.event.clientX - 100;
           var posTop = window.event.clientY-100;
           var uuid = "<%=uuid %>";
           window.open('http://www.bshare.cn/moreStylesEmbed?uuid=' + uuid + '', 'getCode', 'scrollbars=yes,width=780px,height=300px,top=' + posTop + ',left=' + posLeft);
       })
   })
  </script>

  <div class="popover popover-static">
    <h3 class="popover-title">分享插件设置</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td colspan="2"  >&nbsp;您已经开通bShare服务：</td>
        </tr>
        <tr>
          <td width="20%" align="right"   >&nbsp;<b style="color:#1274BA">用户名：</b></td>
          <td width="80%"   >&nbsp;&nbsp;<asp:Label ID="txtName" runat="server"></asp:Label></td>
        </tr>
        <tr>
          <td align="right"  >&nbsp;<b style="color:#1274BA">UUID：</b></td>
          <td  >&nbsp; <asp:Label ID="txtUuid" runat="server"></asp:Label></td>
        </tr>
        <tr>
          <td colspan="2" align="right"  >&nbsp;</td>
        </tr>
        <tr>
          <td colspan="2" bgcolor="#F9FCEF">前台调用样式/代码：</td>
        </tr>
        <tr>
          <td colspan="2"  >
         
          <p>内容页面中直接加入<span style="color:red">&lt;stl:action type=&quot;Share&quot;&gt;&lt;/stl:action&gt;
      </span><br />
            即可调用分享插件。</p>
            <p>
              <asp:TextBox id="bscode"  runat="server" TextMode="MultiLine" Columns="40" Rows="3" >        </asp:TextBox>
             
             <input  id="selectStyle" value="选择样式" type="button" />
            
             <asp:Button ID="btnSetStyle" runat="server" Text="保存样式" OnClick="btnSetStyle_Click" />
           
             </p>
            
            </td>
        </tr>
        <tr>
          <td colspan="2"  >样式预览：</td>
        </tr>
        <tr>
          <td colspan="2"  >

          <asp:Literal runat="server" ID="ltlScriptView"></asp:Literal>
                </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->