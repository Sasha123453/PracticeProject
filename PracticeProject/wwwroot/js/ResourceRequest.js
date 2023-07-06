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
            if (data.needsToBeAdded) {
                $("#comments").append(`
                <div class="comment">
                    <span>${data.nickname}</span>
                    <span>${text}</span>
                </div
                `);
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    });
});