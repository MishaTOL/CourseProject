"use strict";

var commentConnection = new signalR.HubConnectionBuilder().withUrl("/commentHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

commentConnection.on("ReceiveComment", function () {
    $("#commentsList").load(location.href + " #commentsList > *");
});

commentConnection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var comment = document.getElementById("commentInput").value;
    var userName = document.getElementById("userName").value;
    var postId = document.getElementById("postId").value;
    commentConnection.invoke("SendComment", comment, userName, postId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});