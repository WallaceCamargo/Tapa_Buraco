/// <reference path="../jquery-1.9.1.js"/>
/// <reference path="../jquery-ui-1.10.3.custom.js"/>
/// <reference path="../plugins/jquery.blockUI.js"/>
/// <reference path="../plugins/json2.js"/>
/// <reference path="core.js"/>

var clearCacheMaster = false;

function LoadGridMaster() {
    $("#grid").LoadGrid({
        Parameters: {
            error: $("#textBoxError").val(),
            dateInitial: $("#textBoxDateInitial").val(),
            dateFinal: $("#textBoxDateFinal").val()
        }
    });
    $(".jtable").addClass("tile table table-bordered table-striped");
    $(".jtable-bottom-panel").addClass("tile");
    //$(".jtable-page-size-change select").addClass("select");
}

function CreateGridMaster() {
    $("#grid").CreateGrid({
        options: {
            //title: 'Erros',
            paging: true, //Enable paging
            pageSize: 10, //Set page size (default: 10)
            sorting: true, //Enable sorting
            defaultSorting: 'ErrorDate DESC', //Set default sorting
            saveUserPreferences: false
        },
        actions: {
            listAction: '/Error/Grid',
            createAction: '/Erro/Save',
            deleteAction: '/Event/Delete',
            updateAction: '/Erro/Delete'
        },
        fields: {
            ID: {
                key: true,
                create: false,
                edit: false,
                list: false
            },
            Description: {
                title: 'Descrição',
                width: '23%',
                //inputClass: 'validate[required]',
                //input: function (data) {
                //    var input = '<input type="text" name="NAME" maxlength="50" placeholder="Informe o Nome" ';
                //    if (data.record) {
                //        input += 'value="' + data.record.NAME + '" ';
                //    }
                //    input += '/>';
                //    return input;
                //}
            },
            //MAIL: {
            //    title: 'Email',
            //    width: '23%',
            //    inputClass: 'validate[required,custom[email]]',
            //    input: function (data) {
            //        var input = '<input type="text" name="MAIL" maxlength="50" placeholder="Informe o E-mail" ';
            //        if (data.record) {
            //            input += 'value="' + data.record.MAIL + '" ';
            //        }
            //        input += '/>';
            //        return input;
            //    }
            //},
            //TIPO_PESSOA: {
            //    title: 'Tipo de pessoa:',
            //    type: 'radiobutton',
            //    options: { '0': 'Pessoa Física', '1': 'Pessoa Jurídica' },
            //    defaultValue: '1',
            //    list: false
            //},
            ErrorDate: {
                title: 'Data',
                display: function (data) {
                    return moment(data.record.ErrorDate).format('DD/MM/YYYY HH:mm:ss');
                }
            },
            //CNPJ: {
            //    title: 'CNPJ',
            //    width: '23%',
            //    list: false
            //},
            //CPF_CNPJ: {
            //    title: 'CPF/CNPJ',
            //    width: '23%',
            //    display: function (data) {
            //        //TODO: Format/Mask 
            //        return data.record.CPF ? MaskCPF(data.record.CPF) : MaskCNPJ(data.record.CNPJ);
            //    },
            //    create: false,
            //    edit: false,
            //},
            //LOGIN: {
            //    title: 'Login',
            //    width: '23%',
            //    inputClass: 'validate[required]',
            //    input: function (data) {
            //        var input = '<input type="text" name="LOGIN" maxlength="50" placeholder="Informe o Login" ';
            //        if (data.record) {
            //            input += 'value="' + data.record.LOGIN + '" ';
            //        }
            //        input += '/>';
            //        return input;
            //    }
            //},
            //PASSWORD: {
            //    type: 'password',
            //    title: 'Senha',
            //    width: '23%',
            //    inputClass: 'validate[required]',
            //    input: function (data) {
            //        var input = '<input type="text" name="PASSWORD" maxlength="50" placeholder="Informe a Senha" ';
            //        if (data.record) {
            //            input += 'value="' + data.record.PASSWORD + '" ';
            //        }
            //        input += '/>';
            //        return input;
            //    },
            //    list: false,
            //    edit: false,
            //},
        },
        methods: {
            formCreated: function (event, data) {
                //data.form.find('input[name="CODE"]').addClass('validate[required]');
                //data.form.find('input[name="NAME"]').addClass('validate[required]');
                //data.form.validationEngine();
            },
            formSubmitting: function (event, data) {
                clearCacheMaster = true;
                data.form.validationEngine('attach', {
                    promptPosition: 'topLeft'
                });
                return data.form.validationEngine('validate');
            },
            //Dispose validation logic when form is closed
            formClosed: function (event, data) {
                //data.form.validationEngine('hide');
                //data.form.validationEngine('detach');
            },
            completeCallback: function (data) { //opened handler
                data.childTable.jtable('load');
            }
        }
    });
}

function LoadGridMasterOnReturn(e) {
    if (e.keyCode == 13) {
        LoadGridMaster();
    }
}

function InitializeMaster() {
    //$("#btSearch", "#fieldset").click(function () {
    //    LoadGridMaster();
    //});

    CreateGridMaster();
    LoadGridMaster();

    //$("#txtName", "#fieldset").keypress(LoadGridMasterOnReturn);
    //$("#txtMail", "#fieldset").keypress(LoadGridMasterOnReturn);
    //$("#txtCPF", "#fieldset").keypress(LoadGridMasterOnReturn);
    //$("#txtCNPJ", "#fieldset").keypress(LoadGridMasterOnReturn);
    //$("#txtLogin", "#fieldset").keypress(LoadGridMasterOnReturn);

    //$("#txtCPF", "#fieldset").mask("999.999.999-99");
    //$("#txtCNPJ", "#fieldset").mask("99.999.999/9999-99");
}

$(document).ready(function () {
    InitializeMaster();
});

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