$(document).ready(function () {
    $("#btnselectImage").on("change", function () {
        var fileName = this.files[0].name;
        var oFReader = new FileReader();
        oFReader.readAsDataURL(this.files[0]);
        oFReader.onload = function (oFREvent) {
            $("#ImgNews").attr('src', this.result);
            $("#ImgNews").attr('title', fileName);
        }
    });

    $("#frmCreateNews").on("submit", function () {
        var data = $(this).serializeObject();
        data.Content = CKEDITOR.instances['Content'].getData();
        var message = '';
        if (message != '') {
            notifyError(message);
        }
        else {
            _AjaxPost(domain + '/Admin/News/InsertNews', {
                newsentity: data,
                base64Avatar: $("#ImgNews").attr('src'),
                fileName: $("#ImgNews").attr('title')
            }, function (rs) {
                if (rs.success) {
                    if (rs.type == 0) {
                        notifySuccess('Thêm tin tức');
                        setTimeout(function () {
                            window.location.href = domain + '/Admin/News/Index';
                        }, 1000);
                    }
                    else {
                        notifySuccess('Cập nhật sản phẩm thành công');
                        setTimeout(function () {
                            window.location.href = domain + '/Admin/News/Index';
                        }, 1000);
                    }
                }
                else {
                    notifyError(rs.message);
                }
            });
        }
        return false;
    });
});