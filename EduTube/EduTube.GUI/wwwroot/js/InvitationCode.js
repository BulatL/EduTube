function CheckInvitationCode() {
   let videoId = $('#videoId').val();
   console.log(videoId);
   let invitationCode = $('#invatationCode').val();
   if (invitationCode == "") {
      $('#invitationCodeError').text('Invitation code cant be empty');
      return;
   }
   $.ajax({
      url: `/Videos/CheckInvitationCode?videoId=${videoId}&invitationCode=${invitationCode}`,
      type: 'GET',
      dataType: 'json',
      success: function (response) {
         console.log(response);
         if (response == true) {
            window.location.replace(`/Videos/${videoId}`);
         }
         else
            $('#invitationCodeError').text('Wrong invatation code');
      },
      error: function (response, jqXHR) {
      }
   });
}