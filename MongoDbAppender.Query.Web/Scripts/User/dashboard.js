function loadRepos(container, repos) {
    var repoContainers = $(container).find(".repoContainer");
    $(repos).each(function (index, item) {
        var repoContainer = $(repoContainers[index]);
        loadRepo(item.name, repoContainer);
    });
}

var loadRepo = (function () {
    var repositories = {};

    return function (name, container) {
        var repo = repositories[name];
        if (!repo) {
            var repo = new Repository(name, {
                template: $("#repo"),
                container: container
            });
            repositories[name] = repo;
        }

        repo.refresh();
    }
})();
