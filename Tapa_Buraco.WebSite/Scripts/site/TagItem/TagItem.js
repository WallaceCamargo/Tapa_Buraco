/// <reference path="../jquery-1.9.1.js"/>
/// <reference path="../jquery-ui-1.10.3.custom.js"/>
/// <reference path="../plugins/jquery.blockUI.js"/>
/// <reference path="../plugins/json2.js"/>


var clearCache = false;
var IdUser;

function LoadGridSearch() {

    $("#divGrid").LoadGrid({});
}


function EnableOnlyNumbers(num) {

    var tecla = (window.event) ? event.keyCode : num.which;

    if (tecla > 47 && tecla < 58)
        return true;
    else {
        if (tecla == 8 || tecla == 0)
            return true;
        else
            return false;
    }
}

function CreateGrid() {

    //var IdUser = $("#hiddenUser").val();
    //alert('CreateGrid ' + IdUser);
    $("#divGrid").CreateGrid(
           {
               options: {
                   title: 'Item',
                   paging: true, //Enable paging
                   pageSize: 10, //Set page size (default: 10)
                   sorting: true, //Enable sorting
                   defaultSorting: 'Id ASC', //Set default sorting
                   openChildAsAccordion: true,
                   width: '500px'
               },
               actions: {
                   listAction: '/TagItem/TagItemGrid',
                   deleteAction: '/TagItem/Delete',
                   updateAction: '/TagItem/Save',
                   createAction: '/TagItem/Save'
               },
               fields: {
                   Id: {
                       key: true,
                       create: false,
                       edit: false,
                       list: false
                   },

                   Code: {
                       title: 'Item',
                       width: '20%',
                       list: true,
                       inputClass: 'validate[required]',
                       input: function (data) {
                           var input = '<input type="text" name="Code" maxlength="30" placeholder="Informe o Item" ';
                           if (data.record) {
                               input += 'value="' + data.record.Code + '" ';
                           }
                           input += '/>';
                           return input;
                       }
                   },

                   Description: {
                       title: 'Descrição',
                       width: '20%',
                       inputClass: 'validate[required]',
                       input: function (data) {
                           var input = '<input type="text" name="Description" maxlength="50" placeholder="Informe a Descrição" ';
                           if (data.record) {
                               input += 'value="' + data.record.Description + '" ';
                           }
                           input += '/>';
                           return input;
                       }

                   },

                   Length: {
                       title: 'Tamanho',
                       width: '10%',
                       inputClass: 'validate[required]',
                       input: function (data) {
                           var input = '<input type="text" name="Length" maxlength="3" placeholder="Informe o Tamanho" onKeyPress="return EnableOnlyNumbers(event)" ';
                           if (data.record) {
                               input += 'value="' + data.record.Length + '" ';
                           }
                           input += '/>';
                           return input;
                       }
                   },

                   Mask: {
                       title: 'Máscara',
                       width: '20%',
                       inputClass: 'validate[required]',
                       input: function (data) {
                           var input = '<input type="text" name="Mask" maxlength="20" placeholder="Informe a Máscara" ';
                           if (data.record) {
                               input += 'value="' + data.record.Mask + '" ';
                           }
                           input += '/>';
                           return input;
                       }

                   },

                   TagItemTypeName: {
                       title: 'Tipo do Item',
                       width: '20%',
                       create: false,
                       edit: false,
                       list: true,

                   },

                   IdTagItemType: {
                       title: 'Tipo do Item',
                       width: '20%',
                       list: false,
                       options: '/TagItem/getTagItemType',
                       inputClass: 'validate[required]'

                   },

               },
               methods: {
                   //Initialize validation logic when a form is created
                   formCreated: function (event, data) {
                       data.form.find('input[name="Code"]').addClass('validate[required]');
                       data.form.find('input[name="Description"]').addClass('validate[required]');
                       data.form.find('input[name="Length"]').addClass('validate[required]');
                       data.form.find('input[name="Mask"]').addClass('validate[required]');
                       data.form.validationEngine();
                       // $(".onlyNumeric").OnlyNumeric();

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


    $("#btsearch").click(function () {
        //LoadGridSearch();
        LoadGrid();
    });

    CreateGrid();
    LoadGridSearch();

});

function LoadGrid() {
    $("#divGrid").LoadGrid({

        Parameters: {

            item: $("#TextBoxItemtId").val(),
            description: $("#TextBoxItemDescriptiontId").val(),
            idItemType: $("#ddlitemtypeList").val() == null ? "0" : $("#ddlitemtypeList").val()
        }
    });



}