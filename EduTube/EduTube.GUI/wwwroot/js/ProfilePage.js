$(function () {

})
function OpenEditModal() {
   $('#editModal').modal('show');
   $('html, body').animate({
      scrollTop: $('#editModal').offset().top
   }, 1000);
}

function DeleteUser(userId) {
   $("#deleteUserDialog").load(`/Users/GetDeleteDialog/${userId}`, function (responseTxt, statusTxt, xhr) {
      if (statusTxt == "error")
         console.log("error")
      else {
         $('#deleteUserDialog').modal('show');
      }
   });
}

function DeleteUserConfirm(id) {
   $.ajax({
      url: `/Users/Delete/${id}`,
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