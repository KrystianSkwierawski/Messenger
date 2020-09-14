import { elements, elementStrings } from './base.js';

export const slideOutSideMenu = () => {
    elements.menuContainer.classList.toggle('d-none');
    elements.menuButton.classList.toggle('fa-bars');
    elements.menuButton.classList.toggle('fa-times');
    elements.menuFriendName.classList.toggle('d-none');
};