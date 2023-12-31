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
$(document).ready(function () {
    debugger;
    var params = new URLSearchParams(window.location.search);
    for (let [key, value] of params) {
        if (value === 'true') {
            makeFilterActive(key);
        }
    }
    if ($('.request').length = 0)
    debugger;
});
function makeFilterActive(name) {
    name = '#' + name + '-filter';
    $(name).addClass('active-filter');
}
function getFilterName(queryString) {
    const params = new URLSearchParams(queryString);
    for (let [key, value] of params) {
        if (value === 'true') {
            return key;
        }
    }
    return null;
}
$(document).on("click", "#nothing-filter", function () {
    applyFilter("nothing");
});
$(document).on("click", "#reset-filters", function () {
    resetFilters();
});

function applyFilter(filter) {
    debugger;
    var baseUrl = window.location.href.split('?')[0];
    var params = new URLSearchParams(window.location.search);
    if (params.has(filter)) params.delete(filter);
    else params.append(filter, "true");
    params.set("page", "1");
    var url = baseUrl + "?" + params.toString();
    window.location.href = url;
}
function resetFilters() {
    var baseUrl = window.location.href.split('?')[0];
    var params = new URLSearchParams();
    params.append("page", 1);
    params.append("watched", "true");
    params.append("nothing", "true");
    var url = baseUrl + '?' + params.toString();
    window.location.href = url;
}
$(document).on("click", "#reject-button", function () {
    debugger;
    var form = $(this).closest('form');
    var url = '/Resources/CancelRequest';
    var method = 'POST'
    var data = form.serialize();

    $.ajax({
        url: url,
        method: method,
        data: data,
        success: function (response) {
            form.addClass('rejected');
        },
        error: function (xhr) {
        }
    });
});
$(document).on("click", "#check-button", function () {
    var form = $(this).closest('form');
    var url = '/Resources/MakeRequestWatched';
    var method = 'POST'
    var data = form.serialize();

    $.ajax({
        url: url,
        method: method,
        data: data,
        success: function (response) {
            form.addClass('watched');
        },
        error: function (xhr) {
        }
    });
});