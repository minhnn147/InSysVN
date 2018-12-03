Date.prototype.formatDate = function (format) {
    format = format == null ? app.config.Date.Moment : format;
    return moment(this).format(format);
}
Date.prototype.formatDateTime = function () {
    return this.formatDate(app.config.DateTime.Moment);
}