
var issuer = document.getElementById('issuer');
var respIssuanceReqid = document.getElementById('respIssuanceReq');

if (respIssuanceReqid != null) {
   
    var checkStatus = setInterval(function () {
        if (respIssuanceReqid) {

            fetch('api/status/issuance-response?id=' + respIssuanceReqid.value)
                .then(response => response.text())
                .catch(error => document.getElementById("message").innerHTML = error)
                .then(response => {
                    if (response.length > 0) {
                        respMsg = JSON.parse(response);
                        console.log("status: " + respMsg["status"])
                        // OFFERED, CANCELLED, IN_PROGRESS, ISSUED, SUSPENDED, REVOKED, EXPIRED
                        if (respMsg.status == 'OFFERED') {
                            document.getElementById('message').innerHTML = respMsg["status"];
                        }
                        else if (respMsg.status == 'IN_PROGRESS') {
                            document.getElementById('message').innerHTML = respMsg["status"];
                        }
                        else if (respMsg.status == 'ISSUED') {
                            document.getElementById('message').innerHTML = respMsg["status"];
                            clearInterval(checkStatus);
                        }
                        else {
                            document.getElementById('message').innerHTML = "Unknown status: " + respMsg;
                            clearInterval(checkStatus)
                        }
                    }
                })
        }

    }, 1500); //change this to higher interval if you use ngrok to prevent overloading the free tier service
}
