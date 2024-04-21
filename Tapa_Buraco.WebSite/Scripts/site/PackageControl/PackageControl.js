/// <reference path="../jquery-1.9.1.js"/>
/// <reference path="../jquery-ui-1.10.3.custom.js"/>
/// <reference path="../plugins/jquery.blockUI.js"/>
/// <reference path="../plugins/json2.js"/>




function LoadGridSearch() {

    $("#divGrid").LoadGrid({});
}



function EnableOnlyNumbers(num) {
    
    var tecla=(window.event)?event.keyCode:num.which;
  
    if (tecla > 47 && tecla < 58)
        return true;
    else
    {
        if (tecla == 8 || tecla == 0)
            return true;
        else
            return false;
    }
}

function CreateGrid() {

    
    $("#divGrid").CreateGrid(
           {
               options: {
                   title: 'Recinto',
                   paging: true, //Enable paging
                   pageSize: 10, //Set page size (default: 10)
                   sorting: true, //Enable sorting
                   defaultSorting: 'Id ASC', //Set default sorting
                   openChildAsAccordion: true,
                   width: '500px'
               },
               actions: {
                   listAction: '/PackageControl/Grid',
                   deleteAction: '/PackageControl/Delete',
                   updateAction: '/PackageControl/Save',
                   createAction: '/PackageControl/Save'
               },
               fields: {
                   Id: {
                       key: true,
                       create: false,
                       edit: false,
                       list: false
                   },

                   IdFacility: {
                       title: 'Recinto',
                       width: '20%',
                       inputClass: 'validate[required]',
                       input: function (data) {
                           var input = '<input type="text" name="IdFacility" maxlength="3" placeholder="Informe o Recinto" ';
                           if (data.record) {
                               input += 'value="' + data.record.IdFacility + '" ';
                           }
                           input += '/>';
                           return input;
                       }
                   },

                   IdStart: {
                       title: 'Início',
                       inputClass: 'validate[required]',
                       width: '20%',
                       input: function (data) {
                           var input = '<input type="text" name="IdStart" maxlength="10" placeholder="Informe o Início" onKeyPress="return EnableOnlyNumbers(event)" ';
                            if (data.record) {
                                input += 'value="' + data.record.IdStart + '" ';
                            }
                            input += '/>';
                            return input;
                        }
                   },
                   IdEnd: {
                       title: 'Fim',
                       inputClass: 'validate[required]',
                       width: '20%',
                       input: function (data) {
                           var input = '<input type="text" name="IdEnd" maxlength="10" placeholder="Informe o Fim" onKeyPress="return EnableOnlyNumbers(event)"';
                           if (data.record) {                             
                               input += 'value="' + data.record.IdEnd + '" ';
                           }
                           input += '/>';
                           return input;
                       }
                   },

                   IdCurrent: {
                       title: 'Sequencial',
                       inputClass: 'validate[required]',
                       width: '20%',
                       input: function (data) {
                           var input = '<input type="text" name="IdCurrent" maxlength="10" placeholder="Informe o Sequencial" onKeyPress="return EnableOnlyNumbers(event)"';
                           if (data.record) {                         
                               input += 'value="' + data.record.IdCurrent + '" ';
                           }
                           input += '/>';
                           return input;
                       }
                   }
                   

               },
               methods: {
                   //Initialize validation logic when a form is created
                   formCreated: function (event, data) {
                       data.form.find('input[name="IdFacility"]').addClass('validate[required]');
                       data.form.find('input[name="IdStart"]').addClass('validate[required]');
                       data.form.find('input[name="IdEnd"]').addClass('validate[required]');
                       data.form.find('input[name="IdCurrent"]').addClass('validate[required]');
                       data.form.validationEngine();

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




$(document).ready(function () {


    $("#btSearch").click(function () {
        LoadGridSearch();
    });

   

    CreateGrid();
    LoadGridSearch();

   

});










