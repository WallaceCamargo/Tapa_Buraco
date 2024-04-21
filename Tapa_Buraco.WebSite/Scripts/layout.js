var selectedNav;

$(document).ready(function () {

    $("#liNav01").click(function () {
        $('#carousel-dashboard').carousel('pause');

        if ($("#liNav01").attr("class") == "active") {
            selectedNav = 0;
            $("#liNav01").removeClass("active");
            $("#liNav02").addClass("active");
            $("#liNav03").addClass("active");
            $("#liNav04").addClass("active");
        }
    });

    $("#liNav02").click(function () {
        $('#carousel-dashboard').carousel('pause');

        if ($("#liNav02").attr("class") == "active") {
            selectedNav = 1;
            $("#liNav01").addClass("active");
            $("#liNav02").removeClass("active");
            $("#liNav03").addClass("active");
            $("#liNav04").addClass("active");
        }
    });

    $("#liNav03").click(function () {
        $('#carousel-dashboard').carousel('pause');

        if ($("#liNav03").attr("class") == "active") {
            selectedNav = 2;
            $("#liNav01").addClass("active");
            $("#liNav02").addClass("active");
            $("#liNav03").removeClass("active");
            $("#liNav04").addClass("active");
        }
    });

    $("#liNav04").click(function () {
        $('#carousel-dashboard').carousel();
    });

    $('#carousel-dashboard').on('slide.bs.carousel', function () {
        changeActiveNav();
    });

    selectedNav = 0;
    $("#liNav01").removeClass("active");
    $("#liNav02").addClass("active");
    $("#liNav03").addClass("active");
    $("#liNav04").addClass("active");

    $('#carousel-dashboard').carousel();
    LoadErrors();
});

function changeActiveNav() {
    
    $("#liNav01").addClass("active");
    $("#liNav02").addClass("active");
    $("#liNav03").addClass("active");
    $("#liNav04").addClass("active");

    switch (selectedNav) {
        case 0: selectedNav = 1;
            $("#liNav01").removeClass("active");
            break;
        case 1: selectedNav = 2;
            $("#liNav02").removeClass("active");
            break;
        case 2: selectedNav = 0;
            $("#liNav03").removeClass("active");
            break;
        default:
            selectedNav = 0;
            $("#liNav01").removeClass("active");
            break;
    }
}

function LoadErrors() {
    Page.AjaxService.Request('/Error/LoadErrors', null, LoadErrorsCallBack, CallBackError);
}

function LoadErrorsCallBack(obj) {

    if (obj.TotalRecordCount > 0)
        $("#countErrors").show();
    else
        $("#countErrors").hide();

    $("#divErrorsNotification").html(obj.Records);    
    $("#countErrors").html(obj.TotalRecordCount);
}

function CallBackError(obj) {
    alert('Error: ' + obj.Message);
}