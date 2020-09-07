import { elements, elementStrings } from './views/base.js';
import * as indexView from './views/taskView.js';

elements.sideMenuButton.addEventListener('click', () => {
    indexView.slideOutSideMenu();
});