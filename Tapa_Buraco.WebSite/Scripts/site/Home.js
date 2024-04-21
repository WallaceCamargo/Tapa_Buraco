//(function () {
//    'use strict';

//    angular
//        .module('app')
//        .controller('home', home);

//    home.$inject = ['$location']; 

//    function home($location) {
//        /* jshint validthis:true */
//        var vm = this;
//        vm.title = 'home';

//        activate();

//        function activate() { }
//    }
//})();
(function () {
    'use strict';

    var app = angular.module('DashboardApp', []);

    app.controller('home', home);

    home.$inject = ['$location'];

    function home($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'home';

        activate();

        function activate() { }
    }
})();

$(function () {

    var doughnutData = [
        {
            value: 60,
            color: "#ff0000",
            highlight: "#1ab394",
            label: "Utilizado"
        },
        //{
        //    value: 40,
        //    color: "#00ff00",
        //    highlight: "#1ab394",
        //    label: "Livre"
        //},
        //{
        //    value: 100,
        //    color: "#b5b8cf",
        //    highlight: "#1ab394",
        //    label: "Laptop"
        //}
    ];

    var doughnutOptions = {
        segmentShowStroke: true,
        segmentStrokeColor: "#fff",
        segmentStrokeWidth: 2,
        percentageInnerCutout: 45, // This is 0 for Pie charts
        animationSteps: 100,
        animationEasing: "easeOutBounce",
        animateRotate: true,
        animateScale: false,
        responsive: true,
    };


    //var ctx1 = document.getElementById("doughnutChartProcess").getContext("2d");
    //var myNewChart1 = new Chart(ctx1).Doughnut(doughnutData, doughnutOptions);

    //var ctx2 = document.getElementById("doughnutChartMemory").getContext("2d");
    //var myNewChart2 = new Chart(ctx2).Doughnut(doughnutData, doughnutOptions);

    //var ctx3 = document.getElementById("doughnutChartHardDisk").getContext("2d");
    //var myNewChart3 = new Chart(ctx3).Doughnut(doughnutData, doughnutOptions);

    //var ctx4 = document.getElementById("doughnutChartNetwork").getContext("2d");
    //var myNewChart4 = new Chart(ctx4).Doughnut(doughnutData, doughnutOptions);
});

$(document).ready(function () {
    //Animation();
    //DoAngular();
});

function Animation() {
    $('.contact-box').each(function () {
        animationHover(this, 'pulse');
    });
}

(function () {

    jQuery.ajaxSetup({ cache: false });

    var app = angular.module('DashboardApp', []);

    app.controller('ActivitiesController', function ($scope, $interval, $http, $timeout) {
        //GetLastActivities($scope, $http);
        //GetMachineActivities($scope, $http);

        //$scope.GetLastActivities = $interval(function () { GetLastActivities($scope, $http); }, 10000, 0, true);
        //$timeout(function () { $scope.GetLastActivities(); }, 30000);

        //$scope.GetMachineActivities = $interval(function () { GetMachineActivities($scope, $http); }, 10000, 0, true);
        //$timeout(function () { $scope.GetMachineActivities(); }, 30000);
    });
})();

//function GetLastActivities($scope, $http) {

//    var config = { params: { typeService: 1, number: 16, timeBug: (new Date()).getTime() } };

//    $http.get('/Home/GetLastActivities', config)
//        .success(function (data, status, headers, config) {
//            $scope.LastActivities = data.Records.ListActivities;
//            $scope.Hostname = data.Records.Hostname;
//            $scope.IPv4 = data.Records.IPv4;
//            $scope.IPv6 = data.Records.IPv6;
//            $scope.OperationalSystem = data.Records.OperationalSystem;

//            $scope.Process = data.Records.Process;
//            $scope.Memory = data.Records.Memory;
//            $scope.HardDisk = data.Records.HardDisk;
//            $scope.Network = data.Records.Network;

//            GetColorByPorcent($("#divProcessBar").css({ width: data.Records.Process + '%' }), data.Records.Process);
//            GetColorByPorcent($("#divMemoryBar").css({ width: data.Records.Memory + '%' }), data.Records.Memory);
//            GetColorByPorcent($("#divHardDiskBar").css({ width: data.Records.HardDisk + '%' }), data.Records.HardDisk);
//            GetColorByPorcent($("#divNetworkBar").css({ width: data.Records.Network + '%' }), data.Records.Network);
//        })
//        .error(function (data, status, header, config) {
//            $scope.ResponseDetails = "Data: " + data +
//                "<hr />status: " + status +
//                "<hr />headers: " + header +
//                "<hr />config: " + config;
//        });
//}

//function GetMachineActivities($scope, $http) { manter comentado para não dar erros no console

    //var config = { params: { typeService: 1, number: 6, timeBug: (new Date()).getTime() } };

    //$http.get('/Home/GetMachineActivities', config)
    //    .success(function (data, status, headers, config) {
    //        $scope.ListMachines = data.Records.List;

    //    })
    //    .error(function (data, status, header, config) {
    //        $scope.ResponseDetails = "Data: " + data +
    //            "<hr />status: " + status +
    //            "<hr />headers: " + header +
    //            "<hr />config: " + config;
    //    });
//}

function GetColorByPorcent(bar, percent) {

    bar.removeClass("progress-bar-warning");
    bar.removeClass("progress-bar-danger");

    if (percent > 51 && percent <= 75) {
        bar.addClass("progress-bar-warning");
    }
    if (percent > 76) {
        bar.addClass("progress-bar-danger");
    }
}

