$(function () {

})
function OpenEditModal() {
   $('#editModal').modal('show');
   $('html, body').animate({
      scrollTop: $('#editModal').offset().top
   }, 1000);
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

function OpenMenu() {
   let displayed = $('#profileOptios').is(":visible");
   if (displayed)
      $('#profileOptios').css('display', 'none');
   else
      $('#profileOptios').css('display', 'block');
}