//import { fail } from "assert";

function cargarCamara() {
    showCargando();
    var _ajax = $.ajax({
        url: sURLcargarCamara,
        type: 'POST',
        datatype: 'html'
    });
    _ajax.done(function (sHTML) {
        bootbox.dialog({ title: "", className: "ModalCamara", message: sHTML, closeButton: false });
    });
    _ajax.always(function () {
        hideCargando();
    });
}

function tomarFoto() {
    $('#CamaraCanvas').attr({ "width": videoElement.videoWidth, "height": videoElement.videoHeight })
    $('#CamaraCanvas').css({ "max-width": videoElement.videoWidth + "px", "max-height": videoElement.videoHeight + "px" })

    cxt.drawImage(videoElement, 0, 0);
}

function guardarFoto() {
    console.log("Inciando el guardado de fotografia");

    var sBuferCanvas = canvasElement.toDataURL('image/jpeg', 0.5);
    var sImgTemp = sBuferCanvas.replace('data:image/jpeg;base64,', '');
    var sImgBase64 = sImgTemp.replace(' ', '+');
    var nId = $("#ListadoContenedores tbody tr.danger").attr('nItemContenedor');
    var oListadoContenedores = jQuery.data($('#ListadoContenedores')[0], "ListadoContenedores")[nId];

    console.log("enviando a " , sURLUploadFotoCapturada);
    console.log("base64 a " , sImgBase64);


    var _ajax = $.ajax({
        type: "POST",
        url: sURLUploadFotoCapturada,    
        data: {
            folioServicio: oListadoContenedores.FolioServicio,
            imageStringBase64: sImgBase64
        }
    });
    _ajax.done(function (oJson) {
        console.log("Regresando oJson",oJson);
        if (oJson.Result) {
            Toast.show({ message: "Foto Guardada" })
           // dibujaFotos();
        } else {
            Toast.show({ message: oJson.Mensaje })
        }
        
    });

    _ajax.fail(function (jqXHR, textStatus, errorThrown) {
        if (jqXHR.status === 0) {

            alert('No hay conexión: Verifique la red.');

        } else if (jqXHR.status === 404) {

            alert('Página solicitada no encontrada [404]');

        } else if (jqXHR.status === 500) {

            alert('Error interno en servidor [500].');

        } else if (textStatus === 'parsererror') {

            alert('Falló el parseo de la petición JSON.');

        } else if (textStatus === 'timeout') {

            alert('Error de Time out. La operación tardó demasiado.');

        } else if (textStatus === 'abort') {

            alert('Petición Ajax abortada.');

        } else {

            alert('Error Inesperado: ' + jqXHR.responseText);

        }
    });
}

function dibujaFotos() {
    var nId = $("#ListadoContenedores tbody tr.danger").attr('nItemContenedor');
    var oListadoContenedores = jQuery.data($('#ListadoContenedores')[0], "ListadoContenedores")[nId];

    var _ajax = $.ajax({
        url: sURLBuscarFotografias,
        type: 'POST',
        datatype: 'html',
        data: {
            folioServicio: oListadoContenedores.FolioServicio,
        }
    });
    _ajax.done(function (sHTML) {
        $('#TablaFotos').html(sHTML);
    });
}