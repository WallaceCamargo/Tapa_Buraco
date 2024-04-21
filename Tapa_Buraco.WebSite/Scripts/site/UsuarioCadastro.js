var url_Base_Site = "";
var idSetor = 0;

$(document).ready(function () {

    url_Base_Site = $('#url_Base_Site').data('request-url');

    LoadSelectSetor();
    CarregarUsuario();

    $(".actionChange").change(function () {
        idSetor = $("#selectSetor").find('option:selected').val();
    });

    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
        disabledCheckboxClass: 'icheckbox_square-green.checked.disabled',
        disabledRadioClass: 'iradio_square-green.checked.disabled',
        disabledClass: 'icheckbox_square-green.disabled',
    });


    $('#buttonAdicionaUsuarioCancelar').click(function () {
        if (confirm('Todas as alterações não salvas serão perdidas.\rDeseja continuar mesmo assim?')) {
            window.location = url_Base_Site + "/Usuario/Index";
        }
    });

    $('#buttonAdicionaUsuarioConfirmar').click(function () {
        SalvarUsuario();
    });
});


function SalvarUsuario() {

    var mensagemValidacao = ValidarCampos();
    var ativo = 1;
    if ($("#chkAtivo").is(":checked")) { ativo = 1 } else { ativo = 0 };
    var admin = 0;
    if ($("#chkAdmin").is(":checked")) { admin = 1 } else { admin = 0 };
    if (mensagemValidacao == "") {

        var config = {
            ID: $('#hiddenId').val() > 0 ? $('#hiddenId').val() : 0,
            NOME: $("#textBoxNome").val().toUpperCase(),
            LOGIN: $("#textBoxLogin").val().toLowerCase(),
            EMAIL: $("#textBoxEmail").val().toLowerCase(),
            ATIVO: ativo,
            ADMIN: admin,
        };

        var url = "";

        if ($('#hiddenId').val() > 0) {
            url = '/Usuario/Put'
        } else {
            url = '/Usuario/Post'
        }

        $.ajax({
            url: url,
            method: "POST",
            data: { usuario: config },
            success: function (data) {
                if (data.Result == "OK") {
                    ShowMessage(data.Message, 1);
                    setTimeout(() => {
                        window.location = url_Base_Site + "/Usuario/Index";
                    }, 2000);
                }
                else {
                    ShowMessage(data.Message, 4);
                }
            },
            error: function (data) {
                ShowMessage(data.Message, 4);
            },
        });
    }
    else {
        ShowMessage(mensagemValidacao, 4);
    }
}

function ValidarCampos() {
    var mensagem = "";
    var mensagemFinal = "";

    if ($("#textBoxNome").val() == "" || $("#textBoxNome").val() == null || $("#textBoxNome").val() == undefined) {
        mensagem += mensagem == "" ? "Nome" : ", Nome";
    }

    if ($("#textBoxLogin").val() == "" || $("#textBoxLogin").val() == null || $("#textBoxLogin").val() == undefined) {
        mensagem += mensagem == "" ? "Login" : ", Login";
    }

    if ($("#textBoxEmail").val() == "" || $("#textBoxEmail").val() == null || $("#textBoxEmail").val() == undefined) {
        mensagem += mensagem == "" ? "Email" : ", Email";
    }

    //if (idSetor <= 0) {
    //    mensagem += mensagem == "" ? "Setor" : ", Setor";
    //}

    if (mensagem != "")
        mensagemFinal = "Preencha o(s) campo(s): " + mensagem;

    return mensagemFinal;
}

function LoadSelectSetor() {

    $("#selectSetor").append('<option value=' + 0 + '>' + "Selecione um Setor" + '</option>');
    $('#selectSetor').chosen({ no_results_text: 'Não foi possível encontrar ', allow_single_deselect: true, disable_search_threshold: 100, width: '350px' });
    $('#selectSetor').trigger("chosen:updated");

    $.ajax({
        url: '/Setor/GetAll',
        data: {},
        success: function (data) {

            if (data.Result == "OK") {
                $.each(data.Records, function (key, value) {
                    $("#selectSetor").append('<option value="' + value.ID + '">' + value.NOME + '</option>');
                });
            }

            $('#selectSetor').chosen({ no_results_text: 'Não foi possível encontrar ', allow_single_deselect: true, disable_search_threshold: 100, width: '350px' });
            $('#selectSetor').trigger("chosen:updated");
        },
        error: function (data) {
            ShowMessage(data.Message, 4);
        },
    });
}

function CarregarUsuario() {
    if ($('#hiddenId').val() != 0 && $('#hiddenId').val() > 0) {

        $("#buttonAdicionaUsuarioConfirmar").html("Alterar");
        $("#buttonAbrirBuscaUsuarioAD").hide();

        $.ajax({
            url: '/Usuario/GetById',
            method: "GET",
            data: { id: $('#hiddenId').val() },
            success: function (data) {
                if (data.Result == "OK") {
                    ShowMessage(data.Message, 1);
                    PopularCampos(data);
                }
                else {
                    ShowMessage(data.Message, 4);
                }
            },
            error: function (data) {
                ShowMessage(data.Message, 4);
            },
        });
    }
}

function PopularCampos(data) {

    if (data.Records.length > 0) {
        $("#textBoxNome").val(data.Records[0].NOME);
        $("#textBoxLogin").val(data.Records[0].LOGIN);
        $("#textBoxEmail").val(data.Records[0].EMAIL);


        if (data.Records[0].ATIVO) {
            $("#chkAtivo").parent().addClass("checked");
            $("#chkAtivo").prop('checked', true);
        }
        if (data.Records[0].ADMIN) {
            $("#chkAdmin").parent().addClass("checked");
            $("#chkAdmin").prop('checked', true);
        }
    }
}



(function () {

    jQuery.ajaxSetup({ cache: false });

    var app = angular.module('DashboardApp', [])

    app.controller('ActivitiesController', function ($scope, $interval, $http, $timeout) {

        //$interval(function () { LoadGridMaster(); }, 180000, 0, true);
    });
})();

function MaskCPF(cpf) {
    if (cpf != null) {
        cpf = cpf.toString().substring(0, 3) + "."
            + cpf.toString().substring(3, 6) + "."
            + cpf.toString().substring(6, 9) + "-"
            + cpf.toString().substring(9, 11);
        return cpf;
    } else {
        return "";
    }
}

function MaskCNPJ(cnpj) {
    if (cnpj != null) {
        cnpj = cnpj.toString().substring(0, 2) + "."
            + cnpj.toString().substring(2, 5) + "."
            + cnpj.toString().substring(5, 8) + "/"
            + cnpj.toString().substring(8, 12) + "-"
            + cnpj.toString().substring(12, 14);
        return cnpj;
    } else {
        return "";
    }
}

function LoadDatePicker() {
    var optionsDatePicker = {
        showButtonPanel: true,
        maxDate: '+1y',
        minDate: 1,
        numberOfMonths: 1,
        dateFormat: 'dd/mm/yy',
        showAnim: '',
        closeText: 'Fechar',
        currentText: 'Mês Atual',
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro']
    };

    $(".datepicker").datepicker(optionsDatePicker);
    $(".datepicker").mask("99/99/9999", {
        completed: function () {
        }
    });
}

function FormattedDate(date) {
    var d = date,
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();
    hour = d.getHours().padLeft();
    minutes = d.getMinutes().padLeft();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    if (hour.length < 2) hour = '0' + hour;
    if (minutes.length < 2) minutes = '0' + minutes;

    return [day, month, year].join('/') + ' ' + [hour, minutes].join(':');
}