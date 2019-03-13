"use strict";

var likeConnection = new signalR.HubConnectionBuilder().withUrl("/commentHub").build();
var commentElemId, likeButtonId, likesCountId;

function getElemId(id) {
    commentElemId = "commentId-" + id;
    likeButtonId = id;
    likesCountId = "likesCount-" + id;
}

document.getElementsByName("likeButton").disabled = true;

likeConnection.on("ReceiveLike", function (likesCount) {
    document.getElementById(likesCountId).innerHTML = likesCount;
});

likeConnection.start().then(function () {
    document.getElementsByName("likeButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

var likeButtonlist = document.getElementsByName("likeButton");
[].forEach.call(likeButtonlist, function (i) {
    i.addEventListener("click", function (event) {
        var commentId = document.getElementById(commentElemId).value;
        likeConnection.invoke("LikeComment", commentId).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
});