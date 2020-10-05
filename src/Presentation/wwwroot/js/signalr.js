import * as indexView from './views/taskView.js';

var hub = new signalR.HubConnectionBuilder()
    .withUrl('/chatHub')
    .build();

hub.on('ReceiveMessage', async message => {
    await indexView.renderMessage(message);
    indexView.scrollMessagesContainerToBottom();
});

hub.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});

export const sendMessage = async message => {
   await hub.invoke('Send', message);
};

export const joinGroup = async relationShipId => {
   await hub.invoke('JoinGroup', relationShipId);
};


export const leaveGroup = async relationShipId => {
    await hub.invoke('LeaveGroup', relationShipId);
};