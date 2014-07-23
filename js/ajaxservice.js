// Funcion para conseguir todos los productos y los trabaja en el index.js como OrdersApp

(function (OrdersApp) {
    "use strict";
       var serviceBase = '/Product/',
        getSvcUrl = function (method) { return serviceBase + method; };

    OrdersApp.ajaxService = (function () {
        var ajaxGetJson = function (method, jsonIn, callback) {
            $.ajax({
                url: getSvcUrl(method),
                type: "GET",
                data: ko.toJSON(jsonIn),
                dataType: "json",
                contentType: "application/json",
                success: function (json) {
                    callback(json);
                }
            });
        }
        
        return {
            ajaxGetJson: ajaxGetJson
        };
    })();
}(OrdersApp));