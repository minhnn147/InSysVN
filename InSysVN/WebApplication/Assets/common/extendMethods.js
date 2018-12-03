//#region functionExtend
var objectifyForm = function (formArray) {//serialize data function

    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        returnArray[formArray[i]['name']] = formArray[i]['value'];
    }
    return returnArray;
}
function previewImage(input, previewId) {
    readURL(input, function (e) {
        $(previewId).attr('src', e.target.result);
        $(previewId).removeClass('hide').next().removeClass('hide');
        //$('<input type="button" class="btn btn-default" value="X"/>')
        //    .on('click',
        //        function() {
        //            input.value = 'DELETE';
        //            $(previewId).addClass('hide');
        //            $(previewId).attr('src', '');
        //            $(this).remove();
        //        })
        //    .insertAfter($(previewId));
    }, function () {
        $(input).val(null);
        $(previewId).addClass('hide').next().addClass('hide');
    });
}
var fileTypes = ['jpg', 'jpeg', 'png', 'gif'];  //acceptable file types

function readURL(input, callbackShow, callbackHide) {
    if (input.files && input.files[0]) {
        var extension = input.files[0].name.split('.').pop().toLowerCase(),  //file extension from input file
            isSuccess = fileTypes.indexOf(extension) > -1;  //is extension in acceptable types

        if (isSuccess) { //yes
            var reader = new FileReader();
            reader.onload = function (e) {
                callbackShow(e);
            }

            reader.readAsDataURL(input.files[0]);
        }
        else { //no
            //warning
            callbackHide();
            alert('Invalid image file');
        }
    }
}

var newTabPrint = function (html) {
    var myWindow = window.open('', '');
    myWindow.document.write(html);
    myWindow.document.close();
    myWindow.focus();
    myWindow.print();
    myWindow.close();
}

var moneyToText = function (value) {
    var numToText = function (num) {
        if (num == 0) {
            return "không";
        }
        else if (num == 1) {
            return "một";
        }
        else if (num == 2) {
            return "hai";
        }
        else if (num == 3) {
            return "ba";
        }
        else if (num == 4) {
            return "bốn";
        }
        else if (num == 5) {
            return "năm";
        }
        else if (num == 6) {
            return "sáu";
        }
        else if (num == 7) {
            return "bảy";
        }
        else if (num == 8) {
            return "tám";
        }
        else if (num == 9) {
            return "chín";
        }
        else if (num == 10) {
            return "mười";
        }
    }
    var docdonvi = function (value) {
        if (value.length == 1) {
            return numToText(value);
        }
        else if (value.length == 2) {
            if (value == 10) {
                return numToText(value);
            }
            else {
                if (value[0] == 1) {
                    return "mười " + numToText(value[1]);
                }
                var txt = numToText(value[0]) + " mươi";
                if (value[1] != 0) {
                    if (value[1] == 1) {
                        return txt + " mốt";
                    }
                    return txt + " " + numToText(value[1]);
                }
                return txt;
            }
        }
        else {
            var txt = "";
            txt = numToText(value[0]) + " trăm";
            if (value[1] == 0) {
                if (value[2] != 0) {
                    txt += " linh " + numToText(value[2]);
                }
            }
            else {
                txt += " " + docdonvi(value[1] + value[2]);
            }
            return txt;
        }
    }

    var getDonvi = function (value, boi) {
        boi = boi || 3;
        var donvi = [];
        var temp = "";
        for (var i = value.length; i > 0; i--) {
            var e = value[i - 1];
            temp = e + temp;
            if ((value.length - i + 1) % boi == 0 || i == 1) {
                donvi.unshift(temp);
                temp = "";
            }
        }
        return donvi;
    }

    value = value || "0";
    value += "";
    var txt = "";

    var donvi = getDonvi(value);
    var hangTy = getDonvi(value, 9);

    for (var i = 0; i < donvi.length; i++) {
        var txt1 = docdonvi(donvi[i]);
        switch (donvi.length - i - 1) {
            case 0:
                break;
            case 1:
                txt1 += " nghìn";
                break;
            case 2:
                txt1 += " triệu";
                break;
            case 3:
                txt1 += " tỷ";
                break;
            case 4:
                txt1 += " nghìn tỷ";
                break;
            case 5:
                txt1 += " triệu tỷ";
                break;
            default:
                txt1 += " tỷ tỷ";
        }
        txt += " " + txt1;
        var conlai = donvi.slice(i + 1, donvi.length);
        var check = eval(conlai.join("")) > 0;
        if (!check) {
            break;
        }
        if (i != donvi.length - 1) {
            txt += ",";
        }
    }
    txt = txt.trim();
    return txt.substr(0, 1).toUpperCase() + txt.substr(1);
}

var page_loading = {
    count: 0,
    id_element: "page_loader_modal",
    elem: null,
    show: function () {
        this.count++;
        if (this.count > 1) {
            return;
        }
        this.elem = $("#" + this.id_element);
        if (this.elem.length == 0) {
            $("body").append('\
                <div class="text-center modal" id="{0}" style="z-index: 20001">\
                    <!--<div class="modal-body" style="background-color: white; opacity: 0.75; height: 100%">-->\
                    <div class="modal-body" style="">\
                        <div class="content-loading text-xs-center" style="margin-top: {1}px;">\
                            <i class="fa fa-spinner fa-pulse fa-3x fa-fw margin-bottom" style="color: #fffcfc;"></i>\
                            <!--<img src="/Assets/images/loading_1.gif"/>-->\
                        </div>\
                    </div>\
                </div>\
            '.format(this.id_element, (window.innerHeight - 50) / 2));
            this.elem = $("#" + this.id_element);
        }
        else {
            this.elem.find(".modal-body .content-loading").css("margin-top", ((window.innerHeight - 50) / 2) + "px");
        }
        this.elem.modal({
            backdrop: "static",
            show: true
        });

        var _obj = this.elem.data("bs.modal");
        if (_obj.$backdrop) {
            _obj.$backdrop.css("z-index", 2000);
        }

        app.component.load();
        //var loadertheme = "bg-black";
        //var loaderopacity = "80";
        //var loaderstyle = "light";
        //var loader = '<div id="' + this.id_element + '" class="ui-front loader ui-widget-overlay ' + loadertheme + ' opacity-' + loaderopacity + '"><img src="/Assets/images/spinner/loader-' + loaderstyle + '.gif" alt="" /></div>';

        //$('body').append(loader);
        //$('#' + this.id_element).fadeIn('fast');
    },
    destroy: function () {
        this.count = 1;
        this.hide();
    },
    hide: function () {
        this.count--;
        if (this.count > 0) {
            return;
        }
        if (this.count < 0) {
            this.count = 0;
        }

        if (this.elem != null) {
            this.elem.modal('hide')
        }
        app.component.modalEvent();
        //$('#' + this.id_element).fadeOut('fast');
    }
}

