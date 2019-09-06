﻿$(function () {
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
      let password = $('#password').val();
      let email = $('#email').val();
      let channelDescription = $('#channelDescription').val();

      if (channelName == "")
         $('#channelNameError').text('The Channel name field is required.')
      if (password == "")
         $('#passwordError').text('The Password field is required.')
      if (email == "")
         $('#emailError').text('The Email field is required.')
      if (channelDescription == "")
         $('#channelDescriptionError').text('The Channel description field is required.')

      if (channelName != "" && password != "" && email != "" && channelDescription != "") {
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
      }
      else {
         return false;
      }
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
