/// <reference path="../jquery-1.7.2.js"/>
/// <reference path="../jquery-ui-1.8.20.custom.js"/>
/// <reference path="../plugins/jquery.blockUI.js"/>
/// <reference path="../plugins/json2.js"/>
/// <reference path="../plugins/jquery.me-1.0.0.js"/>

//Objects
function Page() {
    this.ProxyService = new ProxyService;
    this.Keyboard = new Keyboard;
    this.WaitingAction = { Open: 0, Close: 1 };
    this.WaitingStatus = { Opened: 0, Closed: 1 };
    this.FormAction = { Clear: 0, Disable: 1, Enable: 2 };
    this.Waiting = this.WaitingStatus.Closed;
    this.SysNameForm = "__5792eb9289944b9d9d85c9ca5a79efc1ebf0dd136e944e6987ac1";
    this.SysNameFormHotKey = "____cde408f023984a778dee6db5becb6fb6";
    this.DateMax = $("#__datemax").val();
}

function Keyboard() {
    this.Keys = {
        Enter: "return",
        Ctrln: "Ctrl+n",
        Ctrle: "Ctrl+e",
        Ctrlq: "Ctrl+q",
        Ctrls: "Ctrl+s",
        Ctrlr: "Ctrl+r",
        Ctrll: "Ctrl+l"
    }
    this.Enabled = $("#" + Page.SysNameFormHotKey).val() == "true" ? true : false;
}
function AutoCompleteOptions() {
    this.TextBoxId = null;
    this.CheckBoxContainerId = null;
    this.CheckBoxId = null;
    this.CheckBoxTemplate = "<span><input type=\"checkbox\" id=\"__ID__\" name=\"__ID__\" value=\"__VALUE__\" class=\"chk chkAutoCompleteUncheck __ID__\" checked=\"checked\" />__LABEL__</span>";
    this.minLength = null;
    this.url = null;
    this.position = { my: "left top", at: "left bottom", collision: "none" };
    this.callback = null;
    this.callbackError = null;
    this.callbackRemoveCheck = null;
    this.CacheEnabled = true;
    this.Watermark = null;
    this.OnlyOne = false;
    this.ShowAll = true;
    this.HiddenId = null;
    this.extra_data = null;
    this.AppendMore = null;
    this.AfterAppend = null;
}
function ScrollOptions() {
    this.unbindAfter = true;
    this.callback = null;
    this.heightToCall = 1207;
    this.params = null;
}
function DialogOptions(request) {
    this.modal = true;
    this.callbackOK = null;
    this.callbackCancel = null;
    this.height = 140;
    this.width = 140;
    this.AutoHeight = false;
    this.AutoWidth = false;
    this.zIndex = 900;
    this.callbackClose = null;

    this.OpenDialog = true;
    this.title = null;

    this.parent = request;
}
function DropDownListOptions(request) {
    this.element = null;
    this.json = null;
    this.selectFirst = null;
    this.parent = request;
}
function DropDownListResult(text, value) {
    this.Text = text;
    this.Value = value;
}
function ProxyService() {
    this.Manager = $.manageAjax.create('ajaxManagerUnique',
    {
        queue: true,
        maxRequests: 2,
        afterCompleteAll: function (request) {
            Page.DoWaiting(request, Page.WaitingAction.Close);
        }
    });
}
function ProxyServiceRequest() {
    this.ReturnTypes = { JSON: 0, HTML: 1, PopUp: 2, DropDownList: 3 };
    this.ContentTypes = { JSON: "application/json", Form: "application/x-www-form-urlencoded" };
    this.Methods = { POST: "POST", GET: "GET" };
    this.CallTypes = { callback: 0, callbackerror: 1 };
    this.WaitingTypes = { Full: 0, Lite: 1 };

    this.waitingElement = null;
    this.waitingType = this.WaitingTypes.Full;
    this.waiting = true;
    this.url = null;
    this.data = null;
    this.callback = null;
    this.callbackError = null;
    this.progress = null;
    this.isLog = false;
    this.contentType = this.ContentTypes.JSON;
    this.timeout = 1800000;
    this.method = this.Methods.POST;
    this.processData = false;
    this.dataType = "text"; // not "json" we'll parse
    this.returnType = this.ReturnTypes.JSON;
    this.DropDownListOptions = null;
    this.DialogOptions = null;
    this.AutoCompleteOptions = null;

    this.response = null;
}
function ProxyServiceResponse() {
    this.request = null;
    this.data = null;

    this.isError = false;
    this.isAlert = false;
    this.Code = null;
    this.Message = null;
    this.SimpleMessage = false;
    this.HtmlMessage = false;
    this.ExceptionMessage = null;
    this.StackTrace = null;
    this.MessageValues = null;
}
//---------------------

