$(document).ready(function () {
    $("#drdWarehouse").select2({});
    $("#drdRoles").select2({});
    $("#drdWarehouse").on("change", function () {
        $("#tblUser").bootstrapTable("refresh");
    })
    $("#drdRoles").on("change", function () {
        $("#tblUser").bootstrapTable("refresh");
    })
    $("#tblUser").bootstrapTable({
        url: '/Users/GetDataUser',
        ajax: function (config) {
            var p = config.data;
            p.search = $('input[name="txtSearch"]').val();
            var params = {
                obj: p,
                WarehouseId: $("#drdWarehouse").val(),
                RoleId: $("#drdRoles").val()
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
                    console.log(rs.ex);
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
        showColumns: true,
        showRefresh: true,
        minimumCountColumns: 2,
        columns: [
            {
                field: 'operate',
                title: 'Mở rộng',
                align: 'center',
                valign: 'middle',
                searchable: false,
                formatter: function (value, row, index) {
                    return '\
                                        <a class="reset ml10"  href="javascript:void(0)" title="Reset Password">\
                                            <i class="fas fa-key"></i>\
                                        </a>\
                                        <a class="edit ml10" href="'+ '/Users/Update/' + row.Id + '" title="Edit User">\
                                            <i class="fas fa-edit"></i>\
                                        </a>\
                                            <a data-toggle="modal" class="remove ml10" href="javascript:void(0)" title="Delete User">\
                                            <i class="fas fa-trash-alt"></i>\
                                        </a>';
                },
                events: {
                    'click .reset': function (e, value, row, index) {
                        modal.LoadAjax({
                            title: "Thiết lập lại mật khẩu tài khoản <b style='color:red'>" + row['UserName'] + "</b>",
                            html: '\
                                    <form id="frmResetPass" class="form-horizontal" method="post">\
                                        <input name="UserId" type="hidden" value="'+ row.Id + '">\
                                        <div class="form-group row">\
                                            <label class="col-md-4 label-control required" for="PasswordNew">Mật khẩu mới</label>\
                                            <div class="col-md-8">\
                                                <input type="password" class="form-control" minlength = "6" maxlength="200" required="" id="New" name="PasswordNew">\
                                            </div>\
                                        </div>\
                                        <div class="form-group row">\
                                            <label class="col-md-4 label-control required" for="PasswordReNew">Nhập lại mật khẩu</label>\
                                            <div class="col-md-8">\
                                                <input type="password" class="form-control" minlength = "6"  maxlength="200" required="" id="NewVerify" name="PasswordReNew">\
                                            </div>\
                                        </div>\
                                        <div class="form-group row">\
                                            <div class="col-md-4">\
                                            </div>\
                                            <div class="col-md-8">\
                                                <input type="submit" value="Thay đổi" class="btn btn-primary">\
                                            </div>\
                                        </div>\
                                    </form>',
                            onload: function (modalPopup) {
                                $("#frmResetPass").on("submit", function () {
                                    var data = $(this).serializeObject();
                                    _AjaxPost("/Users/ResetPassword", data, function (rs) {
                                        if (rs.success) {
                                            notifySuccess("Đổi Mật Khẩu Thành Công!");
                                            $(".modal.fade.in").modal("hide");;
                                           
                                        }
                                        else {
                                            notifyError(rs.mess == null ? "Đổi Mật Khẩu Không Thành Công!" : rs.mess);
                                        }
                                    });
                                    return false;
                                })


                            }
                        });
                    },
                    'click .remove': function (e, value, row, index) {
                        modal.DeleteComfirm({
                            callback: function () {
                                console.log(row.Id);
                                $.ajax({
                                    type: 'POST',
                                    url: '/users/delete/' + row.Id,
                                    success: function (data) {
                                        debugger;
                                        if (data) {
                                            $('#tblUser')
                                                .bootstrapTable('remove', {
                                                    field: 'Id',
                                                    values: [row.Id],

                                                });
                                            notifySuccess("Xóa User " + row.UserName + " thành công!")

                                            $('#tblUser').bootstrapTable('refresh')
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
            {
                field: "AvatarImg",
                title: 'Avatar',
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value != null) return "<img src='" + value + "'style='max-width:50px'/>"
                    else return '';
                }
            },
            {
                field: 'FullName',
                title: 'Tên đầy đủ',
                align: 'left',
                valign: 'middle',
                sortable: true
            },
            {
                field: 'UserName',
                title: 'Tên đăng nhập',
                align: 'left',
                valign: 'middle',
                sortable: true
            },
            {
                field: 'UserCode',
                title: 'Mã người dùng',
                align: 'left',
                valign: 'middle',
                sortable: true
            },
            {
                field: 'Birthday',
                title: 'Ngày Sinh',
                align: 'center',
                valign: 'middle',
                sortable: true,
                formatter: function (value, row, index) {
                    return formatToDate(value);
                }
            },
            {
                field: 'Phone',
                title: 'Số Điện Thoại',
                align: 'center',
                valign: 'middle',
                sortable: true

            },
            {
                field: 'RoleDisplayName',
                title: 'Quyền',
                align: 'left',
                valign: 'middle',
                sortable: true
            },
            {
                field: 'Email',
                title: 'Email',
                align: 'left',
                valign: 'middle',
                sortable: true
            },
            {
                field: 'ModifiedDate',
                title: 'Ngày thay đổi',
                align: 'center',
                valign: 'middle',
                searchable: false,
                sortable: true,
                formatter: function (value, row, index) {
                    return formatToDate(value);
                }
            }

        ]
    });
    $("#btnSearch").on("click", function () {
        $("#tblUser").bootstrapTable("refresh");
    });
});