﻿document.getElementById("btnProcess").addEventListener("click", ProcessOrders);

async function ProcessOrders() {
    CleanElements();

    const connId = document.getElementById("ConnectionId").value;

    const url = `/home/processorders?ConnectionID=${connId}`;

    const response = await fetch(url, {
        headers: {
            method: 'POST',
            ContentType: 'application/json'
        }
    });

    const result = await response.json();

    document.getElementById("lblMessage").innerHTML = result.message;

    document.getElementById("lblProgress").innerHTML = 100;
    document.getElementById("progress-bar").style.width = "100%";

    document.getElementById("divResult").innerHTML = `<strong>Rejected Orders (${result.rejected.length})</strong>`;

    for (let i = 0; i < result.rejected.length; i++) {
        const row = `<p>${result.rejected[i]}</p>`;
        document.getElementById("divResult").innerHTML += row;
    }

    document.getElementById("divLoading").classList.add("hide");
    document.getElementById("btnProcess").disabled = false;
}

function CleanElements() {
    document.getElementById("lblProgress").innerHTML = 0;
    document.getElementById("divResult").innerHTML = "";
    document.getElementById("lblMessage").innerHTML = "";
    document.getElementById("divLoading").classList.remove("hide");
    document.getElementById("divProgress").classList.remove("hide");
    document.getElementById("progress-bar").style.width = "0";
    document.getElementById("btnProcess").disabled = true;
}