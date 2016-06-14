var filterController = {};

filterController.keywords = utilService.getUrlVar('keywords');
filterController.filters = utilService.getUrlVar('filters');
filterController.order = utilService.getUrlVar('order');
filterController.page = utilService.getUrlVar('page');
filterController.hasGoods = utilService.getUrlVar('hasGoods');
filterController.isCOD = utilService.getUrlVar('isCOD');
filterController.filterSearchWords = utilService.getUrlVar('filterSearchWords');
filterController.isRedirect = utilService.getUrlVar('isRedirect');
filterController.isWaterfall = utilService.getUrlVar('isWaterfall');

filterController.data = {};
filterController.pageItem = {};

//设置选中
filterController.setFlag = function (el) {
    $(el).parent().find("a").removeClass("ser_cuta1");
    $(el).parent().find("a").removeClass("scmr_cuta1");
    if ($(el).hasClass("ser_a1"))
        $(el).addClass("ser_cuta1");
    else if ($(el).hasClass("scmr_a1"))
        $(el).addClass("scmr_cuta1");
};

filterController.isDynamic = true
filterController.pathFilters = utilService.urlToMap(filterController.filters);

if (filterController.keywords || filterController.filters || filterController.order || filterController.page) {
    filterController.isDynamic = true;
}

if (filterController.isDynamic) {

    $('#filterStatic').hide();
    $('#filterLoading').show();

    $pageInfo.stlPageContents = '<stl:pageContents pageNum="20" scope="All"></stl:pageContents></stl:pageContents>';

    var postParams = { keywords: filterController.keywords, filters: filterController.filters, order: filterController.order, page: filterController.page, stlPageContents: $pageInfo.stlPageContents, hasGoods: filterController.hasGoods, isCOD: filterController.isCOD, filterSearchWords: filterController.filterSearchWords, isRedirect: filterController.isRedirect };

    //设置连接的原有参数
    var linkParam = utilService.urlToMap(window.location.search.substring(1));
    for (var p in linkParam) {
        if (!postParams[p])
            postParams[p] = linkParam[p];
    }

    filterService.getResults(postParams, function (data) {
        filterController.data = data;
        filterController.pageItem = data.pageItem;

        $('#filterLoading').hide();
        utilService.render('filterController', filterController);
        utilService.render('filterPageController', filterController);

        //设置选中filter
        if (filterController.filters) {
            var filters = unescape(filterController.filters);
            var filterArray = filters.split('&');
            for (var i = 0; i < filterArray.length; i++) {
                var filter = filterArray[i];
                filter = filter.replace('=', '_');
                filterController.setFlag($("#" + filter));
                var filterID = filter.split('_')[0];
                var itemID = filter.split('_')[1];
                $("#filterCrumbs").append($("<a onclick='filterController.removeFilter(" + filterID + "," + itemID + ")' href='javascript:void(0);'><b>" + $("#Filter_" + filterID).attr("filter") + " : </b><em>" + $("#" + filter).html() + "</em><i></i></a>"));
            }
        }
        //设置选中order
        if (filterController.order) {
            var order = unescape(filterController.order);
            filterController.setFlag($("#" + order));
        }
        //设置hasGoods
        if (filterController.hasGoods) {
            var hasGoods = unescape(filterController.hasGoods);
            if (hasGoods == 'true') {
                $("#ckHasGoods").attr("checked", hasGoods);
            }
            else {
                $("#ckHasGoods").removeAttr("checked");
            }
        }
    });
}

filterController.isFilter = function (filterID, itemID) {
    filterID = filterID + '';
    itemID = itemID + '';

    if (itemID == '0') {
        return filterController.pathFilters[filterID] == '0';
    } else {
        return filterController.pathFilters[filterID] == itemID;
    }
};

filterController.isOrder = function (order) {
    return filterController.data.order == order;
};

filterController.getUrl = function (filterID, itemID, order, page, keywords, hasGoods, isCOD, filterSearchWords, isRedirect) {
    filterMap = utilService.clone(filterController.pathFilters);
    if (filterID && itemID)
        filterMap[filterID] = itemID;
    if (!order) order = filterController.order;
    if (!page) page = 1;//filterController.page;//说明：指向到其他过滤条件时，默认指向第一页
    //if (!keywords) keywords = filterController.keywords;//说明：指向到其他过滤条件时，默认清楚关键字
    if (!hasGoods) hasGoods = filterController.hasGoods;//默认false, 显示所有商品；true - 只显示有货的商品
    if (!isCOD) isCOD = filterController.isCOD;//默认false, 显示所有商品；true - 只显示支持货到付款的商品

    urlMap = {};
    //设置连接的原有参数
    var linkParam = utilService.urlToMap(window.location.search.substring(1));
    for (var p in linkParam) {
        if (!urlMap[p])
            urlMap[p] = linkParam[p];
    }


    if (keywords) {
        urlMap['keywords'] = keywords;
    }
    if (filterMap) {
        urlMap['filters'] = utilService.mapToUrl(filterMap);
    }
    if (order) {
        urlMap['order'] = order;
    }
    if (page) {
        urlMap['page'] = page;
    }
    if (hasGoods) {
        urlMap['hasGoods'] = hasGoods;
    }
    if (isCOD) {
        urlMap['isCOD'] = isCOD;
    }
    if (filterController.filterSearchWords) {
        urlMap['filterSearchWords'] = filterController.filterSearchWords;
    }
    if (filterController.isRedirect) {
        urlMap['isRedirect'] = filterController.isRedirect;
    }

    return "?" + utilService.mapToUrl(urlMap);
};