//Methods
//DropDownListOptions Methods
DropDownListOptions.prototype.GetData = function () {
    return this.parent.response.data;
}
//DialogOptions Methods
DialogOptions.prototype.GetData = function () {
    return this.parent.response.data;
}
//Keyboard Methods
Keyboard.prototype.Any = function (key, obj, callback) {
    if (!Page.Keyboard.Enabled) { return; }
    jQuery(obj).bind('keydown', key, function (evt) {
        callback();
        return false;
    });
}
Keyboard.prototype.Enter = function (obj, callback) {
    jQuery(obj).bind('keydown', Page.Keyboard.Keys.Enter, function (evt) {
        callback();
        return false;
    });
}
//Page Methods
Page.prototype.AutoComplete = function (options) {
    var Cache_AutoComplete = {};
    var lastXhr;
    var input = $("#" + options.TextBoxId);

    $(input).autocomplete("destroy");

    if (options.Watermark != null) {
        input.Watermark(options.Watermark);
    }

    $(input).autocomplete({
        search: function (event, ui) {
            var ret = true;
            if (options.HiddenId == null) {
                if (options.OnlyOne) {
                    if ($('input[id="' + options.CheckBoxId + '"]').length > 0) {
                        ret = false;
                    }
                }
            }
            return ret;
        },
        source: function (request, response) {
            var term = request.term;
            if (options.extra_data != null) {
                request = options.extra_data;
                request.term = term;
            }
            request.onlyone = options.OnlyOne;
            request.showAll = options.ShowAll;
            request.onlyone = options.HiddenId != null ? true : request.onlyone;
            //if ( term in Cache_AutoComplete ) {
            //    response( Cache_AutoComplete[ term ] );
            //    return;
            //}

            lastXhr = $.getJSON(options.url, request, function (data, status, xhr) {
                //Cache_AutoComplete[ term ] = data;
                //if ( xhr === lastXhr ) {
                response(data);
                //}
            });
        },
        minLength: options.minLength,
        position: options.position,
        focus: function (event, ui) {
        },
        change: function (event, ui) {
            if (options.HiddenId != null && ui.item == null) {
                $("#" + options.HiddenId).val("");
            }
        },
        select: function (event, ui) {
            if (options.HiddenId == null) {
                var arr = new Array();
                if (ui.item.isGroup) {
                    arr = ui.item.Inside;
                }
                else {
                    arr.push(ui.item);
                }

                $.each(arr, function (index, value) {
                    if ($('input[id="' + options.CheckBoxId + '"][value="' + value.Id + '"]').length == 0) {

                        var lblAux = value.label;
                        if (ui.item.isGroup) {
                            lblAux += " - (" + ui.item.value + ")";
                        }
                        var templateAux = options.CheckBoxTemplate;
                        templateAux = templateAux.replace(new RegExp('__ID__', "igm"), options.CheckBoxId);
                        templateAux = templateAux.replace(new RegExp('__VALUE__', "igm"), value.Id);
                        templateAux = templateAux.replace(new RegExp('__LABEL__', "igm"), lblAux);
                        if (options.AppendMore != undefined && options.AppendMore != null) {
                            options.AppendMore(index, value, templateAux);
                        }
                        $("#" + options.CheckBoxContainerId).append(templateAux);
                        if (options.AfterAppend != undefined && options.AfterAppend != null) {
                            options.AfterAppend(index, value);
                        }
                    }
                });

                var input = $("#" + options.TextBoxId).val("");
                $(".chkAutoCompleteUncheck").unbind("click");
                $(".chkAutoCompleteUncheck").click(function () {
                    $(this).parent().remove();
                });
                return false;
            }
            else {
                $("#" + options.HiddenId).val(ui.item.Id);

                if (options.TextBoxDestination != null && ui.item != null)
                    $("#" + options.TextBoxDestination).val(ui.item.group);

                return true;
            }
        }
    });
}
Page.prototype.GridUpdate = function (e) {
    e.preventDefault();
    // find the containing element's id then reload the grid
    var url = $(this).attr('href');
    var grid = $(this).parents('#gridBonus'); // get the grid
    var id = grid.attr('id');
    var urlfull = $(this).attr("href").split("?");
    var urlQS = urlfull[1];

    var request = new ProxyServiceRequest();
    var sort = Page.QueryString("sort", urlQS);
    var sortDir = Page.QueryString("sortdir", urlQS);
    var page = Page.QueryString("page", urlQS);
    request.url = urlfull[0];
    request.data = { page: page, sort: sort, sortDir: sortDir };
    request.returnType = request.ReturnTypes.HTML;

    request.callback = function (data) {
        grid.html(data.response.data);
    }
    Page.ProxyService.Invoke(request);

    //    grid.load(url + ' #' + id);
}
Page.prototype.Message = function (msg) {
    var $alert;
    $alert = $('#upalert');
    $alert.html(message);
    var alerttimer = window.setTimeout(function () {
        $alert.trigger('click');
    }, 5000);
    $alert.animate({ height: $alert.css('line-height') || '50px' }, 200)
        .click(function () {
            window.clearTimeout(alerttimer);
            $alert.animate({ height: '0' }, 400);
        });
}
Page.prototype.ShowError = function (msg) {
    alert(msg);
}
Page.prototype.ShowAlert = function (msg) {
    alert(msg);
}
Page.prototype.ShowMessage = function (msg) {
    alert(msg);
}
Page.prototype.CheckBoxAll = function (obj, nameothers) {
    var checked = $(obj).is(":checked");
    $.each($(".gridbonus"), function (key, value) {
        $(value).attr('checked', checked);
    });
}
Page.prototype.FormExact = function (action, force, arr) {

    $.each(arr, function (key, value) {
        var elements = "";
        if (typeof value == "string") {
            //aux = $("#" + value);
            elements = $("[id=" + value + "]");
        }
        else {
            elements = $(value);
        }

        var total = elements.length;
        for (var i = 0; i < total; i++) {
            var element = $(elements[i]);

            //#region Check Type
            var type = "";
            if (element.hasClass("txt")) {
                type = "txt";
            }
            else if (element.hasClass("chk")) {
                type = "chk";
            }
            else if (element.hasClass("ddl")) {
                type = "ddl";
            }
            else if (element.hasClass("btn")) {
                type = "btn";
            }
            else if (element.hasClass("rdo")) {
                type = "chk";
            }
            else {
                type = "txt";
            }
            //#endregion
            switch (type) {
                case "txt":
                    //#region Textbox & Textarea
                    switch (action) {
                        case 0: //Clear
                            if (force || !element.hasClass("donotclear")) {
                                element.val("");
                            }
                            break;
                        case 1: //Disable
                            if (force || !element.hasClass("donotdisable")) {
                                element.attr("disabled", "disabled");
                                element.addClass("disabled");
                            }
                            break;
                        case 2:
                            if (force || !element.hasClass("donotenable")) {
                                element.removeAttr("disabled");
                                element.removeClass("disabled");
                            }
                            break;
                    }
                    //#endregion
                    break;
                case "chk":
                    //#region Checkbox
                    switch (action) {
                        case 0: //Clear
                            if (force || !element.hasClass("donotclear")) {
                                element.attr('checked', false);
                            }
                            break;
                        case 1: //Disable
                            if (force || !element.hasClass("donotdisable")) {
                                element.attr("disabled", "disabled");
                                element.addClass("disabled");
                            }
                            break;
                        case 2:
                            if (force || !element.hasClass("donotenable")) {
                                element.removeAttr("disabled");
                                element.removeClass("disabled");
                            }
                            break;
                    }
                    //#endregion
                    break;
                case "ddl":
                    //#region Dropdownlist
                    switch (action) {
                        case 0: //Clear
                            if (force || !element.hasClass("donotclear")) {
                                element.val(-1);
                            }
                            break;
                        case 1: //Disable
                            if (force || !element.hasClass("donotdisable")) {
                                element.attr("disabled", "disabled");
                                element.addClass("disabled");
                            }
                            break;
                        case 2:
                            if (force || !element.hasClass("donotenable")) {
                                element.removeAttr("disabled");
                                element.removeClass("disabled");
                            }
                            break;
                    }
                    //#endregion
                    break;
                case "btn":
                    //#region Button
                    switch (action) {
                        case 0: //Clear
                            break;
                        case 1: //Disable
                            if (force || !element.hasClass("donotdisable")) {
                                element.attr("disabled", "disabled");
                                element.addClass("noclick");
                            }
                            break;
                        case 2:
                            if (force || !element.hasClass("donotenable")) {
                                element.removeAttr("disabled");
                                element.removeClass("noclick");
                            }
                            break;
                    }
                    //#endregion
                    break;
            }
        }
    });
}
Page.prototype.Form = function (root, action) {
    if (root == null || root == undefined) { root = $(document); }
    var dom = null;

    //#region TextBox and TextArea
    dom = $(root).find(".txt");
    Page.FormExact(action, false, dom);
    dom = $(root).find(".txta");
    Page.FormExact(action, false, dom);
    //#endregion

    //#region CheckBox
    dom = $(root).find(".chk");
    Page.FormExact(action, false, dom);
    //#endregion

    //#region DropDownList
    dom = $(root).find(".ddl");
    Page.FormExact(action, false, dom);
    //#endregion

    //#region button
    dom = $(root).find(".btn");
    Page.FormExact(action, false, dom);
    //#endregion
}

