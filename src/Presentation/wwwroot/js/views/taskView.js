import { elements, elementStrings } from './base.js';

export const slideOutSideMenu = () => {
    elements.sideMenuContainer.classList.toggle('side-menu__container_active');
    elements.sideMenuButton.classList.toggle('fa-bars');
    elements.sideMenuButton.classList.toggle('fa-arrow-left');
};