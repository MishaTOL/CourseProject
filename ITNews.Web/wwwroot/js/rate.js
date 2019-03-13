"use strict";

var rateConnection = new signalR.HubConnectionBuilder().withUrl("/commentHub").build();

document.getElementsByClassName("rating").disabled = true;

rateConnection.on("ReceiveRating", function (message) {
    window.location.reload();
    alert(message);
});

var rateElemId;

function getRatingElemId(id) {
    rateElemId = id;
}

rateConnection.start().then(function () {
    document.getElementsByClassName("rating").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

var rateButtonlist = document.getElementsByClassName("rating");
[].forEach.call(rateButtonlist, function (i) {
    i.addEventListener("click", function (event) {
        var userName = document.getElementById("userName").value;
        var postId = document.getElementById("postId").value;
        var newRating = parseInt(rateElemId.innerText);
        //for (var i = 1; i <= 5; i++) {
        //    if (i <= newRating)
        //    {
        //        $("#star" + i).addClass('checked');
        //    }
        //    else
        //    {
        //        $('#star' + i).removeClass('checked');
        //    }
        //}
        //document.getElementById("ratingValue").value = newRating;
        rateConnection.invoke("SendRating", newRating, userName, postId).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
});
