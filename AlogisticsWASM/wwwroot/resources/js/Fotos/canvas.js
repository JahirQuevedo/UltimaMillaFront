window.getCanvasBoundingRect = (canvas) => {
    return canvas.getBoundingClientRect();
};

window.drawPoint = (canvas, x, y) => {
    var ctx = canvas.getContext("2d");
    ctx.beginPath();
    ctx.arc(x, y, 5, 0, Math.PI * 2); // Dibujar el punto
    ctx.fillStyle = "red"; // Color del punto
    ctx.fill();
};
