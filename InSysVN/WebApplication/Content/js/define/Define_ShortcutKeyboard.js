(function (window, $) {
    window.EventKeyBoard = function (e) {
        if (e.ctrlKey == true && e.which == 66) {
            location.href = "/Orders/Create";
        }
        if (e.ctrlKey == true && e.which == 81) {
            location.href = "/Orders";
        }
    }
})(window, jQuery);

$(document).ready(function () {
    $(document).on("keyup", function (e) {
        EventKeyBoard(e);
    });
});