var url_Base_Site = "";

$(document).ready(function ()
{
    url_Base_Site = $('#url_Base_Site').data('request-url');

    $("#textBoxPassword").keypress(function (e) {
        if (e.which == 13) {

            if (ValidationLoginForm()) {
                DoLogin();
            }
        }
    });

    //$("#buttonLogOut").hide();
    $("#box-changePassword").hide();
    $("#box-forgotPassword").hide();
    $("#box-register").hide();
    $("#box-reset").hide();

    $("#buttonLogIn").click(function () {
        if (ValidationLoginForm()) {
            DoLogin();
        }
    });

    $("#buttonChangePassword").click(function () {
        if (ValidationChangePasswordForm())
        {
            DoChangePassword();
        }
    });

    $("#buttonConfirmarCPF").click(function ()
    {
        if (ValidationConfirmarCPFForgotPasswordForm())
        {
            DoConfirmarCPFForgotPassword();
        }
    });

    $("#buttonConfirmarForgotPassword").click(function () {
        if (ValidationConfirmarForgotPasswordForm()) {
            DoConfirmarForgotPassword();
        }
    });

    $("#buttonRegister").click(function () {
        if (ValidationRegisterForm()) {
            DoRegister();
        }
    });

    $("#buttonRecoverPassword").click(function () {
        if (ValidationRecoverPasswordForm()) {
            DoRecoverPassword();
        }
    });


    $("#buttonLoginRegister").click(function ()
    {
        $("#box-register").show();

        $("#box-changePassword").hide();
        $("#box-forgotPassword").hide();
        $("#box-login").hide();
        $("#box-reset").hide();
    });

    $("#buttonAlterPassword").click(function () {        
        $("#box-changePassword").show();

        $("#textBoxLoginChangePassword").parent().removeClass("has-error");
        $("#textBoxAtualPasswordChangePassword").parent().removeClass("has-error");
        $("#textBoxNovoPasswordChangePassword").parent().removeClass("has-error");
        $("#textBoxConfirmacaoNovoPasswordChangePassword").parent().removeClass("has-error");

        $("#textBoxLoginChangePassword").val($("#textBoxLogin").val());
        $("#textBoxAtualPasswordChangePassword").val("");
        $("#textBoxNovoPasswordChangePassword").val("");
        $("#textBoxConfirmacaoNovoPasswordChangePassword").val("");

        $("#box-register").hide();
        $("#box-forgotPassword").hide();
        $("#box-login").hide();
        $("#box-reset").hide();
    });

    $("#buttonForgotPassword").click(function () {
        $("#box-forgotPassword").show();
        
        $("#textBoxRECPWD_CPF").parent().removeClass("has-error");
        $("#textBoxRECPWD_Login").parent().removeClass("has-error");
        $("#textBoxRECPWD_CodigoVerificacao").parent().removeClass("has-error");
        $("#textBoxRECPWD_NovaSenha").parent().removeClass("has-error");
        $("#textBoxRECPWD_ConfirmacaoNovaSenha").parent().removeClass("has-error");

        $("#textBoxRECPWD_CPF").val("");
        $("#textBoxRECPWD_Login").val("");
        $("#textBoxRECPWD_CodigoVerificacao").val("");
        $("#textBoxRECPWD_NovaSenha").val("");
        $("#textBoxRECPWD_ConfirmacaoNovaSenha").val("");

        $('#textBoxRECPWD_CPF').removeAttr('disabled');
        $('#buttonConfirmarCPF').removeAttr('disabled');

        $('#divCPFCONFIRMED').removeClass("in");
        $("#divCPFCONFIRMED").addClass("collapse");

        $("#box-register").hide();
        $("#box-changePassword").hide();
        $("#box-login").hide();
        $("#box-reset").hide();
    });

    //$("#textBoxRECPWD_CPF").mask("999.999.999-99",
    //{
    //    completed: function () { }
    //});

    $("#textBoxRECPWD_CPF").mask("999.999.999-99");

    $(".buttonHaveAccess").click(function () {
        $("#box-register").hide();
        $("#box-changePassword").hide();
        $("#box-forgotPassword").hide();
        $("#box-reset").hide();
        $("#box-login").show();
    });

    //$("#buttonLoginRememberPassword").click(function () {
    //$("#box-register").hide();
    //$("#box-reset").show();
    //$("#box-login").hide();
//});

    //$('#dialog-New').dialog({
    //    modal: true,
    //    height: 500,
    //    width: 400,
    //    open: function() {
    //        $(this).parent().find(".ui-dialog-titlebar-close").html("x");
    //        $(this).parent().find(".ui-dialog-buttonpane").find('input[type="submit"] ,button').addClass("btn btn-sm");
    //    },
    //    buttons: {
    //        Cancelar: function () {
    //            $(this).dialog("close");


    //        },
    //        Salvar: function () {

    //        }
    //    },

    //    close: function (ev, ui) {

    //    },


    //}).dialog('open');
});

