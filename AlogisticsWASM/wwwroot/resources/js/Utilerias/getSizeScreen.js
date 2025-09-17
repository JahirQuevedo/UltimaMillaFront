window.getSizeScreen = function () {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};

window.registerResizeHandler = function (dotNetHelper) {
    window.addEventListener('resize', () => {
        dotNetHelper.invokeMethodAsync('OnBrowserResize', {
            width: window.innerWidth,
            height: window.innerHeight
        });
    });
};
