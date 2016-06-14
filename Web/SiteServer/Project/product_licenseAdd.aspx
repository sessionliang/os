<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundLicenseAdd" %>

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
  <bairong:alerts runat="server" />

  <div class="popover popover-static">
    <h3 class="popover-title">制作许可证</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="170" align="left">许可产品：</td>
          <td align="left">
            <asp:DropDownList ID="ddlProductID" OnSelectedIndexChanged="ddlProductID_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
          </td>
        </tr>
        <tr>
          <td width="170" align="left">域名：</td>
          <td align="left">
            <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" style="width:250px;" id="Domain" runat="server"/>
          </td>
        </tr>
        <tr>
          <td width="170" align="left">Mac地址：</td>
          <td align="left">
            <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" style="width:250px;" id="MacAddress" runat="server"/>
          </td>
        </tr>
        <asp:PlaceHolder id="phProductType" runat="server">
          <tr>
            <td width="170" align="left">产品版本：</td>
            <td align="left">
              <asp:DropDownList ID="ddlProductType" OnSelectedIndexChanged="ddlProductType_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
            </td>
          </tr>
        </asp:PlaceHolder>
        <asp:PlaceHolder id="phMaxSiteNumber" runat="server">
          <tr>
            <td width="170" align="left">可管理网站数：</td>
            <td align="left">
              <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" style="width:250px;" id="tbMaxSiteNumber" runat="server"/>
              <asp:RequiredFieldValidator
                ControlToValidate="tbMaxSiteNumber"
                ErrorMessage="*"
                Display="Dynamic"
                runat="server"/>
            </td>
          </tr>
        </asp:PlaceHolder>
        <tr>
          <td width="170" align="left">站点名称：</td>
          <td align="left">
            <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" style="width:250px;" id="SiteName" runat="server"/>
            <asp:RequiredFieldValidator
              ControlToValidate="SiteName"
              ErrorMessage="*"
              Display="Dynamic"
              runat="server"/>
          </td>
        </tr>
        <tr>
          <td width="170" align="left">客户简称：</td>
          <td align="left">
            <asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" style="width:250px;" id="ClientName" runat="server"/>
          </td>
        </tr>
        <tr>
          <td width="170" align="left">关联客户：</td>
          <td align="left">
            <input id="accountID" name="accountID" value="<%=GetLicenseValue("AccountID")%>" type="hidden">
            <a id="accountName" href="javascript:;" onclick="<%=GetLicenseValue("SelectAccount")%>" class="btn btn-info"><%=GetLicenseValue("AccountName")%></a>
            <script language="javascript">
            function selectAccount(accountName, accountID){
                $('#accountID').val(accountID);
                $('#accountName').html(accountName);
            }
            </script>
          </td>
        </tr>
        <tr>
          <td width="170" align="left">关联订单：</td>
          <td align="left">
            <input id="orderID" name="orderID" value="<%=GetLicenseValue("OrderID")%>" type="hidden">
            <a id="orderName" href="javascript:;" onclick="<%=GetLicenseValue("SelectOrder")%>" class="btn btn-info"><%=GetLicenseValue("OrderName")%></a>
            <script language="javascript">
            function selectOrder(orderName, orderID){
                $('#orderID').val(orderID);
                $('#orderName').html(orderName);
            }
            </script>
          </td>
        </tr>
        <asp:PlaceHolder id="phExpireDate" runat="server">
          <tr>
            <td width="170" align="left">产品到期时间：</td>
            <td align="left">
              <bairong:DateTimeTextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" id="tbExpireDate" runat="server"/>
            </td>
          </tr>
        </asp:PlaceHolder>
        <tr>
          <td width="170" align="left">备注：</td>
          <td align="left">
            <asp:TextBox style="width:450px;" id="Summary" textMode="MultiLine" rows="5" runat="server"/>
          </td>
        </tr>
      </table>
  
      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn btn-primary" id="Submit" text="保 存" onclick="Submit_OnClick" runat="server"/>
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
