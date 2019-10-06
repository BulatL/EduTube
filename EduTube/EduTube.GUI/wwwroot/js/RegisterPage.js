$(function () {
   $("#profileImage").click(function (e) {
      $("#imageUpload").click();
   });
   $("#imageUpload").change(function () {
      fasterPreview(this);
   });

   $(".toggle-password").click(function () {

      $(this).toggleClass("fa-eye fa-eye-slash");
      var input = $($(this).attr("toggle"));
      if (input.attr("type") === "password") {
         input.attr("type", "text");
      } else {
         input.attr("type", "password");
      }
   });
   $('#form').submit(function () {
      let channelName = $('#channelName').val();
      let password = $('#password').val();
      let email = $('#email').val();
      let channelDescription = $('#channelDescription').val();
      let validationError = false;
      let upperCase = new RegExp('[A-Z]');
      let lowerCase = new RegExp('[a-z]');

      let channelNameEror = "";
      let emailError = "";
      let channelDescriptionError = "";
      let passwordError = "";


      let birthDay = $('#BirthDay').val();
      let birthMonth = $('#BirthMonth').val();
      let birthYear = $('#BirthYear').val();
      $('#dateOfBirth').val(birthDay + "-" + (parseInt(birthMonth) + 1) + "." + birthYear);

      if (channelName == "") {
         channelNameEror = 'The Channel name field is required.';
         validationError = true;
      }
      if (email == "") {
         emailError = 'The Email field is required.';
         validationError = true;
      }
      if (channelDescription == "") {
         channelDescriptionError = 'The Channel description field is required.';
         validationError = true;
      }

      if (password == "") {
         passwordError = 'The Password field is required. ';
         validationError = true;
      }
      if (password.length < 6) {
         passwordError += 'Passwords must be at least six characters. ';
         validationError = true;
      }
      if (!password.match(upperCase)) {
         passwordError += 'Passwords must contain at least 1 uppercase character. ';
         validationError = true;
      }
      if (!password.match(lowerCase)) {
         passwordError += 'Passwords must contain at least 1 non uppercase character. ';
         validationError = true;
      }

      console.log(passwordError);
      if (validationError == true) {
         $('#channelNameError').text(channelNameEror);
         $('#emailError').text(emailError);
         $('#channelDescriptionError').text(channelDescriptionError);
         $('#passwordError').text(passwordError);
         return false;
      }

      $.ajax({
         url: `/Users/ChannelNameEmailExist?channelName=${channelName}&email=${email}&userId=${0}`,
         type: 'GET',
         dataType: 'json',
         contentType: "application/json",
         async: false,
         success: function (response) {
            if (response.channelNameExist === true) {
               $('#channelNameError').text("Channel name already taken");
            }
            if (response.emailExist === true) {
               $('#emailError').text("Email already taken");
            }
            if (response.emailExist === true || response.channelNameExist === true) {
               return false
            }
            else if (response.emailExist !== true && response.channelNameExist !== true) {
               return true;
            }
         },
         error: function (response) {
            return false;
         }
      });
   });
   //$('#datepicker').datepicker();
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
   if (firstname === "") {
      $('#firstnameError').text("The First name field is required.")
   }
   if (lastname === "") {
      $('#lastnameError').text("The Last name field is required.")
   }
   if (firstname !== "" && lastname !== "") {
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