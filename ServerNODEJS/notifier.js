class ChannelsNotifier {
    constructor() {
        this.channels = [];
    }

    addChannel(channel) {
        this.channels.push(channel);
    }

    removeChannel(channel) {
        if (this.channels.includes(channel)) {
            this.channels.splice(this.channels.indexOf(channel), 1);
        }
    }

    notify(channel, message) {
        for (var i=0; i < this.channels.length; i++) {
            if (this.channels[i] != channel) {
                this.channels[i].notify(message);
            }
        }
    }
    
    notifyAll(message) {
        for (var i =0; i < this.channels.length; i++) {
            this.channels[i].notify(message);
        }
    }
}

module.exports.ChannelsNotifier = ChannelsNotifier;

