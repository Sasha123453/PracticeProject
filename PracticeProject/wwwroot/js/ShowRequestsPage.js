$(document).on("click", "#watched-filter", function () {
    applyFilter("watched");
});

$(document).on("click", "#completed-filter", function () {
    applyFilter("completed");
});

$(document).on("click", "#rejected-filter", function () {
    debugger;
    applyFilter("rejected");
});

$(document).on("click", "#nothing-filter", function () {
    applyFilter("nothing");
});
$(document).on("click", "#reset-filters", function () {
    resetFilters();
});

function applyFilter(filter) {
    debugger;
    var baseUrl = window.location.href.split('?')[0];
    var url = baseUrl + "?" + "page=1" + filter + "=true";
    window.location.href = url;
}
function resetFilters() {
    var baseUrl = window.location.href.split('?')[0];
    var url = baseUrl + "?" + "page=1";
    window.location.href = url;
}
