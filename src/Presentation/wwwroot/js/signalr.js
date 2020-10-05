import * as indexView from './views/taskView.js';

var hub = new signalR.HubConnectionBuilder()
    .withUrl('/chatHub')
    .build();

hub.on('ReceiveMessage', message => {
    indexView.renderMessage(message);
});

hub.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});

export const sendMessage = message => {
    hub.invoke('Send', message);
};

export const joinGroup = relationShipId => {
    hub.invoke('JoinGroup', relationShipId);
};


export const leaveGroup = relationShipId => {
    hub.invoke('LeaveGroup', relationShipId);
};