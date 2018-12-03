function _AjaxPost(url, data, funcSuccess) {
    $.ajax({
        url: url,
        type: "POST", 
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        success: funcSuccess,
        error: function (er) {
            notifyError("Lỗi Truyền Dữ Liệu!");
            console.log(er);
        }
    });
}

function _AjaxPostForm(url, data, funcSuccess) {
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        success: funcSuccess,
        error: function (er) {
            notifyError("Lỗi Truyền Dữ Liệu!");
            console.log(er);
        }
    });
}

function getValueDatePicker(elementDatepicker) {
    return (new Date($(elementDatepicker).datepicker('getUTCDate'))).toUTCString()
}
function formatToDate(value) {
    return moment(value).format('DD/MM/YYYY');
}
function formatToDateTime(value) {
    return moment(value).format('DD/MM/YYYY HH:mm:ss');
}
var Modal = {
    html: "\
            <div class=\"modal fade\" id=\"modalRender\" role=\"dialog\">\
                <div class=\"modal-dialog\">\
                    <div class=\"modal-content\">\
                    <div class=\"modal-header\">\
                        <button type=\"button\" class=\"close\" data-dismiss=\"modal\">&times;</button>\
                        <h4 class=\"modal-title\">Modal Header</h4>\
                    </div>\
                    <div class=\"modal-body\">\
                    </div>\
                    <div class=\"modal-footer\">\
                        <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Close</button>\
                    </div>\
                    </div>\
                </div>\
            </div>\
        ",
    Show: function () {
        $("#modalRender").on("show.bs.modal", function () {
            app.component.Loading.Show();
        });
    },
    Shown: function () {
        $("#modalRender").on("shown.bs.modal", function () {
            app.component.Loading.Hide();
        });
    },
    RenderView: function () {
        $("body").append(Modal.html);
        $("#modalRender").modal();
        Modal.Show();
        Modal.Shown();
    }
}

function GetValueDate(value) {
    return moment(value).format('YYYY-MM-DD');
}

var cusmodal = {
    html: '<div class=\"modal fade\" id=\"mdlCustom\" role=\"dialog\">\
            <div class=\"modal-dialog\">\
              <div class=\"modal-content\">\
              </div>\
            </div>\
          </div>'
    ,
    ShowView: function (url, funcReady) {
        if ($("#mdlCustom").length > 0) {
            $("#mdlCustom").each(function () {
                $(this).remove();
            })
        }
        $('body').append(cusmodal.html);
        $("#mdlCustom .modal-content").load(url, function () {
            $("#mdlCustom").modal("show");
            funcReady();
        });
    },
}
function disable(element) {
    $(element).prop('disabled', true);
}
function enable(element) {
    $(element).removeAttr('disabled');
}