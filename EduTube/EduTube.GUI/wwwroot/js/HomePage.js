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
});

function populateReccomendedVideo(videos) {
   let firstRecommendedVideosRow = $("#firstRecommendedVideosRow");
   let firstRecommendedVideosContent = [];
   let secondRecommendedVideosRow = $("#secondRecommendedVideosRow");
   let secondRecommendedVideosContent = [];

   if (videos.firstRecommended.length > 0) {
      for (let i = 0; i < videos.firstRecommended.length; i++) {
         let thumbnail = "";
         if (videos.firstRecommended[i].fileName === null) {
            thumbnail = videos.firstRecommended[i].thumbnail;
         }
         else {
            thumbnail = "/thumbnails/" + videos.firstRecommended[i].thumbnail;
         }
         firstRecommendedVideosContent.push(
            `<div class="col-lg-2">
               <a href="/Videos/${videos.firstRecommended[i].id}">
                  <img src="${thumbnail}" class="videoThumbnails"/>
                  <p class="text-white videoName">${videos.firstRecommended[i].name}</p>
               </a>
               <a href="/Users/${videos.firstRecommended[i].userChannelName.replace(/ /g, '-')}">
                  <p class="text-white fontSize08">${videos.firstRecommended[i].userChannelName}</p>
               </a>
               <p class="fontSize08">${videos.firstRecommended[i].dateCreatedOn}</p>
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
      for (let i = 0; i < videos.secondRecommended.length; i++) {
         let thumbnail = "";
         if (videos.secondRecommended[i].fileName === null) {
            thumbnail = videos.secondRecommended[i].thumbnail;
         }
         else {
            thumbnail = "/thumbnails/" + videos.secondRecommended[i].thumbnail;
         }
         secondRecommendedVideosContent.push(
            `<div class="col-lg-2">
               <a href="/Videos/${videos.secondRecommended[i].id}">
                  <img src="${thumbnail}" class="videoThumbnails"/>
                  <p class="text-white videoName">${videos.secondRecommended[i].name}</p>
               </a>
               <a href="/Users/${videos.secondRecommended[i].userChannelName.replace(/ /g, '-')}">
                  <p class="text-white fontSize08">${videos.secondRecommended[i].userChannelName}</p>
               </a>
               <p class="fontSize08">${videos.secondRecommended[i].dateCreatedOn}</p>
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