$(function () {
	let videoVisibility = $('#videoVisibility').val();
	let allowAccess = $('#allowAccess').val();
	if (videoVisibility === 'Invitation' && allowAccess === 'False') {
		$('#firstDiv').hide();
		$('#inviteCodeDialog').modal({
			show: true
		});
	}
	else {
		SetViewsAndReactions();
	}
});

function ShowMoreComments() {
   let lastCommentId = $('#lastCommentId').val();
   if (lastCommentId === undefined)
      return;

   let videoId = $('#videoId').val();
   $.ajax({
      url: `/Comments/GetByVideo?videoId=${videoId}&lastCommentId=${lastCommentId}`,
      type: 'GET',
      dataType: 'json',
      success: function (response) {
         if (response.length > 0) {
            PopulateComments(response);
            $('#lastCommentId').val(response[response.length - 1].id);
            //response.reverse();
            if (response.length < 5) {
               $('#showMoreComments').hide();
            }
            else {
               $('#commentsCount').val(response.length);
            }
         }
         else {
            $('#showMoreComments').hide();
         }
      },
      error: function (response, jqXHR) {
      }
   });
}

function PopulateComments(comments) {
   let currentUserId = $('#currentUserId').val();
   let commentsDiv = $('#commentsDiv');
   let commentsContent = [];
   for (var i = 0; i < comments.length; i++) {
      let likeCount = 0;
      let dislikeCount = 0;
      let likeResizeCommentEmoji = '';
      let likeResizeCommentEmojiCount = '';
      let dislikeResizeCommentEmoji = '';
      let dislikeResizeCommentEmojiCount = '';

      for (var j = 0; j < comments[i].reactions.length; j++) {
         if (comments[i].reactions[j].emojiId === 1) {
            likeCount++;
            if (comments[i].reactions[j].userId === currentUserId) {
               likeResizeCommentEmoji = 'resizeCommentEmoji';
               likeResizeCommentEmojiCount = 'resizeCommentEmojiCount';
            }
         }
         else {
            dislikeCount++;
            if (comments[i].reactions[j].userId === currentUserId) {
               dislikeResizeCommentEmoji = 'resizeCommentEmoji';
               dislikeResizeCommentEmojiCount = 'resizeCommentEmojiCount';
            }
         }
      }
      commentsContent.push(
         `<div class="col-12" id="comment_${comments[i].id}">
            <div class="row">
               <div class="col-1">
                  <a href="/Users/${comments[i].user.channelName.replace(' ', '-')}">
                     <div class="commentImageDiv">
                        <img src="/profileImages/${comments[i].user.profileImage}" />
                     </div>
                  </a>
               </div>
               <div class="col-11">
                  <div class="row">
                     <div class="col-8">
                        <span><a href="/Users/${comments[i].user.channelName.replace(' ', '-')}"><strong>   ${comments[i].user.channelName}</strong></a> ${FormatDateString(comments[i].dateCreatedOn)}</span>
                     </div>`);

      if (comments[i].userId === currentUserId) {
         commentsContent.push(
                     `<div class="col-4" id="mainCommentOptionsDiv_${comments[i].id}">
                        <button class="dotBtn" onclick="showCommentOptionDiv(${comments[i].id})">
                           <div class="dotMenu float-right">
                           </div>
                        </button>
                        <div class="commentsOptionsDiv float-right" style="display: none;" id="commentOptionsDiv_${comments[i].id}">
                           <div class="hoverGray"><button class="transparentBtn" onclick="MakeCommentEditable(${comments[i].id})">Edit <i class='fas fa-edit'></i></button></div>
                           <div class="hoverGray"><button class="transparentBtn" onclick="DeleteComment(${comments[i].id})">Delete <i class='far fa-trash-alt'></i></button></div>
                        </div>
                     </div>`);
      }


      commentsContent.push(
                  `<div class="col-12" id="commentContentDiv_${comments[i].id}">
                     <span id="commentContentSpan_${comments[i].id}">${comments[i].content}</span>
                  </div>
                  </div>
                  <div class="col-12 inline-flex noPadding">
                     <div>
                        <button class="commentEmojiBtn" onclick="Reaction(1, null, ${comments[i].id})">
                           <img src="/images/likeEmoji.png" class="${likeResizeCommentEmoji}" id="commentEmoji1_${comments[i].id}" class="" />
                           <h6 class="whiteColor moveLeft ${likeResizeCommentEmojiCount}" id="emojiCount1Comment_${comments[i].id}">${likeCount}</h6>
                        </button>
                     </div>
                     <div>
                        <button class="commentEmojiBtn" onclick="Reaction(2, null, ${comments[i].id})">
                           <img src="/images/dislikeEmoji.png" class="${dislikeResizeCommentEmoji}" id="commentEmoji2_${comments[i].id}" class="" />
                           <h6 class="whiteColor moveLeft ${dislikeResizeCommentEmojiCount}" id="emojiCount2Comment_${comments[i].id}">${dislikeCount}</h6>
                        </button>
                     </div>
                  </div>`);

      if (comments[i].userId === currentUserId) {
         commentsContent.push(
                  `<div class="float-right displayNone" id="editCommentBtnDiv_${comments[i].id}">
                     <button class="grayBtn" id="cancelComment" onclick="CancelCommentEdit(${comments[i].id})">Cancel</button>
                     <button class="orangeBtn" id="saveComment" onclick="EditComment(${comments[i].id})">Save</button>
                  </div>`
         );
      }
      commentsContent.push(
            `</div>
            </div>
         </div>
         <div class="col-12 comment_${comments[i].id}" id="breakAfterOwner">
            <div class="breakLine"></div>
         </div>`
      );
   }
   commentsDiv.append(commentsContent.join(''));
   $('html, body').animate({
      scrollTop: $(`#comment_${comments[0].id}`).offset().top
   }, 1000);
}

