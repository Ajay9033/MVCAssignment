(function (factory, undefined) {
    if (typeof define === 'function' && define.amd) {
        // AMD
        define(['jquery'], factory);
    } else if (typeof module === 'object' && typeof module.exports === 'object') {
        // CommonJS
        module.exports = factory(require('jquery'));
    } else {
        // Global jQuery
        factory(jQuery);
    }
}(function ($, undefined) {
    $.fn.resizableTableColumns = function (opt) {
        opt = $.extend({
            resizeHeight: false,
            handleSelector: "> .resizer",
        }, opt);

        return this.each(function () {
            $(this)
                .css({ position: "relative" })
                .prepend("<div class='resizer'></div>")
                .resizableSafe(opt);
        });
    };
}));