<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundAnalysisAdministratorImage" EnableViewState="false" %>

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

        <div class="well well-small">
            开始时间：
    <bairong:DateTimeTextBox ID="StartDate" class="input-small" Columns="30" runat="server" />
            结束时间：
    <bairong:DateTimeTextBox ID="EndDate" class="input-small" Columns="30" runat="server" />
            <asp:Button class="btn" ID="Analysis" Style="margin-bottom: 0px;" OnClick="Analysis_OnClick" Text="分 析" runat="server" />
            <%if (!string.IsNullOrEmpty(this.returnUrl))
              {%>
            <input type="button" onclick="window.location.href = '<%=this.returnUrl%>    ';" value="返 回" class="btn" />
            <%} %>
        </div>

        <div class="popover popover-static">
            <h3 class="popover-title">按栏目统计</h3>
            <div class="popover-content">

                <div style="width: 100%">
                    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
                    <div id="new" style="height: 400px; width: 90%; display: inline-block"></div>
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
                                var newChart = ec.init(document.getElementById('new'));
                                //x array
                                var xArrayNew = [];

                                //y array
                                var yArrayNew = [];
                                var yArrayUpdate = [];
                                var yArrayRemark = [];
                                //title
                                var newTitle = "新增信息数目";
                                var updateTitle = "更新信息数目";
                                var remarkTitle = "新增评论数目";

                                <%foreach (int key in this.keyArrayList)
                                  {%>
                                <%string yValueNew = GetYHashtable(key, SiteServer.CMS.BackgroundPages.ConsoleSiteAnalysis1.YType_New);
                                  string yValueUpdate = GetYHashtable(key, SiteServer.CMS.BackgroundPages.ConsoleSiteAnalysis1.YType_Update);
                                  string yValueRemark = GetYHashtable(key, SiteServer.CMS.BackgroundPages.ConsoleSiteAnalysis1.YType_Remrk);
                                  %>
                                xArrayNew.push('<%=GetXHashtable(key)%>');

                                yArrayNew.push('<%=yValueNew%>');

                                yArrayUpdate.push('<%=yValueUpdate%>');

                                yArrayRemark.push('<%=yValueRemark%>'); 
                            <%}%>


                                if (xArrayNew.length == 0) {
                                    xArrayNew = ["暂无数据"];
                                    yArrayNew = [0];
                                }

                                var option = {
                                    tooltip: {
                                        show: true
                                    },
                                    legend: {
                                        data: [newTitle, updateTitle, remarkTitle]
                                    },
                                    toolbox: {
                                        show: true,
                                        feature: {
                                            dataView: { show: true, readOnly: false },
                                            restore: { show: true },
                                            saveAsImage: { show: true }
                                        }
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
                                            "name": newTitle,
                                            "type": "bar",
                                            "data": []
                                        },
                                       {
                                           "name": updateTitle,
                                           "type": "bar",
                                           "data": []
                                       },
                                      {
                                          "name": remarkTitle,
                                          "type": "bar",
                                          "data": []
                                      },
                                    ]
                                };
                                // 新增
                                option.xAxis[0].data = xArrayNew;
                                option.series[0].data = yArrayNew;
                                //更新
                                option.series[1].data = yArrayUpdate;
                                //评论
                                option.series[2].data = yArrayRemark;
                                newChart.setOption(option);
                            }
                    );
                    </script>
                </div>

            </div>
        </div>

        <div class="popover popover-static">
            <h3 class="popover-title">按管理员统计</h3>
            <div class="popover-content">
                <div style="width: 100%">
                    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
                    <div id="user" style="height: 400px; width: 90%; display: inline-block"></div>
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
                                var newChart = ec.init(document.getElementById('user'));
                                //x array
                                var xArrayNew = [];

                                //y array
                                var yArrayNew = [];
                                var yArrayUpdate = [];
                                var yArrayRemark = [];
                                //title
                                var newTitle = "新增信息数目";
                                var updateTitle = "更新信息数目";
                                var remarkTitle = "新增评论数目";

                                <%foreach (string key in this.keyArrayListUser)
                                  {%>
                                <%string yValueNew = GetYHashtableUser(key, SiteServer.CMS.BackgroundPages.ConsoleSiteAnalysis1.YType_New);
                                  string yValueUpdate = GetYHashtableUser(key, SiteServer.CMS.BackgroundPages.ConsoleSiteAnalysis1.YType_Update);
                                  string yValueRemark = GetYHashtableUser(key, SiteServer.CMS.BackgroundPages.ConsoleSiteAnalysis1.YType_Remrk);
                                  %>
                                xArrayNew.push('<%=GetXHashtableUser(key)%>');


                                yArrayNew.push('<%=yValueNew%>');

                                yArrayUpdate.push('<%=yValueUpdate%>');

                                yArrayRemark.push('<%=yValueRemark%>'); 
                                <%}%>

                                if (xArrayNew.length == 0) {
                                    xArrayNew = ["暂无数据"];
                                    yArrayNew = [0];
                                }

                                var option = {
                                    tooltip: {
                                        show: true
                                    },
                                    legend: {
                                        data: [newTitle, updateTitle, remarkTitle]
                                    },
                                    toolbox: {
                                        show: true,
                                        feature: {
                                            dataView: { show: true, readOnly: false },
                                            restore: { show: true },
                                            saveAsImage: { show: true }
                                        }
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
                                            "name": newTitle,
                                            "type": "bar",
                                            "data": []
                                        },
                                       {
                                           "name": updateTitle,
                                           "type": "bar",
                                           "data": []
                                       },
                                      {
                                          "name": remarkTitle,
                                          "type": "bar",
                                          "data": []
                                      }
                                    ]
                                };
                                // 新增
                                option.xAxis[0].data = xArrayNew;
                                option.series[0].data = yArrayNew;
                                //更新
                                option.series[1].data = yArrayUpdate;
                                //评论
                                option.series[2].data = yArrayRemark;
                                newChart.setOption(option);
                            }
                    );
                    </script>
                </div>
            </div>
        </div>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