function SetViewsAndReactions() {
   let reaction = $('#reactionHidden').val();
   let videoId = $('#videoId').val();
   let userReactionsComentId = $('.userReactionsComentId');

   if (reaction !== undefined || reaction !== "" || reaction !== 0) {
      $(`#emoji${reaction}`).addClass("resizeVideoEmoji");
      $(`#emojiCount${reaction}`).addClass("resizeVideoEmojiCount");
   }
   if (userReactionsComentId !== undefined) {
      for (var i = 0; i < userReactionsComentId.length; i++) {
         let commentId = userReactionsComentId[i].value;
         let emojiId = $(`#userReactionEmojiId_${commentId}`).val();
         $(`#commentEmoji${emojiId}_${commentId}`).addClass('resizeCommentEmoji');
         $(`#emojiCount${emojiId}Comment_${commentId}`).addClass('resizeCommentEmojiCount');
      }
   }

   $.ajax({
      url: 'https://api.ipify.org/?format=json',
      type: 'GET',
      dataType: 'json',
      success: function (ipAddress) {
         $.ajax({
            url: `/Views?videoId=${videoId}&ipAddress=${ipAddress.ip}`,
            type: 'GET',
            dataType: 'json',
            success: function (viewsCount) {
               $("#viewsCount").text(`${viewsCount} Views `);
            },
            error: function (response, jqXHR) {
            }
         });
      },
      error: function (response, jqXHR) {
      }
   });
}

function CheckInvitationCode() {
   let videoId = $('#videoId').val();
   let invitationCode = $('#invatationCode').val();
   $.ajax({
      url: `/Videos/CheckInvitationCode?videoId=${videoId}&invitationCode=${invitationCode}`,
      type: 'GET',
      dataType: 'json',
      success: function (response) {
         if (response === true) {
            SetViewsAndReactions();
            $('#firstDiv').show();
            $('#inviteCodeDialog').modal('hide');
         }
         else 
            $('#invitationCodeError').text('Wrong invatation code');
      },
      error: function (response, jqXHR) {
      }
   });
}

