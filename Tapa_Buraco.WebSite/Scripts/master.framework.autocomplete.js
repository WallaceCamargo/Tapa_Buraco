//#region AutoComplete
(function (Page) {
	"use strict";

	Page.AutoComplete = (function () {


		var optionsDefaults = {
			TextBoxId : null,
			CheckBoxContainerId : null,
			CheckBoxId : null,
			CheckBoxTemplate : "<span><input type=\"checkbox\" id=\"__ID__\" name=\"__ID__\" value=\"__VALUE__\" class=\"chk chkAutoCompleteUncheck __ID__\" checked=\"checked\" />__LABEL__</span>",
			minLength : null,
			url : null,
			position : { my: "left top", at: "left bottom", collision: "none" },
			callback : null,
			callbackError : null,
			callbackRemoveCheck : null,
			CacheEnabled : true,
			Watermark : null,
			OnlyOne : false,
			ShowAll : true,
			HiddenId : null,
			extra_data : null,
			AppendMore : null,
			AfterAppend : null,
			Search : null
		}

		function exec (options) {

			if (options === undefined) options = {};
			var localOptions = $.extend({}, optionsDefaults, options);

			

			var Cache_AutoComplete = {};
			var lastXhr;
			var input = $("#" + localOptions.TextBoxId);

			try {
				$(input).autocomplete("destroy");
			} catch (e) {

			}

			if (localOptions.Watermark != null) {
				input.Watermark(localOptions.Watermark);
			}

			$(input).autocomplete({
				search: function (event, ui) {

					var ret = true;

					if (localOptions.Search != null) {
						ret = localOptions.Search(event, ui);
					}
					else {

						if (localOptions.HiddenId == null) {
							if (localOptions.OnlyOne) {
								if ($('input[id="' + localOptions.CheckBoxId + '"]').length > 0) {
									ret = false;
								}
							}
						}
					}
					return ret;
				},
				source: function (request, response) {
					var term = request.term;

					if (localOptions.extra_data != null) {

						////add 12/01/2014
						//var option_extra_data = options["extra_data"];
						//var option_extra_data_new ={};
						//for (name in option_extra_data) {
						//	option_extra_data_new[name] = $("#" + option_extra_data[name]).val();							
						//}

						//request = option_extra_data_new;

						request = localOptions.extra_data;
						request.term = term;
					}
					request.onlyone = localOptions.OnlyOne;
					request.showAll = localOptions.ShowAll;
					request.onlyone = localOptions.HiddenId != null ? true : request.onlyone;
					//if ( term in Cache_AutoComplete ) {
					//    response( Cache_AutoComplete[ term ] );
					//    return;
					//}

					lastXhr = Page.AjaxService.Request(localOptions.url, request, function (data) {
						response(data);
					});
					//lastXhr = $.getJSON(options.url, request, function (data, status, xhr) {
					//    //Cache_AutoComplete[ term ] = data;
					//    //if ( xhr === lastXhr ) {
					//    response(data);
					//    //}
					//});
				},
				minLength: localOptions.minLength,
				position: localOptions.position,
				focus: function (event, ui) {
				},
				change: function (event, ui) {
					if (localOptions.HiddenId != null && ui.item == null) {
						$("#" + localOptions.HiddenId).val("");
					}
				},
				select: function (event, ui) {
					if (localOptions.HiddenId == null) {
						var arr = new Array();
						if (ui.item.isGroup) {
							arr = ui.item.Inside;
						}
						else {
							arr.push(ui.item);
						}

						$.each(arr, function (index, value) {
							if ($('input[id="' + localOptions.CheckBoxId + '"][value="' + value.Id + '"]').length == 0) {

								var lblAux = value.label;
								if (ui.item.isGroup) {
									lblAux += " - (" + ui.item.value + ")";
								}
								var templateAux = localOptions.CheckBoxTemplate;
								templateAux = templateAux.replace(new RegExp('__ID__', "igm"), localOptions.CheckBoxId);
								templateAux = templateAux.replace(new RegExp('__VALUE__', "igm"), value.Id);
								templateAux = templateAux.replace(new RegExp('__LABEL__', "igm"), lblAux);
								if (localOptions.AppendMore != undefined && localOptions.AppendMore != null) {
									localOptions.AppendMore(index, value, templateAux);
								}
								$("#" + localOptions.CheckBoxContainerId).append(templateAux);
								if (localOptions.AfterAppend != undefined && localOptions.AfterAppend != null) {
									localOptions.AfterAppend(index, value);
								}
							}
						});

						var input = $("#" + localOptions.TextBoxId).val("");
						$(".chkAutoCompleteUncheck").unbind("click");
						$(".chkAutoCompleteUncheck").click(function () {
							$(this).parent().remove();
						});
						return false;
					}
					else {
						$("#" + localOptions.HiddenId).val(ui.item.Id);

						if (localOptions.AfterAppend != undefined && localOptions.AfterAppend != null) {
							localOptions.AfterAppend(ui.item.Id, ui.item.label);
						}

						return true;
					}
				}
			});
		}


		return {

			Exec:exec
		};

	})();


})(Page);

////#endregion