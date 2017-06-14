(function () {
    this.guiHelper = this.guiHelper || {};
    var ns = this.guiHelper;

    ns.getUserName = function () {
        return $('#txtUserName').val();
    };

    ns.buttonConnectEnable = function () {
        $('#btnConnect').removeAttr('disabled');
    };

    ns.buttonConnectDisable = function () {
        $('#btnConnect').attr('disabled', 'disabled');
    };

    ns.getUserMsg = function () {
        return $('#txtMessage').val();
    };

    ns.setUserMsg = function (messageText) {
        $('#txtMessage').val(messageText);
    };

    ns.clearMessageBox = function () {
        $('#txtMessage').val('');
    };

    ns.messageLineBuilder = (function () {
        function lineBuilder() {
        }

        lineBuilder.peopleMessage = function (text) {
            $('<p class="people">' + text + '</p>').appendTo($('.messages_column'));
        };

        lineBuilder.myMessage = function (text) {
            $('<p class="me">' + text + '</p>').appendTo($('.messages_column'));
        };

        //lineBuilder.infoMessage = function (text) {
        //    $('<p class="info">' + text + '</p>').appendTo($('.messages_column'));
        //};

        return lineBuilder;
    })();

})();
