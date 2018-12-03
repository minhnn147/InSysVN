$(document).ready(function () {
    //var drdwh = document.getElementById("drdWarehouse");
    function ShowToolBar() {

        var id = $("#drdWarehouse").val() == null ? $("#WarehousIdOfCus").val() : $("#drdWarehouse").val();

        _AjaxPost("/Warehouses/CheckSyncWarehouse", { warehouseid: id }, function (rs) {
            if (!rs.data) {
                $("#btnImportExcelProduct").show();
                $("#btnSynProduct").hide();
            }
            else {
                $("#btnImportExcelProduct").hide();
                $("#btnSynProduct").show();   
            }
        });
    }
    ShowToolBar();
    $("#drdWarehouse").select2();
    $("#drdWarehouse").on("change", function () {

        ShowToolBar();
        $('#tableProducts').bootstrapTable("refresh");
    });

    $("#btnImportExcelProduct").click(function () {

        $("#ipIEProduct").trigger("click");

    });

    $("#ipIEProduct").on("change", function () {
        app.component.Loading.Show();
        var formData = new FormData($('form.form-inline')[0]);
        var IDWarehouse = $("#drdWarehouse").val() != undefined ? $("#drdWarehouse").val() : $("#WareHouseId").val();
        formData.append('IDWarehouse', IDWarehouse);

        $.ajax({
            url: "/Products/ReadFileImportExcel",
            method: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (rs) {

                $("#ipIEProduct").val(null);
                app.component.Loading.Hide();
                if (rs.success) {
                    if (rs.listError.length > 0) {
                        $("#txtLstError").text("Có " + rs.listError.length + " lỗi trong file Excel.");
                        var htmlError = "";
                        rs.listError.forEach(function (item) {
                            htmlError += "<tr><td>" + item + "</td></tr>";
                        });
                        $("#tblImportError").html(htmlError);
                    }
                    else {
                        $("#txtLstError").text("File excel không có lỗi!");
                    }
                    $("#tblProductImport").bootstrapTable({
                        striped: true,
                        pagination: true,
                        limit: 10,
                        pageSize: 10,
                        pageList: [10, 25, 50, 100, 200],
                        search: true,
                        showColumns: true,
                        showRefresh: true,
                        minimumCountColumns: 2,
                        columns: [
                            {
                                title: "Danh Mục",
                                field: "ProductCategory",
                                align: 'left',
                                valign: 'middle',
                                sortable: true,
                            },
                            {
                                title: "Tên",
                                field: "ProductName",
                                align: 'left',
                                valign: 'middle',
                                sortable: true,
                            },
                            {
                                title: "Barcode",
                                field: "Barcode",
                                align: 'center',
                                valign: 'middle',
                            },
                            {
                                title: "Số Lượng",
                                field: "InventoryNumber",
                                align: 'right',
                                valign: 'middle',
                                formatter: function (value, row, index) {
                                    if (value == null) {
                                        return '';
                                    }
                                    else return formatNumber(value);
                                }
                            },
                            {
                                title: "Đơn Vị",
                                field: "ComputeUnit",
                                align: 'center',
                                valign: 'middle',
                            },
                            {
                                title: "Giá Nhập",
                                field: "Price",
                                align: 'right',
                                valign: 'middle',
                                formatter: function (value, row, index) {
                                    if (value == null) {
                                        return '';
                                    }
                                    else return formatMoney(value);
                                }
                            },
                            {
                                title: "Giá Bán",
                                field: "SellPrice",
                                align: 'right',
                                valign: 'middle',
                                formatter: function (value, row, index) {
                                    if (value == null) {
                                        return '';
                                    }
                                    else return formatMoney(value);
                                }
                            },
                            {
                                title: "Mô tả",
                                field: "Description",
                                align: 'center',
                                valign: 'middle',
                            },
                            {
                                field: 'ExpiredDate',
                                title: 'Hạn sử dụng',
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
                                title: "Xóa",
                                align: 'center',
                                valign: 'middle',
                                formatter: function (value, row, index) {
                                    return '<a class="remove" href="javascript:void(0)" style="color:red" title="Xóa"><i class="glyphicon glyphicon-remove"></i></a>';
                                },
                                events: {
                                    'click .remove': function (e, value, row, index) {
                                        if (row != null) {
                                            $("#tblProductImport").bootstrapTable("remove", { field: 'Barcode', values: [row.Barcode] });
                                        }
                                    }
                                }
                            },
                            {
                                title: "Thông tin Import",
                                field: "ImportStatus",
                                align: 'center',
                                valign: 'middle',
                                sortable: true,
                                formatter: function (value, row, index) {
                                    console.log(value);
                                    if (value == "") {
                                        return 'Chưa thực hiện';
                                    }
                                    else if (value == "1") {
                                        return "<i style='color:green'>Thành công</i>";
                                    }
                                    else if (value == "2") {
                                        return "<i style='color:blue'>Cập nhật số lượng thành công!</i>";
                                    }
                                    else {
                                        return "<i style='color:red'>" + value + "</i>";
                                    }
                                }
                            }
                        ]
                    });
                    $("#tblProductImport").bootstrapTable('removeAll');
                    if (rs.list.length > 0) {
                        $("#tblProductImport").bootstrapTable('load', rs.list);
                    }
                    $('#btnAddProductsImport').removeAttr('disabled');
                    $("#ProductsImport").modal("show");
                    $("#ProductsImport .modal-dialog").attr("style", "width:80%;");
                }
                else {
                    notifyError('Import gặp lỗi.');
                    console.log(rs.message);
                }
                $('#tableProducts').bootstrapTable("refresh");
            }
        });
    });

    //$("#btnCreate").click(function () {
    //    _AjaxPost("/Warehouses/CheckSyncWarehouse", { warehouseid: $("#drdWarehouse").val() }, function (rs) {
    //        console.log(rs.data);
    //        if (!rs.data) {
    //            window.location.href = '/Products/Create';
    //        }
    //        else {
    //            notifyWarning(rs.name + " được đồng bộ với kho nên không thể thêm mới sản phẩm");
    //        }
    //    });

    //})

    $("#btnAddProductsImport").on("click", function () {
        app.component.Loading.Show();
        var dataImport = $("#tblProductImport").bootstrapTable('getData');
        var data = $.map(dataImport, function (it) {
            if (it.ImportStatus == "") {
                return {
                    Barcode: it.Barcode,
                    ComputeUnit: it.ComputeUnit,
                    Description: it.Description,
                    ExpiredDate: GetValueDate(it.ExpiredDate),
                    InventoryNumber: it.InventoryNumber,
                    Price: it.Price,
                    ProductCategory: it.ProductCategory,
                    ProductName: it.ProductName,
                    SellPrice: it.SellPrice
                }
            }
        })
        if (data.length > 0) {
            _AjaxPost("/Products/UpdateImportProducts", { data: data, WarehouseId: $("#drdWarehouse").val() }, function (rs) {
                app.component.Loading.Hide();
                notifySuccess("Import xong");
                $("#tblProductImport").bootstrapTable('load', rs.data);
                $("#tableProducts").bootstrapTable("refresh");
            });
        }
        else {
            app.component.Loading.Hide();
            notifyWarning("Không có dữ liệu đúng để Import");
        }
        $(this).attr('disabled', 'true');
    });
    $('#tableProducts').bootstrapTable({
        url: "/Products/GetDataProducts",
        method: "POST",
        ajax: function (config) {
            app.component.Loading.Show();
            _AjaxPost(config.url, { obj: config.data, WarehouseId: $("#drdWarehouse").val() }, function (rs) {
                app.component.Loading.Hide();
                if (rs.success) {
                    config.success({
                        total: rs.total,
                        rows: rs.data,
                        AllowEdit: rs.AllowEdit
                    });
                }
                else {
                    notifyError("Lỗi lấy dữ liệu!");
                    console.log(rs.ex);
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
                title: 'Thao Tác',
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (row.AllowEdit) {
                        if (row.DateSync == null) {
                            var htmlCell = '<a href="/Products/Edit/' + row.Id + '"><i class="fas fa-edit"></i></a>';
                            return htmlCell;
                        }
                        else {
                            return '';
                        }
                    }
                    else {
                        return '';
                    }
                }
            },
            {
                field: 'ProductCategory',
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
                field: 'Barcode',
                title: 'Barcode',
                align: 'center',
                valign: 'middle',
                sortable: true
            },
            {
                field: 'InventoryNumber',
                title: 'Số lượng',
                align: 'center',
                valign: 'middle',
                sortable: true,
                formatter: function (value, row, index) {
                    if (value <= 0) {
                        return '<b style="color:red">Hết hàng</b>';
                    }
                    else {
                        return formatNumber(value);
                    }
                },
            },
            {
                field: 'Price',
                title: 'Giá nhập',
                align: 'right',
                valign: 'middle',
                sortable: true,
                formatter: function (value, row, index) {
                    return formatMoney(value);
                },
            },
            {
                field: 'SellPrice',
                title: 'Giá bán',
                align: 'right',
                valign: 'middle',
                sortable: true,
                formatter: function (value, row, index) {
                    return formatMoney(value);
                },
            },
            //{
            //    field: 'SellPriceShop',
            //    title: 'Giá bán',
            //    align: 'right',
            //    valign: 'middle',
            //    sortable: true,
            //    formatter: function (value, row, index) {
            //        return formatMoney(value);
            //    }
            //    ,editable: {
            //        mode: "popup",
            //        inputclass: 'ipMoney',
            //        type: 'text',
            //    }
            //},
            {
                field: 'ExpiredDate',
                title: 'Hạn sử dụng',
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
                field: 'ModifiedDate',
                title: 'Ngày sửa',
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
                field: 'DateSync',
                title: 'Ngày đồng bộ',
                align: 'center',
                valign: 'middle',
                searchable: false,
                sortable: true,
                formatter: function (value, row, index) {
                    if (value == null) {
                        return '';
                    }
                    else return formatToDateTime(value);
                }
            }
        ],
        onEditableShown: function (field, row, oldValue, $el) {
            if (field == "SellPriceShop") {
                new AutoNumeric(".ipMoney", row[field], opMoney);
            }
        },
        onEditableSave: function (field, row, oldValue, $el) {
            if (field == "SellPriceShop") {
                row[field] = getMoney_AutoNumeric(row[field]);
            }
            _AjaxPost("/Products/UpdateProduct", { product: row, WarehouseId: $("#drdWarehouse").val() }, function (rs) {
                if (rs.success) {
                    notifySuccess("Cập nhật thành công");
                } else {
                    notifyError("Có lỗi xảy ra trong quá trình xử lý. Vui lòng liên hệ với kỹ thuật viên.");
                    $('#tableProducts').bootstrapTable("refresh");
                }
            });
        },
        onLoadSuccess: function (data) {
            if (!data.AllowEdit) {
                $("#tableProducts tr").find('.editable').editable('disable');
            }
        }
    });
    $('#btnSynProduct').on("click", function () {
        app.component.Loading.Show();
        _AjaxPost("/Products/ProductSync", { WarehouseId: $("#drdWarehouse").val() }, function (rs) {
            app.component.Loading.Hide();
            notify(rs);
            $("#tableProducts").bootstrapTable('refresh');
        });
    });
    $('#btnExportExcelProduct').on("click", function () {
        _AjaxPost("/Products/Export", {
            txtSearch: $("#tableProducts").bootstrapTable("getOptions").searchText,
            WarehouseId: $("#drdWarehouse").val()
        }, function (rs) {
            SaveFileAs(rs.urlFile, rs.fileName);
        });
    });
    //if (drdwh == null) {
    //    $('#tableProducts').bootstrapTable('hideColumn', 'SellPrice');
    //}

});