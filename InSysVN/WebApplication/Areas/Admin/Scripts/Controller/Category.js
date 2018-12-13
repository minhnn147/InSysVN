var category = {
    ui: {},
    registerControl: function () {
        category.loadCategory();
    },
    loadCategory: function () {
        $('#tblCategory').bootstrapTable({
            url: domain + '/Admin/Category/GetCateData',
            method: 'get',
            queryParams: function (p) {
                return {
                    txtSearch: $('#ipSearch').val(),
                }
            },
            limit: 10,
            pageList: [10, 25, 50, 100],
            pagination: true,
            paginationVAlign: 'both',
            sidePagination: 'server',
            columns: [
                {
                    field: "Name",
                    title: "Tên danh mục"
                },
                {
                    field: "ImagePath",
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
                    field: "Description",
                    title: "Chi tiết"
                },
                {
                    field: "isActive",
                    title: "Kích hoạt",
                    formatter: function (value, row, index) {
                        if (row.isActive)
                            return '<input type="checkbox" onclick="category.updateisActive(' + row.Id + ')" checked="checked">';
                        else
                            return '<input type="checkbox" onclick="category.updateisActive(' + row.Id + ')">';
                    }
                },
                {
                    title: "Thao tác",
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return '\
                                        <a class="edit ml10" href="'+ domain + '/Admin/Category/Eidt/' + row.Id + '" title="Sửa danh mục">\
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
                                        url: domain + '/admin/category/delete/' + row.Id,
                                        success: function (data) {
                                            debugger;
                                            if (data.success) {
                                                $('#tblCategory')
                                                    .bootstrapTable('remove', {
                                                        field: 'Id',
                                                        values: [row.Id],

                                                    });
                                                notifySuccess("Xóa Danh mục " + row.Name + " thành công!")

                                                $('#tblCategory').bootstrapTable('refresh')
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
                }
            ],
            onLoadSuccess: function (data) {
                if (data.total > 0 && data.rows.length == 0) {
                    $(this).bootstrapTable('refresh');
                };
            }
        });
    },
    updateisActive: function (cateId) {
        $.ajax({
            url: domain + '/Admin/Category/UpdateIsActive',
            data: {
                cateId: cateId
            },
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res) {
                    notifySuccess('Cập nhật trạng thái thành công');
                    $('#tblCategory').bootstrapTable('refresh');
                }
                else {
                    notifyError('Cập nhật trạng thái thất bại');
                }
            }
        });
    }
};

$(document).ready(function () {
    category.registerControl();
});