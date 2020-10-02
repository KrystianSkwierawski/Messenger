﻿import { elements, elementStrings } from './views/base.js';
import * as indexView from './views/taskView.js';
import * as Index from './models/Index.js';

elements.searchInput.addEventListener('change', searchFriendsByUserName);

elements.menuButton.addEventListener('click', indexView.slideOutSideMenu);

elements.addFriendButton.addEventListener('click', trySendFriendRequest);

elements.friendsContainer.addEventListener('click', async e => {

    if (e.target.matches(`.${elementStrings.friendAcceptRequest}`)) {
        await acceptFriendRequest(e);
    }

    if (e.target.matches(`.${elementStrings.friendRejectRequest}`)) {
        await rejectFriendRequest(e);
    }

    if (e.target.matches(`.${elementStrings.friendDetails}, .${elementStrings.friendDetails} *`)) {
        await openRelationShip(e);
    }
});

elements.inputToSendMessages.addEventListener('keypress', async () => {
    const enterKey = 13;
    const message = indexView.getInputToSendMessagesValue();

    const inputIsNotEmpty = message.trim() ? true : false;

    if (event.keyCode === enterKey && inputIsNotEmpty && !event.shiftKey) {
        await sendMessage(message);
    }
});

async function sendMessage(message) {
    const relationShipId = indexView.getRelationShipId();

    await Index.addMessage(message, relationShipId);
};

async function acceptFriendRequest(e) {
    const friendContainer = e.target.parentNode.parentNode;
    indexView.removeFriendRequestContainer(friendContainer);

    const relationShips = await Index.acceptFriendRequest(friendContainer.id);
    indexView.enableFriendDetails(friendContainer);

    indexView.setRelationShipsDataset(JSON.stringify(relationShips));
}

async function rejectFriendRequest(e) {
    const friendContainer = e.target.parentNode.parentNode;
    indexView.removeFriendContainer(friendContainer);

    const result = await Index.rejectFriendRequest(friendContainer.id);

    indexView.setFriendDataset(JSON.stringify(result.friends));
    indexView.setRelationShipsDataset(JSON.stringify(result.relationShips));
}

async function trySendFriendRequest() {
    const friendName = prompt('Friend name:');

    if (doesNotInvitingHimSelf(friendName) && doesNotHaveThisFriend(friendName)) {
        const request = await Index.sendFriendRequest(friendName);

        if (request.userExist) {

            indexView.setFriendDataset(JSON.stringify(request.friends));
            toastr.success('The friend request has been sent');
        }
        else {
            toastr.info('There is no such user');
        }
    }
    else {
        console.log('error');
    }
}

function searchFriendsByUserName() {
    const friends = indexView.getFriends();
    const userName = indexView.getSearchingUserName().toLowerCase();

    const filteredFriends = friends.filter(x => x.userName.toLowerCase().includes(userName));

    if (filteredFriends) {
        indexView.clearFriendsContainer();
        indexView.renderFriends(filteredFriends);
    }  
}

const openRelationShip = async e => {  
    const friendDetails = indexView.getFriendDetails(e);

    const result = await Index.getMessagesOfCurrentRelationShipAndRelationShipId(friendDetails.id);

    indexView.setRelationShipIdDataset(result.relationShipId);
    indexView.renderRelationShip(result.messages, friendDetails.userName);
};

const doesNotHaveThisFriend = friendName => {
    const friends = indexView.getFriends();

    const result = friends.filter(x => x.userName === friendName);

    return result.length === 0 ? true : false;
};

const doesNotInvitingHimSelf = friendName => {
    const currentUserId = indexView.getUserName();

    return friendName === currentUserId ? false : true;
};
