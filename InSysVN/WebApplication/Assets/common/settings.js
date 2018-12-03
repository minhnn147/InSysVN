var AppHubConnect = {
    Hub: null,
    Load: function(){
        var _this = this;
        console.log(1)
        _this.Hub = new HubObj.Create('ConnectionApp', {
            transport: 'longPolling',
            listeners: {
                'Listener_GetAllConnect': function (AllConnect) {
                    //console.log(AllConnect)
                },
                'Listener_Message': function (obj) {
                    var ChatBoxId = ChatBox.GenericId(appSetting.User.UserId, obj.From.UserId);
                    var search = ChatBox.List.filter(function (e) {
                        return e.Id == ChatBoxId;
                    })[0];

                    if (search == null) {
                        search = new ChatBox.Create({
                            Title: obj.From.UserName,
                            AvatarImg: appSetting.User.AvatarImg,
                            UserToId: obj.From.UserId,
                            Id: ChatBoxId
                        });
                    }

                    search.addMessage({
                        Message: obj.Message,
                        isMe: false,
                        AvatarImg: obj.From.AvatarImg,
                        Time: obj.From.Time
                    });
                },
                'Listener_SystemMessage': function (obj) {
                }
            },
            methods: [
                'GetAllConnect',
                'SendMessageTo',
            ],
            errorHandler: function (error) {
                console.info(error);
            }
        });

        _this.Hub.promise.then(function () {
            console.log(arguments)
        });
    }
}

//#region settingApp
var appSetting = {
    User: {
        UserId: 0,
        AvatarImg: "",
    },
    config: {
        date:{
            moment: "DD/MM/YYYY",

        },
    },
    localStorage:{
        name: "",
        Data: null,
        init: function(){
            var _this = this;
            _this.name = window.location.host + appSetting.User.UserId;
            _this.Data = $.extend(true,{
                ChatBox: {
                    Current: []
                }
            },JSON.parse(window.localStorage.getItem(_this.name) || "{}"));

            _this.Save();
        },
        Save: function(){
            var _this = this;
            window.localStorage.setItem(_this.name, JSON.stringify( _this.Data));
        }
    },
    documentReady: [],
    init: function () {
        var _this = this;

        _this.localStorage.init();

        AppHubConnect.Load();

        ChatBox.init();

        $(document).on('blur', '.input_blur', function (event) {
            this.value = this.value.trim();
        });
        $(document).on('click', '.input_select', function (event) {
            this.select();
        });
        _this.addExtedComponent.ValidateForm.init();
        _this.addExtedComponent.load();

        for(var i in appSetting.documentReady){
            appSetting.documentReady[i]();
        }
    },
    addExtedComponent: {
        load: function () {
            var _this = this;
            _this.select2Default();
            _this.select2FixPosition();
            _this.modalEvent();
            _this.ValidateForm.defaultRegistry();
            _this.InputCurrency();
            _this.DatePicker();
            _this.EventCalculate();
            _this.TextAreaAutoFitContent();
            _this.SingleUpload();

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
                    errorClass: 'danger',
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
                        $(element).closest('.form-group').removeClass('has-error');

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
                        $(element).closest('.form-group').removeClass('has-error');
                        // setTimeout(function () {
                        // }, 00);
                        $(element).closest('.form-group').addClass('has-error');

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
                        $(element).closest('.form-group').removeClass('has-error');

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
        InputCurrency: function () {
            //money format
            $('[currencyMask]').each(function (index, elm) {
                elm = $(elm);
                options = elm.data();
                options.currencyMaskMap = options.currencyMaskMap || "";
                if (!elm.data("numFormat")) {
                    var DecimalPlaces = 0;
                    var DecimalSeparator = ".";
                    var ThousandsSeparator = ",";
                    if((elm.data("percent") || false) == true){
                        DecimalPlaces = 0;
                        DecimalSeparator = ".";
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
        DatePicker: function () {
            //date picker
            $('.datePicker').each(function (index, elm) {
                elm = $(elm);
                if (!elm.data("datepicker")) {
                    var options = elm.data();
                    //options.pickerFormat = options.pickerFormat || "mm/dd/yyyy";
                    options.pickerFormat = options.pickerFormat || "dd/mm/yyyy";
                    options.clearBtn = options.clearBtn != null ? options.clearBtn : false;

                    elm.datepicker({
                        format: options.pickerFormat,
                        weekStart: 8,
                        maxViewMode: 2,
                        todayBtn: "linked",
                        calendarWeeks: true,
                        autoclose: true,
                        todayHighlight: true,
                        clearBtn: options.clearBtn
                    });

                    var date = elm.val() || "";
                    date = new Date(date);
                    if (!isNaN(date)) {
                        elm.datepicker('update', date);
                    }
                }
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
        TextAreaAutoFitContent: function(){
            $('textarea[auto-fitcontent]').each(function (index, elm) {
                elm = $(elm);
                    elm.height( elm[0].scrollHeight );
                //elm.on('keyup change blur', function () {
                //    elm.height( elm[0].scrollHeight );
                //});
            });
        },
        SingleUpload: function(){
            $('[data-upload-single]').each(function (index, elm) {
                elm = $(elm);
                if (!elm.data("UploadSingle")) {
                    elm.UploadSingle(elm.data());
                }
            });
        }
    },
};


$(document).ready(function () {
    appSetting.init();
});
//#endregion settingApp