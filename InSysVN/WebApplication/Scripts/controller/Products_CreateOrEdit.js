$(document).ready(function () {
    app.component.FormatInput();
    app.component.DatePicker();
    $("#drdWarehouse").select2();
    $('#SelectCate').select2({
        placeholder: 'Nhập tên, Mã danh mục.'
    });
    $("#frmUpdateProduct").on("submit", function () {
        var data = $(this).serializeObject();
        data.InventoryNumber = getPosInt_AutoNumeric(data.InventoryNumber);
        data.Price = getMoney_AutoNumeric(data.Price);
        data.SellPrice = getMoney_AutoNumeric(data.SellPrice);
        data.ExpiredDate = GetValueDate($("#ExpiredDate").datepicker('getDate'))
        var message = '';
        if (data.ProductName.trim() == '') {
            message += '*Tên sản phẩm không để trống!';
        }
        if (data.Barcode.trim() == '') {
            message += '*Barcode sản phẩm không để trống!';
        }
        if (data.ComputeUnit.trim() == '') {
            message += '*Đơn vị tính không để trống!';
        }
        if (message != '') {
            notifyError(message);
        }
        else {
            _AjaxPost("/Products/InsertProduct", { data, WarehouseId: $("#drdWarehouse").val(), selectCate: $("#SelectCate").val() }, function (rs) {
                if (rs.success) {
                    if (rs.type == 0) {
                        notifySuccess('Thêm sản phẩm thành công');
                        setTimeout(function () {
                            window.location.href = "/Products/Index";
                        }, 1000);
                    }
                    else {
                        notifySuccess('Cập nhật sản phẩm thành công');
                        setTimeout(function () {
                            window.location.href = "/Products/Index";
                        }, 1000);
                    }
                    if (rs.message != '') notifyWarning(rs.message);

                }
                else {
                    notifyError(rs.message);
                }
            });
        }
        return false;
    });
});