Page.prototype.ScrollTo = function (obj) {
    if (typeof obj == "number") {
        $('html, body').scrollTop(obj);
    }
    else {
        if (obj != null && obj != undefined) {
            $('html, body').scrollTop(obj.offset().top);
        }
    }
}
Page.prototype.ScrollUnbind = function () {
    $(window).unbind("scroll");
}
Page.prototype.Scroll = function (options) {
    //var options = new ScrollOptions();
    $(window).scroll(function () {
        if ($(window).scrollTop() == 0) {
            //TOP
        }
        else if ($(window).height() + $(window).scrollTop() >= $(document).height() - options.heightToCall) {
            if (options.callback != null && options.callback != undefined) {
                options.callback(options.params);
            }

            if (options.unbindAfter) {
                Page.ScrollUnbind();
            }
        }
    });
}
Page.prototype.UniqueCheckBox = function (className, remove) {
    if (remove == null || remove == undefined) { remove = false; }
    var $unique = $('.' + className);
    if (remove) {
        $.each($unique, function (key, value) {
            $(value).unbind('click');
        });
    }
    else {
        $unique.click(function () {
            $unique.filter(':checked').not(this).removeAttr('checked');
        });
    }
}
Page.prototype.GetSys = function () {
    return $("#" + Page.SysNameForm).val();
}
Page.prototype.AppendUser = function (form) {
    $("<input type='hidden'>")
                    .attr("name", Page.SysNameForm)
                    .attr("value", Page.GetSys())
                    .appendTo(form);
}

