const messages = [];
const names = [];

function addMessage(user, message) {
    messages.push({ key: user, value: message });
}


function getMessages(count) {
    if (messages.length <= count) {
        return messages;
    }
    var returnVal = [];
    for (var i = messages.length; i < (messages.length - count); i--) {
        returnVal.push(messages[i]);
    }
    return returnVal;
}

function addName(name) {
    names.push(name);
}

function getNames() {
    return names;
}

function removeName(name) {
    if (name != undefined) {
        var index = names.indexOf(name);
        if (index >= 0) {
            names.splice(index, 1);
        }
    }
}

exports.addMessage = addMessage;
exports.getMessages = getMessages;
exports.addName = addName;
exports.getNames = getNames;
exports.removeName = removeName;