function DoLogin() {
    $('#buttonLogIn').attr('disabled', 'disabled');
    $('#buttonLogIn').text('Aguarde...');
    Page.AjaxService.Request(url_Base_Site + '/Login/DoLogin', DoLoginReadData(), DoLoginReadCallBack, ReadCallBackError);
}

function DoLoginReadCallBack(obj) {
    if (obj.Result)
    {
        ShowMessage(obj.Message, 1);
        if (obj.ReturnUrl != null && obj.ReturnUrl != "")
            window.location = url_Base_Site + obj.ReturnUrl;
        else
        {
            window.location = url_Base_Site + '/Home/Index';
        }
            
    }
    else {
        ShowMessage(obj.Message, 4);
        $('#buttonLogIn').removeAttr('disabled');
        $('#buttonLogIn').text('Entrar');
    }
}

function DoLoginReadData() {
    return { LogIn: $("#textBoxLogin").val(), Password: $("#textBoxPassword").val() };
}


function DoChangePassword()
{
    $('#buttonChangePassword').attr('disabled', 'disabled');
    $('#buttonChangePassword').text('Aguarde...');
    Page.AjaxService.Request(url_Base_Site + '/Login/DoChangePassword', DoChangePasswordReadData(), DoChangePasswordReadCallBack, ReadCallBackError);
}

function DoChangePasswordReadData() {
    return { LogIn: $("#textBoxLoginChangePassword").val(), PasswordAntigo: $("#textBoxAtualPasswordChangePassword").val(), PasswordNovo: $("#textBoxNovoPasswordChangePassword").val(), PasswordConfirmacaoNovo: $("#textBoxConfirmacaoNovoPasswordChangePassword").val() };
}

function DoChangePasswordReadCallBack(obj)
{
    $('#buttonChangePassword').removeAttr('disabled');
    $('#buttonChangePassword').text('Alterar Senha');

    if (obj.Result)
    {
        ShowMessage(obj.Message, 1);
        $("#box-login").show();
        $("#textBoxLogin").val($("#textBoxLoginChangePassword").val());
        $("#textBoxPassword").val("");

        $("#box-register").hide();
        $("#box-changePassword").hide();


        $("#box-reset").hide();
    }
    else {
        ShowMessage(obj.Message, 4);
    }
}


function DoConfirmarCPFForgotPassword() {
    $('#buttonConfirmarCPF').attr('disabled', 'disabled');
    $('#buttonConfirmarCPF').text('Aguarde...');
    Page.AjaxService.Request(url_Base_Site + '/Login/DoConfirmarCPFForgotPassword', DoConfirmarCPFForgotPasswordReadData(), DoConfirmarCPFForgotPasswordReadCallBack, ReadCallBackError);
}

function DoConfirmarCPFForgotPasswordReadData() {
    return { CPF: $("#textBoxRECPWD_CPF").val() };
}

