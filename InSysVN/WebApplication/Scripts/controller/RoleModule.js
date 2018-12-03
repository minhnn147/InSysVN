$(document).ready(function () {
    _AjaxPost("/RoleModule/GetDataRoleModule", { RoleId: $("#selectRoles").val() }, function (rs) {
        $("#tblRoleModule").html(rs);
    });
    $("#selectRoles").on("change", function () {
        _AjaxPost("/RoleModule/GetDataRoleModule", { RoleId: $(this).val() }, function (rs) {
            $("#tblRoleModule").html(rs);
        });
        $('#drdRoles').val($("#selectRoles").val()).trigger('change');
    })
    $("#selectRoles").select2();
    $("#drdRoles").select2().on("change", function () {
        selectedModule();
    });
    $("#drdModules").select2({
        multiple: true,
        closeOnSelect: false,
        selectOnClose: false
    });
    selectedModule();
    function selectedModule() {
        if ($('#drdRoles').val() != null) {
            app.component.Loading.Show();
            _AjaxPost("/Modules/GetModuleByRoleId", { RoleId: $('#drdRoles').val() }, function (rs) {
                app.component.Loading.Hide();
                if (rs.success) {
                    var itemselect = $.map(rs.data, function (item) {
                        return item.Id
                    });
                    $('#drdModules').val(itemselect).trigger('change');
                }
            });
        }
    }
    $("#frmAddModuleForRole").on("submit", function () {
        _AjaxPost("/RoleModule/AddModuleToRole", {
            RoleId: $('#drdRoles').val(),
            ModulesId: $('#drdModules').val()
        }, function (rs) {
            $("#modalUpdateModal").modal("hide");
            if (rs.success) {
                notifySuccess("Cập nhật thành công!");
                _AjaxPost("/RoleModule/GetDataRoleModule", { RoleId: $("#selectRoles").val() }, function (rs) {
                    $("#tblRoleModule").html(rs);
                });
            }
            else notifyError("Cập nhật không thành công!");
        });
        return false;
    });
});