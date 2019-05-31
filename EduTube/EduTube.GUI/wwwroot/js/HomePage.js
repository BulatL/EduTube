$(function () {
    $.ajax({
        url: 'https://api.ipify.org/?format=json',
        type: 'GET',
        dataType: 'json',
        success: function (ipAddress) {
            $.ajax({
                url: '/Videos/RecommendedVideos/' + ipAddress.ip,
                type: 'GET',
                dataType: 'json',
                success: function (videos) {
                    populateReccomendedVideo(videos);
                },
                error: function (response, jqXHR) {
                }
            });
        },
        error: function (response, jqXHR) {
        }
    });
})
function populateReccomendedVideo(videos) {
    let firstRecommendedVideosRow = $("#firstRecommendedVideosRow");
    let firstRecommendedVideosContent = [];
    let secondRecommendedVideosRow = $("#secondRecommendedVideosRow");
    let secondRecommendedVideosContent = [];

    if (videos.firstRecommended.length > 0) {
        for (var i = 0; i < videos.firstRecommended.length; i++) {
            if (videos.firstRecommended[i].youtubeUrl != null) {
                firstRecommendedVideosContent.push(
                    `<div class="col-lg-4">
                        <h2><a href="#">${videos.firstRecommended[i].name}</a></h2>
                        <h2>${videos.firstRecommended[i].id}</h2>
                        <iframe id="videoFrame" width="300" height="200" src="${videos.firstRecommended[i].youtubeUrl}" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
                     </div>`
                );
            }
            else {
                firstRecommendedVideosContent.push(
                    `<div class="col-lg-4>"
                        <h2><a href="#">${videos.firstRecommended[i].name}</a></h2>
                        <h2>${videos.firstRecommended[i].id}</h2>
                        <video width="300" controls>
                            <source src="~/DR%c5%bdAVNI%20POSAO%20[HQ]%20-%20Ep.218%20%c4%8cekiranje%20(30.09.2013.).mp4" type="video/mp4">
                            Your browser does not support HTML5 video.
                        </video>
                     </div>`
                );
            }
        }
        firstRecommendedVideosRow.html(firstRecommendedVideosContent.join(''));
        $("#firstRecommendedHeading").text("Recommended because u watched video with " + videos.firstHashtag + " hashtag");
    }
    else {
        firstRecommendedVideosRow.remove();
        $("#firstRecommendedHeading").remove();
    }

    if (videos.secondRecommended.length > 0) {
        for (var i = 0; i < videos.secondRecommended.length; i++) {
            if (videos.secondRecommended[i].youtubeUrl != null) {
                secondRecommendedVideosContent.push(
                    `<div class="col-lg-4">
                        <h2><a href="#">${videos.secondRecommended[i].name}</a></h2>
                        <h2>${videos.secondRecommended[i].id}</h2>
                        <iframe id="videoFrame" width="300" height="200" src="${videos.secondRecommended[i].youtubeUrl}" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
                     </div>`
                );
            }
            else {
                secondRecommendedVideosContent.push(
                    `<div class="col-lg-4>"
                        <h2><a href="#">${videos.secondRecommended[i].name}</a></h2>
                        <h2>${videos.secondRecommended[i].id}</h2>
                        <video width="300" controls>
                            <source src="~/DR%c5%bdAVNI%20POSAO%20[HQ]%20-%20Ep.218%20%c4%8cekiranje%20(30.09.2013.).mp4" type="video/mp4">
                            Your browser does not support HTML5 video.
                        </video>
                     </div>`
                );
            }
        }
        secondRecommendedVideosRow.html(secondRecommendedVideosContent.join(''));
        $("#secondRecommendedHeading").text("Recommended because u watched video with " + videos.secondHashtag + " hashtag");
    }
    else {
        secondRecommendedVideosRow.remove();
        $("#secondRecommendedHeading").remove();
    }
}