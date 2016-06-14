<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundAnalysisContentDownloads1" %>

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
            <div id="Download" style="height: 400px; width: 100%; display: inline-block"></div>
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
                        var newChart = ec.init(document.getElementById('Download'));
                        //x array
                        var xArrayDownload = [];
                        //y array
                        var yArrayDownload = [];
                        //title
                        var DownloadTitle = "文件下载量";

                        <%foreach (int key in this.keyArrayList)
                          {%>
                        <%string yValueDownload = GetYHashtable(key);

                          string xValue = GetXHashtable(key);
                          if (xValue.Length > 10) xValue = xValue.Substring(0, 10);
                          %>

                        xArrayDownload.push('<%=xValue%>');

                        yArrayDownload.push('<%=yValueDownload%>');

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
                                    "name": DownloadTitle,
                                    "type": "bar",
                                    "data": []
                                }
                            ]
                        };
                        //前10条
                        option.xAxis[0].data = xArrayDownload;
                        //点击量
                        option.series[0].data = yArrayDownload;

                        newChart.setOption(option);
                    }
                    );
            </script>
        </div>

        <table class="table table-bordered table-hover">
            <tr class="info thead">
                <td>内容标题(点击查看)</td>
                <td width="180">所属栏目</td>
                <td width="300">附件地址</td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="ltlItemTitle" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlChannel" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlFileUrl" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td colspan="3" style="text-align: right; padding-right: 30px;">
                    <asp:LinkButton ID="lbMore" runat="server" Style="font-family: 'Microsoft YaHei'; font-size: 14px;" Text="查看更多"></asp:LinkButton>
                </td>
            </tr>
        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" Visible="false" />

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
