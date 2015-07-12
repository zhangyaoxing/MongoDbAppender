function Repository(name, statMins) {
    this.URL = "/api/repositories/";
    this.name = name;
    if (statMins) {
        this.statMins = statMins;
    }
    this.state = AjaxState.Init;
    this.refresh();
}

Repository.prototype = {
    refresh: function () {
        var that = this;
        this.state = AjaxState.Loading;
        $.ajax({
            url: this.URL + this.name + (this.statMins ? ("?statMins=" + this.statMins) : "")
        }).done(function (data) {
            debugger;
            that.name = data.name;
            that.stat = data.stat;
            $(that).dequeue("update");
        }).fail(function () {
            // TODO: reload
        })
    },
    update: function (container, templateObj) {
        var update = function () {
            var templateHtml = templateObj.html();
            var template = Mustache.parse(templateHtml);
            var html = Mustache.render(template, {
                name: this.name,
                stat: this.stat
            });
            container.append(html);
        }.bind(this);
        
        if (this.state == AjaxState.Ready) {
            update()
        } else {
            $(this).queue("update", function (next) {
                update();
                next();
            });
        }
    }
};

var AjaxState = {
    Init: "Init",
    Loading: "Loading",
    Ready: "Ready",
    Fail: "Fail"
};