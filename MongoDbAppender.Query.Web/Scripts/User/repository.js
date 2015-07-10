function Repository(name) {
    this.URL = "/api/repositories/";
    this.name = name;
}

Repository.prototype = {
    load: function (callback) {
        $.ajax({
            url: this.URL + this.name
        }).done(function (data) {
            alert(data);
            callback(html);
        }).fail(function () {

        })
    }
};