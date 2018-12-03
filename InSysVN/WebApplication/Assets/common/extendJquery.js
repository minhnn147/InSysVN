(function ($) {
    var NAMESPACE = 'DataViewTemplate';
    var DataViewTemplate = function (elm, options) {
        elm = $(elm);
        options = options || {};

        options = jQuery.extend(true, {
            URLTemplate: "/Base/DataViewTemplate",
            ViewURL: "",
            AutoSerialize: false,
            isAppend: false,
            BtTable: null,
            List: [],
            Load: function () { },
            Add: null,
            Del: function () { },
            GetData: function (element, index, _this, dataRow, serialize) { return dataRow },
            DataSend: function (_this) {
                return {

                };
            },
            FunExtend: []
        }, options);

        options.isAppend = false;

        var configBT = {};
        if (options.BtTable != null) {
            configBT.ruleObj = options.BtTable.ruleObj || {};
            configBT.calculatorRow = options.BtTable.calculatorRow || function () { };

            options.BtTable.config = options.BtTable.config || {};

            configBT.config = jQuery.extend(true, {
                striped: true,
                pagination: true,
                paginationVAlign: 'bottom',
                limit: 10,
                pageSize: 10,
                pageList: [10, 25, 50, 100, 200],
                showFooter: true,
                search: true,
                onEditableSave: function (field, row, oldValue, $el) {
                    var _this = $el.closest("table");
                    var index = $el.closest('[data-index]').data("index");

                    configBT.calculatorRow(row, field, oldValue, $el);

                    _this.bootstrapTable("updateRow", row);
                },
                onEditableShown: function (field, row, $el, editable) {
                    var index = $el.closest('[data-index]').data("index");
                    app.component.InputCurrency();
                },
                detailView: false,
                detailFormatter: function (index, row, elm) {
                    var check = ObjectValid.valid(row, configBT.ruleObj);
                    var html = 'ul';
                    return '<ul class="text-danger">' + (check.map(function (e) {
                        return '<li>{0}</li>'.format(e.message);
                    }).join(" ")) + '</ul>';
                },
                columns: [],
                data: []
            }, options.BtTable.config);

            configBT.config.columns = [];

            if (options.BtTable.config.isColumnValid) {
                configBT.config.columns.push({
                    field: 'valid',
                    title: '<i class="fa fa-check" aria-hidden="true"></i>',
                    align: 'center',
                    valign: 'top',
                    sortable: true,
                    formatter: function (value, row, index) {
                        //configBT.calculatorRow(row);
                        var check = ObjectValid.valid(row, configBT.ruleObj);
                        return (check.length > 0 ? '<i class="fa fa-ban" aria-hidden="true" style="color: red"></i>' : '<i class="fa fa-check" aria-hidden="true" style="color: green"></i>');
                    }
                });
            }

            if (options.BtTable.config.isSTT) {
                configBT.config.columns.push({
                    field: '',
                    title: 'STT',
                    align: 'center',
                    valign: 'top',
                    formatter: function (value, row, index) {
                        return index + 1;
                    }
                });
            }

            for (var i in options.BtTable.config.columns) {
                var e = options.BtTable.config.columns[i];
                var column = jQuery.extend(true, {
                    field: 'productName',
                    title: 'Tên sản phẩm',
                    align: 'left',
                    valign: 'top',
                    sortable: true
                }, e);

                if (e.isEditable) {
                    column.editable = jQuery.extend(true, {
                        //mode: "inline",
                        //showbuttons: false,
                        onblur: "submit",
                        inputclass: 'input-width-grid',
                        type: 'text',
                        highlight: "#FFFF80",
                        success: function (response, newValue) {
                            //newValue = parseInt(newValue);
                            //return {
                            //    newValue: newValue
                            //};
                        },
                        validate: function (value) {
                            var $el = $(this);
                            var _this = $el.closest("table");
                            var index = $el.closest('[data-index]').data("index");
                            var field = $el.data("name");
                            var objTemp = {};
                            objTemp[field] = value;
                            var errors = ObjectValid.valid(objTemp, configBT.ruleObj);

                            if (errors.length > 0) {
                                return errors[0].message;
                            }
                        },
                        disabled: true
                    }, e.editable);
                }
                configBT.config.columns.push(column);
            }

            if (options.BtTable.config.isDelRow) {
                configBT.config.columns.push({
                    field: 'operate',
                    title: 'Actions',
                    align: 'center',
                    valign: 'middle',
                    searchable: false,
                    formatter: function (value, row, index) {
                        var errors = ObjectValid.valid(row, configBT.ruleObj);
                        return [
                             '<a data-toggle="modal" class="info ml10 ' + (errors.length > 0 ? '' : 'hidden') + '" href="javascript:void(0)" title="">',
                                '<i class="glyphicon glyphicon glyphicon-exclamation-sign"></i>',
                            '</a>',
                             '<a data-toggle="modal" class="remove ml10" href="javascript:void(0)" title="xóa">',
                                '<i class="glyphicon glyphicon-remove"></i>',
                            '</a>',
                        ].join(' ');
                    },
                    events: {
                        'click .remove': function (e, value, row, index) {
                            modal.DeleteComfirm({
                                callback: function () {
                                    var data = elm.bootstrapTable('getData');
                                    data.splice(index, 1);
                                    elm.bootstrapTable('load', data);
                                }
                            });
                        },
                        'click .info': function (e, value, row, index) {

                            var errors = ObjectValid.valid(row, configBT.ruleObj);
                            modal.LoadAjax({
                                title: "Cảnh báo",
                                html: '<ul class="text-danger">' + (errors.map(function (e) {
                                    return '<li>{0}</li>'.format(e.message);
                                }).join(" ")) + '</ul>'
                            });
                        }
                    },
                });
            }
        }

        var template = {
            Data: null,
            List: [],
            Elem: elm,
            FolderTemp: "",
            DataSend: function () {
                var _this = this;
                var data = options.DataSend(_this);
                data.index = !options.isAppend ? 0 : _this.List.length - 1 <= 0 ? 0 : _this.List.length - 1;
                data.View = options.ViewURL;
                if (!options.isAppend) {
                    data.data = _this.List;
                }
                else {
                    data.data = _this.ListAdd;
                }
                if ("/Base/DataViewTemplate" == options.URLTemplate) {
                    data.data = data.data.map(function (e) {
                        return JSON.stringify(e);
                    });
                }

                return data;
            },
            Load: function (list) {
                var _this = this;


                if (options.BtTable != null) {
                    if (!_this.Elem.data("bootstrap.table")) {
                        _this.Elem.bootstrapTable(configBT.config);
                    }
                    if (list != null) {
                        list = list.map(function (e) {
                            e.valid = ObjectValid.valid(e, configBT.ruleObj).length == 0;
                            configBT.calculatorRow(e);
                            return e;
                        });
                        _this.Elem.bootstrapTable("load", list);
                    }
                    return;
                }

                if (list != null) {
                    _this.List = list;
                    _this.ListAdd = list;
                }

                _this.List = _this.List || [];
                var data = _this.DataSend();
                _AjaxAPI.post(options.URLTemplate, data, function (data) {
                    var elm;
                    if (!options.isAppend) {
                        _this.Elem.html(data);
                        elm = _this.Elem;
                    }
                    else {
                        elm = $(data);
                        _this.Elem.append(elm);
                    }

                    app.component.load();

                    elm.find(!options.isAppend ? "[ElementIndex] .btnDel" : ".btnDel").click(function () {
                        var elm = $(this);
                        var row = elm.closest("[ElementIndex]");
                        var index = eval(row.attr("ElementIndex"));
                        _this.Del(index);
                    });
                    options.Load(_this, elm);
                });
            },
            Add: function (data) {
                if (data == null) {
                    return;
                }
                if (!$.isArray(data)) {
                    data = [data];
                }
                var _this = this;
                if (options.Add) {
                    data = data.map(function (e) {
                        return options.Add(_this, e);
                    }).filter(function (e) { return e != null });
                }


                if (options.BtTable != null) {

                    data = data.map(function (e) {
                        e.valid = ObjectValid.valid(e, configBT.ruleObj).length == 0;
                        configBT.calculatorRow(e);
                        return e;
                    });
                    _this.Elem.bootstrapTable("append", data);
                    return;
                }

                _this.ListAdd = [];
                if (options.AutoSerialize) {
                    var serialize = element.find("*");
                    serialize = serialize.serializeObject({ validateName: false });
                    _this.List = [];
                }
                else {
                    _this.List = _this.GetData();
                }
                _this.List = _this.List.concat(data);
                if (!options.isAppend) {
                }
                else {
                    _this.ListAdd = data;
                }
                _this.Load();
            },
            Del: function (index) {
                var _this = this;
                modal.DeleteComfirm({
                    callback: function () {
                        //_this.List = _this.GetData();
                        _this.List.splice(index, 1);
                        options.Del(_this, index);
                        var ell = _this.Elem.find('[ElementIndex="' + index + '"]');
                        ell.remove();
                        //_this.Load();
                    }
                });
            },
            GetData: function () {
                var _this = this;

                if (options.BtTable != null) {
                    var data = _this.Elem.bootstrapTable("getData");
                    if (options.GetData) {
                        data = data.map(function (e, index) {
                            return options.GetData(null, index, _this, e, null);
                        });
                    }
                    return data;
                }

                var data = [];
                var form = _this.Elem;
                form.find("[ElementIndex]").each(function (index, element) {
                    element = $(element);
                    var serialize = element.find("*");
                    serialize = serialize.serializeObject({ validateName: false });
                    var dataRow = _this.List[index];
                    var obj = options.GetData(element, index, _this, dataRow, serialize);
                    data.push(obj);
                });
                return data;
            },
            CheckBT: function () {
                var _this = this;
                if (options.BtTable != null) {
                    _this.Elem.bootstrapTable("resetSearch");
                    var datas = _this.Elem.bootstrapTable("getData");
                    for (var i = 0; i < datas.length; i++) {
                        var e = datas[i];
                        if (ObjectValid.valid(e, configBT.ruleObj).length > 0) {
                            var pageSize = _this.Elem.bootstrapTable('getOptions').pageSize;
                            var page = CalculatorPage(i + 1, pageSize);
                            _this.Elem.bootstrapTable('selectPage', page);
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        return template;
    }
    function isUndefined(n) {
        return typeof n === 'undefined';
    }
    $.fn.DataViewTemplate = function (options) {
        for (var _len = arguments.length, args = Array(_len > 1 ? _len - 1 : 0), _key = 1; _key < _len; _key++) {
            args[_key - 1] = arguments[_key];
        }
        var result;
        this.each(function (i, element) {
            var $this = $(element);
            var data = $this.data(NAMESPACE);
            if (data != null) {

                if (typeof options === 'string') {
                    var fn = data[options];

                    if ($.isFunction(fn)) {
                        result = fn.apply(data, args);
                    }
                }

            }
            else {
                $this.data(NAMESPACE, data = new DataViewTemplate(element, options));
                data.Load(options.List);
            }

        });
        return isUndefined(result) ? this : result;
    }
})(jQuery);

(function ($) {
    var NAMESPACE = 'UploadSingle';
    var UploadSingle = function (elm, options) {
        elm = $(elm);

        var _return = {
            Load: function () {
                options = $.extend(true, {
                    url: '/Upload/UploadImage',
                    folder: 'ImgTest',
                    imgView: '',
                    prgressBar: '',
                    fileSize: 10,
                    multiUpload: false,
                    onUpload: function () {

                    },
                    onSuccess: function () {

                    },
                    onProgress: function () {

                    },
                    onFailed: function () {

                    }
                }, options);

                var inputFile = elm.find('input[type="file"]');
                var inputReturn = elm.find('input.value');
                var divProgress = elm.find('.progress');
                if (inputFile.length <= 0) {
                    inputFile = $('<input type="file">');
                    elm.append(inputFile);
                }

                if (inputReturn.length <= 0) {
                    inputReturn = $('<input type="hidden" name="value" class="value">');
                    elm.append(inputReturn);
                }

                if (divProgress.length <= 0 && options.prgressBar == '') {
                    divProgress = $('\
                        <div class="progress"> \
                            <div class="progress-bar-succes progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="40" aria-valuemin="0" aria-valuemax="100" style="width=0px;"> \
                                0% \
                            </div> \
                        </div>\
                    ');
                    elm.append(divProgress);
                }
                else if (options.prgressBar != '') {
                    divProgress = $(options.options.prgressBar)
                }
                divProgress.hide();

                elm.addClass("btn-file");

                inputFile.change(function (event) {
                    var _this = $(this);
                    var files = _this[0].files;
                    if (window.FormData !== undefined) {
                        var formData = new FormData();
                        for (var x = 0; x < files.length; x++) {
                        }
                        formData.append("file", files[0]);
                        formData.append("folder", options.folder);

                        options.onUpload(elm);
                        $.ajax({
                            type: 'POST',
                            url: options.url,
                            data: formData,
                            dataType: 'json', // what to expect back from the script, if anything
                            cache: false,
                            contentType: false,
                            processData: false,  // tell jQuery not to convert to form data
                            progress: function (progress) {
                                if (progress.lengthComputable) {
                                    var total = Math.round(progress.loaded / progress.total * 100);

                                    divProgress.show();
                                    divProgress.find('.progress-bar').css({ width: total });
                                    divProgress.find('.progress-bar').text("" + total + "%");

                                    options.onProgress(total, elm);

                                } else {
                                    console.warn('Content Length not reported!');
                                }
                            },
                            success: function (data) {
                                divProgress.hide();
                                if (data.Success || data.success) {
                                    inputReturn.val((data.Data || data.data).fileLink);
                                }
                                else {
                                    options.onFailed(elm);
                                }
                                options.onSuccess(data, elm);
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                options.onFailed(elm);
                            }
                        });
                    }
                    _this.val('');
                });
            }
        }

        return _return;
    }

    function isUndefined(n) {
        return typeof n === 'undefined';
    }
    $.fn.UploadSingle = function (options) {
        for (var _len = arguments.length, args = Array(_len > 1 ? _len - 1 : 0), _key = 1; _key < _len; _key++) {
            args[_key - 1] = arguments[_key];
        }
        var result;
        this.each(function (i, element) {
            var $this = $(element);
            var data = $this.data(NAMESPACE);
            if (data != null) {

                if (typeof options === 'string') {
                    var fn = data[options];

                    if ($.isFunction(fn)) {
                        result = fn.apply(data, args);
                    }
                }

            }
            else {
                $this.data(NAMESPACE, data = new UploadSingle(element, options));
                data.Load();
            }

        });
        return isUndefined(result) ? this : result;
    }
})(jQuery);

(function ($) {
    var NAMESPACE = 'UploadCopperImg';
    var UploadCopperImg = function (elemTarget, options) {
        elemTarget = $(elemTarget);

        var _return = {
            Load: function () {
                options = $.extend(true, {
                    //option Copper
                    aspectRatio: (16 / 9),
                    //aspectRatio: 1 / 1,
                    width: 500,
                    height: 400,
                    viewMode: 2,
                    dragMode: "move",

                    elemTarget: elemTarget,
                    uploadUrl: "/Upload/UploadImage",
                    folder: 'ImgTest',
                    imgView: '',
                    hiddenFiled: '',
                    title: "Upload Image",
                    imgDefault: "/App_Assets/images/thumbnail-default.jpg",
                    popupClass: 'modal-lg',
                    onUpload: function (formData, image_cropper, modal) {
                        return formData;
                    },
                    onSuccess: function () {

                    },
                    onProgress: function () {

                    },
                    onFailed: function () {

                    }
                }, options);

                var htmlPopup = $('#UploadCopperImgTemplate').html();

                var modalPopup = modal.LoadAjax({
                    title: options.title,
                    html: htmlPopup,
                    classname: options.popupClass,
                    backdrop: 'static',
                    buttonClose: {
                        Text: "Đóng",
                        isShow: false,
                        isIconX: true
                    },
                    buttonOk: {
                        Text: "Đồng ý",
                        isShow: false,
                    },
                });

                var Load = function (img) {
                    setTimeout(function () {
                        var image_cropper = modalPopup.modal.find("#image_cropper");
                        image_cropper.attr('src', img);

                        var fileExtensionUpload = fileExtension(img);

                        image_cropper.cropper(options);

                        $inputImage = modalPopup.modal.find('#inputImage');
                        $inputImage.change(function () {
                            var files = this.files,
                            file;

                            if (files && files.length) {
                                file = files[0];

                                fileExtensionUpload = fileExtension(file.name);

                                if (/^image\/\w+$/.test(file.type)) {
                                    blobURL = URL.createObjectURL(file);
                                    image_cropper.one('built.cropper', function () {
                                        URL.revokeObjectURL(blobURL); // Revoke when load complete
                                    }).cropper('reset', true).cropper('replace', blobURL);
                                    $inputImage.val('');
                                } else {
                                    //showMessage('Please choose an image file.');
                                }
                            }
                        });

                        modalPopup.modal.find('[data-method]').click(function () {
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

                                result = image_cropper.cropper(data.method, data.option);

                            }
                        });
                        modalPopup.modal.find('#btnUpload').click(function () {
                            image_cropper.cropper('getCroppedCanvas', {
                                fillColor: "#fff",
                                imageSmoothingEnabled: false,
                                imageSmoothingQuality: "high"
                            }).toBlob(function (blob) {
                                //console.log(blob)
                                var formData = new FormData();

                                formData.append('file', blob, 'img_' + (new Date().getTime()) + fileExtensionUpload);
                                formData.append('folder', options.folder);

                                if (options.onUpload) {
                                    formData = options.onUpload(formData, image_cropper, modalPopup.modal);
                                }

                                page_loading.show();

                                $.ajax(options.uploadUrl, {
                                    method: "POST",
                                    data: formData,
                                    processData: false,
                                    contentType: false,
                                    success: function (data) {
                                        if (data.Success || data.success) {
                                            if (options.onSuccess) {
                                                options.onSuccess(data);
                                            }
                                            data = data.Data || data.data;
                                            if (options.imgView != "") {
                                                var view = $(options.imgView);
                                                view.attr('src', data.fileLink);
                                            }
                                            if (options.hiddenFiled != "") {
                                                var view = $(options.hiddenFiled);
                                                view.val(data.fileLink);
                                            }
                                            if (options.elemTarget[0].nodeName == "IMG") {
                                                options.elemTarget.attr('src', data.fileLink);
                                            }
                                            Notification.Success("Upload thành công");
                                            modalPopup.remove();
                                        }
                                        else {
                                            Notification.Error(data.Message || data.message);
                                        }
                                        page_loading.hide();
                                    },
                                    error: function () {
                                        Notification.Error('Upload error');
                                        console.info('Upload error', arguments);
                                        page_loading.hide();
                                    }
                                });
                            }, 'image/jpeg');
                        });

                    }, 300);
                }

                var imgDefault = "/App_Assets/images/thumbnail-default.jpg";
                var img = new Image();
                img.src = options.imgDefault;
                img.onload = function () {
                    Load(options.imgDefault);
                };
                img.onerror = function () {
                    Load(imgDefault);
                };
            }
        }
        return _return;
    }
    function isUndefined(n) {
        return typeof n === 'undefined';
    }
    $.fn.UploadCopperImg = function (options) {
        for (var _len = arguments.length, args = Array(_len > 1 ? _len - 1 : 0), _key = 1; _key < _len; _key++) {
            args[_key - 1] = arguments[_key];
        }
        var result;
        this.each(function (i, element) {
            var $this = $(element);
            var data = $this.data(NAMESPACE);
            if (data != null) {

                if (typeof options === 'string') {
                    var fn = data[options];

                    if ($.isFunction(fn)) {
                        result = fn.apply(data, args);
                    }
                }

            }
            else {
                $this.data(NAMESPACE, data = new UploadCopperImg(element, options));
                data.Load();
            }

        });
        return isUndefined(result) ? this : result;
    }
})(jQuery);

(function ($) {
    //http://johnny.github.io/jquery-sortable/
    var NAMESPACE = 'SortableTool';
    var SortableTool = function (elm, options) {
        elm = $(elm);

        var getCheckedOl = function (elm) {

            var allLI = elm.find('> li.sortable-tool-li-item');

            var Data = [];

            var check = 0;

            for (var i = 0; i < allLI.length; i++) {
                var elmChild = $(allLI[i]);
                var dataItem = dataTemp[elmChild.data('index')];
                var olChild = elmChild.find('> ol.sortable-tool-parent');

                var isChild = false;
                var isChild = olChild.find('> li.sortable-tool-li-item').length > 0;

                var btnExpand = elmChild.find('.btnExpand');
                var nextOlParent = elmChild.next();

                if (olChild.length <= 0) {
                    elmChild.addClass('item-not-child');
                }

                if (btnExpand.length > 0) {
                    if (isChild) {
                        btnExpand.show();
                    }
                    else {
                        btnExpand.hide();
                    }
                }

                dataItem.childs = [];
                if (olChild.length > 0 && isChild) {
                    dataItem.childs = getCheckedOl(olChild);
                }

                var btnCheck = elmChild.find('> .sortable-tool-item > .btnCheck');
                if (btnCheck.hasClass('check')) {
                    check++;
                    dataItem.isChecked = true;
                    dataItem.isMinus = false;
                }
                else if (btnCheck.hasClass('uncheck')) {
                    dataItem.isChecked = false;
                    dataItem.isMinus = false;
                }
                else if (btnCheck.hasClass('minus')) {
                    dataItem.isChecked = false;
                    dataItem.isMinus = true;
                }

                if (i == allLI.length - 1) {
                    var btnCheck = elm.prev().find('.btnCheck');
                    if (btnCheck.length <= 0) {

                    }
                    else {
                        btnCheck.removeClass('uncheck').removeClass('minus').removeClass('check');
                        if (check > 0) {
                            if (check == allLI.length) {
                                btnCheck.addClass('check');
                            }
                            else {
                                btnCheck.addClass('minus');
                            }
                        }
                        else {
                            btnCheck.addClass('uncheck');
                        }
                    }
                }

                Data.push(dataItem);
            }
            return Data;
        }


        var htmlLi = $('#sortable_templateLi').html();
        var dataTemp = {};
        var indexLocal = 0;

        var repeatElm = function (datas, level) {
            level = level || 1;
            var html = '';
            if (level == 1) {
                html = '<ol class="sortable-tool-parent">';
            }
            for (var i = 0; i < datas.length; i++) {
                var item = datas[i];
                indexLocal++;

                var indexHtml = 'index-' + indexLocal;
                dataTemp[indexHtml] = item;

                html += '<li class="sortable-tool-li-item" data-index="' + indexHtml + '">';

                //html += htmlLi.format(item.label);
                html += options.templateItem(item);

                if (item.childs != null && item.isChild) {
                    html += '<ol class="sortable-tool-parent">';
                    html += repeatElm(item.childs, level + 1);
                    html += "</ol>";
                }
                html += "</li>";

            }
            if (level == 1) {
                html += "</ol>";
            }
            return html;
        }

        var _return = {
            checkAll: function (bool) {
                bool = bool != null ? bool : false;
                elm.find('.sortable-tool-item .btnCheck').removeClass('uncheck').removeClass('minus').removeClass('check').addClass(!bool ? 'uncheck' : 'check');
            },
            expandAll: function (bool) {
                bool = bool != null ? bool : false;
                elm.find('.btnExpand').removeClass('isExpand').removeClass('isCollapse').addClass(bool ? 'isCollapse' : 'isExpand').trigger('click');
            },
            refresh: function () {
                return getCheckedOl(options.elm.find('> ol.sortable-tool-parent'));
            },
            GetData: function () {
                return getCheckedOl(options.elm.find('> ol.sortable-tool-parent'));
            },
            Load: function (datas) {
                options = $.extend(true, {
                    elm: elm,
                    isShowCheck: false,
                    isShowMove: true,
                    isShowExpand: true,
                    data: [
                        {
                            id: 0,
                            label: "dkamphuoc1",
                            isChild: true,
                            childs: []
                        }
                    ],
                    templateItem: function (item) {
                        return htmlLi.format(item.label);
                    },
                    onDrag: function () {
                    },
                    onClickCheck: function () {

                    },
                    onProgress: function () {

                    },
                    onFailed: function () {

                    }
                }, options);

                elm.html("");

                var html = repeatElm(datas || options.data);
                elm.html(html);

                if (options.isShowCheck) {
                    elm.find('li .btnCheck').show();
                }
                else {
                    elm.find('li .btnCheck').hide();
                }

                if (options.isShowMove) {
                    elm.find('li .btnMove').show();
                }
                else {
                    elm.find('li .btnMove').hide();
                }

                if (options.isShowExpand) {
                    elm.find('li .btnExpand').show();
                }
                else {
                    elm.find('li .btnExpand').hide();
                }

                elm.find('> ol').sortable({
                    handle: '.btnMove',
                    onDrop: function ($item, position, _super, event) {
                        getCheckedOl(options.elm.find('> ol.sortable-tool-parent'));
                        _super($item, position, _super, event);
                        options.onDrag($item, position, _super, event);
                    }
                });

                elm.find('.btnExpand').click(function () {
                    var elm = $(this);
                    var isCollapse = elm.hasClass('isCollapse');
                    elm.removeClass('fa fa-chevron-down').removeClass('fa fa-chevron-right').removeClass('isCollapse').removeClass('isExpand').addClass(isCollapse ? 'fa fa-chevron-down' : 'fa fa-chevron-right').addClass(isCollapse ? 'isExpand' : 'isCollapse');

                    elm.closest('.sortable-tool-item').next().removeClass('isCollapse').removeClass('isExpand').addClass(isCollapse ? 'isExpand' : 'isCollapse');
                });
                elm.find('.btnCheck').click(function () {
                    var elm = $(this);
                    var isChecked = elm.hasClass('check');
                    var isMinus = elm.hasClass('minus');
                    elm.removeClass('uncheck').removeClass('minus').removeClass('check');
                    if (isChecked == true) {
                        elm.addClass('uncheck');
                    }
                    else {
                        elm.addClass('check');
                    }

                    elm.closest('.sortable-tool-item').next().find('.sortable-tool-item .btnCheck').removeClass('uncheck').removeClass('minus').removeClass('check').addClass(isChecked ? 'uncheck' : 'check');
                    getCheckedOl(options.elm.find('> ol.sortable-tool-parent'));
                    options.onClickCheck();
                });

                getCheckedOl(options.elm.find('> ol.sortable-tool-parent'));
            }
        }
        return _return;
    }
    function isUndefined(n) {
        return typeof n === 'undefined';
    }
    $.fn.SortableTool = function (options) {
        for (var _len = arguments.length, args = Array(_len > 1 ? _len - 1 : 0), _key = 1; _key < _len; _key++) {
            args[_key - 1] = arguments[_key];
        }
        var result;
        this.each(function (i, element) {
            var $this = $(element);
            var data = $this.data(NAMESPACE);
            if (data != null) {

                if (typeof options === 'string') {
                    var fn = data[options];

                    if ($.isFunction(fn)) {
                        result = fn.apply(data, args);
                    }
                }

            }
            else {
                $this.data(NAMESPACE, data = new SortableTool(element, options));
                data.Load();
            }

        });
        return isUndefined(result) ? this : result;
    }
})(jQuery);

(function ($) {
    $.fn.serializeObject = function (options) {
        options = jQuery.extend(true, {
            validateName: true
        }, options);

        var self = this,
            json = {},
            push_counters = {},
            patterns = {
                "validate": /^[a-zA-Z][a-zA-Z0-9_]*(?:\[(?:\d*|[a-zA-Z0-9_]+)\])*$/,
                "key": /[a-zA-Z0-9_]+|(?=\[\])/g,
                "push": /^$/,
                "fixed": /^\d+$/,
                "named": /^[a-zA-Z0-9_]+$/
            };


        this.build = function (base, key, value) {
            base[key] = value;
            return base;
        };

        this.push_counter = function (key) {
            if (push_counters[key] === undefined) {
                push_counters[key] = 0;
            }
            return push_counters[key]++;
        };

        $.each($(this).serializeArray(), function () {

            // skip invalid keys
            if (!patterns.validate.test(this.name) && options.validateName) {
                return;
            }

            var k,
                keys = this.name.match(patterns.key),
                merge = this.value,
                reverse_key = this.name;

            while ((k = keys.pop()) !== undefined) {

                // adjust reverse_key
                reverse_key = reverse_key.replace(new RegExp("\\[" + k + "\\]$"), '');

                // push
                if (k.match(patterns.push)) {
                    merge = self.build([], self.push_counter(reverse_key), merge);
                }

                    // fixed
                else if (k.match(patterns.fixed)) {
                    merge = self.build([], k, merge);
                }

                    // named
                else if (k.match(patterns.named)) {
                    merge = self.build({}, k, merge);
                }
            }

            json = $.extend(true, json, merge);
        });

        return json;
    };
})(jQuery);
