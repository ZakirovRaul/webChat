window.onload = function () {
    var guih = this.guiHelper;
    var webSocket = null;

    $('#btnConnect').click(function() {
        var name = guih.getUserName();
        webSocket = new window.WebSocket('ws://localhost:8090?name=' + name, ["chat"]);
        //webSocket = new window.WebSocket('ws://localhost:8090', ["chat"]);
        webSocket.onopen = hasOpened;
        webSocket.onmessage = hasMessaged;
        webSocket.onclose = hasClosed;
        webSocket.onerror = handleError;
    });

    $('#btnDisconnect').click(function () {
        webSocket.close(1000);
    });

    $('#btnMessage').click(function () {
        if (webSocket != null && webSocket.readyState == 1) {
		    var message = guih.getUserMsg();
			if(message != ''){
				webSocket.send(message);
				guih.messageLineBuilder.myMessage(message);
			    guih.clearMessageBox();
			}
		}
    });
	
    function hasOpened(e) {
        if (e.target.readyState == true) {
            guih.buttonConnectDisable();
        }
        else {
            guih.buttonConnectEnable();
        }
    };
	
	function hasClosed(e) {
        guih.buttonConnectEnable();
    };
	
    function handleError(e) { 
	};

    function hasMessaged(e) {
		var response = JSON.parse(e.data);
		switch(response.type){
			case "MESSAGE":
			    guih.messageLineBuilder.peopleMessage(response.data);
			break;
			case "USERLIST":
				var users = response.data;
				var usesrInfo = '';
				for(var i = 0; i < users.length; i++){
					usesrInfo += '<p>' + users[i] + '</p>';
				}
				$('.users_column').html(usesrInfo);
			break;
			case "HISTORY":
			    var messages = response.data;
				for(var i = 0; i < messages.length; i++){
				    guih.messageLineBuilder.peopleMessage(messages[i].key + ":" + messages[i].value);
				}
			break;
		}

    };
	


    //ns.output = function(msg) {
    //    $('<p>' + msg + '</p>').appendTo($('.messages_column'));
    //};

    //ns.outputMe = function (msg) {
    //    var html = $('.messages_column').html();
    //    html = html + '<p class="me">' + msg + '</p>';
    //    $('.messages_column').html(html);
    //    //$('<p class="me">' + msg + '</p>').appendTo($('.messages_column'));
    //};
	

};