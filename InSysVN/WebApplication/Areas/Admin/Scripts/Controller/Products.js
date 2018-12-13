var product = {
    ui: {},
    registerControl: function () {
        product.loadproduct();
    },
    loadproduct: function () {
        $('#tableProducts').bootstrapTable({
            url: domain + '/Admin/Products/GetDataProducts',
            method: "POST",
            ajax: function (config) {
                app.component.Loading.Show();
                _AjaxPost(config.url, { obj: config.data }, function (rs) {
                    app.component.Loading.Hide();
                    if (rs.success) {
                        config.success({
                            total: rs.total,
                            rows: rs.data,
                            AllowEdit: rs.AllowEdit
                        });
                    }
                    else {
                        alert();
                        notifyError("Lỗi lấy dữ liệu!");
                    }
                });
            },
            striped: true,
            sidePagination: 'server',
            pagination: true,
            paginationVAlign: 'both',
            limit: 10,
            formatLoadingMessage: function () {
                return "Đang Tải";
            },
            pageSize: 10,
            pageList: [10, 25, 50, 100, 200],
            search: true,
            showColumns: true,
            showRefresh: true,
            minimumCountColumns: 2,
            toolbar: "#toolbar",
            columns: [
                {
                    field: 'CategoryName',
                    title: 'Danh Mục',
                    align: 'left',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value == null) {
                            return '';
                        }
                        else {
                            return value;
                        }
                    },
                },
                {
                    field: 'ProductName',
                    title: 'Tên sản phẩm',
                    align: 'left',
                    valign: 'middle',
                    sortable: true
                },
                {
                    field: "Image",
                    title: "Hình ảnh",
                    align: 'center',
                    valign: 'center',
                    formatter: function (value, row, index) {
                        if (value != null)
                            return "<img src='" + value + "'style='max-width:50px'/>";
                        return '';
                    }
                },
                {
                    field: 'ComputeUnit',
                    title: 'Đơn vị tính',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                },
                {
                    field: 'Price',
                    title: 'Giá',
                    align: 'right',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        return formatMoney(value);
                    },
                },
                {
                    field: "Description",
                    title: "Miêu tả"
                },
                {
                    field: "Specifications",
                    title: "Thông số kỹ thuật"
                },
                {
                    field: 'CreatedDate',
                    title: 'Ngày tạo',
                    align: 'center',
                    valign: 'middle',
                    searchable: false,
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value == null) {
                            return '';
                        }
                        else return formatToDate(value);
                    }
                },
                {
                    field: "Status",
                    title: "Kích hoạt",
                    formatter: function (value, row, index) {
                        if (row.Status)
                            return '<input type="checkbox" onclick="product.updateisActive(' + row.Id + ')" checked="checked">';
                        else
                            return '<input type="checkbox" onclick="product.updateisActive(' + row.Id + ')">';
                    }
                },
                {
                    title: "Thao tác",
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return '\
                                        <a class="edit ml10" href="'+ domain + '/Admin/Products/Edit/' + row.Id + '" title="Sửa danh mục">\
                                            <i class="fas fa-edit"></i>\
                                        </a>\
                                        <a data-toggle="modal" class="remove ml10" href="javascript:void(0)" title="Xóa danh mục">\
                                            <i class="fas fa-trash-alt"></i>\
                                        </a>';
                    },
                    events: {
                        'click .remove': function (e, value, row, index) {
                            modal.DeleteComfirm({
                                callback: function () {
                                    console.log(row.Id);
                                    $.ajax({
                                        type: 'POST',
                                        url: domain + '/admin/Products/delete/' + row.Id,
                                        success: function (data) {
                                            debugger;
                                            if (data.success) {
                                                $('#tableProducts')
                                                    .bootstrapTable('remove', {
                                                        field: 'Id',
                                                        values: [row.Id],

                                                    });
                                                notifySuccess("Xóa Danh mục " + row.Name + " thành công!")

                                                $('#tableProducts').bootstrapTable('refresh')
                                            } else {
                                                notifySuccess("Lỗi không xóa được")
                                            }

                                        },
                                        error: function (xhr, status, err) {
                                            notifySuccess("Có lỗi xảy ra!");
                                        }
                                    });
                                }
                            });
                        }
                    }
                },
            ],
            onLoadSuccess: function (data) {
                if (!data.AllowEdit) {
                    $("#tableProducts tr").find('.editable').editable('disable');
                }
            }
        });
    },
    updateisActive: function (productId) {
        $.ajax({
            url: domain + '/Admin/Products/UpdateIsActive',
            data: {
                productId: productId
            },
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res) {
                    notifySuccess('Cập nhật trạng thái thành công');
                    $('#tableProducts').bootstrapTable('refresh');
                }
                else {
                    notifyError('Cập nhật trạng thái thất bại');
                }
            }
        });
    }
};

$(document).ready(function () {
    product.registerControl();
});