function Reaction(emojiId, videoId, commentId) {
   let oldReaction = $('#reactionHidden').val();
   $.ajax({
      url: '/Reactions/Create',
      type: 'POST',
      data: {
         'videoId': videoId,
         'emojiId': emojiId,
         'commentId': commentId
      },
      dataType: 'json',
      success: function (data, textStatus, xhr) {
      },
      error: function (data, xhr) {
         if (data.status === 201) {
            if (videoId !== null) {
               $('.resizeVideoEmoji').removeClass('resizeVideoEmoji');
               $('.resizeVideoEmojiCount').removeClass('resizeVideoEmojiCount');
               //decrease emoji reactions count for new reaction
               $(`#emojiCount${oldReaction}`).html(parseInt($(`#emojiCount${oldReaction}`).html()) - 1);
               if (oldReaction !== emojiId) {
                  $(`#emoji${emojiId}`).addClass("resizeVideoEmoji");
                  $(`#emojiCount${emojiId}`).addClass("resizeVideoEmojiCount");
                  //increase emoji reactions count for new reaction
                  $(`#emojiCount${emojiId}`).html(parseInt($(`#emojiCount${emojiId}`).html()) + 1);
                  $('#reactionHidden').val(emojiId);
               }
            }
            else if (commentId !== null) {
               let userReactionsCommentDiv = $('#userReactionsCommentDiv');
               let oldCommentReaction = $(`#userReactionCommentId_${commentId}`).val();
               if (oldCommentReaction === undefined) {

                  //if user first time liked/disliked this comment
                  $(`#commentEmoji${emojiId}_${commentId}`).addClass("resizeCommentEmoji");
                  let reactionsCount = $(`#emojiCount${emojiId}Comment_${commentId}`);
                  reactionsCount.addClass('resizeCommentEmojiCount');
                  reactionsCount.html(parseInt(reactionsCount.html()) + 1);
                  userReactionsCommentDiv.append(
                     `<input type="hidden" value="${commentId}" class="userReactionsComentId" id="userReactionCommentId_${commentId}" />
                      <input type="hidden" value="${emojiId}" id="userReactionEmojiId_${commentId}" />`
                  );
               }
               else {
                  //if user already liked/disliked this comment
                  let oldEmoji = $(`#userReactionEmojiId_${commentId}`).val();
                  if (oldEmoji === emojiId) {

                     //if the user want to remove like/dislike on decrease size of emoji
                     $(`#commentEmoji${emojiId}_${commentId}`).removeClass('resizeCommentEmoji');
                     let reactionsCount = $(`#emojiCount${emojiId}Comment_${commentId}`);
                     reactionsCount.removeClass('resizeCommentEmojiCount');
                     reactionsCount.html(parseInt(reactionsCount.html()) - 1);

                     //remove reaction for this comment in hidden inputs
                     $(`#userReactionCommentId_${commentId}`).remove();
                     $(`#userReactionEmojiId_${commentId}`).remove();

                  }
                  else {
                     //if user first time like/dislike this comment with this emoji
                     //resize selected emoji and incrise count
                     $(`#commentEmoji${emojiId}_${commentId}`).addClass('resizeCommentEmoji');
                     let reactionsCount = $(`#emojiCount${emojiId}Comment_${commentId}`);
                     reactionsCount.addClass('resizeCommentEmojiCount');
                     reactionsCount.html(parseInt(reactionsCount.html()) + 1);

                     //remove reaction for this comment in hidden inputs
                     $(`#userReactionCommentId_${commentId}`).remove();
                     $(`#userReactionEmojiId_${commentId}`).remove();

                     //add new user reaction to hidden input
                     userReactionsCommentDiv.append(
                        `<input type="hidden" value="${commentId}" class="userReactionsComentId" id="userReactionCommentId_${commentId}" />
                      <input type="hidden" value="${emojiId}" id="userReactionEmojiId_${commentId}" />`
                     );

                     //decrise previouse emoji of this comment and decrise count
                     let previouseEmoji = $(`#userReactionEmojiId_${commentId}`).val();
                     if (emojiId === 1) {
                        $(`#commentEmoji${2}_${commentId}`).removeClass('resizeCommentEmoji');
                        let reactionsCount = $(`#emojiCount${2}Comment_${commentId}`);
                        reactionsCount.removeClass('resizeCommentEmojiCount');
                        reactionsCount.html(parseInt(reactionsCount.html()) - 1);
                     }
                     else {
                        $(`#commentEmoji${1}_${commentId}`).removeClass('resizeCommentEmoji');
                        let reactionsCount = $(`#emojiCount${1}Comment_${commentId}`);
                        reactionsCount.removeClass('resizeCommentEmojiCount');
                        reactionsCount.html(parseInt(reactionsCount.html()) - 1);
                     }
                  }
               }
            }
         }
         else if (data.status === 401) {
            let pageVideoId = $("#videoId").val();
            RedirectToLogin(pageVideoId);
         }
      }
   });
}

function CreateComment(videoId) {
   let commentContent = $('#newComment').val();

   if (commentContent === "")
      return;

   $.ajax({
      url: '/Comments/Create',
      type: 'POST',
      data: {
         "CommentContent": commentContent,
         "VideoId": videoId
      },
      dataType: 'json',
      success: function (response) {
         ResetTextarea();
         populateNewCommentDiv(response);
         let oldCommentsCount = $('#commentsCount').val();
         $('#commentsCount').val(parseInt(oldCommentsCount) + 1);
      },
      error: function (response, jqXHR) {
      }
   });
}

