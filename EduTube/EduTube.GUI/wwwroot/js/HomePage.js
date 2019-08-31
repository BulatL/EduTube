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

   /*
    <div class="col-lg-2">
         <a href="/Videos/@video.Id">
            <img src="~/thumbnails/@video.Thumbnail" width="160" height="180"/>
            <h6 class="text-white">@video.Name</h6>
         </a>
         <a href="/Users/@video.UserChannelName.Replace(" ", "-")">
            <h6 class="text-white">@video.UserChannelName</h6>
         </a>
         <h6>@video.DateCreatedOn</h6>
      </div>*/
   if (videos.firstRecommended.length > 0) {
      for (var i = 0; i < videos.firstRecommended.length; i++) {
         console.log(videos.firstRecommended[i]);
         firstRecommendedVideosContent.push(
            `<div class="col-lg-2">
               <a href="/Videos/${videos.firstRecommended[i].id}">
                  <img src="/thumbnails/${videos.firstRecommended[i].thumbnail}" width="160" height="180"/>
                  <h6 class="text-white">${videos.firstRecommended[i].name}</h6>
               </a>
               <a href="/Users/${videos.firstRecommended[i].userChannelName.replace(/ /g, '-')}">
                  <h6 class="text-white">${videos.firstRecommended[i].userChannelName}</h6>
               </a>
               <h6>${videos.firstRecommended[i].fateCreatedOn}</h6>
            </div>`
         );
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
         secondRecommendedVideosContent.push(
            `<div class="col-lg-2">
               <a href="/Videos/${videos.secondRecommended[i].id}">
                  <img src="/thumbnails/${videos.secondRecommended[i].thumbnail}" width="160" height="180"/>
                  <h6 class="text-white">${videos.secondRecommended[i].name}</h6>
               </a>
               <a href="/Users/${videos.secondRecommended[i].userChannelName.replace(/ /g, '-')}">
                  <h6 class="text-white">${videos.secondRecommended[i].userChannelName}</h6>
               </a>
               <h6>${videos.secondRecommended[i].fateCreatedOn}</h6>
            </div>`
         );
      }
      secondRecommendedVideosRow.html(secondRecommendedVideosContent.join(''));
      $("#secondRecommendedHeading").text("Recommended because u watched video with " + videos.secondHashtag + " hashtag");
   }
   else {
      secondRecommendedVideosRow.remove();
      $("#secondRecommendedHeading").remove();
   }
}