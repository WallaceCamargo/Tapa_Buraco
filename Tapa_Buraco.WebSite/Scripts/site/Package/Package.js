/// <reference path="../jquery-1.9.1.js"/>
/// <reference path="../jquery-ui-1.10.3.custom.js"/>
/// <reference path="../plugins/jquery.blockUI.js"/>
/// <reference path="../plugins/json2.js"/>


function PackageFilter() {

    var evento = $("#ddlEvent").val();
    var initialTime = $("#txtInitialTime").val();
    var initialHour = $("#txtInitialHour").val();
    var finalTime = $("#txtFinalTime").val();
    var finalHour = $("#txtFinalHour").val();
    var status = $("#ddlStatus").val();
    var Process = $("#ddlProcess").val();

    var dtInicial = initialTime + " " + initialHour;
    var dtFinal = finalTime + " " + finalHour;

    Page.AjaxService.Request('/Package/Grid',
        {

            IDYARD: yard,
            IDSERVICE: service,
            INITIALHOUR: initialTime,
            FINALHOUR: finalTime
        },

        function (data) {

            if (data.Result == "OK") {

                return;
            }
        },
           null);

}


function LoadGrid() {
    $("#divGrid").LoadGrid({
        Parameters: {
            evento: $("#ddlEvent").val() == null ? "0" : $("#ddlEvent").val(),
            initialTime: $("#txtInitialDate").val(),
            finalTime: $("#txtFinalDate").val(),
            status: $("#ddlStatus").val() == null ? "0" : $("#ddlStatus").val(),
            process: $("#ddlProcess").val() == null ? "0" : $("#ddlProcess").val()
        }
    });
}

function LoadGridSearch() {

    $("#divGrid").LoadGrid({});
}


function CreateGrid() {


    $("#divGrid").CreateGrid(
           {
               options: {
                   title: 'Lista de Pacotes',
                   paging: true, //Enable paging
                   pageSize: 10, //Set page size (default: 10)
                   sorting: false, //Enable sorting
                   defaultSorting: 'EventNumber ASC', //Set default sorting
                   openChildAsAccordion: true,
                   width: '500px'
                  
               },
               actions: {
                   listAction: '/Package/Grid'
               },
               fields: {
                   //Id Package
                   Id: {
                       key: true,
                       create: false,
                       edit: false,
                       list: false
                   },

                   // Sentido - Package
                   Direction: {
                       title: "Sentido",
                       inputClass: 'validate[required]',
                       width: '10%'

                   },

                   //Package - DtRegisterFormat  (Data do Pacote)  
                   DtRegisterFormat: {
                       title: 'Data',
                       inputClass: 'validate[required]',
                       width: '10%'
                   },
                   //Package - Código  (EventCode de Package)  
                   EventCode: {
                       title: 'Evento',
                       inputClass: 'validate[required]',
                       width: '30%'
                   },

                   //Package - Status (transformado, arq. gerado, enviado, alterado, erro)
                   Status: {
                       title: 'Status',
                       inputClass: 'validate[required]',
                       width: '20%'
                   },

                   //Package - Processo (resultado com base no valor do campo Status se o registro consta na Tabela Package Critical)
                   Process: {
                       title: 'Processo',
                       inputClass: 'validate[required]',
                       width: '20%'
                   },

                   //Sequencial de Package Critical
                   EventNumber: {
                       title: 'Sequencial',
                       inputClass: 'validate[required]',
                       width: '10%'
                   },

                   Critical: {
                       title: '',
                       create: false,
                       edit: false,
                       list: true,
                       listClass: 'jtable-command-column-header',

                       display: function (item) {

                           //Create an image that will be used to open child table
                           var $img = $('<img class="jtable-command-column-header"   src="/Scripts/jtable/themes/metro/list_metro.png" title="Pacotes com Crítica" />');

                           //Open child table when user clicks the image
                           $img.click(function () {

                               $('#divGrid').CreateGrid({
                                   tableType: "child",
                                   row: $img.closest('tr'),
                                   options: {
                                       //title: 'Descrição do Evento: ' + item.record.Description,
                                       title: 'Pacotes com Crítica',
                                       create: false,
                                       edit: true,
                                       paging: true, //Enable paging
                                       pageSize: 10, //Set page size (default: 10)
                                       sorting: true, //Enable sorting
                                       defaultSorting: 'RecordNumber ASC', //Set default sorting
                                       saveUserPreferences: false,
                                     

                                   },
                                   actions: {
                                       listAction: '/Package/GridPackageCriticalById?id=' + item.record.Id,
                                   },
                                   fields: {
                                       Id: {
                                           key: true,
                                           create: false,
                                           edit: false,
                                           list: false
                                       },


                                       CodRecord: {
                                           title: 'Código',
                                           width: '30%'
                                       },

                                       RecordItem: {
                                           title: 'Item',
                                           width: '20%'

                                       },

                                       RecordNumber: {
                                           title: 'Número',
                                           width: '20%'

                                       },
                                       Critical: {
                                           title: 'Crítica',
                                           width: '30%',

                                       },
                                       Alterar: {
                                           title: 'Alterar',
                                           width: '30%',
                                           sorting: false,
                                           display: function (data) {
                                               if (item.record.Status != "Alterado")
                                                   return '<center><a href="/Tree/Index/' + data.record.Id + '"><img class="jtable-command-button jtable-edit-command-button"  src="/Scripts/jtable/themes/lightcolor/edit.png" title="Alterar" /></a></center>';
                                               else
                                                   return "Alterado";
                                           },

                                       }

                                   },
                                   methods: {
                                       completeCallback: function (data) { //opened handler
                                           data.childTable.jtable('load');
                                       }
                                   }
                               });

                           });

                           //Return image to show on the person row
                           return $img;
                       }
                   }


               },
               methods: {
                   //Initialize validation logic when a form is created
                   formCreated: function (event, data) {
                       data.form.validationEngine();
                       //$(".onlyNumeric").OnlyNumeric();

                   },
                   //Validate form when it is being submitted
                   formSubmitting: function (event, data) {
                       clearCache = true;
                       return data.form.validationEngine('validate');
                   },
                   //Dispose validation logic when form is closed
                   formClosed: function (event, data) {
                       data.form.validationEngine('hide');
                       data.form.validationEngine('detach');
                       LoadGridSearch();
                   }
               }


           });
}



function Initialize() {
    $("#btSearch").click(function () {
        LoadGrid();
    });


    $(".span1").datetimepicker({
        lang: 'pt',
        i18n: {
            pt: {
                months: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                dayOfWeek: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S']
            }
        },

        format: 'd/m/Y H:i'
    });

    CreateGrid();
    LoadGridSearch();

    //LoadDatePicker();

}


$(document).ready(function () {

    Initialize();


});










