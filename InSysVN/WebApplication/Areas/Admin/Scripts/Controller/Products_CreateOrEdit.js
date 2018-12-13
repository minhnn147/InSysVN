$(document).ready(function () {
    app.component.FormatInput();
    $("#btnselectImage").on("change", function () {
        var fileName = this.files[0].name;
        var oFReader = new FileReader();
        oFReader.readAsDataURL(this.files[0]);
        oFReader.onload = function (oFREvent) {
            $("#ImgProducts").attr('src', this.result);
            $("#ImgProducts").attr('title', fileName);
        }
    });

    $('#SelectCate').select2({
        placeholder: 'Nhập tên, Mã danh mục.'
    });

    $("#frmUpdateProduct").on("submit", function () {
        var data = $(this).serializeObject();
        data.Price = getMoney_AutoNumeric(data.Price);
        data.ProductCategory = $("#SelectCate").val();
        data.Description = CKEDITOR.instances['Description'].getData();
        data.Specifications = CKEDITOR.instances['Specifications'].getData();
        var message = '';
        if (data.ProductName.trim() == '') {
            message += '*Tên sản phẩm không để trống!';
        }        
        if (data.ComputeUnit.trim() == '') {
            message += '*Đơn vị tính không để trống!';
        }
        if (message != '') {
            notifyError(message);
        }
        else {
            _AjaxPost(domain + '/Admin/Products/InsertProduct', {
                data,
                base64Avatar: $("#ImgProducts").attr('src'),
                fileName: $("#ImgProducts").attr('title')
            }, function (rs) {
                if (rs.success) {
                    if (rs.type == 0) {
                        notifySuccess('Thêm sản phẩm thành công');
                        setTimeout(function () {
                            window.location.href = domain + '/Admin/Products/Index';
                        }, 1000);
                    }
                    else {
                        notifySuccess('Cập nhật sản phẩm thành công');
                        setTimeout(function () {
                            window.location.href = domain + '/Admin/Products/Index';
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