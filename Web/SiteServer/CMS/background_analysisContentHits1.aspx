<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundAnalysisContentHits1" %>

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
        <bairong:Alerts runat="server" />
        <asp:Literal ID="ltlBreadCrumb" runat="server" />

        <div style="width: 100%">
            <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
            <div id="hits" style="height: 400px; width: 100%; display: inline-block"></div>
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
                        var xArrayHits = [];
                        //y array
                        var yArrayHits = [];
                        var yArrayHitsDay = [];
                        var yArrayHitsWeek = [];
                        var yArrayHitsMonth = [];
                        //title
                        var hitsTitle = "点击量";
                        var hitsTitleDay = "日点击量";
                        var hitsTitleWeek = "周点击量";
                        var hitsTitleMonth = "月点击量";

                        <%foreach (int key in this.keyArrayList)
                          {%>
                        <%string yValueHits = GetYHashtable(key, SiteServer.CMS.BackgroundPages.BackgroundAnalysisContentHits1.YType_Hits);
                          string yValueHitsDay = GetYHashtable(key, SiteServer.CMS.BackgroundPages.BackgroundAnalysisContentHits1.YType_HitsDay);
                          string yValueHitsWeek = GetYHashtable(key, SiteServer.CMS.BackgroundPages.BackgroundAnalysisContentHits1.YType_HitsWeek);
                          string yValueHitsMonth = GetYHashtable(key, SiteServer.CMS.BackgroundPages.BackgroundAnalysisContentHits1.YType_HitsMonth);
                          string xValue = GetXHashtable(key);
                          if (xValue.Length > 10) xValue = xValue.Substring(0, 10);
                          %>

                        xArrayHits.push('<%=xValue%>');

                        yArrayHits.push('<%=yValueHits%>');
                        yArrayHitsDay.push('<%=yValueHitsDay%>');
                        yArrayHitsWeek.push('<%=yValueHitsWeek%>');
                        yArrayHitsMonth.push('<%=yValueHitsMonth%>'); 

                            <%}%>

                        if (xArrayHits.length == 0) {
                            xArrayHits = ["暂无数据"];
                            yArrayHits = [0];
                            yArrayHitsDay = [0];
                            yArrayHitsWeek = [0];
                            yArrayHitsMonth = [0];
                        }

                        var option = {
                            tooltip: {
                                show: true
                            },
                            legend: {
                                data: []
                            },
                            xAxis: [
                                {
                                    type: 'category',
                                    data: []
                                }
                            ],
                            yAxis: [
                                {
                                    type: 'value'
                                }
                            ],
                            series: [
                                {
                                    "name": hitsTitle,
                                    "type": "bar",
                                    "data": []
                                },
                               {
                                   "name": hitsTitleDay,
                                   "type": "bar",
                                   "data": []
                               },
                              {
                                  "name": hitsTitleWeek,
                                  "type": "bar",
                                  "data": []
                              },
                              {
                                  "name": hitsTitleMonth,
                                  "type": "bar",
                                  "data": []
                              }
                            ]
                        };
                        //前10条
                        option.xAxis[0].data = xArrayHits;
                        //点击量
                        option.series[0].data = yArrayHits;
                        //日点击量
                        option.series[1].data = yArrayHitsDay;
                        //周点击量
                        option.series[2].data = yArrayHitsWeek;
                        //月点击量
                        option.series[3].data = yArrayHitsMonth;
                        newChart.setOption(option);
                    }
                    );
            </script>
        </div>

        <table class="table table-bordered table-hover">
            <tr class="info thead">
                <td>内容标题(点击查看)</td>
                <td>所属栏目</td>
                <td width="60" class="center">点击量</td>
                <td width="60" class="center">日点击量</td>
                <td width="60" class="center">周点击量</td>
                <td width="60" class="center">月点击量</td>
                <td width="120" class="center">最后点击时间</td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="ltlItemTitle" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="ltlChannel" runat="server"></asp:Literal></td>
                        <td class="center">
                            <asp:Literal ID="ltlHits" runat="server"></asp:Literal></td>
                        <td class="center">
                            <asp:Literal ID="ltlHitsByDay" runat="server"></asp:Literal></td>
                        <td class="center">
                            <asp:Literal ID="ltlHitsByWeek" runat="server"></asp:Literal></td>
                        <td class="center">
                            <asp:Literal ID="ltlHitsByMonth" runat="server"></asp:Literal></td>
                        <td class="center">
                            <asp:Literal ID="ltlLastHitsDate" runat="server"></asp:Literal></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td colspan="7" style="text-align: right; padding-right: 30px;">
                    <asp:LinkButton ID="lbMore" runat="server" Style="font-family: 'Microsoft YaHei'; font-size: 14px;" Text="查看更多"></asp:LinkButton>
                </td>
            </tr>
        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" Visible="false" />

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