function populateNewCommentDiv(comment) {
   let commentsDiv = $('#commentsDiv');
   commentsDiv.prepend(
      `<div class="col-12 comment_${comment.commentId}" id="comment_${comment.commentId}">
         <div class="row">
            <div class="col-1">
               <a href="/Users/${comment.ownerChannelName.replace(' ', '-')}">
                  <div class="commentImageDiv">
                     <img src="/profileImages/${comment.ownerProfileImage}" />
                  </div>
               </a>
            </div>
            <div class="col-11">
               <div class="row">
                  <div class="col-8">
                     <span><a href="/Users/${comment.ownerChannelName.replace(' ', '-')}"><strong>   ${comment.ownerChannelName}</strong></a> ${FormatDateString(comment.dateCreatedOn)}</span>
                  </div>
                     <div class="col-4" id="mainCommentOptionsDiv_${comment.commentId}">
                        <button class="dotBtn" onclick="showCommentOptionDiv(${comment.commentId})">
                           <div class="dotMenu float-right">
                           </div>
                        </button>
                        <div class="commentsOptionsDiv float-right" style="display: none;" id="commentOptionsDiv_${comment.commentId}">
                           <div class="hoverGray"><button class="transparentBtn" onclick="MakeCommentEditable(${comment.commentId})">Edit <i class='fas fa-edit'></i></button></div>
                           <div class="hoverGray"><button class="transparentBtn" onclick="DeleteComment(${comment.commentId})">Delete <i class='far fa-trash-alt'></i></button></div>
                        </div>
                     </div>
                  <div class="col-12" id="commentContentDiv_${comment.commentId}">
                     <span id="commentContentSpan_${comment.commentId}">${comment.commentContent}</span>
                  </div>
               </div>
               <div class="col-12 inline-flex noPadding">
                  <div>
                     <button class="commentEmojiBtn" onclick="Reaction(1, null, ${comment.commentId})">
                        <img src="/images/likeEmoji.png" id="commentEmoji1_${comment.commentId}" class="" />
                        <h6 class="whiteColor moveLeft" id="emojiCount1Comment_${comment.commentId}">0</h6>
                     </button>
                  </div>
                  <div>
                     <button class="commentEmojiBtn" onclick="Reaction(2, null, ${comment.commentId})">
                        <img src="/images/dislikeEmoji.png" id="commentEmoji2_${comment.commentId}" class="" />
                        <h6 class="whiteColor moveLeft" id="emojiCount2Comment_${comment.commentId}">0</h6>
                     </button>
                  </div>
               </div>
               <div class="float-right displayNone" id="editCommentBtnDiv_${comment.commentId}">
                  <button class="grayBtn" id="cancelComment" onclick="CancelCommentEdit(${comment.commentId})">Cancel</button>
                  <button class="orangeBtn" id="saveComment" onclick="EditComment(${comment.commentId})">Save</button>
               </div>
            </div>
         </div>
      </div>
      <div class="col-12 comment_${comment.commentId}" id="breakAfterOwner">
         <div class="breakLine"></div>
      </div>`
   );
   $('html, body').animate({
      scrollTop: $(`#comment_${comment.commentId}`).offset().top
   }, 1000);
}

function ResetTextarea() {
   $('#newComment').val("");
}

function DeleteComment(commentId) {
   $("#deleteCommentDialog").load(`/Comments/GetDeleteDialog?id=${commentId}`, function (responseTxt, statusTxt, xhr) {
      if (statusTxt == "error")
         console.log("error")
      else {
         $('#deleteCommentDialog').modal('show');
      }
   });
}

function DeleteCommentConfirm(id) {
   $.ajax({
      url: `/Comments/Delete/${id}`,
      type: 'DELETE',
      dataType: 'json',
      success: function (response) {
      },
      error: function (data, xhr) {
         if (data.status === 200) {
            $('#deleteCommentDialog').modal('hide');
            $(`#comment_${id}`).remove();
         }
         if (data.status !== 200)
            alert('Delete operation failed');
      }
   });
}

function DeleteVideo(videoId) {
   $("#deleteVideoDialog").load(`/Videos/GetDeleteDialog?id=${videoId}`, function (responseTxt, statusTxt, xhr) {
      if (statusTxt == "error")
         console.log("error")
      else {
         $('#deleteVideoDialog').modal('show');
      }
   });
}

function DeleteVideoConfirm(id) {
   $.ajax({
      url: `/Videos/Delete/${id}`,
      type: 'DELETE',
      dataType: 'json',
      success: function (response) {
      },
      error: function (data, xhr) {
         if (data.status === 200)
            window.location.replace('/');

         else
            alert('Delete operation failed');
      }
   });
}

function showCommentOptionDiv(commentId) {
   let displayed = $(`#commentOptionsDiv_${commentId}`).is(":visible");
   if (displayed)
      $(`#commentOptionsDiv_${commentId}`).css('display', 'none');
   else
      $(`#commentOptionsDiv_${commentId}`).css('display', 'block');
}

