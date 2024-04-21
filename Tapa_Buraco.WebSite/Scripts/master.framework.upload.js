function FileUpload() {
    this.inProgress = false;
    this.formName = "";
    this.id = 0;
    this.divPanel = null;
    this.loadGridURL = "";
    this.uploadURI = "";
    this.onComplete = null;
}

FileUpload.prototype.SetProgress = function (value) {
    var progressBar = $(FileUpload.formName + '_progress');
    var percentageDiv = $(FileUpload.formName + '_percentage');

    if (progressBar != null) {
        var maxWidth = progressBar.attr('maxWidth');
        var width = maxWidth * value / 100;
        progressBar.css('width', width);
    }
    if (percentageDiv != null) percentageDiv.html(value + '%');
}
FileUpload.prototype.SetAnswer = function (value) {
    var answerDiv = $(FileUpload.formName + '_answer');
    if (answerDiv != null) answerDiv.html(value);
}

FileUpload.prototype.ResetToolbar = function () {
    FileUpload.SetProgress(0);
    FileUpload.SetAnswer("");
    FileUpload.Result("Nenhum arquivo processado.");
    //$("#" + Page.FileUpload.formName + " .result").html("Nenhum arquivo processado.");
    //$("#uploadResult").html("Nenhum arquivo processado.");
}

FileUpload.prototype.Result = function (msg) {
    $(FileUpload.formName + " .result").html(msg);
}
FileUpload.prototype.Success = function (data) {
    FileUpload.SetProgress(100);
    if (data.success) {
        FileUpload.SetAnswer('Arquivo carregado com sucesso.');
        var results = data.results;
        var htmlRes = '<table class="result-container">';
        htmlRes += '<tr><th>Container</th><th>Sucesso</th></tr>';
        for (var i = 0; i < results.length; i++) {
            for (var j = 0; j < results[i].Results.length; j++) {
                htmlRes += '<tr><td>' + results[i].Results[j].Number + '</td><td>' + (results[i].Results[j].Success ? 'Sim' : 'Não') + '</td></tr>';
            }
        }
        htmlRes += '</table>';
        FileUpload.Result(htmlRes);
        if (FileUpload.onComplete != null && FileUpload.onComplete != undefined) {
            FileUpload.onComplete(data);
        }
    }
    else {
        FileUpload.SetAnswer(data.error);
    }
    FileUpload.inProgress = false;
}

FileUpload.prototype.Upload = function (form) {
    if (!FileUpload.inProgress) {
        if (form != null) {
            FileUpload.formName = form.selector;

            if ($(FileUpload.formName + " input:file").val() == '') {
                FileUpload.SetAnswer('Selecione um arquivo para carregar.');
                FileUpload.SetProgress(0);
                return;
            }
            FileUpload.inProgress = true;

            form.ajaxForm({
                uploadProgress: function (event, position, total, percentComplete) {
                    FileUpload.SetProgress(percentComplete);
                },
                success: FileUpload.Success,
                error: function () {
                    FileUpload.SetAnswer('Arquivo não carregado.');
                    FileUpload.inProgress = false;
                },
                dataType: 'json',
                url: FileUpload.uploadURI,
                resetForm: true
            }).submit();
        }
    }
    else {
        FileUpload.SetAnswer('Arquivo sendo enviado');
    }
}

var FileUpload = new FileUpload();

; (function ($) {
    "use strict";
    $.fn.uploadFile = function (uri, onComplete) {
        FileUpload.uploadURI = uri;
        FileUpload.onComplete = onComplete;
        FileUpload.Upload(this);
    };
})(jQuery);

