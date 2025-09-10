window.loadImageOnCanvas = (imagePath) => {
    const canvas = document.getElementById("markerCanvas");
    const ctx = canvas.getContext("2d");
    const img = new Image();
    
    img.onload = () => {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
    };
    
    img.src = imagePath;
};

window.drawMarker = (x, y) => {
    const canvas = document.getElementById("markerCanvas");
    const ctx = canvas.getContext("2d");

    ctx.fillStyle = "red";
    ctx.beginPath();
    ctx.arc(x, y, 5, 0, 2 * Math.PI);
    ctx.fill();
};

window.drawMarkerWithLabel = (x, y, label) => {
    const canvas = document.getElementById("markerCanvas");
    const ctx = canvas.getContext("2d");

    ctx.fillStyle = "red";
    ctx.beginPath();
    ctx.arc(x, y, 5, 0, 2 * Math.PI);
    ctx.fill();
};

window.redrawCanvas = (imagePath, markers) => {
    const canvas = document.getElementById("markerCanvas");
    const ctx = canvas.getContext("2d");
    const img = new Image();
    
    img.onload = () => {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);

        ctx.fillStyle = "red";
        markers.forEach(marker => {
            ctx.beginPath();
            ctx.arc(marker.x, marker.y, 5, 0, 2 * Math.PI);
            ctx.fill();
        });
    };
    
    img.src = imagePath;
};

window.saveCanvasImage = () => {
    const canvas = document.getElementById("markerCanvas");
    const link = document.createElement("a");
    link.download = "imagen_marcada.png";
    link.href = canvas.toDataURL("image/png");
    link.click();
};

window.getCanvasBase64 = () => {
    const canvas = document.getElementById("markerCanvas");
    return canvas.toDataURL("image/png").replace("data:image/png;base64,", "");
};