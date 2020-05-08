# EduTube

Edutube is application  for managing educational video content. The application allows video sharing. Users can add video, search,
comment, leave reactions, follow other users, exchange messages using chat. Admins in addition to user functionality can also add new chats, emojis,
modify any videos or users. This paper describes the technologies used for the implementation of the application, system specifications and
implementation. 

## Entities

![Model](https://github.com/BulatL/EduTube/blob/master/Images/Model.png "Model")

Here we can see entities of application

- ApplicationUser entity represent user channel
  - ChannelName - user channel name
  - Firstname - user first name.
  - Lastname - user last name.
  - ChannelDescription - user channel description.
  - ProfileImage - ser profile image.
  - DateOfBirth - user date of birth.
  - Blocked - represent if user channel is blocked.
  - Deleted - represent if user channel is deleted.
  
- Chat entity represent chat room
  - Id - primary key.
  - Name - chat name.
  - Deleted - represent if chat is deleted.
  
- ChatMessage:
  - Id- primary key.
  - Message- content of message.
  - DateCreatedOn- date of message.
  - UserId- foreign key of user who sent message.
  - ChatId - foreign key of chat in which message was sent.
  - Deleted - represent if message is deleted.
  
- Comment entity represent comment on video:
  - Id - primary key.
  - Content - content of comment.
  - DateCreatedOn - date of comment.
  - UserId - foreign key of user who post comment.
  - VideoId - foreign key of video on which comment was post.
  - Deleted - represent if comment is deleted.
  
- Emoji entity represent emoji for leaving reactions on video or comment:
  - Id - primary key.
  - Name - name of emoji.
  - ImagePath - image name of emoji.
  - Deleted - represent if emoji is deleted.
  
- Notification entity represet notification for new uploaded video:
  - Id - primary key
  - Content - content of notification.
  - DateCreatedOn - date when notification was created.
  - UserId -  foreign key of user to whom the notification is intended.
  - UserProfileImage - profile image name of user who created notification.
  - Deleted - represent if notification is deleted.
  
- Reaction entity represent reaction on video or comment
  - Id - primary key
  - DateCreatedOn - date when reaction was created.
  - EmojiId - foreign key of emoji used for reaction.
  - UserId - foreign key of user who left.
  - VideoId - foreign key of video on which reaction was left.
  - CommentId - foreign key of comment on which reaction was left.
  - Deleted - represent if reaction is deleted.
  
- Subscription entity represent user follower.
  - Id - primary key
  - SubscriberId - foreign key of user who wants to subscribe.
  - SubscribedOnId- foreign key of user they want to subscribe on.
  - Deleted - represent if subscription is deleted.
  
- Tag entity represent tag
  - Id - primary key
  - Name - tag name.
  
- TagRelationship entity that join video or chat with tag:
  - Id - primary key
  - VideoId - foreign key of video.
  - ChatId - foreign key of chat.
  - TagId - foreign key of tag.
  
- Video entitet koji predstavlja video. Od atributa sadrži:
  - Id - primary key
  - Name - video name.
  - Description - video description.
  - DateCreatedOn - date when video was created.
  - YoutubeUrl - url of the video from youtube if the video is on youtube.
  - FileName - file name of video if video is uploaded from device.
  - AllowComments - represent if comments are allowed in video.
  - Deleted - represent if video is deleted.
  - InvitationCode - invitation code (if visibility is invitation).
  - Duration - duration of video.
  - Thumbnail - thumbnail name.
  - UserId - foreign key of video owner.
  - VideoVisibility - enum that represent video visibility (public - everyone can see it, private - only logged in user can see it, invitation - only users that enter ivnitation code can see it).
  
- View entity represent one view of video
  - Id - primary key.
  - IpAddress - ip address from which video was viewed.
  - UserId - foreign key of user who viewd video.
  - VideoId - foreign key of video which was viewed.
  
  
  ## Functionality
  
![home page](https://github.com/BulatL/EduTube/blob/master/Images/HomePage.png "home page")
  
  This image represent home page. In first row user see 6 most popular (by number of views) videos. In next 2 rows, user will see 6 or less videos for 2 tags he watched mosts.
  
  
![Profile Page](https://github.com/BulatL/EduTube/blob/master/Images/ProfilePage.png "Profile Page")

  This image represnt user profile. On left side is user image,  channel name, user fisrt name and last name and channel description.
  On right we can see 3 tabs. First tab is videos where we can see all videos that user uploaded, next tab show all users that are subscribed on this profile and in last tab we can see all users on which this profile is subscribed.
  
  
![profile options](https://github.com/BulatL/EduTube/blob/master/Images/profile%20options.png "profile options")
  
  In this image image we can see options for managing user profile. We can edit basic info of user profile, change password, demote user if he was admin or promote to admin if he was basic user (functionality is avaliable only for admins), block and delete user profile 
  
  
  
![vdeo create](https://github.com/BulatL/EduTube/blob/master/Images/CreateVideo2.png "video create")

  This image represent page for uploading video. Tags input is for adding tags to the video. Input work like autocomplite, so user can see all existing tags or he can create new one. 
  VideoVisibility input is dropdown. There is 3 types of visibility, **Public** everyone can see video, **Private** only logged in user can see video and **Invitation** only users that enter invitation code can see video.
  Invitation code is random generated string.
  User have2 options for uploading video. 
    - Upload video from device.
    - Pass the youtube link of video.
    
  User can upload custom thumbnail or for thumbnail will be taken frame at 5s of video if it is uploaded from device or it will be taken from youtube api.
  And last checkbox is if user want to allow comments on video
  
  
![prikaz videa](https://github.com/BulatL/EduTube/blob/master/Images/Prikaz%20Videa.png "prikaz videa")

  This image represent uploaded video page. We can watch video, see basic info about video, see basic name of the video owner, see comments or post comment and leave reaction on video or comment.
  
  
  
![Chat](https://github.com/BulatL/EduTube/blob/master/Images/Chat.png "Chat")

  This image represent page for chat rooms. On left side user can see chat rooms. When user join chat he will see previous messages and will be able to send new ones.
  Sending  message functionality was implemented with SignalR library.
  
  

Project is using Elasticsearch 6.5.4 version