function MakeCommentEditable(commentId) {
   $(`#commentOptionsDiv_${commentId}`).hide();
   $(`#editCommentBtnDiv_${commentId}`).show();
   let commentContent = $(`#commentContentSpan_${commentId}`);
   let commentDiv = $(`#commentContentDiv_${commentId}`);
   commentContent.attr('contentEditable', true);
   commentDiv.addClass('commentBorder');
   commentDiv.append(`<input type="hidden" id="oldComment_${commentId}" value="${commentContent.text()}" />`);
}

function CancelCommentEdit(commentId) {
   $(`#commentContent_${commentId}`).attr('contentEditable', false);
   $(`#editCommentBtnDiv_${commentId}`).hide();
   let commentContent = $(`#commentContentSpan_${commentId}`);
   let commentDiv = $(`#commentContentDiv_${commentId}`);
   let oldComment = $(`#oldComment_${commentId}`);
   commentDiv.removeClass('commentBorder');
   commentContent.text(oldComment.val());
   oldComment.remove();
}

function EditComment(commentId) {
   let commentContent = $(`#commentContentSpan_${commentId}`).text();
   let oldComment = $(`#oldComment_${commentId}`).val();

   if (commentContent === "")
      return;
   if (commentContent === oldComment) {
      CancelCommentEdit(commentId);
      return;
   }

   $.ajax({
      url: `/Comments/Edit/${commentId}`,
      type: 'PUT',
      dataType: 'json',
      data: { "CommentContent": commentContent },
      success: function (response) {
      },
      error: function (data, xhr) {
         if (data.status === 200) {
            $(`#commentContent_${commentId}`).attr('contentEditable', false);
            $(`#editCommentBtnDiv_${commentId}`).hide();
            $(`#commentContentDiv_${commentId}`).removeClass('commentBorder');
         }

         else
            alert('Delete operation failed');
      }
   });
}

function OpenEditVideoModal() {
   $('#editVideoModal').modal('show');
   $('html, body').animate({
      scrollTop: $('#editVideoModal').offset().top
   }, 1000);
}

function EditVideo() {
   $('#editVideoModal').modal('hide');
}

//resize textarea
(function () {
   var measurer = $('<span>', {
      style: "display:inline-block;word-break:break-word;visibility:hidden;white-space:pre-wrap;"
   })
      .appendTo('body');
   function initMeasurerFor(textarea) {
      if (!textarea[0].originalOverflowY) {
         textarea[0].originalOverflowY = textarea.css("overflow-y");
      }
      var maxWidth = textarea.css("max-width");
		measurer.text(textarea.text())
			.css("max-width", maxWidth === "none" ? textarea.width() + "px" : maxWidth)
			.css('font', textarea.css('font'))
			.css('overflow-y', textarea.css('overflow-y'))
			.css("max-height", textarea.css("max-height"))
			.css("min-height", textarea.css("min-height"))
			.css("padding", textarea.css("padding"))
			.css("border", textarea.css("border"))
			.css("box-sizing", textarea.css("box-sizing"));
   }
   function updateTextAreaSize(textarea) {
      textarea.height(measurer.height());
      var w = measurer.width();
      if (textarea[0].originalOverflowY === "auto") {
         var mw = textarea.css("max-width");
         if (mw !== "none") {
            if (w === parseInt(mw)) {
               textarea.css("overflow-y", "auto");
            } else {
               textarea.css("overflow-y", "hidden");
            }
         }
      }
      textarea.width(w + 2);
   }
   $('textarea.autofit').on({
      input: function () {
         var text = $(this).val();
         if ($(this).attr("preventEnter") === undefined) {
            text = text.replace(/[\n]/g, "<br>&#8203;");
         }
         measurer.html(text);
         updateTextAreaSize($(this));
      },
      focus: function () {
         initMeasurerFor($(this));
      },
      keypress: function (e) {
         if (e.which === 13 && $(this).attr("preventEnter") !== undefined) {
            e.preventDefault();
         }
      }
   });
})();

function RedirectToLogin(videoId) {
   window.location.replace(`/Login?redirectUrl=/Videos/${videoId}`);
}

function ShowMore() {
   let previouseHeight = $('.panel').css('height');
   $('.panel').css('max-height', 'max-content');
   let newHeight = $('.panel').css('height');
	$('#show').attr('style', 'display:none !important');
   if (previouseHeight === newHeight) {
		$('#hide').attr('style', 'display:none !important');
      return;
   }
	$('#hide').attr('style', 'display:block !important');
}

function ShowLess() {
   $('.panel').css('max-height', '4.5rem');
   $('#show').show();
   $('#hide').hide();
}