Page.prototype.TransferTo = function (address, isNewWindow) {
    Page.DoWaiting(new ProxyServiceRequest(), Page.WaitingAction.Open);
    var $div = $("<div>").css("display", "none");
    var formAux = "<form method='POST'" + (isNewWindow ? " target='_blank' " : "") + ">"
    var $form = $(formAux).attr("action", address);
    Page.AppendUser($form);
    $form.appendTo($div);
    $div.appendTo("body");
    $form.submit();
    //    window.location = address;
}
Page.prototype.Transfer = function (address) {
    Page.TransferTo(address, false);
}
Page.prototype.QueryString = function (key, url) {
    var ret = "";
    index = 0;

    var element = "";
    var uri = (url == undefined || url == null) ? window.location.search.substring(1).split("&") : url.substring(0).split("&");

    key = key.toLowerCase();

    for (var i = 0, leng = uri.length; i < leng; i++) {
        element = uri[i].split("=");
        if (element[0].toLowerCase() == key) {
            ret = element[1];
            break;
        }
    }
    return ret;
}
Page.prototype.DialogClose = function (name) {
    if (name == null || name == undefined) {
        name = "dialog-modal";
    }
    $("#" + name).dialog("close");
}
Page.prototype.Dialog = function (request) {
    //$("#dialog-modal").dialog("destroy");
    $("#dialog-modal").attr("title", request.DialogOptions.title);
    $("#dialog-modal").empty();
    $("#dialog-modal").html(request.DialogOptions.GetData());


    $("#dialog-modal").dialog({
        height: request.DialogOptions.AutoHeight ? 'auto' : request.DialogOptions.height,
        width: request.DialogOptions.AutoWidth ? 'auto' : request.DialogOptions.width,
        modal: request.DialogOptions.modal,
        close: request.DialogOptions.callbackClose,
        zIndex: request.DialogOptions.zIndex
    });
    if (request.DialogOptions.callbackOK != null && request.DialogOptions.callbackOK != undefined) {
        request.DialogOptions.callbackOK(request);
    }
}
Page.prototype.DropDownListClear = function (ddl) {
    var ddl = new DropDownListOptions();
    if (ddl.element != null) {
        ddl.element.children().remove().end();
    }
    if (ddl.selectFirst != null) {
        ddl.element.append("<option value='" + ddl.selectFirst.Value + "'>" + ddl.selectFirst.Text + "</option>");
    }
}
Page.prototype.DropDownList = function (request) {
    var ddl = request.DropDownListOptions;
    //var ddl = new DropDownListOptions();
    //options: { Text: "Selecione uma cidade", Value: -1 }
    if (typeof ddl.GetData() == "string") {
        ddl.json = JSON.parse(ddl.GetData());
    }
    else {
        ddl.json = ddl.GetData();
    }
    ddl.element.children().remove().end();
    if (ddl.selectFirst != null) {
        ddl.element.append("<option value='" + ddl.selectFirst.Value + "'>" + ddl.selectFirst.Text + "</option>");
    }
    $.each(ddl.json, function (val, text) {
        ddl.element.append("<option value='" + text.Value + "'>" + text.Text + "</option>");
    });
}
Page.prototype.DoWaiting = function (request, type) {
    //    var request = new ProxyServiceRequest();
    if (request.waiting) {
        switch (type) {
            case 0: //Page.WaitingAction.Open
                if (Page.Waiting == Page.WaitingStatus.Opened && request.waitingElement == null) { return; }

                if (request.waitingType == request.WaitingTypes.Full) {
                    $.blockUI({ message: $("#waitingFull").html(), css: { border: 'none', padding: '15px', backgroundColor: '#000', '-webkit-border-radius': '10px', '-moz-border-radius': '10px', opacity: .5, color: '#fff' } });
                }
                else {
                    request.waitingElement.block({ message: $("#waitingLite").html(), css: { border: 'none', padding: '15px', backgroundColor: '#000', '-webkit-border-radius': '10px', '-moz-border-radius': '10px', opacity: .5, color: '#fff' } });
                }
                if (request.waitingElement == null) { Page.Waiting = Page.WaitingStatus.Opened; }
                break;
            case 1: //a
                if (Page.Waiting == Page.WaitingStatus.Closed && request.waitingElement == null) { return; }

                if (request.waitingType == request.WaitingTypes.Full) {
                    $.unblockUI();
                }
                else {
                    request.waitingElement.unblock();
                }
                if (request.waitingElement == null) { Page.Waiting = Page.WaitingStatus.Closed; }
                break;
        }
    }
}

