import { elements, elementStrings } from './views/base.js';
import * as indexView from './views/indexView.js';
import * as Index from './models/Index.js';
import * as Emotes from './models/Emotes.js';
import * as messengerHub from './messengerHub.js';

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
        indexView.scrollMessagesContainerToBottom();  
    }
});

elements.inputToSendMessages.addEventListener('keypress', async () => {
    const enterKey = 13;
    const message = indexView.getInputToSendMessagesValue();

    const inputIsNotEmpty = message.trim() ? true : false;

    if (event.keyCode === enterKey && inputIsNotEmpty && !event.shiftKey) {       
       await sendMessage(message);      
       indexView.clearInputToSendMessages();             
    }
});

window.addEventListener('resize', indexView.scrollMessagesContainerToBottom);

async function sendMessage(message) {
    const relationShipId = indexView.getRelationShipId();

    const convertedMessage = Emotes.convertTextToEmotes(message);

    const resultMessage = await Index.addMessage(convertedMessage, relationShipId);
    await messengerHub.sendMessage(resultMessage);    
};

async function acceptFriendRequest(e) {
    const friendContainer = e.target.parentNode.parentNode;
    indexView.removeFriendRequestContainer(friendContainer);

    const result = await Index.acceptFriendRequest(friendContainer.id);
    indexView.enableFriendDetails(friendContainer);

    indexView.setRelationShipsDataset(JSON.stringify(result.relationShips));
    indexView.setFriendDataset(JSON.stringify(result.friends));

    await messengerHub.tryToRenderAFriendToTheSender(friendContainer.id);
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
        const result = await Index.sendFriendRequest(friendName);

        if (result.userExist) {
            toastr.success('The friend request has been sent');
            indexView.setRelationShipsDataset(JSON.stringify(result.relationShips));
            indexView.setFriendDataset(JSON.stringify(result.friends));
            messengerHub.sendFriendRequest(friendName);
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

    await messengerHub.joinGroup(result.relationShipId);
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
