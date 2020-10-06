import * as indexView from './views/taskView.js';

var hub = new signalR.HubConnectionBuilder()
    .withUrl('/messengerHub')
    .build();

hub.on('RenderAcceptedFriend', async friend => {
    await indexView.renderAcceptedFriend(friend);
});

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
    await tryLeaveGroup(relationShipId);
   await hub.invoke('JoinGroup', relationShipId);
};

const tryLeaveGroup = async () => {
    await hub.invoke('TryLeaveGroup');
};

export const tryToRenderAFriendToTheSender = async invitingUserId => {
    await hub.invoke('TryToRenderAFriendToTheSender', invitingUserId);
};
