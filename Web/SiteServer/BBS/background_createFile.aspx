<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundCreateFile" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

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
  <bairong:alerts text="选择需要生成的文件页后点击“生成选定文件”即可生成对应的文件页面。" runat="server"></bairong:alerts>

  <script type="text/javascript" language="javascript">
  function selectAll(isChecked)
  {
    for(var i=0; i<document.getElementById('<%=FileCollectionToCreate.ClientID%>').options.length; i++)
    {
      document.getElementById('<%=FileCollectionToCreate.ClientID%>').options[i].selected = isChecked;
    }
  }
  </script>

  <div class="popover popover-static">
  <h3 class="popover-title">生成文件页</h3>
  <div class="popover-content">
    
    <table class="table noborder">
      <tr>
        <td width="160">
          生成文件页：
        </td>
        <td><asp:ListBox ID="FileCollectionToCreate" SelectionMode="Multiple" Rows="19" runat="server"></asp:ListBox>
        &nbsp;&nbsp;
        <label class="checkbox"><input id="CheckAll" type="checkbox" onClick="selectAll(this.checked);">全选</label>
        </td>
      </tr>
    </table>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <asp:Button class="btn btn-primary" id="CreateFileButton" text="生成选定文件" onclick="CreateFileButton_OnClick" runat="server" />
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->