$(document).ready(function () {
    $('#main-menu').smartmenus({
        mainMenuSubOffsetX: -1,
        mainMenuSubOffsetY: 4,
        subMenusSubOffsetX: 6,
        subMenusSubOffsetY: -6
    });
});

; (function ($) {
    "use strict";
    $.fn.LoadDropDown = function (action) {
        var select = $(this);

        Page.AjaxService.Request(
            action,
            { },
            //beforeSend: function( xhr ) {
            //    select.prepend(
            //        $('<option>', { value: '' }).text('Carregando...')
            //    );
            //},
            function (data) {
                //$('option:first', select).remove();

                if (data.Result == 'OK') {
                    $.each(data.Options, function (index, item) {
                        select.append(
                            $('<option>', { value: item.Value }).text(item.DisplayText)
                        );
                    });
                }
            },
            null
        );
    };

})(jQuery);
