/// <reference path="../jquery-1.9.1.js"/>
/// <reference path="../jquery-ui-1.10.3.custom.js"/>
/// <reference path="../plugins/jquery.blockUI.js"/>
/// <reference path="../plugins/json2.js"/>


var clearCache = false;
var IdUser;
var HiddenTagId = 0;

function LoadGridSearch() {

    $("#divGrid", "#fieldset").LoadGrid({
        Parameters: {
            //code: $("#txtCode", "#fieldset").val(),
            //name: $("#txtName", "#fieldset").val()
        }
    });
}


function CreateGrid() {

    //var IdUser = $("#hiddenUser").val();
    //alert('CreateGrid ' + IdUser);
    $("#divGrid").CreateGrid(
           {
               options: {
                   title: 'Criar Tag',
                   paging: true, //Enable paging
                   pageSize: 10, //Set page size (default: 10)
                   sorting: true, //Enable sorting
                   defaultSorting: 'Id ASC', //Set default sorting
                   openChildAsAccordion: true,
                   width: '500px'
               },
               actions: {
                   listAction: '/Tag/TagGrid',
                   deleteAction: '/Tag/Delete',
                   updateAction: '/Tag/Save',
                   createAction: '/Tag/Save'
               },
               fields: {
                   Id: {
                       key: true,
                       create: false,
                       edit: false,
                       list: false
                   },

                   Code: {
                       title: 'Tag',
                       list: true,
                       inputClass: 'validate[required]',
                       input: function (data) {
                           var input = '<input type="text" name="Code" maxlength="3" placeholder="Informe a Tag" ';
                           if (data.record) {
                               input += 'value="' + data.record.Code + '" ';
                           }
                           input += '/>';
                           return input;
                       }
                   },

                   Description: {
                       title: 'Descrição',
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

                   FK_TagComposite_Tag: {
                       title: '',
                       create: false,
                       edit: false,
                       sorting: false,
                       list: true,
                       // listClass: 'child-opener-image-column',                     
                       listClass: 'jtable-command-column-header',
                       display: function (item) {

                           //Create an image that will be used to open child table
                           // var $img = $('<img class="child-opener-image"   src="/Scripts/jtable/themes/metro/list_metro.png" title="Itens" />');
                           var $img = $('<img class="jtable-command-column" src="/Scripts/jtable/themes/metro/list_metro.png" title="Itens" />');
                           //Open child table when user clicks the image
                           $img.click(function () {
                               //if ($("#divGrid", "#fieldset").jtable("isChildRowOpen", $img.closest("tr"))) {
                               //    $("#divGrid", "#fieldset").jtable("closeChildRow", $img.closest("tr"));
                               //}
                               //else {
                               $('#divGrid').CreateGrid({
                                   tableType: "child",
                                   row: $img.closest('tr'),
                                   options: {
                                       //title: 'Descrição do Item: ' + item.record.Description,
                                       title: 'Vinculação de Item a Tag',
                                       create: false,
                                       edit: false,
                                       paging: true, //Enable paging
                                       pageSize: 10, //Set page size (default: 10)
                                       sorting: true, //Enable sorting
                                       defaultSorting: 'Ordem ASC', //Set default sorting
                                       saveUserPreferences: false,
                                       toolbar: {
                                           items: [{
                                               text: 'Associar Item',
                                               click: function () {

                                                   HiddenTagId = item.record.Id;
                                                   $("#dialog").dialog("open");
                                               },
                                           }]
                                       },
                                   },
                                   actions: {
                                       listAction: '/TagItem/TagCompositeGrid?idTag=' + item.record.Id,
                                       deleteAction: '/TagComposite/Delete'
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

                                       IdTagItem: {
                                           title: 'tag',
                                           type: 'hidden',
                                           sorting: false,
                                       },

                                       Ordem: {
                                           title: 'Ordem',
                                           width: '20%',
                                           sorting: true,
                                       },
                                       Code: {
                                           title: 'Item',
                                           width: '30%',
                                           sorting: false,
                                       },
                                       Description: {
                                           title: 'Descrição',
                                           width: '40%',
                                           sorting: false,
                                       },

                                   },
                                   methods: {
                                       completeCallback: function (data) { //opened handler
                                           data.childTable.jtable('load');
                                       }
                                   }
                               });
                               // }
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
//}


function SaveTagItem(Tag, TagItem, Ordem) {

    Page.AjaxService.Request(
        '/TagComposite/Save',
        {
            IdTag: Tag,
            IdTagItem: TagItem,
            Ordem: Ordem

        },
        function (data) {

            if (data.Result == "OK") {

                $("#txtTagItem", ".boxInsertInput").val("");
                $("#txtTagOrder", ".boxInsertInput").val("");

                LoadGridTagItem();
            }
        },
        null
    );
}

function InitDialog() {
    $("#dialog").dialog({
        autoOpen: false,
        title: 'Item',
        height: 440,
        width: 1000,
        modal: true,
        buttons: {
            "Cancelar": function () {
                $(this).dialog("close");
                CreateGridTagItem();

            }
        },
        close: function () {
            CreateGridTagItem();

        },
        open: function (event, ui) {

            $("#btAdd").on("click", function () {
                var Tag = HiddenTagId;
                var TagItem = $("#txtTagItemId", ".boxInsert").val();
                // Recurso para inserir Ordem automaticamente
                var Ordem = $("#txtTagOrder", ".boxInsert").val();

                SaveTagItem(Tag, TagItem, Ordem);
            });

            //#region AutoComplete field Value                                        
            var TextBoxId = $("#txtTagItem", ".boxInsert").attr("id");
            var HiddenId = $("#txtTagItemId", ".boxInsert").attr("id");
            $("#txtTagId", ".boxInsert").val(HiddenTagId);


            LoadTagItemComplete(TextBoxId, HiddenId);
            //#endregion AutoComplete field Value

            if (!$("#dialog").data('Initialize')) {
                CreateGridTagItem();
                LoadGridTagItem();

                //Preencher DropDownList de Ordem
                // $("#ddlTagOrder").LoadDropDown("/TagComposite/getTagCompositeOrder?idTag="+ HiddenTagId);

                $("#dialog").data('Initialize', true);
            }
             LoadGridTagItem();
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

function LoadGridTagItem() {
    $("#divGrid", "#dialog").LoadGrid({
        Parameters: {
            Tag: $("#txtTagId", "#dialog").val()
        }
    });
}

function CreateGridTagItem() {
    $("#divGrid", "#dialog").CreateGrid({
        options: {
            paging: true, //Enable paging
            pageSize: 10, //Set page size (default: 10)
            sorting: false, //Enable sorting
            defaultSorting: 'Ordem ASC', //Set default sorting

        },
        actions: {
            listAction: '/TagItem/TagCompositeGrid?idTag=' + HiddenTagId,
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

            IdTagItem: {
                title: 'tag',
                type: 'hidden',
                sorting: false,
            },

            Ordem: {
                title: 'Ordem',
                width: '20%',
                sorting: true,
            },
            Code: {
                title: 'Item',
                width: '30%',
                sorting: false,
            },
            Description: {
                title: 'Descrição',
                width: '40%',
                sorting: false,
            },

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



function LoadTagItemComplete(TextBoxId, HiddenId) {


    var options = {
        url: "/TagItem/getTagItem",
        minLength: 1,
        TextBoxId: TextBoxId,
        HiddenId: HiddenId
    };

    Page.AutoComplete.Exec(options);
}

function LoadGrid() {
    $("#divGrid").LoadGrid({
        Parameters: {
            tag: $("#TextBoxTag").val(),
            description: $("#TextBoxTagDescription").val(),
        }
    });
}
