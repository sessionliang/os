<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.BackgroundTemplateImport" %>

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

  <style type="text/css">
  .carousel-control {font-size: 60px !important; line-height: 30px  !important; background-color: transparent; border: none;  }
  .carousel .left {left: 5px !important;}
  .carousel .right {right: 10px !important;}
  .hero-unit {padding: 30px; margin-bottom: 10px;}
  </style>

  <div class="popover popover-static">
    <h3 class="popover-title"><asp:Literal id="ltlPageTitle" runat="server" /></h3>
    <div class="popover-content">
    
      <blockquote>
        <p>选择模板</p>
        <small>欢迎使用模板选择向导，您可以选择使用选中的模板替换现有模板。</small>
      </blockquote>

        <asp:repeater id="rptContents" runat="server">
          <ItemTemplate>
            <div style="float:left; text-align:center; margin-right:10px;" class="hero-unit">
              
              <h5><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></h5>
              <asp:Literal ID="ltlImageUrl" runat="server"></asp:Literal>
              <asp:Literal ID="ltlSelect" runat="server"></asp:Literal>

            </div>
          </ItemTemplate>
        </asp:repeater>

        <div class="clearfix"></div>

    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->