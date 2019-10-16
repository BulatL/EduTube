$(function () {
   let visibility = $('#visibilitySelect').val(); 
   if (visibility == 2) {
      $('#InvitationCodeDiv').show();
   }

   $("#customThumbnail").on('change',function () {
      console.log("kliknuo");
      FasterPreview(this);
   });
   let oldThumbnail = $('#oldThumbnail').val();
   if (oldThumbnail != undefined && oldThumbnail != '') {
      $('#thumbnail').attr("src", oldThumbnail);
   }
   //create invitation code
   $('#visibilitySelect').on('change', function () {
      if (this.value == 2) {
         let code = $('#InvitationCode').val();
         console.log(code);
         if (code == undefined || code == '') {
            var result = '';
            var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
            var charactersLength = characters.length;
            for (var i = 0; i < 18; i++) {
               result += characters.charAt(Math.floor(Math.random() * charactersLength));
            }
            $('#InvitationCodeHidden').val(result);
            $('#InvitationCode').val(result);
         }
         $('#InvitationCodeDiv').show();
      }
      else 
         $('#InvitationCodeDiv').hide();
   });
   //get all tags 
   $.ajax({
      url: '/Tags/GetAll',
      type: 'GET',
      dataType: 'json',
      async: false,
      success: function (data) {
         $('#tagsInput').tagsInput({
            'delimiter': [',', ';'],
            'autocomplete': {
               source: data
            }
         });
      },
      error: function (response, jqXHR) {
      }
   });
   //preview img of youtube video
   $("#youtubeUrl").on("change paste keyup", function () {
      let youtubeId = GetYoutubeId(this.value);
      if (this.value != '') {
         if (youtubeId != 'error') {
            $('#thumbnail').attr('src', `https://img.youtube.com/vi/${youtubeId}/0.jpg`);
            $('#youtubeUrlError').text('');
         }
         else {
            $('#youtubeUrlError').text('Youtube url is not valid');
            $('#thumbnail').attr('src', ``);
         }
      }
      else {
         $('#youtubeUrlError').text('');
      }
   });

   $('#form').submit(function () {
      let video = $('#videoFile').val();
      let youtubeUrl = $('#youtubeUrl').val();

      $("#modalDialog").modal("show");
      let videoName = $("#videoName").val();
      $("modalHeader").text(videoName);

      if (video == null || video == '') {
         let id = GetYoutubeId(youtubeUrl)
         if (id == 'error') {
            $('#youtubeUrlError').text('Youtube url is not valid');
            return false;
         }
         $.ajax({
            url: 'https://www.googleapis.com/youtube/v3/videos?id=' + id + '&key=AIzaSyDYwPzLevXauI-kTSVXTLroLyHEONuF9Rw&part=snippet,contentDetails',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (data) {
               if (data.items.length > 0) {
                  console.log(data.items[0])
                  let duration = data.items[0].contentDetails.duration;
                  //let convertedDuration = convert_time(duration);
                  console.log(duration);
                  $('#videoDuration').val(duration);

                  return true;
               }
            },
            error: function (response, jqXHR) {
            }
         });
         $('#videoDuration').val();
         $('#youtubeId').val(id);
      }
      else
         return true;

      //return true; // return false to cancel form action
   });
})

function GetYoutubeId(url) {
   var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*/;
   var match = url.match(regExp);

   if (match && match[2].length == 11) {
      return match[2];
   } else {
      return 'error';
   }
}

function convert_time(duration) {
   var a = duration.match(/\d+/g);

   if (duration.indexOf('M') >= 0 && duration.indexOf('H') == -1 && duration.indexOf('S') == -1) {
      a = [0, a[0], 0];
   }

   if (duration.indexOf('H') >= 0 && duration.indexOf('M') == -1) {
      a = [a[0], 0, a[1]];
   }
   if (duration.indexOf('H') >= 0 && duration.indexOf('M') == -1 && duration.indexOf('S') == -1) {
      a = [a[0], 0, 0];
   }

   duration = 0;

   if (a.length == 3) {
      duration = duration + parseInt(a[0]) * 3600;
      duration = duration + parseInt(a[1]) * 60;
      duration = duration + parseInt(a[2]);
   }

   if (a.length == 2) {
      duration = duration + parseInt(a[0]) * 60;
      duration = duration + parseInt(a[1]);
   }

   if (a.length == 1) {
      duration = duration + parseInt(a[0]);
   }
   var h = Math.floor(duration / 3600);
   var m = Math.floor(duration % 3600 / 60);
   var s = Math.floor(duration % 3600 % 60);
   return ((h > 0 ? h + ":" + (m < 10 ? "0" : "") : "") + m + ":" + (s < 10 ? "0" : "") + s);
}

function Redirect(redirectUrl) {
   window.location.replace(redirectUrl);
}

function FasterPreview(uploader) {
   console.log("usao");
   console.log(uploader);
   if (uploader.files && uploader.files[0]) {
      console.log(uploader.files[0]);
      $('#thumbnail').attr('src',
         window.URL.createObjectURL(uploader.files[0]));
   }
}
