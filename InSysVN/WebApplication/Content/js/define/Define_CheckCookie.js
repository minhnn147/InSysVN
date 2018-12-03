$(document).ready(function () {
    var cookieStall = $("#cookieStall").text();
    if (cookieStall == null || cookieStall == "") {
        $.ajax({
            url: "/Stall/GetOrSetStall",
            type: "POST",
            success: function (rs) {
                if (rs.success) {
                    $("#cookieStall").text(rs.StallName)
                }
                else {
                    alert("Đã có lỗi không xác định được QUẦY. Vui lòng liên hệ kỹ thuật viên!");
                    notifyError("Đã có lỗi không xác định được QUẦY. Vui lòng liên hệ kỹ thuật viên!");
                }
            }
        });
    }
});