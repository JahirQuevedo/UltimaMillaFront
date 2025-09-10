'use strict';

videoElement = document.getElementById('CamaraVideo');
videoSelect = document.getElementById('CamaraSelect');
canvasElement = document.getElementById('CamaraCanvas');
cxt = canvasElement.getContext('2d');

navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia;
var flash = false;
if (typeof (Storage) !== "undefined") {

} else {
    alert('Este Navegador NO Soporta Web Storage.');
}

function onSelectCameraClick() {
    console.log("onSelectCameraClick");
    navigator.mediaDevices.enumerateDevices()
        .then(function (deviceInfos) {
            //console.log("Obteniendo dispositivos de video..");
            for (var i = 0; i !== deviceInfos.length; ++i) {
                var deviceInfo = deviceInfos[i];
                var option = document.createElement('option');
                option.value = deviceInfo.deviceId;

                if (deviceInfo.kind === 'videoinput') {
                    option.text = deviceInfo.label || 'camera ' + (videoSelect.length + 1);
                    videoSelect.appendChild(option);
                }
            }
            //console.log("Valor en localStorage(): "+localStorage.getItem("CamaraSelected"));
            //console.log("Valor en combobox: "+$('#CamaraSelect').val());
            if (localStorage.getItem("CamaraSelected") != $('#CamaraSelect').val()) {
                $('#CamaraSelect option[value="' + localStorage.getItem("CamaraSelected") + '"]').prop('selected', true);
            }
            //console.log("Asignando listener..");
            videoSelect.onchange = onchangestart;
        })
        .then(start)
        .catch(function () {
            console.log("Error al Cargar el combo de seleccion de las Camaras");
        });
}
//console.log("Soporta MediaStream? : "+typeof(MediaStreamTrack));
if (typeof (MediaStreamTrack) === "undefined") {
    alert('Este Navegador NO Soporta "MediaStreamTrack".');
} else {
    //console.log("01 onSelectCameraClick()");
    onSelectCameraClick();
}

function successCallback(stream) {
    //console.log("successCallback()...");
    //videoElement.src = null;
    window.stream = stream;
    //videoElement.src = window.URL.createObjectURL(stream);
    videoElement.srcObject = stream;
    videoElement.play();
}


function errorCallback(error) {
    alert("Error del Navegador al Cargar la Camara, Cierre la modal e intente de nuevo");
}

function start() {
    console.log("start()...");
    
    var videoSource = videoSelect.value;
    var constraints = {
        audio: false,
        video: {
            deviceId: videoSource ? { exact: videoSource } : undefined,
            facingMode: 'environment',
            width: 1280,
            height: 720,
        }
    };

    console.log("...start() --> videoSource = " + videoSource);
    //navigator.getUserMedia(constraints, successCallback, errorCallback);
    navigator.mediaDevices.getUserMedia(constraints)
        .then((stream) => {
            const video = document.querySelector('video');
            video.srcObject = stream;

            // get the active track of the stream
            const track = stream.getVideoTracks()[0];

            video.addEventListener('loadedmetadata', (e) => {
                window.setTimeout(() => (
                    onCapabilitiesReady(track.getCapabilities())
                ), 500);
            });

            
            if ($('#cmbFlash').is(":checked")) {
                flash = true;
            } else {
                flash = false;
            }

            function onCapabilitiesReady(capabilities) {
                if (capabilities.torch) {
                    track.applyConstraints({
                        advanced: [{ torch: flash }]
                    })
                        .catch(e => console.log(e));
                }
            }

        })
        .catch(errorCallback);
}


function onchangestart() {
    localStorage.setItem("CamaraSelected", $('#CamaraSelect').val());

    console.log("onchangestart() Valor : " + $('#CamaraSelect').val());

    EndFotografias();
    start();
}

function EndFotografias() {
    console.log("EndFotografias()...");

    const video = document.querySelector('video');

    const stream = video.srcObject;
    if (stream == null) {
        console.log("Nada que cerrar");
    }
    const tracks = stream.getTracks();
    console.log(tracks);
    tracks.forEach(function (track) {
        track.stop();
    });

    video.srcObject = null;
}