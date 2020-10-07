import * as indexView from './views/taskView.js';
import * as Index from './models/Index.js';

var hub = new signalR.HubConnectionBuilder()
    .withUrl('/messengerHub')
    .build();

hub.on('RenderAcceptedFriend', async invitedUser => {
    indexView.renderAcceptedFriend(invitedUser);

    const result = await Index.getFriendsAndRelationShips();

    indexView.setRelationShipsDataset(JSON.stringify(result.relationShips));
    indexView.setFriendDataset(JSON.stringify(result.friends));
});

hub.on('RenderNotAcceptedFriend', async invitingUser => {
    indexView.renderNotAcceptedFriend(invitingUser);

    const result = await Index.getFriendsAndRelationShips();

    indexView.setRelationShipsDataset(JSON.stringify(result.relationShips));
    indexView.setFriendDataset(JSON.stringify(result.friends));
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
    await hub.invoke('SendMessage', message);
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

export const sendFriendRequest = async invitedUserName => {
    await hub.invoke('SendFriendRequest', invitedUserName);
};