function DoConfirmarCPFForgotPasswordReadCallBack(obj)
{
    $('#buttonConfirmarCPF').removeAttr('disabled');
    $('#buttonConfirmarCPF').text('Confirme o CPF');

    if (obj != undefined && obj.Result != null && obj.Result && obj.Record != null)
    {
        ShowMessage(obj.Message, 1);

        var objRet = obj.Record;

        $('#textBoxRECPWD_CPF').attr('disabled', 'disabled');
        $('#buttonConfirmarCPF').attr('disabled', 'disabled');

        $('#divCPFCONFIRMED').removeClass("collapse");
        $("#divCPFCONFIRMED").addClass("in");

        $('#textBoxRECPWD_Login').val(objRet.LogIn);
        $('#textBoxRECPWD_CodigoVerificacao').val('');
        $('#textBoxRECPWD_NovaSenha').val('');
        $('#textBoxRECPWD_ConfirmacaoNovaSenha').val('');
    }
    else
    {
        if (obj != undefined)
            ShowMessage(obj.Message, 4);
    }
}


function DoConfirmarForgotPassword()
{
    $('#buttonConfirmarForgotPassword').attr('disabled', 'disabled');
    $('#buttonConfirmarForgotPassword').text('Aguarde...');
    Page.AjaxService.Request(url_Base_Site + '/Login/DoConfirmarForgotPassword', DoConfirmarForgotPasswordReadData(), DoConfirmarForgotPasswordReadCallBack, ReadCallBackError);
}

function DoConfirmarForgotPasswordReadData()
{
    return { CPF: $("#textBoxRECPWD_CPF").val(), LogIn: $("#textBoxRECPWD_Login").val(), CodigoVerificacao: $("#textBoxRECPWD_CodigoVerificacao").val(), SenhaNova: $("#textBoxRECPWD_NovaSenha").val(), SenhaNovaConfirmacao: $("#textBoxRECPWD_ConfirmacaoNovaSenha").val() };
}

function DoConfirmarForgotPasswordReadCallBack(obj)
{
    $('#buttonConfirmarForgotPassword').removeAttr('disabled');
    $('#buttonConfirmarForgotPassword').text('Redefinir Senha');

    if (obj != undefined && obj.Result != null && obj.Result)
    {
        ShowMessage(obj.Message, 1);
        $("#box-login").show();

        $("#textBoxLogin").val($("#textBoxRECPWD_Login").val());
        $("#textBoxPassword").val("");

        $('#textBoxRECPWD_CPF').removeAttr('disabled');
        $('#buttonConfirmarCPF').removeAttr('disabled');

        $('#textBoxRECPWD_CPF').val('');
        $('#textBoxRECPWD_Login').val('');
        $('#textBoxRECPWD_CodigoVerificacao').val('');
        $('#textBoxRECPWD_NovaSenha').val('');
        $('#textBoxRECPWD_ConfirmacaoNovaSenha').val('');

        $('#divCPFCONFIRMED').removeClass("in");
        $("#divCPFCONFIRMED").addClass("collapse");

        $("#box-register").hide();
        $("#box-changePassword").hide();
        $("#box-forgotPassword").hide();

        $("#box-reset").hide();
    }
    else
    {
        if (obj != undefined)
            ShowMessage(obj.Message, 4);
    }
}


function DoRegister()
{
    Page.AjaxService.Request(url_Base_Site + '/Login/DoRegister', DoRegisterReadData(), DoRegisterReadCallBack, ReadCallBackError);
}

function DoRegisterReadCallBack(obj) {
    if (obj.Result) {
        ShowMessage(obj.Message, 1);
        $("#box-register").hide();
        $("#box-reset").hide();
        $("#box-login").show();
    }
    else {
        ShowMessage(obj.Message, 4);
    }
}

