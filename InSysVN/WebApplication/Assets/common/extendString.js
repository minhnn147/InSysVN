String.prototype.escapeHTML = function () {
    return (
      this.replace(/>/g, '&gt;').
           replace(/</g, '&lt;').
           replace(/"/g, '&quot;')
    );
};
String.prototype.toObject = function () {
    try {
        return JSON.parse(this);
    }
    catch (ex) {
        return {};
    }
}
String.prototype.toPercent = function () {
    var temp = this.length == 0 ? "0" : this;
    return (temp) + " %";
}
String.prototype.toVND = function () {
    return $.number(this || 0, app.config.Currency.DecimalPlaces, app.config.Currency.ThousandsSeparator, app.config.Currency.DecimalSeparator) + app.config.Currency.Suffix;
}
String.prototype.dateFromJsonCSharp = function () {
    return dateFromJsonCSharp(this)
}
String.repeat = function (chr, count) {
    var str = "";
    for (var x = 0; x < count; x++) { str += chr };
    return str;
}
String.prototype.padL = function (width, pad) {
    if (!width || width < 1)
        return this;
    if (!pad) pad = " ";
    var length = width - this.length;
    if (length < 1) return this.substr(0, width);

    return (String.repeat(pad, length) + this).substr(0, width);
}
String.prototype.padR = function (width, pad) {
    if (!width || width < 1)
        return this;
    if (!pad) pad = " ";
    var length = width - this.length;
    if (length < 1) this.substr(0, width);

    return (this + String.repeat(pad, length)).substr(0, width);
}
String.prototype.replaceAll = function (search, replacement) {
    return this.split(search).join(replacement);
};
String.prototype.formatDateTime = function () {
    return (new Date(this)).formatDateTime();
}
String.prototype.toDate = function (format) {
    format = format || app.config.Date.Moment;
    return moment(this, format).toDate();
}
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
          ? args[number]
          : match
        ;
    });
};