function loadMoreVideos(page) {
    let search_query = $('#search_query').val();
    $.ajax({
        url: '/Search?search_query=' + search_query +'&search_etity=videos' + '&page=' + page,
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (response) {
            appendVideos(response.videos, page, response.totalPagesVideos);
        },
        error: function (response, jqXHR) {
            console.log(response)
        }
    });
}

function loadPreviouseVideos() {
    let search_query = $('#search_query').val();
    let previousePage = $('li[class*="paginationVideoLi active"]')[0].id.split('_')[1];
    let nextPage = parseInt(previousePage) - 1;
    $.ajax({
        url: '/Search?search_query=' + search_query + '&search_etity=videos' + '&page=' + nextPage,
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (response) {
            appendVideos(response.videos, nextPage, response.totalPagesVideos);
        },
        error: function (response, jqXHR) {
            console.log(response)
        }
    });
}

function loadNextVideos() {
    let search_query = $('#search_query').val();
    let previousePage = $('li[class*="paginationVideoLi active"]')[0].id.split('_')[1];
    let nextPage = parseInt(previousePage) + 1;
    $.ajax({
        url: '/Search?search_query=' + search_query + '&search_etity=videos' + '&page=' + nextPage,
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (response) {
            appendVideos(response.videos, nextPage, response.totalPagesVideos);
        },
        error: function (response, jqXHR) {
            console.log(response)
        }
    });
}

function loadMoreUsers(page) {
    let search_query = $('#search_query').val();
    $.ajax({
        url: '/Search?search_query=' + search_query + '&search_etity=users' + '&page=' + page,
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (response) {
            appendVideos(response.users, page, response.totalPagesUsers);
        },
        error: function (response, jqXHR) {
            console.log(response)
        }
    });
}

function loadPreviouseUsers() {
    let search_query = $('#search_query').val();
    let previousePage = $('li[class*="paginationUserLi active"]')[0].id.split('_')[1];
    let nextPage = parseInt(previousePage) - 1;
    $.ajax({
        url: '/Search?search_query=' + search_query + '&search_etity=users' + '&page=' + nextPage,
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (response) {
            appendUsers(response.Users, nextPage, response.totalPagesUsers);
        },
        error: function (response, jqXHR) {
            console.log(response)
        }
    });
}

function loadNextUsers() {
    let search_query = $('#search_query').val();
    let previousePage = $('li[class*="paginationUserLi active"]')[0].id.split('_')[1];
    let nextPage = parseInt(previousePage) + 1;
    $.ajax({
        url: '/Search?search_query=' + search_query + '&search_etity=users' + '&page=' + nextPage,
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (response) {
            appendUsers(response.Users, nextPage, response.totalPagesUsers);
        },
        error: function (response, jqXHR) {
            console.log(response)
        }
    });
}

function appendVideos(videos, currentPageNumber, totalPages) {
    let videosDiv = $('#videosDiv');
    let previousePage = $('li[class*="paginationVideoLi active"]');
    let videosContent = [];

    videosDiv.empty();

    for (var i = 0; i < videos.length; i++) {
        videosContent.push(`
            <div class="col-12">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">${videos[i].name}</h5>
                        <p class="card-text">${videos[i].description}</p>
                        <p class="card-text">${videos[i].hashtags}</p>
                        <p class="card-text">${videos[i].userChannelName}</p>
                        <a href="${videos[i].userId}" class="btn btn-success">${videos[i].userChannelName}</a>
                    </div>
                </div>
            </div>
        `);
    }
    previousePage.removeClass('active');
    $('#paginationVideo_' + currentPageNumber).addClass('active');

    if (totalPages == currentPageNumber)
        $('.paginationVideoNextLi').addClass('disabled');
    if (totalPages > currentPageNumber)
        $('.paginationVideoNextLi').removeClass('disabled');
    if (currentPageNumber > 1)
        $('.paginationVideoPreviouseLi').removeClass('disabled');
    else if (currentPageNumber == 1)
        $('.paginationVideoPreviouseLi').addClass('disabled');

    videosDiv.html(videosContent.join(''));
}

function appendUsers(users, currentPageNumber, totalPages) {
    let videosDiv = $('#usersDiv');
    let previousePage = $('li[class*="paginationUserLi active"]');
    let videosContent = [];

    videosDiv.empty();

    for (var i = 0; i < users.length; i++) {
        videosContent.push(`
            <div class="col-12">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">${users[i].channelName}</h5>
                        <p class="card-text">${users[i].firstname}</p>
                        <p class="card-text">${users[i].lastname}</p>
                        <p class="card-text">${users[i].channelDescription}</p>
                        <a href="${users[i].id}" class="btn btn-success">${users[i].channelName}</a>
                    </div>
                </div>
            </div>
        `);
    }
    previousePage.removeClass('active');

    if (totalPages == currentPageNumber)
        $('.paginationUserNextLi').addClass('disabled');
    if (totalPages > currentPageNumber)
        $('.paginationUserNextLi').removeClass('disabled');
    if (currentPageNumber > 1)
        $('.paginationUserPreviouseLi').removeClass('disabled');
    else if (currentPageNumber == 1)
        $('.paginationUserPreviouseLi').addClass('disabled');

    videosDiv.html(videosContent.join(''));

}