function DoRegisterReadData() {
    return { LogIn: $("#textBoxName").val(), Email: $("#textBoxEmail").val(), Name: $("#textBoxName").val(), Password: $("#textBoxPasswordRegister").val() };
}


function ReadCallBackError(obj)
{
    if (obj != undefined)
        ShowMessage(obj.Message, 4);

    $('#buttonLogIn').removeAttr('disabled');
    $('#buttonLogIn').text('Entrar');

    $('#buttonChangePassword').removeAttr('disabled');
    $('#buttonChangePassword').text('Alterar Senha');

    $('#buttonConfirmarCPF').removeAttr('disabled');
    $('#buttonConfirmarCPF').text('Confirme o CPF');

    $('#buttonConfirmarForgotPassword').removeAttr('disabled');
    $('#buttonConfirmarForgotPassword').text('Redefinir Senha');
}

function ValidationRegisterForm() {

    var isValidated = true;
    var message = "Preencha todos os campos solicitados!";

    if ($("#textBoxName").val() == "") {
        $("#textBoxName").parent().addClass("has-error");
        //ShowMessage("Preencha o seu Nome!", 4);
        isValidated = false;
    }
    else {
        $("#textBoxName").parent().removeClass("has-error");
    }

    if ($("#textBoxEmail").val() == "") {
        $("#textBoxEmail").parent().addClass("has-error");
        //ShowMessage("Preencha o seu E-mail!", 4);
        isValidated = false;
    }
    else {
        var regexEmail = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

        if (regexEmail.test($("#textBoxEmail").val())) {
            $("#textBoxEmail").parent().removeClass("has-error");
        }
        else {
            $("#textBoxEmail").parent().addClass("has-error");
            //ShowMessage("Digite um E-mail válido!", 4);
            isValidated = false;
        }
    }

    if ($("#textBoxPasswordRegister").val() == "") {
        $("#textBoxPasswordRegister").parent().addClass("has-error");
        //ShowMessage("Preencha a sua Senha!", 4);
        isValidated = false;
    }
    else {
        $("#textBoxPasswordRegister").parent().removeClass("has-error");
    }

    if ($("#textBoxPasswordConfirm").val() == "") {
        $("#textBoxPasswordConfirm").parent().addClass("has-error");
        //ShowMessage("Confirme a sua Senha!", 4);
        isValidated = false;
    }
    else {
        $("#textBoxPasswordConfirm").parent().removeClass("has-error");

        if ($("#textBoxPasswordRegister").val() != $("#textBoxPasswordConfirm").val()) {
            $("#textBoxPasswordRegister").parent().addClass("has-error");
            $("#textBoxPasswordConfirm").parent().addClass("has-error");
            message ="A sua senha precisa ser igual a senha de confirmação!";
            isValidated = false;
        }
        else {
            $("#textBoxPasswordRegister").parent().removeClass("has-error");
            $("#textBoxPasswordConfirm").parent().removeClass("has-error");
        }
    }

    if (!isValidated)
        ShowMessage(message, 4);

    return isValidated;
}

function ValidationLoginForm() {

    var isValidated = true;
    var message = "Preencha todos os campos solicitados!";

    if ($("#textBoxLogin").val() == "") {
        $("#textBoxLogin").parent().addClass("has-error");
        //ShowMessage("Preencha o seu Login!", 4);
        isValidated = false;
    }
    else {
        $("#textBoxLogin").parent().removeClass("has-error");
    }

    if ($("#textBoxPassword").val() == "") {
        $("#textBoxPassword").parent().addClass("has-error");
        //ShowMessage("Preencha a sua Senha!", 4);
        isValidated = false;
    }
    else {
        $("#textBoxPassword").parent().removeClass("has-error");
    }

    if (!isValidated)
        ShowMessage(message, 4);

    return isValidated;
}

