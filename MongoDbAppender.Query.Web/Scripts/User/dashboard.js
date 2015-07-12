function loadRepos(container, repos) {
    $(repos).each(function (index, item) {
        var repo = new Repository(item.name);
        repo.update(container, $("#stat"));
    });
}

function loadStat(name, id) {
    var repository = new Repository(name);
    repository.load(function (data) {
        var container = $("#" + id);
    })
}