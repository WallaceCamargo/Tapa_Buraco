var url_Base_Site = "";
var idSetor = 0;
var img = null;
$(document).ready(function () {

    url_Base_Site = $('#url_Base_Site').data('request-url');

    CarregarSolicitacao();
    //$("textBoxCEP").removeAttr('disabled');
    $("#textBoxCEP").mask("99999-999");

    $('#buttonAdicionaUsuarioCancelar').click(function () {
        if (confirm('Todas as alterações não salvas serão perdidas.\rDeseja continuar mesmo assim?')) {
            window.location = url_Base_Site + "/Solicitacao/Index";
        }
    });

    $('#buttonAdicionaSolicitacaoConfirmar').click(function () {
        SalvarSolicitacao();
    });

    SalvaFoto();
});

function consultaCep() {
    var cep = $("#textBoxCEP").val();
    var url = "https://viacep.com.br/ws/" + cep + "/json/";
    
    console.log(cep);

    $.ajax({
        url: url,
        type: "GET",
        success: function (response) {
            console.log(response);
            $("#textBoxBairro").val(response.bairro);
            $("#textBoxCidade").val(response.localidade);
            $("#textBoxUF").val(response.uf);
            $("#textBoxLogradouro").val(response.logradouro);
        }
    })
}
function SalvarSolicitacao() {

    var mensagemValidacao = ValidarCampos();

    if (mensagemValidacao == "") {

        var config = {
            ID: $('#hiddenId').val() > 0 ? $('#hiddenId').val() : 0,
            PRIORIDADE: $("#textBoxPrioridade").val(), 
            DT_PRAZO: $("#dataPrazo").val(),
            //IMG_URL: img.src,
            //IMG_file: img.src,
            LOGRADOURO: $("#textBoxLogradouro").val(),
            CEP: $("#textBoxCEP").val(),
            BAIRRO: $("#textBoxBairro").val(),
            CIDADE: $("#textBoxCidade").val(),
            ZONA: null,
            ESTADO: $("#textBoxUF").val(),
            PONTO_REFERENCIA: $("#textBoxPontRe").val(),
            DT_ACATAMENTO: $("#dataAcatamento").val(),
            DT_FISCALIZACAO: $("#dataFiscalizacao").val(),
            DT_AGENDAMENTO: $("#dataAgendamento").val(),
            DT_ATENDIMENTO: $("#dataAtendimento").val(),
            DT_FINALIZACAO: $("#dataFinalizacao").val()
        };

        var url = "";

        if ($('#hiddenId').val() > 0) {
            url = url_Base_Site + '/Solicitacao/Put'
        } else {
            url = url_Base_Site + '/Solicitacao/Post'
        }

        $.ajax({
            url: url,
            method: "POST",
            data: { Solicitacao: config },
            success: function (data) {
                if (data.Result == "OK") {
                    ShowMessage(data.Message, 1);
                    setTimeout(() => {
                        window.location = url_Base_Site + "/Solicitacao/Index";
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

    if ($("#textBoxPrioridade").val() == "" || $("#textBoxPrioridade").val() == null || $("#textBoxPrioridade").val() == undefined) {
        mensagem += mensagem == "" ? "PRIORIDADE" : ", PRIORIDADE";
    }

    if ($("#textBoxCEP").val() == "" || $("#textBoxCEP").val() == null || $("#textBoxCEP").val() == undefined) {
        mensagem += mensagem == "" ? "CEP" : ", CEP";
    }

    if ($("#textBoxPontRe").val() == "" || $("#textBoxPontRe").val() == null || $("#textBoxPontRe").val() == undefined) {
        mensagem += mensagem == "" ? "PONTO DE REFERENCIA" : ", PONTO DE REFERENCIA";
    }
    //if (img.src == "" || img.src == null || img.src == undefined) {
    //    mensagem += mensagem == "" ? "Email" : ", Email";
    //}

    if (mensagem != "")
        mensagemFinal = "Preencha o(s) campo(s): " + mensagem;

    return mensagemFinal;
}

//function LoadSelectSetor() {

//    $("#selectSetor").append('<option value=' + 0 + '>' + "Selecione um Setor" + '</option>');
//    $('#selectSetor').chosen({ no_results_text: 'Não foi possível encontrar ', allow_single_deselect: true, disable_search_threshold: 100, width: '350px' });
//    $('#selectSetor').trigger("chosen:updated");

//    $.ajax({
//        url: '/Setor/GetAll',
//        data: {},
//        success: function (data) {

//            if (data.Result == "OK") {
//                $.each(data.Records, function (key, value) {
//                    $("#selectSetor").append('<option value="' + value.ID + '">' + value.NOME + '</option>');
//                });
//            }

//            $('#selectSetor').chosen({ no_results_text: 'Não foi possível encontrar ', allow_single_deselect: true, disable_search_threshold: 100, width: '350px' });
//            $('#selectSetor').trigger("chosen:updated");
//        },
//        error: function (data) {
//            ShowMessage(data.Message, 4);
//        },
//    });
//}

function CarregarSolicitacao() {
    if ($('#hiddenId').val() != null && $('#hiddenId').val() > 0) {

        $("#buttonAdicionaSolicitacaoConfirmar").html("Alterar");

        $.ajax({
            url: '/Solicitacao/GetById',
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
        $("#textBoxCEP").val(data.Records[0].CEP);
        $("#textBoxPontRe").val(data.Records[0].PONTO_REFERENCIA);
        $("#textBoxPrioridade").val(data.Records[0].PRIORIDADE);
        $("#dataPrazo").val(data.Records[0].DT_PRAZO_STRING);
        $("#dataAcatamento").val(data.Records[0].DT_ACATAMENTO_STRING);
        $("#dataFiscalizacao").val(data.Records[0].DT_FISCALIZACAO_STRING);
        $("#dataAgendamento").val(data.Records[0].DT_AGENDAMENTO_STRING);
        $("#dataAtendimento").val(data.Records[0].DT_ATENDIMENTO_STRING);
        $("#dataFinalizacao").val(data.Records[0].DT_FINALIZACAO_STRING);
        $("#campos_datas").show();
        $("textBoxCEP").attr('disabled', 'disabled');
        consultaCep();
    }
}

//// salvar foto /////////
function SalvaFoto() {
    const inputFile = document.querySelector("#file");
    const pictureImage = document.querySelector(".picture__image");
    const pictureImageTxt = "Choose an image";
    pictureImage.innerHTML = pictureImageTxt;

    inputFile.addEventListener("change", function (e) {
        const inputTarget = e.target;
        const file = inputTarget.files[0];

        if (file) {
            const reader = new FileReader();

            reader.addEventListener("load", function (e) {
                const readerTarget = e.target;
                console.log(readerTarget);
                img = document.createElement("img");
                img.src = readerTarget.result;
                img.classList.add("picture__img");

                pictureImage.innerHTML = "";
                pictureImage.appendChild(img);
            });

            reader.readAsDataURL(file);
        } else {
            pictureImage.innerHTML = pictureImageTxt;
        }
    });
};

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