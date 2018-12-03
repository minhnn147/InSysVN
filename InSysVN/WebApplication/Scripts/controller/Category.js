$(document).ready(function () {
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
                field:"Name",
                title:"Tên danh mục"
            },
            {
                field: "ImagePath",
                title: "Hình ảnh",
                formatter: function (value, row, index) {
                    if (value != null) return "<img src='http://localhost:59765/" + value + "'style='max-width:50px'/>"
                    else return '';
                }
            },
            {
                field: "Description",
                title: "Chi tiết"
            },
            {
                field: "",
                title: "Kích hoạt"
            },
            {
                title: "Thao tác",
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    return '\
                                        <a class="edit ml10" href="'+ '/Category/Eidt/' + row.Id + '" title="Edit Category">\
                                            <i class="fas fa-edit"></i>\
                                        </a>';
                },
            }
        ],
        onLoadSuccess: function (data) {
            if (data.total > 0 && data.rows.length == 0) {
                $(this).bootstrapTable('refresh')
            };
        }
    });
});