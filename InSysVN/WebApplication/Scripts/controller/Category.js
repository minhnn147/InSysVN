var category = {
    ui: {},
    registerControl: function () {
        category.loadCategory();
    },
    loadCategory: function () {
        $('#tblCategory').bootstrapTable({
            url: "/Category/GetCateData",
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
                            return "<img src='http://localhost:59765/" + value + "'style='max-width:50px'/>";
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
                            return '<input type="checkbox" onclick="category.updateisActive(' + row.Id +')">';
                    }
                },
                {
                    title: "Thao tác",
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return '\
                                        <a class="edit ml10" href="'+ '/Category/Eidt/' + row.Id + '" title="Sửa danh mục">\
                                            <i class="fas fa-edit"></i>\
                                        </a>';
                    },
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
            url: '/Category/UpdateIsActive',
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