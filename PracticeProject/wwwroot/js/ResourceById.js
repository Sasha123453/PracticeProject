$(window).scroll(function () {

    if ($(window).scrollTop() + $(window).height() == $(document).height()) {
        var page = parseInt($('#page').val()) + 1;
        var urlParams = new URLSearchParams(window.location.search);
        var id = urlParams.get('Id');
        $.ajax({
            url: '/Resources/LoadMoreComments',
            type: 'POST',
            data: {
                resourceId: id,
                page: page
            },
            dataType: 'json',
            success: function (data) {
                debugger;
                for (const x of data) {
                    $("#comments").append(`
                <div class="comment">
                    <span>${x.nickname}</span>
                    <span>${x.commentText}</span>
                </div
                `);
                }
                $('#page').val(parseInt(page) + 1);
            },
            error: function (xhr) {
                console.log(xhr.responseText);
            }
        });
    }
});
$("#comment-button").click(function () {
    var urlParams = new URLSearchParams(window.location.search);
    var id = urlParams.get('Id');
    var text = $('#comment-text').val();
    var page = $('#page').val();
    $.ajax({
        url: '/Resources/SendComment',
        type: 'GET',
        data: {
            text: text,
            id: id,
            page: page
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
