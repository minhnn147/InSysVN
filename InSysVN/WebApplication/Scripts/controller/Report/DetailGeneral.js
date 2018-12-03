var DetailGeneral = {
    ui: {

    },
    load: function () {
        app.component.DatePicker();
        DetailGeneral.component.select2User();
        DetailGeneral.component.select2Customer();
        $("input[name='opType']").on("change", function () {
            var value = $(this).val();
            if (value == 3) {
                $("#customDate").show();
                $("#char").hide();
                $("#tblOrdersReport").bootstrapTable("refresh");
            }
            else {
                $("#customDate").hide();
                $("#char").show();
                $("#tblOrdersReport").bootstrapTable("refresh");
                DetailGeneral.component.char();
            }
        });
        $("#checkQuest[name='opQuest']").on('change', function () {
            $("#selectCustomer").val("null").trigger('change');
            $("#tblOrdersReport").bootstrapTable("refresh");
        });
        $("#startdate").datepicker().on('changeDate', function (selected) {
            var minDate = new Date(selected.date.valueOf());
            if ($('#enddate').datepicker('getDate') <= minDate) {
                $("#enddate").datepicker("update", minDate);
            }
            $('#enddate').datepicker('setStartDate', minDate);
            $("#tblOrdersReport").bootstrapTable("refresh");
        });

        $("#enddate").datepicker().on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            if ($('#startdate').datepicker('getDate') >= maxDate) {
                $("#startdate").datepicker("update", maxDate);
            }
            $('#startdate').datepicker('setEndDate', maxDate);
            $("#tblOrdersReport").bootstrapTable("refresh");
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
        var arrMergeCell = [];
        $("#tblOrdersReport").bootstrapTable({
            url: "/Report/GetDataDetailGeneral",
            method: "POST",
            ajax: function (config) {
                config.data.search = $("#txtsearch").val();
                var params = {
                    obj: config.data,
                    opType: $("input[name='opType']:checked").val(),
                    startDate: GetValueDate($("#startdate").datepicker('getDate')),
                    endDate: GetValueDate($("#enddate").datepicker('getDate')),
                    CreatedSearchId: $("#selectCreated").val(),
                    CustomerSearchId: $("#checkQuest[name='opQuest']").prop("checked") == true ? (-1) : $("#selectCustomer").val(),
                    WarehouseId: $("#drdWarehouse").val()
                };
                _AjaxPost(config.url, params, function (rs) {
                    console.log(rs);
                    if (rs.success) {
                        arrMergeCell = [];
                        var OrderId = "";
                        var list = rs.data;
                        var arrData = [];
                        for (var i = 0; i < list.length; i++) {
                            if (OrderId == "") {
                                OrderId = list[i].OrderId;
                                arrData.push({
                                    OrderCode: list[i].OrderCode,
                                    OrderDate: list[i].OrderDate,
                                    CreatedBy_UserName: list[i].CreatedBy_UserName,
                                    CustomerName: list[i].CustomerName,
                                    CardNumber: list[i].CardNumber,
                                    isGroup: true
                                });
                                arrMergeCell.push({
                                    index: i,
                                    field: "ProductName",
                                    colspan: 7,
                                    rowspan: 1
                                });

                                arrData.push({
                                    OrderCode: list[i].OrderCode,
                                    ProductName: list[i].ProductName,
                                    Barcode: list[i].Barcode,
                                    ComputeUnit: list[i].ComputeUnit,
                                    Quantity: list[i].Quantity,
                                    SellPrice: list[i].SellPrice,
                                    Discount_Product: list[i].Discount_Product,
                                    ProductTotal: list[i].ProductTotal,
                                    TotalPrice: list[i].TotalPrice,
                                    isBody: true
                                });
                            }
                            else {
                                if (list[i].OrderId != OrderId) {
                                    //insert Bottom
                                    arrData.push({
                                        ProductTotal: list[i - 1].TotalPrice,
                                        isTotalPrice: true
                                    });
                                    arrMergeCell.push({
                                        index: arrData.length - 1,
                                        field: "ProductName",
                                        colspan: 6,
                                        rowspan: 1
                                    });
                                    arrData.push({
                                        ProductTotal: list[i - 1].Discount,
                                        isDiscount: true
                                    });
                                    arrMergeCell.push({
                                        index: arrData.length - 1,
                                        field: "ProductName",
                                        colspan: 6,
                                        rowspan: 1
                                    });
                                    arrData.push({
                                        ProductTotal: list[i - 1].PointUsed,
                                        isPointUsed: true
                                    });
                                    arrMergeCell.push({
                                        index: arrData.length - 1,
                                        field: "ProductName",
                                        colspan: 6,
                                        rowspan: 1
                                    });
                                    arrData.push({
                                        ProductTotal: list[i - 1].GrandTotal,
                                        isGrandTotal: true
                                    });
                                    arrMergeCell.push({
                                        index: arrData.length - 1,
                                        field: "ProductName",
                                        colspan: 6,
                                        rowspan: 1
                                    });
                                    arrData.push({
                                        ProductTotal: list[i - 1].PaidGuests,
                                        isPaidGuests: true
                                    });
                                    arrMergeCell.push({
                                        index: arrData.length - 1,
                                        field: "ProductName",
                                        colspan: 6,
                                        rowspan: 1
                                    });
                                    arrData.push({
                                        ProductTotal: list[i - 1].RefundMoney,
                                        isRefundMoney: true
                                    });
                                    arrMergeCell.push({
                                        index: arrData.length - 1,
                                        field: "ProductName",
                                        colspan: 6,
                                        rowspan: 1
                                    });

                                    //insert Top
                                    arrData.push({
                                        OrderCode: list[i].OrderCode,
                                        OrderDate: list[i].OrderDate,
                                        CreatedBy_UserName: list[i].CreatedBy_UserName,
                                        CustomerName: list[i].CustomerName,
                                        CardNumber: list[i].CardNumber,
                                        isGroup: true
                                    });
                                    arrMergeCell.push({
                                        index: arrData.length - 1,
                                        field: "ProductName",
                                        colspan: 7,
                                        rowspan: 1
                                    });

                                    arrData.push({
                                        OrderCode: list[i].OrderCode,
                                        ProductName: list[i].ProductName,
                                        Barcode: list[i].Barcode,
                                        ComputeUnit: list[i].ComputeUnit,
                                        Quantity: list[i].Quantity,
                                        SellPrice: list[i].SellPrice,
                                        Discount_Product: list[i].Discount_Product,
                                        ProductTotal: list[i].ProductTotal,
                                        TotalPrice: list[i].TotalPrice,
                                        isBody: true
                                    });
                                }
                                else {
                                    arrData.push({
                                        OrderCode: list[i].OrderCode,
                                        ProductName: list[i].ProductName,
                                        Barcode: list[i].Barcode,
                                        ComputeUnit: list[i].ComputeUnit,
                                        Quantity: list[i].Quantity,
                                        SellPrice: list[i].SellPrice,
                                        Discount_Product: list[i].Discount_Product,
                                        ProductTotal: list[i].ProductTotal,
                                        TotalPrice: list[i].TotalPrice,
                                        isBody: true
                                    });
                                }
                            }
                            OrderId = list[i].OrderId;
                            if (i == (list.length - 1)) {
                                arrData.push({
                                    ProductTotal: list[i].TotalPrice,
                                    isTotalPrice: true
                                });
                                arrMergeCell.push({
                                    index: arrData.length - 1,
                                    field: "ProductName",
                                    colspan: 6,
                                    rowspan: 1
                                });
                                arrData.push({
                                    ProductTotal: list[i].Discount,
                                    isDiscount: true
                                });
                                arrMergeCell.push({
                                    index: arrData.length - 1,
                                    field: "ProductName",
                                    colspan: 6,
                                    rowspan: 1
                                });
                                arrData.push({
                                    ProductTotal: list[i].PointUsed,
                                    isPointUsed: true
                                });
                                arrMergeCell.push({
                                    index: arrData.length - 1,
                                    field: "ProductName",
                                    colspan: 6,
                                    rowspan: 1
                                });
                                arrData.push({
                                    ProductTotal: list[i].GrandTotal,
                                    isGrandTotal: true
                                });
                                arrMergeCell.push({
                                    index: arrData.length - 1,
                                    field: "ProductName",
                                    colspan: 6,
                                    rowspan: 1
                                });
                                arrData.push({
                                    ProductTotal: list[i].PaidGuests,
                                    isPaidGuests: true
                                });
                                arrMergeCell.push({
                                    index: arrData.length - 1,
                                    field: "ProductName",
                                    colspan: 6,
                                    rowspan: 1
                                });
                                arrData.push({
                                    ProductTotal: list[i].RefundMoney,
                                    isRefundMoney: true
                                });
                                arrMergeCell.push({
                                    index: arrData.length - 1,
                                    field: "ProductName",
                                    colspan: 6,
                                    rowspan: 1
                                });
                            }
                        }
                        config.success({
                            total: rs.total,
                            rows: arrData
                        });
                    }
                    else {
                        notifyError("Lấy dữ liệu thất bại!");
                    }
                });

            },
            striped: true,
            sidePagination: 'server',
            pagination: true,
            paginationVAlign: 'both',
            limit: 20,
            pageSize: 20,
            search: false,
            showColumns: false,
            showRefresh: false,
            minimumCountColumns: 2,
            toolbar: "#toolbar",
            onLoadSuccess: function () {
                for (var i = 0; i < arrMergeCell.length; i++) {
                    $('#tblOrdersReport').bootstrapTable('mergeCells', arrMergeCell[i]);
                }
            },
            columns: [
                {
                    field: 'ProductName',
                    title: 'Tên sản phẩm',
                    align: 'left',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        console.log(row);
                        if (row.isGroup) {
                            
                            return '<b style="color:red">' + row.OrderCode + ' - [' + formatToDateTime(row.OrderDate) + '] - ' + row.CreatedBy_UserName + ' - ' + (row.CustomerName == null ? "Khách vãng lai" : (row.CustomerName + (row.CardNumber == null ? "" : " - " + row.CardNumber))) + '</b>';
                        }
                        else if (row.isTotalPrice) {
                            return '<b style="float:right">Tổng thành tiền</b>';
                        }
                        else if (row.isDiscount) {
                            return '<b style="float:right">Giảm giá</b>';
                        }
                        else if (row.isPointUsed) {
                            return '<b style="float:right">Trả bằng điểm</b>';
                        }
                        else if (row.isGrandTotal) {
                            return '<b style="float:right">Tổng tiền</b>';
                        }
                        else if (row.isPaidGuests) {
                            return '<b style="float:right">Khách đưa</b>';
                        }
                        else if (row.isRefundMoney) {
                            return '<b style="float:right">Trả lại</b>';
                        }
                        else if (row.isBody) {
                            return value;
                        }
                        else return '';
                    }
                },
                {
                    field: 'Barcode',
                    title: 'Barcode',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    field: 'ComputeUnit',
                    title: 'Đơn vị tính',
                    align: 'center',
                    valign: 'middle',
                },
                {
                    field: 'Quantity',
                    title: 'Số lượng',
                    align: 'right',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return formatNumber(value);
                    }
                },
                {
                    field: 'SellPrice',
                    title: 'Đơn giá',
                    align: 'right',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return formatMoney(value);
                    }
                },
                {
                    field: 'Discount_Product',
                    title: 'Chiết khấu',
                    align: 'right',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return formatMoney(value);
                    }
                },
                {
                    field: 'ProductTotal',
                    title: 'Thành tiền',
                    align: 'right',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (row.isTotalPrice || row.isDiscount || row.isPointUsed || row.isGrandTotal || row.isPaidGuests || row.isRefundMoney) {
                            return '<b>' + formatMoney(value) + '</b>';
                        }
                        else return formatMoney(value);
                    }
                },
            ]
        });

        $("#btnSearch").on("click", function () {
            $("#tblOrdersReport").bootstrapTable("refresh");
        });
        $("#btnExportExcel").on("click", function () {
            _AjaxPost("/Report/ExportDetailGeneral", {
                txtSearch: $("#txtsearch").val(),
                opType: $("input[name='opType']:checked").val(),
                startDate: GetValueDate($("#startdate").datepicker('getDate')),
                endDate: GetValueDate($("#enddate").datepicker('getDate')),
                CreatedSearchId: $("#selectCreated").val(),
                CustomerSearchId: $("#checkQuest[name='opQuest']").prop("checked") == true ? (-1) : $("#selectCustomer").val(),
                WarehouseId: $("#drdWarehouse").val()
            }, function (rs) {
                if (rs.success) {
                    notifySuccess("Xuất báo cáo thành công!");
                    SaveFileAs(rs.urlFile, rs.fileName);
                }
                else {
                    console.log(rs.message);
                    notifyError("Xuất báo cáo thất bại!");
                }
            });
        });
    },
    component: {
        select2Customer: function () {
            $("#selectCustomer").select2({
                placeholder: 'Nhập tên | Mã khách hàng | Số điện thoại khách hàng',
                minimumInputLength: 1,
                allowClear: true,
                width: "100%",
                ajax: {
                    delay: 500,
                    type: 'POST',
                    url: '/Customer/AutoCompletedCustomer',
                    data: function (params) {
                        var query = {
                            term: params.term,
                            page: params.page || 1
                        };
                        return query;
                    },
                    dataType: 'json',
                    processResults: function (data, params) {
                        params.page = params.page || 1;
                        return {
                            results: $.map(data.results, function (item) {
                                return {
                                    id: item.Id + "",
                                    text: item.Name,
                                    CustomerId: item.Id,
                                    Name: item.Name,
                                    CustomerCode: item.CustomerCode,
                                    Phone: item.Phone,
                                    Point: item.Point
                                };
                            }),
                            pagination: {
                                more: params.page * 10 < data.total
                            }
                        };
                    }
                },
                "pagination": {
                    "more": true
                },
                templateResult: function (data) {
                    if (data.loading)
                        return data.text;
                    var $result = $("<span>" + data.Name + "</span>");
                    $result.append(" <span class='badge'>" + data.Phone + "</span>");
                    $result.append(" <span class='label label-primary'>" + data.CustomerCode + "</label>");
                    return $result;
                }
            })
                .on('select2:select', function (evt) {
                    var item = evt.params.data;
                    if (item != null) {
                        $("#checkQuest[name='opQuest']").prop("checked", false);
                        $("#tblOrdersReport").bootstrapTable("refresh");
                    }
                })
                .on('change', function (evt) {
                    if ($(this).val() == null) {
                        $("#tblOrdersReport").bootstrapTable("refresh");
                    }
                })
        },
        select2User: function () {
            $("#selectCreated").select2({
                placeholder: 'Nhập tên | Tài khoản | Code',
                minimumInputLength: 1,
                allowClear: true,
                width: "100%",
                ajax: {
                    delay: 500,
                    type: 'POST',
                    url: '/Users/AutoCompletedUsers',
                    data: function (params) {
                        var query = {
                            term: params.term,
                            page: params.page || 1
                        };
                        return query;
                    },
                    dataType: 'json',
                    processResults: function (data, params) {
                        console.log(data.results);
                        params.page = params.page || 1;
                        return {
                            results: $.map(data.results, function (item) {
                                return {
                                    id: item.Id + "",
                                    text: item.FullName,
                                    UserName: item.UserName,
                                    UserCode: item.UserCode,
                                };
                            }),
                            pagination: {
                                more: params.page * 10 < data.total
                            }
                        };
                    }
                },
                "pagination": {
                    "more": true
                },
                templateResult: function (data) {
                    if (data.loading)
                        return data.text;
                    var $result = $("<span>" + data.text + "</span>");
                    $result.append(" <span class='badge'>" + data.UserName + "</span>");
                    $result.append(" <span class='label label-primary'>" + data.UserCode + "</label>");
                    return $result;
                }
            })
                .on('select2:select', function (evt) {

                    var item = evt.params.data;
                    if (item != null) {
                        $("#tblOrdersReport").bootstrapTable("refresh");
                    }
                })
                .on('change', function (evt) {
                    if ($(this).val() == null) {
                        $("#tblOrdersReport").bootstrapTable("refresh");
                    }
                })
        },
        char: function () {
            _AjaxPost("/Report/GetDataCharDetailGeneral", { opType: $("input[name='opType']:checked").val(), WarehouseId: $("#drdWarehouse").val() }, function (rs) {
                mychar.data.labels = rs.lstLabels;
                mychar.data.datasets[0].data = rs.lstQuantitys;
                mychar.data.datasets[1].data = rs.lstSales;
                mychar.update();
            });
        }
    }
}
$(document).ready(function () {
    DetailGeneral.load();
    $("#drdWarehouse").select2().on("change", function () {
        $("#tblOrdersReport").bootstrapTable("refresh");
        DetailGeneral.component.char();
    });
});