var _AjaxAPI = {
    showError: function (data) {
        base.noty.msgError({
            titleHeader: "Thông báo",
            timeout: 1500,
            text: data.message || data.Message
        });
    },
    get: function (url, data, success, error) {//đây mới là param
        page_loading.show();
        try {
            $.get(
                url,
                data,
                function (results) {
                    success(results);
                    page_loading.hide();
                }
            );
            //$.ajax({
            //    url: url,
            //    type: "GET",
            //    success: function (results, textStatus, jqXHR) {
            //        success(results);
            //        page_loading.hide();
            //    },
            //    error: function (jqXHR, textStatus, errorThrown) {
            //        page_loading.hide();
            //        if (error) {
            //            error(jqXHR);
            //        }
            //    }
            //});
        } catch (e) {
            page_loading.hide();
            if (error) {
                error(e);
            }
        }

    },
    post: function (url, data, success, error) {
        page_loading.show();
        try {
            $.ajax({
                url: url,
                type: "POST",
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                //dataType: 'json',
                success: function (results, textStatus, jqXHR) {
                    success(results);
                    page_loading.hide();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    page_loading.hide();
                    if (error) {
                        error(jqXHR);
                    }
                }
            });
        } catch (e) {
            page_loading.hide();
            if (error) {
                error(e);
            }
        }
    },
    formData: function (url, data, success, error, process) {
        page_loading.show();
        try {
            $.ajax(url, {
                method: "POST",
                data: data,
                processData: false,
                contentType: false,
                progress: function (data) {
                    if (process) {
                        process(data);
                    }
                },
                success: function (data) {
                    page_loading.hide();
                    success(data);
                },
                error: function (jqXHR) {
                    page_loading.hide();
                    if (error) {
                        error(jqXHR);
                    }
                }
            });
        } catch (e) {
            page_loading.hide();
            if (error) {
                error(e);
            }
        }
    }
}

var toVND = function (obj) {
    obj = obj || "";
    return obj.toVND();
}
var toPercent = function (obj) {
    obj = obj || "";
    return obj.toPercent();
}

var formatDateTime = function (obj) {
    obj = obj || "";
    if (obj != "") {
        return obj.formatDateTime();
    }
}