function ValidationChangePasswordForm() {

    var isValidated = true;
    var message = "Preencha todos os campos solicitados!";

    if ($("#textBoxLoginChangePassword").val() == "") {
        $("#textBoxLoginChangePassword").parent().addClass("has-error");
        isValidated = false;
    }
    else {
        $("#textBoxLoginChangePassword").parent().removeClass("has-error");
    }

    if ($("#textBoxAtualPasswordChangePassword").val() == "") {
        $("#textBoxAtualPasswordChangePassword").parent().addClass("has-error");
        isValidated = false;
    }
    else {
        $("#textBoxAtualPasswordChangePassword").parent().removeClass("has-error");
    }

    if ($("#textBoxNovoPasswordChangePassword").val() == "") {
        $("#textBoxNovoPasswordChangePassword").parent().addClass("has-error");
        isValidated = false;
    }
    else {
        $("#textBoxNovoPasswordChangePassword").parent().removeClass("has-error");
    }

    if ($("#textBoxConfirmacaoNovoPasswordChangePassword").val() == "") {
        $("#textBoxConfirmacaoNovoPasswordChangePassword").parent().addClass("has-error");
        isValidated = false;
    }
    else {
        $("#textBoxConfirmacaoNovoPasswordChangePassword").parent().removeClass("has-error");
    }

    if (isValidated)
    {//ultima validacao - se todas as outras passaram...
        if ($("#textBoxConfirmacaoNovoPasswordChangePassword").val() != $("#textBoxNovoPasswordChangePassword").val()) {
            $("#textBoxNovoPasswordChangePassword").parent().addClass("has-error");
            $("#textBoxConfirmacaoNovoPasswordChangePassword").parent().addClass("has-error");

            message = "Nova senha e confirmação de nova senha não conferem!!!";
            isValidated = false;
        }
        else
        {
            $("#textBoxNovoPasswordChangePassword").parent().removeClass("has-error");
            $("#textBoxConfirmacaoNovoPasswordChangePassword").parent().removeClass("has-error");
        }
    }

    if (!isValidated)
        ShowMessage(message, 4);

    return isValidated;
}


function ValidationConfirmarCPFForgotPasswordForm()
{
    var isValidated = true;
    var message = "Preencha todos os campos solicitados!";

    if ($("#textBoxRECPWD_CPF").val() != null && $("#textBoxRECPWD_CPF").val() != "" &&
        $("#textBoxRECPWD_CPF").val().toString().trim() != "")
    {
        if ($("#textBoxRECPWD_CPF").val().toString().trim().length != 14 ||
            $("#textBoxRECPWD_CPF").val().toString().trim() == "___.___.___-__" ||
            $("#textBoxRECPWD_CPF").val().toString().trim() == "000.000.000-00" ||
            $("#textBoxRECPWD_CPF").val().toString().trim() == "111.111.111-11" ||
            $("#textBoxRECPWD_CPF").val().toString().trim() == "222.222.222-22" ||
            $("#textBoxRECPWD_CPF").val().toString().trim() == "333.333.333-33" ||
            $("#textBoxRECPWD_CPF").val().toString().trim() == "444.444.444-44" ||
            $("#textBoxRECPWD_CPF").val().toString().trim() == "555.555.555-55" ||
            $("#textBoxRECPWD_CPF").val().toString().trim() == "666.666.666-66" ||
            $("#textBoxRECPWD_CPF").val().toString().trim() == "777.777.777-77" ||
            $("#textBoxRECPWD_CPF").val().toString().trim() == "888.888.888-88" ||
            $("#textBoxRECPWD_CPF").val().toString().trim() == "999.999.999-99") {
            $("#textBoxRECPWD_CPF").parent().addClass("has-error");
            message = "CPF inválido !!!";
            isValidated = false;
        }
        else
        {
            $("#textBoxRECPWD_CPF").parent().removeClass("has-error");
        }
    }
    else
    {
        $("#textBoxRECPWD_CPF").parent().addClass("has-error");
        message = "Campo CPF é obrigatório !!!";
        isValidated = false;
    }

    if (!isValidated)
        ShowMessage(message, 4);

    return isValidated;
}

