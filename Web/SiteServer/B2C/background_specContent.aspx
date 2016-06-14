<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundSpecContent" %>

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

  <style>
  .specItem { padding-top:3px; padding-bottom:3px; }
  .specItem div { float:left; cursor:pointer; padding-top: 2px; padding-right: 8px; padding-bottom: 2px; padding-left: 6px; margin-right:5px; border-color: #999999; border-width: 1px; border-style: solid; background-color:#FFF; }
  .specItem a, .specItem a:visited, .specItem a:hover, .specItem a:active { text-decoration: none; }
  .specItem div.cur { padding-top: 1px; padding-right: 7px; padding-bottom: 1px; padding-left: 5px; margin-right:5px; border-color: #c60d0d; border-width: 2px; border-style: solid; background-color:#FFF; }
  .specItem div.cur { position:relative; }
  span.selected { width: 9px; height: 9px; text-indent: -99em; padding:0px; bottom:0px; margin:0px; border-width:0; overflow: hidden; position: absolute; background-image: url("../pic/iconSelected.gif"); background-repeat: no-repeat; }
  .photos img{width:22px; height:22px; margin:2px;}

  .specIcon { padding-top:3px; padding-bottom:3px; }
  .checked { background-color:#CCC }
  .specIcon div { float:left; padding-top: 2px; padding-right: 8px; padding-bottom: 2px; padding-left: 6px; margin-right:5px; border-color: #999999; border-width: 1px; border-style: solid; background-color:#FFF; }
  </style>
  <script>
  $(document).ready(function(){
    
    $("#selAll").click(function() {
      var flag = $(this).attr("checked");
      $("input:checkbox").each(function() {
        $(this).attr("checked", flag);
        if ($(this).is(':checked')){
          $(this).parent().parent().parent().addClass("success");
        }else{
          $(this).parent().parent().parent().removeClass("success");
        }
      })
    })
    
    $("input:checkbox").click(function(){
      if ($(this).is(':checked')){
        $(this).parent().parent().parent().addClass("success");
      }else{
        $(this).parent().parent().parent().removeClass("success");
      }
    });
  });
  </script>

  <div class="popover popover-static">
    <h3 class="popover-title">设置规格项</h3>
    <div class="popover-content">

      <asp:dataGrid id="dgSpec" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" gridlines="none" runat="server">
        <Columns>
        <asp:TemplateColumn HeaderText="规格名称">
          <ItemTemplate> &nbsp;
            <asp:Literal ID="ltlSpecName" runat="server"></asp:Literal>
          </ItemTemplate>
          <ItemStyle cssClass="center" Width="80" />
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="规格值">
          <ItemTemplate>
            <div class="specItem">
              <asp:Literal ID="ltlItem" runat="server"></asp:Literal>
            </div>
          </ItemTemplate>
          <ItemStyle HorizontalAlign="left" />
        </asp:TemplateColumn>
        <asp:TemplateColumn>
          <ItemTemplate>
            <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
          </ItemTemplate>
          <ItemStyle Width="80" cssClass="center" />
        </asp:TemplateColumn>
        </Columns>
      </asp:dataGrid>

    </div>
  </div>

  <div class="popover popover-static">
    <h3 class="popover-title">规格货品设置</h3>
    <div class="popover-content">

      <div class="alert alert-info">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <strong>提示!</strong>&nbsp;&nbsp; 库存不设置代表不限制，市场价以及销售价不设置代表与商品一致，只有上架商品才能在前台显示
      </div>

      <table id="goods" class="table table-bordered table-hover">
        <asp:Repeater ID="rptGoods" runat="server">
          <headertemplate>
            <tr class="info thead">
              <td width="180">货号</td>
              <asp:Literal ID="ltlSpec" runat="server"></asp:Literal>
              <asp:PlaceHolder id="phStock" runat="server">
                  <td width="60">库存</td>
              </asp:PlaceHolder>
              <td width="60">市场价</td>
              <td width="60">销售价</td>
              <td width="90">
                <label class="checkbox">
                  <input type="checkbox" onclick="selectRows(document.getElementById('goods'), this.checked);"> 全部上架
                </label>                
              </td>
            </tr>
          </headertemplate>
          <itemtemplate>
            <asp:Literal ID="ltlTr" runat="server"></asp:Literal>
              <td>
                  <asp:Literal ID="ltlSN" runat="server"></asp:Literal>
              </td>
                  <asp:Literal ID="ltlSpec" runat="server"></asp:Literal>
              <asp:PlaceHolder id="phStock" runat="server">
                  <td>
                      <asp:Literal ID="ltlStock" runat="server"></asp:Literal>
                  </td>
              </asp:PlaceHolder>
              <td>
                  <asp:Literal ID="ltlPriceMarket" runat="server"></asp:Literal>
              </td>
              <td>
                  <asp:Literal ID="ltlPriceSale" runat="server"></asp:Literal>
              </td>
              <td class="center">
                  <asp:Literal ID="ltlIsOnSale" runat="server"></asp:Literal>
              </td>
            </tr>
          </itemtemplate>
        </asp:Repeater>
      </table>

      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server"/>
            <asp:Button class="btn" id="Cancel" text="返 回" OnClick="Return_OnClick" runat="server" />
          </td>
        </tr>
      </table>

    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6.4 html permissions -->