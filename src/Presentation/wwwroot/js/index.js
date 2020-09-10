import { elements, elementStrings } from './views/base.js';
import * as indexView from './views/taskView.js';

elements.menuButton.addEventListener('click', () => {
    indexView.slideOutSideMenu();
});

elements.addFriendButton.addEventListener('click', () => {
    const friendName = prompt('Friend name:');
});