//ProxyServiceRequest Methods
ProxyServiceRequest.prototype.DataParsed = function () {
    var ret = null;
    try { ret = JSON.parse(this.data); } catch (e) { }
    return ret;
}
ProxyServiceRequest.prototype.Call = function (type) {
    var ret = false;
    switch (type) {
        case 0: //callback
            switch (this.returnType) {
                case 0: //JSON
                    if (this.callback != null && this.callback != undefined) {
                        ret = true;
                        this.callback(this);
                    }
                    break;
                case 1: //HTML
                    if (this.callback != null && this.callback != undefined) {
                        ret = true;
                        this.callback(this);
                    }
                    break;
                case 2: //PopUp
                    if (this.DialogOptions.OpenDialog) {
                        Page.Dialog(this);
                    }
                    if (this.callback != null && this.callback != undefined) {
                        ret = true;
                        this.callback(this);
                    }
                    break;
                case 3: //DropDownList
                    Page.DropDownList(this);
                    if (this.callback != null && this.callback != undefined) {
                        ret = true;
                        this.callback(this);
                    }
                    break;
            }
            break;
        case 1: //callbackerror
            if (this.callbackError != null && this.callbackError != undefined) {
                ret = true;
                this.callbackError(this);
            }
            break;
    }
    return ret;
}

