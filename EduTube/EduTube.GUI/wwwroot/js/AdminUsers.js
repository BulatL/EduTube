function Unblock(id) {
   $.ajax({
      url: `/AdminUsers/BlockUnblock?block=False&id=${id}`,
      type: 'GET',
      dataType: 'json',
      async: false,
      success: function (response) {
         console.log(response);
         $(`#unblock_${id}`).attr('hidden', true);
         $(`#block_${id}`).removeAttr('hidden');
      },
      error: function (response, jqXHR) {
         console.log(response);
         if (response.status == 200) {
            $(`#unblock_${id}`).attr('hidden', true);
            $(`#block_${id}`).removeAttr('hidden');
         }
      }
   });
}

function Block(id) {
   $.ajax({
      url: `/AdminUsers/BlockUnblock?block=True&id=${id}`,
      type: 'GET',
      dataType: 'json',
      async: false,
      success: function (response) {
         console.log(response);
         $(`#block_${id}`).attr('hidden', true);
         $(`#unblock_${id}`).removeAttr('hidden');
      },
      error: function (response, jqXHR) {
         console.log(response);
         if (response.status == 200) {
            $(`#block_${id}`).attr('hidden', true);
            $(`#unblock_${id}`).removeAttr('hidden');
         }
      }
   });
}

function Demote(id) {
   $.ajax({
      url: `/AdminUsers/PromoteDemote?promote=False&id=${id}`,
      type: 'GET',
      dataType: 'json',
      async: false,
      success: function (response) {
         console.log(response);
         $(`#demote_${id}`).attr('hidden', true);
         $(`#promote_${id}`).removeAttr('hidden');
      },
      error: function (response, jqXHR) {
         console.log(response);
         if (response.status == 200) {
            $(`#demote_${id}`).attr('hidden', true);
            $(`#promote_${id}`).removeAttr('hidden');
         }
      }
   });
}

function Promote(id) {
   $.ajax({
      url: `/AdminUsers/PromoteDemote?promote=True&id=${id}`,
      type: 'GET',
      dataType: 'json',
      async: false,
      success: function (response) {
         console.log(response);
         $(`#promote_${id}`).attr('hidden', true);
         $(`#demote_${id}`).removeAttr('hidden');
      },
      error: function (response, jqXHR) {
         console.log(response);
         if (response.status == 200) {
            $(`#promote_${id}`).attr('hidden', true);
            $(`#demote_${id}`).removeAttr('hidden');
         }
      }
   });
}

function ShowMore() {
   let usersCount = $('#usersCount').val();
   $.ajax({
      url: `/AdminUsers/ShowMore?skip=${usersCount}`,
      type: 'GET',
      dataType: 'json',
      async: false,
      success: function (response) {
         console.log(response);
         PopulateUsers(users);
      },
      error: function (response, jqXHR) {
         console.log(response);
      }
   });
}

function PopulateUsers(users) {
   let usersDiv = $('#usersDiv');
   for (var i = 0; i < users.length; i++) {
      usersDiv.append(
         `<div class="display-inline-block userDiv" id="user_@user.Id">
            <a href="/Users/@user.ChannelName.Replace(" ", "-")">
               <div class="userProfileImageDiv">
                  <img src="~/profileImages/@user.ProfileImage" />
               </div>
               <h5 class="overflow text-center channelName">@user.ChannelName</h5>
               <h5 class="overflow text-center userRole" id="userRole_@user.Id">@user.Role</h5>
            </a>
            <div class="buttonsDiv">
               <button class="greenBtn editBtn" onclick='RedirectTo("/Users/Edit/@user.Id")'>Edit</button>
               <button class="redBtn deleteBtn">Delete</button>
               @if (user.Blocked)
               {
                  <button class="redBtn promoteBtn" onclick='Unblock("@user.Id")' id="unblock_@user.Id">Unblock</button>
                  <button hidden class="redBtn promoteBtn" onclick='Block("@user.Id")' id="block_@user.Id">Block</button>
               }
               @if (!user.Blocked)
               {
                  <button class="redBtn promoteBtn" onclick='Block("@user.Id")' id="block_@user.Id">Block</button>
                  <button hidden class="redBtn promoteBtn" onclick='Unblock("@user.Id")' id="unblock_@user.Id">Unblock</button>
               }
               @if (user.Role.Equals("Admin"))
               {
                  <button class="redBtn promoteBtn" onclick='Demote("@user.Id")' id="demote_@user.Id">Demote</button>
                  <button hidden class="purpleBtn promoteBtn" onclick='Promote("@user.Id")' id="promote_@user.Id">Promote</button>
               }
               @if (user.Role.Equals("User"))
               {
                  <button class="purpleBtn promoteBtn" onclick='Promote("@user.Id")' id="promote_@user.Id">Promote</button>
                  <button hidden class="redBtn promoteBtn" onclick='Demote("@user.Id")' id="demote_@user.Id">Demote</button>
               }
            </div>
         </div>`
      );
   }
}