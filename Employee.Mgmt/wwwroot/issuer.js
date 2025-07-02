
var issuer = document.getElementById('issuer');
var respIssuanceReqid = document.getElementById('respIssuanceReq');

if (respIssuanceReqid != null) {
   
    var checkStatus = setInterval(function () {
        if (respIssuanceReq) {

            fetch('api/status/issuance-response?id=' + respIssuanceReqid.value)
                .then(response => response.text())
                .catch(error => document.getElementById("message").innerHTML = error)
                .then(response => {
                    if (response.length > 0) {
                        console.log(response)
                        respMsg = JSON.parse(response);
                        
                        if (respMsg.status == 'request_retrieved') {
                            document.getElementById('message-wrapper').style.display = "block";
                            document.getElementById('message').innerHTML = respMsg.status;
                        }
                        if (respMsg.status == 'issuance_successful') {
                            document.getElementById('message').innerHTML = respMsg.status;
                            clearInterval(checkStatus);
                        }
                        if (respMsg.status == 'issuance_error') {
                            document.getElementById('message').innerHTML = "Issuance error occured. Please refresh the page and try again.";
                            document.getElementById('payload').innerHTML = "Payload: " + respMsg.status;
                            clearInterval(checkStatus);
                        }
                    }
                })
        }

    }, 1500); //change this to higher interval if you use ngrok to prevent overloading the free tier service
}
