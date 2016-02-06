// define Repository class
var Repository = (function () {
    // private methods of Repository.
    var private = {
        update: function (data) {
            data.appRoot = appRoot;
            data.name = this.name;
            data[this.activeLevel] = true;
            Mustache.parse(this.template);
            var html = Mustache.render(this.template, data);
            this.container.html(html);
        },
        registerEvents: function () {
            // detect hash change and refresh panel.
            var detectActiveLevel = function () {
                var levelStr = $.url(window.location.href).fparam('level');
                var levelStr = levelStr ? levelStr.toLowerCase() : "";
                var activeLevel = "";
                for (var level in Level) {
                    if (level.toLowerCase() == levelStr) {
                        activeLevel = level;
                        break;;
                    }
                }
                return activeLevel;
            };
            this.activeLevel = detectActiveLevel();
            var that = this;
            $(window).on("hashchange", function () {
                // listen to the hash change event.
                // refresh panel when level changes.
                var activeLevel = detectActiveLevel();
                if (that.activeLevel != activeLevel) {
                    that.activeLevel = activeLevel;
                    that.refresh();
                }
            });

            // Ajax update event
            $(this).on('beforeUpdate', function () {
                // triggers before ajax request is sent
                private.update.bind(this)({
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
            }.bind(this)).on('update', function (e, data) {
                // triggers after ajax returned sucessfully.
                private.update.bind(this)({
                    stat: data.stat
                });
                $(this).trigger('afterUpdate');
            }.bind(this)).on('updateFailed', function () {
                // triggers when ajax failed.
                private.update.bind(this)({
                    stat: {
                        all: "X",
                        trace: "X",
                        debug: "X",
                        info: "X",
                        warn: "X",
                        error: "X",
                        fatal: "X",
                    }
                });
            }.bind(this));
        }
    };

    var clazz = function (name, options) {
        this.options = options || {};
        this.name = name;
        this.container = this.options.container || $(".repoContainer");
        this.template = $(this.options.template || $("#repo")).html();
        private.registerEvents.bind(this)();
    };

    clazz.prototype = {
        refresh: function (filter) {
            // TODO: resolve filter
            var url = appRoot + "api/repositories/";
            var that = this;
            $(this).trigger('beforeUpdate');
            $.ajax({
                url: url + this.name + (this.statMins ? ("?statMins=" + this.statMins) : "")
            }).done(function (data) {
                that.stat = data.stat;
                $(that).trigger('update', data);
            }).fail(function () {
                $(that).trigger('updateFailed');
            })
        }
    };

    return clazz;
})();


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

var Level = {
    All: "All",
    Trace: "Trace",
    Debug: "Debug",
    Info: "Info",
    Warn: "Warn",
    Error: "Error",
    Fatal: "Fatal"
};