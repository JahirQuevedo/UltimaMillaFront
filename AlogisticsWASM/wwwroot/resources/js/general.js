'use strict';

window.abrirEnNuevaVentana = (url) =>  {
    window.open(url, '_blank');
};

function AbrirArchivoArregloByte(Archivo, ContentType, ContentFile) {
    console.log("---> AbrirArchivoArregloByte");
    var link = document.createElement('a');
    link.download = Archivo;
    link.href = 'data:'+ContentType+';base64,' + ContentFile;
    link.click();
    
}