//ProxyService Methods
ProxyService.prototype.ConvertResponse = function (args) {
    var ret = new ProxyServiceResponse();
    var parsed = "";
    try { parsed = JSON.parse(args); } catch (e) { parsed = null; }
    if (!parsed || parsed == null || parsed == undefined) {
        ret.data = args;
        return ret;
    }
    if (parsed != null && parsed != undefined) {
        ret.data = (parsed.Data != null && parsed.Data != undefined) ? parsed.Data : ret.data;
        ret.isError = (parsed.isError != null && parsed.isError != undefined) ? parsed.isError : ret.isError;
        ret.isAlert = (parsed.isAlert != null && parsed.isAlert != undefined) ? parsed.isAlert : ret.isAlert;
        ret.Code = (parsed.Code != null && parsed.Code != undefined) ? parsed.Code : ret.Code;
        ret.Message = (parsed.Message != null && parsed.Message != undefined) ? parsed.Message : ret.Message;
        ret.SimpleMessage = (parsed.SimpleMessage != null && parsed.SimpleMessage != undefined) ? parsed.SimpleMessage : ret.SimpleMessage;
        ret.HtmlMessage = (parsed.HtmlMessage != null && parsed.HtmlMessage != undefined) ? parsed.HtmlMessage : ret.HtmlMessage;
        ret.ExceptionMessage = (parsed.ExceptionMessage != null && parsed.ExceptionMessage != undefined) ? parsed.ExceptionMessage : ret.ExceptionMessage;
        ret.StackTrace = (parsed.StackTrace != null && parsed.StackTrace != undefined) ? parsed.StackTrace : ret.StackTrace;
        ret.MessageValues = (parsed.MessageValues != null && parsed.MessageValues != undefined) ? parsed.MessageValues : ret.MessageValues;
    }
    return ret;
}
ProxyService.prototype.DoError = function (error) {
    var ret = false;
    if (error != null) {
        if (error.isError || error.isAlert) {
            ret = true;
            var msg = eval(error.Code);
            if (error.MessageValues != null && error.MessageValues != undefined) {
                $.each(error.MessageValues, function (index, val) {
                    msg = msg.replace("{" + index + "}", val);
                });
            }
            if (error.SimpleMessage) {
                alert(msg);
                if (error.Code == "M_00023") {
                    window.location = "/entrar";
                }
            }
            else if (error.HtmlMessage) {
                if (error.isError) {
                    Page.ShowError(msg, error);
                }
                else if (error.isAlert) {
                    Page.ShowAlert(msg, error);
                }
            }
        }
    }

    return ret;
}
ProxyService.prototype.ConvertRequest = function (args) {
    ///<summary>Convert para o Objeto Request</summary>
    ///<param name="args">JSON contendo os objetos que deseja passar para o metodo Invoke</param>
    ///<returns type="ProxyServiceRequest" />
    var ret = new ProxyServiceRequest();
    if (args != null && args != undefined) {
        ret.callback = (args.callback != null && args.callback != undefined) ? args.callback : ret.callback;
        ret.callbackError = (args.callbackError != null && args.callbackError != undefined) ? args.callbackError : ret.callbackError;
        ret.data = (args.data != null && args.data != undefined) ? args.data : ret.data;
        ret.isLog = (args.isLog != null && args.isLog != undefined) ? args.isLog : ret.isLog;
        ret.progress = (args.progress != null && args.progress != undefined) ? args.progress : ret.progress;
        ret.returnType = (args.returnType != null && args.returnType != undefined) ? args.returnType : ret.returnType;
        ret.url = (args.url != null && args.url != undefined) ? args.url : ret.url;
        ret.waiting = (args.waiting != null && args.waiting != undefined) ? args.waiting : ret.waiting;
        ret.contentType = (args.contentType != null && args.contentType != undefined) ? args.contentType : ret.contentType;
        ret.timeout = (args.timeout != null && args.timeout != undefined) ? args.timeout : ret.timeout;
        ret.method = (args.method != null && args.method != undefined) ? args.method : ret.method;
        ret.processData = (args.processData != null && args.processData != undefined) ? args.processData : ret.processData;
        ret.dataType = (args.dataType != null && args.dataType != undefined) ? args.dataType : ret.dataType;
        ret.waitingType = (args.waitingType != null && args.waitingType != undefined) ? args.waitingType : ret.waitingType;
    }
    return ret;
}
ProxyService.prototype.InvokePost = function (e) {
    var form = $(this);

    Page.AppendUser(form);

    var request = new ProxyServiceRequest();
    request.method = request.Methods.POST;
    request.returnType = request.ReturnTypes.HTML;
    request.data = form.serialize();
    request.url = form.attr("action");
    request.contentType = request.ContentTypes.Form;
    request.callback = e.data.success;
    request.callbackError = e.data.error;

    Page.ProxyService.Invoke(request);
    e.preventDefault();
}
//url, data, returnType, callback, callbackError, isLog, progressFunction
ProxyService.prototype.Invoke = function (args) {
    var request = null;
    if (args instanceof ProxyServiceRequest) {
        request = args;
    }
    else {
        var request = new ProxyServiceRequest();
        request = Page.ProxyService.ConvertRequest(args);
    }


    if (!request.isLog) {
        var callerInformation = "";
        var pageName = "";

        try {
            callerInformation = arguments.callee.caller;
        } catch (e) { }
        try {
            pageName = window.location.pathname;
        } catch (e) { }
    }
    if (request.data != null) {
        request.data.SysAux = Page.GetSys();
    }

    if (request.contentType == request.ContentTypes.JSON) {
        var seen = [];
        var jsonData = JSON.stringify(request.data, function (key, val) {
            if (typeof val == "object") {
                if ($.inArray(val, seen) >= 0)
                    return undefined;
                seen.push(val);
            }
            return val
        });

        jsonData = "{ request: " + jsonData + " }";
    }
    else {
        var jsonData = request.data;
    }


    var ajaxOptions = {
        internalRequest: request,
        url: request.url,
        data: jsonData,
        type: request.method,
        cache: false,
        processData: request.processData,
        contentType: request.contentType,
        timeout: request.timeout,
        dataType: request.dataType,
        beforeSend: function () {
            Page.DoWaiting(request, Page.WaitingAction.Open);
        },
        success:
            function (data, textStatus, jqXHR) {
                request.response = Page.ProxyService.ConvertResponse(data);

                if (Page.ProxyService.DoError(request.response)) {
                    request.Call(request.CallTypes.callbackerror);
                }
                else {
                    request.Call(request.CallTypes.callback);
                }

            },
        error:
            function (xhr, textStatus) {
                if (Page.ProxyService.DoError(xhr.responseText)) {

                    return;
                }
                request.Call(request.CallTypes.callbackerror);
                //                Page.DoWaiting(request, Page.WaitingAction.Close);
            }
    };
    Page.ProxyService.Manager.add(ajaxOptions);

    //    var ajax = $.ajax({
    //        url: request.url,
    //        data: jsonData,
    //        type: request.method,
    //        processData: request.processData,
    //        contentType: request.contentType,
    //        timeout: request.timeout,
    //        dataType: request.dataType,
    //        beforeSend: function () {
    //            Page.DoWaiting(request, Page.WaitingAction.Open);
    //        },
    //        success:
    //            function (data) {
    //                Page.DoWaiting(request, Page.WaitingAction.Close);
    //                request.response = Page.ProxyService.ConvertResponse(data);

    //                if (Page.ProxyService.DoError(request.response)) {
    //                    request.Call(request.CallTypes.callbackerror);
    //                }
    //                else {
    //                    request.Call(request.CallTypes.callback);
    //                }

    //            },
    //        error:
    //            function (xhr, textStatus) {
    //                Page.DoWaiting(request, Page.WaitingAction.Close);
    //                if (Page.ProxyService.DoError(xhr.responseText)) {

    //                    return;
    //                }
    //                request.Call(request.CallTypes.callbackerror);
    //            }
    //    });
}

