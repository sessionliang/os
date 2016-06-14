<%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.BackgroundAlbumContent" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
 <script type="text/javascript">
     $(function () {
         $('.itemImageUrl').hide();
     });
 </script>
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <link rel="stylesheet" type="text/css" href="css/keywordList.css">

  <!-- start -->
<div class="appmsg_list" id="appmsgList">

  <asp:Repeater ID="rptParentAlbumContents" runat="server">
  <itemtemplate>
   <div class="appmsg_col">
    <div class="inner">
      <div>
        <div class="appmsg multi">
          <div class="appmsg_content">
            <div class="appmsg_info">
              <code class="pull-right">
                <asp:Literal id="ltlkeywrods" runat="server" />
              </code>

              <em class="appmsg_date">
                <asp:Literal id="ltlAddDate" runat="server" />
              </em>
            </div>
            <div class="cover_appmsg_item">
              <h4 class="appmsg_title">
                <asp:Literal id="ltlTitle" runat="server" />
              </h4>
              <div class="appmsg_thumb_wrp">
                <asp:Literal id="ltlLargeImageUrl" runat="server" />
              </div>
            </div>
           
            <asp:DataList ID="dlAlbumContents" width="100%" runat="server" RepeatColumns="3">
              <itemtemplate>
                  <div style="margin:5px;">
                    <asp:TextBox id="tbContentImageUrl" class="itemImageUrl" runat="server"/>
                    <asp:Literal id="ltlImageUrl" runat="server" />
                 </div>
              </itemtemplate>
           </asp:DataList>
         
          </div>
            
          <div class="appmsg_opr">
            <ul>
              <li class="appmsg_opr_item with2">
                <asp:Literal id="ltlAddUrl" runat="server" />
              </li>
              <li class="appmsg_opr_item with2 no_extra">
                <asp:Literal id="ltlDeleteUrl" runat="server" />
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </div>
     
   </itemtemplate>
  </asp:Repeater>
   
</div>
  <!-- end -->

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="btnAddAlbumContent" Text="新建相册" runat="server" />
   
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->