$(function () {

})
function OpenEditModal() {
   $('#editModal').modal('show');
   $('html, body').animate({
      scrollTop: $('#editModal').offset().top
   }, 1000);
}
function DeleteProfile(userId) {
   var r = confirm('Are u sure u want to your account');
   if (r == true) {
      $.ajax({
         url: `/Users/Delete/${userId}`,
         type: 'DELETE',
         dataType: 'json',
         success: function (response) {
            console.log(response);
         },
         error: function (data, xhr) {
            if (data.status == 200)
               window.location.replace(`/Home`);

            else
               alert('Delete operation failed');
         }
      });
   }
}

function OpenTab(tabName) {
   $('.tabsDivActiv').removeClass('tabsDivActiv');

   if (tabName == 'videos') {
      $('#videosTab').addClass('tabsDivActiv');
      $('#videosDiv').show();
      $('#subscribersDiv').hide();
      $('#subscribedOnDiv').hide();
   }
   else if (tabName == 'subscribers') {
      $('#subscribersTab').addClass('tabsDivActiv');
      $('#subscribersDiv').show();
      $('#videosDiv').hide();
      $('#subscribedOnDiv').hide();

   }
   else {
      $('#subscribedOnTab').addClass('tabsDivActiv');
      $('#subscribedOnDiv').show();
      $('#videosDiv').hide();
      $('#subscribersDiv').hide();

   }
}