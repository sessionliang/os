<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundForm" %>

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
  <bairong:alerts runat="server"></bairong:alerts>
  
  <script type="text/javascript">
  $(document).ready(function()
  {
    loopRows(document.getElementById('contents'), function(cur){ cur.onclick = chkSelect; });
    $(".popover-hover").popover({trigger:'hover',html:true});
  });
  </script>

  <div class="well well-small">
    <asp:HyperLink ID="hlAddGroup" NavigateUrl="javascript:;" runat="server" Text="添加组"></asp:HyperLink>
        &nbsp;|&nbsp;
    <asp:HyperLink ID="hlAddElement" NavigateUrl="javascript:;" runat="server" Text="添加表单元素"></asp:HyperLink>
        &nbsp;|&nbsp;
    <asp:HyperLink ID="hlSetting" NavigateUrl="javascript:;" runat="server" Text="设 置"></asp:HyperLink>
  </div>

  <asp:dataGrid id="dgContents" runat="server" showHeader="true"
      ShowFooter="false"
      AutoGenerateColumns="false"
      HeaderStyle-CssClass="info thead"
      CssClass="table table-bordered table-hover"
      gridlines="none">
    <Columns>
      <asp:TemplateColumn HeaderText="元素">
        <ItemTemplate>
          <asp:Literal ID="ltlAttributeName" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="名称">
        <ItemTemplate>
          <asp:Literal ID="ltlDisplayName" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="提交类型">
        <ItemTemplate>
          <asp:Literal ID="ltlInputType" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="100" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="是否显示">
        <ItemTemplate>
          <asp:Literal ID="ltlIsVisible" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="70" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="需要验证">
        <ItemTemplate>
          <asp:Literal ID="ltlIsValidate" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="70" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="上升">
        <ItemTemplate>
          <asp:HyperLink ID="UpLinkButton" runat="server"><img src="../Pic/icon/up.gif" border="0" alt="上升" /></asp:HyperLink>
        </ItemTemplate>
        <ItemStyle Width="40" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="下降">
        <ItemTemplate>
          <asp:HyperLink ID="DownLinkButton" runat="server"><img src="../Pic/icon/down.gif" border="0" alt="下降" /></asp:HyperLink>
        </ItemTemplate>
        <ItemStyle Width="40" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="表单验证">
        <ItemTemplate>
          <asp:Literal ID="ltlEditValidate" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="80" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="ltlEditStyle" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="60" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="ltlDeleteStyle" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="60" cssClass="center" />
      </asp:TemplateColumn>
    </Columns>
  </asp:dataGrid>

  <hr />


    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
  <table id="contents" class="table table-bordered table-hover">
        <tr class="info thead">
          <td>组<asp:Literal ID="ltlIndex" runat="server"></asp:Literal></td>
          <td>组图标 </td>
          <td width="30"></td>
          <td width="30"></td>
          <td width="50"></td>
          <td width="50"></td>
        </tr>

        <tr>
          <td class="center"><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlIconUrl" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlUpUrl" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlDownUrl" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal></td>
        </tr>
        <tr><td colspan="6">

          <asp:HyperLink ID="hlAdd" NavigateUrl="javascript:;" runat="server" Text="添加表单元素" class="btn btn-success pull-right"></asp:HyperLink>
          <br /><br />
          
        <asp:dataGrid id="dgGroupContents" runat="server" showHeader="true"
            ShowFooter="false"
            AutoGenerateColumns="false"
            HeaderStyle-CssClass="info thead"
            CssClass="table table-bordered table-hover"
            gridlines="none">
          <Columns>
            <asp:TemplateColumn HeaderText="元素">
              <ItemTemplate>
                <asp:Literal ID="ltlAttributeName" runat="server"></asp:Literal>
              </ItemTemplate>
              <ItemStyle cssClass="center" />
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="名称">
              <ItemTemplate>
                <asp:Literal ID="ltlDisplayName" runat="server"></asp:Literal>
              </ItemTemplate>
              <ItemStyle cssClass="center" />
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="提交类型">
              <ItemTemplate>
                <asp:Literal ID="ltlInputType" runat="server"></asp:Literal>
              </ItemTemplate>
              <ItemStyle Width="100" cssClass="center" />
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="是否显示">
              <ItemTemplate>
                <asp:Literal ID="ltlIsVisible" runat="server"></asp:Literal>
              </ItemTemplate>
              <ItemStyle Width="70" cssClass="center" />
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="需要验证">
              <ItemTemplate>
                <asp:Literal ID="ltlIsValidate" runat="server"></asp:Literal>
              </ItemTemplate>
              <ItemStyle Width="70" cssClass="center" />
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="上升">
              <ItemTemplate>
                <asp:HyperLink ID="UpLinkButton" runat="server"><img src="../Pic/icon/up.gif" border="0" alt="上升" /></asp:HyperLink>
              </ItemTemplate>
              <ItemStyle Width="40" cssClass="center" />
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="下降">
              <ItemTemplate>
                <asp:HyperLink ID="DownLinkButton" runat="server"><img src="../Pic/icon/down.gif" border="0" alt="下降" /></asp:HyperLink>
              </ItemTemplate>
              <ItemStyle Width="40" cssClass="center" />
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="表单验证">
              <ItemTemplate>
                <asp:Literal ID="ltlEditValidate" runat="server"></asp:Literal>
              </ItemTemplate>
              <ItemStyle Width="80" cssClass="center" />
            </asp:TemplateColumn>
            <asp:TemplateColumn>
              <ItemTemplate>
                <asp:Literal ID="ltlEditStyle" runat="server"></asp:Literal>
              </ItemTemplate>
              <ItemStyle Width="60" cssClass="center" />
            </asp:TemplateColumn>
            <asp:TemplateColumn>
              <ItemTemplate>
                <asp:Literal ID="ltlDeleteStyle" runat="server"></asp:Literal>
              </ItemTemplate>
              <ItemStyle Width="60" cssClass="center" />
            </asp:TemplateColumn>
          </Columns>
        </asp:dataGrid>

       

        </td></tr>
          </table>
           <hr />
      </itemtemplate>
    </asp:Repeater>


</form>
</body>
</html>