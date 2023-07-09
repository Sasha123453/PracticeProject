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
    /*var url = baseUrl + "?" + "page=1&" + filter + "=true";*/
    var params = new URLSearchParams();
    params.set(filter, "true");
    params.set("page", "1");
    var url = baseUrl + "?" + params.toString();
    window.location.href = url;
}
function resetFilters() {
    var baseUrl = window.location.href.split('?')[0];
    var url = baseUrl + "?" + "page=1";
    window.location.href = url;
}
$(document).on("click", "#reject-button", function () {
    debugger;
    var form = $(this).closest('form');
    var url = '/Resources/CancelRequest';
    var method = 'POST'
    var data = form.serialize();

    // Отправляем Ajax-запрос
    $.ajax({
        url: url,
        method: method,
        data: data,
        success: function (response) {
            form.classList.add('rejected');
        },
        error: function (xhr) {
            // Обработка ошибки
        }
    });
});
$(document).on("click", "#check-button", function () {
    var form = $(this).closest('form');
    var url = '/Resources/MakeRequestWatched';
    var method = 'POST'
    var data = form.serialize();

    // Отправляем Ajax-запрос
    $.ajax({
        url: url,
        method: method,
        data: data,
        success: function (response) {
            form.classList.add('watched');
        },
        error: function (xhr) {
            // Обработка ошибки
        }
    });
});