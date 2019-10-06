// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
   var searchInput = document.getElementById("searchInput");
   searchInput.addEventListener("keyup", function (event) {
      if (event.keyCode === 13) {
         let search_query = searchInput.value.replace(/ /g, '-');
         event.preventDefault();
         window.location.href = "/Search?search_query=" + search_query;
      }
   });
   let userLoggedIn = $('#userLoggedIn').val();
   if (userLoggedIn === "true") {
      let notificationsDiv = $('#notificationsDiv');
      //get last 5 notifications for current user
      if (notificationsDiv.children().length === 1) {
         $.ajax({
            url: '/Notifications/GetLast5',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (data) {
               if (data.length > 0) {
                  for (var i = 0; i < data.length; i++) {
                     notificationsDiv.append(
                        `<div class="row colorWhite padding1">
                           <div class="navbarNotificationContent">
                              ${data[i].content}
                           </div>
                           <br/>
                           ${FormatDateString(data[i].dateCreatedOn)}
                        </div>
                        <div class="row breakLineWhite"></div>`
                     );
                  }

                  notificationsDiv.append(
                     `<div class="colorWhite text-center">
                        <a href="/Notifications"><h4 class="padding1">See all notifcations</h4></a>
                     </div>`
                  );

                  var lastId = data[0].id;
                  $('#lastNotificationId').val(lastId);
                  //Check for new notifications for current user
               }
               else {
                  notificationsDiv.append(`<div  class="colorWhite text-center"><h4 class="padding1">You don't have notifications</h4></div>`)
               }
            },
            error: function (response, jqXHR) {
            }
         });
         // Check if there is new notifications every 5 minuts
         window.setInterval(function () {
            GetNewNotifications();
         }, 300000);
      }
   }
})

function GetNewNotifications() {
   let notificationsDiv = $('#notificationsDiv');
   let lastId = $('#lastNotificationId').val();
   $.ajax({
      url: `/Notifications/GetNewNotifications?lastId=${lastId}`,
      type: 'GET',
      success: function (data) {
         if (data.length > 0) {
            data.reverse();
            for (var i = 0; i < data.length; i++) {
               notificationsDiv.prepend(
                  `<div class="row colorWhite padding1">
                     <div class="navbarNotificationContent">
                        ${data[i].content}
                     </div>
                     <br/>
                     ${FormatDateString(data[i].dateCreatedOn)}
                  </div>
                  <div class="row breakLineWhite"></div>`
               );
            }

            var lastId = data[data.length - 1].id;
            $('#lastNotificationId').val(lastId);

            let newNotificationsDiv = $('#newNotificationsDiv');
            newNotificationsDiv.empty();
            newNotificationsDiv.append(`<span>You have ${data.length} new notifications</span>`);
            newNotificationsDiv.fadeIn('slow');

            setTimeout(function () {
               $('#newNotificationsDiv').fadeOut("slow").empty();
            }, 5000);
         }
      }
   });

}
function openNotificationDropdown() {
   document.getElementById("notificationDropdown").classList.toggle("show");
}

function RedirectTo(redirectUrl) {
	window.location.replace(redirectUrl);
}


function FormatDateString(date) {
   let spliteDate = date.split("T")[0];
   let splitedTime = date.split("T")[1].split(".")[0];
   let time = splitedTime.split(":");
   let formatedDate = spliteDate + " " + time[0] + ":" + time[1];
   return formatedDate;
}

function FormatDate(myDate) {
   return myDate.getFullYear() + '-' + ('0' + (myDate.getMonth() + 1)).slice(-2) + '-' + ('0' + myDate.getDate()).slice(-2) + ' ' + myDate.getHours() + ':' + ('0' + (myDate.getMinutes())).slice(-2);
}