$(document).ready(function () {
    app.component.DatePicker();
    app.component.ValidateInputPhone();
    $("#WarehouseId").select2({});
    $("#RoleId").select2({});
    $("#Phone").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
    $("#frmUpdateUser").on("submit", function () {
        var data = $(this).serializeObject();
        data.Birthday = GetValueDate($('#Birthday').datepicker('getDate'));
        _AjaxPost("/Users/SaveUser", {
            user: data,
            base64Avatar: $("#ImgAvatar").attr('src'),
            fileName: $("#ImgAvatar").attr('title')
        }, function (rs) {
            if (rs.success) {
                if (rs.type == 0) {
                    notifySuccess("Thêm mới thành công!")
                    setTimeout(function () {
                        window.location.href = "/Users/Index";
                    }, 1000);
                }
                else {
                    notifySuccess("Cập nhật thành công!")
                    setTimeout(function () {
                        window.location.href = "/Users/Index";
                    }, 1000);
                }
            }
            else {
                if (rs.warning) {
                    notifyWarning("Tài khoản đã tồn tại!")
                }
                else {
                    notifyError("Có lỗi xảy ra!");
                }
            }
        });
        return false;
    });
    $("#ipAvatar").on("change", function () {
        var fileName = this.files[0].name;
        var oFReader = new FileReader();
        oFReader.readAsDataURL(this.files[0]);
        oFReader.onload = function (oFREvent) {
            $("#ImgAvatar").attr('src', this.result);
            $("#ImgAvatar").attr('title', fileName);
        }
    });
});