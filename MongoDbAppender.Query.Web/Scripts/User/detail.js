function Detail(name, tmplId) {
    this.URL = "/api/repositories/";
    this.name = name;
    this.tmplId = tmplId;
}

Detail.prototype = {
    refresh: function (callback) {
        $.ajax({
            url: this.URL + this.name
        }).done(function (data) {

        }).fail(function () {

        })
        callback(html);
    }
};