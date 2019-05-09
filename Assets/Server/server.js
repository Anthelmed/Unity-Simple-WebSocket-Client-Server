const WebSocket = require('ws');

const wss = new WebSocket.Server({
    port: 8080
});

wss.on('connection', function connection(ws) {
    console.log('connected');

    ws.on('close', function close() {
        console.log('disconnected');
    });

    ws.on('message', function incoming(data) {
        console.log("message received");

        wss.clients.forEach(function each(client) {
            if (client !== ws && client.readyState === WebSocket.OPEN) {
                client.send(data);
            }
        });
    });
});