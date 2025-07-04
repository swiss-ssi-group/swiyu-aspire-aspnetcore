
var issuanceManagementId = document.getElementById('issuanceManagementId');

var OFFERED = document.getElementById('OFFERED');
var IN_PROGRESS = document.getElementById('IN_PROGRESS');
var ISSUED = document.getElementById('ISSUED');
var PROBLEM = document.getElementById('PROBLEM');
var message = document.getElementById('message'); 

if (issuanceManagementId != null) {
   
    var checkStatus = setInterval(function () {
        if (issuanceManagementId) {

            fetch('api/status/issuance-response?id=' + issuanceManagementId.value)
                .then(response => response.text())
                .catch(error => document.getElementById("message").innerHTML = error)
                .then(response => {
                    if (response.length > 0) {
                        respMsg = JSON.parse(response);
                        console.log("status: " + respMsg["status"])
                        // OFFERED, CANCELLED, IN_PROGRESS, ISSUED, SUSPENDED, REVOKED, EXPIRED
                        if (respMsg.status == 'OFFERED') {
                            message.innerHTML = respMsg["status"];
                        }
                        else if (respMsg.status == 'IN_PROGRESS') {
                            message.innerHTML = respMsg["status"];

                            OFFERED.style.display = "none";
                            IN_PROGRESS.style.display = "initial";
                            ISSUED.style.display = "none";
                            PROBLEM.style.display = "none";
                        }
                        else if (respMsg.status == 'ISSUED') {
                            message.innerHTML = respMsg["status"];
                            clearInterval(checkStatus);

                            OFFERED.style.display = "none";
                            IN_PROGRESS.style.display = "none";
                            ISSUED.style.display = "initial";
                            PROBLEM.style.display = "none";
                        }
                        else {
                            message.innerHTML = "Unknown status: " + respMsg;
                            clearInterval(checkStatus)

                            OFFERED.style.display = "none";
                            IN_PROGRESS.style.display = "none";
                            ISSUED.style.display = "none";
                            PROBLEM.style.display = "initial";
                        }
                    }
                })
        }

    }, 1500); //change this to higher interval if you use ngrok to prevent overloading the free tier service
}
