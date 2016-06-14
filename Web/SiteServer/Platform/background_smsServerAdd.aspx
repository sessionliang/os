<%@ Page Language="C#" ValidateRequest="false" Inherits="BaiRong.BackgroundPages.BackgroundSMSServerAdd" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <!--#include file="../inc/header.aspx"-->
    <script type="text/javascript">
        function addFormData(key, value) {
            if (!key) {
                alert("KEY不能为空");
                return;
            }
            if (!value) {
                alert("VALUE不能为空");
                return;
            }
            if (isExist(key)) {
                $("#" + key).val(value);
            }
            else {
                var trHtml = "<tr><td>" + key + " : <input name='smsServerParams_" + key + "' id='smsServerParams_" + key + "' value='" + value + "' />&nbsp;&nbsp;<a href='javascript:void(0);' onclick='removeFormData(" + key + ")'>移除</a></td><td></td></tr>";
                $("#tbParams").append($(trHtml));
            }
        }
        function isExist(key) {
            for (var existList = $("input"), i = 0; i < existList.length; i++) {
                if (!$(existList[i]).attr("name"))
                    continue;
                if ($(existList[i]).attr("name").indexOf("smsServerParams_") == 0 && $(existList[i]).attr("name") == "smsServerParams_" + key) {
                    return true;
                }
            }
            return false;
        }
        function removeFormData(key) {
            if (!key)
                return;
            else {
                $(key).parents("tr").remove();
            }
        }
    </script>
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <form class="form-inline" runat="server">
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />
        <asp:HiddenField ID="hfEditParams" runat="server" />
        <div class="popover popover-static">
            <h3 class="popover-title">
                <asp:Literal ID="ltlPageTitle" runat="server" /></h3>
            <div class="popover-content">

                <asp:PlaceHolder ID="SmsServerBase" runat="server" Visible="false">
                    <table class="table noborder table-hover">
                        <tr>
                            <td width="170">短信服务商名称：</td>
                            <td>
                                <asp:TextBox Columns="25" MaxLength="50" ID="SmsServerName" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td width="170">短信服务商英文名称：</td>
                            <td>
                                <asp:TextBox Columns="25" MaxLength="50" ID="SmsServerEName" runat="server" />
                            </td>
                        </tr>
                    </table>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="SmsServerParams" runat="server" Visible="false">
                    <table class="table noborder table-hover" id="tbParams">
                        <tr>
                            <td width="600">KEY :
                                <input type="text" id="txtKey" />&nbsp;&nbsp;VALUE :
                                <input type="text" id="txtValue" />
                            </td>
                            <td>
                                <input type="button" class="btn" value="添加公共参数" id="btnAddParam" onclick="addFormData($('#txtKey').val(), $('#txtValue').val())" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr />
                            </td>
                        </tr>
                        <asp:Literal ID="ltlEditParams" runat="server" Visible="false"></asp:Literal>
                    </table>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="SmsServerSend" runat="server" Visible="false">
                    <table class="table noborder table-hover">
                        <tr>
                            <td width="170">接口地址：</td>
                            <td>
                                <asp:TextBox ID="tbUrl_Send" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage=" *" ControlToValidate="tbUrl_Send" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td width="170">接口参数：</td>
                            <td>
                                <asp:TextBox ID="tbParams_Send" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage=" *" ControlToValidate="tbParams_Send" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td width="170">请求类型：</td>
                            <td>
                                <asp:DropDownList ID="ddlSendRequestType_Send" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage=" *" ControlToValidate="ddlSendRequestType_Send" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td width="170">请求参数类型：</td>
                            <td>
                                <asp:DropDownList ID="ddlSendParamsType_Send" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage=" *" ControlToValidate="ddlSendParamsType_Send" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td width="170">返回内容类型：</td>
                            <td>
                                <asp:DropDownList ID="ddlReturnContentType_Send" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ErrorMessage=" *" ControlToValidate="ddlReturnContentType_Send" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td width="170">请求状态(key)：</td>
                            <td>
                                <asp:TextBox ID="tbOKFlag_Send" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ErrorMessage=" *" ControlToValidate="tbOKFlag_Send" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td width="170">请求状态对比值(value)：</td>
                            <td>
                                <asp:TextBox ID="tbOKValue_Send" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ErrorMessage=" *" ControlToValidate="tbOKValue_Send" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td width="170">错位信息：</td>
                            <td>
                                <asp:TextBox ID="tbMsgFlag_Send" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="SmsServerLastSms" runat="server" Visible="false">
                    <table class="table noborder table-hover">
                        <tr>
                            <td width="170"></td>
                            <td>
                                <tr>
                                    <td width="170">接口地址：</td>
                                    <td>
                                        <asp:TextBox ID="tbUrl_LastSms" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ErrorMessage=" *" ControlToValidate="tbUrl_LastSms" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">接口参数：</td>
                                    <td>
                                        <asp:TextBox ID="tbParams_LastSms" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ErrorMessage=" *" ControlToValidate="tbParams_LastSms" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSendRequestType_LastSms" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ErrorMessage=" *" ControlToValidate="ddlSendRequestType_LastSms" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求参数类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSendParamsType_LastSms" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ErrorMessage=" *" ControlToValidate="ddlSendParamsType_LastSms" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">返回内容类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlReturnContentType_LastSms" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ErrorMessage=" *" ControlToValidate="ddlReturnContentType_LastSms" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求状态(key)：</td>
                                    <td>
                                        <asp:TextBox ID="tbOKFlag_LastSms" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ErrorMessage=" *" ControlToValidate="tbOKFlag_LastSms" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求状态对比值(value)：</td>
                                    <td>
                                        <asp:TextBox ID="tbOKValue_LastSms" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ErrorMessage=" *" ControlToValidate="tbOKValue_LastSms" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">剩余条数(key)：</td>
                                    <td>
                                        <asp:TextBox ID="tbRetrunValueKey_LastSms" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" ErrorMessage=" *" ControlToValidate="tbRetrunValueKey_LastSms" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">错位信息：</td>
                                    <td>
                                        <asp:TextBox ID="tbMsgFlag_LastSms" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </td>
                        </tr>
                    </table>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="SmsServerInsert" runat="server" Visible="false">
                    <table class="table noborder table-hover">
                        <tr>
                            <td width="170"></td>
                            <td>
                                <tr>
                                    <td width="170">接口地址：</td>
                                    <td>
                                        <asp:TextBox ID="tbUrl_Insert" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ErrorMessage=" *" ControlToValidate="tbUrl_Insert" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">接口参数：</td>
                                    <td>
                                        <asp:TextBox ID="tbParams_Insert" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" ErrorMessage=" *" ControlToValidate="tbParams_Insert" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSendRequestType_Insert" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" ErrorMessage=" *" ControlToValidate="ddlSendRequestType_Insert" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求参数类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSendParamsType_Insert" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" ErrorMessage=" *" ControlToValidate="ddlSendParamsType_Insert" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">返回内容类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlReturnContentType_Insert" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" ErrorMessage=" *" ControlToValidate="ddlReturnContentType_Insert" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求状态(key)：</td>
                                    <td>
                                        <asp:TextBox ID="tbOKFlag_Insert" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" ErrorMessage=" *" ControlToValidate="tbOKFlag_Insert" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求状态对比值(value)：</td>
                                    <td>
                                        <asp:TextBox ID="tbOKValue_Insert" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" ErrorMessage=" *" ControlToValidate="tbOKValue_Insert" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">新增模板ID(key)：</td>
                                    <td>
                                        <asp:TextBox ID="tbRetrunValueKey_Insert" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator30" ErrorMessage=" *" ControlToValidate="tbRetrunValueKey_Insert" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">错位信息：</td>
                                    <td>
                                        <asp:TextBox ID="tbMsgFlag_Insert" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </td>
                        </tr>
                    </table>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="SmsServerUpdate" runat="server" Visible="false">
                    <table class="table noborder table-hover">
                        <tr>
                            <td width="170"></td>
                            <td>
                                <tr>
                                    <td width="170">接口地址：</td>
                                    <td>
                                        <asp:TextBox ID="tbUrl_Update" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator47" ErrorMessage=" *" ControlToValidate="tbUrl_Update" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">接口参数：</td>
                                    <td>
                                        <asp:TextBox ID="tbParams_Update" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator32" ErrorMessage=" *" ControlToValidate="tbParams_Update" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSendRequestType_Update" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator33" ErrorMessage=" *" ControlToValidate="ddlSendRequestType_Update" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求参数类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSendParamsType_Update" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator34" ErrorMessage=" *" ControlToValidate="ddlSendParamsType_Update" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">返回内容类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlReturnContentType_Update" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator35" ErrorMessage=" *" ControlToValidate="ddlReturnContentType_Update" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求状态(key)：</td>
                                    <td>
                                        <asp:TextBox ID="tbOKFlag_Update" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator36" ErrorMessage=" *" ControlToValidate="tbOKFlag_Update" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求状态对比值(value)：</td>
                                    <td>
                                        <asp:TextBox ID="tbOKValue_Update" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator37" ErrorMessage=" *" ControlToValidate="tbOKValue_Update" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">新增模板ID(key)：</td>
                                    <td>
                                        <asp:TextBox ID="tbRetrunValueKey_Update" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator38" ErrorMessage=" *" ControlToValidate="tbRetrunValueKey_Update" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">错位信息：</td>
                                    <td>
                                        <asp:TextBox ID="tbMsgFlag_Update" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </td>
                        </tr>
                    </table>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="SmsServerDelete" runat="server" Visible="false">
                    <table class="table noborder table-hover">
                        <tr>
                            <td width="170"></td>
                            <td>
                                <tr>
                                    <td width="170">接口地址：</td>
                                    <td>
                                        <asp:TextBox ID="tbUrl_Delete" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator39" ErrorMessage=" *" ControlToValidate="tbUrl_Delete" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">接口参数：</td>
                                    <td>
                                        <asp:TextBox ID="tbParams_Delete" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator40" ErrorMessage=" *" ControlToValidate="tbParams_Delete" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSendRequestType_Delete" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator41" ErrorMessage=" *" ControlToValidate="ddlSendRequestType_Delete" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求参数类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSendParamsType_Delete" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator42" ErrorMessage=" *" ControlToValidate="ddlSendParamsType_Delete" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">返回内容类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlReturnContentType_Delete" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator43" ErrorMessage=" *" ControlToValidate="ddlReturnContentType_Delete" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求状态(key)：</td>
                                    <td>
                                        <asp:TextBox ID="tbOKFlag_Delete" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator44" ErrorMessage=" *" ControlToValidate="tbOKFlag_Delete" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求状态对比值(value)：</td>
                                    <td>
                                        <asp:TextBox ID="tbOKValue_Delete" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator45" ErrorMessage=" *" ControlToValidate="tbOKValue_Delete" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">错位信息：</td>
                                    <td>
                                        <asp:TextBox ID="tbMsgFlag_Delete" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </td>
                        </tr>
                    </table>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="SmsServerSearch" runat="server" Visible="false">
                    <table class="table noborder table-hover">
                        <tr>
                            <td width="170"></td>
                            <td>
                                <tr>
                                    <td width="170">接口地址：</td>
                                    <td>
                                        <asp:TextBox ID="tbUrl_Search" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" ErrorMessage=" *" ControlToValidate="tbUrl_Search" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">接口参数：</td>
                                    <td>
                                        <asp:TextBox ID="tbParams_Search" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator24" ErrorMessage=" *" ControlToValidate="tbParams_Search" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSendRequestType_Search" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator25" ErrorMessage=" *" ControlToValidate="ddlSendRequestType_Search" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求参数类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSendParamsType_Search" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator26" ErrorMessage=" *" ControlToValidate="ddlSendParamsType_Search" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">返回内容类型：</td>
                                    <td>
                                        <asp:DropDownList ID="ddlReturnContentType_Search" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator27" ErrorMessage=" *" ControlToValidate="ddlReturnContentType_Search" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求状态(key)：</td>
                                    <td>
                                        <asp:TextBox ID="tbOKFlag_Search" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator28" ErrorMessage=" *" ControlToValidate="tbOKFlag_Search" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">请求状态对比值(value)：</td>
                                    <td>
                                        <asp:TextBox ID="tbOKValue_Search" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator29" ErrorMessage=" *" ControlToValidate="tbOKValue_Search" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">模板内容(key)：</td>
                                    <td>
                                        <asp:TextBox ID="tbRetrunValueKey_Search" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" ErrorMessage=" *" ControlToValidate="tbRetrunValueKey_Search" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170">错位信息：</td>
                                    <td>
                                        <asp:TextBox ID="tbMsgFlag_Search" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </td>
                        </tr>
                    </table>
                </asp:PlaceHolder>

                <asp:PlaceHolder ID="Done" runat="server" Visible="false">

                    <blockquote>
                        <p>完成!</p>
                        <small>操作成功。</small>
                    </blockquote>

                </asp:PlaceHolder>
                <asp:PlaceHolder ID="OperatingError" runat="server" Visible="false">

                    <blockquote>
                        <p>发生错误</p>
                        <small>执行向导过程中出错。</small>
                    </blockquote>

                    <div class="alert alert-error">
                        <h4>
                            <asp:Label ID="ErrorLabel" runat="server"></asp:Label></h4>
                    </div>

                </asp:PlaceHolder>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn" ID="Previous" OnClick="PreviousPanel" runat="server" Text="< 上一步"></asp:Button>
                            <asp:Button class="btn btn-primary" ID="Next" OnClick="NextPanel" runat="server" Text="下一步 >"></asp:Button>
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
