var url_Base_Site = "";

$(document).ready(function () {

    url_Base_Site = $('#url_Base_Site').data('request-url');

    CriarGrid();
    CarregarGrid();

    $("#buttonConsultar").click(function () {
        CarregarGrid();
    });

    if ($("#hiddenPerfil").val() == 0) {
        $("#textBoxConsulSolicitacao").hide();
    }

});


function CarregarGrid() {
    $("#grid").LoadGrid({
        Parameters: {
            nm_solicitante: $("#textBoxConsulSolicitacao").val(),
            status: $("#textBoxConsulStatus").val(),
            dt_inicio: $("#textBoxConsulDt_inicio").val(),
            dt_fim: $("#textBoxConsulDt_fim").val(),
        }
    });
}

function CriarGrid() {
    $("#grid").CreateGrid({
        options: {
            title: 'Solicitação',
            paging: true, //Enable paging
            pageSize: 25, //Set page size (default: 10)
            //sorting: true, //Enable sorting
            defaultSorting: 'NOME ASC', //Set default sorting
            saveUserPreferences: false,
            jqueryuiTheme: true,
            tableId: 'GridSolicitacao',
            toolbar: {
                hoverAnimation: true, //Enable/disable small animation on mouse hover to a toolbar item.
                hoverAnimationDuration: 60, //Duration of the hover animation.
                hoverAnimationEasing: undefined, //Easing of the hover animation. Uses jQuery's default animation ('swing') if set to undefined.

                items: MostrarButtonAdicionarSolicitacao(),
            },
        },
        actions: {
            listAction: '/Solicitacao/GetAll',
            //deleteAction: '/User/Delete',
            //updateAction: '/User/Save',
        },
        fields: {
            ID: {
                key: true,
                create: false,
                edit: false,
                list: false
            },
            NM_USUARIO: {
                create: false,
                title: 'SOLICITANTE',
                width: '20%',
            },
            STATUS_STRING: {
                create: false,
                title: 'STATUS',
                width: '15%',
            },
            CEP: {
                create: false,
                title: 'CEP',
                width: '8%',
            },
            DT_REGISTRO_STRING: {
                create: false,
                title: 'DT REGISTRO',
                width: '15%',
            },
            PRIORIDADE: {
                create: false,
                title: 'PRIORIDADE',
                width: '10%',
            },
            DT_PRAZO_STRING: {
                create: false,
                title: 'DT PRAZO',
                width: '10%',
            },
            OpenDialogEdit: {
                title: '',
                width: '1%',
                create: false,
                edit: false,
                list: true,
                sorting: false,
                display: function (item) {
                    //if ($("#hiddenEdit").val() == "EditarSolicitacao") {
                    if (true) {
                        var $img = $('<img class="jtable-command-button release" src="/Scripts/jtable/themes/metro/edit.png" title="Editar" />'); //jtable-delete-command-button
                        $img.click(function (data) {
                            AbriEditar(item);
                        });
                        return $img;
                    }
                    else
                        return "";
                }
            },
            OpenDialogDelete: {
                title: '',
                width: '1%',
                create: false,
                edit: false,
                list: true,
                sorting: false,
                display: function (item) {

                    //if ($("#hiddenDelete").val() == "ExcluirUsuaio") {
                    if (true) {
                        var $img = $('<img class="jtable-command-button delete" src="/Scripts/jtable/themes/metro/delete.png" title="Excluir" />'); //jtable-delete-command-button
                        $img.click(function (data) {
                            AbrirModalExcluir(item);
                        });
                        return $img;
                    }
                    else
                        return "";
                }
            },
        },
        methods: {
            //Initialize validation logic when a form is created
            formCreated: function (event, data) {

                if (data.formType == "edit") {

                }

                data.form.validationEngine();

            },
            //Validate form when it is being submitted
            formSubmitting: function (event, data) {

                clearCache = true;
                data.form.validationEngine('attach', {
                    promptPosition: 'topLeft'
                });
                return data.form.validationEngine('validate');
            },
            //Dispose validation logic when form is closed
            formClosed: function (event, data) {
                data.form.validationEngine('hide');
                data.form.validationEngine('detach');
            },
            completeCallback: function (data) { //opened handler                
                var test = data;
            },
            recordAdded: function (event, data) {
                var test = data;
            },
            recordDeleted: function (event, data) { //opened handler
                if (data.serverResponse.Result == "OK") {
                    ShowMessage("Registro excluido com sucesso!", 1);
                }
                else {
                    ShowMessage(data.serverResponse.Record.Message, 4);
                }
            },
            recordLoaded: function (event, data) { //opened handler
                var test = data;
            },
            recordsLoaded: function (event, data) { //opened handler

                var listTR = $('#GridMotivation > tbody  > tr');

                $.each(listTR, function (index, value) {

                    $.each(data.records, function (indexRecord, record) {

                    });

                });

            },
            recordUpdate: function (event, data) { //opened handler
                if (data.serverResponse.Result == "OK") {
                    ShowMessage("Registro atualizado com sucesso!", 1);
                }
                else {
                    ShowMessage(data.serverResponse.Record.Message, 4);
                }
            },
            rowInserted: function (event, data) { //opened handler

            },
            rowRemoved: function (event, data) { //opened handler

            },
            rowUpdated: function (event, data) {
                ShowMessage(data.record.Message, data.record.IsError ? 4 : 1);
            }
        }
    });
}

