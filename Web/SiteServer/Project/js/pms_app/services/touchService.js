app.factory("touchService", function ($resource) {
     return $resource(
         "/api_pms/touch/:Id?leadID=" + $pageInfo.leadID + "&orderID=" + $pageInfo.orderID,
         {Id: "@Id" },
         { "update": {method:"PUT", isArray: false} }
    );
});