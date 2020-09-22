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
                         <div id="${friend.id}">
                            <div class="friend__request ml-3 mb-1">
                                <i class="fas fa-check text-success friend__accept-request"></i>
                                <i class="fas fa-times ml-2 text-danger friend__reject-request"></i>
                            </div>
                            <div class="friend__details ml-3 mb-4">
                                <img src="./images/avatar.png" class="friend__image rounded-circle" />
                                <h4 class="friend__name text-white ml-2 text-break">${friend.userName}</h4>
                            </div>
                        </div>
            `;

            
        }
        else {
            markup = `
                        <div id="${friend.id}">
                            <div class="friend__details ml-3 mb-4">
                                <img src="./images/avatar.png" class="friend__image rounded-circle" />
                                <h4 class="friend__name text-white ml-2 text-break">${friend.userName}</h4>
                            </div>
                        </div>
            `;
        }
     
        elements.friendsContainer.insertAdjacentHTML('beforeend', markup);
    });
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
