import { elements, elementStrings } from './base.js';

export const slideOutSideMenu = () => {
    elements.menuContainer.classList.toggle('d-none');
    elements.menuButton.classList.toggle('fa-bars');
    elements.menuButton.classList.toggle('fa-times');
    elements.menuFriendName.classList.toggle('d-none');
};

export const getUserName = () => {
    return elements.addFriendButton.dataset.username;
};

export const getFriends = () => {
    const friends = elements.addFriendButton.dataset.friends;

    return JSON.parse(friends);
};

export const getSearchingUserName = () => {
    return elements.searchInput.value;
};

const getRelationShips = () => {
    const relationShips = elements.friendsContainer.dataset.relationships;

    return JSON.parse(relationShips);
};

const clearMessagesContainer = () => {
    elements.messagesContainer.innerHTML = "";
};

export const clearFriendsContainer = () => {
    elements.friendsContainer.innerHTML = "";
};

export const renderAcceptedFriend = friend => {
    const markup = `
                        <div class="${elementStrings.friendContainer}" id="${friend.id}">
                            <button class="${elementStrings.friendDetails} ml-3 mb-4">
                                <img src="${friend.imageUrl}" class="${elementStrings.friendImage} rounded-circle" alt="friend avatar"/>
                                <h2 class="${elementStrings.friendName} ml-2 text-break">${friend.userName}</h2>
                            </button>
                        </div>
    `;

    elements.friendsContainer.insertAdjacentHTML('beforeend', markup);
};

export const renderNotAcceptedFriend = friend => {
    const markup = `
                        <div class="${elementStrings.friendContainer}" id="${friend.id}">
                            <div class="${elementStrings.friendRequest} ml-3 mb-1">
                                <button class="fas fa-check text-success p-0 ${elementStrings.friendAcceptRequest} btn btn-link"></button>
                                <button class="fas fa-times ml-2 text-danger p-0 ${elementStrings.friendRejectRequest} btn btn-link"></button>
                            </div>
                            <button disabled class="${elementStrings.friendDetails} ml-3 mb-4">
                                <img src="${friend.imageUrl}" class="${elementStrings.friendImage} rounded-circle" alt="friend avatar"/>
                                <h2 class="${elementStrings.friendName} ml-2 text-break">${friend.userName}</h2>
                            </button>
                        </div>
    `;

    elements.friendsContainer.insertAdjacentHTML('beforeend', markup);
};


export const renderFriends = friends => {
    const relationShips = getRelationShips();

    friends.forEach(friend => {
        const currentUserName = getUserName();

        const currentNotAcceptedRelationShip = relationShips.filter(x =>
            ((x.invitingUser.userName === friend.userName && x.invitedUser.userName === currentUserName) ||
            (x.invitingUser.userName === currentUserName && x.invitedUser.userName === friend.userName)) &&
            x.isAccepted === false
        );

        const currentRelationShipIsAccepted = currentNotAcceptedRelationShip.length === 0 ? true : false;

        if (currentRelationShipIsAccepted) {
            renderAcceptedFriend(friend);
        }
        else {
            const relationShipsWhereCurrentUserIsInvited = currentNotAcceptedRelationShip.filter(x => x.invitedUser.userName == currentUserName);
            const currentUserIsInvited = relationShipsWhereCurrentUserIsInvited.length === 0 ? false : true;

            if (currentUserIsInvited) {
                renderNotAcceptedFriend(friend);
            }        
        }
    });
};

export const renderRelationShip = async (messages, userName) => {
    setMenuFriendName(userName);
    showInputToSendMessagesContainer();

    if (messages) {
        clearMessagesContainer();
        await messages.forEach(message => renderMessage(message));
    }
};

export const enableFriendDetails = friendContainer => {
    const friendDetails = friendContainer.querySelector(`.${elementStrings.friendDetails}`);
    friendDetails.disabled = false;
};

export const getFriendDetails = e => {
    const friendContainer = e.target.closest(`.${elementStrings.friendContainer}`);

    return {
        id: friendContainer.id,
        userName: friendContainer.querySelector('.friend__name').textContent
    };
};

const showInputToSendMessagesContainer = () => {
    elements.interactionMenuContainer.classList.add('active');
};

