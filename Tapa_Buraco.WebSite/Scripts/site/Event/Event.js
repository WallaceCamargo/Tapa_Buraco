/// <reference path="../jquery-1.9.1.js"/>
/// <reference path="../jquery-ui-1.10.3.custom.js"/>
/// <reference path="../plugins/jquery.blockUI.js"/>
/// <reference path="../plugins/json2.js"/>


var clearCache = false;
var IdUser;
var HiddenEventId = 0;
var HiddenTagId = 0;
var HiddenId = 0;

function LoadGridSearch() {

    $("#divGrid", "#fieldset").LoadGrid({});
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
                   title: 'Criar Evento',
                   paging: true, //Enable paging
                   pageSize: 10, //Set page size (default: 10)
                   sorting: true, //Enable sorting
                   defaultSorting: 'Code ASC', //Set default sorting
                   openChildAsAccordion: true,
                   width: '500px'
               },
               actions: {
                   listAction: '/Event/Grid',
                   deleteAction: '/Event/Delete',
                   updateAction: '/Event/Save',
                   createAction: '/Event/Save'
               },
               fields: {
                   Id: {
                       key: true,
                       create: false,
                       edit: false,
                       list: false
                   },


                   Code: {
                       title: 'Evento',
                       width: '20%',
                       list: true,
                       inputClass: 'validate[required]',
                       input: function (data) {
                           var input = '<input type="text" name="Code" maxlength="3" placeholder="Informe o Evento" onKeyPress="return EnableOnlyNumbers(event)"';
                           if (data.record) {
                               input += 'value= "' + data.record.Code + '"';
                           }
                           input += '/>';
                           return input;
                       }

                   },

                   Description: {
                       title: 'Descrição',
                       width: '30%',
                       inputClass: 'validate[required]',
                       input: function (data) {
                           var input = '<input type="text" name="Description" maxlength="50" placeholder="Informe a Descrição"';
                           if (data.record) {
                               input += 'value= "' + data.record.Description + '"';
                           }
                           input += '/>';
                           return input;
                       }
                   },
                   IdEventType: {
                       title: 'Tipo Evento',
                       width: '20%',
                       list: false,
                       options: '/EventTag/getEventType',
                       inputClass: 'validate[required]'
                   },

                   FileExtension: {
                       title: 'Extensão',
                       width: '20%',
                       defautValue: '',
                       type: 'text',
                       input: function (data) {
                           var input = '<input type="text" name="FileExtension" maxlength="3" placeholder="Informe a Extensão" ';
                           if (data.record) {
                               input += ' value="' + (data.record.FileExtension == null ? "" : data.record.FileExtension) + '" ';
                           } else {
                               input += ' value = "" ';
                           }
                           input += '/>';
                           return input;
                       }
                   },

                   EventTypeName: {
                       title: "Tipo do Evento",
                       width: '20%',
                       create: false,
                       edit: false,
                       list: true

                   },

                   IsEventStart: {
                       list: true,
                       title: 'Evento Inicial',
                       width: '10%',
                       type: 'checkbox',
                       values: { 'false': 'Não', 'true': 'Sim' },
                       defaultValue: 'false'
                   },

                   FK_EventTag_Tag: {
                       title: '',
                       sorting: false,
                       create: false,
                       edit: false,
                       list: true,
                       // listClass: 'child-opener-image-column-header',
                       listClass: 'jtable-command-column-header',
                       display: function (item) {

                           //Create an image that will be used to open child table
                           //  var $img = $('<img class="child-opener-image-column-header"   src="/Scripts/jtable/themes/metro/list_metro.png" title="Tags" />');
                           var $img = $('<img class="jtable-command-column-header"   src="/Scripts/jtable/themes/metro/list_metro.png" title="Tags" />');

                           //Open child table when user clicks the image
                           $img.click(function () {

                               $('#divGrid').CreateGrid({
                                   tableType: "child",
                                   row: $img.closest('tr'),
                                   options: {
                                       //title: 'Descrição do Evento: ' + item.record.Description,
                                       title: 'Vinculação de Tag ao Evento',
                                       create: false,
                                       edit: false,
                                       paging: true, //Enable paging
                                       pageSize: 5, //Set page size (default: 10)
                                       sorting: true, //Enable sorting
                                       defaultSorting: 'TagOrder ASC', //Set default sorting
                                       saveUserPreferences: false,
                                       toolbar: {
                                           items: [{
                                               text: 'Associar Tag',
                                               click: function () {

                                                   HiddenEventId = item.record.Id;

                                                   $("#dialog").dialog("open");
                                               },
                                           }]
                                       },
                                   },
                                   actions: {
                                       listAction: '/Tag/Grid?idEvent=' + item.record.Id,
                                       deleteAction: '/EventTag/Delete'
                                   },
                                   fields: {
                                       Id: {
                                           key: true,
                                           create: false,
                                           edit: false,
                                           list: false
                                       },

                                       IdTag: {
                                           title: 'tag',
                                           type: 'hidden',
                                           sorting: false,
                                       },

                                       TagOrder: {
                                           title: 'Ordem',
                                           width: '20%',
                                           sorting: true,
                                       },

                                       Code: {
                                           title: 'Tag',
                                           width: '20%',
                                           sorting: false,
                                       },
                                       Description: {
                                           title: 'Descrição',
                                           width: '30%',
                                           sorting: false,
                                       },

                                       IsArray: {
                                           title: 'Múltipla',
                                           width: '20%',
                                           sorting: false,
                                           type: 'checkbox',
                                           values: { 'false': 'Não', 'true': 'Sim' },
                                           defaultValue: 'false'
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
                       data.form.find('input[name="Description"]').addClass('validate[required]');
                       data.form.find('input[name="Code"]').addClass('validate[required]');

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



function SaveEventTag(EventId, EventTagItem, TagOrder, IsArray) {

    Page.AjaxService.Request(
        '/EventTag/Save',
        {
            IdEvent: EventId,
            IdTag: $("#" + HiddenId).val(),
            TagOrder: TagOrder,
            IsArray: IsArray

        },
        function (data) {

            if (data.Result == "OK") {

                $("#txtEvent", ".boxInsertInput").val("");
                $("#txtTagOrder", ".boxInsertInput").val("");
                $("#chkIsArray", ".boxInsert").removeAttr("checked");

                LoadGridEventTag();
            }
        },
        null
    );
}



function InitDialog() {

    // $("#txtEvent").keypress(this.val.toUpperCase());

    $("#dialog").dialog({
        autoOpen: false,
        title: 'Associar Tag a Evento',
        height: 440,
        width: 1000,
        modal: true,
        buttons: {
            "Cancelar": function () {
                $(this).dialog("close");
            }
        },
        close: function () {

        },
        open: function (event, ui) {



            $("#btAdd").on("click", function () {

                var EventId = HiddenEventId;
                var EventTagItem = $("#txtEvent", ".boxInsert").val();
                var TagOrder = $("#txtTagOrder", ".boxInsert").val();
                var IsArray = $("#chkIsArray", ".boxInsert").is(':checked');


                SaveEventTag(EventId, EventTagItem, TagOrder, IsArray);
            });

            //$("#btAdd").on("click", function () {
            //    alert("OLA")
            //});

            //#region AutoComplete field Value                                        
            var TextBoxId = $("#txtEvent", ".boxInsert").attr("id");
            HiddenId = $("#txtTagId", ".boxInsertInput").attr("id");



            LoadTagComplete(TextBoxId, HiddenId);
            //#endregion AutoComplete field Value

            if (!$("#dialog").data('Initialize')) {
                CreateGridTag();
                //LoadGridEventTag();

                $("#dialog").data('Initialize', true);
            }
            LoadGridEventTag();
        }
    });
}

function CreateGridTag() {
    $("#divGrid", "#dialog").CreateGrid({
        options: {
            paging: true, //Enable paging
            pageSize: 7, //Set page size (default: 10)
            sorting: false, //Enable sorting
            defaultSorting: 'TagOrder ASC', //Set default sorting

        },
        actions: {
            listAction: '/Tag/Grid?idEvent=' + HiddenEventId,
        },
        fields: {
            Id: {
                key: true,
                create: false,
                edit: false,
                list: false
            },
            IdTag: {
                title: 'tag',
                type: 'hidden',
                sorting: false,
            },

            TagOrder: {
                title: 'Ordem',
                width: '20%',
                sorting: true,
            },

            Code: {
                title: 'Tag',
                width: '20%',
                sorting: false,
            },
            Description: {
                title: 'Descrição',
                width: '30%',
                sorting: false,
            },

            IsArray: {
                title: 'Múltipla',
                width: '20%',
                sorting: false,
                type: 'checkbox',
                values: { 'false': 'Não', 'true': 'Sim' },
                defaultValue: 'false'
            }

        },
        methods: {
            recordsLoaded: function (event, data) {

            },
            loadingRecords: function (event, data) {
                //SetSelectedIdProfiles();

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

    InitDialog();

});

function LoadGridEventTag() {

    //$("#txtEventId").val(HiddenEventId);
    $("#divGrid", "#dialog").LoadGrid({
        Parameters: {
            Tag: $("#txtEventId", "#dialog").val()
        }
    });
}


function LoadTagComplete(TextBoxId, HiddenId) {


    var options = {
        url: "/Tag/getTag",
        minLength: 1,
        TextBoxId: TextBoxId,
        HiddenId: HiddenId
    };

    Page.AutoComplete.Exec(options);
}

function LoadGrid() {
    $("#divGrid").LoadGrid({

        Parameters: {

            idEvent: $("#ddlevent").val() == null ? "0" : $("#ddlevent").val(),
            description: $("#TextBoxEventId").val(),
            idEventtype: $("#ddleventtype").val() == null ? "0" : $("#ddleventtype").val()
        }
    });
}