if ($("#ModalCustom").length == 0) {
    $("body").append('<div id="ModalCustom"></div>');
}
var modal = {
    Html: "\
            <div class=\"modal fade\" id=\"{0}\"  role=\"dialog\" aria-labelledby=\"myModalLabel\" aria-hidden=\"true\" style=\"display: none;\">\
                <div class=\"modal-dialog\">\
                    <div class=\"modal-content\">\
                        <div class=\"modal-header\">\
                            <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">×</button>\
                            <h4 class=\"modal-title\" id=\"title\">Modal title</h4>\
                        </div>\
                        <div class=\"modal-body\">\
                        </div>\
                        <div class=\"modal-footer\">\
                            <button type=\"button\" class=\"btn btn-default\" data-dismiss1=\"modal\" id=\"Close\">Close</button>\
                            <button type=\"button\" class=\"btn btn-primary\" id=\"Ok\">Ok</button>\
                        </div>\
                    </div>\
                </div>\
            </div>",
    Index: 0,
    DeleteComfirm: function (options) {
        modal.Index++;
        options = options || {};
        options = {
            idmodal: "ModalCustom" + modal.Index,
            tilte: options.title || "Thông báo",
            message: options.message || "Bạn có muốn xóa.?",
            classname: options.classname || "",
            onload: options.onload || function () {
            },
            onclose: options.onclose || function () {
                //console.log("onclose");
            },
            dismiss: options.dismiss || function () {
                //console.log("dismiss");
            },
            callback: options.callback || function () {
            },
            buttonClose: options.buttonClose || {
                Text: "Đóng",
                isShow: true,
                isIconX: false
            },
            buttonOk: options.buttonOk || {
                Text: "Đồng ý",
                isShow: true,
            },
            keyboard: options.keyboard != null ? options.keyboard : false,
            backdrop: options.backdrop != null ? options.backdrop : "static",
        }
        var html = modal.Html.format("ModalCustom" + modal.Index, options.buttonClose.Text, options.buttonOk.Text);
        $("#ModalCustom").append(html);

        options.modal = $('#' + options.idmodal);

        options.modal.find('.modal-header .modal-title').text(options.tilte);
        options.modal.find('.modal-body').html(options.message);
        var xClose = options.modal.find('.modal-header .close');
        if (!options.buttonClose.isIconX) {
            xClose.remove();
        }
        options.modal.find('.modal-footer #Close').text(options.buttonClose.Text).addClass(options.buttonClose.isShow ? "" : "hidden");
        options.modal.find('.modal-footer #Ok').text(options.buttonOk.Text).addClass(options.buttonOk.isShow ? "" : "hidden");

        options.modal.modal({
            backdrop: options.backdrop,
            //backdrop: true,
            keyboard: options.keyboard,
            show: true,
        });

        var _obj = options.modal.data("bs.modal");
        //var zIndex = 1052 + (modal.Index * 2 - 1);
        //if (_obj.$backdrop) {
        //    _obj.$backdrop.css("z-index", zIndex);
        //}
        //options.modal.css('z-index', zIndex + 1);

        var checkDismiss = false;
        options.modal.find('#Close').bind('click', function () {
            if (options.dismiss) {
                options.dismiss();
            }
            checkDismiss = true;
            options.modal.modal('hide');
        });
        options.modal.find('#Ok').bind('click', function () {
            options.callback();
            checkDismiss = true;
            options.modal.modal('hide');
        });
        //options.modal.on('hide.bs.modal', function () {
        //    if (options.onclose) {
        //        options.onclose();
        //    }
        //    //options.modal.remove();
        //});
        options.modal.on('hidden.bs.modal', function () {
            if (options.onclose != null && !checkDismiss) {
                options.onclose();
            }
            options.modal.remove();
        });
        app.component.modalEvent();
        return options;
    },
    LoadAjax: function (options) {
        modal.Index++;
        options = options || {};
        options = $.extend(true, {
            idmodal: "ModalCustom" + modal.Index,
            tilte: options.title != null ? options.title : "Title",
            isShowTitle: options.isShowTitle != null ? options.isShowTitle : true,
            url: options.url || "",
            html: options.html || "",
            //classname: options.classname || "normalmodalsmall",
            classname: options.classname || "",
            onload: options.onload || function () {
                //console.log("onload");
            },
            onclose: options.onclose || function () {
                //console.log("onclose");
            },
            dismiss: options.dismiss || function () {
                //console.log("dismiss");
            },
            callback: options.callback || function () {
                //console.log("callback");
            },
            buttonClose: options.buttonClose || {
                Text: "Đóng",
                isShow: false,
                isIconX: true
            },
            buttonOk: options.buttonOk || {
                Text: "Đồng ý",
                isShow: false,
            },
            keyboard: options.keyboard != null ? options.keyboard : true,
            backdrop: options.backdrop != null ? options.backdrop : true,
        }, options);
        var html = modal.Html.format("ModalCustom" + modal.Index);
        $("#ModalCustom").append(html);


        options.modal = $('#' + options.idmodal);
        // remove content
        options.modal.find('#content').html("");

        options.modal.modal({
            backdrop: options.backdrop,
            keyboard: options.keyboard
        });

        //var _obj = options.modal.data("bs.modal");
        //var zIndex = 1052 + (modal.Index * 2 - 1);
        //if (_obj.$backdrop) {
        //    _obj.$backdrop.css("z-index", zIndex);
        //}
        //options.modal.css('z-index', zIndex + 1);

        if (options.tilte == false) {
            options.modal.find('.modal-header').hide();
        }
        options.modal.find('#title').html(options.tilte);

        options.modal.find('.modal-footer #Close').text(options.buttonClose.Text).addClass(options.buttonClose.isShow ? "" : "hidden");
        options.modal.find('.modal-footer #Ok').text(options.buttonOk.Text).addClass(options.buttonOk.isShow ? "" : "hidden");
        if (!options.buttonClose.isShow && !options.buttonOk.isShow) {
            options.modal.find('.modal-footer').hide();
        }
        var xClose = options.modal.find('.modal-header .close');
        options.buttonClose.isIconX = options.buttonClose.isIconX != null ? options.buttonClose.isIconX : true;
        if (!options.buttonClose.isIconX) {
            xClose.remove();
        }

        options.modal.find('.modal-body').append('\
            <div style="text-align: center">\
                <i class="fa fa-refresh fa-spin"></i>\
            </div>\
        ');

        options.modal.find(".modal-dialog").addClass(options.classname);
        if (options.url != "") {
            //options.modal.modal('hide');
            page_loading.show();
            options.modal.find('.modal-body').load(options.url, function (responseText, textStatus, jqXHR) {
                //console.log(jqXHR)
                page_loading.hide();
                if (jqXHR.status == 200) {
                    options.modal.modal('show');
                    app.component.load();
                    if (options.onload) {
                        options.onload(options);
                    }
                }
                else {
                    alert("Cannot load url: {0}".format(options.url));
                }
            });
        }
        else {
            options.modal.find('.modal-body').html(options.html);
            app.component.load();
            if (options.onload) {
                options.onload(options);
            }
        }

        var checkDismiss = false;
        options.modal.find('#Close').bind('click', function () {
            if (options.dismiss) {
                options.dismiss();
            }
            checkDismiss = true;
            options.modal.modal('hide');
        });
        options.modal.find('#Ok').bind('click', function () {
            options.callback();
            checkDismiss = true;
            if (options.calbackVerify != null) {
                options.calbackVerify(function () {
                    options.modal.modal('hide');
                });
            }
            else {
                options.modal.modal('hide');
            }
        });
        //options.modal.on('hide.bs.modal', function () {
        //    if (options.onclose) {
        //        options.onclose();
        //    }
        //    //options.modal.remove();
        //});
        options.modal.on('hidden.bs.modal', function () {
            if (options.onclose != null && !checkDismiss) {
                options.onclose();
            }
            options.modal.remove();
        });
        options.closeCallback = function (data) {
            $('#' + options.idmodal).modal('hide');
            options.callback(data);
        }
        options.remove = function () {
            $('#' + options.idmodal).modal('hide');
        }
        $(options.modal).bind('closeCallback', function (event, data) {
            $('#' + options.idmodal).modal('hide');
            options.callback(data);
        });
        return options;
    }
}

function dateFromJsonCSharp(jsonDate) {
    //var jsonDate = "/Date(1245398693390)/";  // returns "/Date(1245398693390)/"; 
    var re = /-?\d+/;
    var m = re.exec(jsonDate);
    var d = new Date(parseInt(m[0]));
    return d;
}