var Page = new Page();

$(document).ready(function () {
    Page.DateMax = DateTime.stringToDate($("#__datemax").val());

    $.widget("custom.catcomplete", $.ui.autocomplete, {
        _renderMenu: function (ul, items) {
            var self = this,
				currentCategory = "";
            $.each(items, function (index, item) {
                if (item.category != currentCategory) {
                    if (item.category != null && item.category != "null") {
                        ul.append("<li class='ui-menu-item' role='menuitem'><a class='ui-corner-all' tabindex='-1'>" + item.category + "</a></li>");
                        //ul.append("<li class='ui-autocomplete-cat'>" + item.category + "</li>");
                        currentCategory = item.category;
                    }
                }

                self._renderItem(ul, item);
            });
        }
    });
});

function TestWaiting() {
    var request = new ProxyServiceRequest();
    Page.DoWaiting(request, Page.WaitingAction.Open);
}
function TestWaitingClose() {
    var request = new ProxyServiceRequest();
    Page.DoWaiting(request, Page.WaitingAction.Close);
}

function Example_HTMLLL(t) {
    var request = new ProxyServiceRequest();
    request.url = "/Home/List";
    request.data = { ok: t };
    request.returnType = request.ReturnTypes.HTML;
    request.callback = Example_HTML_callback;

    Page.ProxyService.Invoke(request);
}
function Example_HTMLLL2(t) {
    var request = new ProxyServiceRequest();
    request.url = "/Home/List4";
    request.data = { ok: t };
    request.returnType = request.ReturnTypes.HTML;
    request.callback = Example_HTML_callback;

    Page.ProxyService.Invoke(request);
}


