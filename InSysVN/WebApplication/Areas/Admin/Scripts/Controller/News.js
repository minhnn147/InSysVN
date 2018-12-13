var news = {
    ui: {},
    registerControl: function () {
        news.loadnews();
    },
    loadnews: function () {
        $('#tblNews').bootstrapTable({
            url: domain + '/Admin/News/GetNewsData',
            method: "POST",
            ajax: function (config) {
                app.component.Loading.Show();
                _AjaxPost(config.url, { obj: config.data }, function (rs) {
                    app.component.Loading.Hide();
                    if (rs.status) {
                        config.success({
                            total: rs.total,
                            rows: rs.rows,
                            AllowEdit: true
                        });
                    }
                    else {
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
                    field: 'Title',
                    title: 'Tiêu để',
                    align: 'left',
                    valign: 'middle',
                    sortable: true
                },
                {
                    field: 'ImageTitle',
                    title: 'Hình ảnh',
                    align: 'left',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value != null)
                            return "<img src='" + value + "'style='max-width:50px'/>";
                        return '';
                    }
                },
                {
                    field: 'CreateDate',
                    title: 'Ngày viết',
                    align: 'left',
                    valign: 'middle',
                    sortable: true,
                    formatter: function (value, row, index) {
                        if (value == null) {
                            return '';
                        }
                        else return formatToDate(value);
                    }
                },
                {
                    field: "IsActive",
                    title: "Kích hoạt",
                    formatter: function (value, row, index) {
                        if (row.IsActive)
                            return '<input type="checkbox" onclick="news.updateisActive(' + row.ID + ')" checked="checked">';
                        else
                            return '<input type="checkbox" onclick="news.updateisActive(' + row.ID + ')">';
                    }
                },
                {
                    title: "Thao tác",
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return '\
                                        <a class="edit ml10" href="'+ domain + '/Admin/News/Update/' + row.ID + '" title="Sửa danh mục">\
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
                                        url: domain + '/admin/news/delete/' + row.ID,
                                        success: function (data) {
                                            debugger;
                                            if (data.success) {
                                                $('#tblNews')
                                                    .bootstrapTable('remove', {
                                                        field: 'Id',
                                                        values: [row.ID],

                                                    });
                                                notifySuccess("Xóa tin " + row.Name + " thành công!")

                                                $('#tblNews').bootstrapTable('refresh')
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
                if (!data.AllowEdit) {
                    $("#tblNews tr").find('.editable').editable('disable');
                }
            }
        });
    },
    updateisActive: function (newsId) {
        $.ajax({
            url: domain + '/Admin/News/UpdateIsActive',
            data: {
                newsId: newsId
            },
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res) {
                    notifySuccess('Cập nhật trạng thái thành công');
                    $('#tblNews').bootstrapTable('refresh');
                }
                else {
                    notifyError('Cập nhật trạng thái thất bại');
                }
            }
        });
    },
}

$(document).ready(function () {
    news.registerControl();
});

