function checkPrinterSocket() {
    $('#pnl-checked').hide();
    $('#pnl-checked').empty();
    $('#pnl-submit').hide();
    
    var ws = new WebSocket('ws://127.0.0.1:8888/Printer');
    ws.onerror = function (error) {
        console.log('WebSocket Error ' + error);

        $('#pnl-submit').hide();

        $('<b/>', {
            'class': 'text-danger',
            html: "El servicio de impresion no se encuentra disponible, por favor verifique si se encuentra iniciado"
        }).appendTo('#pnl-checked');

        $('<a/>', {
            'href': 'javascript:checkPrinterSocket();',
            html: "Reintentar"
        }).appendTo('#pnl-checked');

        $('#pnl-checked').show();
    };

    ws.onopen = function () {
        console.log('WebSocket Connected');
        $('#pnl-checked').hide();
        $('#pnl-checked').empty();
        $('#pnl-submit').show();
        ws.close();
        console.log('WebSocket Closed');
    };
}