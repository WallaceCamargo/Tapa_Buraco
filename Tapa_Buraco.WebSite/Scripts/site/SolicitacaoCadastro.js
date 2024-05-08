var url_Base_Site = "";
var idSetor = 0;
var img = null;
$(document).ready(function () {

    url_Base_Site = $('#url_Base_Site').data('request-url');

    CarregarSolicitacao();
    //$("#textBoxCEP").mask("99999-999");

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
            NOME_FOTO: $("#hiddenFotoNome").val(), 
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

    if ($("#dataAcatamento").val() != null && $("#dataAcatamento").val() != "") {
        var data_acatamento = new Date($("#dataAcatamento").val());
        data_acatamento = FormattedDate(data_acatamento);
    }

    if ($("#dataFiscalizacao").val() != null && $("#dataFiscalizacao").val() != "") {
        var data_fiscaliza = new Date($("#dataFiscalizacao").val());
        data_fiscaliza = FormattedDate(data_fiscaliza);
    }

    if ($("#dataAgendamento").val() != null && $("#dataAgendamento").val() != "") {
        var data_agenda = new Date($("#dataAgendamento").val());
        data_agenda = FormattedDate(data_agenda);
    }

    if ($("#dataAtendimento").val() != null && $("#dataAtendimento").val() != "") {
        var data_atende = new Date($("#dataAtendimento").val());
        data_atende = FormattedDate(data_atende);
    }

    if ($("#dataFinalizacao").val() != null && $("#dataFinalizacao").val() != "") {
        var data_finaliza = new Date($("#dataFinalizacao").val());
        data_finaliza = FormattedDate(data_finaliza);
    }


    if ($("#textBoxPrioridade").val() == "" || $("#textBoxPrioridade").val() == null || $("#textBoxPrioridade").val() == undefined) {
        mensagem += mensagem == "" ? "PRIORIDADE" : ", PRIORIDADE";
    }

    if ($("#textBoxCEP").val() == "" || $("#textBoxCEP").val() == null || $("#textBoxCEP").val() == undefined) {
        mensagem += mensagem == "" ? "CEP" : ", CEP";
    }

    if ($("#textBoxPontRe").val() == "" || $("#textBoxPontRe").val() == null || $("#textBoxPontRe").val() == undefined) {
        mensagem += mensagem == "" ? "PONTO DE REFERENCIA" : ", PONTO DE REFERENCIA";
    }
    if ($("#hiddenFotoNome").val() == "" || $("#hiddenFotoNome").val() == null || $("#hiddenFotoNome").val() == undefined) {
        mensagem += mensagem == "" ? "ADICIONE UMA IMAGEM" : ", ADICIONE UMA IMAGEM";
    }
    //if (img.src == "" || img.src == null || img.src == undefined) {
    //    mensagem += mensagem == "" ? "Email" : ", Email";
    //}

    //if (data_acatamento > data_fiscaliza) {
    //    mensagem += mensagem == "" ? "A data de fiscalização não pode ser anterior a de acatamento" : ", A data de fiscalização não pode ser anterior a de acatamento";
    //}
    //if (data_fiscaliza > data_agenda) {
    //    mensagem += mensagem == "" ? "A data de agendamento não pode ser anterior a de fiscalização" : ", A data de agendamento não pode ser anterior a de fiscalização";
    //}
    //if (data_agenda > data_atende) {
    //    mensagem += mensagem == "" ? "A data de Atendimento não pode ser anterior a de agendamento" : ", A data de Atendimento não pode ser anterior a de agendamento";
    //}
    //if (data_atende > data_finaliza) {
    //    mensagem += mensagem == "" ? "A data de finalização não pode ser anterior a de Atendimento" : ", A data de finalização não pode ser anterior a de Atendimento";
    //}

    if (mensagem != "")
        mensagemFinal = "Preencha o(s) campo(s): " + mensagem;

    return mensagemFinal;
}

//function SalvaImg() {
//    var file = $('#file')[0].files[0];

//    if (file) {
//        var storageRef = firebase.storage().ref('../font-awesome/img/' + file.name);
//        var task = storageRef.put(file);

//        task.on('state_changed',
//            function progress(snapshot) {
//                var percentage = (snapshot.bytesTransferred / snapshot.totalBytes) * 100;
//                $('#uploadStatus').html('Progresso do Upload: ' + percentage + '%');
//            },
//            function error(err) {
//                $('#uploadStatus').html('Erro ao enviar a imagem: ' + err.message);
//            },
//            function complete() {
//                $('#uploadStatus').html('Imagem enviada com sucesso!');
//            }
//        );
//    } else {
//        $('#uploadStatus').html('Por favor, selecione um arquivo.');
//    }
//});


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

    $("#textBoxCEP").prop('disabled', true);
    $("#textBoxPontRe").prop('disabled', true);
    $("#button_cep").hide();

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
        $("#stepProgress").show();
        consultaCep();
        $("#hiddenFotoNome").val(data.Records[0].NOME_FOTO);
        var caminhoDaImagem = '../../font-awesome/img/' + data.Records[0].NOME_FOTO;
        var img = $('<img>').attr('src', caminhoDaImagem);
        $('.picture__image').empty().append(img);

        //$("#dataAcatamento").prop('disabled', true);
        $("#dataFiscalizacao").prop('disabled', true);
        $("#dataAgendamento").prop('disabled', true);
        $("#dataAtendimento").prop('disabled', true);
        $("#dataFinalizacao").prop('disabled', true);

        ////// altera as etapas do progress etapa ///////
        if (data.Records[0].DT_ACATAMENTO_STRING != null && data.Records[0].DT_ACATAMENTO_STRING != "") {
            $("#step1").addClass('current-item');
            $("#step0").removeClass('current-item');
            $("#dataFiscalizacao").prop('disabled', false);
        }
        if (data.Records[0].DT_FISCALIZACAO_STRING != null && data.Records[0].DT_FISCALIZACAO_STRING != "") {
            $("#step2").addClass('current-item');
            $("#step1").removeClass('current-item');
            $("#dataAcatamento").prop('disabled', true);
            $("#dataAgendamento").prop('disabled', false);
        }
        if (data.Records[0].DT_AGENDAMENTO_STRING != null && data.Records[0].DT_AGENDAMENTO_STRING != "") {
            $("#step3").addClass('current-item');
            $("#step2").removeClass('current-item');
            $("#dataFiscalizacao").prop('disabled', true);
            $("#dataAtendimento").prop('disabled', false);
        }
        if (data.Records[0].DT_ATENDIMENTO_STRING != null && data.Records[0].DT_ATENDIMENTO_STRING != "") {
            $("#step4").addClass('current-item');
            $("#step3").removeClass('current-item');
            $("#dataAgendamento").prop('disabled', true);
            $("#dataFinalizacao").prop('disabled', false);
        }
        if (data.Records[0].DT_FINALIZACAO_STRING != null && data.Records[0].DT_FINALIZACAO_STRING != "") {
            $("#step5").addClass('current-item');
            $("#step4").removeClass('current-item');
            $("#dataAtendimento").prop('disabled', true);
        }
        ////// fim altera as etapas do progress etapa ///////

        if ($("#hiddenPerfil").val() == 0) {
            $("#textBoxPrioridade").prop('disabled', true);
            $("#dataPrazo").prop('disabled', true);
            $("#dataAcatamento").prop('disabled', true);
            $("#dataFiscalizacao").prop('disabled', true);
            $("#dataAgendamento").prop('disabled', true);
            $("#dataAtendimento").prop('disabled', true);
            $("#dataFinalizacao").prop('disabled', true);
        } 

    }
}

