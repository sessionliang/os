﻿<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundAdminLoginAnalysis" %>

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
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />

        <div class="popover popover-static">
            <div class="well well-small">
                <div id="contentSearch" style="margin-top: 10px;">
                    时间从：
     
                <bairong:DateTimeTextBox ID="dateFrom" class="input-small" Columns="12" runat="server" />
                    到：
     
                <bairong:DateTimeTextBox ID="dateTo" class="input-small" Columns="12" runat="server" />
                    x轴：
     
                <asp:DropDownList ID="ddlXType" runat="server"></asp:DropDownList>
                    <asp:Button class="btn" OnClick="Search_OnClick" Text="搜 索" runat="server" />
                </div>
            </div>

            <h3 class="popover-title">管理员登录最近<%=count %><%=BaiRong.Model.EStatictisXTypeUtils.GetText(BaiRong.Model.EStatictisXTypeUtils.GetEnumType(base.GetQueryString("XType"))) %>分配图表（按日期统计）</h3>
            <div class="popover-content">
                <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
                <div id="main" style="height: 400px"></div>
                <!-- ECharts单文件引入 -->
                <script src="/sitefiles/bairong/jquery/echarts/build/dist/echarts.js"></script>
                <script type="text/javascript">
                    // 路径配置
                    require.config({
                        paths: {
                            echarts: '/sitefiles/bairong/jquery/echarts/build/dist'
                        }
                    });
                    // 使用
                    require(
                        [
                            'echarts',
                            'echarts/chart/line' // 使用柱状图就加载bar模块，按需加载
                        ],
                        function (ec) {
                            // 基于准备好的dom，初始化echarts图表
                            var myChart = ec.init(document.getElementById('main'));
                            //x array
                            var xArray = [];
                            //y array
                            var yArray = [];

                    <%for (int i = 1; i <= this.count; i++)
                      {%>
                            xArray.push('<%=GetGraphicX(i)%>');
                            yArray.push('<%=GetGraphicY(i)%>');
                    <%}%>

                            var option = {
                                tooltip: {
                                    show: true
                                },
                                legend: {
                                    data: ['管理员（按日期统计）']
                                },
                                xAxis: [
                                    {
                                        type: 'category',
                                        data: xArray
                                    }
                                ],
                                yAxis: [
                                    {
                                        type: 'value'
                                    }
                                ],
                                series: [
                                    {
                                        "name": "登录次数",
                                        "type": "line",
                                        "data": yArray
                                    }
                                ]
                            };

                            // 为echarts对象加载数据 
                            myChart.setOption(option);
                        }
            );
                </script>
            </div>
        </div>

        <div class="popover popover-static">
            <h3 class="popover-title">管理员登录最近<%=count %><%=BaiRong.Model.EStatictisXTypeUtils.GetText(BaiRong.Model.EStatictisXTypeUtils.GetEnumType(base.GetQueryString("XType"))) %>分配图表（按管理员统计）</h3>
            <div class="popover-content">
                <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
                <div id="user" style="height: 400px"></div>
                <!-- ECharts单文件引入 -->
                <script src="/sitefiles/bairong/jquery/echarts/build/dist/echarts.js"></script>
                <script type="text/javascript">
                    // 路径配置
                    require.config({
                        paths: {
                            echarts: '/sitefiles/bairong/jquery/echarts/build/dist'
                        }
                    });
                    // 使用
                    require(
                        [
                            'echarts',
                            'echarts/chart/bar' // 使用柱状图就加载bar模块，按需加载
                        ],
                        function (ec) {
                            // 基于准备好的dom，初始化echarts图表
                            var myChart = ec.init(document.getElementById('user'));
                            //x array
                            var xArray = [];
                            //y array
                            var yArray = [];

                            <%foreach (string key in this.adminNumHashtableName.Keys)
                              {%>
                            xArray.push('<%=key%>');
                            yArray.push('<%=GetGraphicYUser(key)%>');
                    <%}%>

                            var option = {
                                tooltip: {
                                    show: true
                                },
                                legend: {
                                    data: ['管理员（按管理员统计）']
                                },
                                xAxis: [
                                    {
                                        type: 'category',
                                        data: xArray
                                    }
                                ],
                                yAxis: [
                                    {
                                        type: 'value'
                                    }
                                ],
                                series: [
                                    {
                                        "name": "登录次数",
                                        "type": "bar",
                                        "data": yArray
                                    }
                                ]
                            };

                            // 为echarts对象加载数据 
                            myChart.setOption(option);
                        }
            );
                </script>
            </div>
        </div>

        <ul class="breadcrumb breadcrumb-button">
            <asp:Button class="btn" ID="ExportTracking" runat="server" Text="导出Excel"></asp:Button>
        </ul>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
