<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundUrlCohort" %>

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
                type: 'area'
            },
            title: {
                text: '同期群分析'
            },
            
            xAxis: {
                categories: [<asp:Literal id="ltlCategories" runat="server" />]
            },
            yAxis: {
                title: {
                    text: null
                },
                labels: {
                    formatter: function() {
                        return this.value +'%';
                    }
                }
            },
            tooltip: {
                formatter: function() {
                    return this.series.name +': ' + this.y +' 个网站(' + Highcharts.numberFormat(this.percentage, 1) + '%) '+ this.x;
                }
            },
            plotOptions: {
                area: {
                    stacking: 'percent'
                }
            },
            series: [
              {
                    name: '仅使用一次',
                    data: [<asp:Literal id="ltlSeries1" runat="server" />]
                },
                {
                    name: '有3次使用',
                    data: [<asp:Literal id="ltlSeries2" runat="server" />]
                },
                {
                    name: '有7次使用',
                    data: [<asp:Literal id="ltlSeries3" runat="server" />]
                },
                {
                    name: '7次以上使用',
                    data: [<asp:Literal id="ltlSeries4" runat="server" />]
                },
                {
                    name: '已购买',
                    data: [<asp:Literal id="ltlSeries5" runat="server" />]
                },
            ]
        });
    });
    

    </script>
    <bairong:Code type="highcharts" runat="server" />

  <div class="popover popover-static">
    <h3 class="popover-title">同期群分析</h3>
    <div class="popover-content">
    
      <div id="container" style="min-width: 400px; height: 400px; margin: 0 auto"></div>
  
    </div>
  </div>

</form>
</body>
</html>