//$("#send-button").click(function () {
//    debugger;
//    var name = $('#name').val();
//    var link = $('#link').val();
//    var description = $('#description').val();
//    var token = $('#token').val();
//    $.ajax({
//        url: '/Resources/SendRequest',
//        type: 'POST',
//        data: {
//            name: name,
//            link: link,
//            description: description,
//            token: token
//        },
//        dataType: 'json',
//        success: function (data) {
//        },
//        error: function (xhr) {
//            console.log(xhr.responseText);
//        }
//    });
//});
$(document).on("click", "#send-button", function () {
    debugger;
    var name = $('#name').val();
    var link = $('#link').val();
    var description = $('#description').val();
    var token = $('#token').val();
    $.ajax({
        url: '/Resources/SendRequest',
        type: 'POST',
        data: {
            name: name,
            link: link,
            description: description,
            token: token
        },
        dataType: 'json',
        success: function (data) {
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    });
});