//$(document).ready(function () {
//    // Função para buscar os dados do servidor e atualizar a tabela
//    function atualizarTabela() {
//        $.ajax({
//            url: '/Home/GetAll',
//            type: 'GET',
//            data: {
//                nome: '', // Adicione aqui os parâmetros de filtro, se necessário
//                armador: '',
//                agencia: '',
//                loa: ''
//            },
//            success: function (result) {
//                if (result.Result === "OK") {
//                    var records = result.Records;
//                    var tableBody = $('table tbody');
//                    tableBody.empty(); // Limpa o conteúdo atual da tabela

//                    $.each(records, function (record) {
//                        // Constrói uma linha da tabela com os dados recebidos
//                        var row = '<tr>' +
//                            '<td>  </td>' + //STATUS
//                            '<td>' + records[record].NOME + '</td>' +
//                            '<td>  </td>' + //LINE
//                            '<td>' + records[record].LOA + '</td>' +
//                            '<td>  </td>' + //OWNER
//                            '<td>' + records[record].NUMERO_VIAGEM + '</td>' +
//                            '<td>' + records[record].AGENCIA + '</td>' +
//                            // Adicione os outros campos da tabela conforme necessário
//                            '</tr>';
//                        tableBody.append(row); // Adiciona a linha à tabela
//                        //console.log(record[0].NOME);
//                    });

//                } else {
//                    ShowMessage(result.Message, 4); // Usar result.Message ao invés de data.Message
//                }
//            },
//            error: function (data) {
//                ShowMessage(data.Message, 4);
//            }
//        });
//    }

//    // Chama a função para buscar e atualizar os dados da tabela ao carregar a página
//    atualizarTabela();
//});
$(document).ready(function () {

    url_Base_Site = $('#url_Base_Site').data('request-url');

    CriarGrid();
    CarregarGrid();
    //$("#textBoxConsultarLOA").change(function () {
    //    var loa = $("#textBoxConsultarLOA").val();
    //    if (loa.length > 2) {
    //        $("#textBoxConsultarLOA").val(LoaComVirgula(loa));
    //    }
    //});

});

function CarregarGrid() {
    $("#grid").LoadGrid({
        Parameters: {
            //nome: $("#textBoxConsultarNavio").val(),
            //joint: $("#textBoxConsultarJoint").val(),
            //loa: $("#textBoxConsultarLOA").val(),
            //numeroViagem: $("#textBoxConsultarNumeroViagem").val(),
            //agencia: $("#textBoxConsultarAgencia").val(),
            
        }
    });
}
function CriarGrid() {
    $("#grid").CreateGrid({
        options: {
            title: 'LINE UP',
            paging: true, //Enable paging
            pageSize: 25, //Set page size (default: 10)
            sorting: true, //Enable sorting
            defaultSorting: 'NOME ASC', //Set default sorting
            saveUserPreferences: false,
            jqueryuiTheme: true,
            tableId: 'gridLineUp',
        },
        actions: {
            listAction: '/Home/GetAll',
        },
        fields: {
            ID: {
                key: true,
                create: false,
                edit: false,
                list: false
            },
            NOME: {
                create: false,
                title: 'Vessel',
                width: '20%',
            },
            JOINT: {
                create: false,
                title: 'Line',
                width: '20%',
            },
            LOA: {
                create: false,
                title: 'LOA',
                width: '20%',
                display: function (item) {
                    return LoaComVirgula(item.record.LOA);
                }
            },
            NUMERO_VIAGEM: {
                create: false,
                title: 'Voyage Number',
                width: '20%',
            },
            AGENCIA: {
                create: false,
                title: 'Agent',
                width: '20%',
            },

        //    OpenDialogEdit: {
        //        title: '',
        //        width: '1%',
        //        create: false,
        //        edit: false,
        //        list: true,
        //        sorting: false,
        //        display: function (item) {
        //            //if ($("#hiddenEdit").val() == "EditarNavio") {
        //            if (true) {
        //                var $img = $('<img class="jtable-command-button release" src="/Scripts/jtable/themes/metro/edit.png" title="Editar" />'); //jtable-delete-command-button
        //                $img.click(function (data) {
        //                    AbriEditar(item);
        //                });
        //                return $img;
        //            }
        //            else
        //                return "";
        //        }
        //    },
        //    OpenDialogDelete: {
        //        title: '',
        //        width: '1%',
        //        create: false,
        //        edit: false,
        //        list: true,
        //        sorting: false,
        //        display: function (item) {

        //            //if ($("#hiddenDelete").val() == "ExcluirUsuaio") {
        //            if (true) {
        //                var $img = $('<img class="jtable-command-button delete" src="/Scripts/jtable/themes/metro/delete.png" title="Excluir" />'); //jtable-delete-command-button
        //                $img.click(function (data) {
        //                    AbrirModalExcluir(item);
        //                });
        //                return $img;
        //            }
        //            else
        //                return "";
        //        }
        //    },
        //},
        methods: {
           
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

                var listTR = $('#grid > tbody  > tr');

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



function LoaComVirgula(loa) {
    loa = loa.replaceAll(',', '');
    var antesDaVirgula = loa.slice(0, -2);
    var depoisDaVirgula = loa.slice(-2);
    var conteudoComVirgula = antesDaVirgula + ',' + depoisDaVirgula;
    $("#textBoxLOA").val(conteudoComVirgula);
}