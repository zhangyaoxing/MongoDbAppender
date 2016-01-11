function Repository(name, statMins) {
    var that = this;
    this.URL = appRoot + "api/repositories/";
    this.name = name;
    if (statMins) {
        this.statMins = statMins;
    }
    this.state = AjaxState.Init;
    var detectActiveLevel = function () {
        var levelStr = $.url(window.location.href).fparam('level');
        var levelStr = levelStr ? levelStr.toLowerCase() : "";
        var activeLevel = Level.All;
        for (var level in Level) {
            if (level.toLowerCase() == levelStr) {
                activeLevel = level;
                break;;
            }
        }
        return activeLevel;
    };
    this.activeLevel = detectActiveLevel();
    $(window).on("hashchange", function () {
        // refresh panel when level changes.
        var activeLevel = detectActiveLevel();
        if (that.activeLevel != activeLevel) {
            that.activeLevel = activeLevel;
            that.update();
        }
    });
    //this.refresh();
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
            $(that).dequeue("update" + that.name);
        }).fail(function () {
            that.state = AjaxState.Fail;
        })
    },
    update: function (container, templateObj) {
        container = container || this.container;
        templateObj = templateObj || this.templateObj;
        if (!this.container) {
            this.container = container;
        }
        if (!this.templateObj) {
            this.templateObj = templateObj;
        }
        this.refresh();
        var update = function (data) {
            data.appRoot = appRoot;
            data.name = this.name;
            data[this.activeLevel] = true;
            var template = templateObj.html();
            Mustache.parse(template);
            var html = Mustache.render(template, data);
            container.html(html);
        }.bind(this);

        // display a default view first.
        update({
            stat: {
                all: "...",
                trace: "...",
                debug: "...",
                info: "...",
                warn: "...",
                error: "...",
                fatal: "...",
            }
        });
        
        if (this.state == AjaxState.Ready) {
            update({
                stat: this.stat
            });
        } else if (this.state == AjaxState.Loading) {
            $(this).queue("update" + this.name, function (next) {
                update({
                    stat: this.stat
                });
                next();
            }.bind(this));
        } else if (this.state == AjaxState.Fail) {
            update({
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

function RepositoryDetail(name, filter) {
    // filter: level, beginAt, endAt, machineName, keyword, pageSize
    this.name = name;
    var conditions = [];
    for (key in filter) {
        var value = filter[key];
        if (value) {
            conditions.push("key=" + value);
        }
    }
    var query = conditions.join("&");
    this.url = appRoot + "api/repositories/" + name + "/entries?" + query;
    this.state = AjaxState.Init;
}

RepositoryDetail.prototype = {
    refresh: function() {
        var that = this;
        this.state = AjaxState.Loading;
        $.ajax({
            url: this.url
        }).done(function (data) {
            $(that).dequeue("update" + that.name);
        }).fail(function () {
            that.state = AjaxState.Fail;
        })
    },
    update: function (container, templateObj) {

    }
}

var AjaxState = {
    Init: "Init",
    Loading: "Loading",
    Ready: "Ready",
    Fail: "Fail"
};

var Level = {
    All: "All",
    Trace: "Trace",
    Debug: "Debug",
    Info: "Info",
    Warn: "Warn",
    Error: "Error",
    Fatal: "Fatal"
};