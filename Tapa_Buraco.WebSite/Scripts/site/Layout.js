var url_Base_Site = "";

$(document).ready(function () 
{
    url_Base_Site = $('#url_Base_Site').data('request-url');

    $("#buttonLogOut").show();
    $("#buttonLogOut").click(function () {
        window.location = url_Base_Site + "/Login/Index";
    });

    var optionsDatePicker = {
        autoclose: true,
        showButtonPanel: true,
        maxDate: '+5y',
        numberOfMonths: 1,
        dateFormat: 'dd/mm/yy',
        showAnim: '',
        closeText: 'Fechar',
        currentText: 'Mês Atual',
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro']
    };

    if ($(".datepicker")[0]) {
        $(".datepicker").datepicker(optionsDatePicker);
        $(".datepicker").mask("99/99/9999", {
            completed: function () {
            }
        });
    }

    if ($(".timeClock")[0]) {
        $(".timeClock").mask("99:99:99", {
            completed: function () {
            }
        });
    }

    var config = {
        '.chosen-select': { no_results_text: 'Não foi possível encontrar ', allow_single_deselect: true, disable_search_threshold: 100 },
        '.chosen-select-deselect': { allow_single_deselect: true },
        '.chosen-select-no-single': { disable_search_threshold: 10 },
        '.chosen-select-no-results': { no_results_text: 'Sem registros!' },
        '.chosen-select-width': { width: "95%" }
    }


    for (var selector in config) {
        if ($(selector)[0]) {
            $(selector).chosen(config[selector]);
        }
    }

    if ($(".double")[0])
        $(".double").maskMoney({ symbol: 'R$ ', showSymbol: false, thousands: '.', decimal: ',', symbolStay: false, precision: 0 });

    if ($(".integer")[0])
        $(".integer").maskMoney({ thousands: '.', decimal: ',', precision: 0 });

    if ($(".onlyInteger")[0])
        $(".onlyInteger").maskMoney({ thousands: '', decimal: ',', precision: 0 });
});

function ShowMessage(message, typeMessage) {
    window.scrollTo(0, 0); //scroll to top

    $("#divAlert").html("");
    if (typeMessage > 0 && message != null && message != undefined && message != "") {
        var alertMessage = "";

        switch (typeMessage) {
            case 1: alertMessage = "alert alert-success";
                break;
            case 2: alertMessage = "alert alert-info";
                break;
            case 3: alertMessage = "alert alert-warning";
                break;
            case 4: alertMessage = "alert alert-danger";
                break;

        }

        $("#divAlert").html($("#divAlert").html() + "<div class=\"" + alertMessage + "\" onclick=\"HideMessage(this) \">" + message + "</div>");

        $("#divAlert").fadeOut(0);
        $("#divAlert").fadeIn(1000);
        $("#divAlert").fadeOut(3000);
        //$("#divAlert").fadeIn(2000);
        //$("#divAlert").fadeOut(10000);
    }
}

function HideMessage(message) {
    $(message).fadeOut(1500);
}


function ReplaceAll(string, token, newtoken) {
    while (string.indexOf(token) != -1) {
        string = string.replace(token, newtoken);
    }
    return string;
}