var Notification = {
    Success: function (mess) {
        mess = mess || "Thành công";
        this.Create({
            heading: "Thông báo",
            text: mess,
            icon: "success"
        });
    },
    Error: function (mess) {
        mess = mess || "Lỗi";
        this.Create({
            heading: "Thông báo",
            text: mess,
            icon: "error"
        });
    },
    Warning: function (mess) {
        mess = mess || "Cảnh báo";
        this.Create({
            heading: "Thông báo",
            text: mess,
            icon: "warning"
        });
    },
    Info: function (mess) {
        mess = mess || "Cảnh báo";
        this.Create({
            heading: "Thông báo",
            text: mess,
            icon: "info"
        });
    },
    Create: function (options) {
        //http://kamranahmed.info/toast
        $.toast($.extend(true, {
            text: "Don't forget to star the repository if you like it.", // Text that is to be shown in the toast
            heading: 'Note', // Optional heading to be shown on the toast
            icon: 'warning', // Type of toast icon
            showHideTransition: 'fade', // fade, slide or plain
            allowToastClose: true, // Boolean value true or false
            hideAfter: 2500, // false to make it sticky or number representing the miliseconds as time after which toast needs to be hidden
            stack: 5, // false if there should be only one toast at a time or a number representing the maximum number of toasts to be shown at a time
            position: 'bottom-right', // bottom-left or bottom-right or bottom-center or top-left or top-right or top-center or mid-center or an object representing the left, right, top, bottom values



            textAlign: 'left',  // Text alignment i.e. left, right or center
            loader: true,  // Whether to show loader or not. True by default
            loaderBg: '#9EC600',  // Background color of the toast loader
            beforeShow: function () { }, // will be triggered before the toast is shown
            afterShown: function () { }, // will be triggered after the toat has been shown
            beforeHide: function () { }, // will be triggered before the toast gets hidden
            afterHidden: function () { }  // will be triggered after the toast has been hidden
        },options));

    }
}

function copy_clone(data) {
    if (data.constructor == Array) {
        return jQuery.extend(true, [], data);
    }
    else if (data.constructor == Object) {
        return jQuery.extend(true, {}, data);
    }
    return data;
}

