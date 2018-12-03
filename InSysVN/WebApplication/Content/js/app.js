var app = {
    config: {
        Date: {
            Moment: "DD/MM/YYYY"
        },
        DateTime: {
            Moment: 'DD/MM/YYYY hh:mm:ss a'
        },
        Currency: {
            Suffix: " VNĐ",
            DecimalPlaces: 0,
            DecimalSeparator: ",",
            ThousandsSeparator: "."
        }
    },
    localStorage: {
        name: "",
        Data: null,
        init: function () {
            var _this = this;
            _this.name = window.location.host + appSetting.User.UserId;
            _this.Data = $.extend(true, {
                ChatBox: {
                    Current: []
                }
            }, JSON.parse(window.localStorage.getItem(_this.name) || "{}"));

            _this.Save();
        },
        Save: function () {
            var _this = this;
            window.localStorage.setItem(_this.name, JSON.stringify(_this.Data));
        }
    },
    documentReady: [],
    init: function () {
        //$.fn.datepicker.dates['vi'] = {
        //    days: ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'],
        //    daysShort: ['C.Nhật', 'T.Hai', 'T.Ba', 'T.Tư', 'T.Năm', 'T.Sáu', 'T.Bảy'],
        //    daysMin: ['CN', 'T.Hai', 'T.Ba', 'T.Tư', 'T.Năm', 'T.Sáu', 'T.Bảy'],
        //    months: ['Tháng Một', 'Tháng Hai', 'Tháng Ba', 'Tháng Tư', 'Tháng Năm', 'Tháng Sáu', 'Tháng Bảy', 'Tháng Tám', 'Tháng Chín', 'Tháng Mười', 'Tháng Mười Một', 'Tháng Mười Hai'],
        //    monthsShort: ['Một', 'Hai', 'Ba', 'Tư', 'Năm', 'Sáu', 'Bảy', 'Tám', 'Chín', 'Mười', 'M.Một', 'M.Hai'],
        //    today: "Hôm nay",
        //    clear: "Xóa",
        //    format: "dd/mm/yyyy",
        //    titleFormat: "MM yyyy", /* Leverages same syntax as 'format' */
        //    weekStart: 0
        //};

        this.component.init();

        for (var i = 0; i < this.documentReady.length; i++) {
            this.documentReady[i]();
        }

        moment.locale("vi");
        this.component.registerHandleInput();
    },
    component: {
        init: function () {
            this.load();
        },
        load: function () {

            //this.select2Default();
            //this.InputCurrency();
            this.FormatText();
            this.FormatInput();
            //this.ValidateForm.init();
            //this.ValidateForm.defaultRegistry();
            //this.DatePicker();
            //this.FormatInputNumber();
            //this.UploadCopperImg();
            //this.ImageView.Single();
            //this.ImgError();

        },
        FormatInput: function () {
            //https://github.com/autoNumeric/autoNumeric
            $('[ipPosInt]').each(function (index, elm) {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    new AutoNumeric(this, opPosInt);
                }
            });
            $('[ipMoney]').each(function (index, elm) {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    new AutoNumeric(this, opMoney);
                }
            });
            $('[ipPercent]').each(function (index, elm) {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    new AutoNumeric(this, opPercent);
                }
            });
        },
        FormattxtMore: function () {
            $('[txtMoneyMore]').each(function (index, elm) {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    ///Define_Autonumeric.js
                    $(this).text(formatMoney($(this).text()));
                }
            });
            $('[txtNumberMore]').each(function () {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    ///Define_Autonumeric.js
                    $(this).text(formatNumber($(this).text()));
                }
            });
        },
        ValidateInputPhone: function () {
            $("input[type='tel']").keydown(function (e) {
                // Allow: backspace, delete, tab, escape, enter and .
                if ($(this).val().substring(0, 2) == '01') { $(this).attr("minlength", 11); $(this).attr("maxlength", 11); }
                else { $(this).attr("minlength", 10); $(this).attr("maxlength", 10); }
                var check = true;
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
                    // Allow: Ctrl+A, Command+A
                    (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: home, end, left, right, down, up
                    (e.keyCode >= 35 && e.keyCode <= 40)) {
                    check = false;
                    // let it happen, don't do anything
                    return;
                }
                // Ensure that it is a number and stop the keypress
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    check = false;
                    e.preventDefault();
                }
                if (check) {
                    if ($(this).val().substring(0, 1) != '0') {
                        $(this).val('0' + $(this).val());
                    }
                }
            });
        },
        ValidateEmail: function () {
            $('input[type="email"]').on('keypress', function (event) {
                var regex = new RegExp("^[a-zA-Z0-9@._]+$");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            });
        },
        FormatText: function (index, elm) {
            $('[txtNumber]').each(function () {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    ///Define_Autonumeric.js
                    $(this).text(formatNumber($(this).text()));
                }
            });
            $('[txtMoney]').each(function (index, elm) {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    ///Define_Autonumeric.js
                    $(this).text(formatMoney($(this).text()));
                }
            });
            $('[txtDateTime]').each(function (index, elm) {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    ///method.js
                    $(this).text(formatToDateTime($(this).text()));
                }
            });
        },
        InputCurrency: function () {
            //money format
            $('[InputNumber]').each(function (index, elm) {
                elm = $(elm);
                options = elm.data();
                options.currencyMaskMap = options.currencyMaskMap || "";
                if (!elm.data("numFormat")) {
                    var DecimalPlaces = 0;
                    var DecimalSeparator = ",";
                    var ThousandsSeparator = ".";
                    if ((elm.data("percent") || false) == true) {
                        DecimalPlaces = 0;
                        DecimalSeparator = ",";
                        ThousandsSeparator = "";
                    }
                    elm.number(true, DecimalPlaces, DecimalSeparator, ThousandsSeparator);
                    //elm.number(true, 0, ",", ".");
                    if (options.currencyMaskMap != "" && options.currencyMaskMap != null && options.currencyMaskMap == true) {
                        var name = elm.prop("name");
                        elm.attr("name", name + "Mask");
                        $('<input type="hidden" name="{0}" value="{1}"/>'.format(name, elm.val())).insertAfter(elm);
                        var _event = function () {
                            var elm = $(this);
                            var a = elm.val();
                            $('[name="' + name + '"]').val(a);
                        }
                        var _event1 = function () {
                            var elm = $(this);
                            var a = elm.val();
                            $('[name="' + name + "Mask" + '"]').val(a);
                        }
                        elm.on("keyup", _event).on("change", _event);
                        $('[name="' + name + '"]').on("keyup", _event1).on("change", _event1);
                    }
                }
            });

        },
        select2Default: function () {
            var elements = $('[select2Search]');
            for (var i = 0; i < elements.length; i++) {
                var elem = $(elements[i]);
                if (!elem.data('select2')) {
                    elem.select2(elem.data());
                }
            }
            this.select2FixPosition();
        },
        select2FixPosition: function () {
            //select2 fix position
            $(".select2-hidden-accessible").map(function (index, elm) {
                $(elm).data("select2").on("results:message", function () {
                    this.dropdown._positionDropdown();
                });
            });

            $("select").on("select2:close", function (e) {
                var _this = $(this);
                var form = _this.closest("form");
                if (form.length > 0 && form.data("validator"))
                    _this.valid();
            });
        },
        modalEvent: function () {
            var modelCheck = function (show) {
                if ($('.modal').hasClass('in') && !$('body').hasClass('modal-open')) {
                    $('body').addClass('modal-open');
                }
            }
            $('.modal').on('hidden.bs.modal', function () {
                modelCheck(false);
            });
            $('.modal').on('shown.bs.modal', function () {
                modelCheck(true);
            });
            modelCheck(true);
        },
        ValidateForm: {
            init: function () {
                var _this = this;
                var getOptionForm = function (element) {
                    var form = $(element).closest("form");
                    var optionsData = form.data();
                    optionsData.tooltip = optionsData.tooltip == null ? false : optionsData.tooltip;
                    optionsData.tooltipPlacement = optionsData.tooltipPlacement == null ? "top" : optionsData.tooltipPlacement;
                    return optionsData;
                }
                $.validator.setDefaults({
                    errorElement: "span", // contain the error msg in a small tag
                    errorClass: 'form-text danger',
                    errorPlacement: function (error, element) {// render error placement for each input type
                        //if (element.attr("type") == "radio" || element.attr("type") == "checkbox") {// for chosen elements, need to insert the error after the chosen container
                        //    error.insertAfter($(element).closest('.form-group').children('div').children().last());
                        //} else if (element.attr("name") == "card_expiry_mm" || element.attr("name") == "card_expiry_yyyy") {
                        //    error.appendTo($(element).closest('.form-group').children('div'));
                        //} else {
                        //    if (error.text() != "") {
                        //        error.insertAfter(element);
                        //    }
                        //    // for other inputs, just perform default behavior
                        //}

                        var form = $(element).closest("form");

                        var optionsData = getOptionForm(element);
                        if (optionsData.tooltip) {

                            var addTooltip = function (element) {
                                var mess = error.text();
                                var elm = $(element);
                                elm.data("message-error", mess);
                                elm.tooltip({
                                    placement: optionsData.tooltipPlacement,
                                    title: mess,
                                    trigger: "hover focus"
                                })
                                    .attr('data-original-title', mess)
                                    .tooltip('show');
                            }

                            if (element.parent('.input-group').length) {
                                addTooltip(element);    // radio/checkbox?
                            } else if (element.hasClass('select2-hidden-accessible')) {
                                addTooltip(element.next('span'));
                            } else {
                                addTooltip(element);
                            }
                        }
                        else {
                            if (element.parent('.input-group').length) {
                                error.insertAfter(element.parent());      // radio/checkbox?
                            } else if (element.hasClass('select2-hidden-accessible')) {
                                error.insertAfter(element.next('span'));  // select2
                            } else {
                                error.insertAfter(element);//default
                            }
                        }

                        form.find("validation-summary-errors").hide();
                    },
                    //ignore: '',
                    ignore: ':hidden, table input',
                    success: function (label, element) {
                        var elm = $(element);
                        var optionsData = getOptionForm(element);
                        if (optionsData.tooltip) {
                            var element1 = elm;
                            if (elm.hasClass('select2-hidden-accessible')) {
                                element1 = $(elm.next('span'));
                            }
                            if (typeof ($(element1).tooltip) == "function" && $(element1).data("bs.tooltip") != null) {
                                $(element1).tooltip('destroy');
                            }
                        }

                        label.addClass('help-block valid');
                        $(element).closest('.form-group').removeClass('has-danger');

                        _this.override.success(label, element);
                    },
                    highlight: function (element) {
                        var elm = $(element);
                        var optionsData = getOptionForm(element);
                        if (optionsData.tooltip) {
                            var mess = $(element).data("message-error") || "";
                            if (mess != "") {
                                if (typeof (elm.tooltip) == "function" && elm.data("bs.tooltip") != null) {
                                    elm.tooltip('show');
                                    //$(element).tooltip('destroy')
                                }
                                else {
                                    elm.tooltip({
                                        placement: optionsData.tooltipPlacement,
                                        title: mess,
                                        trigger: "hover focus"
                                    })
                                        .attr('data-original-title', mess)
                                        .tooltip('show');
                                }

                                //elm.tooltip({
                                //    placement: "top",
                                //    title: mess,
                                //    trigger: "hover focus"
                                //});
                                //$(element).tooltip("show");
                            }
                        }

                        $(element).closest('.help-block').removeClass('valid');
                        $(element).closest('.form-group').removeClass('has-danger');
                        // setTimeout(function () {
                        // }, 00);
                        $(element).closest('.form-group').addClass('has-danger');

                        _this.override.highlight(element);
                    },
                    unhighlight: function (element) {
                        var elm = $(element);
                        var optionsData = getOptionForm(element);
                        if (optionsData.tooltip) {
                            var element1 = elm;
                            if (elm.hasClass('select2-hidden-accessible')) {
                                element1 = $(elm.next('span'));
                            }
                            if (typeof (element1.tooltip) == "function" && element1.data("bs.tooltip") != null) {
                                element1.tooltip('destroy')
                            }
                        }
                        $(element).closest('.form-group').removeClass('has-danger');

                        _this.override.unhighlight(element);
                    }
                });
                // setDefaults jquery validate ==============================================================================
                $.validator.addMethod("selectNotNull", function (value, element) {
                    value = value || "";
                    if (isNaN(value + "")) {
                        return (value + "").trim() != "";
                    }
                    value = value.replace("number:", "");
                    return value > 0;
                }, "Hãy nhập.");
                //}, "This field is required.");

                $.validator.methods.date = function (value, element) {
                    return this.optional(element) || moment(value, 'DD/MM/YYYY').toDate() !== null;
                };


            },
            defaultRegistry: function () {
                $("form.form_validate").each(function (index, elm) {
                    elm = $(elm);
                    if (!elm.data("validator")) {
                        var validOption = {
                            invalidHandler: function (event, validator) {
                            },
                            validateOnInit: true
                        };

                        var dataOptions = elm.data();
                        dataOptions.autoSubmit = dataOptions.autoSubmit == null ? (dataOptions.onSubmit == "" || dataOptions.onSubmit == null) : dataOptions.autoSubmit;
                        dataOptions.validateExtend = dataOptions.validateExtend == null ? true : dataOptions.validateExtend;

                        if (!dataOptions.autoSubmit || true) {
                            validOption.submitHandler = function (form) {
                                if (event != null) {
                                    event.stopPropagation();
                                    event.preventDefault();
                                }
                                form = $(form);

                                var dataOptions = form.data();
                                dataOptions.autoSubmit = dataOptions.autoSubmit == null ? true : dataOptions.autoSubmit;
                                dataOptions.onSubmit = dataOptions.onSubmit == null ? "" : dataOptions.onSubmit;
                                dataOptions.validateExtend = dataOptions.validateExtend == null || dataOptions.validateExtend == "" ? function () { return true; } : dataOptions.validateExtend;

                                var validateExtend = eval(dataOptions.validateExtend);
                                if (validateExtend == false || ($.isFunction(validateExtend) && !validateExtend())) {
                                    return;
                                }


                                var enctype = form.attr("enctype") || "";
                                var action = form.attr("action");

                                var data = form.serializeObject();

                                try {
                                    if (enctype.toLowerCase() == "multipart/form-data".toLowerCase()) {
                                        var data1 = new FormData(form[0]);
                                        form.find('input[type="file"]').map(function (index, elm) {
                                            data1.append(elm.name, elm.files);
                                        });
                                        for (var i in data) {
                                            data1.append(i, data[i]);
                                        }
                                        data = data1;

                                        if (!dataOptions.autoSubmit) {
                                            if (dataOptions.onSubmit != "") {
                                                eval(dataOptions.onSubmit)(form, data);
                                            }
                                            else {
                                                _AjaxAPI.formData(action, data, function (data) {
                                                    console.log(data);
                                                });
                                            }

                                        }
                                        else {
                                            var validator = form.validate();
                                            validator.destroy();
                                            form.submit();
                                        }
                                    }
                                    else {
                                        if (!dataOptions.autoSubmit) {
                                            if (dataOptions.onSubmit != "") {
                                                eval(dataOptions.onSubmit)(form, data);
                                            }
                                            else {
                                                _AjaxAPI.post(action, data, function (data) {
                                                    console.log(data);
                                                });
                                            }
                                        }
                                        else {
                                            var validator = form.validate();
                                            validator.destroy();
                                            form.submit();
                                        }
                                    }
                                } catch (ex) { }
                            };
                        }
                        else {

                        }

                        elm.validate(validOption);
                    }
                });
            },
            override: {
                success: function (label, element) {
                },
                highlight: function (element) {
                    var _element = $(element);

                    // show tab
                    var show_tab = function (e) {
                        var tab_content = e.closest(".tab-pane");
                        if (tab_content.length == 0) {
                            return;
                        }
                        var tab_content_id = tab_content.attr("id");
                        var find_btn_tab = $('[href="#' + tab_content_id + '"][data-toggle="tab"]');
                        if (find_btn_tab.length != 0) {
                            find_btn_tab.click();
                            show_tab(find_btn_tab);
                        }
                    }
                    show_tab(_element);
                    // ==================================
                },
                unhighlight: function (element) {
                }
            }
        },
        DatePicker: function () {
            //bootstrap-datepicker.js
            //bootstrap-datepicker.min.js
            //https://bootstrap-datepicker.readthedocs.io/en/stable/
            $('[datepicker]').each(function (index, elm) {
                var datenow = new Date();
                var yearnow = datenow.getFullYear();

                $(this).datepicker({
                    format: "dd/mm/yyyy",
                    todayBtn: true,
                    clearBtn: true,
                    language: "vi",
                    calendarWeeks: true,
                    autoclose: true,
                    todayHighlight: true,
                    startDate: new Date(yearnow - 100 + '-01-01'),
                    endDate: new Date(yearnow + 100 + '-01-01')
                });
                console.log($(this).datepicker('getStartDate'));
            });
        },
        EventCalculate: function () {
            $('[data-oncalculate]').each(function (index, elm) {
                var run = function (elm) {
                    var func = elm.data("oncalculate") || ""
                    func = eval(func);
                    if ($.isFunction(func)) {
                        func();
                    }
                }
                elm = $(elm);
                elm.on('keyup change blur', function () {
                    var elm = $(this);
                    if (elm.data("numFormat")) {
                        setTimeout(function () {
                            run(elm);
                        }, 100);
                    }
                    else {
                        run(elm);
                    }
                });
                run(elm);
            });
        },
        TextAreaAutoFitContent: function () {
            $('textarea[auto-fitcontent]').each(function (index, elm) {
                elm = $(elm);
                elm.height(elm[0].scrollHeight);
                //elm.on('keyup change blur', function () {
                //    elm.height( elm[0].scrollHeight );
                //});
            });
        },
        ImgError: function () {
            $('img').on('error', function () {
                if (!$(this).hasClass('broken-image')) {
                    $(this).prop('src', '/App_Assets/images/thumbnail-default.jpg').addClass('broken-image');
                }
            });
        },
        SingleUpload: function () {
            $('[data-upload-single]').each(function (index, elm) {
                elm = $(elm);
                if (!elm.data("UploadSingle")) {
                    elm.UploadSingle(elm.data());
                }
            });
        },
        UploadCopperImg: function () {
            $('[data-provider="UploadCopperImg"]').each(function (index, elm) {
                elm = $(elm);
                if (!elm.data("UploadCopperImg")) {
                    elm.css('cursor', 'pointer');
                    elm.click(function (event) {
                        event.stopPropagation();
                        event.preventDefault();
                        if (!elm.data("UploadCopperImg")) {
                            elm.UploadCopperImg(elm.data());
                        }
                        else {
                            elm.UploadCopperImg("Load");
                        }
                    });
                }
            });
        },
        ImageView: {
            Single: function () {
                $('[data-provider="image-viewer-single"]').each(function (index, elm) {
                    elm = $(elm);
                    elm.css('cursor', 'pointer');
                    elm.click(function (event) {
                        event.stopPropagation();
                        event.preventDefault();
                        var options = elm.data();
                        options = $.extend(true, {
                            target: "",
                        }, options);

                        var url = "";
                        if (elm[0].nodeName == "IMG") {
                            url = elm.attr('src');
                        }
                        else if (options.target != "") {
                            var target;
                            target = $(options.target);
                            url = target.attr('src');
                        }
                        else {
                            url = elm.find("img").attr("src");
                        }
                        if (url == "") {
                            return;
                        }

                        var body = $("body");
                        body.css("overflow", "hidden");

                        var data_caption = target.data("caption") || "";
                        var modal = $("#modal-images-viewer-single");
                        var img = modal.find("#img");
                        var caption = modal.find("#caption");
                        var close = modal.find("#close");
                        close[0].onclick = function () {
                            modal.hide();
                            body.css("overflow", "auto");
                        }
                        img.attr("src", url);
                        img.css("max-height", window.innerHeight - 40 + "px");
                        caption.text(data_caption);
                        modal.show();
                    });
                });
            }
        },
        registerHandleInput: function () {

        },
        Loading: {
            html: '<div class="showLoading"><div class="loader"></div></div>',
            Show: function () {
                if ($(".showLoading").length > 0) {
                    $(".showLoading").each(function () {
                        $(this).remove();
                    });
                }
                $("body").append(app.component.Loading.html);
            },
            Hide: function () {
                if ($(".showLoading").length > 0) {
                    $(".showLoading").each(function () {
                        $(this).remove();
                    });
                }
            }
        }
    }
}