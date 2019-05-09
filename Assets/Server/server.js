const WebSocket = require('ws');

const wss = new WebSocket.Server({
    port: 8080
});

wss.on('connection', function connection(ws) {
    console.log('connected');

    ws.on('close', function close() {
        console.log('disconnected');
    });

    ws.on('message', function incoming(message) {
        ws.send(message);
    });
});