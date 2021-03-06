﻿import * as indexView from './views/indexView.js';
import * as Index from './models/Index.js';
import * as index from './index.js';

var hub = new signalR.HubConnectionBuilder()
    .withUrl('/messengerHub')
    .build();

hub.on('RenderAcceptedFriend', async invitedUser => {
    indexView.renderAcceptedFriend(invitedUser);

    const result = await Index.getFriendsAndRelationShips();

    indexView.setRelationShipsDataset(JSON.stringify(result.relationShips));
    indexView.setFriendDataset(JSON.stringify(result.friends));
});


hub.on('EditMessage', async (messageId, content) => {
    indexView.editMessage(messageId, content);
});

hub.on('RemoveMessage', messageId => {
    const message = document.getElementById(messageId);
    if (message) {
        indexView.removeMessage(message);
    }
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
    index.addEventListeningToAllEditMessageButtons();
    index.addEventListeningToAllRemoveMessageButtons();
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

export const tryRemoveMessage = async messageId => {
    await hub.invoke('TryRemoveMessage', messageId);
};

export const tryEditMessage = async (messageId, content) => {
    await hub.invoke('TryEditMessage', messageId, content);
};
