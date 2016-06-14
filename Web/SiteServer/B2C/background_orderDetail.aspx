<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundOrderDetail" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
<title>订单号：<asp:Literal id="ltlPageTitle" runat="server" /></title>
<link href="css/font-awesome/css/font-awesome.min.css" rel="stylesheet"> 
<link href="css/single.css" rel="stylesheet">
<script type="text/javascript">
$(document).ready(function(){
  $('#order_consignee_copy').click(function(e){
    var copyText = $(this).attr('info');
    if (window.clipboardData){
        window.clipboardData.setData("Text", copyText);
        $('#order_consignee_copy').parent().html('信息已经复制');
    }else{
        prompt('请复制收货人信息：',copyText);
    }
  });
});
</script>
</head>

<body>
<!-- Header starts -->
  <header>
    <div class="container-fluid">
      <div class="row-fluid">

        <!-- Logo section -->
        <div class="span4">
          <!-- Logo. -->
          <div class="logo">
            <p class="meta">订单号：</p>
            <h1><asp:Literal ID="ltlOrderSN" runat="server"></asp:Literal></h1>
          </div>
          <!-- Logo ends -->
        </div>

        <!-- Button section -->
        <div class="span4">

          <ul class="nav nav-pills">

            <!-- <li class="dropdown dropdown-big">
              <a class="dropdown-toggle" href="http://responsivewebinc.com/premium/macadmin/index.html#" data-toggle="dropdown">
                <i class="icon-comments"></i> Chats <span class="badge badge-info">6</span> 
              </a>

                <ul class="dropdown-menu">
                  <li>
                    <h5><i class="icon-comments"></i> Chats</h5>
                    <hr>
                  </li>
                  <li>
                    <h6><a href="http://responsivewebinc.com/premium/macadmin/index.html#">Hi :)</a> <span class="label label-warning pull-right">10:42</span></h6>
                    <div class="clearfix"></div>
                    <hr>
                  </li>
                  <li>
                    <h6><a href="http://responsivewebinc.com/premium/macadmin/index.html#">How are you?</a> <span class="label label-warning pull-right">20:42</span></h6>
                    <div class="clearfix"></div>
                    <hr>
                  </li>
                  <li>
                    <h6><a href="http://responsivewebinc.com/premium/macadmin/index.html#">What are you doing?</a> <span class="label label-warning pull-right">14:42</span></h6>
                    <div class="clearfix"></div>
                    <hr>
                  </li>                  
                  <li>
                    <div class="drop-foot">
                      <a href="http://responsivewebinc.com/premium/macadmin/index.html#">View All</a>
                    </div>
                  </li>                                    
                </ul>
            </li> -->

<!--             <li class="dropdown dropdown-big">
              <a class="dropdown-toggle" href="http://responsivewebinc.com/premium/macadmin/index.html#" data-toggle="dropdown">
                <i class="icon-envelope-alt"></i> Inbox <span class="badge badge-important">6</span> 
              </a>

                <ul class="dropdown-menu">
                  <li>
                    <h5><i class="icon-envelope-alt"></i> Messages</h5>
                    <hr>
                  </li>
                  <li>
                    <h6><a href="http://responsivewebinc.com/premium/macadmin/index.html#">Hello how are you?</a></h6>
                    <p>Quisque eu consectetur erat eget  semper...</p>
                    <hr>
                  </li>
                  <li>
                    <h6><a href="http://responsivewebinc.com/premium/macadmin/index.html#">Today is wonderful?</a></h6>
                    <p>Quisque eu consectetur erat eget  semper...</p>
                    <hr>
                  </li>
                  <li>
                    <div class="drop-foot">
                      <a href="http://responsivewebinc.com/premium/macadmin/index.html#">View All</a>
                    </div>
                  </li>                                    
                </ul>
            </li>

            <li class="dropdown dropdown-big">
              <a class="dropdown-toggle" href="http://responsivewebinc.com/premium/macadmin/index.html#" data-toggle="dropdown">
                <i class="icon-user"></i> Users <span class="badge badge-success">6</span> 
              </a>

                <ul class="dropdown-menu">
                  <li>
                    <h5><i class="icon-user"></i> Users</h5>
                    <hr>
                  </li>
                  <li>
                    <h6><a href="http://responsivewebinc.com/premium/macadmin/index.html#">Ravi Kumar</a> <span class="label label-warning pull-right">Free</span></h6>
                    <div class="clearfix"></div>
                    <hr>
                  </li>
                  <li>
                    <h6><a href="http://responsivewebinc.com/premium/macadmin/index.html#">Balaji</a> <span class="label label-important pull-right">Premium</span></h6>
                    <div class="clearfix"></div>
                    <hr>
                  </li>
                  <li>
                    <h6><a href="http://responsivewebinc.com/premium/macadmin/index.html#">Kumarasamy</a> <span class="label label-warning pull-right">Free</span></h6>
                    <div class="clearfix"></div>
                    <hr>
                  </li>                  
                  <li>
                    <div class="drop-foot">
                      <a href="http://responsivewebinc.com/premium/macadmin/index.html#">View All</a>
                    </div>
                  </li>                                    
                </ul>
            </li>  -->

          </ul>

        </div>

        <div class="span4">
          <div class="header-data">

            <div class="hdata">
              <div class="mcol-left">
                <i class="icon-user bblue"></i> 
              </div>
              <div class="mcol-right">
                <p>
                    <asp:Literal id="ltlOrderStatus" runat="server" />
                    <em>订单状态</em>
                </p>
              </div>
              <div class="clearfix"></div>
            </div>

            <div class="hdata">
              <div class="mcol-left">
                <i class="icon-money bgreen"></i> 
              </div>
              <div class="mcol-right">
                <p>
                    <asp:Literal id="ltlPaymentStatus" runat="server" />
                    <em>支付状态</em>
                </p>
              </div>
              <div class="clearfix"></div>
            </div>

            <div class="hdata">
              <div class="mcol-left">
                <i class="icon-signal bred"></i> 
              </div>
              <div class="mcol-right">
                <p>
                    <asp:Literal id="ltlShipmentStatus" runat="server" />
                    <em>发货状态</em>
                </p>
              </div>
              <div class="clearfix"></div>
            </div>

          </div>
        </div>

      </div>
    </div>
  </header>

