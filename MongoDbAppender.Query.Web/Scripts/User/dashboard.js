function loadRepos(container, repos) {
    var repoContainers = $(container).find(".repoContainer");
    $(repos).each(function (index, item) {
        var repoContainer = $(repoContainers[index]);
        loadRepo(item.name, repoContainer);
    });
}

function loadRepo(name, container) {
    var repo = new Repository(name);
    repo.update(container, $("#repo"));
}