export const renderMessage = message => {
    let editOrRemoveMarkup = "";
    const userName = getUserName();

    if (userName === message.applicationUser.userName) {
        editOrRemoveMarkup = `
                  <div class="dropright ml-2">
                    <button class="${elementStrings.messageSettingsButton}" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <i class="fas fa-ellipsis-h"></i>
                    </button>

                    <div class="${elementStrings.messageDropdownMenu} dropdown-menu" aria-labelledby="dropdownMenuButton">
                        <button class="dropdown-item text-white ${elementStrings.editMessageButton}">Edit</button>
                        <button class="dropdown-item text-white ${elementStrings.removeMessageButton}">Remove</button>
                    </div>
                   </div>
        `;
    }
    const markup = `
                <div class="${elementStrings.message} mt-3" id="${message.id}">
                    <img src="${message.applicationUser.imageUrl}" class="${elementStrings.messageProfilePicture} rounded-circle" alt="friend avatar"/>
                    <div class="${elementStrings.messageTextContainer}">
                        <div class="${elementStrings.informationAboutTheMessageContainer}">
                            <h3 class="${elementStrings.messageProfileName} text-break ml-3 mb-1 ">${message.applicationUser.userName}</h3>
                            <p class="${elementStrings.messageDateSended} ml-1">${message.dateSended}</p>
                            ${editOrRemoveMarkup}
                        </div>

                        <div class="ml-3 ${elementStrings.messageContentContainer} text-break">
                            ${message.content}
                        </div>
                        
                    </div>
                </div>
        `;

    elements.messagesContainer.insertAdjacentHTML('beforeend', markup);
};

const setMenuFriendName = userName => {
    elements.menuFriendName.textContent = userName;
};

export const setFriendDataset = friends => {
    elements.addFriendButton.dataset.friends = friends;
};

export const setRelationShipsDataset = relationShips => {
    elements.friendsContainer.dataset.relationships = relationShips;
}

export const displayEmojisContainer = () => {
    elements.emojisContainer.classList.toggle('active');
};

export const removeFriendRequestContainer = friendContainer => {
    friendContainer.removeChild(friendContainer.firstElementChild);
};

export const removeFriendContainer = friendContainer => {
    const friendsContainer = friendContainer.parentNode;
    friendsContainer.removeChild(friendContainer);
};

export const setRelationShipIdDataset = id => {
    elements.mainContainer.dataset.relatioshipid = id;
};

export const getRelationShipId = () => {
    return elements.mainContainer.dataset.relatioshipid;
};

export const getInputToSendMessagesValue = () => {
    return elements.inputToSendMessages.value;
};

export const clearInputToSendMessages = () => {
    elements.inputToSendMessages.value = "";
};

export const addEmojiToInputToSendMessagesInput = emoji => {
    elements.inputToSendMessages.value += emoji;
};

export const scrollMessagesContainerToBottom = () => {
    const scrollHeight = elements.messagesContainer.scrollHeight;
    const clientHeight = elements.messagesContainer.clientHeight;

    elements.messagesContainer.scrollTop = scrollHeight - clientHeight;
};

export const getCurrentUserId = () => {
    return elements.mainContainer.dataset.userid;
};

export const renderEmojisToEmojisContainer = async emojis => {
    elements.emojis.innerHTML = "";

    await emojis.forEach(emoji => {
        const markup = `<button class="${elementStrings.emojiButton}">${emoji}</button>`;

        elements.emojis.insertAdjacentHTML('beforeend', markup);
    });
};

export const renderOriginalEmojisToEmojisContainer = async emojis => {
    elements.emojis.innerHTML = "";

    await emojis.forEach(emoji => {
        const markup = `<button class="${elementStrings.originalEmojiButton}"><img alt="emoji" class="${elementStrings.originalEmoji}" src="./images/original-emojis/${emoji}" width="60" height="60"/></button>`;

        elements.emojis.insertAdjacentHTML('beforeend', markup);
    });
};

export const changeActiveEmojiTypeButton = currentTypeButton => {
    const previousEmojiTypeButton = document.querySelector(`.${elementStrings.emojiTypeButtonActive}`);
    previousEmojiTypeButton.classList.toggle(elementStrings.emojiTypeButtonActive);

    currentTypeButton.classList.toggle(elementStrings.emojiTypeButtonActive);
};

export const removeMessage = message => {   
    message.parentNode.removeChild(message);
};

export const getCurrentRelationShipFriendName = () => {
    return elements.menuFriendName.innerHTML;
};

export const messageContentContainerChangeToInput = message => {
    const messageContentContainer = message.querySelector(`.${elementStrings.messageContentContainer}`);
    const content = messageContentContainer.innerText;

    const markup =
       `
        <div class="edit-message__container">
            <textarea class="${elementStrings.editMessageInput} p-2" >${content}</textarea>
            <button class="${elementStrings.saveEditMessageButton}">Save</button>
            <button class="${elementStrings.cancelEditMessageButton} pl-2">Cancel</button
        </div>
        `;

    messageContentContainer.innerHTML = markup;
};

export const messageContentContainerChangeToText = editMessageContainer => {
    const content = editMessageContainer.querySelector(`.${elementStrings.editMessageInput}`).innerHTML;

    const messageContentContainer = editMessageContainer.parentNode;

    messageContentContainer.innerHTML = content;
};

export const editMessage = (messageId, content) => {
    const message = document.getElementById(messageId);
    const messageContentContainer = message.querySelector(`.${elementStrings.messageContentContainer}`);

    messageContentContainer.innerHTML = content;
};

export const showOrHideVoiceMessageContainer = () => {
    elements.voiceMessageContainer.classList.toggle('active');
};





