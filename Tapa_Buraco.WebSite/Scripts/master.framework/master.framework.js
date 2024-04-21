function Page() {
    this.Keyboard = new Keyboard;
    this.JTableMessages = {
        serverCommunicationError: 'Falha ao tentar se comunicar com o servidor.',
        loadingMessage: 'Carregando...',
        noDataAvailable: 'Sem registros.',
        addNewRecord: 'Novo Registro',
        editRecord: 'Editar Registro',
        areYouSure: 'Você tem certeza?',
        deleteConfirmation: 'Esse registro será removido. Você tem certeza?',
        save: 'Salvar',
        saving: 'Salvando',
        cancel: 'Cancelar',
        deleteText: 'Remover',
        deleting: 'Removendo',
        error: 'Erro',
        close: 'Fechar',
        cannotLoadOptionsFor: 'Can not load options for field {0}',
        pagingInfo: 'Registros {0}-{1} de {2}',
        pageSizeChangeLabel: 'Qtd Registros',
        gotoPageLabel: 'Página',
        canNotDeletedRecords: 'Can not deleted {0} of {1} records!',
        deleteProggress: 'Removendo registro {0} de {1}, processando...'
    };
}


function JTableEvents() {
    this.closeRequested = null,
    this.formClosed = null,
    this.formCreated = null,
    this.formSubmitting = null,
    this.loadingRecords = null,
    this.recordAdded = null,
    this.recordDeleted = null,
    this.recordsLoaded = null,
    this.recordUpdated = null,
    this.rowInserted = null,
    this.rowsRemoved = null,
    this.rowUpdated = null,
    this.selectionChanged = null
}



function Keyboard() {
    this.Keys = {
        Enter: "return",
        Ctrln: "Ctrl+n",
        Ctrle: "Ctrl+e",
        Ctrlq: "Ctrl+q",
        Ctrls: "Ctrl+s",
        Ctrlr: "Ctrl+r",
        Ctrll: "Ctrl+l"
    }
}



Page.prototype.SimpleAlert = function (text, isError) {
    var n = noty({
        text: text,
        layout: "center",
        modal: true,
        closeWith: ['click'],
        type: isError ? "error" : "alert"
        //closeWith: ['button'],
        //buttons: [{
        //    addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
        //        $noty.close();
        //    }
        //}]
    });
}

Page.prototype.GoTo = function (url) {
    window.location = url;
};

Page.prototype.JoinToString = function (obj) {
    var checkeds = obj.map(function () { return $(this).val(); }).get().join(",");
    return checkeds;
};

Keyboard.prototype.Enter = function (obj, callback) {
    jQuery(obj).bind('keydown', Page.Keyboard.Keys.Enter, function (evt) {
        callback();
        return false;
    });
}
Keyboard.prototype.Any = function (key, obj, callback) {
    if (!Page.Keyboard.Enabled) { return; }
    jQuery(obj).bind('keydown', key, function (evt) {
        callback();
        return false;
    });
}


; (function ($) {
    "use strict";
    $.noty.defaults = {
        layout: 'top',
        theme: 'defaultTheme',
        type: 'alert',
        text: '',
        dismissQueue: true, // If you want to use queue feature set this true
        template: '<div class="noty_message"><span class="noty_text"></span><div class="noty_close"></div></div>',
        animation: {
            open: { height: 'toggle' },
            close: { height: 'toggle' },
            easing: 'swing',
            speed: 500 // opening & closing animation speed
        },
        timeout: false, // delay for closing event. Set false for sticky notifications
        force: false, // adds notification to the beginning of queue when set to true
        modal: false,
        maxVisible: 5, // you can set max visible notification for dismissQueue true option
        closeWith: ['click'], // ['click', 'button', 'hover']
        callback: {
            onShow: function () { },
            afterShow: function () { },
            onClose: function () { },
            afterClose: function () { }
        },
        buttons: false // an array of buttons
    };

    $.fn.LoadGrid = function (args) {
        this.jtable('load', args.Parameters, args.OnComplete);
    };

    $.fn.CreateGrid = function (args)
    {
        var fulloptions = args.options;

        fulloptions.actions = args.actions;
        fulloptions.fields = args.fields;
        fulloptions.closeRequested = args.methods.closeRequested;
        fulloptions.formClosed = args.methods.formClosed;
        fulloptions.formCreated = args.methods.formCreated;
        fulloptions.formSubmitting = args.methods.formSubmitting;
        fulloptions.loadingRecords = args.methods.loadingRecords;
        fulloptions.recordAdded = args.methods.recordAdded;
        fulloptions.recordDeleted = args.methods.recordDeleted;
        fulloptions.recordsLoaded = args.methods.recordsLoaded;
        fulloptions.recordUpdated = args.methods.recordUpdated;
        fulloptions.rowInserted = args.methods.rowInserted;
        fulloptions.rowsRemoved = args.methods.rowsRemoved;
        fulloptions.rowUpdated = args.methods.rowUpdated;
        fulloptions.selectionChanged = args.methods.selectionChanged;
        fulloptions.messages = Page.JTableMessages;

        if (args.tableType == "child")
        {
            this.jtable("openChildTable", args.row, fulloptions, args.methods.completeCallback);
        }
        else {
            this.jtable(fulloptions);
        }
    }

    $.fn.Grid = function (args) {
        this.CreateGrid(args);
    };


    jQuery.migrateMute = false;

})(jQuery);

$(function () {

});

var Page = new Page();

/************************************************************************
* CUSTOM BUTTONS extension for jTable                              *
*************************************************************************/
(function ($) {
    //Reference to base object members
    var base = {
        _addColumnsToHeaderRow: $.hik.jtable.prototype._addColumnsToHeaderRow,
        _addCellsToRowUsingRecord: $.hik.jtable.prototype._addCellsToRowUsingRecord
    };

    $.extend(true, $.hik.jtable.prototype, {
        options: {
            customButtons: {
                items: [] // fields: icon, text, click, position (left or right)
            }
        },

        /* Overrides base method to add custom buttons to header row.
         *************************************************************************/
        _addColumnsToHeaderRow: function ($tr) {
            base._addColumnsToHeaderRow.apply(this, arguments);
            for (var i = 0; i < this.options.customButtons.items.length; i++) {
                if (this.options.customButtons.items[i].position == 'left') {
                    $tr.prepend(this._createEmptyCommandHeader());
                } else {
                    $tr.append(this._createEmptyCommandHeader());
                }
            }
        },

        /* Overrides base method to add custom buttons to a row.
         *************************************************************************/
        _addCellsToRowUsingRecord: function ($row) {
            base._addCellsToRowUsingRecord.apply(this, arguments);

            var self = this;
            for (var i = 0; i < this.options.customButtons.items.length; i++) {
                var item = self.options.customButtons.items[i];
                $button = self._createCustomButton(item, $row.data('record'));
                var $buttonCell = $('<td></td>').addClass('jtable-command-column').append($button);
                if (item.position == 'left') {
                    $buttonCell.prependTo($row);
                } else {
                    $buttonCell.appendTo($row);
                }
            }
        },

        _createCustomButton: function (item, record) {
            var $button = $('<button title="' + item.text + '"></button>');
            $button.css('background', 'url(' + (item.icon ? item.icon : '../default-16x16.png') + ') no-repeat');
            $button.css('width', '16px');
            $button.css('height', '16px');
            $button.addClass('jtable-command-button');
            $button.append($('<span></span>').html(item.text));
            if (item.click) {
                $button.click(function () {
                    item.click(record);
                });
            }
            return $button;
        }
    });

})(jQuery);