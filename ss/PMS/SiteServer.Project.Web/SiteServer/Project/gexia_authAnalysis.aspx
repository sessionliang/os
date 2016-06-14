<%@ Page Language="C#" Inherits="SiteServer.GeXia.BackgroundPages.BackgroundAuthAnalysis" %>

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

  <script type="text/javascript">
$(function () {
  $('#container').highcharts({
      chart: {
          type: 'spline'
      },
      title: {
          text: '阁下用户注册量'
      },
      yAxis: {
          title: {
              text: '注册人数'
          },
          min: 0,
          minorGridLineWidth: 0,
          gridLineWidth: 1,
          alternateGridColor: '#c0c0c0'
      },
      tooltip: {
          valueSuffix: ' 个'
      },
      plotOptions: {
          spline: {
              lineWidth: 4,
              states: {
                  hover: {
                      lineWidth: 5
                  }
              },
              marker: {
                  enabled: false
              },
              // pointInterval: 3600000, // one hour
              // pointStart: Date.UTC(2009, 9, 6, 0, 0, 0)
          }
      },
      series: [{
          name: '总注册量',
          data: [<asp:Literal id="ltlTotalCount" runat="server" />]
      },{
          name: '阁下注册量',
          data: [<asp:Literal id="ltlGeXiaCount" runat="server" />]
      },{
          name: '微信云注册量',
          data: [<asp:Literal id="ltlQCloudCount" runat="server" />]
      }]
      ,
      navigation: {
          menuItemStyle: {
              fontSize: '10px'
          }
      }
  });
});
    </script>
    <bairong:Code type="highcharts" runat="server" />

  <div class="popover popover-static">
    <h3 class="popover-title">阁下用户注册量</h3>
    <div class="popover-content">
    
      <div id="container" style="min-width: 400px; height: 400px; margin: 0 auto"></div>
  
    </div>
  </div>

  <ul class="breadcrumb breadcrumb-button">
    <a class="btn" href="?isAll=True">查询全部时间段</a>
  </ul>

</form>
</body>
</html>