filterController.redirect = function (filterID, itemID, order, page, keywords, hasGoods, isCOD, filterSearchWords, isRedirect, isWaterfall) {
    if (!isWaterfall) {
        var url = filterController.getUrl(filterID, itemID, order, page, keywords, hasGoods, isCOD, filterSearchWords, isRedirect);
        location.href = url;
    } else {

        //设置连接的原有参数
        var linkParam = utilService.urlToMap(window.location.search.substring(1));
        for (var p in linkParam) {
            if (!postParams[p])
                postParams[p] = linkParam[p];
        }

        //下一页
        postParams.page++;
        filterService.getResults(postParams, function (data) {
            if (data.contents.length == 0) {
                postParams.page--;
                notifyService.error("到底了！");
                return;
            }


            filterController.data = data;
            filterController.pageItem = data.pageItem;

            $('#filterLoading').hide();
            utilService.render('filterController', filterController, true);
            utilService.render('filterPageController', filterController);

            //设置选中filter
            if (filterController.filters) {
                var filters = unescape(filterController.filters);
                var filterArray = filters.split('&');
                for (var i = 0; i < filterArray.length; i++) {
                    var filter = filterArray[i];
                    filter = filter.replace('=', '_');
                    filterController.setFlag($("#" + filter));
                    var filterID = filter.split('_')[0];
                    var itemID = filter.split('_')[1];
                    $("#filterCrumbs").append($("<a onclick='filterController.removeFilter(" + filterID + "," + itemID + ")' href='javascript:void(0);'><b>" + $("#Filter_" + filterID).attr("filter") + " : </b><em>" + $("#" + filter).html() + "</em><i></i></a>"));
                }
            }
            //设置选中order
            if (filterController.order) {
                var order = unescape(filterController.order);
                filterController.setFlag($("#" + order));
            }
            //设置hasGoods
            if (filterController.hasGoods) {
                var hasGoods = unescape(filterController.hasGoods);
                if (hasGoods == 'true') {
                    $("#ckHasGoods").attr("checked", hasGoods);
                }
                else {
                    $("#ckHasGoods").removeAttr("checked");
                }
            }
        });
    }
};

//取消过滤
filterController.removeFilter = function (filterID, itemID) {
    filterController.filters = filterController.filters.replace(filterID + "=" + itemID, "");
    filterController.pathFilters = utilService.urlToMap(filterController.filters);
    var url = filterController.getUrl();
    location.href = url;
};

//是否有货
filterController.hasGoodsChange = function (ck) {
    if (!!ck) {
        if ($(ck).is(':checked') == true) {
            filterController.redirect(0, 0, '', '', '', 'true', '');
        }
        else {
            filterController.redirect(0, 0, '', '', '', 'false', '');
        }
    }
}

//是否支持货到付款
filterController.isCODChange = function (ck) {
    if (!!ck) {
        if ($(ck).is(':checked') == true) {
            filterController.redirect(0, 0, '', '', '', '', 'true');
        }
        else {
            filterController.redirect(0, 0, '', '', '', '', 'false');
        }
    }
}

String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g, function (m, i) {
        return args[i];
    });
}

//全局搜索
//isRedirect <> false - 异步加载数据，不跳转; true - 不加载数据，直接跳转
filterController.systemFilterSearch = function (isRedirect, filterSearchDataTemplate, searchUrl) {
    var filterSearchWords = $('#filterSearchWords').val();
    if (!filterSearchWords) {
        alert("请输入关键字搜索");
    }
    else {
        if (!isRedirect) {
            filterService.getResults({ filterSearchWords: filterSearchWords, isRedirect: isRedirect }, function (data) {
                data = eval('(' + data + ')');
                if (data.item.length == 0) {
                    $('#filterSearchPannel').append('未找到相关商品');
                }
                $('#filterSearchPannel').html('');
                for (var i = 0; i < data.item.length; i++) {
                    $('#filterSearchPannel').append(filterSearchDataTemplate.format(data.item[i].channelName, data.item[i].navigationUrl, data.item[i].contentCount));
                }
            });
        }
        else {
            top.location.href = searchUrl + '?filterSearchWords=' + encodeURIComponent(filterSearchWords);
        }
    }
}