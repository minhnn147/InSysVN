$(document).ready(function () {
    app.component.DatePicker();
    app.component.ValidateInputPhone();
    app.component.ValidateEmail();
    $("#frmUpdateUser").on("submit", function () {
        var data = $(this).serializeObject();
        data.Birthday = GetValueDate($('#Birthday').datepicker('getDate'));
        var message = '';
        if (data.FullName.trim() == '') {
            message += '*Vui lòng nhập tên khách hàng!'
        }
        if (message != '') {
            notifyError(message);
        }
        else {
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
                        if (!rs.IsStaff) {
                            setTimeout(function () {
                                window.location.href = "/Users/Index";
                            }, 1000);
                        }
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
        }
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