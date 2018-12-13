$(document).ready(function () {
    $("#btnselectImage").on("change", function () {
        var fileName = this.files[0].name;
        var oFReader = new FileReader();
        oFReader.readAsDataURL(this.files[0]);
        oFReader.onload = function (oFREvent) {
            $("#ImgCate").attr('src', this.result);
            $("#ImgCate").attr('title', fileName);            
        }
    });
    $("#frmCreateCate").on("submit", function () {
        var data = $(this).serializeObject();
        data.Description = CKEDITOR.instances['Description'].getData();
        _AjaxPost(domain + '/Admin/Category/InsertCate', {
            category: data,
            base64Avatar: $("#ImgCate").attr('src'),
            fileName: $("#ImgCate").attr('title')
        }, function (rs) {
            if (rs.success) {
                if (rs.type == 0) {
                    notifySuccess("Thêm mới thành công!")
                    setTimeout(function () {
                        window.location.href = "/Admin/Category/Index";
                    }, 1000);
                }
                else {
                    notifySuccess("Cập nhật thành công!")
                    setTimeout(function () {
                        window.location.href = "/Admin/Category/Index";
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
});