function ValidationConfirmarForgotPasswordForm() {

    var isValidated = true;
    var message = "Preencha todos os campos solicitados!";

    if ($("#textBoxRECPWD_CPF").val() == "")
    {
        $("#textBoxRECPWD_CPF").parent().addClass("has-error");
        isValidated = false;
    }
    else
    {
        $("#textBoxRECPWD_CPF").parent().removeClass("has-error");
    }

    if ($("#textBoxRECPWD_Login").val() == "")
    {
        $("#textBoxRECPWD_Login").parent().addClass("has-error");
        isValidated = false;
    }
    else
    {
        $("#textBoxRECPWD_Login").parent().removeClass("has-error");
    }

    if ($("#textBoxRECPWD_CodigoVerificacao").val() == "")
    {
        $("#textBoxRECPWD_CodigoVerificacao").parent().addClass("has-error");
        isValidated = false;
    }
    else
    {
        $("#textBoxRECPWD_CodigoVerificacao").parent().removeClass("has-error");
    }

    if ($("#textBoxRECPWD_NovaSenha").val() == "")
    {
        $("#textBoxRECPWD_NovaSenha").parent().addClass("has-error");
        isValidated = false;
    }
    else
    {
        $("#textBoxRECPWD_NovaSenha").parent().removeClass("has-error");
    }

    if ($("#textBoxRECPWD_ConfirmacaoNovaSenha").val() == "")
    {
        $("#textBoxRECPWD_ConfirmacaoNovaSenha").parent().addClass("has-error");
        isValidated = false;
    }
    else
    {
        $("#textBoxRECPWD_ConfirmacaoNovaSenha").parent().removeClass("has-error");
    }

    if (isValidated)
    {//ultima validacao - se todas as outras passaram...
        if ($("#textBoxRECPWD_NovaSenha").val() != $("#textBoxRECPWD_ConfirmacaoNovaSenha").val())
        {
            $("#textBoxRECPWD_NovaSenha").parent().addClass("has-error");
            $("#textBoxRECPWD_ConfirmacaoNovaSenha").parent().addClass("has-error");

            message = "Nova senha e confirmação de nova senha não conferem!!!";
            isValidated = false;
        }
        else
        {
            $("#textBoxRECPWD_NovaSenha").parent().removeClass("has-error");
            $("#textBoxRECPWD_ConfirmacaoNovaSenha").parent().removeClass("has-error");
        }
    }

    if (!isValidated)
        ShowMessage(message, 4);

    return isValidated;
}


function DoRecoverPassword()
{
    Page.AjaxService.Request(url_Base_Site + '/Login/SendEmail', DoRecoverPasswordReadData(), DoRecoverPasswordReadCallBack, ReadCallBackError);
}

function DoRecoverPasswordReadCallBack(obj) {
    if (obj.Result) {
        ShowMessage(obj.Message, 1);
    }
    else {
        ShowMessage(obj.Message, 4);
    }
}

function DoRecoverPasswordReadData() {
    return { Email: $("#textBoxEmailRecoverPassword").val() };
}

function ValidationRecoverPasswordForm() {

    var isValidated = true;

    if ($("#textBoxEmailRecoverPassword").val() == "") {
        $("#textBoxEmailRecoverPassword").addClass("classError");
        ShowMessage("Preencha o seu E-mail!", 4);
        isValidated = false;
    }
    else {
        var regexEmail = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

        if (regexEmail.test($("#textBoxEmailRecoverPassword").val())) {
            $("#textBoxEmailRecoverPassword").removeClass("classError");
        }
        else {
            $("#textBoxEmailRecoverPassword").addClass("classError");
            ShowMessage("Digite um E-mail válido!", 4);
            isValidated = false;
        }
    }

    return isValidated;
}