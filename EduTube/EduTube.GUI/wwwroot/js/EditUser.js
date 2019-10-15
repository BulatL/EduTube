$(function () {
   let dateOfBirth = $('#dateOfBirthHidden').val();

   $('#BirthDay').val(parseInt(dateOfBirth.split('-')[0]));
   $('#BirthMonth').val(parseInt(dateOfBirth.split('-')[1]) -1);
   $('#BirthYear').val(parseInt(dateOfBirth.split('-')[2]));

   $('#accountTabDiv').css('display', 'none');
   $("#profileImage").click(function (e) {
      $("#imageUpload").click();
   });

   $("#imageUpload").change(function () {
      fasterPreview(this);
   });

   $('#form').submit(function () {
      let userId = $('#userId').val();
      let channelName = $('#channelName').val();
      let email = $('#email').val();

      let birthDay = $('#BirthDay').val();
      let birthMonth = $('#BirthMonth').val();
      let birthYear = $('#BirthYear').val();
      $('#dateOfBirth').val(birthDay + "-" + (parseInt(birthMonth) + 1) + "." + birthYear);

      $.ajax({
         url: `/Users/ChannelNameEmailExist?channelName=${channelName}&email=${email}&userId=${userId}`,
         type: 'GET',
         dataType: 'json',
         contentType: "application/json",
         success: function (response) {
            console.log(response);
            if (response.channelNameExist == true) {
               $('#channelNameError').text("Channel name already taken");
               console.log()
            }
            if (response.emailExist == true) {
               $('#emailError').text("Email already taken");
            }
            if (response.emailExist == true || response.channelNameExist == true) {
               return false
            }
            else if (response.emailExist != true && response.channelNameExist != true)
               return true;
         },
         error: function (response) {
            return false;
         }
      });
   });
});

function fasterPreview(uploader) {
   if (uploader.files && uploader.files[0]) {
      $('#profileImage').attr('src',
         window.URL.createObjectURL(uploader.files[0]));
   }
}

function OpenAccountTab() {
   let firstname = $('#firstname').val();
   let lastname = $('#lastname').val();
   if (firstname == "") {
      $('#firstnameError').text("The First name field is required.")
   }
   if (lastname == "") {
      $('#lastnameError').text("The Last name field is required.")
   }
   if (firstname != "" && lastname != "") {
      $('#aboutTab').removeClass('tabsDivActiv');
      $('#accountTab').addClass('tabsDivActiv');
      $('#aboutTabDiv').css('display', 'none');
      $('#accountTabDiv').css('display', 'block');
      $('#firstnameError').text("");
      $('#lastnameError').text("");
   }
}

function OpenAboutTab() {
   $('#aboutTab').addClass('tabsDivActiv');
   $('#accountTab').removeClass('tabsDivActiv');
   $('#aboutTabDiv').css('display', 'block');
   $('#accountTabDiv').css('display', 'none');
}