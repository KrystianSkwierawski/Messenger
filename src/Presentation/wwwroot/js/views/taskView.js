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

export const renderFriends = friends => {
    const relationShips = getRelationShips();

    friends.forEach(friend => {
        let markup;
        const currentUserName = getUserName();

        const relationShipsThatAreNotAccepted = relationShips.filter(x =>
            x.invitingUser.userName === friend.userName &&
            x.invitedUser.userName === currentUserName &&
            x.isAccepted === false
        );

        const userIsAccepted = relationShipsThatAreNotAccepted.length !== 0 ? true : false;

        if (userIsAccepted) {
            markup = `
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


        }
        else {
            markup = `
                        <div class="friend__container" id="${friend.id}">
                            <button class="friend__details ml-3 mb-4">
                                <img src="./images/avatar.jpg" class="friend__image rounded-circle" alt="friend avatar"/>
                                <h2 class="friend__name text-white ml-2 text-break">${friend.userName}</h2>
                            </button>
                        </div>
            `;
        }

        elements.friendsContainer.insertAdjacentHTML('beforeend', markup);
    });
};

export const renderRelationShip = (messages, userName) => {
    setMenuFriendName(userName);  
    showInputToSendMessagesContainer();

    if (messages) {
        clearMessagesContainer();
        renderMessages(messages);
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

const renderMessages = messages => {
    messages.forEach(message => {
        const markup = `
                <div class="message mt-3">
                    <img src="./images/avatar.jpg" class="message__profile-picture rounded-circle" alt="friend avatar"/>
                    <div class="message__text-container">
                        <div class="information-about-the-message__container">
                            <h3 class="message__profile-name text-white ml-3 text-primary">${message.applicationUser.userName}</h3>
                            <p class="message__date-sended ml-1 text-secondary">${message.dateSended}</p>
                        </div>

                        <div class="ml-3 text-white">
                            <p>${message.content}</p>
                        </div>
                    </div>
                </div>
        `;

        elements.messagesContainer.insertAdjacentHTML('beforeend', markup);
    });
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

export const removeFriendRequestContainer = friendContainer => {
    friendContainer.removeChild(friendContainer.firstElementChild);
};

export const removeFriendContainer = friendContainer => {
    const friendsContainer = friendContainer.parentNode;
    friendsContainer.removeChild(friendContainer);
};