<!-- Header ends -->

<!-- Main content starts -->

<div class="content">

  	<!-- Sidebar -->
  	<div class="sidebar">
      	<div class="sidebar-dropdown"><a href="http://responsivewebinc.com/premium/macadmin/index.html#">Navigation</a></div>

      	<!--- Sidebar navigation -->
      	<!-- If the main navigation has sub navigation, then add the class "has_sub" to "li" of main navigation. -->
      	<ul id="nav">
        	<!-- Main menu with font awesome icon -->
          <li><a href="single_page/index.html" class="open"><i class="icon-home"></i> 基本信息</a>
            <!-- Sub menu markup 
            <ul>
              <li><a href="#">Submenu #1</a></li>
              <li><a href="#">Submenu #2</a></li>
              <li><a href="#">Submenu #3</a></li>
            </ul>-->
          </li>
          <!-- <li class="has_sub"><a href="http://responsivewebinc.com/premium/macadmin/index.html#"><i class="icon-list-alt"></i> Widgets  <span class="pull-right"><i class="icon-chevron-right"></i></span></a>
            <ul>
              <li><a href="http://responsivewebinc.com/premium/macadmin/widgets1.html">Widgets #1</a></li>
              <li><a href="http://responsivewebinc.com/premium/macadmin/widgets2.html">Widgets #2</a></li>
              <li><a href="http://responsivewebinc.com/premium/macadmin/widgets3.html">Widgets #3</a></li>
            </ul>
          </li>  
          <li class="has_sub"><a href="http://responsivewebinc.com/premium/macadmin/index.html#"><i class="icon-file-alt"></i> Pages #1  <span class="pull-right"><i class="icon-chevron-right"></i></span></a>
            <ul>
              <li><a href="http://responsivewebinc.com/premium/macadmin/post.html">Post</a></li>
              <li><a href="http://responsivewebinc.com/premium/macadmin/login.html">Login</a></li>
              <li><a href="http://responsivewebinc.com/premium/macadmin/register.html">Register</a></li>
              <li><a href="http://responsivewebinc.com/premium/macadmin/support.html">Support</a></li>
              <li><a href="http://responsivewebinc.com/premium/macadmin/invoice.html">Invoice</a></li>
              <li><a href="http://responsivewebinc.com/premium/macadmin/profile.html">Profile</a></li>
              <li><a href="http://responsivewebinc.com/premium/macadmin/gallery.html">Gallery</a></li>
            </ul>
          </li> 
          <li class="has_sub"><a href="http://responsivewebinc.com/premium/macadmin/index.html#"><i class="icon-file-alt"></i> Pages #2  <span class="pull-right"><i class="icon-chevron-right"></i></span></a>
            <ul>
              <li><a href="http://responsivewebinc.com/premium/macadmin/media.html">Media</a></li>
              <li><a href="http://responsivewebinc.com/premium/macadmin/statement.html">Statement</a></li>
              <li><a href="http://responsivewebinc.com/premium/macadmin/error.html">Error</a></li>
              <li><a href="http://responsivewebinc.com/premium/macadmin/error-log.html">Error Log</a></li>
              <li><a href="http://responsivewebinc.com/premium/macadmin/calendar.html">Calendar</a></li>
              <li><a href="http://responsivewebinc.com/premium/macadmin/grid.html">Grid</a></li>
            </ul>
          </li>                             
          <li><a href="http://responsivewebinc.com/premium/macadmin/charts.html"><i class="icon-bar-chart"></i> Charts</a></li> 
          <li><a href="http://responsivewebinc.com/premium/macadmin/tables.html"><i class="icon-table"></i> Tables</a></li>
	        <li><a href="http://responsivewebinc.com/premium/macadmin/forms.html"><i class="icon-tasks"></i> Forms</a></li>
          <li><a href="http://responsivewebinc.com/premium/macadmin/ui.html"><i class="icon-magic"></i> User Interface</a></li> -->
      	</ul>
  	</div>

  	<!-- Sidebar ends -->

  	<!-- Main bar -->
  	<div class="mainbar">
      
	    <!-- Page heading -->
	    <div class="page-head">
	      <h2 class="pull-left">基本信息</h2>

        <!-- Breadcrumb -->
        <div class="bread-crumb pull-right">
          <a href="single_page/index.html"><i class="icon-home"></i> 订单管理</a> 
          <!-- Divider -->
          <span class="divider">/</span> 
          <a href="http://responsivewebinc.com/premium/macadmin/index.html#" class="bread-current">基本信息</a>
        </div>

        <div class="clearfix"></div>

	    </div>
	    <!-- Page heading ends -->



	    <!-- Matter -->

	    <div class="matter">
        <div class="container-fluid">

          <!-- 商品信息 -->
          <div class="row-fluid">
            <div class="span12">

              <div class="widget">
                <div class="widget-head">
                  <div class="pull-left">订单商品</div>
                  <div class="widget-icons pull-right">
                    <a href="#" class="wminimize"><i class="icon-chevron-up"></i></a> 
                    <a href="#" class="wclose"><i class="icon-remove"></i></a>
                  </div>  
                  <div class="clearfix"></div>
                </div>
                <div class="widget-content">
                  <div class="padd statement">
                    
                    <div class="row-fluid">

                      <div class="span2">
                        <div class="well">
                          <h2>
                            <asp:Literal id="ltlPriceTotal" runat="server" />
                          </h2>
                          <p>商品金额</p>
                        </div>
                      </div>

                      <div class="span2">
                        <div class="well">
                          <h2>
                            <asp:Literal id="ltlPriceReturn" runat="server" />
                          </h2>
                          <p>返现金额</p>
                        </div>
                      </div>

                      <div class="span2">
                        <div class="well">
                          <h2>
                            <asp:Literal id="ltlPriceShipment" runat="server" />
                          </h2>
                          <p>运费金额</p>
                        </div>
                      </div>

                      <div class="span2">
                        <div class="well success">
                          <h2>
                            <asp:Literal id="ltlPriceActual" runat="server" />
                          </h2>
                          <p><span class="label label-success">实际支付金额</span></p>
                        </div>
                      </div>

                      <!-- <div class="span2">
                        <div class="well">
                          <h2>$883</h2>
                          <p>配送费用</p>                        
                        </div>
                      </div>

                      <div class="span2">
                        <div class="well">
                          <h2>$23232</h2>
                          <p>合计</p>
                        </div>
                      </div>

                      <div class="span2">
                        <div class="well">
                          <h2>$23232</h2>
                          <p>合计</p>
                        </div>
                      </div>

                      <div class="span2">
                        <div class="well">
                          <h2>$23232</h2>
                          <p>合计</p>
                        </div>
                      </div> -->

                    </div>

                    <div class="row-fluid">

                      <div class="span12">
                        <hr>
                        <table class="table table-striped table-hover table-bordered">
                          <thead>
                            <tr>
                              <th>#</th>
                              <th>货号</th>
                              <th>商品名称</th>
                              <th>规格</th>
                              <th>价格</th>
                              <th>购买量</th>
                              <th>是否发货</th>
                            </tr>
                          </thead>
                          <tbody>
                            <asp:Repeater ID="rptOrderItems" runat="server">
                              <itemtemplate>
                                <tr>
                                  <td>
                                    <asp:Literal ID="ltlItemIndex" runat="server"></asp:Literal>
                                  </td>
                                  <td>
                                    <asp:Literal ID="ltlGoodsSN" runat="server"></asp:Literal>
                                  </td>
                                  <td>
                                    <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
                                  </td>
                                  <td>
                                    <div class="specItem"><asp:Literal ID="ltlSpec" runat="server"></asp:Literal></div>
                                  </td>
                                  <td>
                                    <asp:Literal ID="ltlPrice" runat="server"></asp:Literal>
                                  </td>
                                  <td>
                                    <asp:Literal ID="ltlPurchaseNum" runat="server"></asp:Literal>
                                  </td>
                                  <td>
                                    <asp:Literal ID="ltlIsShipment" runat="server"></asp:Literal>
                                  </td>
                                </tr>
                              </itemtemplate>
                            </asp:Repeater>
                            
                          </tbody>
                        </table>

                      </div>

                    </div>

                  </div>
                  <div class="widget-foot">
                  </div>
                </div>
              </div>  
              
            </div>
          </div>

          <!-- 基本信息 -->
          <div class="row-fluid">


            <!-- 订单信息 -->
            <div class="span4">
              <div class="widget">
                <div class="widget-head">
                  <div class="pull-left">订单信息</div>
                  <div class="widget-icons pull-right">
                    <a href="#" class="wminimize"><i class="icon-chevron-up"></i></a> 
                    <a href="#" class="wclose"><i class="icon-remove"></i></a>
                  </div>  
                  <div class="clearfix"></div>
                </div>
                <div class="widget-content">
                  
                  <table class="table table-striped table-bordered table-hover">
                    <tbody>
                      <asp:Literal id="ltlAttibutesOrder" runat="server"></asp:Literal>
                    </tbody>
                  </table>

                  <div class="widget-foot">
                  </div>
                </div>
              </div>
            </div>

            <!-- 会员信息 -->
            <div class="span4">
              <div class="widget">
                <div class="widget-head">
                  <div class="pull-left">会员信息</div>
                  <div class="widget-icons pull-right">
                    <a href="#" class="wminimize"><i class="icon-chevron-up"></i></a> 
                    <a href="#" class="wclose"><i class="icon-remove"></i></a>
                  </div>  
                  <div class="clearfix"></div>
                </div>
                <div class="widget-content">
                  
                  <table class="table table-striped table-bordered table-hover">
                    <tbody>
                      <asp:Literal id="ltlAttibutesUser" runat="server"></asp:Literal>
                    </tbody>
                  </table>

                  <div class="widget-foot">
                  </div>
                </div>
              </div>
            </div>

            <!-- 收货人信息 -->
            <div class="span4">
              <div class="widget">
                <div class="widget-head">
                  <div class="pull-left">收货人信息</div>
                  <div class="widget-icons pull-right">
                    <a href="#" class="wminimize"><i class="icon-chevron-up"></i></a> 
                    <a href="#" class="wclose"><i class="icon-remove"></i></a>
                  </div>  
                  <div class="clearfix"></div>
                </div>
                <div class="widget-content">
                  
                  <table class="table table-striped table-bordered table-hover">
                    <tbody>
                      <asp:Literal id="ltlAttibutesConsignee" runat="server"></asp:Literal>
                      <tr><td colspan="2" class="right">
                        <asp:Literal id="ltlConsigneeCopy" runat="server"></asp:Literal>
                      </td></tr>
                    </tbody>
                  </table>

                  <div class="widget-foot">
                  </div>
                </div>
              </div>
            </div>

          </div>

          <div class="row-fluid">
            
            <!-- 发票信息 -->
            <asp:PlaceHolder id="phInvoice" runat="server">
            <div class="span4">
              <div class="widget">
                <div class="widget-head">
                  <div class="pull-left">发票信息</div>
                  <div class="widget-icons pull-right">
                    <a href="#" class="wminimize"><i class="icon-chevron-up"></i></a>
                    <a href="#" class="wclose"><i class="icon-remove"></i></a>
                  </div>  
                  <div class="clearfix"></div>
                </div>
                <div class="widget-content">
                  
                  <table class="table table-striped table-bordered table-hover">
                    <tbody>
                      <asp:Literal id="ltlAttibutesInvoice" runat="server"></asp:Literal>
                    </tbody>
                  </table>

                  <div class="widget-foot">
                  </div>
                </div>
              </div>
            </div>
            </asp:PlaceHolder>

          </div>


        </div>
		  </div>

		<!-- Matter ends -->

    </div>

   <!-- Mainbar ends -->	    	
   <div class="clearfix"></div>

</div>
<!-- Content ends -->

</body></html>