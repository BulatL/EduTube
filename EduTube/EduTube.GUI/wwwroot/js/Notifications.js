function ShowMore() {
   let notificationsCount = $('#notificationsCount').val();
   if (notificationsCount != undefined || notificationsCount >= 5) {
      $.ajax({
         url: `/Notifications/LoadMore?skip=${notificationsCount}`,
         type: 'GET',
         dataType: 'json',
         async: false,
         success: function (data) {
            if (data.length > 0) {
               let notifDiv = $('#notifDiv');
               for (var i = 0; i < data.length; i++) {
                  notifDiv.append(
                    `<div class="row breakLineWhite"></div>
                     <div class="row padding1">
                        <div class="col-lg-10 col-md-10 col-sm-12">
                           <div class="row">
                              <div class="col-lg-1 col-md-2 col-sm-2 marginAuto">
                                 <div class="notificationUserImageDiv">
                                    <img src="/profileImages/${data[i].userProfileImage}">
                                 </div>
                              </div>
                              <div class="col-lg-11 col-md-10 col-sm-10 breakWord marginAuto">
                                 ${data[i].content}  
                              </div>
                           </div>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-12 marginAuto">
                           <span class="float-right" id="dateSpan">
                              ${FormatDateString(data[i].dateCreatedOn)}
                           </span>
                        </div>
                     </div>`
                  );
               }
               if (data.length < 5) {
                  $('#showMore').hide();
               }
               $('#notificationsCount').val(parseInt(notificationsCount) + data.length);
            }
            else {
               $('#showMore').hide();}
         },
         error: function (response, jqXHR) {
         }
      });
   }
}

/*function FormatDate(unformatedDate) {
   var date = new Date(unformatedDate);
   return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();

}*/