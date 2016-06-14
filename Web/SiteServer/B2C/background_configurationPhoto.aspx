<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundConfigurationPhoto" %>

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
  <bairong:alerts text="在此设置商品详细页预览/放大图片的保存尺寸" runat="server"></bairong:alerts>

  <div class="popover popover-static">
    <h3 class="popover-title">商品图片尺寸</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="150">商品图片（小尺寸）：</td>
          <td> 宽：
            <asp:TextBox  Columns="25" MaxLength="50" Width="50" id="PhotoSmallWidth" runat="server"/>
            <asp:RequiredFieldValidator
                      ControlToValidate="PhotoSmallWidth"
                      errorMessage=" *" foreColor="red" 
                      Display="Dynamic"
                      runat="server"/>
            (px)&nbsp;
            高：
            <asp:TextBox  Columns="25" MaxLength="50" Width="50" id="PhotoSmallHeight" runat="server"/>
            <asp:RequiredFieldValidator
                      ControlToValidate="PhotoSmallHeight"
                      errorMessage=" *" foreColor="red" 
                      Display="Dynamic"
                      runat="server"/>
          (px) </td>
        </tr>
        <tr>
          <td>商品图片（中尺寸）：</td>
          <td> 宽：
            <asp:TextBox  Columns="25" MaxLength="50" Width="50" id="PhotoMiddleWidth" runat="server"/>
            <asp:RequiredFieldValidator
                      ControlToValidate="PhotoMiddleWidth"
                      errorMessage=" *" foreColor="red" 
                      Display="Dynamic"
                      runat="server"/>
            (px)&nbsp;
            高：
            <asp:TextBox  Columns="25" MaxLength="50" Width="50" id="PhotoMiddleHeight" runat="server"/>
            <asp:RequiredFieldValidator
                      ControlToValidate="PhotoMiddleHeight"
                      errorMessage=" *" foreColor="red" 
                      Display="Dynamic"
                      runat="server"/>
            (px) </td>
        </tr>
      </table>
  
      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server" />
          </td>
        </tr>
      </table>

    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6.4 html permissions -->