// load all the repository stats.
function loadRepos(container, repos) {
    var repoContainers = $(container).find(".repoContainer");
    $(repos).each(function (index, item) {
        var repoContainer = $(repoContainers[index]);
        loadRepo(item.name, repoContainer);
    });
}

// load repository stats.
// maintains a singleton for each repository.
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

// load repository detail.
// maintains a singleton for each repository.
var loadRepoDetail = (function () {
    var repositories = {};

    return function (name, container) {
        var repo = repositories[name];
        if (!repo) {
            var repo = new RepositoryDetail(name, {
                detailTemplate: $("#repoDetail"),
                exceptionTemplate: $("#exception"),
                container: container
            });
            repositories[name] = repo;
        }

        repo.refresh();
    };
})();