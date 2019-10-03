"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/Chats/Index").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (userId, channelName, profileImage, message) {
   console.log("ReceiveMessage");
   let msg_history = $('#msg_history');

   if (message == "joined") {
      msg_history.append(
         `<h5 class="messageInfo">${userChannelName} joined chat</h5>`
      );
      return;
   }
   let current_user_id = $('#current_user_id').val();
   let current_chat = $('#current_chat').val();
   if (current_user_id == userId) {

      msg_history.append(
         `<div class="msg outgoing_msg">
               <div class="sent_msg">
                  <p>
                    ${message}
                  </p>
                  <span class="time_date">${new Date}</span>
               </div>
            </div>`
      );
   }
   else {
      msg_history.append(
         `<div class="msg incoming_msg">
            <div class="incoming_msg_img"> 
               <img src="/profileImages/${profileImage}"> 
            </div>
            <div class="received_msg">
               <div class="received_withd_msg">
                  <span class="sender_name" id="sender_name_${current_chat}">${channelName}</span>
                  <p>
                     ${message}
                  </p>
                  <span class="time_date">${new Date}</span>
               </div>
            </div>
         </div>`
      );
   }
   $('#msg_history ').animate({
      scrollTop: $('#msg_history .msg:last-child').position().top
   }, 'slow');
});

connection.start().then(function () {
   document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
   return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
   let message = $('#message_input').val();
   let chat_name = $('#chatName').text();
   let current_chat = $('#current_chat').val();

   if (current_chat == undefined || current_chat == "")
      return;

   console.log("sending message");
   connection.invoke("SendMessage", message, current_chat, chat_name).catch(function (err) {
      return console.error(err.toString());
   });
   $('#message_input').val("")
   event.preventDefault();
});

function JoinChat(id) {
   console.log("joinChat");
   let chatName = $(`#chat_name_${id}`).text();
   let previouseChat = $('#current_chat').val();

   if (previouseChat == undefined || previouseChat != "") {
      LeavRoom(previouseChat);
   }

   $.ajax({
      url: `/Chats/GetMessages/${id}`,
      type: 'GET',
      success: function (response) {
         PopulateMessages(response);
         $('#current_chat').val(id);
         $('#chatName').html(chatName);
         //current user join chat room
         connection.invoke("JoinRoom", chatName).catch(function (err) {
            return console.error(err.toString());
         });

         $('#msg_history ').animate({
            scrollTop: $('#msg_history .msg:last-child').position().top
         }, 'slow');
      }
   });
}

function PopulateMessages(messages) {
   let msg_history = $('#msg_history');
   let current_user_id = $('#current_user_id').val();

   msg_history.empty();

   if (messages.length == 0) {
      msg_history.append('<h5 class="text-center">No previouse messages</h5>');
      return;
   }
   for (var i = 0; i < messages.length; i++) {
      if (messages[i].userId == current_user_id) {
         PopulateOutgoingMessage(messages[i]);
      }
      else {
         PopulateIncomingMessage(messages[i]);
      }
   }
}

function PopulateOutgoingMessage(message) {
   let msg_history = $('#msg_history');
   msg_history.append(
      `<div class="msg outgoing_msg">
               <div class="sent_msg">
                  <p>
                    ${message.message}
                  </p>
                  <span class="time_date">${message.dateCreatedOn}</span>
               </div>
            </div>`
   );
}

function PopulateIncomingMessage(message) {
   let msg_history = $('#msg_history');
   msg_history.append(
      `<div class="msg incoming_msg">
         <div class="incoming_msg_img"> 
            <img src="/profileImages/${message.user.profileImage}"> 
         </div>
         <div class="received_msg">
            <div class="received_withd_msg">
               <span class="sender_name" id="sender_name_${message.chatId}">${message.user.channelName}</span>
               <p>
                  ${message.message}
               </p>
               <span class="time_date">${message.dateCreatedOn}</span>
            </div>
         </div>
      </div>`
   );
}

function LeavRoom(previouseChat) {
   console.log("Leav room");
   let previouseChatName = $(`#chat_name_${previouseChat}`).text();
   connection.invoke("LeaveRoom", previouseChatName).catch(function (err) {
      return console.error(err.toString());
   });
}

function OpenModal(id, name) {
   $('#modalDialogHeader').text(`Are you sure you want to delete: ${name} chat?`);
   $('#modalDialogChat').val(id);
   $('#modalDialog').modal('show');
}

function DeleteChat() {
   let id = $('#modalDialogChat').val();
   $.ajax({
      url: `/Chats/Delete/${id}`,
      type: 'Delete',
      success: function (response) {
         $(`#chat_${id}`).remove();
         $('#modalDialog').modal('hide');
      },
      error: function (data, xhr) {
         console.log(data);
      }
   });
}