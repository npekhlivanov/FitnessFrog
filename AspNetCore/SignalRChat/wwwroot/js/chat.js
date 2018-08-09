// The following sample code uses modern ECMAScript 6 features 
// that aren't supported in Internet Explorer 11.
// To convert the sample for environments that do not support ECMAScript 6, 
// such as Internet Explorer 11, use a transpiler such as 
// Babel at http://babeljs.io/. 
//
// See Es5-chat.js for a Babel transpiled version of the following code:

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.on("ReceiveMessage", function(user, message) {
    const msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    const encodedMsg = user + " says " + msg;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});


document.getElementById("sendButton").addEventListener("click", function(event) {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message)
        .catch(function (err) {
            return console.error(err.toString());
        });
    event.preventDefault();
});

document.getElementById("getButton").addEventListener("click", function() {
    const user = document.getElementById("userInput").value;
    connection.invoke("GetNotification", user)
        .catch(function (err) {
            return console.error(err.toString());
        });
    event.preventDefault();
});

connection.on("ReceiveNotification", function(message) {
    var myRow = document.getElementById("myRow");
    myRow.innerText = message;
});

connection.on("ProgressNotification", function (message) {
    var myProgress = document.getElementById("myProgress");
    var percent = parseInt(message, 10);
    myProgress.style.width = percent + "%";
    myProgress.innerText = percent + '%';
});

connection.start()
    .catch(function(err) {
        return console.error(err.toString());
    });
   