//Examples Methods
function Example_Waiting(tipo) {
    var request = new ProxyServiceRequest();
    if (tipo) {
        Page.DoWaiting(request, Page.WaitingAction.Open);
    }
    else {
        Page.DoWaiting(request, Page.WaitingAction.Close);
    }
}
function aa(message) {

}
function Example_Alert(msg) {
    if (msg == undefined) { msg = "Testando msg!"; }
    $(document).bar({ message: 'Olá vc!' });
}
function Example_Dialog() {
    var request = new ProxyServiceRequest();
    request.returnType = request.ReturnTypes.PopUp;
    request.url = "/Home/List3";

    request.DialogOptions = new DialogOptions(request);
    request.DialogOptions.title = "teste";

    Page.ProxyService.Invoke(request);
}
function Example_HTML() {
    var request = new ProxyServiceRequest();
    request.url = "/Home/List";
    request.data = { ok: "teste" };
    request.returnType = request.ReturnTypes.HTML;
    request.callback = Example_HTML_callback;

    Page.ProxyService.Invoke(request);
}
function Example_HTML_callback(request) {
    var ok = "";
    $("#ttt2").html(request.response.data);
}
function Example_JSON() {
    var request = new ProxyServiceRequest();
    request.url = "/Home/List2";
    request.returnType = request.ReturnTypes.JSON;
    request.callback = function (data) {
        var ok = "";
    };

    Page.ProxyService.Invoke(request);
}
function Example_DropDownList() {
    var request = new ProxyServiceRequest();
    request.url = "/Home/List2";

    request.returnType = request.ReturnTypes.DropDownList;
    request.DropDownListOptions = new DropDownListOptions(request);
    request.DropDownListOptions.element = $("#ttt");
    request.DropDownListOptions.selectFirst = new DropDownListResult("Selecione...", "0");

    Page.ProxyService.Invoke(request);
}

String.prototype.format = String.prototype.f = function () {
    var s = this,
        i = arguments.length;

    while (i--) {
        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
    }
    return s;
};
