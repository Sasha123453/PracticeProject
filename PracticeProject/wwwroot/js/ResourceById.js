
var connection = new signalR.HubConnectionBuilder().withUrl("/commentHub").build();

connection.on("NewComment", function (comment) {
    debugger;
    var id = $('#resourceId').val();
    if (isLastPage && comment.resourceId == id) {
        $("#comments").append(`
        <div class="comment">
            <div class="comment-head">
                <h3>${x.nickname}</h3>
                <span>${x.createdAt}</span >
            </div>
            <div><span>${x.text}</span ></div >
        </div>
        `);
    }
});

connection.start().then(function () {
    console.log("SignalR connection established.");
}).catch(function (err) {
    console.error(err.toString());
});
var isLastPage = false;
var isLoading = false;
var page = 1;

$(window).scroll(function () {
    if ($(window).scrollTop() + $(window).height() > $(document).height() - 100) {
        if (!isLoading) {
            isLoading = true;
            loadMoreComments(page);
        }
    }
});

function loadMoreComments() {
    if (isLastPage) return;
    var urlParams = new URLSearchParams(window.location.search);
    var id = urlParams.get('Id');
    $.ajax({
        url: '/Resources/LoadMoreComments',
        type: 'POST',
        data: {
            resourceId: id,
            page: page + 1
        },
        dataType: 'json',
        success: function (data) {
            if (data.length > 0) {
                for (const x of data) {
                    $("#comments").append(`
                     <div class="comment">
                        <div class="comment-head">
                            <h3>${x.nickname}</h3>
                            <span>${x.createdAt}</span >
                        </div>
                        <div><span>${x.commentText}</span ></div >
                      </div>
                    `);
                }
                debugger;
                isLoading = false;
                page++;
            }
            else { isLastPage = true; }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
            isLoading = false;
        }
    });
}
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
           
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    });
});