//#region ObjectValid
ObjectValid = {
    defaultMessageValid: {
        required: 'This field({0}) is required.',
        number: 'Please enter({0}) a valid number.',
        digits: "Please enter({0}) only digits.",
        equalTo: "Please enter({0}) the same value({1}) again.",
        maxlength: "Please enter({0}) no more than {1} characters.",
        minlength: "Please enter({0}) at least {1} characters.",
        rangelength: "Please enter({0}) a value between {1} and {2} characters long.",
        min: "Please enter({0}) a value greater than or equal to {1}.",
        max: "Please enter({0}) a value less than or equal to {1}.",
        range: "Please enter({0}) a value between {1} and {2}.",
        email: "Please enter({0}) a valid email address.",
        url: "Please enter({0}) a valid URL.",
        date: "Please enter({0}) a valid date.",
        dateISO: "Please enter({0}) a valid date (ISO).",
    },
    defaultMessageValid_vi: {
        required: 'Hãy nhâp({0}).',
        number: 'Hãy nhập số({0}).',
        digits: "Hãy nhập chữ số({0}).",
        equalTo: "Hãy nhập({0}) thêm lần nữa({1}).",
        maxlength: "Hãy nhập({0}) từ {1} kí tự trở lên.",
        minlength: "Hãy nhập({0}) từ {1} kí tự trở xuống.",
        rangelength: "Hãy nhập({0}) từ {1} đến {2} kí tự.",
        min: "Hãy nhập({0}) từ {1} trở lên.",
        max: "Hãy nhập({0}) từ {1} trở xuống.",
        range: "Hãy nhập({0}) từ {1} đến {2}.",
        email: "Hãy nhập({0}) email.",
        url: "Hãy nhập({0}) URL.",
        date: "Hãy nhập({0}) ngày.",
        dateISO: "Hãy nhập({0}) ngày (ISO).",
    },
    lan: null,
    valid: function (object, rules) {
        var defaultMessage = this.lan == null ? this.defaultMessageValid : this["defaultMessageValid_" + this.lan];
        var pushMess = function (key, value, message, value1) {
            _return.push({
                field: key,
                value: value,
                message: message
            });
        }

        var _return = [];
        rules = rules || {};
        for (var key in rules) {
            var rule = rules[key];
            if (!object.hasOwnProperty(key)) {
                continue;
            }

            var displayName = rule.displayName || key;
            var value = object[key];
            if (rule.required != null) {
                var required;
                if (typeof (rule.required) === 'function') {
                    try {
                        required = eval(rule.required)();
                    }
                    catch (ex) {
                        required = eval(rule.required);
                    }
                }
                else if (typeof (rule.required) == "boolean") {
                    required = rule.required;
                }
                else if (typeof (rule.required) == "object" && rule.required.constructor == Object && rule.required.valid != null) {
                    if (typeof (rule.required.valid) == "function") {
                        try {
                            required = eval(rule.required.valid)();
                        }
                        catch (ex) {
                            required = eval(rule.required.valid);
                        }
                    }
                    else if (typeof (rule.required.valid) == "boolean") {
                        required = rule.required.valid;
                    }
                }
                if (value == null == required) {
                    var message = rule.required.constructor == Object && rule.required.message != null ? rule.required.message : (rule.message || defaultMessage["required"].format(displayName));
                    pushMess(key, value, message);
                    continue;
                }
            }

            var getRuleObj = function (rule) {
                if (typeof (rule) == "object" && rule.constructor == Object && rule.valid != null) {
                    return {
                        valid: typeof (rule.valid) == "function" ? rule.valid(object, rule) : rule.valid,
                        message: function (ruleConfig) {
                            if (typeof (rule.message) == "string") {
                                return rule.message;
                            }
                            else if (typeof (rule.message) == "function") {
                                return rule.message(arguments);
                            }
                            return null;
                        }
                    }
                }
                return {
                    message: function () {
                        return null;
                    }
                };
            }

            if (rule.number != null) {
                var _rule = getRuleObj(rule.number);
                if (!/^-?(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/.test(value) == (_rule.valid || rule.number)) {
                    var message = _rule.message(rule, value) || rule.message || defaultMessage["number"].format(displayName);
                    pushMess(key, value, message);
                    continue;
                }
            }

            if (rule.digits != null) {
                var _rule = getRuleObj(rule.digits);
                if (!/^\d+$/.test(value) == (_rule.valid || rule.digits)) {
                    var message = _rule.message(rule, value) || rule.message || defaultMessage["digits"].format(displayName);
                    pushMess(key, value, message);
                    continue;
                }
            }

            if (rule.equalTo != null) {
                var _rule = getRuleObj(rule.equalTo);
                var value1 = this[(_rule.valid || rule.equalTo)];
                if (value1 != value) {
                    var message = _rule.message(rule, value) || rule.message || defaultMessage["equalTo"].format(displayName, (_rule.valid || rule.equalTo));
                    pushMess(key, value, message);
                    continue;
                }
            }

            if (rule.maxlength != null) {
                var _rule = getRuleObj(rule.maxlength);
                var length = $.isArray(value) ? value.length : typeof (value) == 'string' ? value.length : 0;
                if ((_rule.valid || rule.maxlength) < length && value != null) {
                    var message = _rule.message(rule, value) || rule.message || defaultMessage["maxlength"].format(displayName, (_rule.valid || rule.maxlength));
                    pushMess(key, value, message);
                    continue;
                }
            }

            if (rule.minlength != null) {
                var _rule = getRuleObj(rule.minlength);
                var length = $.isArray(value) ? value.length : typeof (value) == 'string' ? value.length : 0;
                if ((_rule.valid || rule.minlength) > length && value != null) {
                    var message = _rule.message(rule, value) || rule.message || defaultMessage["minlength"].format(displayName, (_rule.valid || rule.minlength));
                    pushMess(key, value, message);
                    continue;
                }
            }

            if (rule.rangelength != null) {
                var _rule = getRuleObj(rule.rangelength);
                var rangelength1 = _rule.valid != null ? rule.valid[0] : rule.rangelength[0];
                var rangelength2 = _rule.valid != null ? rule.valid[1] : rule.rangelength[1];
                var length = $.isArray(value) ? value.length : typeof (value) == 'string' ? value.length : 0;
                if ((rangelength1 > length || rangelength2 < length) && value != null) {
                    var message = _rule.message(rule, value) || rule.message || defaultMessage["rangelength"].format(displayName, rangelength1, rangelength2);
                    pushMess(key, value, message);
                    continue;
                }
            }

            if (rule.min != null) {
                var _rule = getRuleObj(rule.min);
                if ((_rule.valid || rule.min) > value && value != null) {
                    var message = _rule.message(rule, value) || rule.message || defaultMessage["min"].format(displayName, (_rule.valid || rule.min));
                    pushMess(key, value, message);
                    continue;
                }
            }

            if (rule.max != null) {
                var _rule = getRuleObj(rule.max);
                if ((_rule.valid || rule.max) < value && value != null) {
                    var message = _rule.message(rule, value) || rule.message || defaultMessage["max"].format(displayName, (_rule.valid || rule.max));
                    pushMess(key, value, message);
                    continue;
                }
            }

            if (rule.range != null) {
                var _rule = getRuleObj(rule.range);
                var range1 = _rule.valid != null ? rule.valid[0] : rule.range[0];
                var range2 = _rule.valid != null ? rule.valid[1] : rule.range[1];
                if ((range2 < value || range1 > value) && value != null) {
                    var message = _rule.message(rule, value) || rule.message || defaultMessage["range"].format(displayName, range1, range2);
                    pushMess(key, value, message);
                    continue;
                }
            }

            if (rule.email != null) {
                var _rule = getRuleObj(rule.email);
                if ((_rule.valid || rule.email) == !/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/i.test(value)) {
                    var message = _rule.message(rule, value) || rule.message || defaultMessage["email"].format(displayName);
                    pushMess(key, value, message);
                    continue;
                }
            }

            if (rule.url != null) {
                var _rule = getRuleObj(rule.url);
                if ((_rule.valid || rule.url) == !/^(https?|s?ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(value)) {
                    var message = _rule.message(rule, value) || rule.message || defaultMessage["url"].format(displayName);
                    pushMess(key, value, message);
                    continue;
                }
            }

            if (rule.date != null) {
                var _rule = getRuleObj(rule.date);
                if ((_rule.valid || rule.date) == /Invalid|NaN/.test(new Date('31/31/2017').toString())) {
                    var message = _rule.message(rule, value) || rule.message || defaultMessage["date"].format(displayName);
                    pushMess(key, value, message);
                    continue;
                }
            }

            if (rule.dateISO != null) {
                var _rule = getRuleObj(rule.dateISO);
                if ((_rule.valid || rule.dateISO) == !/^\d{4}[\/\-]\d{1,2}[\/\-]\d{1,2}$/.test(value)) {
                    var message = _rule.message(rule, value) || rule.message || defaultMessage["dateISO"].format(displayName);
                    pushMess(key, value, message);
                    continue;
                }
            }
        }
        return _return;
    }
}
ObjectValid.lan = "vi";
var a = {
    Name1: "dasdas@adfas.fdsf",
    Name2: null
};
ObjectValid.lan = "vi"
ObjectValid.valid(a, {
    Name1: {
        url: true
    },
});
//#endregion ObjectValid

var select2Ctrl = {
    defaultAutoComplete: function (options) {
        options = options || {};
        options.Elem = $(options.Elem);
        options.config = options.config || {};
        options.select2Current = "";
        options.GetViewSelect = options.GetViewSelect || 2;
        options.GetViewSelected = options.GetViewSelected || 2;
        options.GetParams = options.GetParams || function (params) {
            return params;
        };
        options.SetDataView = options.SetDataView || function (data) {
            return data;
        };
        options.config = jQuery.extend(true, {
            ajax: {
                url: "",
                dataType: 'json',
                delay: 250,
                type: 'POST',
                data: function (params) {
                    var _this = this;
                    var select2Current = options.select2Current = _this.attr("id") || _this.attr("name");
                    var _return = {
                        Name: params.term, // search term
                        Page: params.page,
                        Type: select2Current,
                    };
                    _return = options.GetParams(_return, options.Elem);
                    return _return;
                },
                processResults: function (data, params) {
                    var _this = this.$element;
                    var elementID = _this.attr("id") || _this.attr("name");
                    if (data.Success || data.success) {
                        data = data.Data || data.data;
                        var _data = data.Data || data.data;
                        var select2Current = options.select2Current = elementID;
                        results = _data.map(function (e) {
                            e.id = e.Id || e.id;
                            e.FullName = e.FullName || "";
                            e.Phone = e.Phone || "";
                            e.Email = e.Email || "";
                            e.Address = e.Address || "";

                            e = options.SetDataView(e);

                            return e;
                        });

                        var TotalRow = data.TotalRow || data.totalRow;
                        var Page = params.page || 1;
                        var PageSize = data.PageSize || data.pageSize;

                        return {
                            results: results,
                            pagination: {
                                more: (Page * PageSize) < TotalRow
                            }
                        };
                    }
                    else {
                        return null;
                    }
                },
                cache: true
            },
            escapeMarkup: function (markup) {
                //debugger
                return markup;
            },
            minimumInputLength: 1,
            templateResult: function (data) {
                if (data.loading)
                    return data.text;
                var _return = "<div class='select2_group_select clearfix'>\
                                            <div class='select2_group_select_img'>\
                                                <img src='" + (data.AvatarImage || "/Assets/images/no-thumbnail.jpg") + "'/>\
                                            </div>\
                                            <div class='select2_group_select_content'>\
                                                <div style='font-weight: bold;'>\
                                                    #"+ data.id + " " + data.name + "\
                                                </div>\
                                                <div style='font-style: italic; color: #a9a59e;'>\
                                                    <i class='fa fa-envelope-o'></i>" + data.FullName + "\
                                                </div>\
                                                <div style='font-style: italic; color: #a9a59e;'>\
                                                    <i class='fa fa-envelope-o'></i>" + data.Email + "\
                                                </div>\
                                                <div style='font-style: italic; color: #a9a59e;'>\
                                                    <i class='glyphicon glyphicon-earphone'></i>" + data.PhoneNumber + "\
                                                </div>\
                                                <div style='font-style: italic; color: #a9a59e;'>\
                                                    <i class='glyphicon glyphicon-home'></i>"+ data.Address + "\
                                                </div>\
                                            </div>\
                                        </div>\
                                        ";

                if (options.GetViewSelect == 1) {

                }
                else if (options.GetViewSelect == 2) {
                    _return = data.name;
                }
                else if (typeof (options.GetViewSelect) == "function") {
                    _return = options.GetViewSelect(data, options.select2Current);
                }
                else {
                    _return = data.name;
                }

                return _return;
            },
            templateSelection: function (data) {
                var _return = '';
                if (options.GetViewSelect == 1) {
                    return "#" + data.id + "-" + (data.name || data.text) + (data.FullName != null ? " - " + data.FullName : "");
                }
                else if (options.GetViewSelect == 2) {
                    _return = data.name;
                }
                else if (typeof (options.GetViewSelect) == "function") {
                    _return = options.GetViewSelect(data, options.select2Current);
                }
                else {
                    _return = data.name;
                }
                return _return;
            }
        }, options.config);

        options.Elem.select2(options.config);

        return options;
    },
    setData: function (elem, val, data) {
        var elem = $(elem);
        data = data || [];
        if (data.length > 0) {
            elem.empty();
            var option = "";
            data.map(function (e) {
                option += '<option value="' + e.id + '">' + e.label + '</option>';
            });
            elem.html(option);
        }
        elem.val(val).trigger("change");
    },
    getData: function (elem) {
        var elem = $(elem);
        return elem.select2('data');
    }
}

function CalculatorPage(total, pageSize) {
    if (pageSize <= 0) {
        return total;
    }
    if (total <= 0) {
        return 1;
    }
    var page = Math.round(total / pageSize);
    var percent = total % pageSize
    page += percent < pageSize / 2 && percent != 0 ? 1 : 0;
    return page;
}

var convertObject = function (object, UpperCase) {
    var checkUpperCase = function (value) {
        value = value || "";
        return value[0] == value[0].toUpperCase();
    }
    var checkLowerCase = function (value) {
        value = value || "";
        return value[0] == value[0].toLowerCase();
    }

    var toUpperCase = function (value) {
        value = value || "";
        return value.substr(0, 1).toUpperCase() + value.substr(1);
    }

    var toLowerCase = function (value) {
        value = value || "";
        return value.substr(0, 1).toLowerCase() + value.substr(1);
    }

    var obj = {};
    for (var key in object) {
        var temp = object[key];

        if (UpperCase == null) {
            key = checkUpperCase(key) ? toLowerCase(key) : toUpperCase(key);
        }
        else if (UpperCase == true) {
            key = toUpperCase(key);
        }
        else {
            key = toLowerCase(key);
        }

        if (temp != null && typeof (temp) == 'object') {
            if (temp.constructor == Object) {
                temp = convertObject(temp, UpperCase);
            }
            else if (temp.constructor == Array) {
                for (var i in temp) {
                    temp[i] = convertObject(temp[i], UpperCase);
                }
            }
        }

        obj[key] = temp;
    }
    return obj;
}
//#endregion functionExtend

var BtDefault = {
    formatVND: function (value, row, elm) {
        return (value || "").toVND();
    },
    formatPercent: function (value, row, elm) {
        return (value || "").toPercent();
    },
    validateCell: function ($el, value, rule) {
        //var $el = $(this);
        $el = $($el);
        var _this = $el.closest("table");
        var index = $el.closest('[data-index]').data("index");
        var field = $el.data("name");
        var objTemp = {};
        objTemp[field] = value;
        var errors = ObjectValid.valid(objTemp, rule);

        if (errors.length > 0) {
            return errors[0].message;
        }
    }
}

var ChatBox = {
    init: function () {
        //for(var i in appSetting.localStorage.Data.ChatBox.Current){
        //    var item = appSetting.localStorage.Data.ChatBox.Current[i];
        //    ChatBox.Create({
        //        Title: item.Title,
        //        AvatarImg: item.AvatarImg,
        //        UserToId: item.UserToId,
        //        Id: item.Id
        //    });
        //}
    },
    Html: '\
        <div class="panel panel-chat" id="{0}">\
            <div class="panel-heading">\
                <a href="#" class="chatMinimize" onclick="return false"><span>{1}</span></a>\
                <a href="#" class="chatClose" onclick="return false"><i class="glyphicon glyphicon-remove"></i></a>\
                <div class="clearFix"></div>\
            </div>\
            <div class="panel-body">\
                <div class="messageMe">\
                    <img src="http://www.bitrebels.com/wp-content/uploads/2011/02/Original-Facebook-Geek-Profile-Avatar-1.jpg" alt="" />\
                    <span>asdasdssadasdasdassssssssssssssssssssssssssssssssssssssssssdasdasd</span>\
                    <div class="clearFix"></div>\
                </div>\
                <div class="clearFix"></div>\
            </div>\
            <div class="panel-footer">\
                <textarea name="textMessage" cols="0" rows="0"></textarea>\
            </div>\
        </div>\
    ',
    List: [],
    GenericId: function (UserId, UserToId) {
        UserId = eval(UserId);
        UserToId = eval(UserToId);
        return 'ChatBox-' + (UserId > UserToId ? UserToId + 'x' + UserId : UserId + 'x' + UserToId);
    },
    ReloadPos: function () {
        //return
        for (var i = 0; i < ChatBox.List.length; i++) {
            var item = ChatBox.List[i];
            var elm = $(item.elm);
            var left = (screen.width - 50) - ((i + 1) * (320));
            elm.css("left", left).css("display", "inline-table");
        }
    },
    Create: function (options) {
        var _this = this;
        var html = ChatBox.Html.format(options.Id, options.Title);
        var root = $("#ChatBoxRoot");

        root.append(html);

        options.elm = root.find("#" + options.Id);
        var body = options.elm.find(".panel-body").html("");

        var imgDefault = '/Assets/images/Avatar-Thumbnail.png';

        var current = "";
        options.addMessage = function (obj) {

            obj.isMe = obj.isMe == null ? true : obj.isMe;
            body.append('\
                <div class="message {2} start end">\
                    <img src="{0}" alt="" />\
                    <span>{1}</span>\
                    <div class="clearFix"></div>\
                </div>\
            '.format(obj.AvatarImg || imgDefault, obj.Message, obj.isMe == null || obj.isMe == true ? 'messageMe' : 'messageHer'));

            if (current !== '') {
                if (current == obj.isMe) {
                    body.find(".message:last").removeClass("start");
                    body.find(".message:last").prev().removeClass("end");
                }
                else {
                    //body.find(".message:last").prev().removeClass("end");
                }
            }

            current = obj.isMe;

            body.animate({ scrollTop: body[0].scrollHeight }, 0);
        }

        options.elm.find('[name="textMessage"]').keyup(function (event) {
            var target = $(event.currentTarget);
            if (event.keyCode == 13) {
                var message = target.val().trim();
                if (message != "") {
                    options.addMessage({
                        Message: message,
                        isMe: true,
                        AvatarImg: options.AvatarImg || imgDefault
                    });

                    AppHubConnect.Hub.SendMessageTo(options.UserToId, message).then(function (data) {
                        //console.log(data)
                    });
                }
                target.val("");
                //root.find('.panel-body').animate({ height: "0" }, 500);
            }
        });

        options.elm.find(".panel-heading > .chatMinimize").click(function () {
            if ($(this).parent().parent().hasClass('mini')) {
                $(this).parent().parent().removeClass('mini').addClass('normal');

                options.elm.find('.panel-body').animate({ height: "250px" }, 500).show();

                options.elm.find('.panel-footer').animate({ height: "75px" }, 500).show();
            }
            else {
                $(this).parent().parent().removeClass('normal').addClass('mini');

                options.elm.find('.panel-body').animate({ height: "0" }, 500);

                options.elm.find('.panel-footer').animate({ height: "0" }, 500);

                setTimeout(function () {
                    options.elm.find('.panel-body').hide();
                    options.elm.find('.panel-footer').hide();
                }, 500);


            }

        });
        options.elm.find(".panel-heading > .chatClose").click(function () {
            $(this).parent().parent().remove();
            ChatBox.List = ChatBox.List.filter(function (e) { return e.Id != options.Id });

            ChatBox.ReloadPos();
            //appSetting.localStorage.Data.ChatBox.Current = appSetting.localStorage.Data.ChatBox.Current.filter(function (e) { return e.Id != options.Id });
            //appSetting.localStorage.Save();
        });

        ChatBox.List.push(options);

        ChatBox.ReloadPos();

        //appSetting.localStorage.Data.ChatBox.Current.push({
        //    Title: options.Title,
        //    AvatarImg: options.AvatarImg,
        //    UserToId: options.UserToId,
        //    Id: options.Id
        //});
        //appSetting.localStorage.Save();

        return options;
    }
}

var HubObj = {
    //This will allow same connection to be used for all Hubs
    //It also keeps connection as singleton.
    globalConnections: [],
    initNewConnection: function (options) {
        var connection = null;
        if (options && options.rootPath) {
            connection = $.hubConnection(options.rootPath, { useDefaultPath: false });
        } else {
            connection = $.hubConnection();
        }

        connection.logging = (options && options.logging ? true : false);
        return connection;
    },

    getConnection: function (options) {
        var _this = this;
        var useSharedConnection = !(options && options.useSharedConnection === false);
        if (useSharedConnection) {
            return typeof _this.globalConnections[options.rootPath] === 'undefined' ?
			_this.globalConnections[options.rootPath] = _this.initNewConnection(options) :
			_this.globalConnections[options.rootPath];
        }
        else {
            return _this.initNewConnection(options);
        }
    },

    Create: function (hubName, options) {
        var _this = this;
        var Hub = {};

        Hub.connection = HubObj.getConnection(options);
        Hub.proxy = Hub.connection.createHubProxy(hubName);

        Hub.on = function (event, fn) {
            Hub.proxy.on(event, fn);
        };
        Hub.invoke = function (method, args) {
            return Hub.proxy.invoke.apply(Hub.proxy, arguments)
        };
        Hub.disconnect = function () {
            Hub.connection.stop();
        };
        Hub.connect = function () {
            return Hub.connection.start(options.transport ? { transport: options.transport } : null);
        };

        if (options && options.listeners) {
            Object.getOwnPropertyNames(options.listeners)
			.filter(function (propName) {
			    return typeof options.listeners[propName] === 'function';
			})
		        .forEach(function (propName) {
		            Hub.on(propName, options.listeners[propName]);
		        });
        }
        if (options && options.methods) {
            //angular.forEach(, function (method) {
            //for (var i in options.methods) {
            $.each(options.methods, function (index, method) {
                //var method = options.methods[i];
                Hub[method] = function () {
                    var args = $.makeArray(arguments);
                    args.unshift(method);
                    return Hub.invoke.apply(Hub, args);
                };
            });
        }
        Hub.AddListener = function (listener, func) {
            Hub.on(listener, func);
        }
        Hub.AddMethod = function (method) {
            Hub[method] = function () {
                var args = $.makeArray(arguments);
                args.unshift(method);
                return Hub.invoke.apply(Hub, args);
            };
        }
        if (options && options.queryParams) {
            Hub.connection.qs = options.queryParams;
        }
        if (options && options.errorHandler) {
            Hub.connection.error(options.errorHandler);
        }
        //DEPRECATED
        //Allow for the user of the hub to easily implement actions upon disconnected.
        //e.g. : Laptop/PC sleep and reopen, one might want to automatically reconnect 
        //by using the disconnected event on the connection as the starting point.
        if (options && options.hubDisconnected) {
            Hub.connection.disconnected(options.hubDisconnected);
        }
        if (options && options.stateChanged) {
            Hub.connection.stateChanged(options.stateChanged);
        }

        //Adding additional property of promise allows to access it in rest of the application.
        Hub.promise = Hub.connect();
        return Hub;
    }
};

var ScrollByParams = function (elmScrollName, elmParent, elmName, animate_) {
    var elmScrollName = $(elmScrollName);
    var table = $(elmParent);
    var row = $(elmName);
    if (row.length > 1) {
        row = row.last();
    }
    var divScrollTop = elmScrollName.offset().top;
    var tableTop = table.offset().top;
    var rowTop = row.offset().top;
    var scroll = Math.abs(tableTop - rowTop);
    elmScrollName.scrollTop(scroll);
    elmScrollName.scrollLeft(0);
    var animate = "flash animated";
    if (animate_) {
        animate = animate_;
    }
    // var animate = "fadeIn animated"
    row.addClass(animate);
    setTimeout(function () {
        row.removeClass(animate);
    }, 1000);
}

var fileExtension = function (filename) {
    return "." + filename.substr((filename.lastIndexOf('.') + 1));
}
var fileName = function (filename) {
    return filename.replace(/^.*[\\\/]/, '');
}
var fileNameNotExtentsion = function (filename) {
    return filename.replace(/.[^.]+$/, '');
}

var SortArrayObject = {
    defaultCompare: function(row1, row2, field) {
        var x = row1[field]
        var y = row2[field]

        if (x === y) return 0;
        return x > y ? 1 : -1;
    }
}
var CropperObj = {
    Avatar: function (options) {
        options.aspectRatio = options.aspectRatio || (16 / 9);
        options.uploadURL = options.uploadURL || "/Upload/UploadImage";
        options.callback = options.callback || function (e) {
        }
        options.crop = options.crop || function (e) {
            // Output the result data for cropping image.
            //console.log(e);
        }
        options.elemRoot = $(options.elemRoot);
        options.elem = $(options.elem);

        options.built = function () {
            options.elem.cropper("setCropBoxData", { width: "100", height: "50" });
        }

        options.elem.cropper(options);

        $inputImage = options.elemRoot.find('#inputImage');
        $inputImage.change(function () {
            var files = this.files,
                file;

            if (files && files.length) {
                file = files[0];

                if (/^image\/\w+$/.test(file.type)) {
                    blobURL = URL.createObjectURL(file);
                    options.elem.one('built.cropper', function () {
                        URL.revokeObjectURL(blobURL); // Revoke when load complete
                    }).cropper('reset', true).cropper('replace', blobURL);
                    $inputImage.val('');
                } else {
                    //showMessage('Please choose an image file.');
                }
            }
        });
        options.elemRoot.find('[data-method]').click(function () {
            var data = $(this).data(),
                $target,
                result;

            if (data.method) {
                data = $.extend({}, data); // Clone a new one

                if (typeof data.target !== 'undefined') {
                    $target = $(data.target);

                    if (typeof data.option === 'undefined') {
                        try {
                            data.option = JSON.parse($target.val());
                        } catch (e) {
                            console.log(e.message);
                        }
                    }
                }

                result = options.elem.cropper(data.method, data.option);

            }
        });
        var _return = {
            options: options,
            elem: options.elem,
            UploadCropper: function () {
                //var result = options.elem.cropper("getDataURL", {height: 180, width: 180});
                //console.log(result)
                //return
                options.elem.cropper('getCroppedCanvas', {
                    fillColor: "#fff",
                    imageSmoothingEnabled: false,
                    imageSmoothingQuality: "high"
                }).toBlob(function (blob) {
                    console.log(blob)
                    var formData = new FormData();

                    formData.append('file', blob, 'avatar.jpg');
                    formData.append('folder', "/Avatar");

                    //return;

                    page_loading.show();

                    $.ajax(options.uploadURL, {
                        method: "POST",
                        data: formData,
                        processData: false,
                        contentType: false,
                        success: function (data) {
                            if (options.callback) {
                                options.callback(data);
                            }
                            page_loading.hide();
                        },
                        error: function () {
                            console.log('Upload error');
                            page_loading.hide();
                        }
                    });
                }, 'image/jpeg');
            }
        }
        return _return;
    }
}
var setFormData = function (data, form) {
    form = $(form);
    for (var key in data) {
        var value = data[key];
        var elms = form.find('[name="' + key + '"]');
        elms.each(function (index, elm) {
            elm = $(elm);
            var tag = elm.prop("tagName");
            if (tag == "INPUT") {
                if (["checkbox", "radio"].indexOf(elm.attr("type")) != -1) {
                    elm.attr('checked', value);
                }
                else {
                    elm.val(value);
                }
            }
            else if (tag == "SELECT") {
                elm.val(value);
            }
            else if (tag == "LABEL") {
                elm.text(value);
            }
        });
    }
}
var SaveFileAs = function (uri, filename) {
    var link = document.createElement('a');
    if (typeof link.download === 'string') {
        link.href = uri;
        link.download = filename;

        //Firefox requires the link to be in the body
        document.body.appendChild(link);

        //simulate click
        link.click();

        //remove the link when done
        document.body.removeChild(link);
    } else {
        window.open(uri);
    }
}