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

export const setFriendDataset = friends => {
    elements.addFriendButton.dataset.friends = friends;
};
