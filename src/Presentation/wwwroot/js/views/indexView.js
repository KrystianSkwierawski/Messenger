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
                        <div class="friend__container" id="${friend.id}">
                            <button class="friend__details ml-3 mb-4">
                                <img src="./images/avatar.jpg" class="friend__image rounded-circle" alt="friend avatar"/>
                                <h2 class="friend__name text-white ml-2 text-break">${friend.userName}</h2>
                            </button>
                        </div>
    `;

    elements.friendsContainer.insertAdjacentHTML('beforeend', markup);
};

export const renderNotAcceptedFriend = friend => {
    const markup = `
                        <div class="friend__container" id="${friend.id}">
                            <div class="friend__request ml-3 mb-1">
                                <button class="fas fa-check text-success p-0 friend__accept-request btn btn-link"></button>
                                <button class="fas fa-times ml-2 text-danger p-0 friend__reject-request btn btn-link"></button>
                            </div>
                            <button disabled class="friend__details ml-3 mb-4">
                                <img src="./images/avatar.jpg" class="friend__image rounded-circle" alt="friend avatar"/>
                                <h2 class="friend__name text-white ml-2 text-break">${friend.userName}</h2>
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
    elements.inputToSendMessagesContainer.classList.add('active');
};

export const renderMessage = message => {
    const markup = `
                <div class="message mt-3" id="${message.id}">
                    <img src="./images/avatar.jpg" class="message__profile-picture rounded-circle" alt="friend avatar"/>
                    <div class="message__text-container">
                        <div class="information-about-the-message__container">
                            <h3 class="message__profile-name text-white ml-3 mb-0 text-primary">${message.applicationUser.userName}</h3>
                            <p class="message__date-sended ml-1 text-secondary">${message.dateSended}</p>

                            <div class="dropright ml-2">
                              <button class="message-settings__button" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <i class="fas fa-ellipsis-h"></i>
                              </button>
                              <div class="message-dropdown-menu dropdown-menu" aria-labelledby="dropdownMenuButton">
                                <button class="dropdown-item text-white edit-message__button">Edit</button>
                                <button class="dropdown-item text-white remove-message__button">Remove</button>
                              </div>
                            </div>

                        </div>

                        <div class="ml-3 text-white message-content__container text-break">
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
        const markup = `<button class="emoji__button">${emoji}</button>`;

        elements.emojis.insertAdjacentHTML('beforeend', markup);
    });
};

export const changeActiveEmojiTypeButton = currentTypeButton => {
    const previousEmojiTypeButton = document.querySelector(`.${elementStrings.emojiTypeButtonActive}`);
    previousEmojiTypeButton.classList.toggle(elementStrings.emojiTypeButtonActive);

    currentTypeButton.classList.toggle(elementStrings.emojiTypeButtonActive);
};

export const getMessageId = e => {
    return e.target.closest(`.${elementStrings.message}`).id;
};

export const removeMessage = message => {   
    message.parentNode.removeChild(message);
};

export const getCurrentRelationShipFriendName = () => {
    return elements.menuFriendName.innerHTML;
};




