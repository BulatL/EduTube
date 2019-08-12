$(function () {
    let subscribedHidden = $("#subscribedHidden");
    console.log(subscribedHidden.val());
    if (subscribedHidden != undefined) {
        let subscribeBtn = $("#subscribeBtn");

        if (subscribedHidden.val() == "Subscribe") {
            subscribeBtn.text("Subscribe");
        }
        else if (subscribedHidden.val() == "Unsubscribe") {
            console.log("unsubscribe je");
            subscribeBtn.text("Unsubscribe");
        }
        else if (subscribedHidden.val() == "UserNotLogged") {
            subscribeBtn.text("Subscribe");
        }
    }
})
function Subscribe() {
    let subscribeOn = $("#userIdHidden").val();
    let subscribedHidden = $("#subscribedHidden");

    if (subscribedHidden != undefined) {
        let method = "";
        let btnNewValue = "";
        let subscribeBtn = $("#subscribeBtn");

        if (subscribedHidden.val() == "Subscribe") {
            method = "Subscribe";
            btnNewValue = "Unsubscribe";
        }
        else if (subscribedHidden.val() == "Unsubscribe") {
            method = "Unsubscribe";
            btnNewValue = "Subscribe";
        }
        else if (subscribedHidden.val() == "UserNotLogged") {
            alert("login first");
            return;
        }   
        $.ajax({
            url: `/Subscribe/${subscribeOn}/${method}`,
            type: 'GET',
            dataType: 'json',
            contentType: 'application/json',
            success: function (response) {
                if (response == "Error")
                    alert("Error");

                else {
                    if (method == "Subscribe") {
                        let channelName = response.item1;
                        let profileImage = response.item2;
                        
                        let subscribersDiv = $("#subscribersDiv");
                        subscribersDiv.append(
                            `<div class="display-inline-block" id="subscriber_${channelName.replace(/ /g, '-')}">
                                <a href="/Users/${channelName.replace(/ /g, '-')}">
                                    <div id="subProfileImageDiv">
                                        <img id="subProfileImage" src="/profileImages/${profileImage}" />
                                    </div>
                                    <p>${channelName}</p>
                                </a>
                            </div>`
                        );
                    }
                    else {
                        $(`#subscriber_${response.replace(/ /g, '-')}`).remove();
                    }
                    subscribeBtn.text(btnNewValue);
                    subscribedHidden.val(btnNewValue);
                }
            },
            error: function (response, jqXHR) {
                console.log(response)
            }
        });
    }
}