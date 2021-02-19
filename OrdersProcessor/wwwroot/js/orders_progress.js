const connection = new signalR.HubConnectionBuilder()
    .withUrl("/orderhub").build();

connection.on("ReceiveProgress", (progress) => {
    document.getElementById("lblProgress").innerHTML = progress;
    document.getElementById("progress-bar").style.width = `${progress}%`;
});

connection.start().then(() => {
    connection.invoke('getConnectionId')
        .then(function (connectionId) {
            document.getElementById("ConnectionId").value = connectionId;
        });
}).catch(err => console.error(err.toString()));