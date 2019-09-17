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
         let thumbnail = "";
         if (videos.firstRecommended[i].fileName == null) {
            thumbnail = videos.firstRecommended[i].thumbnail;
         }
         else {
            thumbnail = "/thumbnails/" + videos.firstRecommended[i].thumbnail;
         }
         firstRecommendedVideosContent.push(
            `<div class="col-lg-2">
               <a href="/Videos/${videos.firstRecommended[i].id}">
                  <img src="${thumbnail}" class="videoThumbnails"/>
                  <h6 class="text-white">${videos.firstRecommended[i].name}</h6>
               </a>
               <a href="/Users/${videos.firstRecommended[i].userChannelName.replace(/ /g, '-')}">
                  <h6 class="text-white">${videos.firstRecommended[i].userChannelName}</h6>
               </a>
               <h6>${videos.firstRecommended[i].dateCreatedOn}</h6>
            </div>`
         );
      }
      firstRecommendedVideosRow.html(firstRecommendedVideosContent.join(''));
      $("#firstRecommendedHeading").text("Recommended because u watched video with " + videos.firstTag + " tag");
   }
   else {
      firstRecommendedVideosRow.remove();
      $("#firstRecommendedHeading").remove();
   }

   if (videos.secondRecommended.length > 0) {
      for (var i = 0; i < videos.secondRecommended.length; i++) {
         let thumbnail = "";
         if (videos.secondRecommended[i].fileName == null) {
            thumbnail = videos.secondRecommended[i].thumbnail;
         }
         else {
            thumbnail = "/thumbnails/" + videos.secondRecommended[i].thumbnail;
         }
         secondRecommendedVideosContent.push(
            `<div class="col-lg-2">
               <a href="/Videos/${videos.secondRecommended[i].id}">
                  <img src="${thumbnail}" class="videoThumbnails"/>
                  <h6 class="text-white">${videos.secondRecommended[i].name}</h6>
               </a>
               <a href="/Users/${videos.secondRecommended[i].userChannelName.replace(/ /g, '-')}">
                  <h6 class="text-white">${videos.secondRecommended[i].userChannelName}</h6>
               </a>
               <h6>${videos.secondRecommended[i].dateCreatedOn}</h6>
            </div>`
         );
      }
      secondRecommendedVideosRow.html(secondRecommendedVideosContent.join(''));
      $("#secondRecommendedHeading").text("Recommended because u watched video with " + videos.secondTag + " tag");
   }
   else {
      secondRecommendedVideosRow.remove();
      $("#secondRecommendedHeading").remove();
   }
}