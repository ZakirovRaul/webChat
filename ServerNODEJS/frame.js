const buf = require('buffer');

function decodeMessage(bytes) {
    var incomingData = '';
    var secondByte = bytes[1];
    var dataLength = secondByte & 127;
    var indexFirstMask = 2;
    if (dataLength == 126)
        indexFirstMask = 4;
    else if (dataLength == 127)
        indexFirstMask = 10;

    var keys = bytes.slice(indexFirstMask, indexFirstMask + 4);
    var indexFirstDataByte = indexFirstMask + 4;

    var decoded = buf.Buffer.alloc(bytes.length - indexFirstDataByte);
    for (var i = indexFirstDataByte, j = 0; i < bytes.length; i++ , j++) {
        decoded[j] = (bytes[i] ^ keys[j % 4]) & 255;
    }

    return incomingData = decoded.toString();
}

function encodeMessage(message) {
    var response;
    var bytesRaw = new buf.Buffer(message);
    var frame = buf.Buffer.alloc(10);

    var indexStartRawData = -1;
    var length = bytesRaw.length;

    frame[0] = 129;
    if (length <= 125) {
        frame[1] = length & 255;
        indexStartRawData = 2;
    }
    else if (length >= 126 && length <= 65535) {
        frame[1] = 126;
        frame[2] = (length >> 8) & 255;
        frame[3] = length & 255;
        indexStartRawData = 4;
    }
    else {
        frame[1] = 127;
        frame[2] = ((length >> 56) & 255);
        frame[3] = ((length >> 48) & 255);
        frame[4] = ((length >> 40) & 255);
        frame[5] = ((length >> 32) & 255);
        frame[6] = ((length >> 24) & 255);
        frame[7] = ((length >> 16) & 255);
        frame[8] = ((length >> 8) & 255);
        frame[9] = (length & 255);

        indexStartRawData = 10;
    }

    response = buf.Buffer.alloc(indexStartRawData + length);

    var i, reponseIdx = 0;

    //Add the frame bytes to the reponse
    for (i = 0; i < indexStartRawData; i++) {
        response[reponseIdx] = frame[i];
        reponseIdx++;
    }

    //Add the data bytes to the response
    for (i = 0; i < length; i++) {
        response[reponseIdx] = bytesRaw[i];
        reponseIdx++;
    }

    return response;
}

function isClose(message) {
    return (message[0] & 127) == 0x08;
}

function isPong(message) {
    return (message[0] & 127) == 0xA;
}

exports.decodeMessage = decodeMessage;
exports.encodeMessage = encodeMessage;
exports.isClose = isClose;
exports.isPong = isPong;