//// salvar foto /////////
function SalvaFoto() {
    const inputFile = document.querySelector("#file");
    const pictureImage = document.querySelector(".picture__image");
    const pictureImageTxt = "Escolha uma imagem";
    pictureImage.innerHTML = pictureImageTxt;

    inputFile.addEventListener("change", function (e) {
        const inputTarget = e.target;
        const file = inputTarget.files[0];

        if (file) {
            const reader = new FileReader();

            reader.addEventListener("load", function (e) {
                const readerTarget = e.target;
                console.log(readerTarget);
                const img = document.createElement("img");
                img.src = readerTarget.result;
                img.classList.add("picture__img");

                pictureImage.innerHTML = "";
                pictureImage.appendChild(img);

                // Aqui você pode enviar a imagem para o servidor e salvar no diretório desejado
                salvarImagemNoServidor(file);
            });

            reader.readAsDataURL(file);
        } else {
            pictureImage.innerHTML = pictureImageTxt;
        }
    });
};


function salvarImagemNoServidor(file) {
    const formData = new FormData();
    formData.append('file', file);
    console.log(formData)
    $.ajax({
        url: url_Base_Site + '/Solicitacao/SalvarImagem', // Endpoint para enviar a imagem
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            $("#hiddenFotoNome").val(response.fileName);
            console.log(response); // Lidar com a resposta do servidor aqui
        },
        error: function (xhr, status, error) {
            console.error(status, error); // Exibir erro no console em caso de falha
        }
    });
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
    //hour = d.getHours().padLeft();
    //minutes = d.getMinutes().padLeft();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    //if (hour.length < 2) hour = '0' + hour;
    //if (minutes.length < 2) minutes = '0' + minutes;

    return [day, month, year].join('/') + ' ' + [00, 00].join(':');
}