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

	/*
	 
	 <div class="col-lg-2 videoDiv">
			<div style="width: 10rem;">
				<a href="/Videos/@video.Id">
					<div class="videoThumbnailsDiv">
						<img src="@video.Thumbnail" class="videoThumbnails" />
						<p class="bottom-right greyBackground videoDuration">@video.Duration</p>
					</div>
					<p class="text-white videoName bold" title="@video.Name">@video.Name</p>
				</a>
			</div>
			<a href="/Users/@video.UserChannelName.Replace(" ", "-")" class="display-block">
				<span class="text-white fontSize08">@video.UserChannelName</span>
			</a>
			<span class="fontSize08">@video.DateCreatedOn.ToString("yyyy-MM-dd hh:mm")</span>
		</div>
	 */
   console.log(videos.firstRecommended);
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
            `<div class="col-lg-2 videoDiv">
					<div style="width: 10rem;">
						<a href="/Videos/${videos.firstRecommended[i].id}">
							<div class="videoThumbnailsDiv">
								 <img src="${videos.firstRecommended[i].thumbnail}" class="videoThumbnails"/>
								<p class="bottom-right greyBackground videoDuration">${videos.firstRecommended[i].duration}</p>
							</div>
							<p class="text-white videoName bold" title="${videos.firstRecommended[i].name}">${videos.firstRecommended[i].name}</p>
						</a>
					</div>
               </a>
               <a href="/Users/${videos.firstRecommended[i].userChannelName.replace(/ /g, '-')}" class="display-block">
                  <span class="text-white fontSize08">${videos.firstRecommended[i].userChannelName}</span>
               </a>
               <span class="fontSize08">${FormatDateString(videos.firstRecommended[i].dateCreatedOn)}</span>
            </div>`
         );
		}
		$('#breakLine').show();
      firstRecommendedVideosRow.html(firstRecommendedVideosContent.join(''));
      $("#firstRecommendedHeading").text("Recommended because u watched video with " + videos.firstTag + " tag");
   }
   else {
      firstRecommendedVideosRow.remove();
      $("#firstRecommendedHeading").remove();
   }

   console.log(videos.secondRecommended);
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
				`<div class="col-lg-2 videoDiv">
					<div style="width: 10rem;">
						<a href="/Videos/${videos.secondRecommended[i].id}">
							<div class="videoThumbnailsDiv">
								 <img src="${videos.secondRecommended[i].thumbnail}" class="videoThumbnails"/>
								<p class="bottom-right greyBackground videoDuration">${videos.secondRecommended[i].duration}</p>
							</div>
							<p class="text-white videoName bold" title="${videos.secondRecommended[i].name}">${videos.secondRecommended[i].name}</p>
						</a>
					</div>
               </a>
               <a href="/Users/${videos.secondRecommended[i].userChannelName.replace(/ /g, '-')}" class="display-block">
                  <span class="text-white fontSize08">${videos.secondRecommended[i].userChannelName}</span>
               </a>
               <span class="fontSize08">${FormatDateString(videos.secondRecommended[i].dateCreatedOn)}</span>
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