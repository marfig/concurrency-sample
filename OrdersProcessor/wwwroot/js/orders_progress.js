const connection = new signalR.HubConnectionBuilder()
    .withUrl("/orderhub").build();

connection.on("ReceiveProgress", (progress) => {
    document.getElementById("lblProgress").innerHTML = progress;
});

connection.start().catch(err => console.error(err.toString()));