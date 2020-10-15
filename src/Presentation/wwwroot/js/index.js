import { elements, elementStrings } from './views/base.js';
import * as indexView from './views/indexView.js';
import * as Index from './models/Index.js';
import * as EmojisConverter from './models/EmojisConverter.js';
import * as Emojis from './models/Emojis.js';
import * as messengerHub from './messengerHub.js';

addEmojisToEmojisContainer(Emojis.smileys);

async function addEmojisToEmojisContainer(emojis) {   
    await indexView.renderEmojisToEmojisContainer(emojis);
    const emojiButtons = document.querySelectorAll(`.${elementStrings.emojiButton}`);

    Array.from(emojiButtons).forEach(emojiButton => {
        emojiButton.addEventListener('click', e => {
            indexView.addEmojiToInputToSendMessagesInput(e.target.innerText);
        });
    });
};

Array.from(elements.emojiTypeButton).forEach(emojiTypeButton => {
    emojiTypeButton.addEventListener('click', e => {
        indexView.changeActiveEmojiTypeButton(e.target);

        const emojiType = e.target.id;

        switch (emojiType) {
            case 'smileys':
                addEmojisToEmojisContainer(Emojis.smileys);
                break;

            case 'gesturesAndBodyParts':
                addEmojisToEmojisContainer(Emojis.gesturesAndBodyParts);
                break;

            case 'peopleAndFantasy':
                addEmojisToEmojisContainer(Emojis.peopleAndFantasy);
                break;

            case 'activityAndSports':
                addEmojisToEmojisContainer(Emojis.activityAndSports);
                break;

            case 'foodAndDrink':
                addEmojisToEmojisContainer(Emojis.foodAndDrink);
                break;

            case 'animalsAndNature':
                addEmojisToEmojisContainer(Emojis.animalsAndNature);
                break;
        }
    });
});


function searchFriendsByUserName() {
    const friends = indexView.getFriends();
    const userName = indexView.getSearchingUserName().toLowerCase();

    const filteredFriends = friends.filter(x => x.userName.toLowerCase().includes(userName));

    if (filteredFriends) {
        indexView.clearFriendsContainer();
        indexView.renderFriends(filteredFriends);
    }
}

elements.searchInput.addEventListener('change', searchFriendsByUserName);

elements.menuButton.addEventListener('click', indexView.slideOutSideMenu);

const doesNotHaveThisFriend = friendName => {
    const friends = indexView.getFriends();

    const result = friends.filter(x => x.userName === friendName);

    return result.length === 0 ? true : false;
};

const doesNotInvitingHimSelf = friendName => {
    const currentUserId = indexView.getUserName();

    return friendName === currentUserId ? false : true;
};

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

elements.addFriendButton.addEventListener('click', trySendFriendRequest);

async function sendMessage(message) {
    const relationShipId = indexView.getRelationShipId();

    const convertedMessage = EmojisConverter.convertTextToEmojis(message);

    const resultMessage = await Index.addMessage(convertedMessage, relationShipId);
    await messengerHub.sendMessage(resultMessage);
};

elements.inputToSendMessages.addEventListener('keypress', async () => {
    const enterKey = 13;
    const message = indexView.getInputToSendMessagesValue();

    const inputIsNotEmpty = message.trim() ? true : false;

    if (event.keyCode === enterKey && inputIsNotEmpty && !event.shiftKey) {
        await sendMessage(message);
        indexView.clearInputToSendMessages();
    }
});


elements.sendMessageButton.addEventListener('click', async () => {
    const message = indexView.getInputToSendMessagesValue();
    const inputIsNotEmpty = message.trim() ? true : false;

    if (inputIsNotEmpty) {
        await sendMessage(message);
    }
    
    indexView.clearInputToSendMessages();
});

elements.friendsContainer.addEventListener('click', async e => {

    if (e.target.matches(`.${elementStrings.friendAcceptRequest}`)) {
        await acceptFriendRequest(e);        
    }

    if (e.target.matches(`.${elementStrings.friendRejectRequest}`)) {
        await rejectFriendRequest(e);
    }

    if (e.target.matches(`.${elementStrings.friendDetails}, .${elementStrings.friendDetails} *`)) {
        await openRelationShip(e);
        indexView.slideOutSideMenu();
        indexView.scrollMessagesContainerToBottom();  

        addEventListeningToAllRemoveMessageButtons();
        addEventListeningToAllEditMessageButtons();
    }
});

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

const openRelationShip = async e => {
    const friendDetails = indexView.getFriendDetails(e);

    const result = await Index.getMessagesOfCurrentRelationShipAndRelationShipId(friendDetails.id);

    await messengerHub.joinGroup(result.relationShipId);

    indexView.setRelationShipIdDataset(result.relationShipId);
    await indexView.renderRelationShip(result.messages, friendDetails.userName);
};


export const addEventListeningToAllRemoveMessageButtons = () => {
    const removeMessageButtons = document.querySelectorAll(`.${elementStrings.removeMessageButton}`);

    Array.from(removeMessageButtons).forEach(removeMessageButton => {
        removeMessageButton.addEventListener('click', async e => {

            const message = e.target.closest(`.${elementStrings.message}`)

            if (message.id) {
                await Index.removeMessage(message.id);

                await messengerHub.tryRemoveMessage(message.id);
            }
        });
    });
};

export const addEventListeningToAllEditMessageButtons = () => {
    const editMessageButtons = document.querySelectorAll(`.${elementStrings.editMessageButton}`);

    Array.from(editMessageButtons).forEach(editMessageButton => {
        editMessageButton.addEventListener('click', e => {

            const message = e.target.closest(`.${elementStrings.message}`)

            if (message.id) {

                const anyEditMessageContainer = document.querySelector(`.${elementStrings.editMessageContainer}`);
                if (anyEditMessageContainer) {
                    indexView.messageContentContainerChangeToText(anyEditMessageContainer);
                }

                indexView.messageContentContainerChangeToInput(message);

                addEventListeningToSaveEditMessage();
                addEventListeningToCancelEditMessage();
            }
        });
    });
};

export const addEventListeningToSaveEditMessage = () => {
    document.querySelector(`.${elementStrings.saveEditMessageButton}`).addEventListener('click', e => {
        console.log('save');
    });
};

export const addEventListeningToCancelEditMessage = () => {
    document.querySelector(`.${elementStrings.cancelEditMessageButton}`).addEventListener('click', e => {
        const message = e.target.closest(`.${elementStrings.message}`);
        
        const editMessageContainer = e.target.closest(`.${elementStrings.editMessageContainer}`);
        indexView.messageContentContainerChangeToText(editMessageContainer);      
    });
};

window.addEventListener('resize', indexView.scrollMessagesContainerToBottom);

elements.displayEmojisButton.addEventListener('click', () => {
    indexView.displayEmojisContainer();
});
