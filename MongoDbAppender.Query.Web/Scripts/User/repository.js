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
            that.name = data.name;
            that.stat = data.stat;
            that.state = AjaxState.Ready;
            $(that).dequeue("update");
        }).fail(function () {
            that.state = AjaxState.Fail;
        })
    },
    update: function (container, templateObj) {
        var update = function (data) {
            var template = templateObj.html();
            Mustache.parse(template);
            var html = Mustache.render(template, data);
            container.html(html);
        }.bind(this);

        // display a default view first.
        update({
            appRoot: appRoot,
            name: this.name,
            stat: {
                all: "loading...",
                trace: "loading...",
                debug: "loading...",
                info: "loading...",
                warn: "loading...",
                error: "loading...",
                fatal: "loading...",
            }
        });
        
        if (this.state == AjaxState.Ready) {
            update({
                appRoot: appRoot,
                name: this.name,
                stat: this.stat
            });
        } else if (this.state == AjaxState.Loading) {
            $(this).queue("update", function (next) {
                this.update(container, templateObj);
                next();
            }.bind(this));
        } else if (this.state == AjaxState.Fail) {
            update({
                appRoot: appRoot,
                name: this.name,
                stat: {
                    all: "loading failed",
                    trace: "loading failed",
                    debug: "loading failed",
                    info: "loading failed",
                    warn: "loading failed",
                    error: "loading failed",
                    fatal: "loading failed",
                }
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