function MostrarButtonAdicionarSolicitacao() {
    var array = [];
    //if ($("#hiddenAdd").val() == "AdicionarSolicitacao") {
    if (true) {
        var item = {
            icon: '../../Scripts/jtable/themes/jqueryui/add.png',
            text: 'Adicionar',
            position: 'left',
            click: function () {
                window.location = url_Base_Site + "/Solicitacao/Cadastro/0";
            }
        }
        array.push(item);
    } else {
        ShowMessage("Sem permissão para executar essa chamada!", 4);
    }
    return array;
}

function AbriEditar(item) {
    window.location = url_Base_Site + "/Solicitacao/Cadastro/" + item.record.ID;
}

function AbrirModalExcluir(item) {

    $.ajax({
        url: '/Solicitacao/_ModalExluir',
        success: function (data) {

            $('#dialog-ExcluirSolicitacao').html(data);
            CarregarModalExcluir(item);
            $('#modalConfirmaExclusao').modal();
            $('#modalConfirmaExclusao').modal({ keyboard: false });
            $('#modalConfirmaExclusao').modal('show');

            if (item != null) {
                $("#divContentConfirmaExclusao").html("<p>Deseja excluir a solicitação ?</p>");
            }

            $("#divMensagemErro").html();
            $("#divMensagemErro").hide();

        },
        error: function (data) { ShowMessage("ERROR", 4); },
    });
}

function CarregarModalExcluir(item) {

    $('#modalConfirmaExclusao').on('show.bs.modal', function (e) {

    });

    $('#modalConfirmaExclusao').on('shown.bs.modal', function (e) {

    });

    $('#modalConfirmaExclusao').on('hide.bs.modal', function (e) {

    });

    $('#modalConfirmaExclusao').on('hidden.bs.modal', function (e) {
        $("#divContentConfirmaExclusao").html("");
    });

    $("#buttonConfimarExclusao").click(function () {

        $.ajax({
            url: '/Solicitacao/Delete',
            method: "POST",
            data: { id: item.record.ID },
            success: function (data) {
                if (data.Result == "OK") {
                    $('#modalConfirmaExclusao').modal('hide');
                    ShowMessage(data.Message, 1);
                    CarregarGrid();
                }
                else {
                    $('#modalConfirmaExclusao').modal('hide');
                    ShowMessage(data.Message, 4);
                }
            },
            error: function (data) {
                $('#modalConfirmaExclusao').modal('hide');
                ShowMessage(data.Message, 4);
            },
        });
    });
}

(function () {

    jQuery.ajaxSetup({ cache: false });

    var app = angular.module('DashboardApp', []);

    app.controller('ActivitiesController', function ($scope, $interval, $http, $timeout) {

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