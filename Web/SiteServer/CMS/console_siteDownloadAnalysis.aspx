<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.ConsoleSiteDownloadAnalysis" EnableViewState="false" %>

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
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />

        <div class="popover popover-static">
            <h3 class="popover-title">应用文件下载统计</h3>
            <div class="popover-content">

                <div style="width: 100%">
                    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
                    <div id="hits" style="height: 400px; width: 90%; display: inline-block"></div>
                    <!-- ECharts单文件引入 -->
                    <script src="/sitefiles/bairong/jquery/echarts/build/dist/echarts.js"></script>
                    <script type="text/javascript">
                        // 路径配置
                        require.config({
                            paths: {
                                echarts: '/sitefiles/bairong/jquery/echarts/build/dist'
                            }
                        });
                        // 新增信息数目
                        require(
                            [
                                'echarts',
                                'echarts/chart/bar' // 使用柱状图就加载bar模块，按需加载
                            ],
                            function (ec) {
                                // 基于准备好的dom，初始化echarts图表
                                var newChart = ec.init(document.getElementById('hits'));
                                //x array
                                var xArrayDownload = [];

                                //y array
                                var yArrayDownload = [];

                                //title
                                var newTitle = "文件下载量";

                                <%foreach (int key in this.keyArrayList)
                                  {%>
                                <%string yValueDownload = GetYHashtable(key);%>

                                <%if (yValueDownload != "0")
                                  {%>
                                xArrayDownload.push('<%=GetXHashtable(key)%>');
                                yArrayDownload.push('<%=yValueDownload%>');
                                <%}%>
                                <%}%>


                                if (xArrayDownload.length == 0) {
                                    xArrayDownload = ["暂无数据"];
                                    yArrayDownload = [0];
                                }

                                var option = {
                                    tooltip: {
                                        show: true
                                    },
                                    legend: {
                                        data: ['下载量']
                                    },
                                    xAxis: [
                                        {
                                            type: 'category',
                                            data: xArrayDownload
                                        }
                                    ],
                                    yAxis: [
                                        {
                                            type: 'value'
                                        }
                                    ],
                                    series: [
                                        {
                                            "name": "值",
                                            "type": "bar",
                                            "data": yArrayDownload
                                        }
                                    ]
                                };
                                // 文件下载量
                                option.xAxis[0].data = xArrayDownload;
                                option.series[0].data = yArrayDownload;
                                option.series[0].name = "文件下载量";
                                option.legend.data = [newTitle];
                                newChart.setOption(option);
                            }
                    );
                    </script>
                </div>


                <table class="table table-bordered table-hover">
                    <tr class="info thead">
                        <td>应用名称</td>
                        <td>文件下载量</td>
                    </tr>

                    <asp:Repeater runat="server" ID="rpContents">
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Literal ID="ltlPublishmentSystemName" runat="server"></asp:Literal></td>
                                <td style="text-align: center">
                                    <asp:Literal ID="ltlDownloadNum" runat="server"></asp:Literal></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td style="text-align: left">
                            <%=GetXHashtable(SiteServer.CMS.BackgroundPages.ConsoleSiteAnalysis1.YType_Other) %>
                        </td>
                        <td style="text-align: center">
                            <%=GetYHashtable(SiteServer.CMS.BackgroundPages.ConsoleSiteAnalysis1.YType_Other)%></td>
                    </tr>
                    <tr>
                        <td colspan="5" style="text-align: right; padding-right: 30px;">
                            <asp:LinkButton ID="lbMore" runat="server" Style="font-family: 'Microsoft YaHei'; font-size: 14px;" Text="查看更多"></asp:LinkButton>
                        </td>
                    </tr>
                </table>

            </div>
        </div>

        <div class="popover popover-static">
            <h3 class="popover-title">总计</h3>
            <div class="popover-content">

                <table class="table table-bordered table-hover">
                    <tr class="info thead">
                        <td>所有应用</td>
                        <td>文件下载量</td>
                    </tr>
                    <tr>
                        <td class="center" style="width: 250px;">总计 </td>
                        <td class="center" style="width: 100px;"><%=GetVertical(YType_Download)%></td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
