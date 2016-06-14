function hideMenu() {
    $('.main-nav li.open').removeClass('open');
};

function submitAjaxUrl(ajaxUrl, parameters) {
    $.post(ajaxUrl, parameters, function (data) {
        if (typeof (data) == "string")
            data = eval('(' + data + ')');
        if (data.isNext == 'true' && data.ajaxUrl) {
            submitAjaxUrl(data.ajaxUrl, data.parameters);
        }
    }).error(function () {
        $.post("/siteserver/stl/background_serviceStl.aspx", { type: "AjaxUrlFSONext" }, function (data) {
            if (typeof (data) == "string")
                data = eval('(' + data + ')');
            if (data.isNext == 'true' && data.ajaxUrl) {
                submitAjaxUrl(data.ajaxUrl, data.parameters);
            }
        });
    });
}

$(document).ready(function () {
    $(".toggle-nav").click(function (e) {
        e.preventDefault();
        $("#left").toggle().toggleClass("forced-hide");
        $("div.right").toggleClass("reight_p");
        var i = $('i', this);
        if (i.hasClass("icon-arrow-left")) {
            i.removeClass().addClass('icon-arrow-right');
        } else {
            i.removeClass().addClass('icon-arrow-left');
        }
    });
});