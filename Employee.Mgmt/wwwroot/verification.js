
var verificationId = document.getElementById('verificationId');

var PENDING = document.getElementById('PENDING');
var SUCCESS = document.getElementById('SUCCESS');
var FAILED = document.getElementById('FAILED');
var PROBLEM = document.getElementById('PROBLEM');
var message = document.getElementById('message'); 
var qrCodeImage = document.getElementById('qrCodeImage'); 
var verifiedData = document.getElementById('verifiedData'); 
var verifiedDataButton = document.getElementById('verifiedDataButton');

if (verificationId != null) {
   
    var checkStatus = setInterval(function () {
        if (verificationId) {

            fetch('api/status/verification-response?id=' + verificationId.value)
                .then(response => response.text())
                .catch(error => document.getElementById("message").innerHTML = error)
                .then(response => {
                    if (response.length > 0) {
                        respMsg = JSON.parse(response);
                        console.log("status: " + respMsg["state"])
                        // PENDING, SUCCESS, FAILED
                        if (respMsg.state == 'PENDING') {
                            message.innerHTML = respMsg["state"];
                        }
                        else if (respMsg.state == 'SUCCESS') {
                            message.innerHTML = respMsg["state"];
                            clearInterval(checkStatus)
                            console.log("VC data: " + JSON.stringify(respMsg["wallet_response"]["credential_subject_data"]))

                            PENDING.style.display = "none";
                            SUCCESS.style.display = "initial";
                            FAILED.style.display = "none";
                            PROBLEM.style.display = "none";

                            qrCodeImage.style.display = "none";
                            verifiedData.style.display = "initial";
                            verifiedDataButton.style.display = "none";
                            verifiedData.innerHTML = JSON.stringify(respMsg["wallet_response"]["credential_subject_data"]);
                        }
                        else if (respMsg.state == 'FAILED') {
                            message.innerHTML = respMsg["state"];
                            clearInterval(checkStatus);

                            PENDING.style.display = "none";
                            SUCCESS.style.display = "none";
                            FAILED.style.display = "initial";
                            PROBLEM.style.display = "none";
                        }
                        else {
                            message.innerHTML = "Unknown status: " + respMsg["state"];
                            clearInterval(checkStatus)

                            PENDING.style.display = "none";
                            SUCCESS.style.display = "none";
                            FAILED.style.display = "none";
                            PROBLEM.style.display = "initial";
                        }
                    }
                })
        }

    }, 1500); //change this to higher interval if you use ngrok to prevent overloading the free tier service
}
