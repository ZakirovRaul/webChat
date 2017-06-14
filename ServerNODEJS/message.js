const storage = require('./storage');

class Message{

    constructor(_type, _data) {
        this.type = _type;
        this.data = _data;
        this.creationDate = new Date();
    }
}

var MessageFactory = {
    createMessage: (data) => {
        return new Message("MESSAGE", data);
    },    
    createUsersList: () => {
        return new Message("USERLIST", storage.getNames());
    },
    createHistory: () => {
        return new Message("HISTORY", storage.getMessages(10));
    }
}

module.exports.Message = Message;
module.exports.MessageFactory = MessageFactory;