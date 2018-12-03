$(document).ready(function () {

    $("#drdWarehouse").select2();
    $("#drdWarehouse").on("change", function () {
        $("#tblProductsReport").bootstrapTable("refresh");
    });
    app.component.DatePicker();
    $("#startdate").datepicker().on('changeDate', function (selected) {
        var minDate = new Date(selected.date.valueOf());
        if ($('#enddate').datepicker('getDate') <= minDate) {
            $("#enddate").datepicker("update", minDate);
        }
        $('#enddate').datepicker('setStartDate', minDate);
        $("#tblProductsReport").bootstrapTable("refresh");
    });

    $("#enddate").datepicker().on('changeDate', function (selected) {
        var maxDate = new Date(selected.date.valueOf());
        if ($('#startdate').datepicker('getDate') >= maxDate) {
            $("#startdate").datepicker("update", maxDate);
        }
        $('#startdate').datepicker('setEndDate', maxDate);
        $("#tblProductsReport").bootstrapTable("refresh");
    });

    var d = new Date();
    var firstMonth = "01/" + (d.getMonth() + 1) + "/" + d.getFullYear();
    var today = d.getDay() + "/" + (d.getMonth() + 1) + d.getFullYear();
    $("#startdate").datepicker("update", firstMonth);
    $("#enddate").datepicker("update", today);

    $("#startdate").datepicker().on('show', function (selected) {
        var minDate = new Date(selected.date.valueOf());
        $('#enddate').datepicker('setStartDate', minDate);
        $('#startdate').datepicker('setEndDate', $("#enddate").datepicker('getDate'));
    });

    $("#enddate").datepicker().on('show', function (selected) {
        var maxDate = new Date(selected.date.valueOf());
        $('#startdate').datepicker('setEndDate', maxDate);
        $('#enddate').datepicker('setStartDate', $("#startdate").datepicker('getDate'));
    });
    $("#tblProductsReport").bootstrapTable({
        url: "/Products/GetDataProductReport",
        method: "POST",
        ajax: function (config) {
            config.data.search = $("#txtsearch").val();
            var params = {
                obj: config.data,
                startDate: GetValueDate($("#startdate").datepicker('getDate')),
                endDate: GetValueDate($("#enddate").datepicker('getDate')),
                WarehouseId: $("#drdWarehouse").val()
            };
            _AjaxPost(config.url, params, function (rs) {
                if (rs.success) {
                    config.success({
                        total: rs.total,
                        rows: rs.data
                    });
                }
                else {
                    notifyError("Lấy dữ liệu thất bại!");
                    console.log(rs.message);
                }
            });

        },
        striped: true,
        sidePagination: 'server',
        pagination: true,
        paginationVAlign: 'both',
        limit: 10,
        pageSize: 10,
        pageList: [10, 25, 50, 100, 200],
        search: false,
        showColumns: false,
        showRefresh: false,
        minimumCountColumns: 2,
        toolbar: "#toolbar",
        columns: [
            {
                field: 'ProductName',
                title: 'Tên sản phẩm',
                align: 'left',
                valign: 'middle',
                sortable: true
            },
            {
                field: 'Barcode',
                title: 'Barcode',
                align: 'left',
                valign: 'middle',
                sortable: true
            },
            {
                field: 'ProductCategory',
                title: 'Danh mục',
                align: 'center',
                valign: 'middle',
                sortable: true,
                formatter: function (value, row, index) {
                    if (value == null) {
                        return '';
                    }
                    else return value;
                },
            },
            {
                field: 'QuantitySell',
                title: 'Số lượng bán',
                align: 'right',
                valign: 'middle',
                sortable: true,
                formatter: function (value, row, index) {
                    return formatNumber(value);
                },
            },
            {
                field: 'QuantityPromotion',
                title: 'Số lượng khuyến mại',
                align: 'right',
                valign: 'middle',
                sortable: true,
                formatter: function (value, row, index) {
                    //return (value == 0 ? '' : formatNumber(value))
                    return formatNumber(value);
                },
            },
            {
                field: 'QuantityReturn',
                title: 'Số lượng trả',
                align: 'right',
                valign: 'middle',
                sortable: true,
                formatter: function (value, row, index) {
                    //return (value == 0 ? '' : formatNumber(value))
                    return formatNumber(value);
                },
            },
            {
                field: 'QuantityInventory',
                title: 'Số lượng tồn',
                align: 'right',
                valign: 'middle',
                sortable: true,
                formatter: function (value, row, index) {
                    return formatNumber(value);
                },
            },
            {
                field: 'TotalPriceSell',
                title: 'Tổng tiền bán hàng',
                align: 'right',
                valign: 'middle',
                sortable: true,
                formatter: function (value, row, index) {
                    //return (row.QuantitySell == 0 ? '' : formatNumber(value));
                    return formatNumber(value);
                },
            },
            {
                field: 'TotalPriceReturn',
                title: 'Tổng tiền trả hàng',
                align: 'right',
                valign: 'middle',
                sortable: true,
                formatter: function (value, row, index) {
                    //return (row.QuantityReturn == 0 ? '' : formatNumber(value));
                    return formatNumber(value);
                },
            }
        ]
    });

    $("#btnSearch").on("click", function () {
        $("#tblProductsReport").bootstrapTable("refresh");
    });
    $("#btnExportExcel").on("click", function () {
        _AjaxPost("/Products/ExportReport", {
            txtSearch: $("#txtsearch").val(),
            startDate: GetValueDate($("#startdate").datepicker('getDate')),
            endDate: GetValueDate($("#enddate").datepicker('getDate')),
            WarehouseId: $("#drdWarehouse").val()
        }, function (rs) {
            if (rs.success) {
                notifySuccess("Xuất báo cáo thành công!");
                SaveFileAs(rs.urlFile, rs.fileName);
            }
            else {
                notifyError("Xuất báo cáo thất bại!");
                console.log(rs.message);
            }
        });
    });
    $("#txtsearch").on("keydown", function (e) {
        if (e.keyCode == 13) {
            $("#tblProductsReport").bootstrapTable('refresh');
        };
    });
});