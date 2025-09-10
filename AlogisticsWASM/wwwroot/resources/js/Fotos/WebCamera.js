'use strict';

let currentFacingMode = "environment"; // Por defecto cámara trasera

function startVideo(useFrontCamera) {
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {

        currentFacingMode = useFrontCamera ?  "user" : "environment";

        const constraints = {
            audio: false,
            video: {
                facingMode: currentFacingMode,
                width: 1280,
                height: 720,
            }
        }

        navigator.mediaDevices.getUserMedia(constraints).then(function (stream) {
            let video = document.getElementById("videoFeed");
            if ("srcObject" in video) {
                video.srcObject = stream;
            } else {
                video.src = window.URL.createObjectURL(stream);
            }

            video.onloadedmetadata = function (e) {
                video.play();
            };
            // //mirror image
            video.style.webkitTransform = "scaleX(-1)";
            video.style.transform = "scaleX(-1)";

            function onCapabilitiesReady(capabilities) {
                if (capabilities.torch) {
                    track.applyConstraints({
                        advanced: [{ torch: flash }]
                    })
                        .catch(e => console.log(e));
                }
            }
        });
    }
}

function switchCamera(src, useFrontCamera) 
{
    // se vuelve a cargar la cámara.
    stopVideo(src);
    startVideo(useFrontCamera);
}

function getFrame(src, dest, dotNetHelper) {
    let video = document.getElementById(src);
    let canvas = document.getElementById(dest);

    let ctx = canvas.getContext('2d');
    ctx.save();
    ctx.translate(1280, 0);
    ctx.scale(-1, 1);
    ctx.drawImage(video, 0, 0, 1280, 720);
    ctx.setTransform(1,0,0,1,0,0);
    ctx.restore();

    let dataUrl = canvas.toDataURL("image/jpeg", 0.5);

    dotNetHelper.invokeMethodAsync('ProcesarImagen', dataUrl);
}

function stopVideo(src) {
    let video = document.getElementById(src);

    video.pause();
    video.currentTime = 0;

    if (video && "srcObject" in video) {
        const stream = video.srcObject;
        if (stream) {
            const tracks = stream.getTracks();
            tracks.forEach(track => track.stop());
        }
        video.srcObject = null;
    }
}

function onchangestart() {
    localStorage.setItem("CamaraSelected", $('#CamaraSelect').val());

    console.log("onchangestart() Valor : " + $('#CamaraSelect').val());

    stopVideo();
    startVideo();
}