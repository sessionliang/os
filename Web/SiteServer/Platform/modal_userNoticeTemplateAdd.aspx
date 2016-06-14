<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.Modal.UserNoticeTemplateAdd" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <!--#include file="../inc/header.aspx"-->
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <script type="text/javascript">
        (function ($) {
            $.fn.caret = function (pos) {
                var target = this[0];
                //get
                if (arguments.length == 0) {
                    //HTML5
                    if (window.getSelection) {
                        //contenteditable
                        if (target.contentEditable == 'true') {
                            target.focus();
                            var range1 = window.getSelection().getRangeAt(0),
                                range2 = range1.cloneRange();
                            range2.selectNodeContents(target);
                            range2.setEnd(range1.endContainer, range1.endOffset);
                            return range2.toString().length;
                        }
                        //textarea
                        return target.selectionStart;
                    }
                    //IE<9
                    if (document.selection) {
                        target.focus();
                        //contenteditable
                        if (target.contentEditable == 'true') {
                            var range1 = document.selection.createRange(),
                                range2 = document.body.createTextRange();
                            range2.moveToElementText(target);
                            range2.setEndPoint('EndToEnd', range1);
                            return range2.text.length;
                        }
                        //textarea
                        var pos = 0,
                            range = target.createTextRange(),
                            range2 = document.selection.createRange().duplicate(),
                            bookmark = range2.getBookmark();
                        range.moveToBookmark(bookmark);
                        while (range.moveStart('character', -1) !== 0) pos++;
                        return pos;
                    }
                    //not supported
                    return 0;
                }
                //set
                //HTML5
                if (window.getSelection) {
                    //contenteditable
                    if (target.contentEditable == 'true') {
                        target.focus();
                        window.getSelection().collapse(target.firstChild, pos);
                    }
                        //textarea
                    else
                        target.setSelectionRange(pos, pos);
                }
                    //IE<9
                else if (document.body.createTextRange) {
                    var range = document.body.createTextRange();
                    range.moveToElementText(target)
                    range.moveStart('character', pos);
                    range.collapse(true);
                    range.select();
                }
            }
        })(jQuery)

        function AddOnPos(value) {
            var dest = $("#hidDest").val();
            var val = $('#' + dest).val();
            var i = $('#' + dest).caret();
            if (i == 0) {
                val = val + value;
            } else {
                val = val.substr(0, i) + value + val.substr(i, val.length);
            }
            $('#' + dest).val(val);
            if (i > 0) {
                $('#' + dest).caret(i + value.length);
            } else {
                $('#' + dest).caret(val.length);
            }
        }

        $(function () {
            $("#tbTitle").focus(function () {
                $("#hidDest").val($(this).attr("id"));
            });

            $("#tbContent").focus(function () {
                $("#hidDest").val($(this).attr("id"));
            });
        });
    </script>
    <form class="form-inline" runat="server">
        <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
        <bairong:Alerts runat="server" />

        <div class="popover popover-static">
            <h3 class="popover-title"></h3>
            <div class="popover-content">
                <table class="table table-bordered table-hover">
                    <tr class="info">
                        <td>规则</td>
                        <td>含义</td>
                        <td>规则</td>
                        <td>含义</td>
                        <td>规则</td>
                        <td>含义</td>
                    </tr>
                    <asp:Literal ID="ltRules" runat="server"></asp:Literal>
                </table>
                <input type="hidden" id="hidDest" value="tbTitle"/>
                <hr />
                <table class="table noborder table-hover">
                    <tr>
                        <td width="120">模板名称：</td>
                        <td>
                            <asp:TextBox ID="tbName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator
                                ControlToValidate="tbName"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td width="120">消息所属：</td>
                        <td>
                            <asp:DropDownList ID="ddlUseNoticeType" Width="360" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator
                                ControlToValidate="ddlUseNoticeType"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td width="120">模板类型：</td>
                        <td>
                            <asp:DropDownList ID="ddlUserNoticeTemplateType" Width="360" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator
                                ControlToValidate="ddlUserNoticeTemplateType"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td width="120">标题：</td>
                        <td>
                            <asp:TextBox ID="tbTitle" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator
                                ControlToValidate="tbTitle"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                            <br />
                            <span>可从上方的规则中点击选取插入的内容</span>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">内容：</td>
                        <td>
                            <asp:TextBox ID="tbContent" runat="server" TextMode="MultiLine" Height="150" Width="348"></asp:TextBox>
                            <asp:RequiredFieldValidator
                                ControlToValidate="tbContent"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                            <br />
                            <span>可从上方的规则中点击选取插入的内容</span>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">是否可用：</td>
                        <td>
                            <asp:RadioButtonList ID="rbIsEnable" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
                            <asp:RequiredFieldValidator
                                ControlToValidate="rbIsEnable"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
