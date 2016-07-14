// define Repository class
var Repository = (function () {
    var clazz = function (name, options) {
        this.options = options || {};
        this.name = name;
        this.container = this.options.container || $(".repoContainer");
        this.template = $(this.options.template || $("#repo")).html();
        Mustache.parse(this.template);
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
                data.activeLevel = that.activeLevel;
                $(that).trigger('update', data);
            }).fail(function () {
                $(that).trigger('updateFailed');
            })
        }
    };
    // private methods of Repository.
    var private = {
        update: function (data) {
            data.appRoot = appRoot;
            data.name = this.name;
            data[this.activeLevel] = true;
            data.panelClass = levelToColor(this.activeLevel);
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
                        break;
                    }
                }
                if (activeLevel == "") {
                    activeLevel = Level.Warn;
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

    return clazz;
})();


var RepositoryDetail = (function () {
    var clazz = function (name, options) {
        this.name = name;
        this.options = options || {};
        this.container = this.options.container || $("#repos_detail");
        this.detailTemplate = $(this.options.detailTemplate || $("#repoDetail")).html();
        this.exceptionTemplate = $(this.options.exceptionTemplate || $("#exception")).html();
        // compile template for future usage.
        Mustache.parse(this.detailTemplate);
        Mustache.parse(this.exceptionTemplate);

        private.registerEvents.bind(this)();
    };

    clazz.prototype = {
        refresh: function (filter) {
            var that = this;
            // filter: level, from, to, machine_name, keyword, page_size, page_index
            var conditions = [];
            for (key in filter) {
                var value = filter[key];
                if (value) {
                    conditions.push(key + "=" + value);
                }
            }
            var query = conditions.join("&");
            this.url = appRoot + "api/repositories/" + this.name + "/entries?" + query;
            $(this).trigger('beforeUpdate');
            $.ajax({
                url: this.url
            }).done(function (data) {
                that.data = data;
                $(that).trigger('update', data);
            }).fail(function () {
                $(that).trigger('updateFailed');
            })
        }
    };

    var private = {
        update: function(data) {
            data.appRoot = appRoot;
            data.name = this.name;
            $(data.logEntries).each(function (index, log) {
                log.panelClass = levelToColor(log.level);
            });

            var html = Mustache.render(this.detailTemplate, data, this.exceptionTemplate);
            this.container.html(html);
        },
        registerEvents: function () {
            $(this).on('beforeUpdate', function () {
            }.bind(this)).on('update', function (e, data) {
                private.update.bind(this)(data);
            }.bind(this)).on('updateFailed', function () {
            }.bind(this));
        }
    };

    return clazz;
})();

function levelToColor(level) {
    var levelUpper = (level || "ALL").toUpperCase();
    switch (levelUpper) {
        case "FATAL":
            return 'panel-danger';
        case "ERROR":
            return 'panel-primary';
        case "WARN":
            return 'panel-warning';
        case "INFO":
            return 'panel-info';
        case "DEBUG":
            return 'panel-success';
        case "TRACE":
        case "ALL":
            return 'panel-default';
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