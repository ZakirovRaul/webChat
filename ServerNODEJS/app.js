const net = require('net');
const server = new net.Server();
const crypto = require('crypto');
const frame = require('./frame');
const msg = require('./message');
const EventEmitter = require('events');
const storage = require('./storage');

const notifier = require('./notifier');
const channelsNotifier = new notifier.ChannelsNotifier();


server.on('error', (err) => {
    throw err;
});

server.on('connection', (socket) => {
    var channel = new networkChannel(socket);

    socket.on('end', () => {
        console.log('client disconnected');
    });

    socket.on('data', (data) => {
        channel.getEmitter().emit('request', data);//examine this line in detail
       
    });


});


server.listen(8090, () => {
    console.log('server bound');
});

class networkChannel {

    constructor(_socket) {
        this.socket = _socket;
        this.emitter = new EventEmitter();
        channelsNotifier.addChannel(this);

        this.emitter.once('request', (data) => {
            this.processHandShake(this.socket, data);
            this.emitter.on('request', (data1) => {
                if (frame.isClose(data1))
                {
                    this.socket.destroy();
                    storage.removeName(this.userName);
                    channelsNotifier.removeChannel(this);
                    channelsNotifier.notifyAll(msg.MessageFactory.createUsersList());
                    channelsNotifier.notifyAll(msg.MessageFactory.createMessage(this.userName + " has disconnected"));
                } else
                {
                    var message = frame.decodeMessage(data1);
                    storage.addMessage(this.userName, message);
                    channelsNotifier.notify(this, msg.MessageFactory.createMessage(message));
                }
            });
        });
    }

    getEmitter() {
        return this.emitter;
    }

    processHandShake(socket, chunk) {
        var headers = chunk.toString().trim().split('\r\n');
        var webSocketKey = channelHelper.extractWebSocketKey(headers);
        if (webSocketKey != null) {
            var responseKey = channelHelper.handShakeResponse(webSocketKey);
            var response =
                'HTTP/1.1 101 Switching Protocols\r\nUpgrade: websocket\r\nConnection: Upgrade\r\nSec-WebSocket-Accept: ' + responseKey + '\r\nSec-WebSocket-Protocol: chat\r\n\r\n';
            socket.write(response);
            this.userName = headers[0].match('^GET \\/\\?name=(.+) HTTP')[1];


            this.notify(msg.MessageFactory.createHistory());
            this.notify(msg.MessageFactory.createMessage(this.userName + " has connected"));

            storage.addName(this.userName);
            channelsNotifier.notifyAll(msg.MessageFactory.createUsersList());
        }
    }

    notify(message) {
        var encodedMsg = frame.encodeMessage(JSON.stringify(message));
        this.socket.write(encodedMsg);
    }

}

channelHelper = {

    extractWebSocketKey : (headers) => {
        for (var i = 0; i < headers.length; i++) {
            if (headers[i].includes("Sec-WebSocket-Key: ")) {
                return headers[i].replace("Sec-WebSocket-Key: ", "");
            }
        }
        return null;
    },

    handShakeResponse : (socketKey) => {
        socketKey = socketKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        var hash = crypto.createHash("SHA1");
        hash.update(socketKey);
        return hash.digest('base64');
    }
}
