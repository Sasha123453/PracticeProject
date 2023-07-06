$("send-button").click(function () {
    var name = $('#name').val();
    var link = $('#link').val();
    var description = $('#description').val();
    $.ajax({
        url: '/Resources/SendRequest',
        type: 'GET',
        data: {
            name: name,
            link: link,
            description: description
        },
        dataType: 'json',
        success: function (data) {
            debugger;
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    });
});