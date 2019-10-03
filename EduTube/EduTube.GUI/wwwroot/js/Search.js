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

function OpenTab(tabName) {
   $('.tabsDivActiv').removeClass('tabsDivActiv');

   if (tabName == 'videos') {
      $('#videosTab').addClass('tabsDivActiv');
      $('#videosDiv').show();
      $('#showMoreVideosDiv').show();
      $('#usersDiv').hide();
      $('#showMoreUsersDiv').hide();
   }
   else if (tabName == 'users') {
      $('#usersTab').addClass('tabsDivActiv');
      $('#usersDiv').show();
      $('#showMoreUsersDiv').show();
      $('#videosDiv').hide();
      $('#showMoreVideosDiv').hide();
   }
}

function ShowMoreVideos() {
   let search_query = $('#search_query').val();
   let videosCount = $('#videosCount').val();
   $.ajax({
      url: '/Search?search_query=' + search_query + '&search_etity=videos' + '&page=' + (parseInt(videosCount) / 10),
      type: 'GET',
      dataType: 'json',
      contentType: 'application/json',
      success: function (response) {
         console.log(response)
         let newVideoCount = parseInt(videosCount) + response.videos.length;
         $('#videosCount').val(newVideoCount);
         if (response.videos.length < 10)
            $('#showMoreVideosDiv').hide();

         PopulateVideos(response.videos);
      },
      error: function (response, jqXHR) {
         console.log(response)
      }
   });
}

function ShowMoreUsers() {
   let search_query = $('#search_query').val();
   let usersCount = $('#usersCount').val();
   $.ajax({
      url: '/Search?search_query=' + search_query + '&search_etity=users' + '&page=' + (parseInt(usersCount) / 10),
      type: 'GET',
      dataType: 'json',
      contentType: 'application/json',
      success: function (response) {
         console.log(videos)
         $('#usersCount').val(parseInt(usersCount) + response.users.length);
         if (response.users.length < 10)
            $('#showMoreUsersDiv').hide();

         PopulateUsers(response.users);
      },
      error: function (response, jqXHR) {
         console.log(response)
      }
   });
}

function PopulateVideos(videos) {
   let videosDiv = $('#videosDiv');
   for (var i = 0; i < videos.length; i++) {
      videosDiv.append(
         `<div class="videoDiv">
            <a href="/Videos/${videos[i].id}">
               <div class="videoThumbnailsDiv">
                  <img class="videoThumbnails" src="${videos[i].thumbnail}" />
               </div>
            </a>
            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
               <a href="/Videos/${videos[i].id}">
                  <p class="searchVideoName" title="${videos[i].name}">
                     ${videos[i].name}
                  </p>
               </a>
               <span class="searchVideoOwner">
                  <a href="/Users/${videos[i].userChannelName.replace(" ", "-")}">${videos[i].userChannelName}</a>     ${videos[i].dateCreatedOn}
               </span>
               <p class="searchTags">
                  Tags: ${videos[i].tags}
               </p>
            </div>
            <div class="col-lg-6 col-md-5 col-sm-12 col-xs-12">
               <div class="searchVideoDescription">
                  ${videos[i].description}
               </div>
            </div>
         </div>
         <div class="breakLineWhite"></div>`
      );
   }
}

function PopulateUsers(users) {
   let usersContainer = $('#usersContainer');
   for (var i = 0; i < videos.length; i++) {
      usersContainer.append(
         `<div class="display-inline-block userDiv">
            <a href="/Users/${users[i].channelName.replace(" ", "-")}">
               <div class="userProfileImageDiv">
                  <img src="~/profileImages/${users[i].profileImage}" />
               </div>
               <h4 class="overflow text-center">${users[i].channelName}</h4>
            </a>
         </div>`
      );
   }
}