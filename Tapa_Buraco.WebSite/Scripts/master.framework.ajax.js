(function (Page) {
    "use strict";
    var Queue = $.manageAjax.create('ajaxManagerUnique',
        {
            queue: true,
            maxRequests: 2,
            afterCompleteAll: function (request) {
                if (Page.AjaxService.AfterCompleteAll != null) Page.AjaxService.AfterCompleteAll(request);
            }
        });

    function ConvertToJSON (data, paramRequestName) {
        var seen = [];
        var jsonData = JSON.stringify(data, function (key, val) {
            if (typeof val == "object") {
                if ($.inArray(val, seen) >= 0)
                    return undefined;
                seen.push(val);
            }
            return val
        });
        if (paramRequestName == "" || paramRequestName === undefined) paramRequestName = "request";
        return "{ " + paramRequestName + ": " + jsonData + " }";
    };

    var ReturnType = { JSON: 0, HTML: 1 };
    var ContentType = { JSON: 'application/json', Form: 'application/x-www-form-urlencoded' };
    var Methods = { Post: 'POST', Get: 'GET' };
   
    Page.AjaxService = (function () {

        var AfterCompleteAll = null;
        var BeforeSend = null;

        var requestDefaults = {
            contentType: ContentType.JSON,
            returnType: ReturnType.JSON,
            methods: Methods.Post,
            requestName: '',
            response: null
        };

        function request(url, data, callback, callbackError, options) {
            if (options === undefined) options = {};
            var localOptions = $.extend({}, requestDefaults, options);

            var requestData = localOptions.contentType == ContentType.JSON ? ConvertToJSON(data, localOptions.requestName) : data;

            var ajaxOptions = {
                url: url,
                data: requestData,
                type: localOptions.methods,
                cache: false,
                processData: false,
                contentType: localOptions.contentType,
                timeout: 1800000,
                dataType: "text",
                beforeSend:
                    function (data) {
                        if (Page.AjaxService.BeforeSend != null) Page.AjaxService.BeforeSend();
                    },
                success:
                    function (data, textStatus, jqXHR) {
                        var dataParsed = (localOptions.returnType == ReturnType.JSON ? JSON.parse(data) : data);
                        if (callback != null) callback(dataParsed);
                    },
                error:
                    function (xhr, textStatus) {
                        if (callbackError != null) callbackError(xhr.responseText);
                    }
            };
            Queue.add(ajaxOptions);
        }

        return {
            Request: request
        };
    })();

    Page.DropDown = (function () {
        function cascade(obj, data, callback, callbackError) {

        }
        return { Cascade: cascade };
    })();
})(Page);